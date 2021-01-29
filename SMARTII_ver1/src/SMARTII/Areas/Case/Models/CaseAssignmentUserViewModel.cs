using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMARTII.Areas.Model;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;

namespace SMARTII.Areas.Case.Models
{
    public class CaseAssignmentUserViewModel : OrganizationUserViewModel
    {
        public CaseAssignmentUserViewModel()
        {
        }

        public CaseAssignmentUserViewModel(CaseAssignmentUser user)
        {
            this.AssignmentID = user.AssignmentID;
            this.BUID = user.BUID;
            this.BUName = user.BUName;
            this.CaseComplainedUserType = user.CaseComplainedUserType;
            this.CaseID = user.CaseID;
            this.ID = user.ID;
            this.IsApply = user.IsApply;
            this.JobID = user.JobID;
            this.StoreNo = user.StoreNo;
            this.JobName = user.JobName;
            this.NodeID = user.NodeID;
            this.NodeName = user.NodeName;
            this.OrganizationType = user.OrganizationType;
            this.ParentPathName = user.ParentPathName;
            this.UnitType = user.UnitType;
            this.UserID = user.UserID;
            this.UserName = user.UserName;
            this.UnitTypeName = user.UnitType.GetDescription();

        }

        public CaseAssignmentUser ToDoamin()
        {
            var user = new CaseAssignmentUser();
            user.AssignmentID = this.AssignmentID;
            user.BUID = this.BUID;
            user.BUName = this.BUName;
            user.CaseComplainedUserType = this.CaseComplainedUserType;
            user.CaseID = this.CaseID;
            user.ID = this.ID;
            user.IsApply = this.IsApply;
            user.JobID = this.JobID;
            user.JobName = this.JobName;
            user.StoreNo = this.StoreNo;
            user.NodeID = this.NodeID;
            user.NodeName = this.NodeName;
            user.OrganizationType = this.OrganizationType;
            user.ParentPathName = this.ParentPathName;
            user.UnitType = this.UnitType;
            user.UserID = this.UserID;
            user.UserName = this.UserName;

            return user;
        }

        /// <summary>
        /// 識別直
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 案件代號
        /// </summary>
        public string CaseID { get; set; }

        /// <summary>
        /// 轉派序號
        /// </summary>
        public int AssignmentID { get; set; }

        /// <summary>
        /// 是否為結案單位
        /// </summary>
        public bool IsApply { get; set; }

        /// <summary>
        /// 權責單位/知會單位
        /// </summary>
        public CaseComplainedUserType CaseComplainedUserType { get; set; }
    }
}
