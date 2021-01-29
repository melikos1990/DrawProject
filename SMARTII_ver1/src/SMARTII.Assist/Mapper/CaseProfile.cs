using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using AutoMapper;
using Newtonsoft.Json;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Case;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Security;

namespace SMARTII.Assist.Mapper
{
    public static class CaseProfile
    {
        public class _CaseProfile : AutoMapper.Profile
        {
            public _CaseProfile()
            {

                CreateMap<CASE, Case>()
                     .ForMember(dest => dest.GroupID, opt => opt.MapFrom(src => src.GROUP_ID))
                     .ForMember(dest => dest.CaseID, opt => opt.MapFrom(src => src.CASE_ID))
                     .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                     .ForMember(dest => dest.OrganizationType, opt => opt.MapFrom(src => (OrganizationType)src.ORGANIZATION_TYPE))
                     .ForMember(dest => dest.SourceID, opt => opt.MapFrom(src => src.SOURCE_ID))
                     .ForMember(dest => dest.ApplyUserName, opt => opt.MapFrom(src => src.APPLY_USERNAME))
                     .ForMember(dest => dest.ApplyDateTime, opt => opt.MapFrom(src => src.APPLY_DAETTIME))
                     .ForMember(dest => dest.ApplyUserID, opt => opt.MapFrom(src => src.APPLY_USER_ID))
                     .ForMember(dest => dest.GroupID, opt => opt.MapFrom(src => src.GROUP_ID))
                     .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.CONTENT))
                     .ForMember(dest => dest.CaseType, opt => opt.MapFrom(src => (CaseType)src.CASE_TYPE))
                     .ForMember(dest => dest.PromiseDateTime, opt => opt.MapFrom(src => src.PROMISE_DATETIME))
                     .ForMember(dest => dest.ExpectDateTime, opt => opt.MapFrom(src => src.EXPECT_DATETIME))
                     .ForMember(dest => dest.FilePath, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.FILE_PATH) ? new string[] { } : JsonConvert.DeserializeObject<string[]>(src.FILE_PATH)))
                     .ForMember(dest => dest.FinishContent, opt => opt.MapFrom(src => src.FINISH_CONTENT))
                     .ForMember(dest => dest.FinishFilePath, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.FINISH_FILE_PATH) ? new string[] { } : JsonConvert.DeserializeObject<string[]>(src.FINISH_FILE_PATH)))
                     .ForMember(dest => dest.FinishDateTime, opt => opt.MapFrom(src => src.FINISH_DATETIME))
                     .ForMember(dest => dest.FinishUserName, opt => opt.MapFrom(src => src.FINISH_USERNAME))
                     .ForMember(dest => dest.FinishEMLFilePath, opt => opt.MapFrom(src => src.FINISH_EML_FILE_PATH))
                     .ForMember(dest => dest.FinishReplyDateTime, opt => opt.MapFrom(src => src.FINISH_REPLY_DATETIME))
                     .ForMember(dest => dest.IsAttension, opt => opt.MapFrom(src => src.IS_ATTENSION))
                     .ForMember(dest => dest.IsReport, opt => opt.MapFrom(src => src.IS_REPORT))
                     .ForMember(dest => dest.QuestionClassificationID, opt => opt.MapFrom(src => src.QUESION_CLASSIFICATION_ID))
                     .ForMember(dest => dest.RelationCaseIDs, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.RELATION_CASE_IDs) ? new string[] { } : JsonConvert.DeserializeObject<string[]>(src.RELATION_CASE_IDs)))
                     .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                     .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                     .ForMember(dest => dest.UpdateUserName, opt => opt.MapFrom(src => src.UPDATE_USERNAME))
                     .ForMember(dest => dest.UpdateDateTime, opt => opt.MapFrom(src => src.UPDATE_DATETIME))
                     .ForMember(dest => dest.EMLFilePath, opt => opt.MapFrom(src => src.EML_FILE_PATH))
                     .ForMember(dest => dest.CaseAssignments, opt => opt.MapFrom(src => src.CASE_ASSIGNMENT))
                     .ForMember(dest => dest.ComplaintNotice, opt => opt.MapFrom(src => src.CASE_ASSIGNMENT_COMPLAINT_NOTICE))
                     .ForMember(dest => dest.ComplaintInvoice, opt => opt.MapFrom(src => src.CASE_ASSIGNMENT_COMPLAINT_INVOICE))
                     .ForMember(dest => dest.CaseAssignmentCommunicates, opt => opt.MapFrom(src => src.CASE_ASSIGNMENT_COMMUNICATE))
                     .ForMember(dest => dest.CaseComplainedUsers, opt => opt.MapFrom(src => src.CASE_COMPLAINED_USER))
                     .ForMember(dest => dest.CaseConcatUsers, opt => opt.MapFrom(src => src.CASE_CONCAT_USER))
                     .ForMember(dest => dest.CaseFinishReasonDatas, opt => opt.MapFrom(src => src.CASE_FINISH_REASON_DATA))
                     .ForMember(dest => dest.CaseReminds, opt => opt.MapFrom(src => src.CASE_REMIND))

                     .ForMember(dest => dest.CaseSource, opt => opt.MapFrom(src => src.CASE_SOURCE))
                     .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.CASE_ITEM))
                     .ForMember(dest => dest.CaseWarning, opt => opt.MapFrom(src => src.CASE_WARNING))
                     .ForMember(dest => dest.CaseWarningID, opt => opt.MapFrom(src => src.CASE_WARNING_ID))
                     .ForMember(dest => dest.CaseTags, opt => opt.MapFrom(src => src.CASE_TAG))
                     .ForMember(dest => dest.JContent, opt => opt.MapFrom(src => src.J_CONTENT))

                     .ReverseMap()
                     .ForPath(dest => dest.CASE_TAG, opt => opt.MapFrom(src => default(IEnumerable<CASE_TAG>)))
                     .ForPath(dest => dest.CASE_TYPE, opt => opt.MapFrom(src => (byte)src.CaseType))
                     .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte)src.OrganizationType))
                     .ForPath(dest => dest.FILE_PATH, opt => opt.MapFrom(src => src.FilePath == null ? JsonConvert.SerializeObject(new string[] { }) : JsonConvert.SerializeObject(src.FilePath)))
                     .ForPath(dest => dest.FINISH_FILE_PATH, opt => opt.MapFrom(src => src.FinishFilePath == null ? JsonConvert.SerializeObject(new string[] { }) : JsonConvert.SerializeObject(src.FinishFilePath)))
                     .ForPath(dest => dest.RELATION_CASE_IDs, opt => opt.MapFrom(src => src.RelationCaseIDs == null ? JsonConvert.SerializeObject(new string[] { }) : JsonConvert.SerializeObject(src.RelationCaseIDs)))
                     .IgnoreAllNonExisting();
            }
        }

        public class CaseAssignmentProfile : AutoMapper.Profile
        {
            public CaseAssignmentProfile()
            {
                CreateMap<CASE_ASSIGNMENT, CaseAssignment>()
                     .ForMember(dest => dest.AssignmentID, opt => opt.MapFrom(src => src.ASSIGNMENT_ID))
                     .ForMember(dest => dest.CaseID, opt => opt.MapFrom(src => src.CASE_ID))
                     .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                     .ForMember(dest => dest.OrganizationType, opt => opt.MapFrom(src => (OrganizationType)src.ORGANIZATION_TYPE))
                     .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.CONTENT))
                     .ForMember(dest => dest.FilePath, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.FILE_PATH) ? new string[] { } : JsonConvert.DeserializeObject<string[]>(src.FILE_PATH)))
                     .ForMember(dest => dest.FinishContent, opt => opt.MapFrom(src => src.FINISH_CONTENT))
                     .ForMember(dest => dest.FinishFilePath, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.FINISH_FILE_PATH) ? new string[] { } : JsonConvert.DeserializeObject<string[]>(src.FINISH_FILE_PATH)))
                     .ForMember(dest => dest.FinishDateTime, opt => opt.MapFrom(src => src.FINISH_DATETIME))
                     .ForMember(dest => dest.FinishUserName, opt => opt.MapFrom(src => src.FINISH_USERNAME))
                     .ForMember(dest => dest.NotificationBehaviors, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.NOTIFICATION_BEHAVIORS) ? new string[] { } : JsonConvert.DeserializeObject<string[]>(src.NOTIFICATION_BEHAVIORS)))
                     .ForMember(dest => dest.NotificationDateTime, opt => opt.MapFrom(src => src.NOTICE_DATETIME))
                     .ForMember(dest => dest.CaseAssignmentType, opt => opt.MapFrom(src => (CaseAssignmentType)src.CASE_ASSIGNMENT_TYPE))
                     .ForMember(dest => dest.RejectType, opt => opt.MapFrom(src => (RejectType)src.REJECT_TYPE))
                     .ForMember(dest => dest.RejectReason, opt => opt.MapFrom(src => src.REJECT_REASON))
                     .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                     .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                     .ForMember(dest => dest.UpdateDateTime, opt => opt.MapFrom(src => src.UPDATE_DATETIME))
                     .ForMember(dest => dest.UpdateUserName, opt => opt.MapFrom(src => src.UPDATE_USERNAME))
                     .ForMember(dest => dest.EMLFilePath, opt => opt.MapFrom(src => src.EML_FILE_PATH))
                     .ForMember(dest => dest.RecallTimes, opt => opt.MapFrom(src => src.RECALL_TIMEs))
                     .ForMember(dest => dest.FinishNodeID, opt => opt.MapFrom(src => src.FINISH_NODE_ID))
                     .ForMember(dest => dest.FinishNodeName, opt => opt.MapFrom(src => src.FINISH_NODE_NAME))
                     .ForMember(dest => dest.FinishOrganizationType, opt => opt.MapFrom(src => (OrganizationType?)src.FINISH_ORGANIZATION_TYPE))
                     .ForMember(dest => dest.Case, opt => opt.MapFrom(src => src.CASE))
                     .ForMember(dest => dest.NoticeUsers, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.NOTICE_USERs) ? new string[] { } : JsonConvert.DeserializeObject<string[]>(src.NOTICE_USERs)))
                     .ForMember(dest => dest.CaseAssignmentConcatUsers, opt => opt.MapFrom(src => src.CASE_ASSIGNMENT_CONCAT_USER))
                     .ForMember(dest => dest.CaseAssignmentUsers, opt => opt.MapFrom(src => src.CASE_ASSIGNMENT_USER))
                     .ReverseMap()
                     .ForPath(dest => dest.FINISH_ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte?)src.FinishOrganizationType))
                     .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte)src.OrganizationType))
                     .ForPath(dest => dest.REJECT_TYPE, opt => opt.MapFrom(src => (byte)src.RejectType))
                     .ForPath(dest => dest.CASE_ASSIGNMENT_TYPE, opt => opt.MapFrom(src => (byte)src.CaseAssignmentType))
                     .ForPath(dest => dest.NOTIFICATION_BEHAVIORS, opt => opt.MapFrom(src => src.NotificationBehaviors == null ? JsonConvert.SerializeObject(new string[] { }) : JsonConvert.SerializeObject(src.NotificationBehaviors)))
                     .ForPath(dest => dest.FILE_PATH, opt => opt.MapFrom(src => src.FilePath == null ? JsonConvert.SerializeObject(new string[] { }) : JsonConvert.SerializeObject(src.FilePath)))
                     .ForPath(dest => dest.FINISH_FILE_PATH, opt => opt.MapFrom(src => src.FinishFilePath == null ? JsonConvert.SerializeObject(new string[] { }) : JsonConvert.SerializeObject(src.FinishFilePath)))
                     .ForPath(dest => dest.NOTICE_USERs, opt => opt.MapFrom(src => src.NoticeUsers == null ? JsonConvert.SerializeObject(new string[] { }) : JsonConvert.SerializeObject(src.NoticeUsers)))
                     .IgnoreAllNonExisting();
            }
        }


        public class CaseAssignmentCommunicateProfile : AutoMapper.Profile
        {
            public CaseAssignmentCommunicateProfile()
            {
                CreateMap<CASE_ASSIGNMENT_COMMUNICATE, CaseAssignmentCommunicate>()
                     .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                     .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.CASE_ASSIGNMENT_COMMUNICATE_USER))
                     .ForMember(dest => dest.Case, opt => opt.MapFrom(src => src.CASE))
                     .ForMember(dest => dest.CaseID, opt => opt.MapFrom(src => src.CASE_ID))
                     .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.CONTENT))
                     .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                     .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                     .ForMember(dest => dest.EMLFilePath, opt => opt.MapFrom(src => src.EML_FILE_PATH))
                     .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                     .ForMember(dest => dest.NotificationDateTime, opt => opt.MapFrom(src => src.NOTICE_DATETIME))
                     .ForMember(dest => dest.OrganizationType, opt => opt.MapFrom(src => (OrganizationType)src.ORGANIZATION_TYPE))
                     .ReverseMap()
                     .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte)src.OrganizationType))

                     .IgnoreAllNonExisting();
            }
        }

        public class CaseAssignmentCommunicateUserProfile : AutoMapper.Profile
        {
            public CaseAssignmentCommunicateUserProfile()
            {
                CreateMap<CASE_ASSIGNMENT_COMMUNICATE_USER, CaseAssignmentCommunicateUser>()
                     .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.ADDRESS))
                     .ForMember(dest => dest.BUID, opt => opt.MapFrom(src => src.BU_ID))
                     .ForMember(dest => dest.CommunicateID, opt => opt.MapFrom(src => src.COMMUNICATE_ID))
                     .ForMember(dest => dest.Communicate, opt => opt.MapFrom(src => src.CASE_ASSIGNMENT_COMMUNICATE))
                     .ForMember(dest => dest.BUName, opt => opt.MapFrom(src => src.BU_NAME))
                     .ForMember(dest => dest.CaseID, opt => opt.MapFrom(src => src.CASE_ID))
                     .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EMAIL))
                     .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => (GenderType?)src.GENDER))
                     .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                     .ForMember(dest => dest.StoreNo, opt => opt.MapFrom(src => src.STORE_NO))
                     .ForMember(dest => dest.JobID, opt => opt.MapFrom(src => src.JOB_ID))
                     .ForMember(dest => dest.JobName, opt => opt.MapFrom(src => src.JOB_NAME))
                     .ForMember(dest => dest.Mobile, opt => opt.MapFrom(src => src.MOBILE))
                     .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                     .ForMember(dest => dest.NodeName, opt => opt.MapFrom(src => src.NODE_NAME))
                     .ForMember(dest => dest.NotificationBehavior, opt => opt.MapFrom(src => src.NOTIFICATION_BEHAVIOR))
                     .ForMember(dest => dest.NotificationKind, opt => opt.MapFrom(src => src.NOTIFICATION_KIND))
                     .ForMember(dest => dest.NotificationRemark, opt => opt.MapFrom(src => src.NOTIFICATION_REMARK))
                     .ForMember(dest => dest.OrganizationType, opt => opt.MapFrom(src => (OrganizationType?)src.ORGANIZATION_TYPE))
                     .ForMember(dest => dest.Telephone, opt => opt.MapFrom(src => src.TELEPHONE))
                     .ForMember(dest => dest.ParentPathName, opt => opt.MapFrom(src => src.PARENT_PATH_NAME))
                     .ForMember(dest => dest.TelephoneBak, opt => opt.MapFrom(src => src.TELEPHONE_BAK))
                     .ForMember(dest => dest.UnitType, opt => opt.MapFrom(src => (UnitType)src.UNIT_TYPE))
                     .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.USER_ID))
                     .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.USER_NAME))
                     .AfterMap((src, dest) => dest.Decrypt())
                     .ReverseMap()
                     .BeforeMap((src, dest) => src.Encrypt())
                     .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte?)src.OrganizationType))
                     .ForPath(dest => dest.GENDER, opt => opt.MapFrom(src => (byte?)src.Gender))
                     .ForPath(dest => dest.UNIT_TYPE, opt => opt.MapFrom(src => (byte)src.UnitType))
                     .IgnoreAllNonExisting();


                CreateMap<CASE_ASSIGNMENT_COMPLAINT_NOTICE_USER, ConcatableUser>()
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
                 .ForMember(dest => dest.ParentPathName, opt => opt.MapFrom(src => src.PARENT_PATH_NAME))
                 .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EMAIL))
                 .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.ADDRESS))
                 .ForMember(dest => dest.Telephone, opt => opt.MapFrom(src => src.TELEPHONE))
                 .ForMember(dest => dest.TelephoneBak, opt => opt.MapFrom(src => src.TELEPHONE_BAK))
                 .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => (GenderType?)src.GENDER))
                 .AfterMap((src, dest) => dest.Decrypt())
                 .ReverseMap()
                 .BeforeMap((src, dest) => src.Encrypt())
                 .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte?)src.OrganizationType))
                 .ForPath(dest => dest.UNIT_TYPE, opt => opt.MapFrom(src => (byte)src.UnitType))
                 .ForPath(dest => dest.GENDER, opt => opt.MapFrom(src => (byte?)src.Gender))
                 .IgnoreAllNonExisting();

                CreateMap<CASE_ASSIGNMENT_COMPLAINT_NOTICE_USER, OrganizationUser>()
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
                   .ForMember(dest => dest.ParentPathName, opt => opt.MapFrom(src => src.PARENT_PATH_NAME))
                   .AfterMap((src, dest) => dest.Decrypt())
                   .ReverseMap()
                   .BeforeMap((src, dest) => src.Encrypt())
                   .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte?)src.OrganizationType))
                   .ForPath(dest => dest.UNIT_TYPE, opt => opt.MapFrom(src => (byte)src.UnitType))
                   .IgnoreAllNonExisting();
            }
        }

        public class CaseAssignmentComplaintInvoiceProfile : AutoMapper.Profile
        {
            public CaseAssignmentComplaintInvoiceProfile()
            {
                CreateMap<CASE_ASSIGNMENT_COMPLAINT_INVOICE, CaseAssignmentComplaintInvoice>()
                     .ForMember(dest => dest.Case, opt => opt.MapFrom(src => src.CASE))
                     .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.CASE_ASSIGNMENT_COMPLAINT_INVOICE_USER))
                     .ForMember(dest => dest.CaseID, opt => opt.MapFrom(src => src.CASE_ID))
                     .ForMember(dest => dest.NotificationDateTime, opt => opt.MapFrom(src => src.NOTICE_DATETIME))
                     .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.CONTENT))
                     .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                     .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                     .ForMember(dest => dest.EMLFilePath, opt => opt.MapFrom(src => src.EML_FILE_PATH))
                     .ForMember(dest => dest.FilePath, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.FILE_PATH) ? new string[] { } : JsonConvert.DeserializeObject<string[]>(src.FILE_PATH)))
                     .ForMember(dest => dest.InvoiceID, opt => opt.MapFrom(src => src.INVOICE_ID))
                     .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                     .ForMember(dest => dest.OrganizationType, opt => opt.MapFrom(src => (OrganizationType)src.ORGANIZATION_TYPE))
                     .ForMember(dest => dest.NotificationBehaviors, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.NOTIFICATION_BEHAVIORS) ? new string[] { } : JsonConvert.DeserializeObject<string[]>(src.NOTIFICATION_BEHAVIORS)))
                     .ForMember(dest => dest.CaseAssignmentComplaintInvoiceType, opt => opt.MapFrom(src => (CaseAssignmentComplaintInvoiceType)src.TYPE))
                     .ForMember(dest => dest.InvoiceType, opt => opt.MapFrom(src => src.INVOICE_TYPE))
                     .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                     .ForMember(dest => dest.IsRecall, opt => opt.MapFrom(src => src.IS_RECALL))
                     .ForMember(dest => dest.NoticeUsers, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.NOTICE_USERs) ? new string[] { } : JsonConvert.DeserializeObject<string[]>(src.NOTICE_USERs)))
                     .ReverseMap()
                     .ForPath(dest => dest.TYPE, opt => opt.MapFrom(src => (byte)src.CaseAssignmentComplaintInvoiceType))
                     .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte)src.OrganizationType))
                     .ForPath(dest => dest.NOTIFICATION_BEHAVIORS, opt => opt.MapFrom(src => src.NotificationBehaviors == null ? JsonConvert.SerializeObject(new string[] { }) : JsonConvert.SerializeObject(src.NotificationBehaviors)))
                     .ForPath(dest => dest.FILE_PATH, opt => opt.MapFrom(src => src.FilePath == null ? JsonConvert.SerializeObject(new string[] { }) : JsonConvert.SerializeObject(src.FilePath)))
                     .ForPath(dest => dest.NOTICE_USERs, opt => opt.MapFrom(src => src.NoticeUsers == null ? JsonConvert.SerializeObject(new string[] { }) : JsonConvert.SerializeObject(src.NoticeUsers)))
                     .IgnoreAllNonExisting();
            }
        }

        public class CaseAssignmentComplaintInvoiceUserProfile : AutoMapper.Profile
        {
            public CaseAssignmentComplaintInvoiceUserProfile()
            {
                CreateMap<CASE_ASSIGNMENT_COMPLAINT_INVOICE_USER, CaseAssignmentComplaintInvoiceUser>()
                     .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.ADDRESS))
                     .ForMember(dest => dest.BUID, opt => opt.MapFrom(src => src.BU_ID))
                     .ForMember(dest => dest.InvoiceIdentityID, opt => opt.MapFrom(src => src.INVOICE_IDENTITY_ID))
                     .ForMember(dest => dest.BUName, opt => opt.MapFrom(src => src.BU_NAME))
                     .ForMember(dest => dest.Invoice, opt => opt.MapFrom(src => src.CASE_ASSIGNMENT_COMPLAINT_INVOICE))
                     .ForMember(dest => dest.CaseID, opt => opt.MapFrom(src => src.CASE_ID))
                     .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EMAIL))
                     .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => (GenderType?)src.GENDER))
                     .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                     .ForMember(dest => dest.StoreNo, opt => opt.MapFrom(src => src.STORE_NO))
                     .ForMember(dest => dest.InvoiceID, opt => opt.MapFrom(src => src.INVOICE_ID))
                     .ForMember(dest => dest.JobID, opt => opt.MapFrom(src => src.JOB_ID))
                     .ForMember(dest => dest.JobName, opt => opt.MapFrom(src => src.JOB_NAME))
                     .ForMember(dest => dest.Mobile, opt => opt.MapFrom(src => src.MOBILE))
                     .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                     .ForMember(dest => dest.NodeName, opt => opt.MapFrom(src => src.NODE_NAME))
                     .ForMember(dest => dest.ParentPathName, opt => opt.MapFrom(src => src.PARENT_PATH_NAME))
                     .ForMember(dest => dest.NotificationBehavior, opt => opt.MapFrom(src => src.NOTIFICATION_BEHAVIOR))
                     .ForMember(dest => dest.NotificationKind, opt => opt.MapFrom(src => src.NOTIFICATION_KIND))
                     .ForMember(dest => dest.NotificationRemark, opt => opt.MapFrom(src => src.NOTIFICATION_REMARK))
                     .ForMember(dest => dest.OrganizationType, opt => opt.MapFrom(src => (OrganizationType?)src.ORGANIZATION_TYPE))
                     .ForMember(dest => dest.Telephone, opt => opt.MapFrom(src => src.TELEPHONE))
                     .ForMember(dest => dest.TelephoneBak, opt => opt.MapFrom(src => src.TELEPHONE_BAK))
                     .ForMember(dest => dest.UnitType, opt => opt.MapFrom(src => (UnitType)src.UNIT_TYPE))
                     .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.USER_ID))
                     .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.USER_NAME))
                     .AfterMap((src, dest) => dest.Decrypt())
                     .ReverseMap()
                     .BeforeMap((src, dest) => src.Encrypt())
                     .ForPath(dest => dest.UNIT_TYPE, opt => opt.MapFrom(src => (byte)src.UnitType))
                     .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte?)src.OrganizationType))
                     .ForPath(dest => dest.GENDER, opt => opt.MapFrom(src => (byte?)src.Gender))
                     .IgnoreAllNonExisting();


                CreateMap<CASE_ASSIGNMENT_COMPLAINT_INVOICE_USER, ConcatableUser>()
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
                    .ForMember(dest => dest.ParentPathName, opt => opt.MapFrom(src => src.PARENT_PATH_NAME))
                    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.USER_NAME))
                    .ForMember(dest => dest.UnitType, opt => opt.MapFrom(src => (UnitType)src.UNIT_TYPE))
                    .ForMember(dest => dest.Mobile, opt => opt.MapFrom(src => src.MOBILE))
                    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EMAIL))
                    .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.ADDRESS))
                    .ForMember(dest => dest.Telephone, opt => opt.MapFrom(src => src.TELEPHONE))
                    .ForMember(dest => dest.TelephoneBak, opt => opt.MapFrom(src => src.TELEPHONE_BAK))
                    .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => (GenderType?)src.GENDER))
                    .AfterMap((src, dest) => dest.Decrypt())
                    .ReverseMap()
                    .BeforeMap((src, dest) => src.Encrypt())
                    .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte?)src.OrganizationType))
                    .ForPath(dest => dest.UNIT_TYPE, opt => opt.MapFrom(src => (byte)src.UnitType))
                    .ForPath(dest => dest.GENDER, opt => opt.MapFrom(src => (byte?)src.Gender))
                    .IgnoreAllNonExisting();

                CreateMap<CASE_ASSIGNMENT_COMPLAINT_INVOICE_USER, OrganizationUser>()
                   .ForMember(dest => dest.BUID, opt => opt.MapFrom(src => src.BU_ID))
                   .ForMember(dest => dest.BUName, opt => opt.MapFrom(src => src.BU_NAME))
                   .ForMember(dest => dest.JobName, opt => opt.MapFrom(src => src.JOB_NAME))
                   .ForMember(dest => dest.JobID, opt => opt.MapFrom(src => src.JOB_ID))
                   .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                   .ForMember(dest => dest.NodeName, opt => opt.MapFrom(src => src.NODE_NAME))
                   .ForMember(dest => dest.OrganizationType, opt => opt.MapFrom(src => (OrganizationType?)src.ORGANIZATION_TYPE))
                   .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.USER_ID))
                   .ForMember(dest => dest.ParentPathName, opt => opt.MapFrom(src => src.PARENT_PATH_NAME))
                   .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.USER_NAME))
                   .ForMember(dest => dest.UnitType, opt => opt.MapFrom(src => (UnitType)src.UNIT_TYPE))
                   .AfterMap((src, dest) => dest.Decrypt())
                   .ReverseMap()
                   .BeforeMap((src, dest) => src.Encrypt())
                   .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte?)src.OrganizationType))
                   .ForPath(dest => dest.UNIT_TYPE, opt => opt.MapFrom(src => (byte)src.UnitType))
                   .IgnoreAllNonExisting();
            }
        }

        public class CaseAssignmentComplaintNoticeProfile : AutoMapper.Profile
        {
            public CaseAssignmentComplaintNoticeProfile()
            {
                CreateMap<CASE_ASSIGNMENT_COMPLAINT_NOTICE, CaseAssignmentComplaintNotice>()
                     .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                     .ForMember(dest => dest.Case, opt => opt.MapFrom(src => src.CASE))
                     .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.CASE_ASSIGNMENT_COMPLAINT_NOTICE_USER))
                     .ForMember(dest => dest.CaseID, opt => opt.MapFrom(src => src.CASE_ID))
                     .ForMember(dest => dest.NotificationDateTime, opt => opt.MapFrom(src => src.NOTICE_DATETIME))
                     .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.CONTENT))
                     .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                     .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                     .ForMember(dest => dest.EMLFilePath, opt => opt.MapFrom(src => src.EML_FILE_PATH))
                     .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                     .ForMember(dest => dest.OrganizationType, opt => opt.MapFrom(src => (OrganizationType)src.ORGANIZATION_TYPE))
                     .ForMember(dest => dest.FilePath, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.FILE_PATH) ? new string[] { } : JsonConvert.DeserializeObject<string[]>(src.FILE_PATH)))
                     .ForMember(dest => dest.NotificationBehaviors, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.NOTIFICATION_BEHAVIORS) ? new string[] { } : JsonConvert.DeserializeObject<string[]>(src.NOTIFICATION_BEHAVIORS)))
                     .ForMember(dest => dest.NoticeUsers, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.NOTICE_USERs) ? new string[] { } : JsonConvert.DeserializeObject<string[]>(src.NOTICE_USERs)))
                     .ReverseMap()
                     .ForPath(dest => dest.NOTIFICATION_BEHAVIORS, opt => opt.MapFrom(src => src.NotificationBehaviors == null ? JsonConvert.SerializeObject(new string[] { }) : JsonConvert.SerializeObject(src.NotificationBehaviors)))
                     .ForPath(dest => dest.FILE_PATH, opt => opt.MapFrom(src => src.FilePath == null ? JsonConvert.SerializeObject(new string[] { }) : JsonConvert.SerializeObject(src.FilePath)))
                     .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte)src.OrganizationType))
                     .ForPath(dest => dest.NOTICE_USERs, opt => opt.MapFrom(src => src.NoticeUsers == null ? JsonConvert.SerializeObject(new string[] { }) : JsonConvert.SerializeObject(src.NoticeUsers)))

                     .IgnoreAllNonExisting();
            }
        }

        public class CaseAssignmentComplaintNoticeUserProfile : AutoMapper.Profile
        {
            public CaseAssignmentComplaintNoticeUserProfile()
            {
                CreateMap<CASE_ASSIGNMENT_COMPLAINT_NOTICE_USER, CaseAssignmentComplaintNoticeUser>()
                     .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.ADDRESS))
                     .ForMember(dest => dest.BUID, opt => opt.MapFrom(src => src.BU_ID))
                     .ForMember(dest => dest.NoticeID, opt => opt.MapFrom(src => src.NOTICE_ID))
                     .ForMember(dest => dest.Notice, opt => opt.MapFrom(src => src.CASE_ASSIGNMENT_COMPLAINT_NOTICE))
                     .ForMember(dest => dest.BUName, opt => opt.MapFrom(src => src.BU_NAME))
                     .ForMember(dest => dest.CaseID, opt => opt.MapFrom(src => src.CASE_ID))
                     .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EMAIL))
                     .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => (GenderType?)src.GENDER))
                     .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                     .ForMember(dest => dest.StoreNo, opt => opt.MapFrom(src => src.STORE_NO))
                     .ForMember(dest => dest.JobID, opt => opt.MapFrom(src => src.JOB_ID))
                     .ForMember(dest => dest.JobName, opt => opt.MapFrom(src => src.JOB_NAME))
                     .ForMember(dest => dest.Mobile, opt => opt.MapFrom(src => src.MOBILE))
                     .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                     .ForMember(dest => dest.NodeName, opt => opt.MapFrom(src => src.NODE_NAME))
                     .ForMember(dest => dest.NotificationBehavior, opt => opt.MapFrom(src => src.NOTIFICATION_BEHAVIOR))
                     .ForMember(dest => dest.NotificationKind, opt => opt.MapFrom(src => src.NOTIFICATION_KIND))
                     .ForMember(dest => dest.NotificationRemark, opt => opt.MapFrom(src => src.NOTIFICATION_REMARK))
                     .ForMember(dest => dest.OrganizationType, opt => opt.MapFrom(src => (OrganizationType?)src.ORGANIZATION_TYPE))
                     .ForMember(dest => dest.Telephone, opt => opt.MapFrom(src => src.TELEPHONE))
                     .ForMember(dest => dest.ParentPathName, opt => opt.MapFrom(src => src.PARENT_PATH_NAME))
                     .ForMember(dest => dest.TelephoneBak, opt => opt.MapFrom(src => src.TELEPHONE_BAK))
                     .ForMember(dest => dest.UnitType, opt => opt.MapFrom(src => (UnitType)src.UNIT_TYPE))
                     .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.USER_ID))
                     .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.USER_NAME))
                     .AfterMap((src, dest) => dest.Decrypt())
                     .ReverseMap()
                     .BeforeMap((src, dest) => src.Encrypt())
                     .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte?)src.OrganizationType))
                     .ForPath(dest => dest.GENDER, opt => opt.MapFrom(src => (byte?)src.Gender))
                     .ForPath(dest => dest.UNIT_TYPE, opt => opt.MapFrom(src => (byte)src.UnitType))
                     .IgnoreAllNonExisting();


                CreateMap<CASE_ASSIGNMENT_COMPLAINT_NOTICE_USER, ConcatableUser>()
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
                 .ForMember(dest => dest.ParentPathName, opt => opt.MapFrom(src => src.PARENT_PATH_NAME))
                 .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EMAIL))
                 .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.ADDRESS))
                 .ForMember(dest => dest.Telephone, opt => opt.MapFrom(src => src.TELEPHONE))
                 .ForMember(dest => dest.TelephoneBak, opt => opt.MapFrom(src => src.TELEPHONE_BAK))
                 .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => (GenderType?)src.GENDER))
                 .AfterMap((src, dest) => dest.Decrypt())
                 .ReverseMap()
                 .BeforeMap((src, dest) => src.Encrypt())
                 .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte?)src.OrganizationType))
                 .ForPath(dest => dest.UNIT_TYPE, opt => opt.MapFrom(src => (byte)src.UnitType))
                 .ForPath(dest => dest.GENDER, opt => opt.MapFrom(src => (byte?)src.Gender))
                 .IgnoreAllNonExisting();

                CreateMap<CASE_ASSIGNMENT_COMPLAINT_NOTICE_USER, OrganizationUser>()
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
                   .ForMember(dest => dest.ParentPathName, opt => opt.MapFrom(src => src.PARENT_PATH_NAME))
                   .AfterMap((src, dest) => dest.Decrypt())
                   .ReverseMap()
                   .BeforeMap((src, dest) => src.Encrypt())
                   .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte?)src.OrganizationType))
                   .ForPath(dest => dest.UNIT_TYPE, opt => opt.MapFrom(src => (byte)src.UnitType))
                   .IgnoreAllNonExisting();
            }
        }

        public class CaseSourceProfile : AutoMapper.Profile
        {
            public CaseSourceProfile()
            {
                CreateMap<CASE_SOURCE, CaseSource>()
                     .ForMember(dest => dest.GroupID, opt => opt.MapFrom(src => src.GROUP_ID))
                     .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                     .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                     .ForMember(dest => dest.UpdateDateTime, opt => opt.MapFrom(src => src.UPDATE_DATETIME))
                     .ForMember(dest => dest.UpdateUserName, opt => opt.MapFrom(src => src.UPDATE_USERNAME))
                     .ForMember(dest => dest.IncomingDateTime, opt => opt.MapFrom(src => src.INCOMING_DATETIME))
                     .ForMember(dest => dest.SourceID, opt => opt.MapFrom(src => src.SOURCE_ID))
                     .ForMember(dest => dest.IsPrevention, opt => opt.MapFrom(src => src.IS_PREVENTION))
                     .ForMember(dest => dest.IsTwiceCall, opt => opt.MapFrom(src => src.IS_TWICE_CALL))
                     .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                     .ForMember(dest => dest.CaseSourceUser, opt => opt.MapFrom(src => src.CASE_SOURCE_USER))
                     .ForMember(dest => dest.Remark, opt => opt.MapFrom(src => src.REMARK))
                     .ForMember(dest => dest.RelationCaseSourceID, opt => opt.MapFrom(src => src.RELATION_CASE_SOURCE_ID))
                     .ForMember(dest => dest.RelationCaseIDs, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.RELATION_CASE_IDs) ? new string[] { } : JsonConvert.DeserializeObject<string[]>(src.RELATION_CASE_IDs)))
                     .ForMember(dest => dest.CaseSourceType, opt => opt.MapFrom(src => (CaseSourceType)src.SOURCE_TYPE))
                     .ForMember(dest => dest.VoiceID, opt => opt.MapFrom(src => src.VOICE_ID))
                     .ForMember(dest => dest.VoiceLocator, opt => opt.MapFrom(src => src.VOICE_LOCATOR))
                     .ForMember(dest => dest.Cases, opt => opt.MapFrom(src => src.CASE))
                     .ReverseMap()
                     .ForPath(dest => dest.CASE, opt => opt.MapFrom(src => default(IEnumerable<CASE>)))
                     .ForPath(dest => dest.SOURCE_TYPE, opt => opt.MapFrom(src => (byte)src.CaseSourceType))
                     .ForPath(dest => dest.RELATION_CASE_IDs, opt => opt.MapFrom(src => src.RelationCaseIDs == null ? JsonConvert.SerializeObject(new string[] { }) : JsonConvert.SerializeObject(src.RelationCaseIDs)))
                     .IgnoreAllNonExisting();
            }
        }

        public class CaseSourceHistoryProfile : AutoMapper.Profile
        {
            public CaseSourceHistoryProfile()
            {
                CreateMap<CASE_SOURCE_HISTORY, CaseSourceHistory>()
                     .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                     .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                     .ForMember(dest => dest.SourceID, opt => opt.MapFrom(src => src.SOURCE_ID))
                     .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.CONTENT))
                     .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                     .ReverseMap()
                     .IgnoreAllNonExisting();
            }
        }




        public class CaseSourceUserProfile : AutoMapper.Profile
        {
            public CaseSourceUserProfile()
            {
                CreateMap<CASE_SOURCE_USER, CaseSourceUser>()
                     .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.ADDRESS))
                     .ForMember(dest => dest.BUID, opt => opt.MapFrom(src => src.BU_ID))
                     .ForMember(dest => dest.BUName, opt => opt.MapFrom(src => src.BU_NAME))
                     .ForMember(dest => dest.CaseSource, opt => opt.MapFrom(src => src.CASE_SOURCE))
                     .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EMAIL))
                     .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => (GenderType?)src.GENDER))
                     .ForMember(dest => dest.JobID, opt => opt.MapFrom(src => src.JOB_ID))
                     .ForMember(dest => dest.JobName, opt => opt.MapFrom(src => src.JOB_NAME))
                     .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.USER_NAME))
                     .ForMember(dest => dest.Mobile, opt => opt.MapFrom(src => src.MOBILE))
                     .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                     .ForMember(dest => dest.ParentPathName, opt => opt.MapFrom(src => src.PARENT_PATH_NAME))
                     .ForMember(dest => dest.NodeName, opt => opt.MapFrom(src => src.NODE_NAME))
                     .ForMember(dest => dest.OrganizationType, opt => opt.MapFrom(src => (OrganizationType?)src.ORGANIZATION_TYPE))
                     .ForMember(dest => dest.SourceID, opt => opt.MapFrom(src => src.SOURCE_ID))
                     .ForMember(dest => dest.StoreNo, opt => opt.MapFrom(src => src.STORE_NO))
                     .ForMember(dest => dest.Telephone, opt => opt.MapFrom(src => src.TELEPHONE))
                     .ForMember(dest => dest.TelephoneBak, opt => opt.MapFrom(src => src.TELEPHONE_BAK))
                     .ForMember(dest => dest.UnitType, opt => opt.MapFrom(src => (UnitType)src.UNIT_TYPE))
                     .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.USER_ID))
                     .AfterMap((src, dest) => dest.Decrypt())
                     .ReverseMap()
                     .BeforeMap((src, dest) => src.Encrypt())
                     .ForPath(dest => dest.GENDER, opt => opt.MapFrom(src => (byte?)src.Gender))
                     .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte?)src.OrganizationType))
                     .ForPath(dest => dest.UNIT_TYPE, opt => opt.MapFrom(src => (byte)src.UnitType))
                     .IgnoreAllNonExisting();
            }
        }

        public class CaseHistoryProfile : AutoMapper.Profile
        {
            public CaseHistoryProfile()
            {
                CreateMap<CASE_HISTORY, CaseHistory>()
                     .ForMember(dest => dest.CaseType, opt => opt.MapFrom(src => (CaseType)src.CASE_TYPE))

                     .ForMember(dest => dest.CaseID, opt => opt.MapFrom(src => src.CASE_ID))
                     .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.CONTENT))
                     .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                     .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                     .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                     .ReverseMap()
                     .ForPath(dest => dest.CASE_TYPE, opt => opt.MapFrom(src => (byte)src.CaseType))
                     .IgnoreAllNonExisting();
            }
        }

        public class CaseConcatUserProfile : AutoMapper.Profile
        {
            public CaseConcatUserProfile()
            {
                CreateMap<CASE_CONCAT_USER, CaseConcatUser>()
                     .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.ADDRESS))
                     .ForMember(dest => dest.BUID, opt => opt.MapFrom(src => src.BU_ID))
                     .ForMember(dest => dest.BUName, opt => opt.MapFrom(src => src.BU_NAME))
                     .ForMember(dest => dest.Case, opt => opt.MapFrom(src => src.CASE))
                     .ForMember(dest => dest.CaseID, opt => opt.MapFrom(src => src.CASE_ID))
                     .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => (GenderType?)src.GENDER))
                     .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                     .ForMember(dest => dest.StoreNo, opt => opt.MapFrom(src => src.STORE_NO))
                     .ForMember(dest => dest.JobID, opt => opt.MapFrom(src => src.JOB_ID))
                     .ForMember(dest => dest.JobName, opt => opt.MapFrom(src => src.JOB_NAME))
                     .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                     .ForMember(dest => dest.NodeName, opt => opt.MapFrom(src => src.NODE_NAME))
                     .ForMember(dest => dest.ParentPathName, opt => opt.MapFrom(src => src.PARENT_PATH_NAME))
                     .ForMember(dest => dest.NotificationBehavior, opt => opt.MapFrom(src => src.NOTIFICATION_BEHAVIOR))
                     .ForMember(dest => dest.NotificationKind, opt => opt.MapFrom(src => src.NOTIFICATION_KIND))
                     .ForMember(dest => dest.NotificationRemark, opt => opt.MapFrom(src => src.NOTIFICATION_REMARK))
                     .ForMember(dest => dest.OrganizationType, opt => opt.MapFrom(src => (OrganizationType?)src.ORGANIZATION_TYPE))
                     .ForMember(dest => dest.UnitType, opt => opt.MapFrom(src => (UnitType)src.UNIT_TYPE))
                     .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.USER_ID))
                     .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.USER_NAME))
                     .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EMAIL))
                     .ForMember(dest => dest.Telephone, opt => opt.MapFrom(src => src.TELEPHONE))
                     .ForMember(dest => dest.TelephoneBak, opt => opt.MapFrom(src => src.TELEPHONE_BAK))
                     .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.ADDRESS))
                     .ForMember(dest => dest.Mobile, opt => opt.MapFrom(src => src.MOBILE))
                     .AfterMap((src, dest) => dest.Decrypt())
                     .ReverseMap()
                     .BeforeMap((src, dest) => src.Encrypt())
                     .AfterMap((src, dest) => src.Decrypt())
                     .ForPath(dest => dest.UNIT_TYPE, opt => opt.MapFrom(src => (byte)src.UnitType))
                     .ForPath(dest => dest.GENDER, opt => opt.MapFrom(src => (byte?)src.Gender))
                     .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte?)src.OrganizationType))
                     .IgnoreAllNonExisting();
            }
        }

        public class CaseComplaintUserProfile : AutoMapper.Profile
        {
            public CaseComplaintUserProfile()
            {
                CreateMap<CASE_COMPLAINED_USER, CaseComplainedUser>()
                     .ForMember(dest => dest.BUID, opt => opt.MapFrom(src => src.BU_ID))
                     .ForMember(dest => dest.BUName, opt => opt.MapFrom(src => src.BU_NAME))
                     .ForMember(dest => dest.Case, opt => opt.MapFrom(src => src.CASE))
                     .ForMember(dest => dest.CaseID, opt => opt.MapFrom(src => src.CASE_ID))
                     .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                     .ForMember(dest => dest.JobID, opt => opt.MapFrom(src => src.JOB_ID))
                     .ForMember(dest => dest.StoreNo, opt => opt.MapFrom(src => src.STORE_NO))
                     .ForMember(dest => dest.JobName, opt => opt.MapFrom(src => src.JOB_NAME))
                     .ForMember(dest => dest.ParentPathName, opt => opt.MapFrom(src => src.PARENT_PATH_NAME))
                     .ForMember(dest => dest.CaseComplainedUserType, opt => opt.MapFrom(src => (CaseComplainedUserType)src.COMPLAINED_USER_TYPE))
                     .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                     .ForMember(dest => dest.NodeName, opt => opt.MapFrom(src => src.NODE_NAME))
                     .ForMember(dest => dest.OrganizationType, opt => opt.MapFrom(src => (OrganizationType?)src.ORGANIZATION_TYPE))
                     .ForMember(dest => dest.UnitType, opt => opt.MapFrom(src => (UnitType)src.UNIT_TYPE))
                     .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.USER_ID))
                     .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.USER_NAME))
                     .ForMember(dest => dest.OwnerUserName, opt => opt.MapFrom(src => src.OWNER_USERNAME))
                     .ForMember(dest => dest.OwnerJobName, opt => opt.MapFrom(src => src.OWNER_JOB_NAME))
                     .ForMember(dest => dest.OwnerUserPhone, opt => opt.MapFrom(src => src.OWNER_USER_PHONE))
                     .ForMember(dest => dest.OwnerUserEmail, opt => opt.MapFrom(src => src.OWNER_USER_EMAIL))
                     .ForMember(dest => dest.SupervisorUserName, opt => opt.MapFrom(src => src.SUPERVISOR_USERNAME))
                     .ForMember(dest => dest.SupervisorUserPhone, opt => opt.MapFrom(src => src.SUPERVISOR_USER_PHONE))
                     .ForMember(dest => dest.SupervisorUserEmail, opt => opt.MapFrom(src => src.SUPERVISOR_USER_EMAIL))
                     .ForMember(dest => dest.SupervisorJobName, opt => opt.MapFrom(src => src.SUPERVISOR_JOB_NAME))
                     .ForMember(dest => dest.SupervisorNodeName, opt => opt.MapFrom(src => src.SUPERVISOR_NODE_NAME))
                     .ForMember(dest => dest.Mobile, opt => opt.MapFrom(src => src.MOBILE))
                     .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EMAIL))
                     .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.ADDRESS))
                     .ForMember(dest => dest.Telephone, opt => opt.MapFrom(src => src.TELEPHONE))
                     .ForMember(dest => dest.TelephoneBak, opt => opt.MapFrom(src => src.TELEPHONE_BAK))
                     .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => (GenderType?)src.GENDER))
                     .AfterMap((src, dest) => dest.Decrypt())
                     .ReverseMap()
                     .BeforeMap((src, dest) => src.Encrypt())
                     .AfterMap((src, dest) => src.Decrypt())
                     .ForPath(dest => dest.UNIT_TYPE, opt => opt.MapFrom(src => (byte)src.UnitType))
                     .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte?)src.OrganizationType))
                     .ForPath(dest => dest.COMPLAINED_USER_TYPE, opt => opt.MapFrom(src => (byte)src.CaseComplainedUserType))
                     .IgnoreAllNonExisting();


                CreateMap<CASE_COMPLAINED_USER, ConcatableUser>()
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
                  .ForMember(dest => dest.ParentPathName, opt => opt.MapFrom(src => src.PARENT_PATH_NAME))
                  .ForMember(dest => dest.UnitType, opt => opt.MapFrom(src => (UnitType)src.UNIT_TYPE))
                  .ForMember(dest => dest.Mobile, opt => opt.MapFrom(src => src.MOBILE))
                  .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EMAIL))
                  .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.ADDRESS))
                  .ForMember(dest => dest.Telephone, opt => opt.MapFrom(src => src.TELEPHONE))
                  .ForMember(dest => dest.TelephoneBak, opt => opt.MapFrom(src => src.TELEPHONE_BAK))
                  .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => (GenderType?)src.GENDER))
                  .AfterMap((src, dest) => dest.Decrypt())
                  .ReverseMap()
                  .BeforeMap((src, dest) => src.Encrypt())
                  .AfterMap((src, dest) => src.Decrypt())
                  .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte?)src.OrganizationType))
                  .ForPath(dest => dest.UNIT_TYPE, opt => opt.MapFrom(src => (byte)src.UnitType))
                  .ForPath(dest => dest.GENDER, opt => opt.MapFrom(src => (byte?)src.Gender))
                  .IgnoreAllNonExisting();

                CreateMap<CASE_COMPLAINED_USER, OrganizationUser>()
                   .ForMember(dest => dest.BUID, opt => opt.MapFrom(src => src.BU_ID))
                   .ForMember(dest => dest.BUName, opt => opt.MapFrom(src => src.BU_NAME))
                   .ForMember(dest => dest.JobName, opt => opt.MapFrom(src => src.JOB_NAME))
                   .ForMember(dest => dest.JobID, opt => opt.MapFrom(src => src.JOB_ID))
                   .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                   .ForMember(dest => dest.NodeName, opt => opt.MapFrom(src => src.NODE_NAME))
                   .ForMember(dest => dest.ParentPathName, opt => opt.MapFrom(src => src.PARENT_PATH_NAME))
                   .ForMember(dest => dest.OrganizationType, opt => opt.MapFrom(src => (OrganizationType?)src.ORGANIZATION_TYPE))
                   .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.USER_ID))
                   .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.USER_NAME))
                   .ForMember(dest => dest.UnitType, opt => opt.MapFrom(src => (UnitType)src.UNIT_TYPE))
                   .AfterMap((src, dest) => dest.Decrypt())
                   .ReverseMap()
                   .BeforeMap((src, dest) => src.Encrypt())
                   .AfterMap((src, dest) => src.Decrypt())
                   .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte?)src.OrganizationType))
                   .ForPath(dest => dest.UNIT_TYPE, opt => opt.MapFrom(src => (byte)src.UnitType))
                   .IgnoreAllNonExisting();
            }



        }

        public class CaseAssignmentHistoryProfile : AutoMapper.Profile
        {
            public CaseAssignmentHistoryProfile()
            {
                CreateMap<CASE_ASSIGNMENT_HISTORY, CaseAssignmentHistory>()
                     .ForMember(dest => dest.CaseAssignmentType, opt => opt.MapFrom(src => (CaseAssignmentType)src.CASE_ASSIGNMENT_TYPE))
                     .ForMember(dest => dest.AssignemtID, opt => opt.MapFrom(src => src.ASSIGNMENT_ID))
                     .ForMember(dest => dest.CaseID, opt => opt.MapFrom(src => src.CASE_ID))
                     .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.CONTENT))
                     .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                     .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                     .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                     .ReverseMap()
                     .ForPath(dest => dest.CASE_ASSIGNMENT_TYPE, opt => opt.MapFrom(src => (byte)src.CaseAssignmentType))
                     .IgnoreAllNonExisting();
            }
        }

        public class CaseAssignmentResumeProfile : AutoMapper.Profile
        {
            public CaseAssignmentResumeProfile()
            {
                CreateMap<CASE_ASSIGNMENT_RESUME, CaseAssignmentResume>()
                     .ForMember(dest => dest.CaseAssignmentType, opt => opt.MapFrom(src => (CaseAssignmentType)src.CASE_ASSIGNMENT_TYPE))
                     .ForMember(dest => dest.CaseID, opt => opt.MapFrom(src => src.CASE_ID))
                     .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.CONTENT))
                     .ForMember(dest => dest.CaseAssignmentID, opt => opt.MapFrom(src => src.CASE_ASSIGNMENT_ID))
                     .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                     .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                     .ForMember(dest => dest.CreateNodeID, opt => opt.MapFrom(src => src.CREATE_NODE_ID))
                     .ForMember(dest => dest.CreateNodeName, opt => opt.MapFrom(src => src.CREATE_NODE_NAME))
                     .ForMember(dest => dest.CreateOrganizationType, opt => opt.MapFrom(src => (OrganizationType?)src.CREATE_ORGANIZIATION_TYPE))
                     .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                     .ForMember(dest => dest.IsReply, opt => opt.MapFrom(src => src.IS_REPLY))
                     .ReverseMap()
                     .ForPath(dest => dest.CASE_ASSIGNMENT_TYPE, opt => opt.MapFrom(src => (byte)src.CaseAssignmentType))
                     .ForPath(dest => dest.CREATE_ORGANIZIATION_TYPE, opt => opt.MapFrom(src => (byte?)src.CreateOrganizationType))
                     .IgnoreAllNonExisting();
            }
        }

        public class CaseAssignmentUserProfile : AutoMapper.Profile
        {
            public CaseAssignmentUserProfile()
            {
                CreateMap<CASE_ASSIGNMENT_USER, CaseAssignmentUser>()
                     .ForMember(dest => dest.IsApply, opt => opt.MapFrom(src => src.IS_APPLY))
                     .ForMember(dest => dest.BUID, opt => opt.MapFrom(src => src.BU_ID))
                     .ForMember(dest => dest.CaseID, opt => opt.MapFrom(src => src.CASE_ID))
                     .ForMember(dest => dest.BUID, opt => opt.MapFrom(src => src.BU_ID))
                     .ForMember(dest => dest.BUName, opt => opt.MapFrom(src => src.BU_NAME))
                     .ForMember(dest => dest.CaseAssignment, opt => opt.MapFrom(src => src.CASE_ASSIGNMENT))
                     .ForMember(dest => dest.AssignmentID, opt => opt.MapFrom(src => src.ASSIGNMENT_ID))
                     .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                     .ForMember(dest => dest.StoreNo, opt => opt.MapFrom(src => src.STORE_NO))
                     .ForMember(dest => dest.JobID, opt => opt.MapFrom(src => src.JOB_ID))
                     .ForMember(dest => dest.ParentPathName, opt => opt.MapFrom(src => src.PARENT_PATH_NAME))
                     .ForMember(dest => dest.JobName, opt => opt.MapFrom(src => src.JOB_NAME))
                     .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                     .ForMember(dest => dest.NodeName, opt => opt.MapFrom(src => src.NODE_NAME))
                     .ForMember(dest => dest.OrganizationType, opt => opt.MapFrom(src => (OrganizationType?)src.ORGANIZATION_TYPE))
                     .ForMember(dest => dest.UnitType, opt => opt.MapFrom(src => (UnitType)src.UNIT_TYPE))
                     .ForMember(dest => dest.CaseComplainedUserType, opt => opt.MapFrom(src => (CaseComplainedUserType)src.COMPLAINED_USER_TYPE))
                     .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.USER_ID))
                     .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.USER_NAME))
                     .AfterMap((src, dest) => dest.Decrypt())
                     .ReverseMap()
                     .BeforeMap((src, dest) => src.Encrypt())
                     .ForPath(dest => dest.COMPLAINED_USER_TYPE, opt => opt.MapFrom(src => (byte)src.CaseComplainedUserType))
                     .ForPath(dest => dest.UNIT_TYPE, opt => opt.MapFrom(src => (byte)src.UnitType))
                     .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte?)src.OrganizationType))
                     .IgnoreAllNonExisting();
            }
        }

        public class CaseAssignmentConcatUserProfile : AutoMapper.Profile
        {
            public CaseAssignmentConcatUserProfile()
            {
                CreateMap<CASE_ASSIGNMENT_CONCAT_USER, CaseAssignmentConcatUser>()

                     .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.ADDRESS))
                     .ForMember(dest => dest.BUID, opt => opt.MapFrom(src => src.BU_ID))
                     .ForMember(dest => dest.CaseComplainedUserType, opt => opt.MapFrom(src => (CaseComplainedUserType)src.COMPLAINED_USER_TYPE))
                     .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                     .ForMember(dest => dest.BUName, opt => opt.MapFrom(src => src.BU_NAME))
                     .ForMember(dest => dest.AssignmentID, opt => opt.MapFrom(src => src.ASSIGNMENT_ID))
                     .ForMember(dest => dest.CaseID, opt => opt.MapFrom(src => src.CASE_ID))
                     .ForMember(dest => dest.ParentPathName, opt => opt.MapFrom(src => src.PARENT_PATH_NAME))
                     .ForMember(dest => dest.CaseAssignment, opt => opt.MapFrom(src => src.CASE_ASSIGNMENT))
                     .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EMAIL))
                     .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => (GenderType?)src.GENDER))
                     .ForMember(dest => dest.StoreNo, opt => opt.MapFrom(src => src.STORE_NO))
                     .ForMember(dest => dest.JobID, opt => opt.MapFrom(src => src.JOB_ID))
                     .ForMember(dest => dest.JobName, opt => opt.MapFrom(src => src.JOB_NAME))
                     .ForMember(dest => dest.Mobile, opt => opt.MapFrom(src => src.MOBILE))
                     .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                     .ForMember(dest => dest.NodeName, opt => opt.MapFrom(src => src.NODE_NAME))
                     .ForMember(dest => dest.NotificationBehavior, opt => opt.MapFrom(src => src.NOTIFICATION_BEHAVIOR))
                     .ForMember(dest => dest.NotificationKind, opt => opt.MapFrom(src => src.NOTIFICATION_KIND))
                     .ForMember(dest => dest.NotificationRemark, opt => opt.MapFrom(src => src.NOTIFICATION_REMARK))
                     .ForMember(dest => dest.OrganizationType, opt => opt.MapFrom(src => (OrganizationType?)src.ORGANIZATION_TYPE))
                     .ForMember(dest => dest.Telephone, opt => opt.MapFrom(src => src.TELEPHONE))
                     .ForMember(dest => dest.TelephoneBak, opt => opt.MapFrom(src => src.TELEPHONE_BAK))
                     .ForMember(dest => dest.UnitType, opt => opt.MapFrom(src => (UnitType)src.UNIT_TYPE))
                     .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.USER_ID))
                     .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.USER_NAME))
                     .AfterMap((src, dest) => dest.Decrypt())
                     .ReverseMap()
                     .BeforeMap((src, dest) => src.Encrypt())
                     .ForPath(dest => dest.GENDER, opt => opt.MapFrom(src => (byte?)src.Gender))
                     .ForPath(dest => dest.UNIT_TYPE, opt => opt.MapFrom(src => (byte)src.UnitType))
                     .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte?)src.OrganizationType))
                     .ForPath(dest => dest.COMPLAINED_USER_TYPE, opt => opt.MapFrom(src => (byte)src.CaseComplainedUserType))
                     .IgnoreAllNonExisting();
            }
        }

        public class CaseRemindProfile : AutoMapper.Profile
        {
            public CaseRemindProfile()
            {
                CreateMap<CASE_REMIND, CaseRemind>()

                     .ForMember(dest => dest.ActiveEndDateTime, opt => opt.MapFrom(src => src.ACTIVE_END_DAETTIME))
                     .ForMember(dest => dest.ActiveStartDateTime, opt => opt.MapFrom(src => src.ACTIVE_START_DAETTIME))
                     .ForMember(dest => dest.Case, opt => opt.MapFrom(src => src.CASE))
                     .ForMember(dest => dest.CaseID, opt => opt.MapFrom(src => src.CASE_ID))
                     .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.CONTENT))
                     .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                     .ForMember(dest => dest.AssignmentID, opt => opt.MapFrom(src => src.ASSIGNMENT_ID))
                     .ForMember(dest => dest.ConfirmDateTime, opt => opt.MapFrom(src => src.CONFIRM_DATETIME))
                     .ForMember(dest => dest.ConfirmUserName, opt => opt.MapFrom(src => src.CONFIRM_USERNAME))
                     .ForMember(dest => dest.ConfirmUserID, opt => opt.MapFrom(src => src.CONFIRM_USER_ID))
                     .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                     .ForMember(dest => dest.CreateUserID, opt => opt.MapFrom(src => src.CREATE_USER_ID))
                     .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                     .ForMember(dest => dest.IsConfirm, opt => opt.MapFrom(src => src.IS_CONFIRM))
                     .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                     .ForMember(dest => dest.OrganizationType, opt => opt.MapFrom(src => (OrganizationType)src.ORGANIZATION_TYPE))
                     .ForMember(dest => dest.Type, opt => opt.MapFrom(src => (CaseRemindType)src.TYPE))
                     .ForMember(dest => dest.UpdateDateTime, opt => opt.MapFrom(src => src.UPDATE_DATETIME))
                     .ForMember(dest => dest.UpdateUserName, opt => opt.MapFrom(src => src.UPDATE_USERNAME))
                     .ForMember(dest => dest.UserIDs, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.USER_IDs) ? new string[] { } : JsonConvert.DeserializeObject<string[]>(src.USER_IDs)))
                     .ReverseMap()
                     .ForPath(dest => dest.TYPE, opt => opt.MapFrom(src => (byte)src.Type))
                     .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte?)src.OrganizationType))
                     .ForPath(dest => dest.USER_IDs, opt => opt.MapFrom(src => src.UserIDs == null ? JsonConvert.SerializeObject(new string[] { }) : JsonConvert.SerializeObject(src.UserIDs)))

                     .IgnoreAllNonExisting();
            }
        }

        public class CaseWarningProfile : AutoMapper.Profile
        {
            public CaseWarningProfile()
            {
                CreateMap<CASE_WARNING, CaseWarning>()
                     .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                     .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                     .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                     .ForMember(dest => dest.IsEnabled, opt => opt.MapFrom(src => src.IS_ENABLED))
                     .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NAME))
                     .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                     .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.ORDER))
                     .ForMember(dest => dest.OrganizationType, opt => opt.MapFrom(src => (OrganizationType)src.ORGANIZATION_TYPE))
                     .ForMember(dest => dest.UpdateDateTime, opt => opt.MapFrom(src => src.UPDATE_DATETIME))
                     .ForMember(dest => dest.UpdateUserName, opt => opt.MapFrom(src => src.UPDATE_USERNAME))
                     .ForMember(dest => dest.WorkHour, opt => opt.MapFrom(src => src.WORK_HOUR))
                     .ReverseMap()
                     .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte)src.OrganizationType))
                     .IgnoreAllNonExisting();
            }
        }

        public class CaseTemplateProfile : AutoMapper.Profile
        {
            public CaseTemplateProfile()
            {
                CreateMap<CASE_TEMPLATE, CaseTemplate>()
                     .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                     .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                     .ForMember(dest => dest.ClassificKey, opt => opt.MapFrom(src => src.CLASSIFIC_KEY))
                     .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.TITLE))
                     .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.CONTENT))
                     .ForMember(dest => dest.IsDefault, opt => opt.MapFrom(src => src.IS_DEFAULT))
                     .ForMember(dest => dest.IsFastFinished, opt => opt.MapFrom(src => src.IS_FAST_FINISH))
                     .ForMember(dest => dest.EmailTitle, opt => opt.MapFrom(src => src.EMAIL_TITLE))
                     .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                     .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                     .ForMember(dest => dest.UpdateDateTime, opt => opt.MapFrom(src => src.UPDATE_DATETIME))
                     .ForMember(dest => dest.UpdateUserName, opt => opt.MapFrom(src => src.UPDATE_USERNAME))
                     .ReverseMap()
                     .IgnoreAllNonExisting();
            }
        }

        public class CaseTagProfile : AutoMapper.Profile
        {
            public CaseTagProfile()
            {
                CreateMap<CASE_TAG, CaseTag>()
                     .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                     .ForMember(dest => dest.IsEnabled, opt => opt.MapFrom(src => src.IS_ENABLED))
                     .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NAME))
                     .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                     .ForMember(dest => dest.OrganizationType, opt => opt.MapFrom(src => (OrganizationType)src.ORGANIZATION_TYPE))
                     .ForMember(dest => dest.Cases, opt => opt.MapFrom(src => src.CASE))
                     .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                     .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                     .ForMember(dest => dest.UpdateDateTime, opt => opt.MapFrom(src => src.UPDATE_DATETIME))
                     .ForMember(dest => dest.UpdateUserName, opt => opt.MapFrom(src => src.UPDATE_USERNAME))
                     .ReverseMap()
                     .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte)src.OrganizationType))
                     .IgnoreAllNonExisting();
            }
        }

        public class CaseAssignGroupProfile : AutoMapper.Profile
        {
            public CaseAssignGroupProfile()
            {
                CreateMap<CASE_ASSIGN_GROUP, CaseAssignGroup>()
                     .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                     .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                     .ForMember(dest => dest.CaseAssignGroupUsers, opt => opt.MapFrom(src => src.CASE_ASSIGN_GROUP_USER))
                     .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NAME))
                     .ForMember(dest => dest.OrganizationType, opt => opt.MapFrom(src => (OrganizationType)src.ORGANIZATION_TYPE))
                     .ForMember(dest => dest.CaseAssignGroupType, opt => opt.MapFrom(src => (CaseAssignGroupType)src.TYPE))
                     .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                     .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                     .ForMember(dest => dest.UpdateDateTime, opt => opt.MapFrom(src => src.UPDATE_DATETIME))
                     .ForMember(dest => dest.UpdateUserName, opt => opt.MapFrom(src => src.UPDATE_USERNAME))
                     .ReverseMap()
                     .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte)src.OrganizationType))
                     .ForPath(dest => dest.TYPE, opt => opt.MapFrom(src => (byte)src.CaseAssignGroupType))
                     .IgnoreAllNonExisting();
            }
        }

        public class CaseAssignGroupUserProfile : AutoMapper.Profile
        {
            public CaseAssignGroupUserProfile()
            {
                CreateMap<CASE_ASSIGN_GROUP_USER, CaseAssignGroupUser>()
                    .ForMember(dest => dest.BUID, opt => opt.MapFrom(src => src.BU_ID))
                    .ForMember(dest => dest.BUName, opt => opt.MapFrom(src => src.BU_NAME))
                    .ForMember(dest => dest.CaseAssignGroup, opt => opt.MapFrom(src => src.CASE_ASSIGN_GROUP))
                    .ForMember(dest => dest.GroupID, opt => opt.MapFrom(src => src.GROUP_ID))
                    .ForMember(dest => dest.JobID, opt => opt.MapFrom(src => src.JOB_ID))
                    .ForMember(dest => dest.JobName, opt => opt.MapFrom(src => src.JOB_NAME))
                    .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                    .ForMember(dest => dest.NodeName, opt => opt.MapFrom(src => src.NODE_NAME))
                    .ForMember(dest => dest.UnitType, opt => opt.MapFrom(src => (UnitType)src.UNIT_TYPE))
                    .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.USER_ID))
                    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.USER_NAME))
                    .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                    .ForMember(dest => dest.OrganizationType, opt => opt.MapFrom(src => (OrganizationType?)src.ORGANIZATION_TYPE))
                    .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.ADDRESS))
                    .ForMember(dest => dest.Mobile, opt => opt.MapFrom(src => src.MOBILE))
                    .ForMember(dest => dest.NotificationBehavior, opt => opt.MapFrom(src => src.NOTIFICATION_BEHAVIOR))
                    .ForMember(dest => dest.NotificationRemark, opt => opt.MapFrom(src => src.NOTIFICATION_REMARK))
                    .ForMember(dest => dest.NotificationKind, opt => opt.MapFrom(src => src.NOTIFICATION_KIND))
                    .ForMember(dest => dest.Telephone, opt => opt.MapFrom(src => src.TELEPHONE))
                    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EMAIL))
                    .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => (GenderType?)src.GENDER))
                    .ReverseMap()
                    .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte?)src.OrganizationType))
                    .ForPath(dest => dest.UNIT_TYPE, opt => opt.MapFrom(src => (byte)src.UnitType))
                    .ForPath(dest => dest.GENDER, opt => opt.MapFrom(src => (byte?)src.Gender))
                    .IgnoreAllNonExisting();
            }
        }

        public class CaseFinishReasonClassificationProfile : AutoMapper.Profile
        {
            public CaseFinishReasonClassificationProfile()
            {
                CreateMap<CASE_FINISH_REASON_CLASSIFICATION, CaseFinishReasonClassification>()
                     .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                     .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                     .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                     .ForMember(dest => dest.IsEnabled, opt => opt.MapFrom(src => src.IS_ENABLED))
                     .ForMember(dest => dest.IsMultiple, opt => opt.MapFrom(src => src.IS_MULTIPLE))
                     .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.TITLE))
                     .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                     .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.KEY))
                     .ForMember(dest => dest.OrganizationType, opt => opt.MapFrom(src => (OrganizationType)src.ORGANIZATION_TYPE))
                     .ForMember(dest => dest.UpdateDateTime, opt => opt.MapFrom(src => src.UPDATE_DATETIME))
                     .ForMember(dest => dest.UpdateUserName, opt => opt.MapFrom(src => src.UPDATE_USERNAME))
                     .ForMember(dest => dest.CaseFinishReasonDatas, opt => opt.MapFrom(src => src.CASE_FINISH_REASON_DATA))
                     .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.ORDER))
                     .ForMember(dest => dest.IsRequired, opt => opt.MapFrom(src => src.IS_REQUIRED))
                     .ReverseMap()
                     .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte)src.OrganizationType))
                     .IgnoreAllNonExisting();
            }
        }

        public class CaseResumeProfile : AutoMapper.Profile
        {
            public CaseResumeProfile()
            {
                CreateMap<CASE_RESUME, CaseResume>()
                     .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                     .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                     .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                     .ForMember(dest => dest.CaseType, opt => opt.MapFrom(src => (CaseType)src.CASE_TYPE))
                     .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.CONTENT))
                     .ForMember(dest => dest.CaseID, opt => opt.MapFrom(src => src.CASE_ID))
                     .ReverseMap()
                     .ForPath(dest => dest.CASE_TYPE, opt => opt.MapFrom(src => (byte)src.CaseType))
                     .IgnoreAllNonExisting();
            }
        }

        public class CaseFinishReasonDataProfile : AutoMapper.Profile
        {
            public CaseFinishReasonDataProfile()
            {
                CreateMap<CASE_FINISH_REASON_DATA, CaseFinishReasonData>()
                     .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                     .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                     .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                     .ForMember(dest => dest.IsEnabled, opt => opt.MapFrom(src => src.IS_ENABLED))
                     .ForMember(dest => dest.UpdateDateTime, opt => opt.MapFrom(src => src.UPDATE_DATETIME))
                     .ForMember(dest => dest.UpdateUserName, opt => opt.MapFrom(src => src.UPDATE_USERNAME))
                     .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.TEXT))
                     .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.ORDER))
                     .ForMember(dest => dest.Default, opt => opt.MapFrom(src => src.DEFAULT))
                     .ForMember(dest => dest.ClassificationID, opt => opt.MapFrom(src => src.CLASSIFICATION_ID))
                     .ForMember(dest => dest.CaseFinishReasonClassification, opt => opt.MapFrom(src => src.CASE_FINISH_REASON_CLASSIFICATION))
                     .ReverseMap()
                     .ForPath(dest => dest.CASE_FINISH_REASON_CLASSIFICATION, opt => opt.MapFrom(src => default(CASE_FINISH_REASON_CLASSIFICATION)))
                     .IgnoreAllNonExisting();
            }
        }

        public class CaseItemProfile : AutoMapper.Profile
        {
            public CaseItemProfile()
            {
                CreateMap<CASE_ITEM, CaseItem>()
                    .ForMember(dest => dest.Case, opt => opt.MapFrom(src => src.CASE))
                    .ForMember(dest => dest.Item, opt => opt.MapFrom(src => src.ITEM))
                    .ForMember(dest => dest.ItemID, opt => opt.MapFrom(src => src.ITEM_ID))
                    .ForMember(dest => dest.JContent, opt => opt.MapFrom(src => src.JCONTENT))
                    .ForMember(dest => dest.CaseID, opt => opt.MapFrom(src => src.CASE_ID))
                    .ReverseMap()
                    .ForPath(dest => dest.CASE, opt => opt.MapFrom(src => default(CASE)))
                    .ForPath(dest => dest.ITEM, opt => opt.MapFrom(src => default(ITEM)))
                    .IgnoreAllNonExisting();
            }
        }

        public class CaseNoticeProfile : AutoMapper.Profile
        {
            public CaseNoticeProfile()
            {
                CreateMap<CASE_NOTICE, CaseNotice>()
                    .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                    .ForMember(dest => dest.ApplyUserID, opt => opt.MapFrom(src => src.APPLY_USER_ID))
                    .ForMember(dest => dest.CaseID, opt => opt.MapFrom(src => src.CASE_ID))
                    .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                    .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                    .ForMember(dest => dest.CaseNoticeType, opt => opt.MapFrom(src => (CaseNoticeType)src.CASE_NOTICE_TYPE))
                    .ReverseMap()
                    .ForPath(dest => dest.CASE_NOTICE_TYPE, opt => opt.MapFrom(src => (byte)src.CaseNoticeType))
                    .IgnoreAllNonExisting();
            }
        }

        public class Sp_GetCaseCustomerListProfile : AutoMapper.Profile
        {
            public Sp_GetCaseCustomerListProfile()
            {
                CreateMap<SP_CASE_SEARCH_CALLCENTER_Result, SP_GetCaseList>()
                    .ForMember(dest => dest.NodeKey, opt => opt.MapFrom(src => src.NodeKey))
                    .ForMember(dest => dest.NodeName, opt => opt.MapFrom(src => src.NodeName))
                    .ForMember(dest => dest.SourceType, opt => opt.MapFrom(src => (CaseSourceType?)src.SourceType))
                    .ForMember(dest => dest.CaseID, opt => opt.MapFrom(src => src.CaseID))
                    .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CreateDateTime))
                    .ForMember(dest => dest.IncomingDateTime, opt => opt.MapFrom(src => src.IncomingDateTime))
                    .ForMember(dest => dest.ExpectDateTime, opt => opt.MapFrom(src => src.ExpectDateTime))
                    .ForMember(dest => dest.ApplyUserName, opt => opt.MapFrom(src => src.ApplyUserName))
                    .ForMember(dest => dest.CaseType, opt => opt.MapFrom(src => (CaseType?)src.CaseType))
                    .ForMember(dest => dest.CaseWarningName, opt => opt.MapFrom(src => src.CaseWarningName))
                    .ForMember(dest => dest.IsPrevention, opt => opt.MapFrom(src => src.IsPrevention))
                    .ForMember(dest => dest.IsAttension, opt => opt.MapFrom(src => src.IsAttension))
                    .ForMember(dest => dest.CaseContent, opt => opt.MapFrom(src => src.CaseContent))
                    .ForMember(dest => dest.PromiseDateTime, opt => opt.MapFrom(src => src.PromiseDateTime))
                    .ForMember(dest => dest.FinishContent, opt => opt.MapFrom(src => src.FinishContent))
                    .ForMember(dest => dest.FinishDateTime, opt => opt.MapFrom(src => src.FinishDateTime))
                    .ForMember(dest => dest.ClassificationID, opt => opt.MapFrom(src => src.ClassificationID))
                    .ForMember(dest => dest.CaseConcatUsersList, opt => opt.Ignore())
                    .ForMember(dest => dest.CaseComplainedUsersList, opt => opt.Ignore())
                    .ForMember(dest => dest.CaseTagList, opt => opt.Ignore())
                    .ForMember(dest => dest.CaseFinishReasonDatas, opt => opt.Ignore())
                    .ReverseMap()
                    .IgnoreAllNonExisting();
            }
        }
        public class Sp_GetCaseHSListProfile : AutoMapper.Profile
        {
            public Sp_GetCaseHSListProfile()
            {
                CreateMap<SP_CASE_SEARCH_HEADQUARTER_STORE_Result, SP_GetCaseList>()
                    .ForMember(dest => dest.NodeKey, opt => opt.MapFrom(src => src.NodeKey))
                    .ForMember(dest => dest.NodeName, opt => opt.MapFrom(src => src.NodeName))
                    .ForMember(dest => dest.SourceType, opt => opt.MapFrom(src => (CaseSourceType?)src.SourceType))
                    .ForMember(dest => dest.CaseID, opt => opt.MapFrom(src => src.CaseID))
                    .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CreateDateTime))
                    .ForMember(dest => dest.CaseType, opt => opt.MapFrom(src => (CaseType?)src.CaseType))
                    .ForMember(dest => dest.CaseWarningName, opt => opt.MapFrom(src => src.CaseWarningName))
                    .ForMember(dest => dest.IsPrevention, opt => opt.MapFrom(src => src.IsPrevention))
                    .ForMember(dest => dest.IsAttension, opt => opt.MapFrom(src => src.IsAttension))
                    .ForMember(dest => dest.CaseContent, opt => opt.MapFrom(src => src.CaseContent))
                    .ForMember(dest => dest.PromiseDateTime, opt => opt.MapFrom(src => src.PromiseDateTime))
                    .ForMember(dest => dest.FinishDateTime, opt => opt.MapFrom(src => src.FinishDateTime))
                    .ForMember(dest => dest.FinishContent, opt => opt.MapFrom(src => src.FinishContent))
                    .ForMember(dest => dest.ClassificationID, opt => opt.MapFrom(src => src.ClassificationID))
                    .ForMember(dest => dest.CaseConcatUsersList, opt => opt.Ignore())
                    .ForMember(dest => dest.CaseComplainedUsersList, opt => opt.Ignore())
                    .ForMember(dest => dest.CaseTagList, opt => opt.Ignore())
                    .ForMember(dest => dest.CaseFinishReasonDatas, opt => opt.Ignore())
                    .ReverseMap()
                    .IgnoreAllNonExisting();
            }
        }
        #region  轉派案件查詢清單(客服)SP
        public class Sp_GetCaseAssignmentCustomerListProfile : AutoMapper.Profile
        {
            public Sp_GetCaseAssignmentCustomerListProfile()
            {
                CreateMap<SP_ASSIGNMENT_SEARCH_CALLCENTER_Result, SP_GetCaseAssignmentList>()
                    .ForMember(dest => dest.NodeKey, opt => opt.MapFrom(src => src.NodeKey))
                    .ForMember(dest => dest.NodeName, opt => opt.MapFrom(src => src.NodeName))
                    .ForMember(dest => dest.CaseID, opt => opt.MapFrom(src => src.CaseID))
                    .ForMember(dest => dest.AssignmentID, opt => opt.MapFrom(src => src.AssignmentID))
                    .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                    .ForMember(dest => dest.CaseType, opt => opt.MapFrom(src => (CaseType?)src.CaseType))
                    .ForMember(dest => dest.CaseWarningName, opt => opt.MapFrom(src => src.CaseWarningName))
                    .ForMember(dest => dest.IncomingDateTime, opt => opt.MapFrom(src => src.IncomingDateTime))
                    .ForMember(dest => dest.IsPrevention, opt => opt.MapFrom(src => src.IsPrevention))
                    .ForMember(dest => dest.IsAttension, opt => opt.MapFrom(src => src.IsAttension))
                    .ForMember(dest => dest.SourceType, opt => opt.MapFrom(src => (CaseSourceType?)src.CaseSourceType))
                    .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CreateDateTime))
                    .ForMember(dest => dest.CaseContent, opt => opt.MapFrom(src => src.CaseContent))
                    .ForMember(dest => dest.ExpectDateTime, opt => opt.MapFrom(src => src.ExpectDateTime))
                    .ForMember(dest => dest.PromiseDateTime, opt => opt.MapFrom(src => src.PromiseDateTime))
                    .ForMember(dest => dest.NoticeDateTime, opt => opt.MapFrom(src => src.NoticeDateTime))
                    .ForMember(dest => dest.NoticeContent, opt => opt.MapFrom(src => src.NoticeContent))
                    .ForMember(dest => dest.FinishDateTime, opt => opt.MapFrom(src => src.FinishDateTime))
                    .ForMember(dest => dest.FinishContent, opt => opt.MapFrom(src => src.FinishContent))
                    .ForMember(dest => dest.CloseDateTime, opt => opt.MapFrom(src => src.CloseDateTime))
                    .ForMember(dest => dest.CloseContent, opt => opt.MapFrom(src => src.CloseContent))
                    .ForMember(dest => dest.CloseUserName, opt => opt.MapFrom(src => src.CloseUserName))
                    
                    .ForMember(dest => dest.ApplyUserName, opt => opt.MapFrom(src => src.ApplyUserName))
                    .ForMember(dest => dest.ClassificationID, opt => opt.MapFrom(src => src.ClassificationID))
                    .AfterMap((src, dest) => dest.Decrypt())
                    .ReverseMap()
                    .BeforeMap((src, dest) => src.Encrypt())
                    .IgnoreAllNonExisting();
            }
        }
        public class Sp_GetCaseAssignmentInvoiceCustomerListProfile : AutoMapper.Profile
        {
            public Sp_GetCaseAssignmentInvoiceCustomerListProfile()
            {
                CreateMap<SP_ASSIGNMENT_COMPLAINT_INVOICE_CALLCENTER_Result, SP_GetCaseAssignmentList>()
                    .ForMember(dest => dest.NodeKey, opt => opt.MapFrom(src => src.NodeKey))
                    .ForMember(dest => dest.NodeName, opt => opt.MapFrom(src => src.NodeName))
                    .ForMember(dest => dest.CaseID, opt => opt.MapFrom(src => src.CaseID))
                    .ForMember(dest => dest.InvoiceID, opt => opt.MapFrom(src => src.InvoiceID))
                    .ForMember(dest => dest.InvoiceType, opt => opt.MapFrom(src => src.InvoiceType))
                    .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                    .ForMember(dest => dest.CaseType, opt => opt.MapFrom(src => (CaseType?)src.CaseType))
                    .ForMember(dest => dest.CaseWarningName, opt => opt.MapFrom(src => src.CaseWarningName))
                    .ForMember(dest => dest.IncomingDateTime, opt => opt.MapFrom(src => src.IncomingDateTime))
                    .ForMember(dest => dest.IsPrevention, opt => opt.MapFrom(src => src.IsPrevention))
                    .ForMember(dest => dest.IsAttension, opt => opt.MapFrom(src => src.IsAttension))
                    .ForMember(dest => dest.SourceType, opt => opt.MapFrom(src => (CaseSourceType?)src.CaseSourceType))
                    .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CreateDateTime))
                    .ForMember(dest => dest.CaseContent, opt => opt.MapFrom(src => src.CaseContent))
                    .ForMember(dest => dest.ExpectDateTime, opt => opt.MapFrom(src => src.ExpectDateTime))
                    .ForMember(dest => dest.PromiseDateTime, opt => opt.MapFrom(src => src.PromiseDateTime))
                    .ForMember(dest => dest.NoticeDateTime, opt => opt.MapFrom(src => src.NoticeDateTime))
                    .ForMember(dest => dest.NoticeContent, opt => opt.MapFrom(src => src.NoticeContent))
                    .ForMember(dest => dest.ApplyUserName, opt => opt.MapFrom(src => src.ApplyUserName))
                    .ForMember(dest => dest.ClassificationID, opt => opt.MapFrom(src => src.ClassificationID))
                    .ForMember(dest => dest.IdentityID, opt => opt.MapFrom(src => src.IdentityID))
                    .AfterMap((src, dest) => dest.Decrypt())
                    .ReverseMap()
                    .BeforeMap((src, dest) => src.Encrypt())
                    .IgnoreAllNonExisting();
            }
        }
        public class Sp_GetCaseAssignmentNoticeCustomerListProfile : AutoMapper.Profile
        {
            public Sp_GetCaseAssignmentNoticeCustomerListProfile()
            {
                CreateMap<SP_ASSIGNMENT_COMPLAINT_NOTICE_CALLCENTER_Result, SP_GetCaseAssignmentList>()
                    .ForMember(dest => dest.NodeKey, opt => opt.MapFrom(src => src.NodeKey))
                    .ForMember(dest => dest.NodeName, opt => opt.MapFrom(src => src.NodeName))
                    .ForMember(dest => dest.CaseID, opt => opt.MapFrom(src => src.CaseID))
                    .ForMember(dest => dest.CaseWarningName, opt => opt.MapFrom(src => src.CaseWarningName))
                    .ForMember(dest => dest.CaseType, opt => opt.MapFrom(src => (CaseType?)src.CaseType))
                    .ForMember(dest => dest.IncomingDateTime, opt => opt.MapFrom(src => src.IncomingDateTime))
                    .ForMember(dest => dest.IsPrevention, opt => opt.MapFrom(src => src.IsPrevention))
                    .ForMember(dest => dest.IsAttension, opt => opt.MapFrom(src => src.IsAttension))
                    .ForMember(dest => dest.SourceType, opt => opt.MapFrom(src => (CaseSourceType?)src.CaseSourceType))
                    .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CreateDateTime))
                    .ForMember(dest => dest.CaseContent, opt => opt.MapFrom(src => src.CaseContent))
                    .ForMember(dest => dest.ExpectDateTime, opt => opt.MapFrom(src => src.ExpectDateTime))
                    .ForMember(dest => dest.PromiseDateTime, opt => opt.MapFrom(src => src.PromiseDateTime))
                    .ForMember(dest => dest.NoticeDateTime, opt => opt.MapFrom(src => src.NoticeDateTime))
                    .ForMember(dest => dest.NoticeContent, opt => opt.MapFrom(src => src.NoticeContent))
                    .ForMember(dest => dest.ApplyUserName, opt => opt.MapFrom(src => src.ApplyUserName))
                    .ForMember(dest => dest.ClassificationID, opt => opt.MapFrom(src => src.ClassificationID))
                    .ForMember(dest => dest.IdentityID, opt => opt.MapFrom(src => src.IdentityID))
                    .AfterMap((src, dest) => dest.Decrypt())
                    .ReverseMap()
                    .BeforeMap((src, dest) => src.Encrypt())
                    .IgnoreAllNonExisting();
            }
        }
        public class Sp_GetCaseAssignmentCommuncateCustomerListProfile : AutoMapper.Profile
        {
            public Sp_GetCaseAssignmentCommuncateCustomerListProfile()
            {
                CreateMap<SP_ASSIGNMENT_COMPLAINT_COMMUNICATE_CALLCENTER_Result, SP_GetCaseAssignmentList>()
                    .ForMember(dest => dest.NodeKey, opt => opt.MapFrom(src => src.NodeKey))
                    .ForMember(dest => dest.NodeName, opt => opt.MapFrom(src => src.NodeName))
                    .ForMember(dest => dest.CaseID, opt => opt.MapFrom(src => src.CaseID))
                    .ForMember(dest => dest.CaseWarningName, opt => opt.MapFrom(src => src.CaseWarningName))
                    .ForMember(dest => dest.CaseType, opt => opt.MapFrom(src => (CaseType?)src.CaseType))
                    .ForMember(dest => dest.IncomingDateTime, opt => opt.MapFrom(src => src.IncomingDateTime))
                    .ForMember(dest => dest.IsPrevention, opt => opt.MapFrom(src => src.IsPrevention))
                    .ForMember(dest => dest.IsAttension, opt => opt.MapFrom(src => src.IsAttension))
                    .ForMember(dest => dest.SourceType, opt => opt.MapFrom(src => (CaseSourceType?)src.CaseSourceType))
                    .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CreateDateTime))
                    .ForMember(dest => dest.CaseContent, opt => opt.MapFrom(src => src.CaseContent))
                    .ForMember(dest => dest.ExpectDateTime, opt => opt.MapFrom(src => src.ExpectDateTime))
                    .ForMember(dest => dest.PromiseDateTime, opt => opt.MapFrom(src => src.PromiseDateTime))
                    .ForMember(dest => dest.NoticeDateTime, opt => opt.MapFrom(src => src.NoticeDateTime))
                    .ForMember(dest => dest.NoticeContent, opt => opt.MapFrom(src => src.NoticeContent))
                    .ForMember(dest => dest.ApplyUserName, opt => opt.MapFrom(src => src.ApplyUserName))
                    .ForMember(dest => dest.ClassificationID, opt => opt.MapFrom(src => src.ClassificationID))
                    .ForMember(dest => dest.IdentityID, opt => opt.MapFrom(src => src.IdentityID))
                    .AfterMap((src, dest) => dest.Decrypt())
                    .ReverseMap()
                    .BeforeMap((src, dest) => src.Encrypt())
                    .IgnoreAllNonExisting();
            }
        }

        #endregion

        #region  轉派案件查詢清單(總部、門市)SP
        public class Sp_GetCaseAssignmentHSListProfile : AutoMapper.Profile
        {
            public Sp_GetCaseAssignmentHSListProfile()
            {
                CreateMap<SP_ASSIGNMENT_SEARCH_HEADQUARTER_STORE_Result, SP_GetCaseAssignmentHSList>()
                    .ForMember(dest => dest.NodeKey, opt => opt.MapFrom(src => src.NodeKey))
                    .ForMember(dest => dest.NodeName, opt => opt.MapFrom(src => src.NodeName))
                    .ForMember(dest => dest.CaseID, opt => opt.MapFrom(src => src.CaseID))
                    .ForMember(dest => dest.AssignmentID, opt => opt.MapFrom(src => src.AssignmentID))
                    .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                    .ForMember(dest => dest.CaseType, opt => opt.MapFrom(src => (CaseType?)src.CaseType))
                    .ForMember(dest => dest.CaseWarningName, opt => opt.MapFrom(src => src.CaseWarningName))
                    .ForMember(dest => dest.IncomingDateTime, opt => opt.MapFrom(src => src.IncomingDateTime))
                    .ForMember(dest => dest.IsPrevention, opt => opt.MapFrom(src => src.IsPrevention))
                    .ForMember(dest => dest.IsAttension, opt => opt.MapFrom(src => src.IsAttension))
                    .ForMember(dest => dest.SourceType, opt => opt.MapFrom(src => (CaseSourceType?)src.CaseSourceType))
                    .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CreateDateTime))
                    .ForMember(dest => dest.CaseContent, opt => opt.MapFrom(src => src.CaseContent))
                    .ForMember(dest => dest.ExpectDateTime, opt => opt.MapFrom(src => src.ExpectDateTime))
                    .ForMember(dest => dest.PromiseDateTime, opt => opt.MapFrom(src => src.PromiseDateTime))
                    .ForMember(dest => dest.NoticeDateTime, opt => opt.MapFrom(src => src.NoticeDateTime))
                    .ForMember(dest => dest.NoticeContent, opt => opt.MapFrom(src => src.NoticeContent))
                    .ForMember(dest => dest.FinishDateTime, opt => opt.MapFrom(src => src.FinishDateTime))
                    .ForMember(dest => dest.FinishContent, opt => opt.MapFrom(src => src.FinishContent))
                    .ForMember(dest => dest.CloseDateTime, opt => opt.MapFrom(src => src.CloseDateTime))
                    .ForMember(dest => dest.CloseContent, opt => opt.MapFrom(src => src.CloseContent))
                    .ForMember(dest => dest.CloseUserName, opt => opt.MapFrom(src => src.CloseUserName))
                    .ForMember(dest => dest.ApplyUserName, opt => opt.MapFrom(src => src.ApplyUserName))
                    .ForMember(dest => dest.ClassificationID, opt => opt.MapFrom(src => src.ClassificationID))
                    .AfterMap((src, dest) => dest.Decrypt())
                    .ReverseMap()
                    .BeforeMap((src, dest) => src.Encrypt())
                    .IgnoreAllNonExisting();
            }
        }
        public class Sp_GetCaseAssignmentInvoiceHSListProfile : AutoMapper.Profile
        {
            public Sp_GetCaseAssignmentInvoiceHSListProfile()
            {
                CreateMap<SP_ASSIGNMENT_COMPLAINT_INVOICE_HEADQUARTER_STORE_Result, SP_GetCaseAssignmentHSList>()
                    .ForMember(dest => dest.NodeKey, opt => opt.MapFrom(src => src.NodeKey))
                    .ForMember(dest => dest.NodeName, opt => opt.MapFrom(src => src.NodeName))
                    .ForMember(dest => dest.CaseID, opt => opt.MapFrom(src => src.CaseID))
                    .ForMember(dest => dest.InvoiceID, opt => opt.MapFrom(src => src.InvoiceID))
                    .ForMember(dest => dest.InvoiceType, opt => opt.MapFrom(src => src.InvoiceType))
                    .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                    .ForMember(dest => dest.CaseType, opt => opt.MapFrom(src => (CaseType?)src.CaseType))
                    .ForMember(dest => dest.CaseWarningName, opt => opt.MapFrom(src => src.CaseWarningName))
                    .ForMember(dest => dest.IncomingDateTime, opt => opt.MapFrom(src => src.IncomingDateTime))
                    .ForMember(dest => dest.IsPrevention, opt => opt.MapFrom(src => src.IsPrevention))
                    .ForMember(dest => dest.IsAttension, opt => opt.MapFrom(src => src.IsAttension))
                    .ForMember(dest => dest.SourceType, opt => opt.MapFrom(src => (CaseSourceType?)src.CaseSourceType))
                    .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CreateDateTime))
                    .ForMember(dest => dest.CaseContent, opt => opt.MapFrom(src => src.CaseContent))
                    .ForMember(dest => dest.ExpectDateTime, opt => opt.MapFrom(src => src.ExpectDateTime))
                    .ForMember(dest => dest.PromiseDateTime, opt => opt.MapFrom(src => src.PromiseDateTime))
                    .ForMember(dest => dest.NoticeDateTime, opt => opt.MapFrom(src => src.NoticeDateTime))
                    .ForMember(dest => dest.NoticeContent, opt => opt.MapFrom(src => src.NoticeContent))
                    .ForMember(dest => dest.ApplyUserName, opt => opt.MapFrom(src => src.ApplyUserName))
                    .ForMember(dest => dest.ClassificationID, opt => opt.MapFrom(src => src.ClassificationID))
                    .ForMember(dest => dest.IdentityID, opt => opt.MapFrom(src => src.IdentityID))
                    .AfterMap((src, dest) => dest.Decrypt())
                    .ReverseMap()
                    .BeforeMap((src, dest) => src.Encrypt())
                    .IgnoreAllNonExisting();
            }
        }
        public class Sp_GetCaseAssignmentNoticeHSListProfile : AutoMapper.Profile
        {
            public Sp_GetCaseAssignmentNoticeHSListProfile()
            {
                CreateMap<SP_ASSIGNMENT_COMPLAINT_NOTICE_HEADQUARTER_STORE_Result, SP_GetCaseAssignmentHSList>()
                    .ForMember(dest => dest.NodeKey, opt => opt.MapFrom(src => src.NodeKey))
                    .ForMember(dest => dest.NodeName, opt => opt.MapFrom(src => src.NodeName))
                    .ForMember(dest => dest.CaseID, opt => opt.MapFrom(src => src.CaseID))
                    .ForMember(dest => dest.CaseWarningName, opt => opt.MapFrom(src => src.CaseWarningName))
                    .ForMember(dest => dest.CaseType, opt => opt.MapFrom(src => (CaseType?)src.CaseType))
                    .ForMember(dest => dest.IncomingDateTime, opt => opt.MapFrom(src => src.IncomingDateTime))
                    .ForMember(dest => dest.IsPrevention, opt => opt.MapFrom(src => src.IsPrevention))
                    .ForMember(dest => dest.IsAttension, opt => opt.MapFrom(src => src.IsAttension))
                    .ForMember(dest => dest.SourceType, opt => opt.MapFrom(src => (CaseSourceType?)src.CaseSourceType))
                    .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CreateDateTime))
                    .ForMember(dest => dest.CaseContent, opt => opt.MapFrom(src => src.CaseContent))
                    .ForMember(dest => dest.ExpectDateTime, opt => opt.MapFrom(src => src.ExpectDateTime))
                    .ForMember(dest => dest.PromiseDateTime, opt => opt.MapFrom(src => src.PromiseDateTime))
                    .ForMember(dest => dest.NoticeDateTime, opt => opt.MapFrom(src => src.NoticeDateTime))
                    .ForMember(dest => dest.NoticeContent, opt => opt.MapFrom(src => src.NoticeContent))
                    .ForMember(dest => dest.ApplyUserName, opt => opt.MapFrom(src => src.ApplyUserName))
                    .ForMember(dest => dest.ClassificationID, opt => opt.MapFrom(src => src.ClassificationID))
                    .ForMember(dest => dest.IdentityID, opt => opt.MapFrom(src => src.IdentityID))
                    .AfterMap((src, dest) => dest.Decrypt())
                    .ReverseMap()
                    .BeforeMap((src, dest) => src.Encrypt())
                    .IgnoreAllNonExisting();
            }
        }
        public class Sp_GetCaseAssignmentCommuncateHSListProfile : AutoMapper.Profile
        {
            public Sp_GetCaseAssignmentCommuncateHSListProfile()
            {
                CreateMap<SP_ASSIGNMENT_COMPLAINT_COMMUNICATE_HEADQUARTER_STORE_Result, SP_GetCaseAssignmentHSList>()
                    .ForMember(dest => dest.NodeKey, opt => opt.MapFrom(src => src.NodeKey))
                    .ForMember(dest => dest.NodeName, opt => opt.MapFrom(src => src.NodeName))
                    .ForMember(dest => dest.CaseID, opt => opt.MapFrom(src => src.CaseID))
                    .ForMember(dest => dest.CaseWarningName, opt => opt.MapFrom(src => src.CaseWarningName))
                    .ForMember(dest => dest.CaseType, opt => opt.MapFrom(src => (CaseType?)src.CaseType))
                    .ForMember(dest => dest.IncomingDateTime, opt => opt.MapFrom(src => src.IncomingDateTime))
                    .ForMember(dest => dest.IsPrevention, opt => opt.MapFrom(src => src.IsPrevention))
                    .ForMember(dest => dest.IsAttension, opt => opt.MapFrom(src => src.IsAttension))
                    .ForMember(dest => dest.SourceType, opt => opt.MapFrom(src => (CaseSourceType?)src.CaseSourceType))
                    .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CreateDateTime))
                    .ForMember(dest => dest.CaseContent, opt => opt.MapFrom(src => src.CaseContent))
                    .ForMember(dest => dest.ExpectDateTime, opt => opt.MapFrom(src => src.ExpectDateTime))
                    .ForMember(dest => dest.PromiseDateTime, opt => opt.MapFrom(src => src.PromiseDateTime))
                    .ForMember(dest => dest.NoticeDateTime, opt => opt.MapFrom(src => src.NoticeDateTime))
                    .ForMember(dest => dest.NoticeContent, opt => opt.MapFrom(src => src.NoticeContent))
                    .ForMember(dest => dest.ApplyUserName, opt => opt.MapFrom(src => src.ApplyUserName))
                    .ForMember(dest => dest.ClassificationID, opt => opt.MapFrom(src => src.ClassificationID))
                    .ForMember(dest => dest.IdentityID, opt => opt.MapFrom(src => src.IdentityID))
                    .AfterMap((src, dest) => dest.Decrypt())
                    .ReverseMap()
                    .BeforeMap((src, dest) => src.Encrypt())
                    .IgnoreAllNonExisting();
            }
        }

        #endregion

        public class CasePPCLifeProfile : AutoMapper.Profile
        {
            public CasePPCLifeProfile()
            {
                CreateMap<CASE_PPCLIFE, CasePPCLife>()
                    .ForMember(dest => dest.CaseID, opt => opt.MapFrom(src => src.CASE_ID))
                    .ForMember(dest => dest.ItemID, opt => opt.MapFrom(src => src.ITEM_ID))
                    .ForMember(dest => dest.IsIgnore, opt => opt.MapFrom(src => src.IS_IGNORE))
                    .ForMember(dest => dest.AllSameFinish, opt => opt.MapFrom(src => src.ALLSAME_FINISH))
                    .ForMember(dest => dest.DiffBatchNoFinish, opt => opt.MapFrom(src => src.DIFF_BATCHNO_FINISH))
                    .ForMember(dest => dest.NothingBatchNoFinish, opt => opt.MapFrom(src => src.NOTHINE_BATCHNO_FINISH))
                    .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                    .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                    .ForMember(dest => dest.PPCLifeEffectiveSummaries, opt => opt.MapFrom(src => src.PPCLIFE_EFFECTIVE_SUMMARY))
                    .ReverseMap()
                    .IgnoreAllNonExisting();
            }
        }
    }


}
