using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Report;

namespace SMARTII.Domain.Case
{
    public interface ICaseSearchService
    {
        /// <summary>
        ///  取得案件查詢清單(客服)SP
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<List<SP_GetCaseList>> GetCaseForCustomerLists(CaseCallCenterCondition condition);

        /// <summary>
        /// 案件查詢-Excel匯出 (客服)SP
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<Byte[]> ExcelCaseForCustomer(CaseCallCenterCondition condition);

        /// <summary>
        /// 取得案件查詢清單(總部、門市)SP
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<List<SP_GetCaseList>> GetCaseForHSLists(CaseHSCondition condition);

        /// <summary>
        /// 案件查詢-Excel匯出 (總部、門市)SP
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<Byte[]> ExcelCaseForHS(CaseHSCondition condition);

        /// <summary>
        /// 取得轉派案件查詢清單(客服)SP
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<List<ExcelCaseAssignmentList>> GetCaseAssignmentForCustomerLists(CaseAssignmentCallCenterCondition condition);

        /// <summary>
        /// 轉派案件查詢-Excel匯出 (客服)SP
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<Byte[]> ExcelCaseAssignmentForCustomer(CaseAssignmentCallCenterCondition condition);

        /// <summary>
        /// 取得轉派案件查詢清單(總部、門市)SP
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<List<ExcelCaseAssignmentList>> GetCaseAssignmentForHSLists(CaseAssignmentHSCondition condition, OrganizationType organizationType);

        /// <summary>
        /// 轉派案件查詢-Excel匯出 (總部、門市)SP
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<Byte[]> ExcelCaseAssignmentForHS(CaseAssignmentHSCondition condition, OrganizationType organizationType);

        /// <summary>
        /// 取得轉派案件查詢清單(廠商)SP
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<List<ExcelCaseAssignmentList>> GetCaseAssignmentForVendorLists(CaseAssignmentHSCondition condition, OrganizationType organizationType);

        /// <summary>
        /// 轉派案件查詢-Excel匯出 (廠商)SP
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<Byte[]> ExcelCaseAssignmentForVendor(CaseAssignmentHSCondition condition, OrganizationType organizationType);
    }
}
