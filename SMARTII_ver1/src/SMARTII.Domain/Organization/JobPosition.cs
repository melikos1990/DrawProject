using System.Collections.Generic;
using Newtonsoft.Json;

namespace SMARTII.Domain.Organization
{
    public class HeaderQuarterJobPosition : JobPosition
    {
        /// <summary>
        /// 該節點所屬的企業別代號
        /// </summary>
        [JsonIgnore]
        public int? BUID
        {
            get
            {
                return OrganizationType == OrganizationType.HeaderQuarter ? IdentificationID : null;
            }
        }

        /// <summary>
        /// 該節點所屬的企業別名稱
        /// </summary>
        [JsonIgnore]
        public string BUName
        {
            get
            {
                if (OrganizationType == OrganizationType.HeaderQuarter && this.Node?.GetType() == typeof(HeaderQuarterNode))
                {
                    return ((HeaderQuarterNode)this.Node).BusinessParent?.Name;
                }
                else
                {
                    return null;
                }
            }
        }
    }

    public class VendorJobPosition : JobPosition
    {
        /// <summary>
        /// 該節點所屬的廠商代號
        /// </summary>
        [JsonIgnore]
        public int? VendorID
        {
            get
            {
                return OrganizationType == OrganizationType.Vendor ? IdentificationID : null;
            }
        }

        /// <summary>
        /// 該節點所屬的廠商名稱
        /// </summary>
        [JsonIgnore]
        public string VendorName
        {
            get
            {
                if (OrganizationType == OrganizationType.Vendor && this.Node?.GetType() == typeof(VendorNode))
                {
                    return ((VendorNode)this.Node).VendorParent?.Name;
                }
                else
                {
                    return null;
                }
            }
        }
    }

    public class CallCenterJobPosition : JobPosition
    {
        /// <summary>
        /// 該節點所屬的客服中心代號
        /// </summary>
        [JsonIgnore]
        public int? CallCenterID
        {
            get
            {
                return OrganizationType == OrganizationType.CallCenter ? IdentificationID : null;
            }
        }

        /// <summary>
        /// 該節點所屬的客服中心名稱
        /// </summary>
        [JsonIgnore]
        public string CallCenterName
        {
            get
            {
                if (OrganizationType == OrganizationType.CallCenter && this.Node?.GetType() == typeof(CallCenterNode))
                {
                    return ((CallCenterNode)this.Node).CallCenterParent.Name;
                }
                else
                {
                    return null;
                }
            }
        }
    }

    public class JobPosition : IOrganizationRelationship
    {
        public int ID { get; set; }
        public int JobID { get; set; }
      
        public int RightBoundary { get; set; }
        public int LeftBoundary { get; set; }
     
        public int? IdentificationID { get; set; }
        public List<User> Users { get; set; }
        public Job Job { get; set; }

    

        #region impl

        public OrganizationType OrganizationType { get; set; }
        public int NodeID { get; set; }
        public string NodeName { get; set; }
        public IOrganizationNode Node { get; set; }

        #endregion
    
    }
}
