namespace SMARTII.Domain.Notification.Line
{
    /// <summary>
    /// MediaMessage 可當 image / video Message
    /// </summary>
    public class MediaMessage : ILineMessage
    {
        public MessageType type { get; set; }

        /// <summary>
        /// 影片播放時間 (只有在 type = MessageType.video 時 有用)
        /// </summary>
        public int duration { get; set; }

        /// <summary>
        /// 提供的檔案資訊
        /// </summary>
        public ContentProvider contentProvider { get; set; }
    }
}