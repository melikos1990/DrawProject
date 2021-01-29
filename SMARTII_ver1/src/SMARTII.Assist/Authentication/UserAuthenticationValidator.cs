using System;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Authentication;
using SMARTII.Domain.Authentication.Service;
using SMARTII.Domain.Organization;
using SMARTII.Resource.Tag;

namespace SMARTII.Assist.Authentication
{
    public class UserAuthenticationValidator : IUserAuthenticationValidator
    {
        private readonly IOrganizationAggregate _OrganizationAggregate;

        public UserAuthenticationValidator(IOrganizationAggregate OrganizationAggregate)
        {
            _OrganizationAggregate = OrganizationAggregate;
        }

        public void Valid(User tokenUser)
        {
            if (ValidEffectiveness(tokenUser) == false)
                throw new Exception(User_lang.USER_INVALID);

            if (ValidVersion(tokenUser) == false)
                throw new UserVersionException();
        }

        public Boolean ValidEffectiveness(User tokenUser)
        {
            var con = new MSSQLCondition<USER>(x => x.ACCOUNT == tokenUser.Account);

            var effect = _OrganizationAggregate.User_T1_T2_.GetOfSpecific(con, x => new
            {
                IsEnabled = x.IS_ENABLED,
                IsLockout = x.LOCKOUT_DATETIME.HasValue
            });

            if (effect.IsEnabled == false || effect.IsLockout)
                return false;

            return true;
        }

        public Boolean ValidVersion(User tokenUser)
        {
            var con = new MSSQLCondition<USER>(x => x.ACCOUNT == tokenUser.Account);

            var version = _OrganizationAggregate.User_T1_T2_.GetOfSpecific(con, x => x.VERSION);

            if (version == null)
                return false;

            if (version > tokenUser.Version)
                return false;

            return true;
        }
    }
}