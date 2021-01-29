using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Notification.Email;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Domain.Transaction;
using SMARTII.Resource.Tag;
using SMARTII.Service.Master.Resolver;

namespace SMARTII.Service.Case.Task
{
    public class OfficialEmailCaseAssignTask : TaskBase, IFlowableTask
    {
        private readonly ICaseService _CaseService;
        private readonly ICaseAggregate _CaseAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly INotificationAggregate _NotificationAggregate;
        private readonly ICaseSourceFacade _CaseSourceFacade;
        private readonly ICaseFacade _CaseFacade;

        public OfficialEmailCaseAssignTask(ICaseService CaseService,
                                    ICaseAggregate CaseAggregate,
                                    IOrganizationAggregate OrganizationAggregate,
                                    INotificationAggregate NotificationAggregate,
                                    ICaseSourceFacade CaseSourceFacade,
                                    ICaseFacade CaseFacade) : base(OrganizationAggregate)
        {
            _CaseService = CaseService;
            _CaseAggregate = CaseAggregate;
            _OrganizationAggregate = OrganizationAggregate;
            _NotificationAggregate = NotificationAggregate;
            _CaseSourceFacade = CaseSourceFacade;
            _CaseFacade = CaseFacade;
        }

        public async Task<IFlowable> Execute(IFlowable flowable, params object[] args)
        {
            IFlowable result = null;

            var caseAssignmentBase = (CaseAssignmentBase)flowable;
            var officialEmailData = (OfficialEmailEffectivePayload)args[0];

            var user = ContextUtility.GetUserIdentity()?.Instance;

            _CaseFacade.CreateResume(caseAssignmentBase.CaseID, null, string.Format(SysCommon_lang.EMAIL_ADOPT_CASE_ASSIGNMENT, officialEmailData.Subject), OfficialEmail_lang.OFFICIAL_EMAIL_ADOPT, user);
            // 新增歷程後記錄&通知
            using (var scope = TrancactionUtility.NoTransactionScope())
            {
                global::System.Threading.Tasks.Task.Run(() =>
                {
                    _CaseFacade.CreateHistory(caseAssignmentBase.CaseID, null, OfficialEmail_lang.OFFICIAL_EMAIL_ADOPT, user);
                    _CaseFacade.CreatePersonalNotification(caseAssignmentBase.CaseID, PersonalNotificationType.CaseModify, user);
                });
            };
            

            // 刪除原有信件資訊
            _NotificationAggregate.OfficialEmailEffectivePayload_T1_T2_.Remove(x => x.MESSAGE_ID == officialEmailData.MessageID && x.NODE_ID == officialEmailData.NodeID);

            return result;
        }

        public async global::System.Threading.Tasks.Task Validator(IFlowable flowable, params object[] args)
        {

        }
    }
}
