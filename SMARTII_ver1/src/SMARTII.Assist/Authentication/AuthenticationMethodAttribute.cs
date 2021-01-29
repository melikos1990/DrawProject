using System.Web.Http;
using System.Web.Http.Controllers;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Authentication.Service;

namespace SMARTII.Assist.Authentication
{
    public class AuthenticationMethodAttribute : AuthorizeAttribute
    {
        public AuthenticationType AuthenticationType { get; set; }

        public IUserAuthenticationManager _UserAuthenticationValidator { get; set; }

        public AuthenticationMethodAttribute(AuthenticationType authenticationType)
        {
            AuthenticationType = authenticationType;
        }

        public override void OnAuthorization(HttpActionContext content)
        {
            // TODO: 使用者認證
            //_UserAuthenticationValidator
        }
    }
}