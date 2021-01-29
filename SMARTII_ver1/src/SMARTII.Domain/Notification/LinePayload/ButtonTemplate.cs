using System.Collections.Generic;

namespace SMARTII.Domain.Notification.Line
{
    public class ButtonTemplate : ITemplate
    {
        public TemplateType type { get; set; } = TemplateType.button;

        /// <summary>
        /// 點擊按鈕的Action
        /// </summary>
        public List<ITemplateAction> actions { get; set; }

        /// <summary>
        /// 點擊訊息的Action
        /// </summary>
        public ITemplateAction defaultAction { get; set; }

        /// <summary>
        /// 圖片的顯示比例
        /// </summary>
        public ImageAspectRatioType imageAspectRatio { get; set; }

        /// <summary>
        /// 圖片的顯示方式
        /// </summary>
        public ImageSizeType imageSize { get; set; }

        /// <summary>
        /// 圖片的底部顏色
        /// </summary>
        public string imageBackgroundColor { get; set; }

        /// <summary>
        /// 訊息文字
        /// </summary>
        public string text { get; set; }

        /// <summary>
        /// 圖片 Url *https*
        /// </summary>
        public string thumbanilImageIUrl { get; set; }

        /// <summary>
        /// 標題
        /// </summary>
        public string title { get; set; }
    }
}