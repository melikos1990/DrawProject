using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac.Features.Indexed;
using Newtonsoft.Json;
using SMARTII.Areas.Organization.Models;
using SMARTII.Areas.Organization.Models.HeaderQuarterNode;
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
    public class HeaderQuarterNodeController : BaseApiController
    {
        private readonly ICommonAggregate _CommonAggregate;
        private readonly IOrganizationNodeProcessProvider _NodeFactory;
        private readonly IOrganizationAggregate _OrganizationAggregate;

        public HeaderQuarterNodeController(ICommonAggregate CommonAggregate,
                                           IOrganizationAggregate OrganizationAggregate,
                                           IIndex<OrganizationType, IOrganizationNodeProcessProvider> NodeFactories)
        {
            _CommonAggregate = CommonAggregate;
            _OrganizationAggregate = OrganizationAggregate;
            _NodeFactory = NodeFactories[OrganizationType.HeaderQuarter];
        }

        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(HeaderQuarterNode_lang.HEADERQUARTER_NODE_GET_ALL))]
        [ModelValidator(false)]
        public async Task<IHttpResult> GetAll()
        {
            try
            {
                var nodes = (await _NodeFactory.GetAll())?
                                        .Cast<HeaderQuarterNode>()
                                        .Select(x => new HeaderQuarterNodeViewModel(x))
                                        .ToList();

                var result = new JsonResult<IEnumerable<HeaderQuarterNodeViewModel>>(nodes, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                   ex.PrefixDevMessage(HeaderQuarterNode_lang.HEADERQUARTER_NODE_GET_ALL_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(HeaderQuarterNode_lang.HEADERQUARTER_NODE_GET_ALL_FAIL), false)
                    .Async();
            }
        }

        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(HeaderQuarterNode_lang.HEADERQUARTER_NODE_GET_NESTED))]
        [ModelValidator(false)]
        public async Task<IHttpResult> GetAllNested()
        {
            try
            {
                var nodes = await _NodeFactory.GetAll();

                var nested = (HeaderQuarterNode)nodes.AsNestedNSM();

                var result = new JsonResult<HeaderQuarterNodeViewModel>(
                    new HeaderQuarterNodeViewModel(nested), true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                   ex.PrefixDevMessage(HeaderQuarterNode_lang.HEADERQUARTER_NODE_GET_NESTED_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(HeaderQuarterNode_lang.HEADERQUARTER_NODE_GET_NESTED_FAIL), false)
                    .Async();
            }
        }

        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Add)]
        [Logger(nameof(HeaderQuarterNode_lang.HEADERQUARTER_NODE_CREATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Create([FromBody] string content)
        {
            try
            {
                // 因為前端會拋出整個組織樹 倒置 ViewModel Binding 過慢, 所以改用Json字串傳遞
                var model = JsonConvert.DeserializeObject<HeaderQuarterNodeViewModel>(content);

                var nestedNode = model.ToDomain();

                await _NodeFactory.Create(nestedNode);

                var result = new JsonResult(HeaderQuarterNode_lang.HEADERQUARTER_NODE_CREATE_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                   ex.PrefixDevMessage(HeaderQuarterNode_lang.HEADERQUARTER_NODE_CREATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(HeaderQuarterNode_lang.HEADERQUARTER_NODE_CREATE_FAIL), false)
                    .Async();
            }
        }

        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Delete)]
        [Logger(nameof(HeaderQuarterNode_lang.HEADERQUARTER_NODE_DISABLE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Disable([Required] int? nodeID)
        {
            try
            {
                await _NodeFactory.Disable(nodeID.Value);

                var result = new JsonResult(HeaderQuarterNode_lang.HEADERQUARTER_NODE_DISABLE_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                   ex.PrefixDevMessage(HeaderQuarterNode_lang.HEADERQUARTER_NODE_DISABLE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(HeaderQuarterNode_lang.HEADERQUARTER_NODE_DISABLE_FAIL), false)
                    .Async();
            }
        }

        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(HeaderQuarterNode_lang.HEADERQUARTER_NODE_GET))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Get([Required] int? nodeID)
        {
            try
            {
                var node = await _NodeFactory.GetComplete(nodeID.Value);

                var model = new HeaderQuarterNodeDetailViewModel((HeaderQuarterNode)node);

                var result = new JsonResult<HeaderQuarterNodeDetailViewModel>(model, HeaderQuarterNode_lang.HEADERQUARTER_NODE_GET_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                                  ex.PrefixDevMessage(HeaderQuarterNode_lang.HEADERQUARTER_NODE_GET_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(HeaderQuarterNode_lang.HEADERQUARTER_NODE_GET_FAIL), false)
                    .Async();
            }
        }

        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Update)]
        [Logger(nameof(HeaderQuarterNode_lang.HEADERQUARTER_NODE_UPDATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Update(HeaderQuarterNodeDetailViewModel model)
        {
            try
            {
                var data = model.ToDomain();

                var node = await _NodeFactory.Update(data);

                var newModel = new HeaderQuarterNodeDetailViewModel((HeaderQuarterNode)node);

                var result = new JsonResult<HeaderQuarterNodeDetailViewModel>(newModel, HeaderQuarterNode_lang.HEADERQUARTER_NODE_UPDATE_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                                  ex.PrefixDevMessage(HeaderQuarterNode_lang.HEADERQUARTER_NODE_UPDATE_FAIL));

                return await new JsonResult(
                        ex.PrefixMessage(HeaderQuarterNode_lang.HEADERQUARTER_NODE_UPDATE_FAIL), false)
                        .Async();
            }
        }

        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Update)]
        [Logger(nameof(HeaderQuarterNode_lang.HEADERQUARTER_UPDATE_TREE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> UpdateTree([FromBody] string content)
        {
            try
            {
                // 因為前端會拋出整個組織樹 倒置 ViewModel Binding 過慢, 所以改用Json字串傳遞
                var model = JsonConvert.DeserializeObject<HeaderQuarterNodeViewModel>(content);

                var data = model.ToDomain();

                await _NodeFactory.UpdateTree(data);

                var result = new JsonResult(HeaderQuarterNode_lang.HEADERQUARTER_UPDATE_TREE_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                                  ex.PrefixDevMessage(HeaderQuarterNode_lang.HEADERQUARTER_UPDATE_TREE_FAIL));

                return await new JsonResult(
                        ex.PrefixMessage(HeaderQuarterNode_lang.HEADERQUARTER_UPDATE_TREE_FAIL), false)
                        .Async();
            }
        }

        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Add)]
        [Logger(nameof(HeaderQuarterNode_lang.HEADERQUARTER_ADD_JOB))]
        [ModelValidator(false)]
        public async Task<IHttpResult> AddJob(AddJobViewModel model)
        {
            try
            {
                await _NodeFactory.AddJobs(model.NodeID.Value, model.JobIDs.ToArray());

                var result = new JsonResult(HeaderQuarterNode_lang.HEADERQUARTER_ADD_JOB_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                                  ex.PrefixDevMessage(HeaderQuarterNode_lang.HEADERQUARTER_ADD_JOB_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(HeaderQuarterNode_lang.HEADERQUARTER_ADD_JOB_FAIL), false)
                    .Async();
            }
        }

        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Delete)]
        [Logger(nameof(HeaderQuarterNode_lang.HEADERQUARTER_DELETE_JOB))]
        [ModelValidator(false)]
        public async Task<IHttpResult> DeleteJob([Required] int? nodeJobID)
        {
            try
            {
                await _NodeFactory.DeleteJob(nodeJobID.Value);

                var result = new JsonResult(HeaderQuarterNode_lang.HEADERQUARTER_DELETE_JOB_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                                  ex.PrefixDevMessage(HeaderQuarterNode_lang.HEADERQUARTER_DELETE_JOB_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(HeaderQuarterNode_lang.HEADERQUARTER_DELETE_JOB_FAIL), false)
                    .Async();
            }
        }

        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Add)]
        [Logger(nameof(HeaderQuarterNode_lang.HEADERQUARTER_ADD_USER))]
        [ModelValidator(false)]
        public async Task<IHttpResult> AddUser(AddUserViewModel model)
        {
            try
            {
                await _NodeFactory.AddUsers(model.NodeJobID.Value, model.UserIDs.ToArray());

                var result = new JsonResult(HeaderQuarterNode_lang.HEADERQUARTER_ADD_USER_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                                  ex.PrefixDevMessage(HeaderQuarterNode_lang.HEADERQUARTER_ADD_USER_SUCCESS));

                return await new JsonResult(
                    ex.PrefixMessage(HeaderQuarterNode_lang.HEADERQUARTER_ADD_USER_FAIL), false)
                    .Async();
            }
        }

        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Delete)]
        [Logger(nameof(HeaderQuarterNode_lang.HEADERQUARTER_DELETE_USER))]
        [ModelValidator(false)]
        public async Task<IHttpResult> DeleteUser([Required] int? nodeJobID, [Required] string userID)
        {
            try
            {
                await _NodeFactory.DeleteUser(nodeJobID.Value, userID);

                var result = new JsonResult(HeaderQuarterNode_lang.HEADERQUARTER_DELETE_USER_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                                  ex.PrefixDevMessage(HeaderQuarterNode_lang.HEADERQUARTER_DELETE_USER_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(HeaderQuarterNode_lang.HEADERQUARTER_DELETE_USER_FAIL), false)
                    .Async();
            }
        }
    }
}
