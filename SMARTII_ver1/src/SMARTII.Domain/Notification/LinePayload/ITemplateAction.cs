namespace SMARTII.Domain.Notification.Line
{
    public interface ITemplateAction
    {
        /// <summary>
        /// 按鈕文字
        /// </summary>
        string label { set; get; }

        TemplateActionType type { get; set; }
    }
}