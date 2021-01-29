﻿using System;
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

namespace SMARTII.OpenPoint.Controllers
{
    [Authentication]
    [RoutePrefix("Api/OpenPoint/Report")]
    public class OpenPoint_ReportController : BaseApiController
    {
        private readonly ICommonAggregate _CommonAggregate;
        private readonly IOpenPointFactory _IOpenPointFactory;

        public OpenPoint_ReportController(ICommonAggregate CommonAggregate, IOpenPointFactory IOpenPointFactory)
        {
            this._CommonAggregate = CommonAggregate;
            this._IOpenPointFactory = IOpenPointFactory;
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

                var bytes = await _IOpenPointFactory.GetOnCallExcel(s.Value, e.Value);
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new ByteArrayContent(bytes);

                response.Content.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");


                response.Content.Headers.ContentDisposition =
                   new ContentDispositionHeaderValue("attachment") { FileName = "OpenPoint來電紀錄-S5.xlsx".Encoding() };


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
