using System;
using System.Linq;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;

namespace SMARTII.Service.Case.Task
{
    public class TaskBase
    {
        protected readonly IOrganizationAggregate _OrganizationAggregate;

        public TaskBase(IOrganizationAggregate OrganizationAggregate)
        {
            _OrganizationAggregate = OrganizationAggregate;
        }

        protected Boolean HasGroupAuth(int groupID)
        {
            if (ContextUtility.GetUserIdentity()?.Instance == null) return false;

            bool hasAuth = ContextUtility.GetUserIdentity()
                                         .Instance
                                         .JobPositions
                                         .Any(x => x.NodeID == groupID &&
                                                   x.OrganizationType == OrganizationType.CallCenter);

            var test = ContextUtility.GetUserIdentity().Instance.JobPositions;

            return hasAuth;
        }

        protected Boolean HasGroupBU(int buID, int groupID)
        {
            bool hasBu = _OrganizationAggregate.CallCenterNode_T1_T2_.HasAny(
                x => x.NODE_ID == groupID &&
                x.HEADQUARTERS_NODE.Any(g => g.NODE_ID == buID));

            return hasBu;
        }



       
    }
}
