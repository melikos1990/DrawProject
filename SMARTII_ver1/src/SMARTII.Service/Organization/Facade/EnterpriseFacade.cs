using System;
using System.Threading.Tasks;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;

namespace SMARTII.Service.Organization.Facade
{
    public class EnterpriseFacade : IEnterpriseFacade
    {
        private readonly IOrganizationAggregate _OrganizationAggregate;

        public EnterpriseFacade(IOrganizationAggregate OrganizationAggregate)
        {
            _OrganizationAggregate = OrganizationAggregate;
        }

        public Task<Enterprise> Create(Enterprise data)
        {
            var item = _OrganizationAggregate.Enterprise_T1_T2_
                                       .Get(x => x.NAME == data.Name);

            if (item != null)
                throw new Exception(Enterprise_lang.ENTERPRISE_NAME_REPEAT);

            data.CreateDateTime = DateTime.Now;
            data.CreateUserName = ContextUtility.GetUserIdentity()?.Name;

            var result = _OrganizationAggregate.Enterprise_T1_T2_.Add(data);

            return result.Async();
        }

        public Task<Enterprise> Update(Enterprise data)
        {
            string name = data.Name.Trim();

            // 修改後的 , 其他的資料(除了本身)是否有相同
            var item = _OrganizationAggregate.Enterprise_T1_T2_
                                       .Get(x => x.ID != data.ID &&
                                                 x.NAME == name);

            if (item != null)
                throw new Exception(Enterprise_lang.ENTERPRISE_NAME_REPEAT);

            var con = new MSSQLCondition<ENTERPRISE>(x => x.ID == data.ID);
            con.ActionModify(x =>
            {
                x.NAME = data.Name;
                x.IS_ENABLED = data.IsEnabled;
                x.UPDATE_DATETIME = DateTime.Now;
                x.UPDATE_USERNAME = ContextUtility.GetUserIdentity()?.Name;
            });

            var result = _OrganizationAggregate.Enterprise_T1_T2_.Update(con);

            return result.Async();
        }
    }
}
