using System.Collections.Generic;
using SMARTII.Areas.Master.Models.Billboard;
using SMARTII.Areas.Master.Models.CaseRemind;
using SMARTII.Areas.Master.Models.PersonalNotification;

namespace SMARTII.Areas.Common.Models.Notification
{
    public class SystemNotificationViewModel
    {
        public TodoyNotification TodayNotification { get; set; }
        public BeforeNotification BeforeNotification { get; set; }
    }

    public class TodoyNotification
    {
        public List<NotificationBaseViewModel> NotificationDatas { get; set; } = new List<NotificationBaseViewModel>();
    }

    public class BeforeNotification
    {
        public List<NotificationBaseViewModel> NotificationDatas { get; set; } = new List<NotificationBaseViewModel>();
    }
}
