namespace SMARTII.Domain.Notification.Line
{
    public class TextMessage : ILineMessage
    {
        public MessageType type { get; set; } = MessageType.text;

        /// <summary>
        /// 推播訊息
        /// </summary>
        public string text { get; set; }
    }
}