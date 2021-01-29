using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Autofac.Features.Indexed;
using ClosedXML.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultipartDataMediaFormatter.Infrastructure;
using Newtonsoft.Json;
using Ptc.Data.Condition2;
using Ptc.Data.Condition2.Common;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.COMMON_BU;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Case;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Master;
using SMARTII.Domain.Notification.Email;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Report;
using SMARTII.Domain.System;
using SMARTII.Domain.Transaction;
using SMARTII.PPCLIFE.Domain;
using SMARTII.Service.Cache;
using SMARTII.Service.Case.Flow;
using SMARTII.Service.Master.Resolver;
using SMARTII.Service.Report.Builder;
using static SMARTII.Domain.Cache.EssentialCache;

namespace SMARTII.Tests
{
    [TestClass]
    public class CaseTest
    {
        private readonly IIndex<string, IFlow> _Flows;
        private readonly IIndex<string, IFlowableTask> _Tasks;
        private readonly ICaseAggregate _CaseAggregate;
        private readonly IMasterAggregate _IMasterAggregate;
        private readonly ISystemAggregate _SystemAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly ICaseService _CaseService;
        private readonly IReportService _ReportService;
        private readonly IImportStoreService _importStoreService;
        private readonly QuestionClassificationResolver _QuestionClassificationResolver;
        private readonly ExcelBuilder _builder;
        private readonly IBatchReportProvider _BatchReportProvider;

        public CaseTest()
        {
            var container = DIConfig.Init();
            MapperConfig.Init(container);
            DataAccessConfiguration.Configure(Condition2Config.Setups);
            DIBuilder.SetContainer(container);
            DataStorage.Initailize(container);

            _Flows = DIBuilder.Resolve<IIndex<string, IFlow>>();
            _Tasks = DIBuilder.Resolve<IIndex<string, IFlowableTask>>();
            _CaseAggregate = DIBuilder.Resolve<ICaseAggregate>();
            _OrganizationAggregate = DIBuilder.Resolve<IOrganizationAggregate>();
            _IMasterAggregate = DIBuilder.Resolve<IMasterAggregate>();
            _SystemAggregate = DIBuilder.Resolve<ISystemAggregate>();
            _CaseService = DIBuilder.Resolve<ICaseService>();
            _ReportService = DIBuilder.Resolve<IReportService>();
            _importStoreService = DIBuilder.Resolve<IImportStoreService>();
            _QuestionClassificationResolver = DIBuilder.Resolve<QuestionClassificationResolver>();
            _builder = new ExcelBuilder();
            _BatchReportProvider = DIBuilder.Resolve<IBatchReportProvider>();
        }
        [TestMethod]
        public void TestPPCLIFE()
        {
            _ReportService.Send21OnCallExcel(DateTime.Now.AddMonths(-10), DateTime.Now);
            DateTime parsedDate = DateTime.Parse("2020/01/31 23:59:59");
            _ReportService.SendASOOnCallExcel(parsedDate.AddMonths(-1).AddDays(1), parsedDate);
            _ReportService.SendColdStoneOnCallExcel( DateTime.Now.AddMonths(-10), DateTime.Now);

            _ReportService.SendDonutOnCallExcel(DateTime.Now.AddMonths(-10), DateTime.Now);
            _ReportService.SendEhopOnCallExcel(DateTime.Now.AddMonths(-10), DateTime.Now);
            _ReportService.SendICCOnCallExcel(DateTime.Now.AddMonths(-10), DateTime.Now);
            _ReportService.SendPPCLIFEBrandCalcExcel( DateTime.Now.AddMonths(-10), DateTime.Now);
            _ReportService.PPCLIFEBatchSendMail( DateTime.Now.AddMonths(-10), DateTime.Now,"日", BatchValue.PPCLIFEGroupMonth);
        }
        [TestMethod]
        public void TestDonut()
        {
            _ReportService.SendDonutOnCallExcel( DateTime.Now.AddMonths(-10), DateTime.Now);
        }
        [TestMethod]
        public void TestOpenPoint()
        {
            _ReportService.SendOpenPointOnCallExcel( DateTime.Now.AddMonths(-10), DateTime.Now);
        }
        [TestMethod]
        public void TestASO()
        {
            DateTime parsedDate = DateTime.Parse("2020/01/31 23:59:59");
            _ReportService.SendASOOnCallExcel(parsedDate.AddMonths(-1).AddDays(1), parsedDate);
        }
        [TestMethod]
        public void Test21()
        {
            _ReportService.Send21OnCallExcel( DateTime.Now.AddMonths(-10), DateTime.Now);
        }

        [TestMethod]
        public void Get_PromiseDateTime_Shouldbe_True()
        {
            // 工作小時為 100小時
            // 例假日為 Saturday,Sunday
            // 工作時段為 06:00-18:00
            // 特殊假日 :
            //      2019-12-07 00:00:00.000 (六) => 上班
            //      2019-12-05 00:00:00.000 (四) => 休假
            // 發生日期 : 2019/12/04 16:30

            //var term = new HeaderQuarterTerm(16, OrganizationType.HeaderQuarter, "002");

            //var service = new PromiseDateTimeStrategy(term);

            //var announce = new DateTime(2019, 12, 4, 16, 30, 0);

            //var result = service.Calculator(100, announce);
            var con = new MSSQLCondition<CASE>(x => x.CASE_ID == "00719121600002");

            con.IncludeBy(x => x.CASE_COMPLAINED_USER);
            con.IncludeBy(x => x.CASE_CONCAT_USER);
            con.IncludeBy(x => x.CASE_TAG);
            con.IncludeBy(x => x.CASE_ASSIGNMENT);
            con.IncludeBy(x => x.CASE_ASSIGNMENT_COMPLAINT_INVOICE);
            con.IncludeBy(x => x.CASE_ASSIGNMENT_COMPLAINT_NOTICE);

            var data = _CaseAggregate.Case_T1_T2_.Get(con);

            var c = AutoMapper.Mapper.Map<CASE>(data);
        }

