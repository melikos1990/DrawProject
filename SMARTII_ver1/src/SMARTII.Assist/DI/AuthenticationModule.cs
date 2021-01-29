using Autofac;
using Autofac.Extras.AggregateService;
using SMARTII.Assist.Authentication;
using SMARTII.Domain.Authentication;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Authentication.Service;
using SMARTII.Service.Organization.Factory;

namespace SMARTII.Assist.DI
{
    public class AuthenticationModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SystemAccountFactory>().As<IAccountFactory>()
                   .Keyed<IAccountFactory>(UserType.System)
                   .InstancePerDependency();

            builder.RegisterType<ADAccountFactory>().As<IAccountFactory>()
                  .Keyed<IAccountFactory>(UserType.AD)
                  .InstancePerDependency();

            builder.RegisterType<LDAPHelper>().SingleInstance();

            builder.RegisterAggregateService<IAuthenticationAggregate>();

            builder.RegisterType<AuthenticationAttribute>().PropertiesAutowired();

            builder.RegisterType<AuthenticationMethodAttribute>().PropertiesAutowired();
        }
    }
}