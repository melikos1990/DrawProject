using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using MoreLinq;
using Newtonsoft.Json;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;
using SMARTII.Domain.System;
using SMARTII.Domain.Thread;

namespace SMARTII.Areas.Common.Controllers
{
    public class SystemController : ApiController
    {
        private readonly ICommonAggregate _CommonAggregate;
        private readonly ISystemAggregate _SystemAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;

        public SystemController(
        ISystemAggregate SystemAggregate,
        ICommonAggregate CommonAggregate,
        IOrganizationAggregate OrganizationAggregate)
        {
            _SystemAggregate = SystemAggregate;
            _CommonAggregate = CommonAggregate;
            _OrganizationAggregate = OrganizationAggregate;
        }

        /// <summary>
        /// 取得商品明細Layout
        /// </summary>
        /// <param name="nodeKey"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetItemDetailTemplate")]
        public async Task<IHttpActionResult> GetItemDetailTemplateAsync(string nodeKey)
        {
            try
            {
                var con = new MSSQLCondition<SYSTEM_PARAMETER>(x => x.ID == EssentialCache.LayoutValue.ItemDeatilTemplate && x.KEY == nodeKey);

                var result = _SystemAggregate.SystemParameter_T1_T2_.Get(con);

                return Ok(result?.Value);
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return await StatusCode(HttpStatusCode.InternalServerError).Async();
            }
        }

        /// <summary>
        /// 取得商品查詢Layout
        /// </summary>
        /// <param name="nodeKey"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetItemListTemplate")]
        public async Task<IHttpActionResult> GetItemListTemplateAsync(string nodeKey)
        {
            try
            {
                var con = new MSSQLCondition<SYSTEM_PARAMETER>(x => x.ID == EssentialCache.LayoutValue.ItemQueryTemplate && x.KEY == nodeKey);

                var result = _SystemAggregate.SystemParameter_T1_T2_.Get(con);

                return Ok(result?.Value);
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return await StatusCode(HttpStatusCode.InternalServerError).Async();
            }
        }

        /// <summary>
        /// 取得門市明細Layout
        /// </summary>
        /// <param name="nodeKey"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetStoreDetailTemplate")]
        public async Task<IHttpActionResult> GetStoreDetailTemplateAsync(string nodeKey)
        {
            try
            {
                var con = new MSSQLCondition<SYSTEM_PARAMETER>(x => x.ID == EssentialCache.LayoutValue.StoreDeatilTemplate && x.KEY == nodeKey);

                var result = _SystemAggregate.SystemParameter_T1_T2_.Get(con);

                return Ok(result?.Value);
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return await StatusCode(HttpStatusCode.InternalServerError).Async();
            }
        }

        /// <summary>
        /// 取得門市查詢Layout
        /// </summary>
        /// <param name="nodeKey"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetStoreListTemplate")]
        public async Task<IHttpActionResult> GetStoreListTemplateAsync(string nodeKey)
        {
            try
            {
                var con = new MSSQLCondition<SYSTEM_PARAMETER>(x => x.ID == EssentialCache.LayoutValue.StoreQueryTemplate && x.KEY == nodeKey);

                var result = _SystemAggregate.SystemParameter_T1_T2_.Get(con);

                return Ok(result?.Value);
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return await StatusCode(HttpStatusCode.InternalServerError).Async();
            }
        }

        /// <summary>
        /// 取得範本類別清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetClassification")]
        public async Task<IHttpActionResult> GetClassificationAsync()
        {
            try
            {
                var con = new MSSQLCondition<SYSTEM_PARAMETER>(x => x.ID == EssentialCache.LayoutValue.CaseTemplate);
                con.OrderBy(x => x.CREATE_DATETIME, OrderType.Desc);

                var result = _SystemAggregate.SystemParameter_T1_T2_.GetPaging(con).ToList();

                var select2 = new Select2Response()
                {
                    items = Select2Response.ToSelectItems(result, x => x.Key.ToString(), x => x.Value)
                };

                return Ok(select2);
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return await StatusCode(HttpStatusCode.InternalServerError).Async();
            }
        }

