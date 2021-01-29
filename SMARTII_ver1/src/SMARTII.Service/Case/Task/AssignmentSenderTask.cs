using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Newtonsoft.Json;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Case;
using SMARTII.Domain.IO;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Notification.Email;
using SMARTII.Domain.Transaction;

namespace SMARTII.Service.Case.Task
{
    public class AssignmentSenderTask : IFlowableTask
    {


        private readonly ICaseAggregate _CaseAggregate;
        private readonly IIndex<NotificationType, INotificationProvider> _NotificationProviders;

        public AssignmentSenderTask(
            ICaseAggregate CaseAggregate, 
            IIndex<NotificationType, INotificationProvider> NotificationProviders)
        {
            _CaseAggregate = CaseAggregate;
            _NotificationProviders = NotificationProviders;
        }

        public async Task<IFlowable> Execute(IFlowable flowable, params object[] args)
        {

            var assignment = (CaseAssignment)flowable;
            var emailPayload = (EmailPayload)args[0];

            if (emailPayload !=null && emailPayload.IsAddCaseAttachment)
            {
                var con_ = new MSSQLCondition<CASE>(x => x.CASE_ID == assignment.CaseID);
                var caseData = _CaseAggregate.Case_T1_T2_.Get(con_);

                caseData.FilePath?.ToList().ForEach(x =>
                {
                    var str = FilePathFormatted.GetVirtualFilePath(x);
                    if (!string.IsNullOrEmpty(str))
                    {
                        emailPayload.FilePaths.Add(str);
                    }
                });
            }

            var con = new MSSQLCondition<CASE_ASSIGNMENT>(x => x.ASSIGNMENT_ID == assignment.AssignmentID && x.CASE_ID == assignment.CaseID);

            // 先找到既有的反應單單據
            var data = _CaseAggregate.CaseAssignment_T1_T2_.Get(con);

            new FileProcessInvoker((context) =>
            {

                using (var scope = TrancactionUtility.TransactionScope())
                {

                    // 進行發信
                    _NotificationProviders[NotificationType.Email].Send(
                    payload: emailPayload,
                    afterSend: AfterSenderHanlder(data, context));
                    
                
                    con.ActionModify(x =>
                    {
                        x.EML_FILE_PATH = data.EMLFilePath;
                    });


                    data = _CaseAggregate.CaseAssignment_T1_T2_.Update(con);

                    scope.Complete();
                }
            });


            return data;
        }

        public global::System.Threading.Tasks.Task Validator(IFlowable flowable, params object[] args)
        {
            throw new NotImplementedException();
        }


        private Action<Object> AfterSenderHanlder(CaseAssignment assignment, FileProcessContext context)
        {
            return (object obj) =>
            {
                var fileName = $"{Guid.NewGuid().ToString()}.eml";

                var emailBytes = (obj as byte[]);

                var physicalDirPath = FilePathFormatted.GetEmailSenderPhysicalDirPath();

                var virtualPath = FilePathFormatted.GetEmailSenderVirtualFilePath(fileName);

                var path = emailBytes.SaveAsFilePath(physicalDirPath, fileName);

                assignment.EMLFilePath = virtualPath;

                context.Paths.Add(path);
            };
        }

    }
}
