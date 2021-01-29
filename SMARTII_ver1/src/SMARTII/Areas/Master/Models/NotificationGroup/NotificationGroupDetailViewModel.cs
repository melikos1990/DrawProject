using System.Collections.Generic;
using System.Linq;
using SMARTII.Areas.Master.Models.NotificationGroupSender;
using SMARTII.Domain.Data;
using SMARTII.Domain.Notification;

namespace SMARTII.Areas.Master.Models.NotificationGroup
{
    public class NotificationGroupDetailViewModel
    {
        public NotificationGroupDetailViewModel()
        {
        }

        public NotificationGroupDetailViewModel(Domain.Notification.NotificationGroup group)
        {
            this.ID = group.ID;
            this.Name = group.Name;
            this.NodeID = group.NodeID;
            this.AlertCycleDay = group.AlertCycleDay;
            this.AlertCount = group.AlertCount;
            this.CalcMode = group.CalcMode;
            this.QuestionClassificationID = group.QuestionClassificationID;
            this.QuestionClassificationName = group.QuestionClassificationName;
            this.ItemID = group.ItemID;
            this.ItemName = group.ItemName;
            this.CreateDateTime = group.CreateDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.CreateUserName = group.CreateUserName;
            this.UpdateDateTime = group.UpdateDateTime.DisplayWhenNull();
            this.UpdateUserName = group.UpdateUserName;

            this.Users = group.NotificationGroupUsers
                              .Select(x => new NotificationGroupUserListViewModel(x))
                              .ToList();

            this.QuestionClassificationGroups = Enumerable.Zip(
                group?.QuestionClassificationParentPathByArray ?? new string[] { },
                group?.QuestionClassificationParentNamesByArray ?? new string[] { },
                (id, text) =>
                                                {
                                                    return new SelectItem()
                                                    {
                                                        id = id,
                                                        text = text
                                                    };
                                                }).ToList();
        }

        public Domain.Notification.NotificationGroup ToDomain()
        {
            var result = new Domain.Notification.NotificationGroup();

            result.ID = this.ID;
            result.AlertCount = this.AlertCount;
            result.AlertCycleDay = this.AlertCycleDay;
            result.Name = this.Name;
            result.NodeID = this.NodeID;
            result.CalcMode = this.CalcMode;
            result.QuestionClassificationID = this.QuestionClassificationID;
            result.ItemID = this.ItemID;
            result.NotificationGroupUsers = this.Users?
                                                .Select(x => x.ToDomain())
                                                .ToList();

            return result;
        }

        public int ID { get; set; }

        public int NodeID { get; set; }

        public string Name { get; set; }

        public int AlertCycleDay { get; set; }

        public int AlertCount { get; set; }

        public NotificationCalcType CalcMode { get; set; }

        public int? QuestionClassificationID { get; set; }

        public string QuestionClassificationName { get; set; }

        public List<SelectItem> QuestionClassificationGroups { get; set; } = new List<SelectItem>();

        public List<NotificationGroupUserListViewModel> Users { get; set; } = new List<NotificationGroupUserListViewModel>();

        public int? ItemID { get; set; }

        public string ItemName { get; set; }

        public string CreateUserName { get; set; }

        public string CreateDateTime { get; set; }

        public string UpdateUserName { get; set; }

        public string UpdateDateTime { get; set; }
    }
}
