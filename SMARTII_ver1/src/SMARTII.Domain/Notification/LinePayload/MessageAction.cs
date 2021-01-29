namespace SMARTII.Domain.Notification.Line
{
    public class MessageAction : ITemplateAction
    {
        public TemplateActionType type { get; set; } = TemplateActionType.message;

        /// <summary>
        /// 按鈕文字
        /// </summary>
        public string label { get; set; }

        /// <summary>
        /// 顯示於畫面上的文字(觸發時)
        /// </summary>
        public string text { get; set; }
    }
}