using System;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;

namespace SMARTII.Service.Organization.Strategy
{
    public class StoreStrategy : IOrganizationProcessStrategy
    {
        private readonly IOrganizationAggregate _OrganizationAggregate;

        public StoreStrategy(IOrganizationAggregate OrganizationAggregate)
        {
            _OrganizationAggregate = OrganizationAggregate;
        }

        public void WhenNodeUpdate(IOrganizationNode node)
        {
            var _node = (HeaderQuarterNode)node;
            var data = _node?.Store;

            if (data == null) return;

            var repeatCon = new MSSQLCondition<STORE>(x => 
                    x.NODE_ID != data.NodeID && 
                    x.CODE == data.Code && 
                    x.HEADQUARTERS_NODE.BU_ID == _node.BUID);

            repeatCon.IncludeBy(x => x.HEADQUARTERS_NODE);

            if (_OrganizationAggregate.Store_T1_.HasAny(repeatCon))
            {
                throw new Exception(HeaderQuarterNode_lang.HEADERQUARTER_STORE_CODE_ERROR);
            }
            

            var con = new MSSQLCondition<STORE>(x => x.NODE_ID == data.NodeID);

            if (_OrganizationAggregate.Store_T1_T2_.HasAny(con) == false)
            {
                data.CreateDateTime = DateTime.Now;
                data.CreateUserName = ContextUtility.GetUserIdentity()?.Name;
                data.IsEnabled = _node.IsEnabled;

                _OrganizationAggregate.Store_T1_T2_.Add(data);

            }
            else
            {
                con.ActionModify(x =>
                {
                    x.CODE = data.Code;
                    x.NAME = data.Name;
                    x.IS_ENABLED = _node.IsEnabled;
                    x.UPDATE_DATETIME = DateTime.Now;
                    x.UPDATE_USERNAME = ContextUtility.GetUserIdentity()?.Name;
                });
                _OrganizationAggregate.Store_T1_T2_.UpdateRange(con);
            }
        }
    }
}
