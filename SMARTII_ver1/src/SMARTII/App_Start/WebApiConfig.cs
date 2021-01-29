using System.Web.Http;
using System.Web.Http.Cors;
using MultipartDataMediaFormatter;
using MultipartDataMediaFormatter.Infrastructure;
using SMARTII.Assist.Converter;

namespace SMARTII
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional });

            // 自行定義 Json Converter
            // 讓傳入參數允許介面化
            // var jsonConverter = new JsonConverter();

            // 自行定義 Mutiple FormData Converter
            // 讓傳入參數允許介面化
            //var mutipartFormDataConverter = new MutipartFormDataConverter(new MultipartFormatterSettings());

            GlobalConfiguration.Configuration.Formatters.Add(new FormMultipartEncodedMediaTypeFormatter());
        }
    }
}
