namespace SMARTII.Domain.Notification.Line
{
    public class ContentProvider
    {
        /// <summary>
        /// 影片或圖片的 url *https* (只在 ContentProviderType.extrnal 有效)
        /// </summary>
        public string originalContentUrl { get; set; }

        /// <summary>
        /// 預覽圖片的 url *https* (只在 ContentProviderType.extrnal 有效)
        /// </summary>
        public string previewImageUrl { get; set; }

        public ContentProviderType type { get; set; }
    }
}