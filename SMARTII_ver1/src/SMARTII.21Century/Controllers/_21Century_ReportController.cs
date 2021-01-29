using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
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

namespace SMARTII._21Century.Controllers
{
    [Authentication]
    [RoutePrefix("Api/21CENTURY/Report")]
    public class _21Century_ReportController : BaseApiController
    {
        private readonly ICommonAggregate _CommonAggregate;
        private readonly I21Factory _I21Factory;

        public _21Century_ReportController(ICommonAggregate CommonAggregate, I21Factory I21Factory)
        {
            this._CommonAggregate = CommonAggregate;
            this._I21Factory = I21Factory;
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

                var bytes = await _I21Factory.GenerateOnCallExcel(s.Value, e.Value);
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new ByteArrayContent(bytes);

                response.Content.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");


                response.Content.Headers.ContentDisposition =
                   new ContentDispositionHeaderValue("attachment") { FileName = "21世紀來電紀錄-S5.xlsx".Encoding() };


                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.ms-excel");

                return response;
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(Report_lang.REPORT_CREATE_FAIL));

                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}
