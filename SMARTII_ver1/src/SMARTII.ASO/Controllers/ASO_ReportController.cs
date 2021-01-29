using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using SMARTII.Assist.Authentication;
using SMARTII.Assist.Logger;
using SMARTII.Assist.Web;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Report;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;

namespace SMARTII.ASO.Controllers
{
    [Authentication]
    [RoutePrefix("Api/ASO/Report")]
    public class ASO_ReportController : BaseApiController
    {
        private readonly ICommonAggregate _CommonAggregate;
        private readonly IASOFactory _ASOFactory;

        public ASO_ReportController(ICommonAggregate CommonAggregate, IASOFactory ASOFactory)
        {
            this._CommonAggregate = CommonAggregate;
            this._ASOFactory = ASOFactory;
        }

        [HttpGet]
        [Route("GetAsoReport")]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(Report_lang.REPORT_CREATE))]
        [ModelValidator(false)]
        public async Task<HttpResponseMessage> GetAsoReport(string ReportType, string DateRange)
        {
            try
            {
                var s = DateRange.StarTime();

                var e = DateRange.EndTime();

                string reportName = "";
                byte[] @byte = default(byte[]);
                if (ReportType == "0")
                {
                    reportName = "ASO客服日報-S5.xlsx";
                    @byte = await _ASOFactory.GenerateEveryDayExcel(s.Value, e.Value);
                }
                else if (ReportType == "1")
                {
                    reportName = "ASO 0800客服 來電紀錄(時效)-S5.xlsx";
                    @byte = await _ASOFactory.GenerateAssignmentOnCallExcel(s.Value, e.Value);
                }

                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

                response.Content = new ByteArrayContent(@byte);
                response.Content.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");
                response.Content.Headers.ContentDisposition =
                   new ContentDispositionHeaderValue("attachment") { FileName = reportName.Encoding() };
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.ms-excel");

                return response;
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixMessage(Report_lang.REPORT_CREATE_FAIL));

                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("GetReport")]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(Report_lang.REPORT_CREATE))]
        [ModelValidator(false)]
        public async Task<HttpResponseMessage> GetReport(string DateRange)
        {
            try
            {
                var s = DateRange.StarTime();

                var e = DateRange.EndTime();
                
                var bytes = await _ASOFactory.GenerateOnCallExcel(s.Value, e.Value);
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new ByteArrayContent(bytes);

                response.Content.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");


                response.Content.Headers.ContentDisposition =
                   new ContentDispositionHeaderValue("attachment") { FileName = "ASO 0800客服 來電紀錄-S5.xlsx".Encoding() };


                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.ms-excel");

                return response;
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixMessage(Report_lang.REPORT_CREATE_FAIL));

                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}
