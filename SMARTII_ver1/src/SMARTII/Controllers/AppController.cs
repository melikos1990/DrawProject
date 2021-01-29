using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using SMARTII.Assist.Web;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Thread;
using SMARTII.Service.Cache;

namespace SMARTII.Controllers
{
    [AllowAnonymous]
    public class AppController : BaseApiController
    {
        private readonly ICommonAggregate _CommonAggregate;

        public AppController(ICommonAggregate CommonAggregate)
        {
            _CommonAggregate = CommonAggregate;
        }

        /// <summary>
        /// 由Client 傳回的Logger
        /// </summary>
        /// <param name="logs"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("Logger")]
        public async Task<IHttpActionResult> LoggerAsync(List<ClientLogInfo> logs)
        {
            try
            {
                // _CommonAggregate.Loggers["Email"].Error("1234444");

                logs?.ForEach(x =>
                {
                    // Process client's logger.
                });

                return await Ok().Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return await StatusCode(HttpStatusCode.InternalServerError).Async();
            }
        }

        [HttpGet]
        [ActionName("RefreshSystemParameter")]
        public async Task<IHttpActionResult> RefreshSystemParameterAsync()
        {
            try
            {

                DataStorage.Refresh();

                return await Ok().Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error("系統參數 API 重整失敗");
                _CommonAggregate.Logger.Error(ex);

                return await StatusCode(HttpStatusCode.InternalServerError).Async();
            }
        }
    }
}
