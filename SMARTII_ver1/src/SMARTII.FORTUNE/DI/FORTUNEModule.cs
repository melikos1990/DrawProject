using Autofac;
using Autofac.Integration.WebApi;
using SMARTII.FORTUNE.Service;
using SMARTII.Domain.Report;
using SMARTII.Domain.Case;
using static SMARTII.Domain.Cache.EssentialCache;

namespace SMARTII.FORTUNE.DI
{
    public class FORTUNEModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var currentAssembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(currentAssembly)
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            builder.RegisterType<FORTUNEFactory>()
                   .As<IFORTUNEFactory>()
                   .InstancePerDependency();

            builder.RegisterType<FORTUNEFacade>()
                   .InstancePerDependency();

            builder.RegisterType<COMMON_BU.Service.ReportFacade>()
                   .InstancePerDependency();
            
            builder.RegisterType<FORTUNEFactory>().As<IImportStoreFactory>()
                  .Keyed<IImportStoreFactory>(BusinessKeyValue.FORTUNE)
                  .InstancePerDependency();

            builder.RegisterApiControllers(currentAssembly);



        }
    }
}
