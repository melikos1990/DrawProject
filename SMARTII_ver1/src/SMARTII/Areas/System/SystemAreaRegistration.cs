using System.Web.Http;
using System.Web.Mvc;

namespace SMARTII.Areas.System
{
    public class SystemAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "System";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.Routes.MapHttpRoute(
                   "System_default_api",
                   "api/System/{controller}/{action}/{id}",
                   new { id = UrlParameter.Optional }
                );

            context.MapRoute(
                "System_default",
                "System/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}