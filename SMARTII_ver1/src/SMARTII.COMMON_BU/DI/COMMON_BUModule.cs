using Autofac;
using Autofac.Integration.WebApi;
using SMARTII.COMMON_BU.Service;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Case;
using SMARTII.Domain.Master;
using SMARTII.Domain.Notification;
using SMARTII.Service.Notification.Factory;


namespace SMARTII.COMMON_BU.DI
{
    public class COMMON_BUModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var currentAssembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(currentAssembly)
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            builder.RegisterType<ItemFactory>()
                   .As<IItemFactory>()
                   .Keyed<IItemFactory>(EssentialCache.BusinessKeyValue.COMMONBU)
                   .InstancePerDependency();


            builder.RegisterType<StoreFactory>()
                   .As<IStoreFactory>()
                   .Keyed<IStoreFactory>(EssentialCache.BusinessKeyValue.COMMONBU)
                   .InstancePerDependency();

            builder.RegisterType<CaseSpecificFactory>()
                   .As<ICaseSpecificFactory>()
                   .Keyed<ICaseSpecificFactory>(EssentialCache.BusinessKeyValue.COMMONBU)
                   .InstancePerDependency();

            #region 解析Email 取得實體信 透過不同協定
            builder.RegisterType<EmailPOP3Factory>()
                   .As<IEmailMailProtocolFactory>()
                   .Keyed<IEmailMailProtocolFactory>(EssentialCache.MailProtocolKeyValue.POP3)
                   .InstancePerDependency();

            builder.RegisterType<EmailOffice365Factory>()
                   .As<IEmailMailProtocolFactory>()
                   .Keyed<IEmailMailProtocolFactory>(EssentialCache.MailProtocolKeyValue.OFFICE365)
                   .InstancePerDependency();
            #endregion

         


            #region 解析Email內容(依各BU)

            builder.RegisterType<EmailParser>()
                   .As<IEmailParser>()
                   .Keyed<IEmailParser>(EssentialCache.BusinessKeyValue.COMMONBU)
                   .InstancePerDependency();

     
            #endregion

            builder.RegisterApiControllers(currentAssembly);
        }
    }
}
