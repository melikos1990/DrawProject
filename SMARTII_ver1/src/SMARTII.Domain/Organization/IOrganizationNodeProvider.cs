using System.Collections.Generic;
using SMARTII.Database.SMARTII;

namespace SMARTII.Domain.Organization
{
    public interface IExecutiveOrganizationNodeProvider<T> where T : EXECUTIVE_NODE, new()
    {
        List<IOrganizationNode> GetOwnDownwardProviderNodes(List<JobPosition> jobPositions, OrganizationType? type = null, bool? isEnabled = null);

        List<IOrganizationNode> GetOwnDownwardProviderNodes(string userID, OrganizationType? type = null, bool? isEnabled = null);

        List<IOrganizationNode> GetOwnDownwardProviderNodes(OrganizationType? type = null, bool? isEnabled = null);
    }
}
