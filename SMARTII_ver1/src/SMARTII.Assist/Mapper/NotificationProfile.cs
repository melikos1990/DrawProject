using System.Dynamic;
using Newtonsoft.Json;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Notification.Email;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Security;

namespace SMARTII.Assist.Mapper
{
    public static class NotificationProfile
    {
        public class NotificationGroupProfile : AutoMapper.Profile
        {
            public NotificationGroupProfile()
            {
                CreateMap<NOTIFICATION_GROUP, NotificationGroup>()
                     .ForMember(dest => dest.CalcMode, opt => opt.MapFrom(src => (NotificationCalcType)src.CALC_MODE))
                     .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                     .ForMember(dest => dest.IsArrive, opt => opt.MapFrom(src => src.IS_ARRIVE))
                     .ForMember(dest => dest.IsNotificated, opt => opt.MapFrom(src => src.IS_NOTIFICATED))
                     .ForMember(dest => dest.ItemID, opt => opt.MapFrom(src => src.ITEM_ID))
                     .ForMember(dest => dest.QuestionClassificationID, opt => opt.MapFrom(src => src.QUESTION_CLASSIFICATION_ID))
                     .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NAME))
                     .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                     .ForMember(dest => dest.NotificationGroupUsers, opt => opt.MapFrom(src => src.NOTIFICATION_GROUP_USER))
                     .ForMember(dest => dest.NotificationGroupResume, opt => opt.MapFrom(src => src.NOTIFICATION_GROUP_RESUME))
                     .ForMember(dest => dest.ActualCount, opt => opt.MapFrom(src => src.ACTUAL_COUNT))
                     .ForMember(dest => dest.OrganizationType, opt => opt.MapFrom(src => (OrganizationType)src.ORGANIZATION_TYPE))
                     .ForMember(dest => dest.AlertCount, opt => opt.MapFrom(src => src.ALERT_COUNT))
                     .ForMember(dest => dest.AlertCycleDay, opt => opt.MapFrom(src => src.ALERT_CYCLE_DAY))
                     .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                     .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                     .ForMember(dest => dest.UpdateDateTime, opt => opt.MapFrom(src => src.UPDATE_DATETIME))
                     .ForMember(dest => dest.UpdateUserName, opt => opt.MapFrom(src => src.UPDATE_USERNAME))