        [TestMethod]
        public void CaseIncomeing_Can_Run()
        {
            var caseSource = new CaseSource()
            {
                NodeID = 24,
                CaseSourceType = CaseSourceType.Phone,
                IsTwiceCall = true,
                IsPrevention = true,
                VoiceID = null,
                VoiceLocator = null,
                Remark = "消費者與門市大吵 , 要客訴",
                RelationCaseIDs = new string[] { },  // 待檢驗項目
                IncomingDateTime = DateTime.Now.AddMinutes(-1),
                OrganizationType = OrganizationType.HeaderQuarter,
                CaseSourceUser = new CaseSourceUser()
                {
                    Address = "民權東路6段",
                    UnitType = UnitType.Customer,
                    Email = "motx2152000@gmail.com.tw",
                    Telephone = "0227912714",
                    TelephoneBak = "0227932710",
                    Mobile = "0926366680",
                    Gender = GenderType.Male,
                    UserName = "陳弘慈"
                }
            };

            var result = _Flows[nameof(CaseSourceIncomingFlow)].Run(caseSource);
        }

        [TestMethod]
        public void CaseFilling_Can_Run()
        {
            var @case = new Case()
            {
                NodeID = 24,
                OrganizationType = OrganizationType.HeaderQuarter,
                IsAttension = true,
                IsReport = true,
                CaseWarningID = 4,
                SourceID = "19120600001",
                RelationCaseIDs = new string[] { },
                QuestionClassificationID = 6,
                CaseTags = new List<CaseTag>()
                {
                    new CaseTag(){
                        Name = "fuck2",
                        Target = true,
                    },
                    new CaseTag(){
                        Name = "fuck",
                        Target = false,
                        ID = 5
                    },
                    new CaseTag(){
                        Name = "fuck1",
                        Target = false,
                        ID = 6
                    },
                },
                CaseConcatUsers = new List<CaseConcatUser>()
                {
                    new CaseConcatUser()
                    {
                         Address = "民權東路6段-1",
                         UnitType = UnitType.Customer,
                         Email = "motx2152000@gmail.com.tw",
                         Telephone = "0227912714",
                         TelephoneBak = "0227932710",
                         Mobile = "0926366680",
                         Gender = GenderType.Male,
                         UserName = "陳弘慈"
                    },
                    new CaseConcatUser()
                    {
                         Address = "民權東路6段-2",
                         UnitType = UnitType.Customer,
                         Email = "motx2152000@gmail.com.tw",
                         Telephone = "0227912714",
                         TelephoneBak = "0227932710",
                         Mobile = "0926366680",
                         Gender = GenderType.Male,
                         UserName = "黃冠儒"
                    }
                },
                CaseComplainedUsers = new List<CaseComplainedUser>()
                {
                    new CaseComplainedUser()
                    {
                         JobID = 1,
                         JobName = "店長",
                         NodeID = 17,
                         NodeName = "北區",
                         BUID = 16,
                         BUName = "ASO",
                         Address = "民權東路6段-2",
                         UnitType = UnitType.Customer,
                         Email = "motx2152000@gmail.com.tw",
                         Telephone = "0227912714",
                         TelephoneBak = "0227932710",
                         Mobile = "0926366680",
                         Gender = GenderType.Male,
                         CaseComplainedUserType = CaseComplainedUserType.Responsibility,
                         UserName = "黃冠儒"
                    }
                },
            };

            var result = _Flows[nameof(CaseFillingFlow)].Run(@case, false);
        }

        [TestMethod]
        public void CaseFinished_Can_Run()
        {
            var @case = new Case()
            {
                CaseID = "00119120600001",
                NodeID = 24,
                OrganizationType = OrganizationType.HeaderQuarter,
                IsAttension = true,
                IsReport = true,
                CaseWarningID = 4,
                SourceID = "19120400002",
                RelationCaseIDs = new string[] { "00219120500015" },
                QuestionClassificationID = 6,
                CaseTags = new List<CaseTag>()
                {
                    new CaseTag(){
                        Name = "fuck2",
                        Target = true,
                    },
                    new CaseTag(){
                        Name = "fuck",
                        Target = false,
                        ID = 5
                    },
                    new CaseTag(){
                        Name = "fuck1",
                        Target = false,
                        ID = 6
                    },
                },
                CaseConcatUsers = new List<CaseConcatUser>()
                {
                    new CaseConcatUser()
                    {
                         Address = "民權東路6段-1",
                         UnitType = UnitType.Customer,
                         Email = "motx2152000@gmail.com.tw",
                         Telephone = "0227912714",
                         TelephoneBak = "0227932710",
                         Mobile = "0926366680",
                         Gender = GenderType.Male,
                         UserName = "陳弘慈"
                    },
                    new CaseConcatUser()
                    {
                         Address = "民權東路6段-2",
                         UnitType = UnitType.Customer,
                         Email = "motx2152000@gmail.com.tw",
                         Telephone = "0227912714",
                         TelephoneBak = "0227932710",
                         Mobile = "0926366680",
                         Gender = GenderType.Male,
                         UserName = "黃冠儒"
                    }
                },
                CaseComplainedUsers = new List<CaseComplainedUser>()
                {
                    new CaseComplainedUser()
                    {
                         JobID = 1,
                         JobName = "店長",
                         NodeID = 17,
                         NodeName = "北區",
                         BUID = 16,
                         BUName = "ASO",
                         Address = "民權東路6段-2",
                         UnitType = UnitType.Customer,
                         Email = "motx2152000@gmail.com.tw",
                         Telephone = "0227912714",
                         TelephoneBak = "0227932710",
                         Mobile = "0926366680",
                         Gender = GenderType.Male,
                         UserName = "黃冠儒"
                    }
                },

                CaseFinishReasonDatas = new List<CaseFinishReasonData>()
                {
                    new CaseFinishReasonData(){
                        ID = 1
                    },
                    new CaseFinishReasonData(){
                        ID = 3
                    },
                    new CaseFinishReasonData(){
                        ID = 5
                    }
                }
            };
            var result = _Flows[nameof(CaseFinishedFlow)].Run(@case, false);
        }

