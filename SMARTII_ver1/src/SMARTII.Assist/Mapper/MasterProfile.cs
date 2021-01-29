using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;

namespace SMARTII.Assist.Mapper
{
    public static class MasterProfile
    {
        public class QuestionClassificationProfile : AutoMapper.Profile
        {
            public QuestionClassificationProfile()
            {
                CreateMap<QUESTION_CLASSIFICATION, QuestionClassification>()
                     .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                     .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                     .ForMember(dest => dest.UpdateDateTime, opt => opt.MapFrom(src => src.UPDATE_DATETIME))
                     .ForMember(dest => dest.UpdateUserName, opt => opt.MapFrom(src => src.UPDATE_USERNAME))
                     .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                     .ForMember(dest => dest.IsEnabled, opt => opt.MapFrom(src => src.IS_ENABLED))
                     .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NAME))
                     .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                     .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.ORDER))
                     .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.CODE))
                     .ForMember(dest => dest.OrganizationType, opt => opt.MapFrom(src => (OrganizationType)src.ORGANIZATION_TYPE))
                     .ForMember(dest => dest.ParentID, opt => opt.MapFrom(src => src.PARENT_ID))
                     .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.LEVEL))
                     .ForMember(dest => dest.Children, opt => opt.MapFrom(src => src.QUESTION_CLASSIFICATION1))
                     .ForMember(dest => dest.Parent, opt => opt.MapFrom(src => src.QUESTION_CLASSIFICATION2))
                     .ForMember(dest => dest.QuesionClassificationAnswer, opt => opt.MapFrom(src => src.QUESTION_CLASSIFICATION_ANSWER))
                     .ForMember(dest => dest.QuesionClassificationGuide, opt => opt.MapFrom(src => src.QUESTION_CLASSIFICATION_GUIDE))
                     .ReverseMap()
                     .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte)src.OrganizationType))
                     .IgnoreAllNonExisting();
                
            }
        }

        public class QuestionClassificationAnswerProfile : AutoMapper.Profile
        {
            public QuestionClassificationAnswerProfile()
            {
                CreateMap<QUESTION_CLASSIFICATION_ANSWER, QuestionClassificationAnswer>()
                     .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                     .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                     .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                     .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.CONTENT))
                     .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.TITLE))
                     .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                     .ForMember(dest => dest.ClassificationID, opt => opt.MapFrom(src => src.CLASSIFICATION_ID))
                     .ForMember(dest => dest.OrganizationType, opt => opt.MapFrom(src => (OrganizationType)src.ORGANIZATION_TYPE))
                     .ForMember(dest => dest.UpdateDateTime, opt => opt.MapFrom(src => src.UPDATE_DATETIME))
                     .ForMember(dest => dest.UpdateUserName, opt => opt.MapFrom(src => src.UPDATE_USERNAME))
                     .ReverseMap()
                     .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte)src.OrganizationType))
                     .IgnoreAllNonExisting();
            }
        }

        public class KMClassificationProfile : AutoMapper.Profile
        {
            public KMClassificationProfile()
            {
                CreateMap<KM_CLASSIFICATION, KMClassification>()

                     .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                     .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NAME))
                     .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                     .ForMember(dest => dest.OrganizationType, opt => opt.MapFrom(src => (OrganizationType)src.ORGANIZATION_TYPE))
                     .ForMember(dest => dest.ParentID, opt => opt.MapFrom(src => src.PARENT_ID))
                     .ForMember(dest => dest.Children, opt => opt.MapFrom(src => src.KM_CLASSIFICATION1))
                     .ForMember(dest => dest.Parent, opt => opt.MapFrom(src => src.KM_CLASSIFICATION2))
                     .ForMember(dest => dest.KMDatas, opt => opt.MapFrom(src => src.KM_DATA))
                     .ReverseMap()
                     .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte)src.OrganizationType))
                     .IgnoreAllNonExisting();

                CreateMap<KM_CLASSIFICATION, IRecursivelyModel>() 
                        .IgnoreAllNonExisting();
               
            }
        }


        public class VW_KMClassificationProfile : AutoMapper.Profile
        {
            public VW_KMClassificationProfile()
            {
                CreateMap<VW_KM_CLASSIFICATION_NESTED, KMClassification>()                
                     .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                     .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NAME))
                     .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                     .ForMember(dest => dest.OrganizationType, opt => opt.MapFrom(src => (OrganizationType)src.ORGANIZATION_TYPE))
                     .ForMember(dest => dest.ParentID, opt => opt.MapFrom(src => src.PARENT_ID))                    
                     .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.LEVEL))
                     .ForMember(dest => dest.ParentNamePath, opt => opt.MapFrom(src => src.PARENT_PATH_NAME))
                     .ForMember(dest => dest.ParentPath, opt => opt.MapFrom(src => src.PARENT_PATH))
                     .IgnoreAllNonExisting();
            }
        }
        public class VW_QuestionClassificationProfileAnswer : AutoMapper.Profile
        {
            public VW_QuestionClassificationProfileAnswer()
            {
                CreateMap<VW_QUESTION_CLASSIFICATION_ANSWER_NESTED, QuestionClassificationAnswer>()
                     .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ANSWER_ID))
                     .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.TITLE))
                     .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.CONTENT))
                     .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                     .ForMember(dest => dest.NodeName, opt => opt.MapFrom(src => src.NODE_NAME))
                     .ForMember(dest => dest.ParentPath, opt => opt.MapFrom(src => src.PARENT_PATH))
                     .ForMember(dest => dest.ParentPathName, opt => opt.MapFrom(src => src.PARENT_PATH_NAME))
                     .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                     .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                     .ForMember(dest => dest.UpdateDateTime, opt => opt.MapFrom(src => src.UPDATE_DATETIME))
                     .ForMember(dest => dest.UpdateUserName, opt => opt.MapFrom(src => src.UPDATE_USERNAME))
                     .IgnoreAllNonExisting();
            }
        }
        public class VW_QuestionClassificationGuideProfile : AutoMapper.Profile
        {
            public VW_QuestionClassificationGuideProfile()
            {
                CreateMap<VW_QUESTION_CLASSIFICATION_GUIDE_NESTED, QuestionClassificationGuide>()
                     .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                     .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.CONTENT))
                     .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                     .ForMember(dest => dest.NodeName, opt => opt.MapFrom(src => src.NODE_NAME))
                     .ForMember(dest => dest.ClassificationID, opt => opt.MapFrom(src => src.CLASSIFICATION_ID))
                     .ForMember(dest => dest.ClassificationName, opt => opt.MapFrom(src => src.CLASSIFICATION_NAME))
                     .ForMember(dest => dest.ParentPath, opt => opt.MapFrom(src => src.PARENT_PATH))
                     .ForMember(dest => dest.ParentPathName, opt => opt.MapFrom(src => src.PARENT_PATH_NAME))
                     .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                     .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                     .ForMember(dest => dest.UpdateDateTime, opt => opt.MapFrom(src => src.UPDATE_DATETIME))
                     .ForMember(dest => dest.UpdateUserName, opt => opt.MapFrom(src => src.UPDATE_USERNAME))
                     .ReverseMap()
                     .IgnoreAllNonExisting();
            }
        }

        


        public class KMDataProfile : AutoMapper.Profile
        {
            public KMDataProfile()
            {
                CreateMap<KM_DATA, KMData>()

                     .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                     .ForMember(dest => dest.ClassificationID, opt => opt.MapFrom(src => src.CLASSIFICATION_ID))
                     .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.TITLE))
                     .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.CONTENT))
                     .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                     .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                     .ForMember(dest => dest.UpdateUserName, opt => opt.MapFrom(src => src.UPDATE_USERNAME))
                     .ForMember(dest => dest.UpdateDateTime, opt => opt.MapFrom(src => src.UPDATE_DATETIME))
                     .ForMember(dest => dest.KMClassification, opt => opt.MapFrom(src => src.KM_CLASSIFICATION))
                     .ForMember(dest => dest.FilePaths, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.FILE_PATH) ? new string[] { } :
                        JsonConvert.DeserializeObject<string[]>(src.FILE_PATH)))
                     .ReverseMap()
                     .ForPath(dest => dest.FILE_PATH, opt => opt.MapFrom(src => src.FilePaths == null ?
                        JsonConvert.SerializeObject(new string[] { }) : JsonConvert.SerializeObject(src.FilePaths)))
                     .IgnoreAllNonExisting();
            }
        }


        public class UserParameterProfile : AutoMapper.Profile
        {
            public UserParameterProfile()
            {
                CreateMap<USER_PARAMETER, UserParameter>()
                     .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.USER_ID))
                     .ForMember(dest => dest.NavigateOfNewbie, opt => opt.MapFrom(src => src.NAVIGATE_OF_NEWBIE))
                     .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.IMAGE_PATH))
                     .ForMember(dest => dest.NoticeOfWebsite, opt => opt.MapFrom(src => src.NOTICE_OF_WEBSITE))
                     .ForMember(dest => dest.FavoriteFeature, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.FAVORITE_FEATURE) ?
                        new List<PageAuthFavorite>() : JsonConvert.DeserializeObject<List<PageAuthFavorite>>(src.FAVORITE_FEATURE)))
                     .ReverseMap()
                     .ForPath(dest => dest.FAVORITE_FEATURE, opt => opt.MapFrom(src => src.FavoriteFeature == null ?
                        JsonConvert.SerializeObject(new PageAuthFavorite[] { }) : JsonConvert.SerializeObject(src.FavoriteFeature.ToArray())))
                     .IgnoreAllNonExisting();
            }
        }

        

        public class CustomerProfile : AutoMapper.Profile
        {
            public CustomerProfile()
            {
                CreateMap<CUSTOMER, Customer>()
                    .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                    .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.ADDRESS))
                    .ForMember(dest => dest.BUID, opt => opt.MapFrom(src => src.BU_ID))
                    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EMAIL))
                    .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => (GenderType)src.GENDER))
                    .ForMember(dest => dest.MemberID, opt => opt.MapFrom(src => src.MEMBER_ID))
                    .ForMember(dest => dest.Mobile, opt => opt.MapFrom(src => src.MOBILE))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NAME))
                    .ForMember(dest => dest.Telephone, opt => opt.MapFrom(src => src.TELEPHONE))
                    .ReverseMap()
                    .ForPath(dest => dest.GENDER, opt => opt.MapFrom(src => (byte)src.Gender))
                    .IgnoreAllNonExisting();
            }
        }

        public class GuideProfile : AutoMapper.Profile
        {
            public GuideProfile()
            {
                CreateMap<QUESTION_CLASSIFICATION_GUIDE, QuestionClassificationGuide>()
                    .ForMember(dest => dest.ClassificationID, opt => opt.MapFrom(src => src.CLASSIFICATION_ID))
                    .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.CONTENT))
                    .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                    .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                    .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                    .ForMember(dest => dest.UpdateDateTime, opt => opt.MapFrom(src => src.UPDATE_DATETIME))
                    .ForMember(dest => dest.UpdateUserName, opt => opt.MapFrom(src => src.UPDATE_USERNAME))
                    .ForMember(dest => dest.OrganizationType, opt => opt.MapFrom(src => (OrganizationType)src.ORGANIZATION_TYPE))
                    .ForMember(dest => dest.QuestionClassification, opt => opt.MapFrom(src => src.QUESTION_CLASSIFICATION))
                    .ReverseMap()
                    .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte)src.OrganizationType))
                    .IgnoreAllNonExisting();
            }
        }

        public class QueueProfile : AutoMapper.Profile
        {
            public QueueProfile()
            {
                CreateMap<QUEUE, Queue>()
                    .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                    .ForMember(dest => dest.IsEnabled, opt => opt.MapFrom(src => src.IS_ENABLED))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NAME))
                    .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                    .ForMember(dest => dest.OrganizationType, opt => opt.MapFrom(src => (OrganizationType)src.ORGANIZATION_TYPE))
                    .ForMember(dest => dest.CallCenterNode, opt => opt.MapFrom(src => src.CALLCENTER_NODE))
                    .ReverseMap()
                    .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte)src.OrganizationType))
                    .IgnoreAllNonExisting();
            }
        }

        public class BillboardProfile : AutoMapper.Profile
        {
            public BillboardProfile()
            {
                CreateMap<BILL_BOARD, Billboard>()
                    .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                    .ForMember(dest => dest.ActiveEndDateTime, opt => opt.MapFrom(src => src.ACTIVE_DATE_END))
                    .ForMember(dest => dest.ActiveStartDateTime, opt => opt.MapFrom(src => src.ACTIVE_DATE_START))
                    .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.CONTENT))
                    .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.TITLE))
                    .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                    .ForMember(dest => dest.CreateUserID, opt => opt.MapFrom(src => src.CREATE_USER_ID))
                    .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                    .ForMember(dest => dest.UpdateDateTime, opt => opt.MapFrom(src => src.UPDATE_DATETIME))
                    .ForMember(dest => dest.UpdateUserID, opt => opt.MapFrom(src => src.UPDATE_USER_ID))
                    .ForMember(dest => dest.UpdateUserName, opt => opt.MapFrom(src => src.UPDATE_USERNAME))
                    .ForMember(dest => dest.IsNotificaed, opt => opt.MapFrom(src => src.IS_NOTIFICATED))
                    .ForMember(dest => dest.BillboardWarningType, opt => opt.MapFrom(src => (BillboardWarningType)src.WARNING_TYPE))
                    .ForMember(dest => dest.FilePaths, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.FILE_PATH) ? new string[] { } :
                        JsonConvert.DeserializeObject<string[]>(src.FILE_PATH)))
                    .ForMember(dest => dest.UserIDs, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.USER_IDs) ? new string[] { } :
                        JsonConvert.DeserializeObject<string[]>(src.USER_IDs)))
                    .ReverseMap()
                    .ForPath(dest => dest.WARNING_TYPE, opt => opt.MapFrom(src => (byte)src.BillboardWarningType))
                    .ForPath(dest => dest.FILE_PATH, opt => opt.MapFrom(src => src.FilePaths == null ?
                        JsonConvert.SerializeObject(new string[] { }) : JsonConvert.SerializeObject(src.FilePaths)))
                     .ForPath(dest => dest.USER_IDs, opt => opt.MapFrom(src => src.UserIDs == null ?
                        JsonConvert.SerializeObject(new string[] { }) : JsonConvert.SerializeObject(src.UserIDs)))
                    .IgnoreAllNonExisting();
            }
        }

        public class WorkScheduleProfile : AutoMapper.Profile
        {
            public WorkScheduleProfile()
            {
                CreateMap<WORK_SCHEDULE, WorkSchedule>()
                    .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                    .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                    .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                    .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.DATE))
                    .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                    .ForMember(dest => dest.OrganizationType, opt => opt.MapFrom(src => (OrganizationType)src.ORGANIZATION_TYPE))
                    .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.TITLE))
                    .ForMember(dest => dest.UpdateDateTime, opt => opt.MapFrom(src => src.UPDATE_DATETIME))
                    .ForMember(dest => dest.UpdateUserName, opt => opt.MapFrom(src => src.UPDATE_USERNAME))
                    .ForMember(dest => dest.WorkType, opt => opt.MapFrom(src => (WorkType)src.WORK_TYPE))
                    .ReverseMap()
                    .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte)src.OrganizationType))
                    .ForPath(dest => dest.WORK_TYPE, opt => opt.MapFrom(src => (byte)src.WorkType))
                    .IgnoreAllNonExisting();
            }
        }

        public class VW_QuestionClassificationProfile : AutoMapper.Profile
        {
            public VW_QuestionClassificationProfile()
            {
                CreateMap<VW_QUESTION_CLASSIFICATION_NESTED, QuestionClassification>()
                     .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                     .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                     .ForMember(dest => dest.UpdateDateTime, opt => opt.MapFrom(src => src.UPDATE_DATETIME))
                     .ForMember(dest => dest.UpdateUserName, opt => opt.MapFrom(src => src.UPDATE_USERNAME))
                     .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                     .ForMember(dest => dest.IsEnabled, opt => opt.MapFrom(src => src.IS_ENABLED))
                     .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NAME))
                     .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                     .ForMember(dest => dest.OrganizationType, opt => opt.MapFrom(src => (OrganizationType)src.ORGANIZATION_TYPE))
                     .ForMember(dest => dest.ParentID, opt => opt.MapFrom(src => src.PARENT_ID))
                     .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.LEVEL))
                     .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.ORDER))
                     .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.CODE))
                     .ForMember(dest => dest.FirstCode, opt => opt.MapFrom(src => src.FRISTCODE))
                     .ForMember(dest => dest.ParentNamePath, opt => opt.MapFrom(src => src.PARENT_PATH_NAME))
                     .ForMember(dest => dest.ParentPath, opt => opt.MapFrom(src => src.PARENT_PATH))
                     .IgnoreAllNonExisting();
            }
        }
    }
}
