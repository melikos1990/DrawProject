using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using SMARTII.Domain.Notification;

namespace SMARTII.Areas.Common.Models.Notification
{
    public class NotificationBaseViewModel
    {

        /// <summary>
        /// 資料識別值
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 標題
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// 內容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 主旨
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 通知型態
        /// </summary>
        public PersonalNotificationType PersonalNotificationType { get; set; }


        /// <summary>
        /// 額外資訊(來源於 PersonalNotification Table)
        /// </summary>
        public ExpandoObject Extend { get; set; }


        /// <summary>
        /// 用於排序的時間(用途 為多個來源時可依時間排序)
        /// </summary>
        public DateTime SortDateTime { get; set; }
    }
}
