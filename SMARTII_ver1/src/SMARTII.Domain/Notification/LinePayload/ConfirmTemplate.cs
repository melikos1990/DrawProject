using System.Collections.Generic;

namespace SMARTII.Domain.Notification.Line
{
    public class ConfirmTemplate : ITemplate
    {
        public TemplateType type { get; set; } = TemplateType.confirm;

        /// <summary>
        /// 訊息文字
        /// </summary>
        public string text { get; set; }

        /// <summary>
        /// 點擊按鈕的Action
        /// </summary>
        public List<ITemplateAction> actions { get; set; }
    }
}