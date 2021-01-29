using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Autofac.Features.Indexed;
using MoreLinq;
using MultipartDataMediaFormatter.Infrastructure;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Case;
using SMARTII.Domain.IO;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Notification.Email;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Report;
using SMARTII.Domain.Transaction;
using SMARTII.Resource.Tag;
using SMARTII.Service.Cache;
using SMARTII.Service.Organization.Provider;

namespace SMARTII.Service.Case.Task
{
    /// <summary>
    /// 反應單發送TASK
    /// </summary>
    public class ComplaintInvoiceSenderTask : IFlowableTask
    {
        private readonly ICaseAggregate _CaseAggregate;
        private readonly ICaseService _CaseService;
        private readonly IReportService _ReportService;
        private readonly IIndex<string, IReportProvider> _ReportProviders;
        private readonly HeaderQuarterNodeProcessProvider _HeaderQuarterNodeProcessProvider;
        private readonly IIndex<NotificationType, INotificationProvider> _NotificationProviders;

        public ComplaintInvoiceSenderTask(ICaseAggregate CaseAggregate,
                                          ICaseService CaseService,
                                          IIndex<string, IReportProvider> ReportProviders,
                                          HeaderQuarterNodeProcessProvider HeaderQuarterNodeProcessProvider,
                                          IReportService ReportService,
                                          IIndex<NotificationType, INotificationProvider> NotificationProviders)
        {
            _CaseAggregate = CaseAggregate;
            _CaseService = CaseService;
            _ReportService = ReportService;
            _ReportProviders = ReportProviders;
            _NotificationProviders = NotificationProviders;
            _HeaderQuarterNodeProcessProvider = HeaderQuarterNodeProcessProvider;
        }

        public async Task<IFlowable> Execute(IFlowable flowable, params object[] args)
        {
            IFlowable result = null;

            await Validator(flowable, args);

            var data = (CaseAssignmentComplaintInvoice)flowable;

            var emailPayload = (EmailPayload)args[0];

            using (var scope = TrancactionUtility.NoTransactionScope())
            {

                // 先找到既有的反應單單據
                data = _CaseAggregate.CaseAssignmentComplaintInvoice_T1_T2_.Get(x => x.ID == data.ID);

                // 找到檔案 , 並夾帶至payload 中
                data.FilePath?.ToList().ForEach(x =>
                {
                    var fileName = x.ParseFileName();

                    var physiclePath = FilePathFormatted.GetCaseComplaintInvoicePhysicalFilePath(
                        data.NodeID, data.CreateDateTime.ToString("yyyyMMdd"), data.CaseID, data.InvoiceID, fileName);

                    var fileInfo = new FileInfo(physiclePath);

                    var fileBytes = FileUtility.GetFileBytes(physiclePath);

                    emailPayload.Attachments.Add(new HttpFile(fileInfo.Name, MimeMapping.GetMimeMapping(fileInfo.Name), fileBytes));

                });

                emailPayload.Attachments = emailPayload.Attachments?.DistinctBy(x => x.FileName)?.ToList() ?? new List<HttpFile>();


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
                var term = (HeaderQuarterTerm)_HeaderQuarterNodeProcessProvider
                            .GetTerm(data.NodeID, data.OrganizationType);

                // 加密附件
                var reportAttach = _ReportService.GetComplaintReport(data.CaseID, data.InvoiceID, true);

                if (DataStorage.CaseInvoiceSheetPasswordDict.TryGetValue(term.NodeKey, out var password))
                {
                    var zipFileName = reportAttach.FileName.Split('.')[0];
                    reportAttach = new List<HttpFile>() { reportAttach }.GenerateZip($"{zipFileName}.zip", password);
                }

                emailPayload.Attachments.Add(reportAttach);

                // 組合更新物件
                data.CaseAssignmentComplaintInvoiceType = CaseAssignmentComplaintInvoiceType.Sended;
            }

            new FileProcessInvoker((context) =>
            {
                using (var scope = TrancactionUtility.TransactionScope())
                {
                    // 進行發信
                    _NotificationProviders[NotificationType.Email].Send(
                        payload: emailPayload,
                        afterSend: AfterSenderHanlder(data, context));


                    // 更新資料
                    var con = new MSSQLCondition<CASE_ASSIGNMENT_COMPLAINT_INVOICE>(
                                  x => x.INVOICE_ID == data.InvoiceID);

                    con.ActionModify(x =>
                    {
                        var ef = AutoMapper.Mapper.Map<CASE_ASSIGNMENT_COMPLAINT_INVOICE>(data);

                        x.TYPE = ef.TYPE;
                        x.EML_FILE_PATH = ef.EML_FILE_PATH;

                    });

                    result = _CaseAggregate.CaseAssignmentComplaintInvoice_T1_T2_.Update(con);



                    scope.Complete();
                }
            });

            return result;
        }

        public async global::System.Threading.Tasks.Task Validator(IFlowable flowable, params object[] args)
        {

            var emailPayload = (EmailPayload)args[0];

            if (emailPayload == null || emailPayload.Sender == null)
            {
                throw new NullReferenceException(Common_lang.EMAIL_SENDER_NOT_ALLOW_NULL);
            }
        }

        private Action<Object> AfterSenderHanlder(CaseAssignmentComplaintInvoice invoice, FileProcessContext context)
        {
            return (object obj) =>
            {
                var fileName = $"{Guid.NewGuid().ToString()}.eml";

                var emailBytes = (obj as byte[]);

                var physicalDirPath = FilePathFormatted.GetEmailSenderPhysicalDirPath();

                var virtualPath = FilePathFormatted.GetEmailSenderVirtualFilePath(fileName);

                var path = emailBytes.SaveAsFilePath(physicalDirPath, fileName);

                invoice.EMLFilePath = virtualPath;

                context.Paths.Add(path);
            };
        }
    }
}
