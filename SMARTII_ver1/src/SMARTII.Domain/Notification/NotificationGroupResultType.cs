using System.ComponentModel;

namespace SMARTII.Domain.Notification
{
    public enum NotificationGroupResultType
    {
        /// <summary>
        /// 通知完成
        /// </summary>
        [Description("通知完成")]
        Finish = 0,

        /// <summary>
        /// 不通知
        /// </summary>
        [Description("不通知")]
        NoSend = 1,

        /// <summary>
        /// 通知失敗
        /// </summary>
        [Description("通知失敗")]
        Error = 2
    }
}