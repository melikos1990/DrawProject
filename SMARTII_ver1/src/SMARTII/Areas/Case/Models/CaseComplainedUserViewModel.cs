
using SMARTII.Areas.Model;
using SMARTII.Domain.Case;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Case.Models
{
    public class CaseComplainedUserViewModel : ConcatableUserViewModel
    {

        public CaseComplainedUserViewModel() { }

        public CaseComplainedUserViewModel(CaseComplainedUser user)
        {
            this.Address = user.Address;
            this.BUID = user.BUID;
            this.BUName = user.BUName;
            this.CaseID = user.CaseID;
            this.Email = user.Email;
            this.Gender = user.Gender;
            this.ID = user.ID;
            this.JobID = user.JobID;
            this.JobName = user.JobName;
            this.StoreNo = user.StoreNo;
            this.Mobile = user.Mobile;
            this.NodeID = user.NodeID;
            this.NodeName = user.NodeName;
            this.OrganizationType = user.OrganizationType;
            this.ParentPathName = user.ParentPathName;
            this.Telephone = user.Telephone;
            this.TelephoneBak = user.TelephoneBak;
            this.UnitType = user.UnitType;
            this.UserID = user.UserID;
            this.UserName = user.UserName;
            this.CaseComplainedUserType = user.CaseComplainedUserType;
            this.OwnerUserName = user.OwnerUserName;
            this.OwnerUserPhone = user.OwnerUserPhone;
            this.OwnerUserEmail = user.OwnerUserEmail;
            this.OwnerJobName = user.OwnerJobName;
            this.SupervisorUserName = user.SupervisorUserName;
            this.SupervisorUserPhone = user.SupervisorUserPhone;
            this.SupervisorUserEmail = user.SupervisorUserEmail;
            this.SupervisorJobName = user.SupervisorJobName;
            this.SupervisorNodeName = user.SupervisorNodeName;
            this.UnitTypeName = user.UnitType.GetDescription();
            this.GenderTypeName = user.Gender?.GetDescription();
            this.CaseComplainedUserTypeName = user.CaseComplainedUserType.GetDescription();
            this.StoreTypeName = user.StoreTypeName;
        }

        public CaseComplainedUser ToDomain()
        {
            var user = new CaseComplainedUser();

            user.Address = this.Address;
            user.BUID = this.BUID;
            user.BUName = this.BUName;
            user.CaseID = this.CaseID;
            user.Email = this.Email;
            user.Gender = this.Gender;
            user.ID = this.ID;
            user.JobID = this.JobID;
            user.JobName = this.JobName;
            user.StoreNo = this.StoreNo;
            user.Mobile = this.Mobile;
            user.NodeID = this.NodeID;
            user.NodeName = this.NodeName;
            user.OrganizationType = this.OrganizationType;
            user.ParentPathName = this.ParentPathName;
            user.Telephone = this.Telephone;
            user.TelephoneBak = this.TelephoneBak;
            user.UnitType = this.UnitType;
            user.UserID = this.UserID;
            user.UserName = this.UserName;
            user.CaseComplainedUserType = this.CaseComplainedUserType;
            user.OwnerUserName = this.OwnerUserName;
            user.OwnerUserPhone = this.OwnerUserPhone;
            user.OwnerJobName = this.OwnerJobName;
            user.OwnerUserEmail = this.OwnerUserEmail;
            user.SupervisorUserName = this.SupervisorUserName;
            user.SupervisorUserPhone = this.SupervisorUserPhone;
            user.SupervisorUserEmail = this.SupervisorUserEmail;
            user.SupervisorJobName = this.SupervisorJobName;
            user.SupervisorNodeName = this.SupervisorNodeName;
            return user;
        }


        /// <summary>
        /// 識別直
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 案件編號
        /// </summary>
        public string CaseID { get; set; }

        /// <summary>
        /// 權責型態 (知會單位/權責單位)
        /// </summary>
        public CaseComplainedUserType CaseComplainedUserType { get; set; }

        /// <summary>
        ///  權責型態 (知會單位/權責單位) 名稱
        /// </summary>
        public string CaseComplainedUserTypeName { get; set; }
        /// <summary>
        /// 負責人姓名
        /// </summary>
        public string OwnerUserName { get; set; }
        /// <summary>
        /// 負責人電話
        /// </summary>
        public string OwnerUserPhone { get; set; }
        /// <summary>
        /// 負責人Email
        /// </summary>
        public string OwnerUserEmail { get; set; }

        /// <summary>
        /// 負責人職稱
        /// </summary>
        public string OwnerJobName { get; set; }
        /// <summary>
        /// 區經理姓名
        /// </summary>
        public string SupervisorUserName { get; set; }
        /// <summary>
        /// 區經理Email
        /// </summary>
        public string SupervisorUserEmail { get; set; }
        /// <summary>
        /// 區經理電話
        /// </summary>
        public string SupervisorUserPhone { get; set; }
        /// <summary>
        /// 區經理職稱
        /// </summary>
        public string SupervisorJobName { get; set; }
        /// <summary>
        /// 組織節點名稱
        /// </summary>
        public string SupervisorNodeName { get; set; }
        /// <summary>
        /// 門市型態名稱
        /// </summary>
        public string StoreTypeName { get; set; }
    }
}
