using System.Threading.Tasks;
using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Case
{
    public interface ICaseAssignmentService
    {

        /// <summary>
        /// 更新轉派 (從轉派)
        /// </summary>
        /// <param name="assignment"></param>
        /// <returns></returns>
        CaseAssignment Update(CaseAssignment assignment , JobPosition jobPosition = null);
        /// <summary>
        /// 更新轉派 (從案件)
        /// </summary>
        /// <param name="assignment"></param>
        /// <returns></returns>
        CaseAssignment UpdateComplete(CaseAssignment assignment);
        /// <summary>
        /// 更新反應單
        /// </summary>
        /// <param name="invoice"></param>
        /// <returns></returns>
        CaseAssignmentComplaintInvoice UpdateInvoice(CaseAssignmentComplaintInvoice invoice);

        /// <summary>
        /// 更新一般通知
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        CaseAssignmentComplaintNotice UpdateNotice(CaseAssignmentComplaintNotice notice);

    }
}
