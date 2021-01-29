using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using SMARTII.Domain.Organization;

namespace SMARTII.Service.Organization.Resolver
{
    public class OrganizationNodeResolver
    {
        private readonly IOrganizationAggregate _OrganizationAggregate;

        public OrganizationNodeResolver(IOrganizationAggregate OrganizationAggregate)
        {
            _OrganizationAggregate = OrganizationAggregate;
        }

        private IOrganizationNode GetNode(int nodeID, OrganizationType organizationType)
        {
            switch (organizationType)
            {
                case OrganizationType.HeaderQuarter:
                    return _OrganizationAggregate
                    .HeaderQuarterNode_T1_IOrganizationNode_
                    .Get(x => x.NODE_ID == nodeID && x.ORGANIZATION_TYPE == (byte)organizationType);

                case OrganizationType.CallCenter:
                    return _OrganizationAggregate
                    .CallCenterNode_T1_IOrganizationNode_
                    .Get(x => x.NODE_ID == nodeID && x.ORGANIZATION_TYPE == (byte)organizationType);

                case OrganizationType.Vendor:
                    return _OrganizationAggregate
                    .VendorNode_T1_IOrganizationNode_
                    .Get(x => x.NODE_ID == nodeID && x.ORGANIZATION_TYPE == (byte)organizationType);

                default:
                    break;
            }

            return null;
        }

        public IEnumerable<T> ResolveCollection<T>(IEnumerable<T> data) where T : IOrganizationRelationship, new()
        {
            IDictionary<string, IOrganizationNode> dist = new Dictionary<string, IOrganizationNode>();

            var group = data.GroupBy(x => new
            {
                NodeID = x.NodeID,
                OrganizationType = x.OrganizationType
            });

            group.ForEach(pair =>
            {
                IOrganizationNode node = GetNode(pair.Key.NodeID, pair.Key.OrganizationType);

                if (node != null)
                    dist.Add($"{pair.Key.NodeID}-{pair.Key.OrganizationType}", node);
            });

            data.ForEach(item =>
            {
                var node = dist[$"{item.NodeID}-{item.OrganizationType}"];

                item.NodeID = node.NodeID;
                item.OrganizationType = node.OrganizationType;
                item.Node = node;
                item.NodeName = node.Name;
            });

            return data;
        }

        public T Resolve<T>(T data) where T : IOrganizationRelationship, new()
        {
            IOrganizationNode node = GetNode(data.NodeID, data.OrganizationType);

            if (node != null)
            {
                data.NodeID = node.NodeID;
                data.OrganizationType = node.OrganizationType;
                data.Node = node;
                data.NodeName = node.Name;
            }

            return data;
        }
    }
}
