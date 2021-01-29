using System.Security.Principal;
using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Authentication.Object
{
    public class UserIdentity : IIdentity
    {
        public UserIdentity()
        {
        }

        public UserIdentity(User user, IIdentity identity)
        {
            this.Email = user.Email;
            this.Name = user.Name;
            this.Account = user.Account;
            this.AuthenticationType = identity.AuthenticationType;
            this.IsAuthenticated = identity.IsAuthenticated;
            this.Instance = user;
        }

        public string Name { get; set; }

        public string Account { get; set; }

        public string AuthenticationType { get; set; }

        public bool IsAuthenticated { get; set; }

        public string Email { get; set; }

        public User Instance { get; set; }
    }
}