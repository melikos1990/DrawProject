using System.Collections.Generic;
using System.Linq;
using SMARTII.Areas.Master.Models.NotificationGroupSender;
using SMARTII.Domain.Data;

namespace SMARTII.Areas.Master.Models.NotificationGroup
{
    public class NotificationGroupListViewModel
    {
        public NotificationGroupListViewModel()
        {
        }

        public NotificationGroupListViewModel(Domain.Notification.NotificationGroup group)
        {
            this.ID = group.ID;
            this.NodeID = group.NodeID;
            this.NodeName = group.NodeName;
            this.CalcMode = group.CalcMode.GetDescription();
            this.Name = group.Name;
            this.Targets = group.TargetNames;
            this.AlertCount = group.AlertCount;
            this.AlertCycleDay = group.AlertCycleDay;

            this.Users = group.NotificationGroupUsers
                              .Select(x => new NotificationGroupUserListViewModel(x))?
                              .ToList();
        }

        public int ID { get; set; }

        public int NodeID { get; set; }

        public string NodeName { get; set; }

        public string CalcMode { get; set; }

        public string Name { get; set; }

        public string[] Targets { get; set; }

        public int AlertCount { get; set; }

        public int AlertCycleDay { get; set; }

        public List<NotificationGroupUserListViewModel> Users { get; set; } = new List<NotificationGroupUserListViewModel>();
    }
}
