using Autofac;
using Autofac.Integration.WebApi;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Case;
using SMARTII.Domain.Master;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Report;
using SMARTII.PPCLIFE.Domain;
using SMARTII.PPCLIFE.Service;
using static SMARTII.Domain.Cache.EssentialCache;

namespace SMARTII.PPCLIFE.DI
{
    public class PPCLIFEModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var currentAssembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(currentAssembly)
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            builder.RegisterType<ItemFactory>().As<IItemFactory>()
                  .Keyed<IItemFactory>(EssentialCache.BusinessKeyValue.PPCLIFE)
                  .InstancePerDependency();

            builder.RegisterType<ReportProvider>().As<IReportProvider>()
                .Keyed<IReportProvider>(EssentialCache.BusinessKeyValue.PPCLIFE)
                .InstancePerDependency();

            builder.RegisterType<CaseSpecificFactory>().As<ICaseSpecificFactory>()
                .Keyed<ICaseSpecificFactory>(EssentialCache.BusinessKeyValue.PPCLIFE)
                .InstancePerDependency();

            builder.RegisterType<PPCLIFEExcelParser>()
                   .As<IExcelParser>()
                   .Keyed<IExcelParser>(BusinessKeyValue.PPCLIFE)
                   .InstancePerDependency();

            builder.RegisterType<PPCLIFENotificationService>()
                   .As<IPPCLIFENotificationService>()
                   .InstancePerDependency();

            builder.RegisterType<PPCLIFEFacade>()
                   .As<IPPCLifeNotificationFacade>()
                   .InstancePerDependency();

            builder.RegisterType<PPCLIFEFactory>()
                   .As<IPPCLIFEFactory>()
                   .InstancePerDependency();

            builder.RegisterType<PPCLIFEFacade>()
                   .InstancePerDependency();

            builder.RegisterType<ReportProvider>()
                   .InstancePerDependency();

            // 因偕同各TYPE 進行依賴注射
            builder.RegisterType<PPCLifeNotificationAllSame>().As<IPPCLifeNotificationFactory>()
               .Keyed<IPPCLifeNotificationFactory>(PPCLifeArriveType.AllSame)
               .InstancePerDependency();

            builder.RegisterType<PPCLifeNotificationDiffBatchNo>().As<IPPCLifeNotificationFactory>()
               .Keyed<IPPCLifeNotificationFactory>(PPCLifeArriveType.DiffBatcNo)
               .InstancePerDependency();

            builder.RegisterType<PPCLifeNotificationNoBatchNo>().As<IPPCLifeNotificationFactory>()
               .Keyed<IPPCLifeNotificationFactory>(PPCLifeArriveType.NothingBatchNo)
               .InstancePerDependency();


            builder.RegisterType<COMMON_BU.Service.ReportFacade>()
                   .InstancePerDependency();
            builder.RegisterType<PPCLIFE.Service.PPCLifeReportFacade>()
                   .InstancePerDependency();

            builder.RegisterType<COMMON_BU.Service.ReportFactory>()
                   .InstancePerDependency();

            builder.RegisterType<PPCLIFENotificationService>()
                   .InstancePerDependency();

            builder.RegisterType<PPCLIFEFactory>().As<IImportStoreFactory>()
                  .Keyed<IImportStoreFactory>(BusinessKeyValue.PPCLIFE)
                  .InstancePerDependency();
            


            builder.RegisterApiControllers(currentAssembly);
        }
    }
}