                     .ReverseMap()
                     .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte)src.OrganizationType))
                     .ForPath(dest => dest.CALC_MODE, opt => opt.MapFrom(src => (byte)src.CalcMode))
                     .IgnoreAllNonExisting();
            }
        }

        public class NotificationGroupResumeProfile : AutoMapper.Profile
        {
            public NotificationGroupResumeProfile()
            {
                CreateMap<NOTIFICATION_GROUP_RESUME, NotificationGroupResume>()
                   .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.CONTENT))
                   .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                   .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                   .ForMember(dest => dest.EMLFilePath, opt => opt.MapFrom(src => src.EML_FILE_PATH))
                   .ForMember(dest => dest.GroupID, opt => opt.MapFrom(src => src.GROUP_ID))
                   .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                   .ForMember(dest => dest.NotificationGroupResultType, opt => opt.MapFrom(src => (NotificationGroupResultType)src.TYPE))
                   .ForMember(dest => dest.Target, opt => opt.MapFrom(src => (string.IsNullOrEmpty(src.TARGET) ?
                        new string[] { } : JsonConvert.DeserializeObject<string[]>(src.TARGET))))
                   .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                   .ForMember(dest => dest.NodeName, opt => opt.MapFrom(src => src.NODE_NAME))
                   .ForMember(dest => dest.NotificationGroup, opt => opt.MapFrom(src => src.NOTIFICATION_GROUP))
                   .ReverseMap()
                   .ForPath(dest => dest.TYPE, opt => opt.MapFrom(src => (byte)src.NotificationGroupResultType))
                   .ForPath(dest => dest.TARGET, opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.Target ?? new string[] { })))
                   .IgnoreAllNonExisting();
            }
        }

        public class NotificationGroupUserProfile : AutoMapper.Profile
        {
            public NotificationGroupUserProfile()
            {
                CreateMap<NOTIFICATION_GROUP_USER, NotificationGroupUser>()
                     .ForMember(dest => dest.BUID, opt => opt.MapFrom(src => src.BU_ID))
                     .ForMember(dest => dest.BUName, opt => opt.MapFrom(src => src.BU_NAME))
                     .ForMember(dest => dest.GroupID, opt => opt.MapFrom(src => src.GROUP_ID))
                     .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                     .ForMember(dest => dest.JobName, opt => opt.MapFrom(src => src.JOB_NAME))
                     .ForMember(dest => dest.JobID, opt => opt.MapFrom(src => src.JOB_ID))
                     .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                     .ForMember(dest => dest.NodeName, opt => opt.MapFrom(src => src.NODE_NAME))
                     .ForMember(dest => dest.NotificationBehavior, opt => opt.MapFrom(src => src.NOTIFICATION_BEHAVIOR))
                     .ForMember(dest => dest.NotificationGroup, opt => opt.MapFrom(src => src.NOTIFICATION_GROUP))
                     .ForMember(dest => dest.NotificationKind, opt => opt.MapFrom(src => src.NOTIFICATION_KIND))
                     .ForMember(dest => dest.NotificationRemark, opt => opt.MapFrom(src => src.NOTIFICATION_REMARK))
                     .ForMember(dest => dest.OrganizationType, opt => opt.MapFrom(src => (OrganizationType?)src.ORGANIZATION_TYPE))
                     .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.USER_ID))
                     .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.USER_NAME))
                     .ForMember(dest => dest.UnitType, opt => opt.MapFrom(src => (UnitType)src.UNIT_TYPE))
                     .ForMember(dest => dest.Mobile, opt => opt.MapFrom(src => src.MOBILE))
                     .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EMAIL))
                     .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.ADDRESS))
                     .ForMember(dest => dest.Telephone, opt => opt.MapFrom(src => src.TELEPHONE))
                     .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => (GenderType?)src.GENDER))
                     .ReverseMap()
                     .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte?)src.OrganizationType))
                     .ForPath(dest => dest.UNIT_TYPE, opt => opt.MapFrom(src => (byte)src.UnitType))
                     .ForPath(dest => dest.GENDER, opt => opt.MapFrom(src => (byte?)src.Gender))
                     .IgnoreAllNonExisting();

                CreateMap<NOTIFICATION_GROUP_USER, ConcatableUser>()
                    .ForMember(dest => dest.BUID, opt => opt.MapFrom(src => src.BU_ID))
                    .ForMember(dest => dest.BUName, opt => opt.MapFrom(src => src.BU_NAME))
                    .ForMember(dest => dest.JobName, opt => opt.MapFrom(src => src.JOB_NAME))
                    .ForMember(dest => dest.JobID, opt => opt.MapFrom(src => src.JOB_ID))
                    .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                    .ForMember(dest => dest.NodeName, opt => opt.MapFrom(src => src.NODE_NAME))
                    .ForMember(dest => dest.NotificationBehavior, opt => opt.MapFrom(src => src.NOTIFICATION_BEHAVIOR))
                    .ForMember(dest => dest.NotificationKind, opt => opt.MapFrom(src => src.NOTIFICATION_KIND))
                    .ForMember(dest => dest.NotificationRemark, opt => opt.MapFrom(src => src.NOTIFICATION_REMARK))
                    .ForMember(dest => dest.OrganizationType, opt => opt.MapFrom(src => (OrganizationType?)src.ORGANIZATION_TYPE))
                    .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.USER_ID))
                    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.USER_NAME))
                    .ForMember(dest => dest.UnitType, opt => opt.MapFrom(src => (UnitType)src.UNIT_TYPE))
                    .ForMember(dest => dest.Mobile, opt => opt.MapFrom(src => src.MOBILE))
                    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EMAIL))
                    .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.ADDRESS))
                    .ForMember(dest => dest.Telephone, opt => opt.MapFrom(src => src.TELEPHONE))
                    .ForMember(dest => dest.TelephoneBak, opt => opt.MapFrom(src => src.TELEPHONE_BAK))
                    .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => (GenderType?)src.GENDER))
                    .ReverseMap()
                    .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte?)src.OrganizationType))
                    .ForPath(dest => dest.UNIT_TYPE, opt => opt.MapFrom(src => (byte)src.UnitType))
                    .ForPath(dest => dest.GENDER, opt => opt.MapFrom(src => (byte?)src.Gender))
                    .IgnoreAllNonExisting();

                CreateMap<NOTIFICATION_GROUP_USER, OrganizationUser>()
                   .ForMember(dest => dest.BUID, opt => opt.MapFrom(src => src.BU_ID))
                   .ForMember(dest => dest.BUName, opt => opt.MapFrom(src => src.BU_NAME))
                   .ForMember(dest => dest.JobName, opt => opt.MapFrom(src => src.JOB_NAME))
                   .ForMember(dest => dest.JobID, opt => opt.MapFrom(src => src.JOB_ID))
                   .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                   .ForMember(dest => dest.NodeName, opt => opt.MapFrom(src => src.NODE_NAME))
                   .ForMember(dest => dest.OrganizationType, opt => opt.MapFrom(src => (OrganizationType?)src.ORGANIZATION_TYPE))
                   .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.USER_ID))
                   .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.USER_NAME))
                   .ForMember(dest => dest.UnitType, opt => opt.MapFrom(src => (UnitType)src.UNIT_TYPE))
                   .ReverseMap()
                   .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte?)src.OrganizationType))
                   .ForPath(dest => dest.UNIT_TYPE, opt => opt.MapFrom(src => (byte)src.UnitType))
                   .IgnoreAllNonExisting();
            }
        }

        public class NotificationGroupEffectiveSummaryProfile : AutoMapper.Profile
        {
            public NotificationGroupEffectiveSummaryProfile()
            {
                CreateMap<NOTIFICATION_GROUP_EFFECTIVE_SUMMARY, NotificationGroupEffectiveSummary>()

                     .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                     .ForMember(dest => dest.GroupID, opt => opt.MapFrom(src => src.NOTIFICATION_GROUP_ID))
                     .ForMember(dest => dest.ExpectArriveCount, opt => opt.MapFrom(src => src.EXPECT_ARRIVE_COUNT))
                     .ForMember(dest => dest.ActualArriveCount, opt => opt.MapFrom(src => src.ACTUAL_ARRIVE_COUNT))
                     .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                     .ForMember(dest => dest.IsProcess, opt => opt.MapFrom(src => src.IS_PROCESS))
                     .ForMember(dest => dest.UpdateDateTime, opt => opt.MapFrom(src => src.UPDATE_DATETIME))
                     .ForMember(dest => dest.UpdateUserName, opt => opt.MapFrom(src => src.UPDATE_USERNAME))
                     .ForMember(dest => dest.CaseIDs, opt => opt.MapFrom(src => (string.IsNullOrEmpty(src.CASE_IDs) ?
                        new string[] { } : JsonConvert.DeserializeObject<string[]>(src.CASE_IDs))))
                     .ReverseMap()
                     .ForPath(dest => dest.CASE_IDs, opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.CaseIDs ?? new string[] { })))
                     .IgnoreAllNonExisting();
            }
        }

        public class OfficialEmailGroupProfile : AutoMapper.Profile
        {
            public OfficialEmailGroupProfile()
            {
                CreateMap<OFFICIAL_EMAIL_GROUP, OfficialEmailGroup>()
                    .ForMember(dest => dest.Account, opt => opt.MapFrom(src => src.ACCOUNT))
                    .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                    .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                    .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                    .ForMember(dest => dest.KeepDay, opt => opt.MapFrom(src => src.KEEP_DAY))
                    .ForMember(dest => dest.MailAddress, opt => opt.MapFrom(src => src.MAIL_ADDRESS))
                    .ForMember(dest => dest.MailDisplayName, opt => opt.MapFrom(src => src.MAIL_DISPLAY_NAME))
                    .ForMember(dest => dest.OfficialEmail, opt => opt.MapFrom(src => src.OFFICIAL_EMAIL))
                    .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                    .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.PASSWORD))
                    .ForMember(dest => dest.AllowReceive, opt => opt.MapFrom(src => src.ALLOW_RECEIVE))
                    .ForMember(dest => dest.UpdateDateTime, opt => opt.MapFrom(src => src.UPDATE_DATETIME))
                    .ForMember(dest => dest.UpdateUserName, opt => opt.MapFrom(src => src.UPDATE_USERNAME))
                    .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.USER))
                    .ForMember(dest => dest.HostName, opt => opt.MapFrom(src => src.HOSTNAME))
                    .ForMember(dest => dest.MailProtocolType, opt => opt.MapFrom(src => (MailProtocolType)src.PROTOCOL))
                    .ForMember(dest => dest.OrganizationType, opt => opt.MapFrom(src => (OrganizationType)src.ORGANIZATION_TYPE))
                    .ForMember(dest => dest.IsEnabled, opt => opt.MapFrom(src => src.IS_ENABLED))
                    .AfterMap((src, dest) => dest.Decrypt())
                    .ReverseMap()
                    .BeforeMap((src, dest) => src.Encrypt())
                    .ForPath(dest => dest.PROTOCOL, opt => opt.MapFrom(src => (byte)src.MailProtocolType))
                    .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte)src.OrganizationType))
                    .IgnoreAllNonExisting();
            }
        }
        public class OfficialEmailEffectiveDataProfile : AutoMapper.Profile
        {
            public OfficialEmailEffectiveDataProfile()
            {
                CreateMap<OFFICIAL_EMAIL_EFFECTIVE_DATA, OfficialEmailEffectivePayload>()
                    .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                    .ForMember(dest => dest.OrganizationType, opt => opt.MapFrom(src => (OrganizationType)src.ORGANIZATION_TYPE))
                    .ForMember(dest => dest.MessageID, opt => opt.MapFrom(src => src.MESSAGE_ID))
                    .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.SUBJECT))
                    .ForMember(dest => dest.FromName, opt => opt.MapFrom(src => src.FROM_NAME))
                    .ForMember(dest => dest.FromAddress, opt => opt.MapFrom(src => src.FROM_ADDRESS))
                    .ForMember(dest => dest.Body, opt => opt.MapFrom(src => src.BODY))
                    .ForMember(dest => dest.ReceivedDateTime, opt => opt.MapFrom(src => src.RECEIVED_DATETIME))
                    .ForMember(dest => dest.FilePath, opt => opt.MapFrom(src => src.FILE_PATH))
                    .ForMember(dest => dest.Body, opt => opt.MapFrom(src => src.BODY))
                    .ForMember(dest => dest.JContent, opt => opt.MapFrom(src => src.J_CONTENT))
                    .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                    .ForMember(dest => dest.CaseID, opt => opt.MapFrom(src => src.CASE_ID))
                    .ForMember(dest => dest.Email_Group_ID, opt => opt.MapFrom(src => src.EMAIL_GROUP_ID))
                    .ForMember(dest => dest.HasAttachment, opt => opt.MapFrom(src => src.HAS_ATTACHMENT))
                    .ReverseMap()
                    .ForMember(dest => dest.J_CONTENT, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.JContent) ? null : src.JContent))
                    .IgnoreAllNonExisting();
            }
        }
        public class OfficialEmailHistoryProfile : AutoMapper.Profile
        {
            public OfficialEmailHistoryProfile()
            {
                CreateMap<OFFICIAL_EMAIL_HISTORY, OfficialEmailHistory>()
                    .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                    .ForMember(dest => dest.MessageID, opt => opt.MapFrom(src => src.MESSAGE_ID))
                    .ForMember(dest => dest.EmailGroupID, opt => opt.MapFrom(src => src.EMAIL_GROUP_ID))
                    .ForMember(dest => dest.DownloadDateTime, opt => opt.MapFrom(src => src.DOWNLOAD_DATETIME))
                    .ForMember(dest => dest.OrganizationType, opt => opt.MapFrom(src => (OrganizationType)src.ORGANIZATION_TYPE))
                    .ReverseMap()
                    .IgnoreAllNonExisting();
            }
        }

        public class PersonalNotificationProfile : AutoMapper.Profile
        {
            public PersonalNotificationProfile()
            {
                CreateMap<PERSONAL_NOTIFICATION, PersonalNotification>()
                     .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                     .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.CONTENT))
                     .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                     .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                     .ForMember(dest => dest.PersonalNotificationType, opt => opt.MapFrom(src => (PersonalNotificationType)src.PERSONAL_NOTIFICATION_TYPE))
                     .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.USER_ID))
                     .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.USER))
                     .ForMember(dest => dest.Extend, opt => opt.MapFrom(src => src.EXTEND))
                     .ReverseMap()
                     .ForPath(dest => dest.PERSONAL_NOTIFICATION_TYPE, opt => opt.MapFrom(src => (byte)src.PersonalNotificationType))
                     .IgnoreAllNonExisting();
            }
        }

        public class PPCLifeEffectiveSummaryProfile : AutoMapper.Profile
        {
            public PPCLifeEffectiveSummaryProfile()
            {
                CreateMap<PPCLIFE_EFFECTIVE_SUMMARY, PPCLifeEffectiveSummary>()
                     .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                     .ForMember(dest => dest.ItemID, opt => opt.MapFrom(src => src.ITEM_ID))
                     .ForMember(dest => dest.PPCLifeArriveType, opt => opt.MapFrom(src => (PPCLifeArriveType)src.ARRIVE_TYPE))
                     .ForMember(dest => dest.BatchNo, opt => opt.MapFrom(src => src.BATCH_NO))
                     .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                     .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                     .ForMember(dest => dest.CasePPCLifes, opt => opt.MapFrom(src => src.CASE_PPCLIFE))
                     .ReverseMap()
                     .ForPath(dest => dest.ARRIVE_TYPE, opt => opt.MapFrom(src => (byte)src.PPCLifeArriveType))
                     .IgnoreAllNonExisting();
            }
        }

        public class PPCLifeResumeProfile : AutoMapper.Profile
        {
            public PPCLifeResumeProfile()
            {
                CreateMap<PPCLIFE_RESUME, PPCLifeResume>()
                     .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                     .ForMember(dest => dest.PPCLifeArriveType, opt => opt.MapFrom(src => (PPCLifeArriveType)src.ARRIVE_TYPE))
                     .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.CONTENT))
                     .ForMember(dest => dest.NotificationGroupResultType, opt => opt.MapFrom(src => (NotificationGroupResultType)src.TYPE))
                     .ForMember(dest => dest.ItemCode, opt => opt.MapFrom(src => src.ITEM_CODE))
                     .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.ITEM_NAME))
                     .ForMember(dest => dest.BatchNo, opt => opt.MapFrom(src => src.BATCH_NO))
                     .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                     .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                     .ForMember(dest => dest.EMLFilePath, opt => opt.MapFrom(src => src.EML_FILE_PATH))
                     .ReverseMap()
                     .ForPath(dest => dest.TYPE, opt => opt.MapFrom(src => (byte)src.NotificationGroupResultType))
                     .ForPath(dest => dest.ARRIVE_TYPE, opt => opt.MapFrom(src => (byte)src.PPCLifeArriveType))
                     .IgnoreAllNonExisting();
            }
        }

    }
}
