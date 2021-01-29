using Autofac;
using Autofac.Integration.WebApi;
using SMARTII.ColdStone.Service;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Report;
using static SMARTII.Domain.Cache.EssentialCache;

namespace SMARTII.ColdStone.DI
{
    public class ColdStoneModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var currentAssembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(currentAssembly)
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            builder.RegisterApiControllers(currentAssembly);

            builder.RegisterType<ReportProvider>()
                .As<IReportProvider>()
                .Keyed<IReportProvider>(EssentialCache.BusinessKeyValue.ColdStone)
                .InstancePerDependency();

            builder.RegisterType<ColdStoneFactory>()
                   .As<IColdStoneFactory>()
                   .InstancePerDependency();

            builder.RegisterType<ColdStoneFacade>()
                   .InstancePerDependency();

            builder.RegisterType<ReportProvider>()
                   .InstancePerDependency();

            builder.RegisterType<COMMON_BU.Service.ReportFacade>()
                   .InstancePerDependency();

            builder.RegisterType<ColdStoneFactory>().As<IImportStoreFactory>()
                  .Keyed<IImportStoreFactory>(BusinessKeyValue.ColdStone)
                  .InstancePerDependency();

            #region 解析Email內容(依各BU)

            builder.RegisterType<EmailParser>()
                   .As<IEmailParser>()
                   .Keyed<IEmailParser>(EssentialCache.BusinessKeyValue.ColdStone)
                   .InstancePerDependency();


            #endregion

        }
    }
}
