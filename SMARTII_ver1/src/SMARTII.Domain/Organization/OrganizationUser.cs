using System.ComponentModel;
using SMARTII.Domain.Common;
using SMARTII.Domain.Security;

namespace SMARTII.Domain.Organization
{
    public class OrganizationUser
    {
        public string UserID { get; set; }

        [Security]
        [Description("姓名")]
        [Custom("姓名")]
        public string UserName { get; set; }

        public string NodeName { get; set; }

        public int? NodeID { get; set; }

        public string JobName { get; set; }

        public int? JobID { get; set; }

        public string StoreNo { get; set; }

        public int? BUID { get; set; }

        public string BUName { get; set; }

        public OrganizationType? OrganizationType { get; set; }

        [Description("類型")]
        public UnitType UnitType { get; set; }

        public string ParentPathName { get; set; }

        public string ParentName {
            get {
                if (string.IsNullOrEmpty(this.ParentPathName)) return string.Empty;

                var paths = this.ParentPathName.Split('/');
                
                return (paths != null && paths.Length > 0) ? paths[paths.Length - 1] : string.Empty;
            }
        }
    }
}
