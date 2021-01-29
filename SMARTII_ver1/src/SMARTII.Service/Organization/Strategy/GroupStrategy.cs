using System;
using System.Collections.Generic;
using System.Linq;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Organization;

namespace SMARTII.Service.Organization.Strategy
{
    public class GroupStrategy : IOrganizationProcessStrategy
    {
        private readonly IOrganizationAggregate _OrganizationAggregate;

        public GroupStrategy(IOrganizationAggregate OrganizationAggregate)
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

                var group = db.CALLCENTER_NODE
                    .Include("HEADQUARTERS_NODE")
                    .Where(x => x.NODE_ID == data.ID).FirstOrDefault();

                group.WORK_PROCESS_TYPE = (byte)data.WorkProcessType;
                group.IS_WORK_PROCESS_NOTICE = data.IsWorkProcessNotice;
                group.NODE_TYPE_KEY = EssentialCache.NodeDefinitionValue.Group;
                group.HEADQUARTERS_NODE = servingBus;

                db.SaveChanges();
            });

            var con = new MSSQLCondition<CALLCENTER_NODE>(x => x.NODE_ID == data.NodeID);
            con.ActionModify(x =>
            {
                x.WORK_PROCESS_TYPE = (byte)data.WorkProcessType;
            });

            _OrganizationAggregate.CallCenterNode_T1_T2_.UpdateRange(con);
        }
    }
}
