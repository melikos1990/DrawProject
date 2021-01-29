namespace SMARTII.Domain.Organization
{

    public interface IOrganizationalTerm { }

    public struct OrganizationalTerm : IOrganizationalTerm
    {
        public OrganizationalTerm(int nodeID, OrganizationType organizationType)
        {
            this.NodeID = nodeID;
            this.OrganizationType = organizationType;
        }

        public int NodeID { get; set; }

        public OrganizationType OrganizationType { get; set; }
    }

    public struct HeaderQuarterTerm : IOrganizationalTerm
    {
        public HeaderQuarterTerm(int nodeID, OrganizationType organizationType, string nodeKey)
        {
            this.NodeID = nodeID;
            this.NodeKey = nodeKey;
            this.OrganizationType = organizationType;
        }

        public int NodeID { get; set; }

        public OrganizationType OrganizationType { get; set; }

        public string NodeKey { get; set; }
    }
}
