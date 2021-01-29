using System.Web.Http;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.SignalR;
using Autofac.Integration.WebApi;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using SMARTII.Assist.Web;

namespace SMARTII.Configuration.DI
{
    public class WebModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var currentAssembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(currentAssembly)
                                .AsImplementedInterfaces()
                                .InstancePerLifetimeScope();

            builder.RegisterWebApiFilterProvider(GlobalConfiguration.Configuration);

            // 註冊SignalR Hub 實體
            builder.RegisterInstance(new HubConfiguration().Resolver.Resolve<IConnectionManager>());
            builder.RegisterHubs(System.Reflection.Assembly.GetExecutingAssembly()).SingleInstance();

            builder.RegisterControllers().PropertiesAutowired();

            builder.RegisterApiControllers(currentAssembly).PropertiesAutowired();
        }
    }
}
