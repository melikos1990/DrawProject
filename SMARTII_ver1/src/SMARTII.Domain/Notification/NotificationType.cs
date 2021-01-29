using System.ComponentModel;

namespace SMARTII.Domain.Notification
{
    /// <summary>
    /// 推播行為
    /// </summary>
    public enum NotificationType
    {
        /// <summary>
        /// EMAIL
        /// </summary>
        [Description("EMAIL")]
        Email,

        /// <summary>
        /// 簡訊
        /// </summary>
        [Description("簡訊")]
        SMS,

        /// <summary>
        /// 畫面通知
        /// </summary>
        [Description("畫面通知")]
        UI,

        /// <summary>
        /// 網頁推播
        /// </summary>
        [Description("網頁推播")]
        WebPush,

        /// <summary>
        /// 手機推播
        /// </summary>
        [Description("手機推播")]
        MobilePush,

        /// <summary>
        /// 社群推播
        /// </summary>
        [Description("社群推播")]
        Social,
    }
}
