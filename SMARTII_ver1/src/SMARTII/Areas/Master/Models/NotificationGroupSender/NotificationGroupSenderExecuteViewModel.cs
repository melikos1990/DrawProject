using SMARTII.Domain.Notification.Email;

namespace SMARTII.Areas.Master.Models.NotificationGroupSender
{
    public class NotificationGroupSenderExecuteViewModel
    {
        public int GroupID { get; set; }

        public EmailPayload Payload { get; set; }
    }
}