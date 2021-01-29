using System;
using System.Collections.Generic;
using System.Linq;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Substitute.Models.CaseNotice
{
    public class CaseNoticeListViewModel
    {
        public CaseNoticeListViewModel()
        {
        }

        public CaseNoticeListViewModel(Domain.Case.CaseNotice data)
        {
            this.ID = data.ID;
            this.NodeID = data.NodeID;
            this.NodeName = data.NodeName;
            this.CaseID = data.CaseID;
            this.CreateDateTime = data.@case.CreateDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            this.CaseType = data.@case.CaseType;
            this.CaseContent = data.@case.Content;
            this.CaseWarningTypeName = data.@case.CaseWarning.Name;
            this.CaseTypeName = data.@case.CaseType.GetDescription();
            this.UnitType = data.@case.CaseSource.CaseSourceUser.UnitType;
            this.UnitTypeName = data.@case.CaseSource.CaseSourceUser.UnitType.GetDescription();
            this.CreateUserName = data.CreateUserName;
            this.ContactUserNeme = getContactUserNeme(data.@case.CaseConcatUsers.FirstOrDefault());       
            this.ApplyUserName = data.@case.ApplyUserName;
            this.ApplyDateTime = data.@case.ApplyDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            this.PromiseDateTime = data.@case.PromiseDateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private string getContactUserNeme(CaseConcatUser caseConcatUsers)
        {
            switch (caseConcatUsers.UnitType)
            {
                case UnitType.Customer:
                    return caseConcatUsers.UserName + caseConcatUsers.Gender.GetDescription();
                case UnitType.Store:
                    return caseConcatUsers.NodeName + caseConcatUsers.StoreNo;
                case UnitType.Organization:
                    return caseConcatUsers.NodeName;
            }

            return "";
        }

        public int ID { get; set; }

        public int? NodeID { get; set; }

        public string NodeName { get; set; }

        public string CaseID { get; set; }

        public string CreateDateTime { get; set; }

        public CaseType CaseType { get; set; }

        public string CaseTypeName { get; set; }

        public string CaseWarningTypeName { get; set; }

        public UnitType UnitType { get; set; }

        public string UnitTypeName { get; set; }

        public string CreateUserName { get; set; }

        public string ContactUserNeme { get; set; }

        public string ApplyUserName { get; set; }

        public string ApplyDateTime { get; set; }

        public string PromiseDateTime { get; set; }

        public string CaseContent { get; set; }
    }
}
