using System.ComponentModel;

namespace SMARTII.Domain.Master
{
    public enum BillboardWarningType
    {
        /// <summary>
        /// 一般
        /// </summary>
        [Description("一般")]
        General = 0,

        /// <summary>
        /// 緊急
        /// </summary>
        [Description("重要")]
        Warning = 1
    }
}
