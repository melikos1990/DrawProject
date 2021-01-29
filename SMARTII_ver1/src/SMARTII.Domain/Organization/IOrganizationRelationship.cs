namespace SMARTII.Domain.Organization
{
    public interface IOrganizationRelationship
    {
        int NodeID { get; set; }

        string NodeName { get; set; }

        OrganizationType OrganizationType { get; set; }

        IOrganizationNode Node { get; set; }
    }
}