using Autofac;

namespace SMARTII.Tests
{
    public class WebModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var currentAssembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(currentAssembly)
                                .AsImplementedInterfaces()
                                .InstancePerLifetimeScope();

            //builder.RegisterWebApiFilterProvider(GlobalConfiguration.Configuration);

            //// 註冊SignalR Hub 實體
            //builder.RegisterInstance(new HubConfiguration().Resolver.Resolve<IConnectionManager>());
            //builder.RegisterHubs(System.Reflection.Assembly.GetExecutingAssembly()).SingleInstance();

            //builder.RegisterApiControllers(currentAssembly);
        }
    }
}