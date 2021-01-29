using System;
using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Notification
{
    public class OfficialEmailHistory
    {
        public int NodeID { get; set; }

        public int EmailGroupID { get; set; }

        public string MessageID { get; set; }

        public DateTime DownloadDateTime { get; set; }

        public OrganizationType OrganizationType { get; set; }

    }
}
