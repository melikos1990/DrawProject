using Autofac;
using Autofac.Integration.WebApi;
using SMARTII.OpenPoint.Domain;
using SMARTII.OpenPoint.Service;
using SMARTII.Domain.Report;
using static SMARTII.Domain.Cache.EssentialCache;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Cache;

namespace SMARTII.OpenPoint.DI
{
    public class OpenPointModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var currentAssembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(currentAssembly)
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();


            builder.RegisterType<OpenPointFactory>()
                   .As<IOpenPointFactory>()
                   .InstancePerDependency();

            builder.RegisterType<OpenPointFacade>()
                   .InstancePerDependency();

            builder.RegisterType<ReportProvider>()
                   .InstancePerDependency();

            builder.RegisterType<COMMON_BU.Service.ReportFacade>()
                   .InstancePerDependency();


            #region 解析Email內容(依各BU)

            builder.RegisterType<EmailParser>()
                   .As<IEmailParser>()
                   .Keyed<IEmailParser>(EssentialCache.BusinessKeyValue.OpenPoint)
                   .InstancePerDependency();


            #endregion


            builder.RegisterApiControllers(currentAssembly);



        }
    }
}
