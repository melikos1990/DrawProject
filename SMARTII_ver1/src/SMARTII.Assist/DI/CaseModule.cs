using Autofac;
using Autofac.Extras.AggregateService;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Case;
using SMARTII.Service.Case.Flow;
using SMARTII.Service.Case.Parser;
using SMARTII.Service.Case.Resolver;
using SMARTII.Service.Case.Task;
using SMARTII.Service.Organization.Provider;

namespace SMARTII.Assist.DI
{
    public class CaseModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAggregateService<ICaseAggregate>();

            #region flow

            builder.RegisterType<CaseAssignFinishFlow>()
                   .As<IFlow>()
                   .Keyed<IFlow>(nameof(CaseAssignFinishFlow))
                   .As<CaseAssignFinishFlow>()
                   .InstancePerDependency();

            builder.RegisterType<CaseAssignFlow>()
                   .As<IFlow>()
                   .Keyed<IFlow>(nameof(CaseAssignFlow))
                   .As<CaseAssignFlow>()
                   .InstancePerDependency();

            builder.RegisterType<CaseAssignProcessedFlow>()
                   .As<IFlow>()
                   .Keyed<IFlow>(nameof(CaseAssignProcessedFlow))
                   .As<CaseAssignProcessedFlow>()
                   .InstancePerDependency();

            builder.RegisterType<CaseAssignRefillFlow>()
                   .As<IFlow>()
                   .Keyed<IFlow>(nameof(CaseAssignRefillFlow))
                   .As<CaseAssignRefillFlow>()
                   .InstancePerDependency();

            builder.RegisterType<CaseFillingFlow>()
                   .As<IFlow>()
                   .Keyed<IFlow>(nameof(CaseFillingFlow))
                   .As<CaseFillingFlow>()
                   .InstancePerDependency();

            builder.RegisterType<CaseFinishedFlow>()
                   .As<IFlow>()
                   .Keyed<IFlow>(nameof(CaseFinishedFlow))
                   .As<CaseFinishedFlow>()
                   .InstancePerDependency();

            builder.RegisterType<CaseSourceIncomingFlow>()
                   .As<IFlow>()
                   .Keyed<IFlow>(nameof(CaseSourceIncomingFlow))
                   .As<CaseSourceIncomingFlow>()
                   .InstancePerDependency();

            builder.RegisterType<CaseComplaintInvoiceSenderFlow>()
                   .As<IFlow>()
                   .Keyed<IFlow>(nameof(CaseComplaintInvoiceSenderFlow))
                   .As<CaseComplaintInvoiceSenderFlow>()
                   .InstancePerDependency();

            builder.RegisterType<CaseComplaintInvoiceResendFlow>()
                   .As<IFlow>()
                   .Keyed<IFlow>(nameof(CaseComplaintInvoiceResendFlow))
                   .As<CaseComplaintInvoiceResendFlow>()
                   .InstancePerDependency();
       

            builder.RegisterType<CaseAssignRejectFlow>()
                   .As<IFlow>()
                   .Keyed<IFlow>(nameof(CaseAssignRejectFlow))
                   .As<CaseAssignRejectFlow>()
                   .InstancePerDependency();

            builder.RegisterType<CaseSourceCompleteIncomingFlow>()
                  .As<IFlow>()
                  .Keyed<IFlow>(nameof(CaseSourceCompleteIncomingFlow))
                  .As<CaseSourceCompleteIncomingFlow>()
                  .InstancePerDependency();

            builder.RegisterType<CaseUnlockFlow>()
                  .As<IFlow>()
                  .Keyed<IFlow>(nameof(CaseUnlockFlow))
                  .As<CaseUnlockFlow>()
                  .InstancePerDependency();



            builder.RegisterType<CaseAssignmentCommunicationFlow>()
                  .As<IFlow>()
                  .Keyed<IFlow>(nameof(CaseAssignmentCommunicationFlow))
                  .As<CaseAssignmentCommunicationFlow>()
                  .InstancePerDependency();

            builder.RegisterType<OfficialEmailAdoptCaseFlow>()
                  .As<IFlow>()
                  .Keyed<IFlow>(nameof(OfficialEmailAdoptCaseFlow))
                  .As<OfficialEmailAdoptCaseFlow>()
                  .InstancePerDependency();

            builder.RegisterType<OfficialEmailAdoptCaseAssignmentFlow>()
                  .As<IFlow>()
                  .Keyed<IFlow>(nameof(OfficialEmailAdoptCaseAssignmentFlow))
                  .As<OfficialEmailAdoptCaseAssignmentFlow>()
                  .InstancePerDependency();

