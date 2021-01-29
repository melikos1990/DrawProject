using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Notification.Email;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Master.Models.CaseAssignGroup
{
    public class CaseAssignGroupUserListViewModel
    {
        public CaseAssignGroupUserListViewModel()
        {
        }

        public CaseAssignGroupUserListViewModel(Domain.Case.CaseAssignGroupUser caseAssignGroupUser)
        {
            this.Address = caseAssignGroupUser.Address;
            this.BUID = caseAssignGroupUser.BUID;
            this.BUName = caseAssignGroupUser.BUName.DisplayWhenNull(); ;
            this.Email = caseAssignGroupUser.Email;
            this.ID = caseAssignGroupUser.ID;
            this.JobID = caseAssignGroupUser.JobID;
            this.JobName = caseAssignGroupUser.JobName.DisplayWhenNull(); ;
            this.Mobile = caseAssignGroupUser.Mobile;
            this.NodeID = caseAssignGroupUser.NodeID;
            this.NodeName = caseAssignGroupUser.NodeName;
            this.NotificationBehavior = caseAssignGroupUser.NotificationBehavior;
            this.NotificationBehaviorName = caseAssignGroupUser.NotificationBehavior
                                    .GetEnumFromKeyString<NotificationType>()
                                    .GetDescription();
            this.NotificationRemark = caseAssignGroupUser.NotificationRemark;
            this.NotificationRemarkName = caseAssignGroupUser.NotificationRemark
                                  .GetEnumFromKeyString<EmailReceiveType>()
                                  .GetDescription();
            this.NotificationKind = caseAssignGroupUser.NotificationKind;
            this.OrganizationType = caseAssignGroupUser.OrganizationType;
            this.OrganizationTypeName = caseAssignGroupUser.OrganizationType.HasValue ? caseAssignGroupUser.OrganizationType.GetDescription() : "無";
            this.Telephone = caseAssignGroupUser.Telephone;
            this.UserID = caseAssignGroupUser.UserID;
            this.UserName = caseAssignGroupUser.UserName;
            this.GroupID = caseAssignGroupUser.GroupID;
            this.UnitType = caseAssignGroupUser.UnitType;
            this.UnitTypeName = caseAssignGroupUser.UnitType.GetDescription();
            this.Gender = caseAssignGroupUser.Gender;
            this.Account = caseAssignGroupUser.User?.Account;
        }

        public Domain.Case.CaseAssignGroupUser ToDomain()
        {
            var result = new Domain.Case.CaseAssignGroupUser();

            result.BUID = this.BUID;
            result.Address = this.Address;
            result.BUName = this.BUName;
            result.Email = this.Email;
            result.Gender = this.Gender;
            result.GroupID = this.GroupID;
            result.ID = this.ID ?? 0;
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

        public string Address { get; set; }
        public int? BUID { get; set; }
        public string BUName { get; set; }
        public string Email { get; set; }
        public int? ID { get; set; }
        public int? JobID { get; set; }
        public string JobName { get; set; }
        public string Mobile { get; set; }
        public int? NodeID { get; set; }
        public string NodeName { get; set; }
        public string NotificationBehavior { get; set; }
        public string NotificationBehaviorName { get; set; }
        public string NotificationRemark { get; set; }
        public string NotificationRemarkName { get; set; }
        public string NotificationKind { get; set; }
        public OrganizationType? OrganizationType { get; set; }
        public string OrganizationTypeName { get; set; }
        public string Telephone { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public int GroupID { get; set; }
        public UnitType UnitType { get; set; }
        public string UnitTypeName { get; set; }
        public GenderType? Gender { get; set; }
        public string Account { get; set; }
    }
}
