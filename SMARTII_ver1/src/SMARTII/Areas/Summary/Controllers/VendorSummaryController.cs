using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Areas.Summary.Models;
using SMARTII.Areas.Summary.Models.VendorSummary;
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

namespace SMARTII.Areas.Summary.Controllers
{
    [Authentication]
    public class VendorSummaryController : BaseApiController
    {
        private readonly ICommonAggregate _CommonAggregate;
        private readonly ICaseSearchFacade _CaseSearchFacade;
        private readonly IExecutiveOrganizationNodeProvider<VENDOR_NODE> _OrganizationNodeProvider;

        public VendorSummaryController(
            ICommonAggregate CommonAggregate,
            ICaseSearchFacade CaseSearchFacade,
            IExecutiveOrganizationNodeProvider<VENDOR_NODE> OrganizationNodeProvider)
        {
            _CommonAggregate = CommonAggregate;
            _CaseSearchFacade = CaseSearchFacade;
            _OrganizationNodeProvider = OrganizationNodeProvider;
        }

        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(VendorSummary_lang.VENDOR_SUMMARY_GET_PROVIDER_BUs))]
        [HttpGet]
        public async Task<IHttpResult> GetProviderBUs()
        {
            try
            {
                var providerBUs = _OrganizationNodeProvider.GetOwnDownwardProviderNodes(OrganizationType.Vendor);

                var list = providerBUs.Select(x => new SummaryTargetViewModel(x));

                var result = new JsonResult<IEnumerable<SummaryTargetViewModel>>(list, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(VendorSummary_lang.VENDOR_SUMMARY_GET_PROVIDER_BUs_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(VendorSummary_lang.VENDOR_SUMMARY_GET_PROVIDER_BUs_FAIL), false)
                    .Async();
            }
        }



        /// <summary>
        /// 未銷案件
        /// </summary>
        /// <param name="searchType"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Read)]
        [ActionName("GetUnFinishCount")]
        [Logger(nameof(VendorSummary_lang.VENDER_SUMMARY_UNFINISH_COUNT))]
        public async Task<IHttpResult> GetUnFinishCountAsync(HeaderQuarterSummarySearchType searchType)
        {
            try
            {

                var count = _CaseSearchFacade.GetVenderUnFinishCaseAssignmentCount(searchType);


                return await new JsonResult<int>(count, true).Async();
            }
            catch (Exception ex)
            {

                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(VendorSummary_lang.VENDER_SUMMARY_UNFINISH_COUNT_FAIL));


                return await new JsonResult(
                    ex.PrefixMessage(VendorSummary_lang.VENDER_SUMMARY_UNFINISH_COUNT_FAIL), false)
                    .Async();
            }
        }



        /// <summary>
        /// 取得清單(未銷案件)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(VendorSummary_lang.VENDER_SUMMARY_UNFINISH_GETLIST))]
        [ActionName("GetList")]
        public async Task<PagingResponse> GetListAsync(PagingRequest<VendorSummarySearchViewModel> model)
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
                con.IncludeBy(x => x.CASE_ASSIGNMENT_USER);
                con.IncludeBy(x => x.CASE.CASE_COMPLAINED_USER);
                con.IncludeBy(x => x.CASE.CASE_CONCAT_USER);

                con.OrderBy(model.sort, model.orderType);

                var caseAssignments = _CaseSearchFacade.GetVenderUnFinishCaseAssignment(criteria.HQHomeSearchType, con);

                var result = caseAssignments.Select(x => new VenderSummaryUnFinishListViewModel(x)).ToList();


                return await new PagingResponse<IEnumerable<VenderSummaryUnFinishListViewModel>>(result)
                {
                    isSuccess = true,
                    totalCount = caseAssignments.TotalCount
                }.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(VendorSummary_lang.VENDER_SUMMARY_UNFINISH_GETLIST_FAIL));

                return await new PagingResponse()
                {
                    isSuccess = false,
                    message = ex.PrefixMessage(VendorSummary_lang.VENDER_SUMMARY_UNFINISH_GETLIST_FAIL)
                }.Async();
            }
        }
    }
}
