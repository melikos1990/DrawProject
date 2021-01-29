using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using Ptc.Data.Condition2.Mssql.Class;
using Ptc.Data.Condition2.Mssql.Repository;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;

namespace SMARTII.Service.Organization.Provider
{
    public class ExecutiveOrganizationNodeProvider<T> : IExecutiveOrganizationNodeProvider<T> where T : EXECUTIVE_NODE, new()
    {
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly IMSSQLRepository<T, IExecutiveOrganizationNode> _OrganizationRepo;

        public ExecutiveOrganizationNodeProvider(
            IOrganizationAggregate OrganizationAggregate,
            IMSSQLRepository<T, IExecutiveOrganizationNode> OrganizationRepo)
        {
            _OrganizationRepo = OrganizationRepo;
            _OrganizationAggregate = OrganizationAggregate;
        }

        public List<IOrganizationNode> GetOwnDownwardProviderNodes(List<JobPosition> jobPositions, OrganizationType? type = null, bool? isEnabled = null)
        {
            var con = new MSSQLCondition<T>();
            con.IncludeBy(x => x.HEADQUARTERS_NODE);

            var target = (type.HasValue) ? jobPositions.Where(x => x.OrganizationType == type).ToList() :
                                           jobPositions;

            if (target.Count == 0) return null;

            con.FilterNodeFromPosition(target);

            var result = _OrganizationRepo.GetList(con).SelectMany(x => x.HeaderQuarterNodes);

            if (isEnabled.HasValue) result = result.Where(x => x.IsEnabled == isEnabled.Value).ToList();

            return result.DistinctBy(x => x.NodeID)
                         .Cast<IOrganizationNode>()
                         .ToList();
        }

        public List<IOrganizationNode> GetOwnDownwardProviderNodes(string userID, OrganizationType? type = null, bool? isEnabled = null)
        {
            var con = new MSSQLCondition<USER>();
            con.IncludeBy(x => x.NODE_JOB);

            var user = _OrganizationAggregate.User_T1_T2_.Get(con);

            return GetOwnDownwardProviderNodes(user.JobPositions, type, isEnabled);
        }

        public List<IOrganizationNode> GetOwnDownwardProviderNodes(OrganizationType? type = null, bool? isEnabled = null)
        {
            var user = ContextUtility.GetUserIdentity().Instance;

            return GetOwnDownwardProviderNodes(user.JobPositions, (OrganizationType)type, isEnabled);
        }
    }
}
