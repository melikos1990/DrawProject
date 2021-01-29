using SMARTII.Areas.Model;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;

namespace SMARTII.Areas.Case.Models
{
    public class CaseAssignmentNoticeUserViewModel : ConcatableUserViewModel
    {
        public CaseAssignmentNoticeUserViewModel()
        {
        }

        public CaseAssignmentNoticeUserViewModel(CaseAssignmentComplaintNoticeUser user)
        {
            this.Address = user.Address;
            this.BUID = user.BUID;
            this.BUName = user.BUName;
            this.CaseID = user.CaseID;
            this.Email = user.Email;
            this.Gender = user.Gender;
            this.ID = user.ID;
            this.JobID = user.JobID;
            this.StoreNo = user.StoreNo;
            this.JobName = user.JobName;
            this.Mobile = user.Mobile;
            this.NodeID = user.NodeID;
            this.NodeName = user.NodeName;
            this.NoticeID = user.NoticeID;
            this.NotificationBehavior = user.NotificationBehavior;
            this.NotificationKind = user.NotificationKind;
            this.NotificationRemark = user.NotificationRemark;
            this.OrganizationType = user.OrganizationType;
            this.ParentPathName = user.ParentPathName;
            this.Telephone = user.Telephone;
            this.TelephoneBak = user.TelephoneBak;
            this.UnitType = user.UnitType;
            this.UserID = user.UserID;
            this.UserName = user.UserName;
            this.UnitTypeName = user.UnitType.GetDescription();
            this.GenderTypeName = user.Gender?.GetDescription();
        }

        public CaseAssignmentComplaintNoticeUser ToDomain()
        {
            var user = new CaseAssignmentComplaintNoticeUser();
            user.Address = this.Address;
            user.BUID = this.BUID;
            user.BUName = this.BUName;
            user.CaseID = this.CaseID;
            user.Email = this.Email;
            user.Gender = this.Gender;
            user.ID = this.ID;
            user.JobID = this.JobID;
            user.JobName = this.JobName;
            user.Mobile = this.Mobile;
            user.StoreNo = this.StoreNo;
            user.NodeID = this.NodeID;
            user.NodeName = this.NodeName;
            user.NoticeID = this.NoticeID;
            user.NotificationBehavior = this.NotificationBehavior;
            user.NotificationKind = this.NotificationKind;
            user.NotificationRemark = this.NotificationRemark;
            user.OrganizationType = this.OrganizationType;
            user.ParentPathName = this.ParentPathName;
            user.Telephone = this.Telephone;
            user.TelephoneBak = this.TelephoneBak;
            user.UnitType = this.UnitType;
            user.UserID = this.UserID;
            user.UserName = this.UserName;

            return user;
        }

        /// <summary>
        /// 識別編號
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 通知序號
        /// </summary>
        public int NoticeID { get; set; }

        /// <summary>
        /// 案件代號
        /// </summary>
        public string CaseID { get; set; }
    }
}