            builder.RegisterType<CaseAssignmentNoticficationNoSendFlow>()
                  .As<IFlow>()
                  .Keyed<IFlow>(nameof(CaseAssignmentNoticficationNoSendFlow))
                  .As<CaseAssignmentNoticficationNoSendFlow>()
                  .InstancePerDependency();


            builder.RegisterType<CaseAssignmentSenderFlow>()
                   .As<IFlow>()
                   .Keyed<IFlow>(nameof(CaseAssignmentSenderFlow))
                   .As<CaseAssignmentSenderFlow>()
                   .InstancePerDependency();

            #endregion


            #region task

            builder.RegisterType<AssignFinishedTask>()
                   .As<IFlowableTask>()
                   .Keyed<IFlowableTask>(nameof(AssignFinishedTask))
                   .As<AssignFinishedTask>()
                   .InstancePerDependency();

            builder.RegisterType<AssignNotificationTask>()
                   .As<IFlowableTask>()
                   .Keyed<IFlowableTask>(nameof(AssignNotificationTask))
                   .As<AssignNotificationTask>()
                   .InstancePerDependency();


            builder.RegisterType<AssignCommunicateTask>()
                .As<IFlowableTask>()
                .Keyed<IFlowableTask>(nameof(AssignCommunicateTask))
                .As<AssignCommunicateTask>()
                .InstancePerDependency();

            builder.RegisterType<AssignProcessedTask>()
                   .As<IFlowableTask>()
                   .Keyed<IFlowableTask>(nameof(AssignProcessedTask))
                   .As<AssignProcessedTask>()
                   .InstancePerDependency();
            
            builder.RegisterType<AssignTask>()
                   .As<IFlowableTask>()
                   .Keyed<IFlowableTask>(nameof(AssignTask))
                   .As<AssignTask>()
                   .InstancePerDependency();

            builder.RegisterType<AssignRejectTask>()
                   .As<IFlowableTask>()
                   .Keyed<IFlowableTask>(nameof(AssignRejectTask))
                   .As<AssignRejectTask>()
                   .InstancePerDependency();

            builder.RegisterType<AssignRefillTask>()
                   .As<IFlowableTask>()
                   .Keyed<IFlowableTask>(nameof(AssignRefillTask))
                   .As<AssignRefillTask>()
                   .InstancePerDependency();


            builder.RegisterType<CaseFillingTask>()
                   .As<IFlowableTask>()
                   .Keyed<IFlowableTask>(nameof(CaseFillingTask))
                   .As<CaseFillingTask>()
                   .InstancePerDependency();

            builder.RegisterType<CaseFinishedTask>()
                   .As<IFlowableTask>()
                   .Keyed<IFlowableTask>(nameof(CaseFinishedTask))
                   .As<CaseFinishedTask>()
                   .InstancePerDependency();

            builder.RegisterType<ComplaintInvoiceSenderTask>()
                   .As<IFlowableTask>()
                   .Keyed<IFlowableTask>(nameof(ComplaintInvoiceSenderTask))
                   .As<ComplaintInvoiceSenderTask>()
                   .InstancePerDependency();

            builder.RegisterType<CaseProcessedTask>()
                   .As<IFlowableTask>()
                   .Keyed<IFlowableTask>(nameof(CaseProcessedTask))
                   .As<CaseProcessedTask>()
                   .InstancePerDependency();

            builder.RegisterType<ComplaintInvoiceResendTask>()
                   .As<IFlowableTask>()
                   .Keyed<IFlowableTask>(nameof(ComplaintInvoiceResendTask))
                   .As<ComplaintInvoiceResendTask>()
                   .InstancePerDependency();

            builder.RegisterType<CaseSourceCreateTask>()
                   .As<IFlowableTask>()
                   .Keyed<IFlowableTask>(nameof(CaseSourceCreateTask))
                   .As<CaseSourceCreateTask>()
                   .InstancePerDependency();

            builder.RegisterType<ComplaintInvoiceTask>()
                   .As<IFlowableTask>()
                   .Keyed<IFlowableTask>(nameof(ComplaintInvoiceTask))
                   .As<ComplaintInvoiceTask>()
                   .InstancePerDependency();

            builder.RegisterType<CaseUnlockTask>()
                 .As<IFlowableTask>()
                 .Keyed<IFlowableTask>(nameof(CaseUnlockTask))
                 .As<CaseUnlockTask>()
                 .InstancePerDependency();

