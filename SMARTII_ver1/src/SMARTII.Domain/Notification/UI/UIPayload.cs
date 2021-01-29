namespace SMARTII.Domain.Notification.UI
{

    public class UIPayload<T> : UIPayload
    {
        public UIPayload(T _data){ this.data = _data; }

        public T data { get; set; }
    }

    public class UIPayload : ISenderPayload
    {
        /// <summary>
        /// Signalr ID
        /// </summary>
        public string ConnectionId { get; set; }

        /// <summary>
        /// 通知內容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 顯示於畫面的位置
        /// </summary>
        public string Kind { get; set; }

        /// <summary>
        /// 標題
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// Clinet監聽的方法
        /// </summary>
        public string ClientMethod { get; set; }

    }
}
