using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using SMARTII.Domain.Notification;

namespace SMARTII.Areas.Master.Models.PersonalNotification
{
    public class PersonalNotificationListViewModel
    {
        public PersonalNotificationListViewModel()
        {
        }
        public PersonalNotificationListViewModel(Domain.Notification.PersonalNotification data)
        {
            this.ID = data.ID;
            this.Content = data.Content;
            this.CreateDateTime  =data.CreateDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            this.PersonalNotificationType = data.PersonalNotificationType;
            this.Extend = data.GetExtend<ExpandoObject>();
        }

        /// <summary>
        /// 流水號
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 內容
        /// </summary>
        public string Content { get; set; }
        
        /// <summary>
        /// 建立時間
        /// </summary>
        public string  CreateDateTime { get; set; }


        /// <summary>
        /// 通知型態(個人)
        /// </summary>
        public PersonalNotificationType PersonalNotificationType { get; set; }
        

        /// <summary>
        /// 額外訊息
        /// </summary>
        public ExpandoObject Extend { get; set; }
        
    }
}
