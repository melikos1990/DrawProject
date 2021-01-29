using System;
using SMARTII.Domain.Authentication;
using SMARTII.Domain.Authentication.Service;
using SMARTII.Domain.Organization;
using SMARTII.Resource.Tag;

namespace SMARTII.Service.Organization.Factory
{
    public class ADAccountFactory : IAccountFactory
    {
        private readonly IAuthenticationAggregate _AuthenticationAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;

        public ADAccountFactory(IOrganizationAggregate OrganizationAggregate,
                                IAuthenticationAggregate AuthenticationAggregate)
        {
            _OrganizationAggregate = OrganizationAggregate;
            _AuthenticationAggregate = AuthenticationAggregate;
        }

        public User Login(User user, string password)
        {
            var result = _AuthenticationAggregate.AD.IsADLogin(user.Account, password);
            if (result.Item1 == false)
                throw new Exception(result.Item2);

            return user;
        }
    }
}
