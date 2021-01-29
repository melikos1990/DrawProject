using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Notification.Email;

namespace SMARTII.Areas.Common.Models.Notification
{
    public class NotificationOfficialEmailViewModel: NotificationBaseViewModel
    {

        public NotificationOfficialEmailViewModel(OfficialEmailEffectivePayload officialEmail)
        {
            this.ID = officialEmail.MessageID;
            this.Subject = officialEmail.Subject;
            this.Content = officialEmail.Body;
            this.Title = $"{officialEmail.NodeName}官網來信通知";
            this.ReceivedDateTime = officialEmail.ReceivedDateTime.ToString("yyyy-MM-dd HH:mm");
            this.NodeID = officialEmail.NodeID;
        }

        public int NodeID { get; set; }

        public string ReceivedDateTime { get; set; }

        new public PersonalNotificationType PersonalNotificationType { get; set; } = PersonalNotificationType.MailIncoming;
    }
}
