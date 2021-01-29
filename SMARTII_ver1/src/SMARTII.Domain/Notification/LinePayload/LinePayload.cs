using System.Collections.Generic;

namespace SMARTII.Domain.Notification.Line
{
    /// <summary>
    /// 使用於 api.line.me/v2/bot/message/push
    /// </summary>
    public class LinePayload : ISenderPayload
    {
        /// <summary>
        /// 通知的 User ID
        /// </summary>
        public List<string> to { get; set; }

        /// <summary>
        /// 訊息內容
        /// </summary>
        public List<ILineMessage> messages { get; set; }
    }
}