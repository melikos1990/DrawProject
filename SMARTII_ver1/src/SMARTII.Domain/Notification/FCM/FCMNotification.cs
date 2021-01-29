namespace SMARTII.Domain.Notification.FCM
{
    public class FCMNotification
    {
        /// <summary>
        /// 推播顯示 icon
        /// </summary>
        public string badge { get; set; }

        /// <summary>
        /// 內容
        /// </summary>
        public string body { get; set; }

        /// <summary>
        /// 使用者點擊時的 url * https *
        /// </summary>
        public string click_action { get; set; }

        /// <summary>
        /// 標題
        /// </summary>
        public string title { get; set; }
    }
}