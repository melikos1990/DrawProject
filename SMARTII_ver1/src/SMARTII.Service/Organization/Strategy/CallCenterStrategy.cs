using System;
using System.Collections.Generic;
using System.Linq;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Organization;

namespace SMARTII.Service.Organization.Strategy
{
    public class CallCenterStrategy : IOrganizationProcessStrategy
    {
        private readonly IOrganizationAggregate _OrganizationAggregate;

        public CallCenterStrategy(IOrganizationAggregate OrganizationAggregate)
        {
            _OrganizationAggregate = OrganizationAggregate;
        }

        public void WhenNodeUpdate(IOrganizationNode node)
        {
            var data = (CallCenterNode)node;

            if (data == null) return;

            var ids = data.HeaderQuarterNodes?.Select(x => x.NodeID) ?? new List<int>();

            _OrganizationAggregate.CallCenterNode_T1_T2_.Operator(context =>
            {
                var db = (SMARTIIEntities)context;
                db.Configuration.LazyLoadingEnabled = false;

                // 取得服務BU
                var servingBus = db.HEADQUARTERS_NODE.Where(x => ids.Contains(x.NODE_ID)).ToList();

                var vendor = db.CALLCENTER_NODE
                    .Include("HEADQUARTERS_NODE")
                    .Where(x => x.NODE_ID == data.ID).FirstOrDefault();

                vendor.CALLCENTER_ID = data.NodeID;
                vendor.NODE_TYPE_KEY = EssentialCache.NodeDefinitionValue.CallCenter;
                vendor.HEADQUARTERS_NODE = servingBus;

                db.SaveChanges();
            });
        }
    }
}
