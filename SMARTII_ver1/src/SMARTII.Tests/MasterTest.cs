using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using Autofac.Features.Indexed;
using ClosedXML.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using OpenPop.Pop3;
using Ptc.Data.Condition2;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Attribute;
using Ptc.Data.Condition2.Mssql.Class;
using Ptc.Data.Condition2.Mssql.Repository;
using SMARTII.COMMON_BU;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;
using SMARTII.Domain.Master;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Notification.Email;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Report;
using SMARTII.Domain.Security;
using SMARTII.Domain.Types;
using SMARTII.EShop;
using SMARTII.PPCLIFE.Domain;
using SMARTII.Service.Cache;
using SMARTII.Service.Organization.Resolver;
using SMARTII.Service.Report.Builder;
using Spire.Xls;

namespace SMARTII.Tests
{
    [TestClass]
    public class MasterTest
    {
        public MasterTest()
        {
            //// 初始化Autofac
            //var container = DIConfig.Init();

            //// 初始化AutoMapper
            //MapperConfig.Init(container);

            //// 初始化MSSQL CONDITION
            //DataAccessConfiguration.Configure(Condition2Config.Setups);

            //DIBuilder.SetContainer(container);
        }

        //var caseSource = new CaseSource();
        
       
        [TestMethod]
        public void Test()
        {
            //var services = DIBuilder.Resolve<IIndex<string, IFlow>>();
            //services[nameof(CaseIncomingFlow)].Run(caseSource);

            //var a = services[EssentialCache.BusinessKeyValue.ColdStone];
            //var b = services[EssentialCache.BusinessKeyValue.MisterDonut];
            //var c = services[EssentialCache.BusinessKeyValue.PPCLIFE];
            //var d = services[EssentialCache.BusinessKeyValue._21Century];

            //FileUtility.SaveAsFilePath(a.ComplaintedReport("a"), @"C:\測試用目錄\", "A.xlsx");
            //FileUtility.SaveAsFilePath(b.ComplaintedReport("b"), @"C:\測試用目錄\", "B.xlsx");
            //FileUtility.SaveAsFilePath(c.ComplaintedReport("c"), @"C:\測試用目錄\", "C.xlsx");
            //FileUtility.SaveAsFilePath(d.ComplaintedReport("d"), @"C:\測試用目錄\", "D.xlsx");

            var model = new TestSearchViewModel()
            {
                Channel = "13",
                CanReturn = true,
                Start = DateTime.Now.AddHours(-1),
                End = DateTime.Now.AddHours(1)
            };

            var con = new MSSQLCondition<PPCLIFE_Item>(model);

            var list = new List<PPCLIFE_Item>()
            {
                new PPCLIFE_Item(){
                    CanReturn = false,
                    Channel = "123917239817239"
                },
                new PPCLIFE_Item(){
                    CanReturn = false,
                    Channel = "4457898"
                },
                new PPCLIFE_Item(){
                    CanReturn = true,
                    Channel = null,
                    StopSalesDateTime = DateTime.Now
                }
            };

            var result = list.Query(con);
        }

        public class TestSearchViewModel
        {
            //[MSSQLFilter(nameof(PPCLIFE_Item.CanReturn), ExpressionType.Equal, PredicateType.And)]
            public bool CanReturn { get; set; }

            [MSSQLFilter("X => X.Particular.Channel.Contains(Value)", PredicateType.And)]
            public string Channel { get; set; }

            //[MSSQLFilter(nameof(PPCLIFE_Item.StopSalesDateTime), ExpressionType.GreaterThanOrEqual, PredicateType.And)]
            public DateTime Start { get; set; }

            //[MSSQLFilter(nameof(PPCLIFE_Item.StopSalesDateTime), ExpressionType.LessThanOrEqual, PredicateType.And)]
            public DateTime End { get; set; }
        }

        [TestMethod]
        public void TestResolver()
        {
            var service = DIBuilder.Resolve<IMasterAggregate>().CaseTemplate_T1_T2_;
            var resolver = DIBuilder.Resolve<OrganizationNodeResolver>();

            var list = service.GetList();
            var list1 = resolver.ResolveCollection(list);
        }

        [TestMethod]
        public void Test_AES_Security()
        {
            //var s1 = "ccc".AesEncryptBase64("kkk");

            //var s2 = s1.AesDecryptBase64("kkk");

            var kv = new CaseConcatUser()
            {
                Email = "motx2152000@ptc-nec.com.tw",
                Address = "民權東路六段",
                Mobile = "999999",
                Telephone = "02-27932710",
                UserName = "陳弘慈"
            };

            var renews = kv.Encrypt();
            var news = kv.Decrypt();
        }

        [TestMethod]
        public void Test_Item_GetList()
        {
            var c = DIBuilder.Resolve<IMSSQLRepository<ITEM, Item<ExpandoObject>>>();
            var result = c.GetList();

            var a = result.First().Particular.CastTo<PPCLIFE_Item>();
        }

        [TestMethod]
        public void Test_EmailSend()
        {
            var c = DIBuilder.Resolve<IIndex<NotificationType, INotificationProvider>>();

            var service = c[NotificationType.Email];

            var mail = new EmailPayload();
            mail.Sender = new Domain.Organization.ConcatableUser()
            {
                Email = "SMARTII_SE@ptc-nec.com.tw"
            };
            mail.Receiver = new List<Domain.Organization.ConcatableUser>()
            {
                new Domain.Organization.ConcatableUser()
            {
                Email = "motx2152000@ptc-nec.com.tw"
            }
            };
            mail.Title = "chen1";

            service.Send(mail);
        }