        [TestMethod]
        public void CaseAssignment_Notice_Can_Run()
        {
            var notice = new CaseAssignmentComplaintNotice()
            {
                CaseID = "00119120600001",
                NodeID = 24,
                OrganizationType = OrganizationType.HeaderQuarter,
                Content = $"一、內政部警政署為規範警察機關處理性侵害案件，特訂定本處理原則。",
                Files = new List<HttpFile>(),
                NotificationBehaviors = new string[] { },
                Users = new List<CaseAssignmentComplaintNoticeUser>()
                {
                    new CaseAssignmentComplaintNoticeUser(){
                        Email = "motx2152000@ptc-nec.com.tw",
                        UserName = "陳弘慈",
                        UnitType = UnitType.Customer,
                        NotificationBehavior  = "0"
                    },
                    new CaseAssignmentComplaintNoticeUser(){
                        Email = "lin@ptc-nec.com.tw",
                        UserName = "林老師",
                        UnitType = UnitType.Customer,
                        NotificationBehavior  = "0"
                    }
                }
            };

            var payload = new EmailPayload();
            payload.Title = "測試";
            payload.Content = notice.Content;
            payload.Sender = new ConcatableUser()
            {
                Email = "motx2152000@ptc-nec.com.tw"
            };
            payload.Receiver = notice.Users.Cast<ConcatableUser>()?.ToList();
            payload.Attachments = notice.Files;

            var result = _Flows[nameof(CaseAssignFlow)].Run(notice, payload);
        }

        [TestMethod]
        public void CaseAssignment_Invoice_Can_Run()
        {
            var invoice = new CaseAssignmentComplaintInvoice()
            {
                CaseID = "00119120600001",
                NodeID = 24,
                OrganizationType = OrganizationType.HeaderQuarter,
                Content = $"一、內政部警政署為規範警察機關處理性侵害案件，特訂定本處理原則。",
                Files = new List<HttpFile>(),
                InvoiceType = "A",
                NotificationBehaviors = new string[] { },
                Users = new List<CaseAssignmentComplaintInvoiceUser>()
                {
                    new CaseAssignmentComplaintInvoiceUser(){
                        Email = "motx2152000@ptc-nec.com.tw",
                        UserName = "陳弘慈",
                        UnitType = UnitType.Customer,
                        NotificationBehavior  = "0"
                    },
                    new CaseAssignmentComplaintInvoiceUser(){
                        Email = "lin@ptc-nec.com.tw",
                        UserName = "林老師",
                        UnitType = UnitType.Customer,
                        NotificationBehavior  = "0"
                    }
                }
            };

            var result = _Flows[nameof(CaseAssignFlow)].Run(invoice);
        }

        [TestMethod]
        public void CompaintInvoiceSender_Can_Run()
        {
            var invoice = new CaseAssignmentComplaintInvoice()
            {
                CaseID = "00119120600001",
                NodeID = 24,
                OrganizationType = OrganizationType.HeaderQuarter,
                Content = $"一、內政部警政署為規範警察機關處理性侵害案件，特訂定本處理原則。",
                Files = new List<HttpFile>(),
                InvoiceType = "A",
                InvoiceID = "A001201912002",
                CreateDateTime = new DateTime(2019, 12, 06),
                NotificationBehaviors = new string[] { },
                NoticeUsers = new string[] { "正孝槍", "陳龍慈" },
                Users = new List<CaseAssignmentComplaintInvoiceUser>()
                {
                    new CaseAssignmentComplaintInvoiceUser(){
                        Email = "motx2152000@ptc-nec.com.tw",
                        UserName = "陳弘慈",
                        UnitType = UnitType.Customer,
                        NotificationBehavior  = "0"
                    },
                    new CaseAssignmentComplaintInvoiceUser(){
                        Email = "lin@ptc-nec.com.tw",
                        UserName = "林老師",
                        UnitType = UnitType.Customer,
                        NotificationBehavior  = "0"
                    }
                }
            };

            var payload = new EmailPayload();
            payload.Title = "測試";
            payload.Content = invoice.Content;
            payload.Sender = new ConcatableUser()
            {
                Email = "motx2152000@ptc-nec.com.tw"
            };
            payload.Receiver = invoice.Users.Cast<ConcatableUser>()?.ToList();
            payload.Attachments = new List<HttpFile>();
            payload.Attachments.AddRange(invoice.Files);

            var result = _Flows[nameof(CaseComplaintInvoiceSenderFlow)].Run(invoice, payload);
        }

