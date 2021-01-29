using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Areas.Summary.Models;
using SMARTII.Areas.Summary.Models.CallCenterSummary;
using SMARTII.Assist.Authentication;
using SMARTII.Assist.Logger;
using SMARTII.Assist.Web;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Case;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;
using static SMARTII.Domain.Cache.EssentialCache;
using SMARTII.Service.Master.Resolver;

namespace SMARTII.Areas.Summary.Controllers
{
    [Authentication]
    public class CallCenterSummaryController : BaseApiController
    {
        private readonly ICaseAggregate _CaseAggregate;
        private readonly ICommonAggregate _CommonAggregate;
        private readonly IExecutiveOrganizationNodeProvider<CALLCENTER_NODE> _OrganizationNodeProvider;    
        private readonly ICaseSearchFacade _CaseSearchFacade;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly QuestionClassificationResolver _QuestionClassificationResolver;

        public CallCenterSummaryController(ICaseAggregate CaseAggregate,
                                           ICommonAggregate CommonAggregate,
                                           IExecutiveOrganizationNodeProvider<CALLCENTER_NODE> OrganizationNodeProvider,
                                           ICaseSearchFacade CaseSearchFacade,
                                           IOrganizationAggregate OrganizationAggregate,
                                           QuestionClassificationResolver QuestionClassificationResolver)
        {
            _CaseAggregate = CaseAggregate;
            _CommonAggregate = CommonAggregate;
            _OrganizationNodeProvider = OrganizationNodeProvider;
            _CaseSearchFacade = CaseSearchFacade;
            _OrganizationAggregate = OrganizationAggregate;
            _QuestionClassificationResolver = QuestionClassificationResolver;
        }