            builder.RegisterType<OfficialEmailAdoptTask>()
                 .As<IFlowableTask>()
                 .Keyed<IFlowableTask>(nameof(OfficialEmailAdoptTask))
                 .As<OfficialEmailAdoptTask>()
                 .InstancePerDependency();

            builder.RegisterType<OfficialEmailCaseAssignTask>()
                 .As<IFlowableTask>()
                 .Keyed<IFlowableTask>(nameof(OfficialEmailCaseAssignTask))
                 .As<OfficialEmailCaseAssignTask>()
                 .InstancePerDependency();

            builder.RegisterType<AssignNotificationNoSendTask>()
                .As<IFlowableTask>()
                .Keyed<IFlowableTask>(nameof(AssignNotificationNoSendTask))
                .As<AssignNotificationNoSendTask>()
                .InstancePerDependency();


            builder.RegisterType<AssignmentSenderTask>()
                   .As<IFlowableTask>()
                   .Keyed<IFlowableTask>(nameof(AssignmentSenderTask))
                   .As<AssignmentSenderTask>()
                   .InstancePerDependency();

            #endregion


            #region Parser

            #region 轉派案件 (客服)
            builder.RegisterType<CaseAssignmentCallCenterData>()
                   .As<ICaseAssignmentCallCenterIntegrationData>()
                   .Keyed<ICaseAssignmentCallCenterIntegrationData>(nameof(CaseAssignmentCallCenterData))
                   .As<CaseAssignmentCallCenterData>()
                   .InstancePerDependency();

            builder.RegisterType<CaseAssignmentCallCenterInvoiceData>()
                   .As<ICaseAssignmentCallCenterIntegrationData>()
                   .Keyed<ICaseAssignmentCallCenterIntegrationData>(nameof(CaseAssignmentCallCenterInvoiceData))
                   .As<CaseAssignmentCallCenterInvoiceData>()
                   .InstancePerDependency();

            builder.RegisterType<CaseAssignmentCallCenterNoticeData>()
                   .As<ICaseAssignmentCallCenterIntegrationData>()
                   .Keyed<ICaseAssignmentCallCenterIntegrationData>(nameof(CaseAssignmentCallCenterNoticeData))
                   .As<CaseAssignmentCallCenterNoticeData>()
                   .InstancePerDependency();
            builder.RegisterType<CaseAssignmentCallCenterCommuncateData>()
                   .As<ICaseAssignmentCallCenterIntegrationData>()
                   .Keyed<ICaseAssignmentCallCenterIntegrationData>(nameof(CaseAssignmentCallCenterCommuncateData))
                   .As<CaseAssignmentCallCenterCommuncateData>()
                   .InstancePerDependency();
            #endregion

            #region 轉派案件 (總部、門市)
            builder.RegisterType<CaseAssignmentHSData>()
                   .As<ICaseAssignmentHSIntegrationData>()
                   .Keyed<ICaseAssignmentHSIntegrationData>(nameof(CaseAssignmentHSData))
                   .As<CaseAssignmentHSData>()
                   .InstancePerDependency();

            builder.RegisterType<CaseAssignmentHSInvoiceData>()
                   .As<ICaseAssignmentHSIntegrationData>()
                   .Keyed<ICaseAssignmentHSIntegrationData>(nameof(CaseAssignmentHSInvoiceData))
                   .As<CaseAssignmentHSInvoiceData>()
                   .InstancePerDependency();

            builder.RegisterType<CaseAssignmentHSNoticeData>()
                   .As<ICaseAssignmentHSIntegrationData>()
                   .Keyed<ICaseAssignmentHSIntegrationData>(nameof(CaseAssignmentHSNoticeData))
                   .As<CaseAssignmentHSNoticeData>()
                   .InstancePerDependency();

            builder.RegisterType<CaseAssignmentHSCommuncateData>()
                   .As<ICaseAssignmentHSIntegrationData>()
                   .Keyed<ICaseAssignmentHSIntegrationData>(nameof(CaseAssignmentHSCommuncateData))
                   .As<CaseAssignmentHSCommuncateData>()
                   .InstancePerDependency();

            #endregion

            #endregion

            builder.RegisterType<CaseSourceResolver>().InstancePerDependency();
            builder.RegisterType<CaseResolver>().InstancePerDependency();
            builder.RegisterType<TaskBase>().PropertiesAutowired();
        }
    }
}
