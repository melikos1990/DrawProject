using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using SMARTII.Assist.Authentication;
using SMARTII.Assist.Web;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Thread;

namespace SMARTII.Areas.Select.Controllers
{
    [Authentication]
    public class MiscController : ApiController
    {
        public readonly ICommonAggregate _CommonAggregate;

        public MiscController(ICommonAggregate CommonAggregate)
        {
            _CommonAggregate = CommonAggregate;
        }

        /// <summary>
        /// 取得系統功能清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetAppFeatures")]
        public async Task<IHttpActionResult> GetAppFeaturesAsync(Select2Request model)
        {
            try
            {
                Assembly[] assemblys = GlobalizationCache.Instance.AssemblyDict.Values.ToArray();

                var dict = assemblys.GetControllerDict();

                var select2 = new Select2Response()
                {
                    items = Select2Response.ToSelectItems(dict, x => x.Key, x => x.Value)
                            .Skip(model.start)
                            .Take(model.size)
                            .ToList()
                };

                if (string.IsNullOrEmpty(model.keyword) == false)
                {
                    select2.items = select2.items.Where(x => x.text.Contains(model.keyword)).ToList();
                };

                return Ok(select2);
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return await StatusCode(HttpStatusCode.InternalServerError).Async();
            }
        }
    }
}