        /// <summary>
        /// 取得個人所服務的BU 清單
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(CallCenterSummary_lang.CALLCENTER_SUMMARY_GET_PROVIDER_BUs))]
        public async Task<IHttpResult> GetProviderBUs()
        {
            try
            {
                var providerBUs = _OrganizationNodeProvider.GetOwnDownwardProviderNodes(OrganizationType.CallCenter, true);

                var list = providerBUs.Select(x => new SummaryTargetViewModel(x));

                var result = new JsonResult<IEnumerable<SummaryTargetViewModel>>(list, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(CallCenterSummary_lang.CALLCENTER_SUMMARY_GET_PROVIDER_BUs_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(CallCenterSummary_lang.CALLCENTER_SUMMARY_GET_PROVIDER_BUs_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 取得個人未結案件數
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(CallCenterSummary_lang.CALLCENTER_SUMMARY_GET_OWN_COUNT))]
        public async Task<IHttpResult> GetOwnCount(int? buID)
        {
            try
            {
                var userID = UserIdentity.Instance?.UserID;

                var con = new MSSQLCondition<CASE>(x => x.NODE_ID == buID &&
                                                        x.ORGANIZATION_TYPE == (byte)OrganizationType.HeaderQuarter &&
                                                        x.CASE_TYPE != (byte)CaseType.Finished &&
                                                        x.APPLY_USER_ID == userID);

                var ccNodeIDs = this.UserIdentity.Instance.JobPositions
                                                .OfType<CallCenterJobPosition>()
                                                .Select(x => x.NodeID);

                var groupIDs = _OrganizationAggregate.CallCenterNode_T1_T2_.GetListOfSpecific(
                    new MSSQLCondition<CALLCENTER_NODE>(x => ccNodeIDs.Contains(x.NODE_ID) && x.NODE_TYPE_KEY == NodeDefinitionValue.Group),
                    x => x.NODE_ID
                );

                con.And(x => groupIDs.Contains((int)x.GROUP_ID));

                var @cases = _CaseAggregate.Case_T1_.GetList(con);

                var attensionCount = @cases.Where(x => x.IS_ATTENSION).Count();

                var nonAttensionCount = @cases.Count() - attensionCount;

                var result = new JsonResult<string>($"{attensionCount}/{nonAttensionCount}", true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(CallCenterSummary_lang.CALLCENTER_SUMMARY_GET_OWN_COUNT_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(CallCenterSummary_lang.CALLCENTER_SUMMARY_GET_OWN_COUNT_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 取得全體未結案件數
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(CallCenterSummary_lang.CALLCENTER_SUMMARY_GET_ALL_COUNT))]
        public async Task<IHttpResult> GetAllCount(int? buID)
        {
            try
            {

                var ccNodeIDs = this.UserIdentity.Instance.JobPositions
                                                .OfType<CallCenterJobPosition>()
                                                .Select(x => x.NodeID);

                var groupIDs = _OrganizationAggregate.CallCenterNode_T1_T2_.GetListOfSpecific(
                                    new MSSQLCondition<CALLCENTER_NODE>(x => ccNodeIDs.Contains(x.NODE_ID) && x.NODE_TYPE_KEY == NodeDefinitionValue.Group),
                                    x => x.NODE_ID
                                );

                var con = new MSSQLCondition<CASE>(x => x.NODE_ID == buID &&
                                                        x.ORGANIZATION_TYPE == (byte)OrganizationType.HeaderQuarter &&
                                                        x.CASE_TYPE != (byte)CaseType.Finished &&
                                                        groupIDs.Contains((int)x.GROUP_ID));


                var @cases = _CaseAggregate.Case_T1_.GetList(con);
                
                var attensionCount = @cases.Where(x => x.IS_ATTENSION).Count();

                var nonAttensionCount = @cases.Count() - attensionCount;

                var result = new JsonResult<string>($"{attensionCount}/{nonAttensionCount}", true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(CallCenterSummary_lang.CALLCENTER_SUMMARY_GET_ALL_COUNT_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(CallCenterSummary_lang.CALLCENTER_SUMMARY_GET_ALL_COUNT_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 取得清單
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(CallCenterSummary_lang.CALLCENTER_SUMMARY_GETLIST))]
        public async Task<PagingResponse> GetList(PagingRequest<CallCenterSummarySearchViewModel>  model)
        {
            try
            {
                var criteria = model.criteria;
                var con = new MSSQLCondition<CASE>(
                    model.criteria,
                    model.pageIndex,
                    model.pageSize
                );


                var ccNodeIDs = this.UserIdentity.Instance.JobPositions
                                                .OfType<CallCenterJobPosition>()
                                                .Select(x => x.NodeID);

                var groupIDs = _OrganizationAggregate.CallCenterNode_T1_T2_.GetListOfSpecific(
                                    new MSSQLCondition<CALLCENTER_NODE>(x => ccNodeIDs.Contains(x.NODE_ID) && x.NODE_TYPE_KEY == NodeDefinitionValue.Group),
                                    x => x.NODE_ID
                                );

                con.IncludeBy(x => x.CASE_CONCAT_USER);
                con.IncludeBy(x => x.CASE_COMPLAINED_USER);
                con.IncludeBy(x => x.CASE_WARNING);
                con.IncludeBy(x => x.CASE_SOURCE);

                con.OrderBy(model.sort, model.orderType); 

                con.And(x => x.CASE_TYPE != (int)CaseType.Finished);
                con.And(x => groupIDs.Contains((int)x.GROUP_ID));

                if (criteria.IsSelf) {
                    con.And(x => x.APPLY_USER_ID == this.UserIdentity.Instance.UserID);
                }


                var cases = _CaseAggregate.Case_T1_T2_.GetPaging(con);

                var result = _QuestionClassificationResolver.ResolveCollection(cases).Select(x => new CallCenterSummaryListViewModel(x)).ToList();
                

                return await new PagingResponse<IEnumerable<CallCenterSummaryListViewModel>>(result)
                {
                    isSuccess = true,
                    totalCount = cases.TotalCount
                }.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(CallCenterSummary_lang.CALLCENTER_SUMMARY_GETLIST_FAIL));

                return await new PagingResponse()
                {
                    isSuccess = false,
                    message = ex.PrefixMessage(CallCenterSummary_lang.CALLCENTER_SUMMARY_GETLIST_FAIL)
                }.Async();
            }
        }

    }
}
