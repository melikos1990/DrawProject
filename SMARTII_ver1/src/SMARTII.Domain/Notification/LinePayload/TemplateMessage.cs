namespace SMARTII.Domain.Notification.Line
{
    public class TemplateMessage : ILineMessage
    {
        public MessageType type { get; set; } = MessageType.template;

        /// <summary>
        /// 提示訊息的文字
        /// </summary>
        public string altText { get; set; }

        /// <summary>
        /// Template樣板
        /// </summary>
        public ITemplate template { get; set; }
    }
}