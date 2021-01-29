using SMARTII.Domain.Authentication.Service;

namespace SMARTII.Domain.Authentication
{
    public interface IAuthenticationAggregate
    {
        LDAPHelper AD { get; }
    }
}