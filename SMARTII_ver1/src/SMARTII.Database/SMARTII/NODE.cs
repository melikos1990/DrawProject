using System.Collections.Generic;

namespace SMARTII.Database.SMARTII
{
    public class EXECUTIVE_NODE : NODE
    {
        public ICollection<HEADQUARTERS_NODE> HEADQUARTERS_NODE { get; set; }
    }

    public class RECEIVE_NODE : NODE { }

    public class NODE
    {
        public int RIGHT_BOUNDARY { get; set; }
        public int LEFT_BOUNDARY { get; set; }
        public int NODE_ID { get; set; }
        public byte ORGANIZATION_TYPE { get; set; }
    }

    public partial class HEADQUARTERS_NODE : RECEIVE_NODE { }

    public partial class CALLCENTER_NODE : EXECUTIVE_NODE { }

    public partial class VENDOR_NODE : EXECUTIVE_NODE { }
}