        [TestMethod]
        public void CaseAssignment_Create_Can_Run()
        {
            var assignment = new CaseAssignment()
            {
                CaseID = "00119120600001",
                NodeID = 24,
                OrganizationType = OrganizationType.HeaderQuarter,
                Content = $"一、內政部警政署為規範警察機關處理性侵害案件，特訂定本處理原則。",
                Files = new List<HttpFile>(),
                CreateDateTime = new DateTime(2019, 12, 06),
                NotificationBehaviors = new string[] { },
                NoticeUsers = new string[] { "正孝槍", "陳龍慈" },
                CaseAssignmentConcatUsers = new List<CaseAssignmentConcatUser>()
                {
                    new CaseAssignmentConcatUser(){
                        Email = "motx2152000@ptc-nec.com.tw",
                        UserName = "陳弘慈",
                        TelephoneBak = "0227912071",
                        UnitType = UnitType.Customer,
                        NotificationBehavior  = "0"
                    },
                    new CaseAssignmentConcatUser(){
                        Email = "lin@ptc-nec.com.tw",
                        UserName = "林老師",
                        Telephone = "0227932710",
                        UnitType = UnitType.Customer,
                        NotificationBehavior  = "0"
                    }
                },
                CaseAssignmentUsers = new List<CaseAssignmentUser>()
                {
                    new CaseAssignmentUser(){
                        UnitType = UnitType.Organization,
                        NodeID = 24,
                        OrganizationType = OrganizationType.Vendor,
                        UserID = "8a873bd9-8124-4137-b615-e5a914184342",
                        UserName = "金城武",
                        NodeName = "B2C 組",
                    },
                    new CaseAssignmentUser(){
                        UnitType = UnitType.Organization,
                        NodeID = 19,
                        OrganizationType = OrganizationType.HeaderQuarter,
                        UserID = "99850c4d-db84-4d11-aa97-51903119df62",
                        UserName = "goo",
                        NodeName = "北一課",
                    },
                }
            };
            var result = _Flows[nameof(CaseAssignFlow)].Run(assignment);
        }

        [TestMethod]
        public void CaseAssignment_Process_Can_Run()
        {
            var assignment = new CaseAssignment()
            {
                CaseID = "00119120600001",
                NodeID = 24,
                OrganizationType = OrganizationType.HeaderQuarter,
                Content = $"我是處理內容ABC , 處理不好喔!!!",
                Files = new List<HttpFile>(),
                FinishContent = $"我是銷案內容BCD , 要這樣處理喔!!!",
                FinishFiles = new List<HttpFile>(),
                CreateDateTime = new DateTime(2019, 12, 06),
                NotificationBehaviors = new string[] { },
                NoticeUsers = new string[] { "正孝槍", "陳龍慈" },
                CaseAssignmentConcatUsers = new List<CaseAssignmentConcatUser>()
                {
                    new CaseAssignmentConcatUser(){
                        Email = "motx2152000@ptc-nec.com.tw",
                        UserName = "陳弘慈",
                        TelephoneBak = "0227912071",
                        UnitType = UnitType.Customer,
                        NotificationBehavior  = "0"
                    },
                    new CaseAssignmentConcatUser(){
                        Email = "lin@ptc-nec.com.tw",
                        UserName = "林老師",
                        Telephone = "0227932710",
                        UnitType = UnitType.Customer,
                        NotificationBehavior  = "0"
                    }
                },
                CaseAssignmentUsers = new List<CaseAssignmentUser>()
                {
                    new CaseAssignmentUser(){
                        UnitType = UnitType.Organization,
                        NodeID = 24,
                        OrganizationType = OrganizationType.Vendor,
                        UserID = "8a873bd9-8124-4137-b615-e5a914184342",
                        UserName = "金城武",
                        NodeName = "B2C 組",
                    },
                    new CaseAssignmentUser(){
                        UnitType = UnitType.Organization,
                        NodeID = 19,
                        OrganizationType = OrganizationType.HeaderQuarter,
                        UserID = "99850c4d-db84-4d11-aa97-51903119df62",
                        UserName = "goo",
                        NodeName = "北一課",
                    },
                }
            };
            var result = _Flows[nameof(CaseAssignProcessedFlow)].Run(assignment);
        }

