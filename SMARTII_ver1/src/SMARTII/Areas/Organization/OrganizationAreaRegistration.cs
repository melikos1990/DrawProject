using System.Web.Http;
using System.Web.Mvc;

namespace SMARTII.Areas.Organization
{
    public class OrganizationAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Organization";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Organization_default",
                "Organization/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );

            context.Routes.MapHttpRoute(
                   "Organization_default_api",
                   "api/Organization/{controller}/{action}/{id}",
                   new { id = UrlParameter.Optional }
                );
        }
    }
}