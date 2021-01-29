using System.ComponentModel;

namespace SMARTII.Domain.Case
{
    public enum CaseAssignmentType
    {
        /// <summary>
        /// 已派工
        /// </summary>
        [Description("已派工")]
        Assigned,

        /// <summary>
        /// 處理完成
        /// </summary>
        [Description("處理完成")]
        Processed,

        /// <summary>
        /// 銷案
        /// </summary>
        [Description("銷案")]
        Finished,
    }
}