        [TestMethod]
        public void CaseAssignment_Finished_Can_Run()
        {
            var assignment = new CaseAssignment()
            {
                CaseID = "00119120600001",
                NodeID = 24,
                OrganizationType = OrganizationType.HeaderQuarter,
                Content = $"我是處理內容ABC , 處理不好喔!!!",
                Files = new List<HttpFile>(),
                FinishContent = $"我是銷案內容BCD , 要這樣處理喔!!!",
                FinishFiles = new List<HttpFile>(),
                CreateDateTime = new DateTime(2019, 12, 06),
                NotificationBehaviors = new string[] { },
                NoticeUsers = new string[] { "正孝槍", "陳龍慈" },
                CaseAssignmentConcatUsers = new List<CaseAssignmentConcatUser>()
                {
                    new CaseAssignmentConcatUser(){
                        Email = "motx2152000@ptc-nec.com.tw",
                        UserName = "陳弘慈",
                        TelephoneBak = "0227912071",
                        UnitType = UnitType.Customer,
                        NotificationBehavior  = "0",
                        CaseComplainedUserType = CaseComplainedUserType.Responsibility
                    },
                    new CaseAssignmentConcatUser(){
                        Email = "lin@ptc-nec.com.tw",
                        UserName = "林老師",
                        Telephone = "0227932710",
                        UnitType = UnitType.Customer,
                        NotificationBehavior  = "0",
                        CaseComplainedUserType = CaseComplainedUserType.Responsibility
                    }
                },
                CaseAssignmentUsers = new List<CaseAssignmentUser>()
                {
                    new CaseAssignmentUser(){
                        UnitType = UnitType.Organization,
                        NodeID = 24,
                        OrganizationType = OrganizationType.Vendor,
                        UserID = "8a873bd9-8124-4137-b615-e5a914184342",
                        UserName = "金城武",
                        NodeName = "B2C 組",
                        IsApply = true,
                    },
                    new CaseAssignmentUser(){
                        UnitType = UnitType.Organization,
                        NodeID = 19,
                        OrganizationType = OrganizationType.HeaderQuarter,
                        UserID = "99850c4d-db84-4d11-aa97-51903119df62",
                        UserName = "goo",
                        NodeName = "北一課",
                    },
                }
            };
            var result = _Flows[nameof(CaseAssignFinishFlow)].Run(assignment);
        }

        [TestMethod]
        public void CaseAssignment_Reject_Can_Run()
        {
            var assignment = new CaseAssignment()
            {
                CaseID = "00119120600001",
                NodeID = 24,
                RejectType = RejectType.Undo,
                OrganizationType = OrganizationType.HeaderQuarter,
                Content = $"我是處理內容ABC , 處理不好喔!!!",
                Files = new List<HttpFile>(),
                FinishContent = $"我是銷案內容BCD , 要這樣處理喔!!!",
                FinishFiles = new List<HttpFile>(),
                CreateDateTime = new DateTime(2019, 12, 06),
                NotificationBehaviors = new string[] { },
                NoticeUsers = new string[] { "正孝槍", "陳龍慈" },
                CaseAssignmentConcatUsers = new List<CaseAssignmentConcatUser>()
                {
                    new CaseAssignmentConcatUser(){
                        Email = "motx2152000@ptc-nec.com.tw",
                        UserName = "陳弘慈",
                        TelephoneBak = "0227912071",
                        UnitType = UnitType.Customer,
                        NotificationBehavior  = "0",
                        CaseComplainedUserType = CaseComplainedUserType.Responsibility
                    },
                    new CaseAssignmentConcatUser(){
                        Email = "lin@ptc-nec.com.tw",
                        UserName = "林老師",
                        Telephone = "0227932710",
                        UnitType = UnitType.Customer,
                        NotificationBehavior  = "0",
                        CaseComplainedUserType = CaseComplainedUserType.Responsibility
                    }
                },
                CaseAssignmentUsers = new List<CaseAssignmentUser>()
                {
                    new CaseAssignmentUser(){
                        UnitType = UnitType.Organization,
                        NodeID = 24,
                        OrganizationType = OrganizationType.Vendor,
                        UserID = "8a873bd9-8124-4137-b615-e5a914184342",
                        UserName = "金城武",
                        NodeName = "B2C 組",
                    },
                    new CaseAssignmentUser(){
                        UnitType = UnitType.Organization,
                        NodeID = 19,
                        OrganizationType = OrganizationType.HeaderQuarter,
                        UserID = "99850c4d-db84-4d11-aa97-51903119df62",
                        UserName = "goo",
                        NodeName = "北一課",
                    },
                }
            };
            var result = _Flows[nameof(CaseAssignRejectFlow)].Run(assignment);
        }

        [TestMethod]
        public void Communicate_Can_Run()
        {
            var communicate = new CaseAssignmentCommunicate()
            {
                CaseID = "00119122100001",
                NodeID = 24,
                OrganizationType = OrganizationType.HeaderQuarter,
                Content = $"測試通知",
                Files = new List<HttpFile>(),
                NotificationBehaviors = new string[] { },
                EMLFilePath = "123"
            };
            var result = _Flows[nameof(CaseAssignmentCommunicationFlow)].Run(communicate);
        }

        [TestMethod]
        public void Case_Get()
        {
            var con = new MSSQLCondition<CASE_SOURCE>(x => x.SOURCE_ID == "19120600002");
            con.IncludeBy(x => x.CASE_SOURCE_USER);
            _CaseAggregate.CaseSource_T1_T2_.Remove(con);
        }

        [TestMethod]
        public void Case_Parsing_Template()
        {
            var con = new MSSQLCondition<CASE>(x => x.CASE_ID == "00119122100001");

            con.IncludeBy(x => x.CASE_COMPLAINED_USER);
            con.IncludeBy(x => x.CASE_CONCAT_USER);
            con.IncludeBy(x => x.CASE_TAG);
            con.IncludeBy(x => x.CASE_ASSIGNMENT);
            con.IncludeBy(x => x.CASE_ASSIGNMENT_COMPLAINT_INVOICE);
            con.IncludeBy(x => x.CASE_ASSIGNMENT_COMPLAINT_NOTICE);

            var data = _CaseAggregate.Case_T1_T2_.Get(con);

            var template = @"信件主旨: {{SourceEmailTitle}} 反應者: {{ConcatUser}} 抱怨 {{ComplaintUser}} 並通知 {{AssignmentNotifyUsers}} \r\n 案件資訊: {{CaseID}} \r\n 案件內容: {{CaseContent}} \r\n 轉派內容: {{AssignmentContent}}";

            var result = data.ParsingTemplate(template);

            //var emailStr = "File/GetOfficialEmail?buID=16&createTime=20191213130544207&fileName=客人詢問_可否維修鞋底.eml";
            //var parser = new CaseTemplateParser();
            //var test = parser.CaseEmailTitleParing(emailStr);

            var wait = 0;
        }

