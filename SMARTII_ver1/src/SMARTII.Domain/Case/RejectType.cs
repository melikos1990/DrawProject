using System.ComponentModel;

namespace SMARTII.Domain.Case
{
    public enum RejectType
    {
        /// <summary>
        /// 無駁回
        /// </summary>
        [Description("無駁回")]
        None = 0,

        /// <summary>
        /// 資料重填
        /// </summary>
        [Description("資料重填")]
        FillContent = 1,

        /// <summary>
        /// 重新處理
        /// </summary>
        [Description("重新處理")]
        Undo = 2,
    }
}
