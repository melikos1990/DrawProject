using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Areas.Summary.Models.HeaderQuarterSummary;
using SMARTII.Assist.Authentication;
using SMARTII.Assist.Logger;
using SMARTII.Assist.Web;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Case;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;
using SMARTII.Service.Master.Resolver;

namespace SMARTII.Areas.Summary.Controllers
{
    [Authentication]
    public class HeaderQuarterSummaryController : BaseApiController
    {
        private readonly ICaseAggregate _CaseAggregate;
        private readonly ICommonAggregate _CommonAggregate;
        private readonly ICaseSearchFacade _CaseSearchFacade;
        private readonly QuestionClassificationResolver _QuestionClassificationResolver;

        public HeaderQuarterSummaryController(
            ICaseAggregate CaseAggregate, 
            ICommonAggregate CommonAggregate, 
            ICaseSearchFacade CaseSearchFacade,
            QuestionClassificationResolver QuestionClassificationResolver)
        {
            _CaseAggregate = CaseAggregate;
            _CommonAggregate = CommonAggregate;
            _CaseSearchFacade = CaseSearchFacade;
            _QuestionClassificationResolver = QuestionClassificationResolver;
        }


        /// <summary>
        /// 未銷案件
        /// </summary>
        /// <param name="searchType"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Read)]
        [ActionName("GetUnFinishCount")]
        [Logger(nameof(HeaderQuarterSummary_lang.HEADQUARTER_SUMMARY_UNFINISH_COUNT))]
        public async Task<IHttpResult> GetUnFinishCountAsync(HeaderQuarterSummarySearchType searchType)
        {
            try
            {

                var count = _CaseSearchFacade.GetHeadquarterUnFinishCaseAssignmentCount(searchType);
                    

                return await new JsonResult<int>(count, true).Async();
            }
            catch (Exception ex)
            {

                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(HeaderQuarterSummary_lang.HEADQUARTER_SUMMARY_UNFINISH_COUNT_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(HeaderQuarterSummary_lang.HEADQUARTER_SUMMARY_UNFINISH_COUNT_FAIL), false)
                    .Async();
            }
        }



        /// <summary>
        /// 未結案件
        /// </summary>
        /// <param name="searchType"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Read)]
        [ActionName("GetUnCloseCount")]
        [Logger(nameof(HeaderQuarterSummary_lang.HEADQUARTER_SUMMARY_UNCLOSE_COUNT))]
        public async Task<IHttpResult> GetUnCloseCountAsync()
        {
            try
            {
                var con = new MSSQLCondition<CASE>();
                var user = this.UserIdentity.Instance;

                con.ComplainedSelfFromPosition<HeaderQuarterJobPosition>(user.JobPositions);
                
                con.And(x => x.CASE_TYPE != (int)CaseType.Finished);

                var count = _CaseAggregate.Case_T1_T2_.Count(con);


                return await new JsonResult<int>(count, true).Async();
            }
            catch (Exception ex)
            {

                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(HeaderQuarterSummary_lang.HEADQUARTER_SUMMARY_UNCLOSE_COUNT));

                return await new JsonResult(
                    ex.PrefixMessage(HeaderQuarterSummary_lang.HEADQUARTER_SUMMARY_UNCLOSE_COUNT), false)
                    .Async();
            }
        }


        /// <summary>
        /// 取得清單(未銷案件)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(HeaderQuarterSummary_lang.HEADQUARTER_SUMMARY_UNFINISH_GETLIST))]
        [ActionName("GetUnFinishList")]
        public async Task<PagingResponse> GetUnFinishListAsync(PagingRequest<HeaderQuarterSummarySearchViewModel> model)
        {
            try
            {
                var criteria = model.criteria;

                var con = new MSSQLCondition<CASE_ASSIGNMENT>(
                    model.criteria,
                    model.pageIndex,
                    model.pageSize
                );

                con.IncludeBy(x => x.CASE);
                con.IncludeBy(x => x.CASE.CASE_WARNING);
                con.IncludeBy(x => x.CASE_ASSIGNMENT_USER);
                con.IncludeBy(x => x.CASE.CASE_COMPLAINED_USER);
                con.IncludeBy(x => x.CASE.CASE_CONCAT_USER);
                con.IncludeBy(x => x.CASE.CASE_SOURCE);

                con.OrderBy(model.sort, model.orderType);
                
                var caseAssignments = _CaseSearchFacade.GetHeadquarterUnFinishCaseAssignment(criteria.HQHomeSearchType, con);

                caseAssignments.ToList().ForEach(x =>
                {
                    x.Case = _QuestionClassificationResolver.Resolve(x.Case);
                }); 

                var result = caseAssignments.Select(x => new HeaderQuarterSummaryUnFinishListViewModel(x)).ToList();


                return await new PagingResponse<IEnumerable<HeaderQuarterSummaryUnFinishListViewModel>>(result)
                {
                    isSuccess = true,
                    totalCount = caseAssignments.TotalCount
                }.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(HeaderQuarterSummary_lang.HEADQUARTER_SUMMARY_UNFINISH_GETLIST_FAIL));

                return await new PagingResponse()
                {
                    isSuccess = false,
                    message = ex.PrefixMessage(HeaderQuarterSummary_lang.HEADQUARTER_SUMMARY_UNFINISH_GETLIST_FAIL)
                }.Async();
            }
        }



        /// <summary>
        /// 取得清單(未結案件)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(HeaderQuarterSummary_lang.HEADQUARTER_SUMMARY_UNCLOSE_GETLIST))]
        [ActionName("GetUnCloseList")]
        public async Task<PagingResponse> GetUnCloseListAsync(PagingRequest model)
        {
            try
            {
                var user = this.UserIdentity.Instance;

                var con = new MSSQLCondition<CASE>(
                    model.pageIndex,
                    model.pageSize
                );

                con.IncludeBy(x => x.CASE_CONCAT_USER);
                con.IncludeBy(x => x.CASE_COMPLAINED_USER);
                con.IncludeBy(x => x.CASE_WARNING);
                con.IncludeBy(x => x.CASE_SOURCE);

                con.OrderBy(model.sort, model.orderType);

                con.ComplainedSelfFromPosition<HeaderQuarterJobPosition>(user.JobPositions);

                con.And(x => x.CASE_TYPE != (int)CaseType.Finished);

                var @cases = _CaseAggregate.Case_T1_T2_.GetPaging(con);

                var result = @cases.Select(x => new HeaderQuarterSummaryUnCloseListViewModel(x)).ToList();


                return await new PagingResponse<IEnumerable<HeaderQuarterSummaryUnCloseListViewModel>>(result)
                {
                    isSuccess = true,
                    totalCount = @cases.TotalCount
                }.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(HeaderQuarterSummary_lang.HEADQUARTER_SUMMARY_UNCLOSE_GETLIST));

                return await new PagingResponse()
                {
                    isSuccess = false,
                    message = ex.PrefixMessage(HeaderQuarterSummary_lang.HEADQUARTER_SUMMARY_UNCLOSE_GETLIST)
                }.Async();
            }
        }
        
    }
}
