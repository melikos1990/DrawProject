using Autofac;
using Autofac.Extras.AggregateService;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Report;
using SMARTII.Service.Notification.Facade;
using SMARTII.Service.Notification.Factory;
using SMARTII.Service.Notification.Provider;
using SMARTII.Service.Notification.Service;
using SMARTII.Service.Report.Provider;

namespace SMARTII.Assist.DI
{
    public class NotificationModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SignalRProvider>().As<INotificationProvider>()
                   .Keyed<INotificationProvider>(NotificationType.UI)
                   .SingleInstance();
            builder.RegisterType<EmailProvider>().As<INotificationProvider>()
                   .Keyed<INotificationProvider>(NotificationType.Email)
                   .SingleInstance();
            builder.RegisterType<SMSProvider>().As<INotificationProvider>()
                   .Keyed<INotificationProvider>(NotificationType.SMS)
                   .SingleInstance();

            // 因偕同各BU MODUEL 進行依賴注射
            builder.RegisterType<NotificationGroupBothFactory>().As<INotificationGroupFactory>()
               .Keyed<INotificationGroupFactory>(NotificationCalcType.Both.ToString())
               .InstancePerDependency();

            builder.RegisterType<NotificationGroupItemFactory>().As<INotificationGroupFactory>()
               .Keyed<INotificationGroupFactory>(NotificationCalcType.ByItem.ToString())
               .InstancePerDependency();

            builder.RegisterType<NGQuestionClassificationFactory>().As<INotificationGroupFactory>()
               .Keyed<INotificationGroupFactory>(NotificationCalcType.ByQuestion.ToString())
               .InstancePerDependency(); 

            builder.RegisterType<NotificationPersonalFacade>()
                .As<INotificationPersonalFacade>()
               .InstancePerDependency(); 

            builder.RegisterType<NotificationPersonalService>()
                .As<INotificationPersonalService>()
               .InstancePerDependency();

            builder.RegisterType<BatchReportProvider>()
                .As<IBatchReportProvider>()
               .InstancePerDependency();

            builder.RegisterAggregateService<INotificationAggregate>();
        }
    }
}
