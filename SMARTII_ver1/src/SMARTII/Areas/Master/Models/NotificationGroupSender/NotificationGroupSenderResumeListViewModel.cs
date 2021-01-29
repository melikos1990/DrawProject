using SMARTII.Domain.Data;

namespace SMARTII.Areas.Master.Models.NotificationGroupSender
{
    public class NotificationGroupSenderResumeListViewModel
    {
        public NotificationGroupSenderResumeListViewModel(Domain.Notification.NotificationGroupResume data)
        {
            this.BUName = data.NodeName;
            this.GroupName = data.NotificationGroup?.Name;
            this.Targets = data.Target;
            this.Content = data.Content;
            this.Type = data.NotificationGroupResultType.GetDescription();
            this.CreateDateTime = data.CreateDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.CreateUserName = data.CreateUserName;
            this.FilePath = data.EMLFilePath;
        }

        public string BUName { get; set; }

        public string GroupName { get; set; }

        public string[] Targets { get; set; }

        public string Content { get; set; }

        public string Type { get; set; }

        public string CreateDateTime { get; set; }

        public string CreateUserName { get; set; }

        public string FilePath { get; set; }
    }
}