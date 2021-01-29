using Ptc.Data.Condition2.Mssql.Attribute;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Model
{
    public class OrganizationUserViewModel
    {
        public OrganizationUserViewModel()
        {
        }

        public string UserID { get; set; }
        
        public string UserName { get; set; }

        public string NodeName { get; set; }

        public int? NodeID { get; set; }

        public string JobName { get; set; }

        public int? JobID { get; set; }

        public string StoreNo { get; set; }

        public int? BUID { get; set; }

        public string BUName { get; set; }

        public OrganizationType? OrganizationType { get; set; }

        public UnitType UnitType { get; set; }

        public string UnitTypeName { get; set; }

        public string ParentPathName { get; set; }
    }
}
