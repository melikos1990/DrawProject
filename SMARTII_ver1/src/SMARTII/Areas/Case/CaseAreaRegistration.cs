using System.Web.Http;
using System.Web.Mvc;

namespace SMARTII.Areas.Case
{
    public class CaseAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Case";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
              "Case_default",
              "Case/{controller}/{action}/{id}",
              new { action = "Index", id = UrlParameter.Optional }
          );

            context.Routes.MapHttpRoute(
                   "Case_default_api",
                   "api/Case/{controller}/{action}/{id}",
                   new { id = UrlParameter.Optional }
                );
        }
    }
}