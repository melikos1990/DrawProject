using System.Web.Http;
using SMARTII.Domain.Authentication.Object;

namespace SMARTII.Assist.Web
{
    public class BaseApiController : ApiController
    {
        public UserIdentity UserIdentity => this.User.Identity as UserIdentity;
    }
}