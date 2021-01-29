using System.Reflection;
using Autofac;
using Ptc.Data.Condition2.Mssql.DI;

namespace SMARTII.Assist.DI
{
    public class MainModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            Assembly[] assemblies = new Assembly[]
            {
                 Assembly.Load("SMARTII.Domain"),
                 Assembly.Load("SMARTII.Service"),
                 Assembly.Load("SMARTII.Assist"),
            };

            builder.RegisterAssemblyTypes(assemblies)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            // SHAERD LIBRARY
            builder.RegisterModule(new MSSQLModule());

            // FEATURE
            builder.RegisterModule(new NotificationModule());
            builder.RegisterModule(new AuthenticationModule());
            builder.RegisterModule(new CommonModule());
            builder.RegisterModule(new SystemModule());
            builder.RegisterModule(new OrganizationModule());
            builder.RegisterModule(new MasterModule());
            builder.RegisterModule(new CaseModule());
        }
    }
}