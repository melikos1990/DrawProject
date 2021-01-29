using System.Web.Http;
using System.Web.Mvc;

namespace SMARTII.Areas.Summary
{
    public class SummaryAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Summary";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
               "Summary_default",
               "Summary/{controller}/{action}/{id}",
               new { action = "Index", id = UrlParameter.Optional }
           );

            context.Routes.MapHttpRoute(
                "Summary_default_api",
                "api/Summary/{controller}/{action}/{id}",
                new { id = UrlParameter.Optional }
            );
        }
    }
}