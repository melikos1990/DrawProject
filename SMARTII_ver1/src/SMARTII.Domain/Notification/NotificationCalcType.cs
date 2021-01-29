using System.ComponentModel;

namespace SMARTII.Domain.Notification
{
    public enum NotificationCalcType
    {
        /// <summary>
        /// 商品
        /// </summary>
        [Description("商品")]
        ByItem,

        /// <summary>
        /// 問題分類
        /// </summary>
        [Description("問題分類")]
        ByQuestion,

        /// <summary>
        /// 問題分類 + 商品
        /// </summary>
        [Description("問題分類+商品")]
        Both,
    }
}