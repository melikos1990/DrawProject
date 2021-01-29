namespace SMARTII.Domain.Notification.Line
{
    public class PostbackAction : ITemplateAction
    {
        public TemplateActionType type { get; set; } = TemplateActionType.postback;

        /// <summary>
        /// 按鈕文字
        /// </summary>
        public string label { get; set; }

        /// <summary>
        /// 傳回 webhook 的文字
        /// </summary>
        public string data { get; set; }
    }
}