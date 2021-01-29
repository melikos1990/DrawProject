using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMARTII.Domain.Report;

namespace SMARTII.Domain.Case
{
    public interface ICaseSearchProvider
    {
        /// <summary>
        /// 案件查詢-Excel匯出 (客服)SP
        /// </summary>
        /// <returns></returns>
        Byte[] CreateCaseCustomerExcel(List<SP_GetCaseList> list, CaseCallCenterCondition condition);
        /// <summary>
        /// 案件查詢(總部、門市)
        /// </summary>
        /// <returns></returns>
        Byte[] CreateCaseHSExcel(List<SP_GetCaseList> list, CaseHSCondition condition);
        /// <summary>
        /// 案件轉派查詢(客服用)
        /// </summary>
        /// <returns></returns>
        Byte[] CreateCaseAssignmentCustomerExcel(List<ExcelCaseAssignmentList> data, CaseAssignmentCallCenterCondition condition);
        /// <summary>
        /// 案件轉派查詢(總部、門市)
        /// </summary>
        /// <returns></returns>
        Byte[] CreateCaseAssignmentHSExcel(List<ExcelCaseAssignmentList> data, CaseAssignmentHSCondition condition);
        /// <summary>
        /// 案件轉派查詢(廠商)
        /// </summary>
        /// <returns></returns>
        Byte[] CreateCaseAssignmentVendorExcel(List<ExcelCaseAssignmentList> data, CaseAssignmentHSCondition condition);



    }
}
