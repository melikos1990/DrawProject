using System.Security.Principal;
using System.Web.Http.Controllers;
using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Authentication.Service
{
    public interface IPrincipalManager
    {
        /// <summary>
        /// 產生憑證
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        IPrincipal Generator(HttpActionContext context, User user);
    }
}