using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Areas.Substitute.Models.CaseApply;
using SMARTII.Assist.Authentication;
using SMARTII.Assist.Logger;
using SMARTII.Assist.Web;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Case;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Substitute;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;
using SMARTII.Service.Organization.Resolver;

namespace SMARTII.Areas.Substitute.Controllers
{
    [Authentication]
    public class CaseApplyController : BaseApiController
    {
        private readonly ICommonAggregate _CommonAggregate;
        private readonly ICaseAggregate _CaseAggregate;
        private readonly ICaseApplyFacade _CaseApplyFacade;
        private readonly IUserFacade _UserFacade;
        private readonly OrganizationNodeResolver _OrganizationResolver;

        public CaseApplyController(ICommonAggregate CommonAggAggregate,
                                   ICaseAggregate CaseAggregate,
                                   ICaseApplyFacade CaseApplyFacade,
                                   IUserFacade UserFacade,
                                   OrganizationNodeResolver OrganizationResolver)
        {
            _CommonAggregate = CommonAggAggregate;
            _CaseAggregate = CaseAggregate;
            _CaseApplyFacade = CaseApplyFacade;
            _UserFacade = UserFacade;
            _OrganizationResolver = OrganizationResolver;
        }

        /// <summary>
        /// 取得清單
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(CaseApply_lang.CASE_APPLY_GETLIST))]
        [ModelValidator(false)]
        public async Task<PagingResponse> GetList(PagingRequest<CaseApplySearchViewModel> model)
        {
            try
            {
                var buIDs = UserIdentity.Instance.DownProviderBUDist.TryGetBuList(Domain.Organization.OrganizationType.CallCenter);

                var searchTerm = model.criteria;

                var con = new MSSQLCondition<CASE>(
                    searchTerm,
                    model.pageIndex,
                    model.pageSize
                    );

                //若無指定BU查詢，將會撈出該人員負責之BU
                if (searchTerm.NodeID == null)
                {
                    con.And(x => buIDs.Contains(x.NODE_ID));
                }

                con.IncludeBy(x => x.CASE_WARNING);
                con.OrderBy(model.sort, model.orderType);

                var list = _CaseAggregate.Case_T1_T2_.GetPaging(con);

                var ui = _OrganizationResolver.ResolveCollection(list).Select(x => new CaseApplyListViewModel(x));

                return await new PagingResponse<IEnumerable<CaseApplyListViewModel>>(ui)
                {
                    isSuccess = true,
                    totalCount = list.TotalCount
                }.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(CaseApply_lang.CASE_APPLY_GETLIST_FAIL));

                return await new PagingResponse()
                {
                    isSuccess = false,
                    message = ex.PrefixMessage(CaseApply_lang.CASE_APPLY_GETLIST_FAIL)
                }.Async();
            }
        }

        /// <summary>
        /// 分派案件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Update)]
        [Logger(nameof(CaseApply_lang.CASE_APPLY_APPLY))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Apply(CaseApplyCommitViewModel model)
        {
            try
            {
                var user = await _UserFacade.GetUserGroupFromIDAsync(model.ApplyUserID);

                if (user == null)
                {
                    throw new Exception(CaseApply_lang.CASE_APPLY_USER_NOTFOUND);
                }

                await _CaseApplyFacade.Apply(user, model.CaseIDs);

                return await new JsonResult(
                    CaseTemplate_lang.CASE_TEMPLATE_CREATE_SUCCESS, true)
                    .Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(CaseApply_lang.CASE_APPLY_APPLY_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(CaseApply_lang.CASE_APPLY_APPLY_FAIL), false)
                    .Async();
            }
        }
    }
}
