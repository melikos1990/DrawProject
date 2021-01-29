using System.Web.Http;
using System.Web.Mvc;

namespace SMARTII.Areas.Select
{
    public class CommonAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Common";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
              "Common_default",
              "Common/{controller}/{action}/{id}",
              new { action = "Index", id = UrlParameter.Optional }
          );

            context.Routes.MapHttpRoute(
                   "Common_default_api",
                   "api/Common/{controller}/{action}/{id}",
                   new { id = UrlParameter.Optional }
                );
        }
    }
}