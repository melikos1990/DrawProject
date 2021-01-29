using System;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Authentication;
using SMARTII.Domain.Authentication.Service;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Security;
using SMARTII.Resource.Tag;

namespace SMARTII.Service.Organization.Factory
{
    public class SystemAccountFactory : IAccountFactory
    {
        private readonly IOrganizationAggregate _OrganizationAggregate;

        public SystemAccountFactory(IOrganizationAggregate OrganizationAggregate)
        {
            _OrganizationAggregate = OrganizationAggregate;
        }

        public User Login(User user, string password)
        {
            var con = new MSSQLCondition<USER>(x => x.ACCOUNT == user.Account);

            if (user.Password.Equals(password.Md5Hash()) == false)
            {
                con.ActionModify(x =>
                {
                    x.ERROR_PASSWORD_COUNT += 1;

                    if (x.ERROR_PASSWORD_COUNT == SecurityCache.ErrorCountLimit)
                    {
                        x.LOCKOUT_DATETIME = DateTime.Now;
                    }
                });

                _OrganizationAggregate.User_T1_T2_.Update(con);

                throw new Exception(Common_lang.USER_LOGIN_ERROR);
            }
            else
            {
                con.ActionModify(x =>
                {
                    x.ERROR_PASSWORD_COUNT = 0;
                    x.LOCKOUT_DATETIME = null;
                });

                _OrganizationAggregate.User_T1_T2_.Update(con);
            }

            if (user.LastChangePasswordDateTime.HasValue == false)
                throw new ResetPasswordException("", new Exception(Common_lang.FIRST_LOGIN));

            if (user.LastChangePasswordDateTime.HasValue &&
                user.LastChangePasswordDateTime.Value < DateTime.Now.AddMonths(-3))
                throw new ResetPasswordException("", new Exception(Common_lang.MONTH_TIMEOUT_LOGIN));

            return user;
        }
    }
}
