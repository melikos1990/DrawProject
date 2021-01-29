using System.ComponentModel;

namespace SMARTII.Domain.Case
{
    public enum CaseRemindType
    {
        /// <summary>
        /// 一般
        /// </summary>
        [Description("一般")]
        General = 0,

        /// <summary>
        /// 緊急
        /// </summary>
        [Description("緊急")]
        Warning = 1,
    }
}