using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using SMARTII.Service.Organization.Resolver;

namespace SMARTII.COMMON_BU.Service
{
    public class StoreFacade : IStoreFacade
    {
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly OrganizationNodeResolver _OrganizationNodeResolver;


        public StoreFacade(IOrganizationAggregate OrganizationAggregate,
                           OrganizationNodeResolver OrganizationNodeResolver)
        {
            _OrganizationAggregate = OrganizationAggregate;
            _OrganizationNodeResolver = OrganizationNodeResolver;
        }

        public Store<T> GetComplete<T>(int nodeID, OrganizationType organizationType)
        {
            var con = new MSSQLCondition<STORE>(x => x.NODE_ID == nodeID &&
                                                     x.ORGANIZATION_TYPE == (byte)organizationType);


            con.IncludeBy(x => x.HEADQUARTERS_NODE.HEADQUARTERS_NODE2);

            var store = _OrganizationAggregate.Store_T1_T2_Expendo_.Get(con);

            if (store.SupervisorNodeJobID.HasValue)
                store.OfcJobPosition = GetApplyJobPositions(store.SupervisorNodeJobID.Value, EssentialCache.JobValue.OFC);

            if (store.OwnerNodeJobID.HasValue)
                store.OwnerJobPosition = GetApplyJobPositions(store.OwnerNodeJobID.Value, EssentialCache.JobValue.OWNER);

            return store as Store<T>;

        }



        #region PRIVATE

        public JobPosition GetApplyJobPositions(int nodeJobID, string key)
        {
            var con = new MSSQLCondition<NODE_JOB>(x => x.ID == nodeJobID && x.JOB.KEY == key);
            con.IncludeBy(x => x.JOB);
            con.IncludeBy(x => x.USER);

            var jobPosition = _OrganizationAggregate.JobPosition_T1_T2_.Get(con);

            jobPosition = jobPosition == null ? jobPosition : _OrganizationNodeResolver.Resolve(jobPosition);

            return jobPosition;

        }

        #endregion



    }
}
