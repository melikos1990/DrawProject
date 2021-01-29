using System.ComponentModel;

namespace SMARTII.Domain.Case
{
    public enum CaseAssignmentProcessType
    {
        /// <summary>
        /// 一般通知
        /// </summary>
        [Description("一般通知")]
        Notice = 0,

        /// <summary>
        /// 開立反應單
        /// </summary>
        [Description("反應單")]
        Invoice = 1,

        /// <summary>
        /// 派工
        /// </summary>
        [Description("派工")]
        Assignment = 2,

        /// <summary>
        /// 聯絡紀錄
        /// </summary>
        [Description("聯絡紀錄")]
        Communication = 3
    }
}
