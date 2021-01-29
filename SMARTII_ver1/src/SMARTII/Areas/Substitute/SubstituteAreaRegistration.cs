using System.Web.Http;
using System.Web.Mvc;

namespace SMARTII.Areas.Substitute
{
    public class SubstituteAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Substitute";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
               "Substitute_default",
               "Substitute/{controller}/{action}/{id}",
               new { action = "Index", id = UrlParameter.Optional }
           );

            context.Routes.MapHttpRoute(
                "Substitute_default_api",
                "api/Substitute/{controller}/{action}/{id}",
                new { id = UrlParameter.Optional }
            );
        }
    }
}
