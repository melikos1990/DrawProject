using Autofac.Features.Indexed;
using Ptc.Data.Condition2.Mssql.Repository;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Notification.Email;

namespace SMARTII.Domain.Notification
{
    public interface INotificationAggregate
    {
        IIndex<NotificationType, INotificationProvider> Providers { get; }

        IMSSQLRepository<NOTIFICATION_GROUP> NotificationGroup_T1_ { get; }

        IMSSQLRepository<NOTIFICATION_GROUP, NotificationGroup> NotificationGroup_T1_T2_ { get; }

        IMSSQLRepository<NOTIFICATION_GROUP_USER> NotificationGroupUser_T1_ { get; }

        IMSSQLRepository<NOTIFICATION_GROUP_USER, NotificationGroupUser> NotificationGroupUser_T1_T2_ { get; }

        IMSSQLRepository<NOTIFICATION_GROUP_EFFECTIVE_SUMMARY> NotificationGroupEffectiveSummary_T1_ { get; }

        IMSSQLRepository<NOTIFICATION_GROUP_EFFECTIVE_SUMMARY, NotificationGroupEffectiveSummary> NotificationGroupEffectiveSummary_T1_T2_ { get; }

        IMSSQLRepository<NOTIFICATION_GROUP_RESUME> NotificationGroupResume_T1_ { get; }

        IMSSQLRepository<NOTIFICATION_GROUP_RESUME, NotificationGroupResume> NotificationGroupResume_T1_T2_ { get; }

        IMSSQLRepository<PERSONAL_NOTIFICATION> PersonalNotification_T1_ { get; }

        IMSSQLRepository<PERSONAL_NOTIFICATION, PersonalNotification> PersonalNotification_T1_T2_ { get; }

        IMSSQLRepository<OFFICIAL_EMAIL_GROUP> OfficialEmailGroup_T1_ { get; }

        IMSSQLRepository<OFFICIAL_EMAIL_GROUP, OfficialEmailGroup> OfficialEmailGroup_T1_T2_ { get; }


        IMSSQLRepository<OFFICIAL_EMAIL_HISTORY> OfficialEmailHistory_T1_ { get; }

        IMSSQLRepository<OFFICIAL_EMAIL_HISTORY, OfficialEmailHistory> OfficialEmailHistory_T1_T2_ { get; }

        IMSSQLRepository<OFFICIAL_EMAIL_EFFECTIVE_DATA> OfficialEmailEffectivePayload_T1_ { get; }
        IMSSQLRepository<OFFICIAL_EMAIL_EFFECTIVE_DATA, OfficialEmailEffectivePayload> OfficialEmailEffectivePayload_T1_T2_ { get; }

        IMSSQLRepository<PPCLIFE_EFFECTIVE_SUMMARY> PPCLifeEffectiveSummary_T1_ { get; }

        IMSSQLRepository<PPCLIFE_EFFECTIVE_SUMMARY, PPCLifeEffectiveSummary> PPCLifeEffectiveSummary_T1_T2_ { get; }

        IMSSQLRepository<PPCLIFE_RESUME> PPCLifeResume_T1_ { get; }

        IMSSQLRepository<PPCLIFE_RESUME, PPCLifeResume> PPCLifeResume_T1_T2_ { get; }
    }
}
