#define Test

using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;
using SMARTII.Domain.IO;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Notification.Email;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;

namespace SMARTII.Service.Case.Task
{
    public class OfficialEmailAdoptTask : TaskBase, IFlowableTask
    {
        private readonly ICaseAggregate _CaseAggregate;
        private readonly ICaseSourceFacade _CaseSourceFacade;
        private readonly INotificationAggregate _NotificationAggregate;

        public OfficialEmailAdoptTask(ICaseAggregate CaseAggregate,
                                    IOrganizationAggregate OrganizationAggregate,
                                    ICaseSourceFacade CaseSourceFacade,
                                    INotificationAggregate NotificationAggregate) : base(OrganizationAggregate)
        {
            _CaseAggregate = CaseAggregate;
            _CaseSourceFacade = CaseSourceFacade;
            _NotificationAggregate = NotificationAggregate;
        }

        public async Task<IFlowable> Execute(IFlowable flowable, params object[] args)
        {
            var caseSoureData = (CaseSource)flowable;
            var officialEmailData = (OfficialEmailEffectivePayload)args[0];
            var isNeedCaseNotice = (bool)args[1];

            var @case = caseSoureData.Cases.First();

            // 新增通知 & 案件歷程
            if (isNeedCaseNotice)
            {
                var caseNotice = new CaseNotice()
                {
                    CaseID = @case.CaseID,
                    ApplyUserID = @case.ApplyUserID,
                    CreateDateTime = DateTime.Now,
                    CreateUserName = ContextUtility.GetUserIdentity()?.Name,
                    CaseNoticeType = CaseNoticeType.OfficialEmail
                };

                _CaseAggregate.CaseNotice_T1_T2_.Add(caseNotice);

                var caseResume = new CaseResume()
                {
                    CaseID = @case.CaseID,
                    CaseType = @case.CaseType,
                    Content = string.Format(SysCommon_lang.EMAIL_ASSIGN_CASE_RESUME, ContextUtility.GetUserIdentity()?.Name,@case.ApplyUserName),
                    CreateDateTime = DateTime.Now,
                    CreateUserName = ContextUtility.GetUserIdentity()?.Name,
                };

                _CaseAggregate.CaseResume_T1_T2_.Add(caseResume);
            }

            // 將信件押上該信件實體路徑

            // 案件副檔
            var pathArray = new string[] { officialEmailData.FilePath };

            var con = new MSSQLCondition<CASE>(x => x.CASE_ID == @case.CaseID);

            con.ActionModify(x =>
            {
                x.FILE_PATH = x.FILE_PATH.InsertArraySerialize(pathArray);
            });

            _CaseAggregate.Case_T1_T2_.Update(con);

            //刪除原有信件資訊
            _NotificationAggregate.OfficialEmailEffectivePayload_T1_T2_.Remove(x => x.MESSAGE_ID == officialEmailData.MessageID && x.NODE_ID == officialEmailData.NodeID);

            return caseSoureData;
        }

        public async global::System.Threading.Tasks.Task Validator(IFlowable flowable, params object[] args)
        {

        }
    }
}