        [TestMethod]
        public void JoinRoom()
        {
            string caseID = "00720010300010";
            var user = new User()
            {
                UserID = "3833d55c-be0b-4dc6-8456-c7c51c077cd1",
                Name = "陳宏慈",
            };
            KeyValueInstance<string, User>.Room.Add(caseID, user);
            var user1 = new User()
            {
                UserID = "3833d55c-be0b-4dc6-8456-c7c51c077cd1",
                Name = "陳宏慈1",
            };
            KeyValueInstance<string, User>.Room.Add(caseID, user1);

            var user2 = new User()
            {
                UserID = "3833d55c-be0b-4dc6-8456-c7c51c077cd1",
                Name = "陳宏慈2",
            };
            KeyValueInstance<string, User>.Room.Add(caseID, user2);

            KeyValueInstance<string, User>.Room.Remove(caseID, user1, x => x.UserID != user.UserID);



        }
        [TestMethod]
        public async Task ExcelTest()
        {

            var con = new CaseCallCenterCondition()
            {
                NodeID = 24,
                ApplyUserName = "goo"
            };
            var @byte = await DIBuilder.Resolve<ICaseSearchService>().ExcelCaseForCustomer(con);

            var url = @"C:\Users\carylin\Desktop\test1.xlsx";

            using (FileStream fs = File.Open(url, FileMode.Create, FileAccess.ReadWrite))
            {
                fs.Write(@byte, 0, @byte.Length);
            }

        }

        [TestMethod]
        public void CaseDiff()
        {
            var con = new MSSQLCondition<CASE_RESUME>(x => x.ID == 0);
            var before = _CaseAggregate.CaseResume_T1_T2_.Get(con);


            var uCon = new MSSQLCondition<CASE_RESUME>(x => x.ID == 0);
            uCon.ActionModify(x =>
            {
                x.CASE_ID = "123";
                x.CASE_TYPE = (byte)CaseType.Finished;
                x.CONTENT = "我誰";
                x.CREATE_DATETIME = DateTime.Now;
                x.CREATE_USERNAME = "我瘋子";
            });
            _CaseAggregate.CaseResume_T1_T2_.Update(uCon);

            var cCon = new MSSQLCondition<CASE_RESUME>(x => x.ID == 0);
            var after = _CaseAggregate.CaseResume_T1_T2_.Get(cCon);

            var diff = $"{before.CustomerDiffFormatter(after)}";
            var ttttt = diff;
        }

        [TestMethod]
        public void CaseObjToString()
        {
            try
            {
                var test = new string[]
                {
                    "00719121600001",
                    //"00719121500016",
                    //"00719121500015",
                    //"00719121500014",
                    //"00719121500012",
                    //"00719121500009",
                    //"00719121500008",
                    //"00719121500007",
                    //"00719121500004",
                    //"00719121500003",
                    //"00719121500002",
                    //"00719121500001",
                    //"00119120600001",
                    //"00719120600001",
                };

                foreach (var caseID in test)
                {
                    var con = new MSSQLCondition<CASE>(x => x.CASE_ID == caseID);
                    con.IncludeBy(x => x.CASE_CONCAT_USER);
                    con.IncludeBy(x => x.CASE_COMPLAINED_USER);
                    con.IncludeBy(x => x.CASE_TAG);
                    con.IncludeBy(x => x.CASE_ITEM);
                    con.IncludeBy(x => x.CASE_FINISH_REASON_DATA);
                    con.IncludeBy(x => x.CASE_ASSIGNMENT);
                    con.IncludeBy(x => x.CASE_ASSIGNMENT.Select(g => g.CASE_ASSIGNMENT_CONCAT_USER));
                    con.IncludeBy(x => x.CASE_ASSIGNMENT.Select(g => g.CASE_ASSIGNMENT_USER));
                    con.IncludeBy(x => x.CASE_ASSIGNMENT_COMMUNICATE);
                    con.IncludeBy(x => x.CASE_ASSIGNMENT_COMPLAINT_INVOICE);
                    con.IncludeBy(x => x.CASE_ASSIGNMENT_COMPLAINT_INVOICE.Select(g => g.CASE_ASSIGNMENT_COMPLAINT_INVOICE_USER));
                    con.IncludeBy(x => x.CASE_ASSIGNMENT_COMPLAINT_NOTICE);
                    con.IncludeBy(x => x.CASE_ASSIGNMENT_COMPLAINT_NOTICE.Select(g => g.CASE_ASSIGNMENT_COMPLAINT_NOTICE_USER));
                    var before = _CaseAggregate.Case_T1_T2_.Get(con);

                    var diff = before.GetObjectContentFromDescription(null);
                    var ttttt = diff;

                    var caseHistory = new CaseHistory()
                    {
                        CaseID = caseID,
                        Content = diff,
                        CaseType = before.CaseType,
                        CreateDateTime = DateTime.Now,
                        CreateUserName = "SYS"
                    };

                    _CaseAggregate.CaseHistory_T1_T2_.Add(caseHistory);
                }

            }
            catch (Exception ex)
            {

            }

        }

