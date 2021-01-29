using Autofac;
using Autofac.Integration.WebApi;
using SMARTII._21Century.Service;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Report;
using static SMARTII.Domain.Cache.EssentialCache;

namespace SMARTII._21Century.DI
{
    public class _21CenturyModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var currentAssembly = System.Reflection.Assembly.GetExecutingAssembly();


            builder.RegisterAssemblyTypes(currentAssembly)
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            builder.RegisterType<ReportProvider>().As<IReportProvider>()
                   .Keyed<IReportProvider>(EssentialCache.BusinessKeyValue._21Century)
                   .InstancePerDependency();


            builder.RegisterType<_21CenturyFactory>()
                   .As<I21Factory>()
                   .InstancePerDependency();

            builder.RegisterType<_21CenturyFacade>()
                   .InstancePerDependency();

            builder.RegisterType<ReportProvider>()
                   .InstancePerDependency();

            builder.RegisterType<COMMON_BU.Service.ReportFacade>()
                   .InstancePerDependency();

            builder.RegisterType<_21CenturyFactory>().As<IImportStoreFactory>()
                  .Keyed<IImportStoreFactory>(BusinessKeyValue._21Century)
                  .InstancePerDependency();

            builder.RegisterApiControllers(currentAssembly);

        }
    }
}
