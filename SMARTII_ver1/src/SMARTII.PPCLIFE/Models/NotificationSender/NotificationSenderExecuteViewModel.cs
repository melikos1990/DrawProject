using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMARTII.Domain.Notification.Email;

namespace SMARTII.PPCLIFE.Models.NotificationSender
{
    public class NotificationSenderExecuteViewModel
    {
        /// <summary>
        /// 流水編號      
        /// </summary>
        public int EffectiveID { get; set; }

        public EmailPayload Payload { get; set; }
    }
}
