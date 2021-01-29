using System.ComponentModel;

namespace SMARTII.Domain.Common
{
    public enum GenderType
    {
        /// <summary>
        /// 小姐
        /// </summary>
        [Description("小姐")]
        Female,

        /// <summary>
        /// 先生
        /// </summary>
        [Description("先生")]
        Male,

        /// <summary>
        /// 其他
        /// </summary>
        [Description("其他")]
        Other
    }
}
