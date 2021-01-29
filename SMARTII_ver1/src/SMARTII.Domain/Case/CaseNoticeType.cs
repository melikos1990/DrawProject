using System.ComponentModel;

namespace SMARTII.Domain.Case
{
    public enum CaseNoticeType
    {
        /// <summary>
        /// 認養工單
        /// </summary>
        [Description("信件分派")]
        OfficialEmail = 0,

        /// <summary>
        /// 代理人分配
        /// </summary>
        [Description("代理人分配")]
        CaseApply = 1,
    }
}
