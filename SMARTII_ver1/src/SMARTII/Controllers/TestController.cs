using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Http;
using ClosedXML.Excel;
using Newtonsoft.Json;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Assist.Authentication;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Case;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Master;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Report;
using SMARTII.Domain.Thread;
using SMARTII.Domain.Transaction;
using SMARTII.Resource.Report;
using SMARTII.Resource.Tag;
using SMARTII.Service.Notification.Provider;

namespace SMARTII.Controllers
{

    //[Authentication]
    public class TestController : ApiController
    {
        private readonly INotificationAggregate _NotificationAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly IMasterAggregate _MasterAggregate;
        private readonly INotificationPersonalFacade _NotificationPersonalFacade;
        private readonly ICaseSourceFacade _CaseSourceFacade;
        private readonly ICommonAggregate _CommonAggregate;

        public TestController(
            INotificationAggregate NotificationAggregate, 
            IOrganizationAggregate OrganizationAggregate, 
            IMasterAggregate MasterAggregate, 
            INotificationPersonalFacade NotificationPersonalFacade,
            ICaseSourceFacade CaseSourceFacade,
            ICommonAggregate CommonAggregate)
        {
            _NotificationAggregate = NotificationAggregate;
            _OrganizationAggregate = OrganizationAggregate;
            _MasterAggregate = MasterAggregate;
            _NotificationPersonalFacade = NotificationPersonalFacade;
            _CaseSourceFacade = CaseSourceFacade;
            _CommonAggregate = CommonAggregate;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> TestXlsNoneFile()
        {
            try
            {

                //var book = new XLWorkbook();


                //var sheet = book.Worksheets.Add("test-pass");

                //sheet.Cell(1, 1).Value = "testtesttesttesttesttesttest";

                //using (MemoryStream test = new MemoryStream())
                //{
                //    book.SaveAs(@"C:\南聯\活頁簿2.xlsx");
                //    //book.SaveAs(test);
                //    //bytes = test.ToArray();
                //}


                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);


                using (FileStream filesm = new FileStream(@"C:\南聯\活頁簿2.xlsx", FileMode.Open, FileAccess.Read))
                {
                    Byte[] bytes = new byte[filesm.Length];
                    filesm.Read(bytes, 0, (int)filesm.Length);
                    response.Content = new ByteArrayContent(bytes);
                }

                

                response.Content.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");


                response.Content.Headers.ContentDisposition =
                   new ContentDispositionHeaderValue("attachment") { FileName = "7777777.pdf" };


                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> TestXls()
        {
            try
            {
                var bytes = ReportUtility.ExcelToPDF(@"C:\南聯\活頁簿1.xlsx");
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new ByteArrayContent(bytes);

                response.Content.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");


                response.Content.Headers.ContentDisposition =
                   new ContentDispositionHeaderValue("attachment") { FileName = "7777777.pdf" };


                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> TestPdf()
        {
            try
            {
                var report = new ComplaintReport();
                report.CaseID = "A200154578";

                var s = ReportUtility.RazorToPDF(report, @"C:\tfs\SMARTII\src\SMARTII.Resource\Report\Complaint.cshtml");

                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new ByteArrayContent(s);
                response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = "123.pdf";
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// ForSignr 測試用 , 未來需移除
        /// </summary>
        /// <param name="masg"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IHttpResult> PushAllUser(string masg)
        {
            try
            {
                _NotificationAggregate.Providers[NotificationType.UI].Send(null);

                return new JsonResult<string>(null, true);
            }
            catch (Exception ex)
            {
                return await new JsonResult(
                    ex.PrefixMessage(Account_lang.ACCOUNT_LOGIN_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// ForSignr 測試用 , 未來需移除
        /// </summary>
        /// <param name="masg"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IHttpResult> RefrachCount(string userID)
        {
            try
            {
                var provider = (SignalRProvider)_NotificationAggregate.Providers[NotificationType.UI];

                provider.RefrachNotificationCount(userID);

                return new JsonResult<string>(null, true);
            }
            catch (Exception ex)
            {
                return await new JsonResult(
                    ex.PrefixMessage(Account_lang.ACCOUNT_LOGIN_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// ForSignr 測試用 , 未來需移除
        /// </summary>
        /// <param name="masg"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IHttpResult> TestNotification()
        {
            try
            {
                _NotificationPersonalFacade.BillBoardNotification();

                //var provider = (SignalRProvider)_NotificationAggregate.Providers[NotificationType.UI];

                //provider.RefrachNotificationCount(userID);

                return new JsonResult<string>(null, true);
            }
            catch (Exception ex)
            {
                return await new JsonResult(
                    ex.PrefixMessage(Account_lang.ACCOUNT_LOGIN_FAIL), false)
                    .Async();
            }
        }


        [HttpGet]
        public async Task<HttpResponseMessage> TestDualList()
        {
            try
            {
                var source = new List<dynamic>();

                for (var i = 1; i <= 10; i++)
                {
                    source.Add(new
                    {
                        key = i,
                        name = $"item#{i}"
                    });
                }

                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(source));

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<HttpResponseMessage> Test_Entity()
        {
            try
            {
                var con = new MSSQLCondition<VENDOR_NODE>();

                con.IncludeBy(x => x.HEADQUARTERS_NODE);
                con.And(x => x.NODE_ID == 2);

                var hList = _OrganizationAggregate.VendorNode_T1_T2_.GetList(con).ToList();

                con.ActionModify(g =>
                {
                    g.HEADQUARTERS_NODE = null;
                });

                _OrganizationAggregate.VendorNode_T1_T2_.UpdateRange(con);

                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent("");

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<HttpResponseMessage> Test_Dnd([Required] int buID, [Required] int parentID)
        {
            try
            {

                var con = new MSSQLCondition<QUESTION_CLASSIFICATION>(x => x.NODE_ID == buID && x.PARENT_ID == parentID);
                var data = _MasterAggregate.QuestionClassification_T1_T2_.GetList(con).ToList();


                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(Select2Response<QuestionClassification>.ToSelectItems(data, x => x.ID.ToString(), x => x.Name, x => x)));

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Authentication]
        public async Task<HttpResponseMessage> Test_Resp_Get_Unauthorized()
        {
            try
            {
                throw new Exception("Test");
            }
            catch (Exception ex)
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                response.Content = new StringContent(ex.Message);

                return response;
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Test_Resp_Post_Unauthorized()
        {
            try
            {
                throw new Exception("Test");
            }
            catch (Exception ex)
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                response.Content = new StringContent(ex.Message);

                return response;
            }
        }


        [HttpGet]
        [Authentication]
        public async Task<HttpResponseMessage> Test_Resp_Get_NotImplemented()
        {
            try
            {
                throw new Exception("Test");
            }
            catch (Exception ex)
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.NotImplemented);
                response.Content = new StringContent(ex.Message);

                return response;
            }
        }


        [HttpPost]
        public async Task<HttpResponseMessage> Test_Resp_Post_NotImplemented()
        {
            try
            {
                throw new Exception("Test");
            }
            catch (Exception ex)
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.NotImplemented);
                response.Content = new StringContent(ex.Message);

                return response;
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> Test_Stress()
        {
            try
            {
                
                

                using (var scope = TrancactionUtility.TransactionScope())
                {
                        var test = _CaseSourceFacade.GetSourceCode();
                    //using (var scope2 = new TransactionScope(scopeOption, options))
                    //{


                    //}


                    scope.Complete();
                    _CommonAggregate.Logger.Info($"CurrentThread ID => {Thread.CurrentThread.ManagedThreadId} Complete....");
                }

                return Ok();
            }
            catch (Exception ex)
            {
                
                _CommonAggregate.Logger.Info($"Error CurrentThread ID => {Thread.CurrentThread.ManagedThreadId} ....");
                _CommonAggregate.Logger.Error(ex);
                return InternalServerError();
            }
        }

    }
}
