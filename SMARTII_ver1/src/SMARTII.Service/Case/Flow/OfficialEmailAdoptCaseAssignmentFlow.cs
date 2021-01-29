using System;
using System.Linq;
using System.Threading.Tasks;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Case;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Notification.Email;
using SMARTII.Domain.Organization;
using SMARTII.Domain.System;
using SMARTII.Domain.Transaction;
using SMARTII.Resource.Tag;
using SMARTII.Service.Case.Task;
using SMARTII.Service.Organization.Provider;

namespace SMARTII.Service.Case.Flow
{
    public class OfficialEmailAdoptCaseAssignmentFlow : IFlow
    {
        private readonly ISystemAggregate _SystemAggregate;
        private readonly CaseAssignmentNoticficationNoSendFlow _CaseAssignmentNoticficationNoSendFlow;
        private readonly CaseAssignmentCommunicationFlow _CaseAssignmentCommunicationFlow;
        private readonly OfficialEmailCaseAssignTask _OfficialEmailCaseAssignTask;
        private readonly HeaderQuarterNodeProcessProvider _HeaderQuarterNodeProcessProvider;

        public OfficialEmailAdoptCaseAssignmentFlow(ISystemAggregate SystemAggregate,
                                                    CaseAssignmentNoticficationNoSendFlow CaseAssignmentNoticficationNoSendFlow,
                                                    CaseAssignmentCommunicationFlow CaseAssignmentCommunicationFlow,
                                                    OfficialEmailCaseAssignTask OfficialEmailCaseAssignTask,
                                                    HeaderQuarterNodeProcessProvider HeaderQuarterNodeProcessProvider)
        {
            _SystemAggregate = SystemAggregate;
            _CaseAssignmentNoticficationNoSendFlow = CaseAssignmentNoticficationNoSendFlow;
            _CaseAssignmentCommunicationFlow = CaseAssignmentCommunicationFlow;
            _OfficialEmailCaseAssignTask = OfficialEmailCaseAssignTask;
            _HeaderQuarterNodeProcessProvider = HeaderQuarterNodeProcessProvider;
        }

        public async Task<IFlowable> Run(IFlowable flowable, params object[] args)
        {
            IFlowable result = null;

            var mailDomain = (OfficialEmailEffectivePayload)args[0];

            using (var scope = TrancactionUtility.TransactionScope())
            {
                // 取得BU識別值
                var term = (HeaderQuarterTerm)_HeaderQuarterNodeProcessProvider.GetTerm(
                            mailDomain.NodeID,
                            mailDomain.OrganizationType);

                var sysSetting = _SystemAggregate.SystemParameter_T1_T2_.Get(x => x.KEY == term.NodeKey && x.ID == EssentialCache.LayoutValue.MailCaseAssignmentNotcieTypeTemplate);

                if (sysSetting == null)
                    throw new Exception(Common_lang.BUSINESSS_NOT_FOUND);

                if (!EssentialCache.OfficialEmailAssignmentValue.All.Contains(sysSetting.Value))
                    throw new Exception(string.Format(SystemParameter_lang.SYSTEM_PARAMETER_SETTING_ERROR, EssentialCache.LayoutValue.MailCaseAssignmentNotcieTypeTemplate));

                var caseAssignmentProcessType = (CaseAssignmentProcessType)int.Parse(sysSetting.Value);
                //新增歷程
                switch (caseAssignmentProcessType)
                {
                    case CaseAssignmentProcessType.Notice: 
                        result = await _CaseAssignmentNoticficationNoSendFlow.Run(flowable);
                        break;

                    case CaseAssignmentProcessType.Communication:
                        var data = (CaseAssignmentBase)flowable;

                        var caseAssignmentCommunicate = new CaseAssignmentCommunicate()
                        {
                            CaseID = data.CaseID,
                            NodeID = data.NodeID,
                            OrganizationType = data.OrganizationType,
                            Content = data.Content,
                            EMLFilePath = data.EMLFilePath,
                        };

                        result = await _CaseAssignmentCommunicationFlow.Run(caseAssignmentCommunicate);
                        break;
                    default:
                        break;
                }

                //新增歷程後相關行為
                await _OfficialEmailCaseAssignTask.Execute(result, mailDomain);

                scope.Complete();
            }

            return result;
        }
    }
}

