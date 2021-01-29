namespace SMARTII.Domain.Organization
{
    public interface IOrganizationProcessStrategy
    {
        void WhenNodeUpdate(IOrganizationNode node);
    }
}