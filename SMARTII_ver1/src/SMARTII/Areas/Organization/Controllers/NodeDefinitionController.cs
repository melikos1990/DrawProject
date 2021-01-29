using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Areas.Organization.Models.OrganizarionNodeDefinition;
using SMARTII.Assist.Authentication;
using SMARTII.Assist.Logger;
using SMARTII.Assist.Web;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;

namespace SMARTII.Areas.Organization.Controllers
{
    [Authentication]
    public class NodeDefinitionController : BaseApiController
    {
        private readonly ICommonAggregate _CommonAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly INodeDefinitionFacade _NodeDefinitionFacade;

        public NodeDefinitionController(ICommonAggregate CommonAggregate,
                                        IOrganizationAggregate OrganizationAggregate,
                                        INodeDefinitionFacade NodeDefinitionFacade)
        {
            _CommonAggregate = CommonAggregate;
            _OrganizationAggregate = OrganizationAggregate;
            _NodeDefinitionFacade = NodeDefinitionFacade;
        }

        /// <summary>
        /// 取得清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(NodeDefinition_lang.ORGANIZATION_NODE_FEFINITION_GETLIST))]
        [ModelValidator(false)]
        public async Task<PagingResponse> GetList(PagingRequest<NodeDefinitionSearchViewModel> model)
        {
            try
            {
                var con = new MSSQLCondition<ORGANIZATION_NODE_DEFINITION>(
                   model.criteria,
                   model.pageIndex,
                   model.pageSize);

                con.OrderBy(model.sort, model.orderType);

                var list = _OrganizationAggregate.OrganizationNodeDefinition_T1_T2_.GetPaging(con);

                var ui = list.Select(x => new NodeDefinitionListViewModel(x));

                return await new PagingResponse<IEnumerable<NodeDefinitionListViewModel>>(ui)
                {
                    isSuccess = true,
                    totalCount = list.TotalCount
                }.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(NodeDefinition_lang.ORGANIZATION_NODE_FEFINITION_GETLIST_FAIL));

                return await new PagingResponse()
                {
                    isSuccess = false,
                    message = ex.PrefixMessage(NodeDefinition_lang.ORGANIZATION_NODE_FEFINITION_GETLIST_FAIL)
                }.Async();
            }
        }

