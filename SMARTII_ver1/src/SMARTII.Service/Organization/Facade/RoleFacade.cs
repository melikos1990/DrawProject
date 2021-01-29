using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;

namespace SMARTII.Service.Organization.Facade
{
    public class RoleFacade
    {
        private readonly ICommonAggregate _CommonAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        public RoleFacade(
            ICommonAggregate CommonAggregate,
            IOrganizationAggregate OrganizationAggregate)
        {
            _CommonAggregate = CommonAggregate;
            _OrganizationAggregate = OrganizationAggregate;
        }

        public async Task<bool> Disable(int? RoleID)
        {
            var result = false;

            var con = new MSSQLCondition<ROLE>(x => x.ID == RoleID);
            con.IncludeBy(x => x.USER);

            var item = _OrganizationAggregate.Role_T1_T2_.Get(con);

            //確認該權限角色是否存在
            if (item == null)
                throw new Exception(Common_lang.USER_UNDEFIND);

            //確認該權限角色底下是否有使用者
            if (item.Users.Count > 0)
            {
                item.Users.ForEach(x =>
                {
                    if (x.IsEnabled)
                        throw new Exception("此權限底下有使用者，尚未停用");
                });
            }

            con.ActionModify(x =>
            {
                x.IS_ENABLED = false;
                x.UPDATE_DATETIME = DateTime.Now;
                x.UPDATE_USERNAME = ContextUtility.GetUserIdentity()?.Name;
            });

            _OrganizationAggregate.Role_T1_T2_.Update(con);

            return await result.Async();
        }
    }
}
