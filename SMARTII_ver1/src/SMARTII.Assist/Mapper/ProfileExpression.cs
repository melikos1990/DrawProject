using AutoMapper;

namespace SMARTII.Assist.Mapper
{
    public class ProfileExpression : Domain.Mapper.IProfileExpression
    {
        public void AddProfile(IMapperConfigurationExpression expression)
        {
            /////////////// MASTER PROFILES ///////////////////
            expression.AddProfile<MasterProfile.QuestionClassificationProfile>();
            expression.AddProfile<MasterProfile.QuestionClassificationAnswerProfile>();
            expression.AddProfile<MasterProfile.GuideProfile>();
            expression.AddProfile<MasterProfile.UserParameterProfile>();
            expression.AddProfile<MasterProfile.CustomerProfile>();
            expression.AddProfile<MasterProfile.KMClassificationProfile>();
            expression.AddProfile<MasterProfile.VW_KMClassificationProfile>();
            expression.AddProfile<MasterProfile.VW_QuestionClassificationProfileAnswer>();
            expression.AddProfile<MasterProfile.VW_QuestionClassificationGuideProfile>();
            expression.AddProfile<MasterProfile.KMDataProfile>();
            expression.AddProfile<MasterProfile.QueueProfile>();
            expression.AddProfile<MasterProfile.BillboardProfile>();
            expression.AddProfile<MasterProfile.VW_QuestionClassificationProfile>();
            expression.AddProfile<MasterProfile.WorkScheduleProfile>();
            

            /////////////// ORGANIZATION PROFILES ///////////////
            expression.AddProfile<OrganizationProfile.UserProfile>();
            expression.AddProfile<OrganizationProfile.RoleProfile>();
            expression.AddProfile<OrganizationProfile.JobProfile>();
            expression.AddProfile<OrganizationProfile.OrganizationNodeDefinitaionProfile>();
            expression.AddProfile<OrganizationProfile.CallCenterNodeProfile>();
            expression.AddProfile<OrganizationProfile.HeaderQuarterNodeProfile>();
            expression.AddProfile<OrganizationProfile.NodeBaseProfile>();
            expression.AddProfile<OrganizationProfile.VendorNodeProfile>();
            expression.AddProfile<OrganizationProfile.UserJobPositionProfile>();
            expression.AddProfile<OrganizationProfile.EnterpriseProfile>();

            ///////////////// USER PROFILES ///////////////////
            expression.AddProfile<CaseProfile.CaseAssignmentConcatUserProfile>();
            expression.AddProfile<CaseProfile.CaseAssignmentHistoryProfile>();
            expression.AddProfile<CaseProfile.CaseAssignmentResumeProfile>();
            expression.AddProfile<CaseProfile.CaseAssignmentProfile>();
            expression.AddProfile<CaseProfile.CaseAssignmentComplaintInvoiceProfile>();
            expression.AddProfile<CaseProfile.CaseAssignmentCommunicateProfile>();            
            expression.AddProfile<CaseProfile.CaseAssignmentCommunicateUserProfile>();            
            expression.AddProfile<CaseProfile.CaseAssignmentComplaintInvoiceUserProfile>();
            expression.AddProfile<CaseProfile.CaseAssignmentComplaintNoticeProfile>();
            expression.AddProfile<CaseProfile.CaseAssignmentComplaintNoticeUserProfile>();
            expression.AddProfile<CaseProfile.CaseAssignmentUserProfile>();
            expression.AddProfile<CaseProfile.CaseComplaintUserProfile>();
            expression.AddProfile<CaseProfile.CaseConcatUserProfile>();
            expression.AddProfile<CaseProfile.CaseHistoryProfile>();
            expression.AddProfile<CaseProfile.CaseRemindProfile>();
            expression.AddProfile<CaseProfile.CaseSourceHistoryProfile>();
            expression.AddProfile<CaseProfile.CaseSourceProfile>();
            expression.AddProfile<CaseProfile.CaseSourceUserProfile>();
            expression.AddProfile<CaseProfile.CaseWarningProfile>();
            expression.AddProfile<CaseProfile._CaseProfile>();
            expression.AddProfile<CaseProfile.CaseTemplateProfile>();
            expression.AddProfile<CaseProfile.CaseAssignGroupProfile>();
            expression.AddProfile<CaseProfile.CaseAssignGroupUserProfile>();
            expression.AddProfile<CaseProfile.CaseTagProfile>();
            expression.AddProfile<CaseProfile.CaseFinishReasonClassificationProfile>();
            expression.AddProfile<CaseProfile.CaseFinishReasonDataProfile>();
            expression.AddProfile<CaseProfile.CaseResumeProfile>();
            expression.AddProfile<CaseProfile.CaseItemProfile>();
            expression.AddProfile<CaseProfile.CaseNoticeProfile>();
            expression.AddProfile<CaseProfile.CasePPCLifeProfile>();

            ////////////// SYSTEM PROFILES ////////////////////
            expression.AddProfile<SystemProfile.SystemParameterProfile>();
            expression.AddProfile<SystemProfile.SystemLogProfile>();

            /////////////// NOTIFICATION PROFILES ///////////////
            expression.AddProfile<NotificationProfile.NotificationGroupProfile>();
            expression.AddProfile<NotificationProfile.NotificationGroupUserProfile>();
            expression.AddProfile<NotificationProfile.NotificationGroupResumeProfile>();
            expression.AddProfile<NotificationProfile.NotificationGroupEffectiveSummaryProfile>();
            expression.AddProfile<NotificationProfile.PersonalNotificationProfile>();
            expression.AddProfile<NotificationProfile.OfficialEmailGroupProfile>();
            expression.AddProfile<NotificationProfile.OfficialEmailEffectiveDataProfile>();
            expression.AddProfile<NotificationProfile.OfficialEmailHistoryProfile>();
            expression.AddProfile<NotificationProfile.PPCLifeEffectiveSummaryProfile>();
            expression.AddProfile<NotificationProfile.PPCLifeResumeProfile>();

            /////////////// Stored Procedure ///////////////
            expression.AddProfile<CaseProfile.Sp_GetCaseCustomerListProfile>();
            expression.AddProfile<CaseProfile.Sp_GetCaseHSListProfile>();
            expression.AddProfile<CaseProfile.Sp_GetCaseAssignmentCustomerListProfile>();
            expression.AddProfile<CaseProfile.Sp_GetCaseAssignmentInvoiceCustomerListProfile>();
            expression.AddProfile<CaseProfile.Sp_GetCaseAssignmentNoticeCustomerListProfile>();
            expression.AddProfile<CaseProfile.Sp_GetCaseAssignmentCommuncateCustomerListProfile>();
            expression.AddProfile<CaseProfile.Sp_GetCaseAssignmentHSListProfile>();
            expression.AddProfile<CaseProfile.Sp_GetCaseAssignmentInvoiceHSListProfile>();
            expression.AddProfile<CaseProfile.Sp_GetCaseAssignmentNoticeHSListProfile>();
            expression.AddProfile<CaseProfile.Sp_GetCaseAssignmentCommuncateHSListProfile>();



        }
    }
}
