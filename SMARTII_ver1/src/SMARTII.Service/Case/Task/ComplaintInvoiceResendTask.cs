using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Autofac.Features.Indexed;
using MultipartDataMediaFormatter.Infrastructure;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Case;
using SMARTII.Domain.IO;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Notification.Email;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Report;
using SMARTII.Domain.Thread;
using SMARTII.Domain.Transaction;
using SMARTII.Resource.Tag;
using SMARTII.Service.Cache;
using SMARTII.Service.Organization.Provider;
using static SMARTII.Domain.Cache.EssentialCache;

namespace SMARTII.Service.Case.Task
{
    /// <summary>
    /// 反應單重送Task
    /// </summary>
    public class ComplaintInvoiceResendTask : IFlowableTask
    {
        private readonly ICaseAggregate _CaseAggregate;
        private readonly ICaseService _CaseService;
        private readonly IReportService _ReportService;
        private readonly ICaseFacade _CaseFacade;
        private readonly IIndex<string, IReportProvider> _ReportProviders;
        private readonly HeaderQuarterNodeProcessProvider _HeaderQuarterNodeProcessProvider;
        private readonly IIndex<NotificationType, INotificationProvider> _NotificationProviders;

        public ComplaintInvoiceResendTask(ICaseAggregate CaseAggregate,
                                          ICaseService CaseService,
                                          IReportService ReportService,
                                          ICaseFacade CaseFacade,
                                          IIndex<string, IReportProvider> ReportProviders,
                                          HeaderQuarterNodeProcessProvider HeaderQuarterNodeProcessProvider,
                                          IIndex<NotificationType, INotificationProvider> NotificationProviders)
        {
            _ReportService = ReportService;
            _CaseService = CaseService;
            _CaseAggregate = CaseAggregate;
            _CaseFacade = CaseFacade;
            _ReportProviders = ReportProviders;
            _NotificationProviders = NotificationProviders;
            _HeaderQuarterNodeProcessProvider = HeaderQuarterNodeProcessProvider;
        }

        public async Task<IFlowable> Execute(IFlowable flowable, params object[] args)
        {
            IFlowable result = null;

            var data = (CaseAssignmentComplaintInvoice)flowable;

            var emailPayload = (EmailPayload)args[0];

            await Validator(flowable, args);


            using (var scope = TrancactionUtility.NoTransactionScope())
            {

                var con = new MSSQLCondition<CASE_ASSIGNMENT_COMPLAINT_INVOICE>(x => x.ID == data.ID);
                con.IncludeBy(x => x.CASE_ASSIGNMENT_COMPLAINT_INVOICE_USER);


                // 先找到既有的反應單單據
                data = _CaseAggregate.CaseAssignmentComplaintInvoice_T1_T2_.Get(con);

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

                // 取得 BU 三碼
                var term = (HeaderQuarterTerm)_HeaderQuarterNodeProcessProvider
                            .GetTerm(data.NodeID, data.OrganizationType);

               
                var reportAttach = _ReportService.GetComplaintReport(data.CaseID, data.InvoiceID , true);
                //主旨
                var con2 = new MSSQLCondition<CASE>(x => x.CASE_ID == data.CaseID);
                con2.IncludeBy(x => x.CASE_COMPLAINED_USER);
                
                if (DataStorage.CaseInvoiceSheetPasswordDict.TryGetValue(term.NodeKey, out var password))
                {
                    var zipFileName = reportAttach.FileName.Split('.')[0];
                    reportAttach = new List<HttpFile>() { reportAttach }.GenerateZip($"{zipFileName}.zip", password);
                }

                emailPayload.Attachments.Add(reportAttach);

                // 組合新增物件
                data.CaseAssignmentComplaintInvoiceType = CaseAssignmentComplaintInvoiceType.Sended;
                data.CreateDateTime = DateTime.Now;
                data.CreateUserName = ContextUtility.GetUserIdentity().Name;
            }

            new FileProcessInvoker((context) =>
            {
                using (var scope = TrancactionUtility.TransactionScope())
                {
                    // 進行發信
                    _NotificationProviders[NotificationType.Email].Send(
                        payload: emailPayload,
                        afterSend: AfterSenderHanlder(data, context));

                    // 新增資料
                    result = _CaseAggregate.CaseAssignmentComplaintInvoice_T1_T2_.Add(data);

                    // TODO 寫入歷程
                    scope.Complete();
                }
            });

            // 新增歷程後記錄
            var user = ContextUtility.GetUserIdentity()?.Instance;
            _CaseFacade.CreateResume(data.CaseID, null, string.Format(SysCommon_lang.CASE_ASSIGNMENT_INVOICE_RESEND_RESUME, data.InvoiceID), Case_lang.CASE_ASSIGNMENT_INVOICE_RESEND, user);

            return result;
        }

        public async global::System.Threading.Tasks.Task Validator(IFlowable flowable, params object[] args)
        {
            var data = (CaseAssignmentComplaintInvoice)flowable;
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
