using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Areas.Master.Models.CaseTemplate;
using SMARTII.Assist.Authentication;
using SMARTII.Assist.Logger;
using SMARTII.Assist.Web;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Case;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using SMARTII.Domain.System;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;
using SMARTII.Service.Organization.Resolver;
using SMARTII.Service.System.Resolver;

namespace SMARTII.Areas.Master.Controllers
{
    [Authentication]
    public class CaseTemplateController : BaseApiController
    {
        private readonly ICommonAggregate _CommonAggregate;
        private readonly IMasterAggregate _MasterAggregate;
        private readonly ISystemAggregate _SystemAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly ICaseTemplateFacade _CaseTemplateFacade;
        private readonly OrganizationNodeResolver _OrganizationResolver;
        private readonly SystemParameterResolver _SystemParameterResolver;

        public CaseTemplateController(
            ICommonAggregate CommonAggregate,
            IMasterAggregate MasterAggregate,
            ISystemAggregate SystemAggregate,
            IOrganizationAggregate OrganizationAggregate,
            ICaseTemplateFacade CaseTemplateFacade,
            OrganizationNodeResolver OrganizationResolver,
            SystemParameterResolver SystemParameterResolver)
        {
            _CommonAggregate = CommonAggregate;
            _MasterAggregate = MasterAggregate;
            _CaseTemplateFacade = CaseTemplateFacade;
            _OrganizationResolver = OrganizationResolver;
            _SystemParameterResolver = SystemParameterResolver;
            _SystemAggregate = SystemAggregate;
            _OrganizationAggregate = OrganizationAggregate;
        }

        /// <summary>
        /// 取得清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(CaseTemplate_lang.CASE_TEMPLATE_GETLIST))]
        [ModelValidator(false)]
        public async Task<PagingResponse> GetList(PagingRequest<CaseTemplateSearchViewModel> model)
        {
            try
            {
                var buIDs = UserIdentity.Instance.DownProviderBUDist.TryGetBuList(Domain.Organization.OrganizationType.CallCenter);

                var searchTerm = model.criteria;

                var con = new MSSQLCondition<CASE_TEMPLATE>(
                   searchTerm,
                   model.pageIndex,
                   model.pageSize);

                //若無指定BU查詢，將會撈出該人員負責之BU
                if (searchTerm.BuID == null)
                {
                    con.And(x => buIDs.Contains(x.NODE_ID));
                }

                con.OrderBy(model.sort, model.orderType);

                var list = _MasterAggregate.CaseTemplate_T1_T2_.GetPaging(con);

                var ui = _SystemParameterResolver.ResolveCollection
                         (
                            _OrganizationResolver.ResolveCollection(list)
                         )
                         .Select(x => new CaseTemplateListViewModel(x));

                return await new PagingResponse<IEnumerable<CaseTemplateListViewModel>>(ui)
                {
                    isSuccess = true,
                    totalCount = list.TotalCount
                }.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(CaseTemplate_lang.CASE_TEMPLATE_GETLIST_FAIL));

                return await new PagingResponse()
                {
                    isSuccess = false,
                    message = ex.PrefixMessage(CaseTemplate_lang.CASE_TEMPLATE_GETLIST_FAIL)
                }.Async();
            }
        }

