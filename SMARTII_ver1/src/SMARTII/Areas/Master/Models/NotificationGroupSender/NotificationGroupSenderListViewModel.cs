using SMARTII.Domain.Data;

namespace SMARTII.Areas.Master.Models.NotificationGroupSender
{
    public class NotificationGroupSenderListViewModel
    {
        public NotificationGroupSenderListViewModel()
        {
        }

        public NotificationGroupSenderListViewModel(Domain.Notification.NotificationGroup group)
        {
            this.NodeID = group.NodeID;
            this.NodeName = group.NodeName;
            this.GroupName = group.Name;
            this.CalcMode = group.CalcMode.GetDescription();
            this.Targets = group.TargetNames;
            this.ExpectCount = group.AlertCount;
            this.ActualCount = group.ActualCount;
            this.GroupID = group.ID;
        }

        public int GroupID { get; set; }

        public int NodeID { get; set; }

        public string NodeName { get; set; }

        public string GroupName { get; set; }

        public string CalcMode { get; set; }

        public string[] Targets { get; set; }

        public int ActualCount { get; set; }

        public int ExpectCount { get; set; }
    }
}