        [TestMethod]
        public void CaseSourceObjToString()
        {
            //var con = new MSSQLCondition<CASE>(x => x.CASE_ID == "00719121600001");
            //var before = _CaseAggregate.Case_T1_T2_.Get(con);

            //var seed =
            //    _QuestionClassificationResolver.Resolve
            //    (
            //        before
            //    );

            //var diff = seed.GetObjectContentFromDescription(null);
            //var ttttt = diff;

            // TODO 通知負責人(PersonalNotification)
            var oCon = new MSSQLCondition<CALLCENTER_NODE>(x => x.NODE_ID == 32);
            oCon.IncludeBy(x => x.ORGANIZATION_NODE_DEFINITION.JOB.Select(y => y.NODE_JOB.Select(z => z.USER)));
            var groupNode = _OrganizationAggregate.CallCenterNode_T1_T2_.Get(oCon);

            var includeStr = "Select".NestedLambdaString("QUESTION_CLASSIFICATION1", "c => c.QUESTION_CLASSIFICATION1", 2);
            var test = typeof(QUESTION_CLASSIFICATION).IncludeExpression<Func<QUESTION_CLASSIFICATION, object>>(includeStr);

            var testtt = groupNode;
        }

        [TestMethod]
        public void ExcelExport()
        {
            var service = DIBuilder.Resolve<IReportService>();
            var result = service.GetComplaintReport("00220011600001", "");
            var url = @"C:\Users\melikos\Desktop\test.xlsx";

            using (FileStream fs = File.Open(url, FileMode.Create, FileAccess.ReadWrite))
            {
                fs.Write(result.Buffer, 0, result.Buffer.Length);
            }


        }

