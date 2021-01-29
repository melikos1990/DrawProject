using SMARTII.Areas.Model;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;

namespace SMARTII.Areas.Case.Models
{
    public class CaseAssignmentConcatUserViewModel : ConcatableUserViewModel
    {
        public CaseAssignmentConcatUserViewModel()
        {
        }

        public CaseAssignmentConcatUserViewModel(CaseAssignmentConcatUser user)
        {
            this.Address = user.Address;
            this.AssignmentID = user.AssignmentID;
            this.BUID = user.BUID;
            this.BUName = user.BUName;
            this.CaseComplainedUserType = user.CaseComplainedUserType;
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
            this.CaseComplainedUserTypeName = user.CaseComplainedUserType.GetDescription();
            this.UnitTypeName = user.UnitType.GetDescription();
            this.GenderTypeName = user.Gender?.GetDescription();
        }

        public CaseAssignmentConcatUser ToDoamin()
        {
            var user = new CaseAssignmentConcatUser();
            user.Address = this.Address;
            user.AssignmentID = this.AssignmentID;
            user.BUID = this.BUID;
            user.BUName = this.BUName;
            user.CaseComplainedUserType = this.CaseComplainedUserType;
            user.CaseID = this.CaseID;
            user.Email = this.Email;
            user.Gender = this.Gender;
            user.ID = this.ID;
            user.StoreNo = this.StoreNo;
            user.JobID = this.JobID;
            user.JobName = this.JobName;
            user.Mobile = this.Mobile;
            user.NodeID = this.NodeID;
            user.NodeName = this.NodeName;
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
        /// 識別序號
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 案件編號
        /// </summary>
        public string CaseID { get; set; }

        /// <summary>
        /// 轉派序號
        /// </summary>
        public int AssignmentID { get; set; }

        /// <summary>
        /// 權責單位/知會單位
        /// </summary>
        public CaseComplainedUserType CaseComplainedUserType { get; set; }

        /// <summary>
        /// 權責單位/知會單位 名稱
        /// </summary>
        public string CaseComplainedUserTypeName { get; set; }


    }
}
