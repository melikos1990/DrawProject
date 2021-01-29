using Autofac;
using Autofac.Integration.WebApi;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Report;
using SMARTII.MisterDonut.Service;
using static SMARTII.Domain.Cache.EssentialCache;

namespace SMARTII.MisterDonut.DI
{
    public class MisterDonutModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var currentAssembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(currentAssembly)
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            builder.RegisterType<ReportProvider>().As<IReportProvider>()
              .Keyed<IReportProvider>(EssentialCache.BusinessKeyValue.MisterDonut)
              .InstancePerDependency();

            builder.RegisterType<MisterDonutFactory>()
                   .As<IMisterDonutFactory>()
                   .InstancePerDependency();

            builder.RegisterType<MisterDonutFacade>()
                   .InstancePerDependency();

            builder.RegisterType<ReportProvider>()
                   .InstancePerDependency();

            builder.RegisterType<COMMON_BU.Service.ReportFacade>()
                   .InstancePerDependency();


            builder.RegisterType<MisterDonutFactory>().As<IImportStoreFactory>()
                  .Keyed<IImportStoreFactory>(BusinessKeyValue.MisterDonut)
                  .InstancePerDependency();

            builder.RegisterApiControllers(currentAssembly);

        }
    }
}
