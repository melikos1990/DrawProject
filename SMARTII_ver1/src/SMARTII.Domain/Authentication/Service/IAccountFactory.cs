using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Authentication.Service
{
    public interface IAccountFactory
    {
        User Login(User user, string password);
    }
}