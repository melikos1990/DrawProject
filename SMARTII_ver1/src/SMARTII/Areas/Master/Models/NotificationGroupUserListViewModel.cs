using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Notification.Email;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Master.Models.NotificationGroupSender
{
    public class NotificationGroupUserListViewModel
    {
        public NotificationGroupUserListViewModel()
        {
        }

        public NotificationGroupUserListViewModel(NotificationGroupUser user)
        {
            this.Address = user.Address;
            this.BUID = user.BUID;
            this.BUName = user.BUName.DisplayWhenNull();
            this.Email = user.Email;
            this.Gender = user.Gender;
            this.JobID = user.JobID;
            this.JobName = user.JobName.DisplayWhenNull();
            this.Mobile = user.Mobile;
            this.NodeID = user.NodeID;
            this.ID = user.ID;
            this.GroupID = user.GroupID;
            this.NodeName = user.NodeName;
            this.NotificationBehavior = user.NotificationBehavior;
            this.NotificationBehaviorName = user.NotificationBehavior
                                                .GetEnumFromKeyString<NotificationType>()
                                                .GetDescription();

            this.NotificationKind = user.NotificationKind;
            this.NotificationRemark = user.NotificationRemark;

            this.NotificationRemarkName = user.NotificationRemark
                                              .GetEnumFromKeyString<EmailReceiveType>()
                                              .GetDescription();

            this.OrganizationType = user.OrganizationType;
            this.OrganizationTypeName = user.OrganizationType.HasValue ? user.OrganizationType.GetDescription() : "無";
            this.Telephone = user.Telephone;
            this.UnitType = user.UnitType;
            this.UnitTypeName = user.UnitType.GetDescription();
            this.UserID = user.UserID;
            this.UserName = user.UserName;
        }

        public NotificationGroupUser ToDomain()
        {
            var result = new NotificationGroupUser();

            result.BUID = this.BUID;
            result.Address = this.Address;
            result.BUName = this.BUName;
            result.Email = this.Email;
            result.Gender = this.Gender;
            result.GroupID = this.GroupID;
            result.ID = this.ID;
            result.JobID = this.JobID;
            result.JobName = this.JobName;
            result.Mobile = this.Mobile;
            result.NodeID = this.NodeID;
            result.NodeName = this.NodeName;
            result.NotificationBehavior = this.NotificationBehavior;
            result.NotificationKind = this.NotificationKind;
            result.NotificationRemark = this.NotificationRemark;
            result.OrganizationType = this.OrganizationType;
            result.Telephone = this.Telephone;
            result.UnitType = this.UnitType;
            result.UserID = this.UserID;
            result.UserName = this.UserName;

            return result;
        }

        public int ID { get; set; }

        public int GroupID { get; set; }

        public string BUName { get; set; }

        public string UserID { get; set; }

        public int? JobID { get; set; }

        public string JobName { get; set; }

        public string UserName { get; set; }

        public string NotificationBehavior { get; set; }

        public string NotificationBehaviorName { get; set; }

        public string NotificationKind { get; set; }

        public string NotificationRemark { get; set; }

        public string NotificationRemarkName { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public string Telephone { get; set; }

        public string Address { get; set; }

        public GenderType? Gender { get; set; }

        public string NodeName { get; set; }

        public int? NodeID { get; set; }

        public int? BUID { get; set; }

        public OrganizationType? OrganizationType { get; set; }

        public string OrganizationTypeName { get; set; }

        public UnitType UnitType { get; set; }

        public string UnitTypeName { get; set; }
    }
}