        /// <summary>
        /// 單一取得
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(NodeDefinition_lang.ORGANIZATION_NODE_FEFINITION_GET))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Get([Required]OrganizationType OrganizationType, [Required]int ID)
        {
            try
            {
                var con = new MSSQLCondition<ORGANIZATION_NODE_DEFINITION>(
                    x => x.ID == ID &&
                    x.ORGANIZATION_TYPE == (byte)OrganizationType);

                con.IncludeBy(x => x.JOB);

                var item = _OrganizationAggregate.OrganizationNodeDefinition_T1_T2_.Get(con);

                if (item == null)
                    throw new Exception(Common_lang.NOT_FOUND_DATA);

                var result = new JsonResult<NodeDefinitionDetailViewModel>(
                                   new NodeDefinitionDetailViewModel(item), true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(NodeDefinition_lang.ORGANIZATION_NODE_FEFINITION_GET_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(NodeDefinition_lang.ORGANIZATION_NODE_FEFINITION_GET_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 單一新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Add)]
        [Logger(nameof(NodeDefinition_lang.ORGANIZATION_NODE_FEFINITION_CREATE))]
        [ModelValidator]
        public async Task<IHttpResult> Create(NodeDefinitionDetailViewModel model)
        {
            try
            {
                var isRepeat = _OrganizationAggregate.OrganizationNodeDefinition_T1_T2_.HasAny(x => x.ORGANIZATION_TYPE == (byte)model.OrganizationType
                                                                                                && x.IDENTIFICATION_ID == model.Identification
                                                                                                && x.NAME == model.Name);
                if (isRepeat)
                    throw new Exception(NodeDefinition_lang.ORGANIZATION_NODE_DEFINITION_NAME_REPEAT);

                var definition = new OrganizationNodeDefinition()
                {
                    Level = model.Level,
                    IsEnabled = model.IsEnabled,
                    Name = model.Name,
                    Identification = model.Identification,
                    IdentificationName = model.IdentificationName,
                    OrganizationType = model.OrganizationType,
                    Key = model.Key,
                    CreateDateTime = DateTime.Now,
                    CreateUserName = this.UserIdentity.Name
                };

                var result = _OrganizationAggregate.OrganizationNodeDefinition_T1_T2_.Add(definition);

                return await new JsonResult<NodeDefinitionDetailViewModel>(
                   new NodeDefinitionDetailViewModel(result),
                   NodeDefinition_lang.ORGANIZATION_NODE_FEFINITION_CREATE_SUCCESS, true)
                   .Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                 ex.PrefixDevMessage(NodeDefinition_lang.ORGANIZATION_NODE_FEFINITION_CREATE_FAIL));

                return await new JsonResult(ex.PrefixMessage(NodeDefinition_lang.ORGANIZATION_NODE_FEFINITION_CREATE_FAIL), false)
                                 .Async();
            }
        }

        /// <summary>
        /// 單一更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Update)]
        [Logger(nameof(NodeDefinition_lang.ORGANIZATION_NODE_FEFINITION_UPDATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Update(NodeDefinitionDetailViewModel model)
        {
            try
            {
                var domain = new OrganizationNodeDefinition()
                {
                    ID = model.ID,
                    Name = model.Name,
                    Identification = model.Identification,
                    IdentificationName = model.IdentificationName,
                    OrganizationType = model.OrganizationType,
                    UpdateDateTime =  DateTime.Now,
                    UpdateUserName = ContextUtility.GetUserIdentity()?.Name,
                    IsEnabled = model.IsEnabled,
                    Level = model.Level,
                    Key = model.Key,
                };

                await _NodeDefinitionFacade.Update(domain);

                return await new JsonResult(
                   NodeDefinition_lang.ORGANIZATION_NODE_FEFINITION_UPDATE_SUCCESS, true)
                   .Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                 ex.PrefixDevMessage(NodeDefinition_lang.ORGANIZATION_NODE_FEFINITION_UPDATE_FAIL));

                return await new JsonResult(
                        ex.PrefixMessage(NodeDefinition_lang.ORGANIZATION_NODE_FEFINITION_UPDATE_FAIL), false).Async();
            }
        }

        /// <summary>
        /// 單一停用
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Delete)]
        [Logger(nameof(NodeDefinition_lang.ORGANIZATION_NODE_FEFINITION_DISABLE))]
        [ModelValidator]
        public async Task<IHttpResult> Disable([Required]OrganizationType organizationType, [Required]int ID)
        {
            try
            {

                await _NodeDefinitionFacade.Disable(organizationType, ID);

                return await new JsonResult(
                  NodeDefinition_lang.ORGANIZATION_NODE_FEFINITION_DISABLE_SUCCESS, true)
                  .Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
               ex.PrefixDevMessage(NodeDefinition_lang.ORGANIZATION_NODE_FEFINITION_DISABLE_FAIL));

                return await new JsonResult(ex.PrefixMessage(NodeDefinition_lang.ORGANIZATION_NODE_FEFINITION_DISABLE_FAIL), false)
                                 .Async();
            }
        }

        /// <summary>
        /// 取得清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(Job_lang.JOB_GET_LIST))]
        [ModelValidator(false)]
        public async Task<PagingResponse> GetJobList(PagingRequest<JobSearchViewModel> model)
        {
            try
            {
                var con = new MSSQLCondition<JOB>(
                   model.criteria,
                   model.pageIndex,
                   model.pageSize);

                con.OrderBy(model.sort, model.orderType);

                var list = _OrganizationAggregate.Job_T1_T2_.GetPaging(con);

                var ui = list.Select(x => new JobListViewModel(x));

                return await new PagingResponse<IEnumerable<JobListViewModel>>(ui)
                {
                    isSuccess = true,
                    totalCount = list.TotalCount
                }.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(Job_lang.JOB_GET_LIST_FAIL));

                return await new PagingResponse()
                {
                    isSuccess = false,
                    message = ex.PrefixMessage(Job_lang.JOB_GET_LIST_FAIL)
                }.Async();
            }
        }

        /// <summary>
        /// 單一新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Add)]
        [Logger(nameof(Job_lang.JOB_CREATE))]
        [ModelValidator]
        public async Task<IHttpResult> CreateJob(JobDetailViewModel model)
        {
            try
            {
                var isRepeat = _OrganizationAggregate.Job_T1_T2_.HasAny(x => x.ORGANIZATION_TYPE == (byte)model.OrganizationType
                                                                                && x.DEFINITION_ID == model.DefinitionID
                                                                                && x.NAME == model.Name);
                if (isRepeat)
                    throw new Exception(Job_lang.JOB_NAME_REPEAT);

                var job = new Job()
                {
                    Level = model.Level,
                    IsEnabled = model.IsEnabled,
                    Name = model.Name,
                    DefinitionID = model.DefinitionID,
                    OrganizationType = model.OrganizationType,
                    CreateDateTime = DateTime.Now,
                    CreateUserName = this.UserIdentity.Name,
                    Key = model.Key
                };

                var data = _OrganizationAggregate.Job_T1_T2_.Add(job);

                return await new JsonResult<JobDetailViewModel>(new JobDetailViewModel(data),
                    Job_lang.JOB_CREATE_SUCCESS, true).Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                 ex.PrefixDevMessage(Job_lang.JOB_CREATE_FAIL));

                return await new JsonResult(ex.PrefixMessage(Job_lang.JOB_CREATE_FAIL), false).Async();
            }
        }

        /// <summary>
        /// 單一更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Update)]
        [Logger(nameof(Job_lang.JOB_UPDATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> UpdateJob(JobDetailViewModel model)
        {
            try
            {
                var domain = new Job()
                {
                    ID = model.ID,
                    Name = model.Name,
                    DefinitionID = model.DefinitionID,
                    IsEnabled = model.IsEnabled,
                    UpdateDateTime = DateTime.Now,
                    UpdateUserName = ContextUtility.GetUserIdentity()?.Name,
                    Level = model.Level,
                    OrganizationType = model.OrganizationType,
                    Key = model.Key,
                };

                var data = await _NodeDefinitionFacade.UpdateJob(domain);

                return await new JsonResult<JobDetailViewModel>(new JobDetailViewModel(data), Job_lang.JOB_UPDATE_SUCCESS, true)
                   .Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                 ex.PrefixDevMessage(Job_lang.JOB_UPDATE_FAIL));

                return await new JsonResult(
                        ex.PrefixMessage(Job_lang.JOB_UPDATE_FAIL), false).Async();
            }
        }

        /// <summary>
        /// 單一停用
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Delete)]
        [Logger(nameof(Job_lang.JOB_DISABLE))]
        [ModelValidator]
        public async Task<IHttpResult> DisableJob([Required]int ID)
        {
            try
            {
                await _NodeDefinitionFacade.DisableJob(ID);

                return await new JsonResult(Job_lang.JOB_DISABLE_SUCCESS, true).Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex.PrefixDevMessage(Job_lang.JOB_DISABLE_FAIL));

                return await new JsonResult(ex.PrefixMessage(Job_lang.JOB_DISABLE_FAIL), false)
                                 .Async();
            }
        }
    }
}
