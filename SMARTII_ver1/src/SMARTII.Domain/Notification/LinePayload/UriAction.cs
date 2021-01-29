namespace SMARTII.Domain.Notification.Line
{
    public class UriAction : ITemplateAction
    {
        public TemplateActionType type { get; set; } = TemplateActionType.uri;

        /// <summary>
        /// 按鈕文字
        /// </summary>
        public string label { get; set; }

        /// <summary>
        /// 開啟的網頁的 uri
        /// </summary>
        public string uri { get; set; }
    }
}