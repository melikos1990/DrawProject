using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Areas.Master.Models.CaseTag;
using SMARTII.Assist.Authentication;
using SMARTII.Assist.Logger;
using SMARTII.Assist.Web;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Case;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Master;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;
using SMARTII.Service.Organization.Resolver;
using SMARTII.Service.System.Resolver;

namespace SMARTII.Areas.Master.Controllers
{
    [Authentication]
    public class CaseTagController : BaseApiController
    {
        private readonly ICommonAggregate _CommonAggregate;
        private readonly IMasterAggregate _MasterAggregate;
        private readonly ICaseTagFacade _CaseTagFacade;
        private readonly OrganizationNodeResolver _OrganizationResolver;
        private readonly SystemParameterResolver _SystemParameterResolver;

        public CaseTagController(
            ICommonAggregate CommonAggregate,
            IMasterAggregate MasterAggregate,
            ICaseTagFacade CaseTagFacade,
            OrganizationNodeResolver OrganizationResolver,
            SystemParameterResolver SystemParameterResolver)
        {
            _CommonAggregate = CommonAggregate;
            _MasterAggregate = MasterAggregate;
            _CaseTagFacade = CaseTagFacade;
            _OrganizationResolver = OrganizationResolver;
            _SystemParameterResolver = SystemParameterResolver;
        }

        /// <summary>
        /// 取得清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(CaseTag_lang.CASE_TAG_GETLIST))]
        [ModelValidator(false)]
        public async Task<PagingResponse> GetList(PagingRequest<CaseTagSearchViewModel> model)
        {
            try
            {
                var buIDs = UserIdentity.Instance.DownProviderBUDist.TryGetBuList(Domain.Organization.OrganizationType.CallCenter);

                var searchTerm = model.criteria;

                var con = new MSSQLCondition<CASE_TAG>(
                   searchTerm,
                   model.pageIndex,
                   model.pageSize);

                //若無指定BU查詢，將會撈出該人員負責之BU
                if (searchTerm.BuID == null)
                {
                    con.And(x => buIDs.Contains(x.NODE_ID));
                }

                con.OrderBy(model.sort, model.orderType);

                var list = _MasterAggregate.CaseTag_T1_T2_.GetPaging(con);

                var ui = _OrganizationResolver.ResolveCollection(list)
                         .Select(x => new CaseTagListViewModel(x));

                return await new PagingResponse<IEnumerable<CaseTagListViewModel>>(ui)
                {
                    isSuccess = true,
                    totalCount = list.TotalCount
                }.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(CaseTag_lang.CASE_TAG_GETLIST_FAIL));

                return await new PagingResponse()
                {
                    isSuccess = false,
                    message = ex.PrefixMessage(CaseTag_lang.CASE_TAG_GETLIST_FAIL)
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
        [Logger(nameof(CaseTag_lang.CASE_TAG_GET))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Get([Required]int? ID)
        {
            try
            {
                var item = _MasterAggregate.CaseTag_T1_T2_
                                           .Get(x => x.ID == ID);

                if (item == null)
                    throw new Exception(Common_lang.NOT_FOUND_DATA);

                var result = new JsonResult<CaseTagDetailViewModel>(
                                   new CaseTagDetailViewModel(item), true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(CaseTag_lang.CASE_TAG_GET_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(CaseTag_lang.CASE_TAG_GET_FAIL), false)
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
        [Logger(nameof(CaseTag_lang.CASE_TAG_CREATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Create(CaseTagDetailViewModel model)
        {
            try
            {
                var domain = new CaseTag()
                {
                    NodeID = model.BuID,
                    IsEnabled = model.IsEnabled,
                    Name = model.Name,
                    OrganizationType = model.OrganizationType
                };

                var result = await _CaseTagFacade.Create(domain);

                return await new JsonResult(
                    CaseTag_lang.CASE_TAG_CREATE_SUCCESS, true)
                    .Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(CaseTag_lang.CASE_TAG_CREATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(CaseTag_lang.CASE_TAG_CREATE_FAIL), false)
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
        [Logger(nameof(CaseTag_lang.CASE_TAG_UPDATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Update(CaseTagDetailViewModel model)
        {
            try
            {
                var domain = new CaseTag()
                {
                    NodeID = model.BuID,
                    ID = model.ID,
                    IsEnabled = model.IsEnabled,
                    Name = model.Name,
                    OrganizationType = model.OrganizationType
                };

                var result = await _CaseTagFacade.Update(domain);

                return await new JsonResult(
                    CaseTag_lang.CASE_TAG_UPDATE_SUCCESS, true)
                    .Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(CaseTag_lang.CASE_TAG_UPDATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(CaseTag_lang.CASE_TAG_UPDATE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 單一停用
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Delete)]
        [Logger(nameof(CaseTag_lang.CASE_TAG_DISABLED))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Disabled([Required]int? ID)
        {
            try
            {
                var con = new MSSQLCondition<CASE_TAG>(x => x.ID == ID);
                con.ActionModify(x =>
                {
                    x.IS_ENABLED = false;
                    x.UPDATE_DATETIME = DateTime.Now;
                    x.UPDATE_USERNAME = UserIdentity.Instance.Name;
                });

                _MasterAggregate.CaseTag_T1_T2_.Update(con);

                var result = new JsonResult<string>(CaseTag_lang.CASE_TAG_DISABLED_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                     ex.PrefixDevMessage(CaseTag_lang.CASE_TAG_DISABLED_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(CaseTag_lang.CASE_TAG_DISABLED_FAIL), false)
                    .Async();
            }
        }
    }
}
