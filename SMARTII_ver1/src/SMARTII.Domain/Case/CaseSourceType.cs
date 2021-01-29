using System.ComponentModel;

namespace SMARTII.Domain.Case
{
    /// <summary>
    /// 案件來源(此列舉為全部顯示, 系統參數的案件來源 ID 需跟此列舉順序一樣)
    /// </summary>
    public enum CaseSourceType
    {
        /// <summary>
        /// 來信
        /// </summary>
        [Description("來信")]
        Email,

        /// <summary>
        /// 來電
        /// </summary>
        [Description("來電")]
        Phone,

        /// <summary>
        /// 其他
        /// </summary>
        [Description("其他")]
        Other ,

        /// <summary>
        /// 門市來信
        /// </summary>
        [Description("門市來信")]
        StoreEmail,
    }
}