        [TestMethod]
        public void Test_EmailSends()
        {
            try
            {
                using (var msg = new MailMessage())
                {
                    msg.To.Add("carylin@ptc-nec.com.tw");//收件者
                    msg.From = new MailAddress("carylin@ptc-nec.com.tw");
                    msg.Subject = "測試主旨_TESTSSSS";//信件主旨

                    using (var client = new SmtpClient("smtp.ptc-nec.com.tw", 25))
                    {
                        client.Send(msg);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        [TestMethod]
        public void Test_SMSSend()
        {
            var c = DIBuilder.Resolve<IIndex<NotificationType, INotificationProvider>>();

            var service = c[NotificationType.SMS];


            service.Send(null);
        }


        [TestMethod]
        public void Test_BuEmail()
        {

            var service = DIBuilder.Resolve<IEmailService>();

            service.ReceiveEmail();

            var wait = 0;
        }


        [TestMethod]
        public void Test_DeleteEmail()
        {

            using (Pop3Client client = new Pop3Client())
            {
                // Connect to the server
                client.Connect("mail.ptc-nec.com.tw", 110, false);

                // Authenticate ourselves towards the server
                client.Authenticate("SMARTII_SE@ptc-nec.com.tw", "pTC12876266");


                var message = client.GetMessage(2);

                var test = message.FindAllAttachments();

                var a = 0;


                //// Get the number of messages on the POP3 server
                //int messageCount = client.GetMessageCount();

                //// Run trough each of these messages and download the headers
                //for (int messageItem = messageCount; messageItem > 0; messageItem--)
                //{

                //    client.GetMessage(1);

                //    var mesgID = client.GetMessageHeaders(1).MessageId;

                //    // If the Message ID of the current message is the same as the parameter given, delete that message
                //    if (mesgID == "0e040738da2f42fc9a820152961386df@P4G0115VM030.ptc-nec.com.tw")
                //    {
                //        // Delete
                //        client.DeleteMessage(messageItem);
                //    }
                //}
                

            }
            
        }

        [TestMethod]
        public void Test1()
        {
            var masterAggregate = DIBuilder.Resolve<IMasterAggregate>();

            var test = masterAggregate.Item_T1_T2_Expendo_.GetList(x => x.NODE_ID == 16).ToList();

            var item2 = test.Select(x =>
            {

                return x.Particular.CastTo<PPCLIFE_Item>();
            }).ToList();
            var item = test.First().Particular.CastTo<PPCLIFE_Item>();


        }



        [TestMethod]
        public void TestExcelToPDF()
        {
            try
            {
                var tempPdfPath = @"C:\南聯\test.pdf";

                using (var xlworkbook = new XLWorkbook())
                using (var workbook = new Workbook())
                using (var memStream = new MemoryStream())
                {
                    var sheet = xlworkbook.Worksheets.Add("test");
                    sheet.Cell(1, 1).Value = "testtesttesttesttesttesttest";

                    xlworkbook.SaveAs(memStream);


                    workbook.LoadFromStream(memStream);


                    workbook.SaveToFile(tempPdfPath, Spire.Xls.FileFormat.PDF);


                }

                using (var fs = new FileStream(tempPdfPath, FileMode.Open, FileAccess.Read))
                {
                    var bytes = new byte[fs.Length];
                    fs.Read(bytes, 0, bytes.Length);

                    fs.Close();
                }

                //File.Delete(tempPdfPath);

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [TestMethod]
        public void TestExcelPassword()
        {
            try
            {
                var pass = "123";
                var book = new XLWorkbook();
                var path = @"C:\南聯\password.xlsx";

                book.Protect(true, true, pass);

                var sheet = book.Worksheets.Add("test-pass");

                sheet.Cell(1, 1).Value = pass;


                //var protection = sheet.Protect(pass);
                //protection.InsertRows = true;
                //protection.InsertColumns = true;

                book.SaveAs(path);

                var test = book.IsPasswordProtected;

                //var bytes = book.ConvertBookToByte();

                //using (var fs = File.Create(path))
                //{
                //    fs.Write(bytes, 0, bytes.Length);
                //}
                book.Dispose();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [TestMethod]
        public void TestTemp()
        {

            var testStr = @"ㄏ哈哈哈哈哈 
[客服中心立案編號：C12312342124]";

            var str = @"
OPENPOINT 帳號：0939941958
姓名：李奕成
Email：leeyii.cheng@msa.hinet.net
聯絡電話：0939941958

手機型號：SM-A7050
手機作業系統：Android
版本：9
APP版本：4.0.0
反應內容：
兌換的東西跑了好幾家都換不到，快要到期了怎麼辦";




            var str1 = @"OPENPOINT 帳號：0933255871姓名：PeggyEmail：rshov0123@ptc-nec.com.tw聯絡電話：0933255871手機型號：iPhone 7 (GSM)手機作業系統：iOS版本：12.2APP版本：3.3.2反應內容：持隨行卡至星八克消費是否可以累積openpoint點數？";
            var bodyList = str1.Split(new string[] { "OPENPOINT 帳號：", "聯絡電話：", "手機作業系統：", "反應內容：" }, StringSplitOptions.None);

            var temp = str.Replace("\r", "").Replace("\n", "");

            var test = testStr.SpecificString(@"\[客服中心立案編號：", @"\]").FirstOrDefault();
            var test1 = str1.Split(new string[] { "手機作業系統：" }, StringSplitOptions.None)[1].Split(new string[] { "版本：" }, StringSplitOptions.None)[0];//.SpecificString("手機作業系統：", "\r\n版本：");
            var test2 = str1.SpecificString("版本：", "APP版本：").FirstOrDefault();//?.Replace("APP", "");

            var wait = 0;
        }


    }
    
}
