using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Threading.Tasks;
using SMARTII.Assist.Authentication;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Data;
using SMARTII.Domain.Report;
using SMARTII.Assist.Logger;
using SMARTII.Resource.Tag;
using SMARTII.Domain.Thread;
using SMARTII.Domain.Common;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SMARTII.PPCLIFE.Controllers
{
    [Authentication]
    [RoutePrefix("Api/PPCLIFE/Report")]
    public class PPCLIFE_ReportController : ApiController
    {
        public readonly IPPCLIFEFactory _PPCLIFEFactory;
        private readonly ICommonAggregate _CommonAggregate;

        public PPCLIFE_ReportController(IPPCLIFEFactory PPCLIFEFactory,
                                        ICommonAggregate CommonAggregate)
        {
            _PPCLIFEFactory = PPCLIFEFactory;
            _CommonAggregate = CommonAggregate;
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

                var bytes = await _PPCLIFEFactory.GenerateOnCallExcel(s.Value, e.Value);
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new ByteArrayContent(bytes);

                response.Content.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");


                response.Content.Headers.ContentDisposition =
                   new ContentDispositionHeaderValue("attachment") { FileName = "統一藥品來電紀錄-S5.xlsx".Encoding() };


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