        [HttpGet]
        [ActionName("GetLanguageFiles")]
        public async Task<IHttpActionResult> GetLanguageFilesAsync()
        {
            try
            {
                var languages = new Dictionary<string, object>();

                var binPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");
                var dirPath = Path.Combine(binPath, "Assets", "i18n");

                var fileNames = Directory.EnumerateFiles(dirPath, "*", SearchOption.AllDirectories)
                                         .Select(Path.GetFileName);

                fileNames.ForEach(x =>
                {
                    var filePath = $"{dirPath}/{x}";

                    var str = File.ReadAllText(filePath, Encoding.UTF8);

                    object jsonObject = JsonConvert.DeserializeObject(str);

                    var fileName = x.Split('.')[0];

                    languages.Add(fileName, jsonObject);
                });

                return Ok(languages);
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return await StatusCode(HttpStatusCode.InternalServerError).Async();
            }
        }

        /// <summary>
        /// 取得系統參數
        /// </summary>
        /// <param name="BuID"></param>
        /// <param name="parameterType">參數型態(ID)</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetSystemParameter")]
        public async Task<IHttpActionResult> GetSystemParameterAsync(int BuID, string ParameterType)
        {
            try
            {
                // 取得 BU 的 NODE_KEY
                var buCon = new MSSQLCondition<HEADQUARTERS_NODE>(x => x.NODE_ID == BuID);
                var bu = _OrganizationAggregate.HeaderQuarterNode_T1_T2_.GetOfSpecific(buCon, x => new HeaderQuarterNode() { NodeKey = x.NODE_KEY });

                // 取得對應系統參數
                var con = new MSSQLCondition<SYSTEM_PARAMETER>(x => x.KEY == bu.NodeKey && x.ID == ParameterType);
                var systemParatem = _SystemAggregate.SystemParameter_T1_T2_.Get(con);

                var result = JsonConvert.DeserializeObject<List<SelectItem>>(systemParatem.Value);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return Json(new List<SelectItem>());
            }
        }

        /// <summary>
        /// 取得門市型態
        /// </summary>
        /// <param name="nodeKey"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetStoreType")]
        public async Task<IHttpActionResult> GetStoreTypeAsync(string nodeKey)
        {
            try
            {
                // 取得對應系統參數
                var con = new MSSQLCondition<SYSTEM_PARAMETER>(x => x.ID == EssentialCache.LayoutValue.StoreTypeTemplate &&
                                                                    x.KEY == nodeKey);

                var systemParatem = _SystemAggregate.SystemParameter_T1_T2_.Get(con);

                var valuelist = JsonConvert.DeserializeObject<List<SystemParameter>>(systemParatem.Value ?? "[]");

                var select2 = new Select2Response()
                {
                    items = Select2Response.ToSelectItems(valuelist, x => x.Key.ToString(), x => x.Value)
                };

                return Ok(select2);
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return Json(new List<SelectItem>());
            }
        }



        /// <summary>
        /// 取得反應單類型
        /// </summary>
        /// <param name="nodeKey"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetInvoiceType")]
        public async Task<IHttpActionResult> GetInvoiceTypeAsync(int buID)
        {
            try
            {
                // 取得 BU 的 NODE_KEY
                var buCon = new MSSQLCondition<HEADQUARTERS_NODE>(x => x.NODE_ID == buID);
                var nodeKey = _OrganizationAggregate.HeaderQuarterNode_T1_T2_.GetOfSpecific(buCon, x => x.NODE_KEY);


                // 取得對應系統參數
                var con = new MSSQLCondition<SYSTEM_PARAMETER>(x => x.ID == EssentialCache.CaseValue.CASE_ASSIGNMENTINVOICECOMPLAIN &&
                                                                    x.KEY == nodeKey);


                var systemParatem = _SystemAggregate.SystemParameter_T1_T2_.Get(con);

                var valuelist = JsonConvert.DeserializeObject<List<SystemParameter>>(systemParatem.Value ?? "[]");

                var select2 = new Select2Response()
                {
                    items = Select2Response.ToSelectItems(valuelist, x => x.ID.ToString(), x => x.Text)
                };

                return Ok(select2);
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return Json(new List<SelectItem>());
            }
        }

    }
}
