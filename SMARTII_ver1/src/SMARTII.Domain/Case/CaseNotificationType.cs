using System.ComponentModel;

namespace SMARTII.Domain.Case
{
    public enum CaseNotificationType
    {
        /// <summary>
        /// 交班/轉派通知
        /// </summary>
        [Description("交班/轉派通知")]
        Assign,

        /// <summary>
        /// 異動通知
        /// </summary>
        [Description("異動通知")]
        OnChange,
    }
}