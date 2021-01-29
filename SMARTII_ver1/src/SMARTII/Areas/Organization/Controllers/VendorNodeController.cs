using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac.Features.Indexed;
using SMARTII.Areas.Organization.Models;
using SMARTII.Areas.Organization.Models.VendorNode;
using SMARTII.Assist.Authentication;
using SMARTII.Assist.Logger;
using SMARTII.Assist.Web;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;

namespace SMARTII.Areas.Organization.Controllers
{
    [Authentication]
    public class VendorNodeController : BaseApiController
    {
        private readonly ICommonAggregate _CommonAggregate;
        private readonly IOrganizationNodeProcessProvider _NodeFactory;
        private readonly IOrganizationAggregate _OrganizationAggregate;

        public VendorNodeController(ICommonAggregate CommonAggregate,
                                           IOrganizationAggregate OrganizationAggregate,
                                           IIndex<OrganizationType, IOrganizationNodeProcessProvider> NodeFactories)
        {
            _CommonAggregate = CommonAggregate;
            _OrganizationAggregate = OrganizationAggregate;
            _NodeFactory = NodeFactories[OrganizationType.Vendor];
        }

        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(VendorNode_lang.VENDOR_NODE_GET_NESTED))]
        [ModelValidator(false)]
        public async Task<IHttpResult> GetAllNested()
        {
            try
            {
                var nodes = await _NodeFactory.GetAll();

                var nested = (VendorNode)nodes.AsNestedNSM();

                var result = new JsonResult<VendorNodeViewModel>(
                    new VendorNodeViewModel(nested), true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                   ex.PrefixDevMessage(VendorNode_lang.VENDOR_NODE_GET_NESTED_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(VendorNode_lang.VENDOR_NODE_GET_NESTED_FAIL), false)
                    .Async();
            }
        }

        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(VendorNode_lang.VENDOR_NODE_CREATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Create(VendorNodeViewModel model)
        {
            try
            {
                var nestedNode = model.ToDomain();
                await _NodeFactory.Create(nestedNode);

                var result = new JsonResult(VendorNode_lang.VENDOR_NODE_CREATE_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                   ex.PrefixDevMessage(VendorNode_lang.VENDOR_NODE_CREATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(VendorNode_lang.VENDOR_NODE_CREATE_FAIL), false)
                    .Async();
            }
        }

        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(VendorNode_lang.VENDOR_NODE_GET))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Get([Required] int? nodeID)
        {
            try
            {
                var node = await _NodeFactory.GetComplete(nodeID.Value);

                var model = new VendorNodeDetailViewModel((VendorNode)node);

                var result = new JsonResult<VendorNodeDetailViewModel>(model, VendorNode_lang.VENDOR_NODE_GET_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                                  ex.PrefixDevMessage(VendorNode_lang.VENDOR_NODE_GET_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(VendorNode_lang.VENDOR_NODE_GET_FAIL), false)
                    .Async();
            }
        }

        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Update)]
        [Logger(nameof(VendorNode_lang.VENDOR_NODE_UPDATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Update(VendorNodeDetailViewModel model)
        {
            try
            {
                var data = model.ToDomain();

                //如果要停用 必須檢查此廠商是否還存在歷程(案件未結案)
                if (data.IsEnabled == false)
                {
                    if (await _NodeFactory.CheckDisableVendor(data.NodeID))
                        throw new Exception(VendorNode_lang.VENDOR_NODE_UPDATE_CHECK_FAIL);
                }

                var node = await _NodeFactory.Update(data);

                var newModel = new VendorNodeDetailViewModel((VendorNode)node);

                var result = new JsonResult<VendorNodeDetailViewModel>(newModel, VendorNode_lang.VENDOR_NODE_UPDATE_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                var message = "";

                if (ex.Message == VendorNode_lang.VENDOR_NODE_UPDATE_CHECK_FAIL)
                    message = VendorNode_lang.VENDOR_NODE_UPDATE_CHECK_FAIL;
                else
                    message = VendorNode_lang.VENDOR_NODE_UPDATE_FAIL;

                _CommonAggregate.Logger.Error(
                                  ex.PrefixDevMessage(message));

                return await new JsonResult(
                        ex.PrefixMessage(message), false)
                        .Async();
            }
        }

        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Update)]
        [Logger(nameof(VendorNode_lang.VEDNOR_UPDATE_TREE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> UpdateTree(VendorNodeViewModel model)
        {
            try
            {
                var data = model.ToDomain();

                await _NodeFactory.UpdateTree(data);

                var result = new JsonResult(VendorNode_lang.VEDNOR_UPDATE_TREE_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                                  ex.PrefixDevMessage(VendorNode_lang.VEDNOR_UPDATE_TREE_FAIL));

                return await new JsonResult(
                        ex.PrefixMessage(VendorNode_lang.VEDNOR_UPDATE_TREE_FAIL), false)
                        .Async();
            }
        }

        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Add)]
        [Logger(nameof(VendorNode_lang.VENDOR_ADD_JOB))]
        [ModelValidator(false)]
        public async Task<IHttpResult> AddJob(AddJobViewModel model)
        {
            try
            {
                await _NodeFactory.AddJobs(model.NodeID.Value, model.JobIDs.ToArray());

                var result = new JsonResult(VendorNode_lang.VENDOR_ADD_JOB_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                                  ex.PrefixDevMessage(VendorNode_lang.VENDOR_ADD_JOB_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(VendorNode_lang.VENDOR_ADD_JOB_FAIL), false)
                    .Async();
            }
        }

        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Add)]
        [Logger(nameof(VendorNode_lang.VENDOR_ADD_USER))]
        [ModelValidator(false)]
        public async Task<IHttpResult> AddUser(AddUserViewModel model)
        {
            try
            {
                await _NodeFactory.AddUsers(model.NodeJobID.Value, model.UserIDs.ToArray());

                var result = new JsonResult(VendorNode_lang.VENDOR_ADD_USER_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                                  ex.PrefixDevMessage(VendorNode_lang.VENDOR_ADD_USER_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(VendorNode_lang.VENDOR_ADD_USER_FAIL), false)
                    .Async();
            }
        }

        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Delete)]
        [Logger(nameof(VendorNode_lang.VENDOR_DELETE_USER))]
        [ModelValidator(false)]
        public async Task<IHttpResult> DeleteUser([Required] int? nodeJobID, [Required] string userID)
        {
            try
            {
                await _NodeFactory.DeleteUser(nodeJobID.Value, userID);

                var result = new JsonResult(VendorNode_lang.VENDOR_DELETE_USER_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                                  ex.PrefixDevMessage(VendorNode_lang.VENDOR_DELETE_USER_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(VendorNode_lang.VENDOR_DELETE_USER_FAIL), false)
                    .Async();
            }
        }

        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Delete)]
        [Logger(nameof(VendorNode_lang.VENDOR_DELETE_JOB))]
        [ModelValidator(false)]
        public async Task<IHttpResult> DeleteJob([Required] int? nodeJobID)
        {
            try
            {
                await _NodeFactory.DeleteJob(nodeJobID.Value);

                var result = new JsonResult(VendorNode_lang.VENDOR_DELETE_JOB_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                                  ex.PrefixDevMessage(VendorNode_lang.VENDOR_DELETE_JOB_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(VendorNode_lang.VENDOR_DELETE_JOB_FAIL), false)
                    .Async();
            }
        }

        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Delete)]
        [Logger(nameof(VendorNode_lang.VEDNOR_NODE_DISABLE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Disable([Required] int? nodeID)
        {
            try
            {
                await _NodeFactory.Disable(nodeID.Value);

                var result = new JsonResult(VendorNode_lang.VEDNOR_NODE_DISABLE_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                   ex.PrefixDevMessage(VendorNode_lang.VEDNOR_NODE_DISABLE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(VendorNode_lang.VEDNOR_NODE_DISABLE_FAIL), false)
                    .Async();
            }
        }
    }
}
