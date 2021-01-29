using System.Web.Http;
using System.Web.Mvc;

namespace SMARTII.Areas.Master
{
    public class MasterAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Master";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Master_default",
                "Master/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );

            context.Routes.MapHttpRoute(
                "Master_default_api",
                "api/Master/{controller}/{action}/{id}",
                new { id = UrlParameter.Optional }
            );
        }
    }
}