        /// <summary>
        /// 單一取得
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(CaseTemplate_lang.CASE_TEMPLATE_GET))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Get([Required]int? ID)
        {
            try
            {
                var item = _MasterAggregate.CaseTemplate_T1_T2_
                                           .Get(x => x.ID == ID);

                if (item == null)
                    throw new Exception(Common_lang.NOT_FOUND_DATA);


                var getBuName = _OrganizationResolver.Resolve(item);

                var complete = _SystemParameterResolver.Resolve(getBuName);

                var result = new JsonResult<CaseTemplateDetailViewModel>(
                                   new CaseTemplateDetailViewModel(complete), true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(CaseTemplate_lang.CASE_TEMPLATE_GET_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(CaseTemplate_lang.CASE_TEMPLATE_GET_FAIL), false)
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
        [Logger(nameof(CaseTemplate_lang.CASE_TEMPLATE_CREATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Create(CaseTemplateDetailViewModel model)
        {
            try
            {
                var domain = new CaseTemplate()
                {
                    NodeID = model.BuID,
                    ClassificKey = model.ClassificKey,
                    Title = model.Title.Trim(),
                    Content = model.Content,
                    EmailTitle = model.EmailTitle,
                    IsDefault = model.IsDefault,
                    IsFastFinished = model.IsFastFinished

                };

                var result = await _CaseTemplateFacade.Create(domain);

                return await new JsonResult(
                    CaseTemplate_lang.CASE_TEMPLATE_CREATE_SUCCESS, true)
                    .Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(CaseTemplate_lang.CASE_TEMPLATE_CREATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(CaseTemplate_lang.CASE_TEMPLATE_CREATE_FAIL), false)
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
        [Logger(nameof(CaseTemplate_lang.CASE_TEMPLATE_UPDATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Update(CaseTemplateDetailViewModel model)
        {
            try
            {
                var domain = new CaseTemplate()
                {
                    ID = model.ID,
                    NodeID = model.BuID,
                    ClassificKey = model.ClassificKey,
                    Title = model.Title.Trim(),
                    Content = model.Content,
                    EmailTitle = model.EmailTitle,
                    IsDefault = model.IsDefault,
                    IsFastFinished = model.IsFastFinished
                };

                var result = await _CaseTemplateFacade.Update(domain);

                return await new JsonResult(
                    CaseTemplate_lang.CASE_TEMPLATE_UPDATE_SUCCESS, true)
                    .Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(CaseTemplate_lang.CASE_TEMPLATE_UPDATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(CaseTemplate_lang.CASE_TEMPLATE_UPDATE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 單一刪除
        /// </summary>
        /// <param name="BuID"></param>
        /// <param name="ClassificKey"></param>
        /// <param name="Title"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Delete)]
        [Logger(nameof(CaseTemplate_lang.CASE_TEMPLATE_DELETE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Delete([Required]int? ID)
        {
            try
            {
                var isSuccess = _MasterAggregate.CaseTemplate_T1_T2_
                                                .Remove(x => x.ID == ID);

                if (isSuccess == false)
                    throw new Exception(Common_lang.DATABASE_DELETE_ERROR);

                return await new JsonResult(
                    CaseTemplate_lang.CASE_TEMPLATE_DELETE_SUCCESS, true)
                    .Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                   ex.PrefixDevMessage(CaseTemplate_lang.CASE_TEMPLATE_DELETE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(CaseTemplate_lang.CASE_TEMPLATE_DELETE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 批次刪除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Delete)]
        [Logger(nameof(CaseTemplate_lang.CASE_TEMPLATE_DELETE_RANGE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> DeleteRange(List<CaseTemplateDetailViewModel> model)
        {
            try
            {
                var con = new MSSQLCondition<CASE_TEMPLATE>();

                model.ForEach(g => con.Or(x => x.ID == g.ID));

                var isSuccess = _MasterAggregate.CaseTemplate_T1_.RemoveRange(con);

                if (isSuccess == false)
                    throw new Exception(Common_lang.DATABASE_DELETE_ERROR);

                return await new JsonResult(
                 CaseTemplate_lang.CASE_TEMPLATE_DELETE_SUCCESS, true)
                 .Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(CaseTemplate_lang.CASE_TEMPLATE_DELETE_RANGE_FAILED));

                return await new JsonResult(
                    ex.PrefixMessage(CaseTemplate_lang.CASE_TEMPLATE_DELETE_RANGE_SUCCESS), false)
                    .Async();
            }
        }


        [HttpGet]
        [ModelValidator(false)]
        public async Task<IHttpResult> CheckFastFinish([Required] int buID)
        {
            try
            {
                var con = new MSSQLCondition<HEADQUARTERS_NODE>(x => x.NODE_ID == buID);
                var nodeKey = _OrganizationAggregate.HeaderQuarterNode_T1_T2_.GetOfSpecific(con, x => x.NODE_KEY);

                // 1 == 快速結案
                var hasFastFinish = _SystemAggregate.SystemParameter_T1_T2_.HasAny(x => x.KEY == nodeKey && x.ID == EssentialCache.CaseValue.CASE_ALLOW_FASTCLOSE && x.VALUE == "1");

                return await new JsonResult<bool>(hasFastFinish, true) { }.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                   ex.PrefixDevMessage(CaseTemplate_lang.CASE_TEMPLATE_CHACK_FASTFINISH));

                return await new JsonResult<bool>(false, false) { }.Async();
            }
        }
    }
}
