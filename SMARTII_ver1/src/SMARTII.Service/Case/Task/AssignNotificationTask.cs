#define Test

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using Autofac.Features.Indexed;
using MultipartDataMediaFormatter.Infrastructure;
using Newtonsoft.Json;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Case;
using SMARTII.Domain.IO;
using SMARTII.Domain.Master;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Notification.Email;
using SMARTII.Domain.Thread;
using SMARTII.Domain.Transaction;
using SMARTII.Resource.Tag;
using SMARTII.Service.Organization.Provider;

namespace SMARTII.Service.Case.Task
{
    /// <summary>
    /// 一般通知建立TASK
    /// </summary>
    public class AssignNotificationTask : IFlowableTask
    {
        private readonly ICaseAggregate _CaseAggregate;
        private readonly IMasterAggregate _MasterAggregate;
        private readonly ICaseAssignmentFacade _CaseAssignmentFacade;
        private readonly HeaderQuarterNodeProcessProvider _HeaderQuarterNodeProcessProvider;
        private readonly IIndex<NotificationType, INotificationProvider> _NotificationProviders;

        public AssignNotificationTask(ICaseAggregate CaseAggregate,
                                      IMasterAggregate MasterAggregate,
                                      ICaseAssignmentFacade CaseAssignmentFacade,
                                      HeaderQuarterNodeProcessProvider HeaderQuarterNodeProcessProvider,
                                      IIndex<NotificationType, INotificationProvider> NotificationProviders)
        {
            _CaseAggregate = CaseAggregate;
            _MasterAggregate = MasterAggregate;
            _CaseAssignmentFacade = CaseAssignmentFacade;
            _NotificationProviders = NotificationProviders;
            _HeaderQuarterNodeProcessProvider = HeaderQuarterNodeProcessProvider;
        }

        public async Task<IFlowable> Execute(IFlowable flowable, params object[] args)
        {
            await Validator(flowable, args);

            IFlowable result = null;

            var data = (CaseAssignmentComplaintNotice)flowable;
            var emailPayload = (EmailPayload)args[0];

            if (emailPayload != null && emailPayload.IsAddCaseAttachment)
            {
                var con = new MSSQLCondition<CASE>(x => x.CASE_ID == data.CaseID);
                var caseData = _CaseAggregate.Case_T1_T2_.Get(con);

                caseData.FilePath?.ToList().ForEach(x =>
                {
                    var str = FilePathFormatted.GetVirtualFilePath(x);
                    if (!string.IsNullOrEmpty(str))
                    {
                        emailPayload.FilePaths.Add(str);
                    }
                });
            }


            // 取得 BU 三碼
            var term = _HeaderQuarterNodeProcessProvider.GetTerm(data.NodeID, data.OrganizationType);

            // 組入新增欄位
            data.CreateDateTime = DateTime.Now;
            data.CreateUserName = ContextUtility.GetUserIdentity()?.Name;
            data.Users?.ForEach(x => x.CaseID = data.CaseID);


            new FileProcessInvoker((context) =>
            {
                using (var scope = TrancactionUtility.TransactionScope())
                {
                    //有勾選email通知，才寄信通知
                    if (data.NotificationBehaviors != null && data.NotificationBehaviors.Contains(((int)NotificationUIType.OnEmal).ToString()))
                    {

                        // 進行發信
                        _NotificationProviders[NotificationType.Email].Send(
                                payload: emailPayload,
                                afterSend: AfterSenderHanlder(data, context));

                    }

                    // 寫入資料
                    result = _CaseAggregate.CaseAssignmentComplaintNotice_T1_T2_.Add(data);
                    data.ID = ((CaseAssignmentComplaintNotice)result).ID;


                    // 建立附件
                    var pathArray = FileSaverUtility.SaveAssignmentNoticeFiles(context, data);
                    data.FilePath = pathArray?.ToArray();


                    // 這邊需要將附件路徑更新回資料當中

                    var con = new MSSQLCondition<CASE_ASSIGNMENT_COMPLAINT_NOTICE>(x => x.ID == data.ID);

                    con.ActionModify(x =>
                    {
                        x.FILE_PATH = JsonConvert.SerializeObject(data.FilePath);

                    });

                    _CaseAggregate.CaseAssignmentComplaintNotice_T1_T2_.Update(con);

                    scope.Complete();

                }

            });


            return result;
        }

        public async global::System.Threading.Tasks.Task Validator(IFlowable flowable, params object[] args)
        {
            var data = (CaseAssignmentComplaintNotice)flowable;

            if (_CaseAggregate.Case_T1_T2_.HasAny(x => x.CASE_ID == data.CaseID && x.CASE_TYPE == (byte)CaseType.Finished))
                throw new NullReferenceException(Case_lang.CASE_ALREADY_FINISH);

            //有勾選email通知，才需判斷
            if (data.NotificationBehaviors != null && data.NotificationBehaviors.Contains(((int)NotificationUIType.OnEmal).ToString()))
            {
                var emailPayload = (EmailPayload)args[0];

                if (emailPayload == null || emailPayload.Sender == null)
                {
                    throw new NullReferenceException(Common_lang.EMAIL_SENDER_NOT_ALLOW_NULL);
                }
            }
        }



        private Action<Object> AfterSenderHanlder(CaseAssignmentComplaintNotice notice, FileProcessContext context)
        {
            return (object obj) =>
            {
                var fileName = $"{Guid.NewGuid().ToString()}.eml";

                var emailBytes = (obj as byte[]);

                var physicalDirPath = FilePathFormatted.GetEmailSenderPhysicalDirPath();

                var virtualPath = FilePathFormatted.GetEmailSenderVirtualFilePath(fileName);

                var path = emailBytes.SaveAsFilePath(physicalDirPath, fileName);

                notice.EMLFilePath = virtualPath;

                context.Paths.Add(path);
            };
        }
    }
}
