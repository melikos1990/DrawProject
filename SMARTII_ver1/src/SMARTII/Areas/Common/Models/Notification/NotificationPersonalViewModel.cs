using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using SMARTII.Domain.Notification;

namespace SMARTII.Areas.Common.Models.Notification
{
    public class NotificationPersonalViewModel: NotificationBaseViewModel
    {

        public NotificationPersonalViewModel(PersonalNotification model)
        {
            this.ID = model.ID.ToString();
            this.Content = model.Content;
            this.CreateDateTime = model.CreateDateTime.ToString("yyyy-MM-dd HH:mm");
            this.SortDateTime = model.CreateDateTime;
            this.PersonalNotificationType = model.PersonalNotificationType;
            this.Extend = model.GetExtend<ExpandoObject>();
        }
        
        /// <summary>
        /// 建立時間
        /// </summary>
        public string CreateDateTime { get; set; }
        
        
    }
}