        [TestMethod]
        public async Task GetSummyDataAsync()
        {
            try
            {
                var xlWorkBook = new XLWorkbook();

                string s = "2019-01-21 13:26";

                DateTime star =
                    DateTime.ParseExact(s, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

                string e = "2020-03-21 13:26";

                DateTime end =
                    DateTime.ParseExact(e, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);


                SummaryInfoData summaryInfoData = new SummaryInfoData();

                //取得問題分類物件
                var includeStr = "Select".NestedLambdaString("QUESTION_CLASSIFICATION1", "c => c.QUESTION_CLASSIFICATION1", 10);
                var expression = await typeof(QUESTION_CLASSIFICATION).IncludeExpression<Func<QUESTION_CLASSIFICATION, object>>(includeStr);

                var con = new MSSQLCondition<QUESTION_CLASSIFICATION>();
                con.IncludeBy(expression);
                con.And(c => c.LEVEL == 1);
                con.And(c => c.NODE_ID == 16);
                //所有大分類項目
                List<QuestionClassification> qcfList = _IMasterAggregate.QuestionClassification_T1_T2_.GetList(con).ToList();

                summaryInfoData.QuestionClassifications.AddRange(qcfList);

                foreach (var item in qcfList)
                {
                    List<int> totalIDs = new List<int>();

                    //遞迴攤平項目子節點
                    var childs = item.Flatten(true).Where(x => x.Level != 1);
                    totalIDs.AddRange(childs.Select(c => c.ID));

                    //找出底下所有分類的案件數量
                    var conCase = new MSSQLCondition<CASE>();
                    conCase.And(c => c.NODE_ID == 16);
                    conCase.And(c => totalIDs.Contains(c.QUESION_CLASSIFICATION_ID));
                    conCase.IncludeBy(c => c.CASE_SOURCE);

                    conCase.And(c => c.CREATE_DATETIME >= star && c.CREATE_DATETIME <= end);

                    var @case = _CaseAggregate.Case_T1_T2_.GetListOfSpecific(conCase, x => new Case()
                    {
                        QuestionClassificationID = x.QUESION_CLASSIFICATION_ID,
                        CaseSource = new CaseSource()
                        {
                            CaseSourceType = (CaseSourceType)x.CASE_SOURCE.SOURCE_TYPE,
                            SourceID = x.SOURCE_ID,
                        },
                    });

                    summaryInfoData.Cases.Add((item.ID, @case.ToList()));
                }

                var _CaseSourceDict =
               _SystemAggregate.SystemParameter_T1_T2_
                               .GetList(x => x.ID == EssentialCache.CaseValue.CASE_SOURCE)?
                               .ToDictionary(x => x.Key, v =>
                               {
                                   return JsonConvert.DeserializeObject<List<SelectItem>>(v.Value);
                               });

                //加入來源項目
                summaryInfoData.SelectItems = _CaseSourceDict.FirstOrDefault(c => c.Key == "007").Value;

                var @byte = _builder
                         .AddWorkSheet(new DailyMonthSummaryWorksheet(), summaryInfoData, "月")
                         .Build();

                var url = @"C:\Users\melikos\Desktop\test1.xlsx";

                using (FileStream fs = File.Open(url, FileMode.Create, FileAccess.ReadWrite))
                {
                    fs.Write(@byte, 0, @byte.Length);
                }

                var dsdd = summaryInfoData;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [TestMethod]
        public async Task testeeee()
        {
            //ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();
            //var collection1 = new List<int>();
            //var collection2 = new List<int>();

            var test1 = Task.Run(() =>
            {

                using (var scope = TrancactionUtility.TransactionScope())
                {

                    _CaseAggregate.Case_T1_T2_.Operator(context =>
                    {
                        var db = (SMARTIIEntities)context;

                        //db.Configuration.LazyLoadingEnabled = false;

                        //var data = db.CASE.FirstOrDefault(x => x.CASE_ID == "00120010700002");

                        db.CASE_SOURCE_CODE.Add(new CASE_SOURCE_CODE { DATE = "200304", SERIAL_CODE = 1 });

                        //db.Entry(data).State = System.Data.Entity.EntityState.Unchanged;
                        //data.APPLY_USERNAME = "test";

                        db.SaveChanges();
                    });

                    _CaseAggregate.Case_T1_T2_.Operator(context =>
                    {
                        var db = (SMARTIIEntities)context;

                        //db.Configuration.LazyLoadingEnabled = false;

                        var data = db.CASE_SOURCE_CODE.FirstOrDefault(x => x.DATE == "200304");

                        //db.Entry(data).State = System.Data.Entity.EntityState.Unchanged;
                        //data.APPLY_USERNAME = "test";


                    });


                    scope.Complete();
                }


            });


            //var test2 = Task.Run(() =>
            //{

            //    _CaseAggregate.Case_T1_T2_.Operator(context =>
            //    {
            //        var db = (SMARTIIEntities)context;

            //        db.Configuration.LazyLoadingEnabled = false;

            //        var data = db.CASE.AsQueryable().FirstOrDefault(x => x.CASE_ID == "00120010700002");

            //            //db.Entry(data).State = System.Data.Entity.EntityState.Unchanged;
            //            //data.APPLY_USERNAME = "test";


            //        });

            //});

            await Task.WhenAll(test1); //test2

            //thread1.Start();
            //thread2.Start();

            //var task1 = Task.Run(() => _CaseService.Adopt("002401d5bfc4$74b43500$5e1c9f00$@com.tw", 32));
            //var task2 = Task.Run(() => _CaseService.Adopt("0dc877a8454d41e29d568c4c7d656cf2@P4G0115VM030.ptc-nec.com.tw", 32));

            //await Task.WhenAll(task1, task2);

            //var thread3 = new Thread(() =>
            //{
            //    cacheLock.EnterWriteLock();
            //});


            //thread1.Start();



            //Thread.Sleep(1000);
            //thread2.Start();

            //Thread.Sleep(1000);
            //thread3.Start();

            //var result = await _CaseService.Adopt("RRRRRR", 13);

        }


        [TestMethod]
        public void Test_CaseRemindReport()
        {
            _importStoreService.ImportStoreInformation();

            //_ReportService.CaseRemindNotification();

            //var payloads = new List<CaseRemind>()
            //{
            //    new CaseRemind{
            //        ActiveStartDateTime = DateTime.Now.AddYears(-1),
            //        ActiveEndDateTime = DateTime.Now.AddDays(-1),
            //        AssignmentID = 0,
            //        CaseID = "Test01110203",
            //        Content = "測試report",
            //        CreateUserName = "goo",
            //        Case = new Case{
            //            ApplyUserName = "goo",
            //            CaseWarning = new CaseWarning{ Name = "案件很緊急" }
            //        },
            //        Type = CaseRemindType.Warning,
            //    },
            //    new CaseRemind{
            //        ActiveStartDateTime = DateTime.Now.AddYears(-1),
            //        ActiveEndDateTime = DateTime.Now.AddDays(-1),
            //        CaseID = "Test999999999",
            //        Content = "測試內容過長測試內容過長測試內容過長測試內容過長測試內容過長測試內\r\n容過長測試內容過長測試內容過長測試內\r\n容過長測試內容過長測試內容過長測試內容過長測試內容過長測試內容過長測試內容過長測試內容過長測試內容過長測試內容過長測試內容過長測試內容過長測試內容過長",
            //        CreateUserName = "goo",
            //        Case = new Case{
            //            ApplyUserName = "goo",
            //            CaseWarning = new CaseWarning{ Name = "案件很緊急" }
            //        },
            //        Type = CaseRemindType.Warning,
            //    }
            //};

            //var @byte = _BatchReportProvider.GenerateCaseRemindReport(payloads);


            //var filePath = @"C:\Users\rshov0123\Desktop\SmartII\reportTest\test.xlsx";
            
            //using (FileStream fs = File.Open(filePath, FileMode.Create, FileAccess.ReadWrite))
            //{
            //    fs.Write(@byte, 0, @byte.Length);
            //}
            
        }

        [TestMethod]
        public void Test()
        {

            var con = new MSSQLCondition<CASE>(x => x.CASE_ID == "00320030500006");
            con.IncludeBy(x => x.CASE_COMPLAINED_USER);

            _CaseAggregate.Case_T1_T2_.Operator(context =>
            {
                var db = (SMARTIIEntities)context;

                db.Configuration.LazyLoadingEnabled = false;


                var query = db.CASE
                    .Include("CASE_CONCAT_USER")
                    .Include("CASE_COMPLAINED_USER")
                    .Where(x => x.CASE_ID == "00320030500006").FirstOrDefault();

                //09515684265
                var concatUsers = query.CASE_CONCAT_USER;

                var first = concatUsers.FirstOrDefault();

                first.MOBILE = "09515684654";

                query.CASE_CONCAT_USER = concatUsers;

                query.UPDATE_DATETIME = DateTime.Now; 

                db.SaveChanges();

                var wait = 0;

            });


            //var test = _CaseAggregate.Case_T1_T2_.Get(x => x.CASE_ID == "00320030500006");

            

        }

    }
}
