using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;

namespace SMARTII.COMMON_BU.Service
{
    public class StoreFactory : IStoreFactory
    {

        private readonly IMasterAggregate _MasterAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;

        public StoreFactory(
            IOrganizationAggregate OrganizationAggregate,
            IMasterAggregate MasterAggregate
            )
        {
            _OrganizationAggregate = OrganizationAggregate;
            _MasterAggregate = MasterAggregate;
        }

        public async Task Update<T>(Store store)
        {
            var con = new MSSQLCondition<STORE>(x => x.NODE_ID == store.NodeID);
            
            con.ActionModify(x =>
            {
                x.CODE = store.Code;
                x.TELEPHONE = store.Telephone;
                x.EMAIL = store.Email;
                x.ADDRESS = store.Address;
                x.SERVICE_TIME = store.ServiceTime;
                x.OWNER_NODE_JOB_ID = store.OwnerNodeJobID;
                x.SUPERVISOR_NODE_JOB_ID = store.SupervisorNodeJobID;
                x.STORE_CLOSE_DATETIME = store.StoreCloseDateTime;
                x.STORE_OPEN_DATETIME = store.StoreOpenDateTime;
                x.MEMO = store.Memo;
                x.STORE_TYPE = store.StoreType;
                x.J_CONTENT = (store as Store<T>)?.JContent;
                x.UPDATE_DATETIME = DateTime.Now;
                x.UPDATE_USERNAME = ContextUtility.GetUserIdentity().Name;
            });

            _OrganizationAggregate.Store_T1_T2_.Update(con);

        }
    }
}
