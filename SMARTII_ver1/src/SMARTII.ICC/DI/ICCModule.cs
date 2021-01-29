using Autofac;
using Autofac.Integration.WebApi;
using SMARTII.Domain.Report;
using SMARTII.ICC.Domain;
using SMARTII.ICC.Service;
using SMARTII.Domain.Case;
using static SMARTII.Domain.Cache.EssentialCache;

namespace SMARTII.ICC.DI
{
    public class ICCModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var currentAssembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(currentAssembly)
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            builder.RegisterType<ICCExcelParser>()
                   .As<IExcelParser>()
                   .Keyed<IExcelParser>(BusinessKeyValue.ICC)
                   .InstancePerDependency();

            builder.RegisterType<CaseSpecificFactory>().As<ICaseSpecificFactory>()
                    .Keyed<ICaseSpecificFactory>(BusinessKeyValue.ICC)
                    .InstancePerDependency();

            builder.RegisterType<ICCFactory>()
                   .As<IICCFactory>()
                   .InstancePerDependency();

            builder.RegisterType<ICCFacade>()
                   .InstancePerDependency();

            builder.RegisterType<ReportProvider>()
                   .InstancePerDependency();

            builder.RegisterType<COMMON_BU.Service.ReportFacade>()
                   .InstancePerDependency();


            builder.RegisterType<ICCFactory>().As<IImportStoreFactory>()
                  .Keyed<IImportStoreFactory>(BusinessKeyValue.ICC)
                  .InstancePerDependency();

            builder.RegisterApiControllers(currentAssembly);



        }
    }
}
