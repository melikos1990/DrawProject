using SMARTII.Domain.Organization;

namespace SMARTII.Models.Account
{
    public class JobPositionViewModel
    {
        private readonly JobPosition _data;

        public JobPositionViewModel(JobPosition data)
        {
            _data = data;

            this.Right = data.RightBoundary;
            this.Left = data.LeftBoundary;
            this.NodeID = data.NodeID;
            this.OrganizationType = data.OrganizationType;
            this.JobName = data.Job?.Name;
            this.NodeName = data.Node?.Name;
            this.NodeTypeKey = data.Node?.NodeTypeKey;
            this.ID = data.ID;
        }

        public int ID { get; set; }
        public int NodeID { get; set; }
        public string NodeName { get; set; }
        public string JobName { get; set; }
        public string NodeTypeKey { get; set; }
        public int Right { get; set; }
        public int Left { get; set; }


        public OrganizationType OrganizationType { get; set; }

        #region identification

        public int? BUID
        {
            get
            {
                if (_data is HeaderQuarterJobPosition)
                {
                    return ((HeaderQuarterJobPosition)_data).BUID;
                }
                else
                {
                    return null;
                }
            }
        }

        public string BUName
        {
            get
            {
                if (_data is HeaderQuarterJobPosition)
                {
                    return ((HeaderQuarterJobPosition)_data).BUName;
                }
                else
                {
                    return null;
                }
            }
        }

        public int? CallCenterID
        {
            get
            {
                if (_data is CallCenterJobPosition)
                {
                    return ((CallCenterJobPosition)_data).CallCenterID;
                }
                else
                {
                    return null;
                }
            }
        }

        public string CallCenterName
        {
            get
            {
                if (_data is CallCenterJobPosition)
                {
                    return ((CallCenterJobPosition)_data).CallCenterName;
                }
                else
                {
                    return null;
                }
            }
        }

        public int? VendorID
        {
            get
            {
                if (_data is VendorJobPosition)
                {
                    return ((VendorJobPosition)_data).VendorID;
                }
                else
                {
                    return null;
                }
            }
        }

        public string VendorName
        {
            get
            {
                if (_data is VendorJobPosition)
                {
                    return ((VendorJobPosition)_data).VendorName;
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion identification
    }
}
