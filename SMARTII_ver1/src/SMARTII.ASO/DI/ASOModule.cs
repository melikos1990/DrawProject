using Autofac;
using Autofac.Integration.WebApi;
using SMARTII.ASO.Domain;
using SMARTII.ASO.Service;
using SMARTII.Domain.Report;
using SMARTII.Domain.Case;
using static SMARTII.Domain.Cache.EssentialCache;

namespace SMARTII.ASO.DI
{
    public class ASOModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var currentAssembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(currentAssembly)
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            builder.RegisterType<ASOExcelParser>()
                   .As<IExcelParser>()
                   .Keyed<IExcelParser>(BusinessKeyValue.ASO)
                   .InstancePerDependency();

            builder.RegisterType<CaseSpecificFactory>().As<ICaseSpecificFactory>()
                .Keyed<ICaseSpecificFactory>(BusinessKeyValue.ASO)
                .InstancePerDependency();

            builder.RegisterType<ASOFactory>()
                   .As<IASOFactory>()
                   .InstancePerDependency();

            builder.RegisterType<ASOFacade>()
                   .InstancePerDependency();

            builder.RegisterType<ReportProvider>()
                   .InstancePerDependency();

            builder.RegisterType<COMMON_BU.Service.ReportFacade>()
                   .InstancePerDependency();
            
            builder.RegisterType<ASOFactory>().As<IImportStoreFactory>()
                  .Keyed<IImportStoreFactory>(BusinessKeyValue.ASO)
                  .InstancePerDependency();

            builder.RegisterApiControllers(currentAssembly);



        }
    }
}
