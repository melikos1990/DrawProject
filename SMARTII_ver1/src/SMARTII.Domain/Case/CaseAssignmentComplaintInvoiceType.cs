using System.ComponentModel;

namespace SMARTII.Domain.Case
{
    public enum CaseAssignmentComplaintInvoiceType
    {
        /// <summary>
        /// 已開立
        /// </summary>
        [Description("已開立")]
        Created = 0,

        /// <summary>
        /// 已發送
        /// </summary>
        [Description("已發送")]
        Sended = 1,

        /// <summary>
        /// 已取消
        /// </summary>
        [Description("已取消")]
        Cancel = 2,
    }
}
