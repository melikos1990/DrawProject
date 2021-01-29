using SMARTII.Areas.Model;
using SMARTII.Domain.Case;
using SMARTII.Domain.Common;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Case.Models
{
    public class CaseSourceUserViewModel : ConcatableUserViewModel
    {
        public CaseSourceUserViewModel()
        {
        }

        public CaseSourceUserViewModel(CaseSourceUser user)
        {

            if (user == null) return;

            this.Address = user.Address;
            this.BUID = user.BUID;
            this.BUName = user.BUName;
            this.Email = user.Email;
            this.Gender = user.Gender;
            this.JobID = user.JobID;
            this.StoreNo = user.StoreNo;
            this.JobName = user.JobName;
            this.Mobile = user.Mobile;
            this.NodeID = user.NodeID;
            this.NodeName = user.NodeName;
            this.ParentPathName = user.ParentPathName;
            this.OrganizationType = user.OrganizationType;
            this.SourceID = user.SourceID;
            this.Telephone = user.Telephone;
            this.TelephoneBak = user.TelephoneBak;
            this.UnitType = user.UnitType;
            this.UserID = user.UserID;
            this.UserName = user.UserName;
        }

        public CaseSourceUser ToDomain()
        {
            var result = new CaseSourceUser();

            result.Address = this.Address;
            result.BUID = this.BUID;
            result.BUName = this.BUName;
            result.Email = this.Email;
            result.Gender = this.Gender;
            result.JobID = this.JobID;
            result.JobName = this.JobName;
            result.Mobile = this.Mobile;
            result.StoreNo = this.StoreNo;
            result.ParentPathName = this.ParentPathName;
            result.NodeID = this.NodeID;
            result.NodeName = this.NodeName;
            result.OrganizationType = this.OrganizationType;
            result.SourceID = this.SourceID;
            result.Telephone = this.Telephone;
            result.TelephoneBak = this.TelephoneBak;
            result.UnitType = this.UnitType;
            result.UserID = this.UserID;
            result.UserName = this.UserName;

            return result;
        }

        /// <summary>
        /// 來源編號
        /// </summary>
        public string SourceID { get; set; }


    }
}
