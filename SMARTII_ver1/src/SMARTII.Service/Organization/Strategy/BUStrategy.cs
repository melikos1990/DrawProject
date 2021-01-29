using System;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Organization;
using SMARTII.Resource.Tag;

namespace SMARTII.Service.Organization.Strategy
{
    public class BUStrategy : IOrganizationProcessStrategy
    {
        private readonly IOrganizationAggregate _OrganizationAggregate;

        public BUStrategy(IOrganizationAggregate OrganizationAggregate)
        {
            _OrganizationAggregate = OrganizationAggregate;
        }

        public void WhenNodeUpdate(IOrganizationNode node)
        {
            var data = (HeaderQuarterNode)node;

            if (data == null) return;
            
            if (_OrganizationAggregate.HeaderQuarterNode_T1_.HasAny(x => x.NODE_ID != data.NodeID && x.NODE_KEY == data.NodeKey))
            {
                throw new Exception(HeaderQuarterNode_lang.HEADERQUARTER_NODE_KEY_ERROR);
            }

            var con = new MSSQLCondition<HEADQUARTERS_NODE>(x => x.NODE_ID == data.ID);

            con.ActionModify(x =>
            {
                x.BU_ID = data.NodeID;
                x.NODE_KEY = data.NodeKey;
                x.ENTERPRISE_ID = data.EnterpriseID;
            });

            _OrganizationAggregate.HeaderQuarterNode_T1_T2_.UpdateRange(con);
        }
    }
}
