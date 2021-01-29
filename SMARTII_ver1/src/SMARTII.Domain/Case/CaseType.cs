using System.ComponentModel;

namespace SMARTII.Domain.Case
{
    public enum CaseType
    {
        /// <summary>
        /// 立案
        /// </summary>
        [Description("立案")]
        Filling,

        /// <summary>
        /// 處理中
        /// </summary>
        [Description("處理中")]
        Process,

        /// <summary>
        /// 結案
        /// </summary>
        [Description("結案")]
        Finished
    }
}