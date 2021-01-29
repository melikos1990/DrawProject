using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Http;
using Autofac.Features.Indexed;
using ExcelDataReader;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Assist.Authentication;
using SMARTII.Assist.Logger;
using SMARTII.Assist.Web;
using SMARTII.COMMON_BU.Models.Item;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.DI;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;
using SMARTII.Service.Organization.Resolver;

namespace SMARTII.COMMON_BU.Controllers
{
    [Authentication]
    [RoutePrefix("Api/COMMON_BU/Item")]
    public class COMMON_BUItemController : BaseApiController
    {
        private readonly ICommonAggregate _CommonAggregate;
        private readonly IMasterAggregate _MasterAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly IIndex<string, IItemFactory> _ItemFactories;
        private readonly OrganizationNodeResolver _OrganizationResolver;

        public COMMON_BUItemController(IMasterAggregate MasterAggregate,
                                       ICommonAggregate CommonAggregate,
                                       IOrganizationAggregate OrganizationAggregate,
                                       OrganizationNodeResolver OrganizationResolver,
                                       IIndex<string, IItemFactory> ItemFactories)
        {
            _MasterAggregate = MasterAggregate;
            _OrganizationResolver = OrganizationResolver;
            _OrganizationAggregate = OrganizationAggregate;
            _ItemFactories = ItemFactories;
            _CommonAggregate = CommonAggregate;
        }

        /// <summary>
        /// 取得清單
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(Item_lang.ITEM_GETLIST))]
        [ModelValidator(false)]
        public async Task<IHttpResult> GetList(ItemSearchViewModel model)
        {
            try
            {
                var con = new MSSQLCondition<ITEM>();

                if (model.NodeID != null)
                    con.And(x => x.NODE_ID == model.NodeID);
                if (model.Code != null)
                    con.And(x => x.CODE.Contains(model.Code));
                if (model.Name != null)
                    con.And(x => x.NAME.Contains(model.Name));
                if (model.IsEnabled != null)
                    con.And(x => x.IS_ENABLED == model.IsEnabled);
                if (!string.IsNullOrEmpty(model.J_Content))
                    con.And(x => x.J_CONTENT.Contains(model.J_Content));

                var list = _MasterAggregate.Item_T1_T2_Expendo_.GetList(con);

                var ui = _OrganizationResolver.ResolveCollection(list)
                                              .Select(x => new ItemListViewModel(x));

                var result = new JsonResult<IEnumerable<ItemListViewModel>>(ui, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(Item_lang.ITEM_GETLIST_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Item_lang.ITEM_GETLIST_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 取得明細
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Get")]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(Item_lang.ITEM_GET))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Get([Required]int? ID)
        {
            try
            {
                var con = new MSSQLCondition<ITEM>(x => x.ID == ID);

                var item = _MasterAggregate.Item_T1_T2_Expendo_.Get(con);

                var model = new ItemDetailViewModel(item);

                var oCon = new MSSQLCondition<HEADQUARTERS_NODE>(x => x.NODE_ID == model.NodeID);
                model.NodeKey = _OrganizationAggregate.HeaderQuarterNode_T1_IOrganizationNode_.GetOfSpecific(oCon, x => x.NODE_KEY);

                var result = new JsonResult<ItemDetailViewModel>(model, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                      ex.PrefixDevMessage(Item_lang.ITEM_GET_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Item_lang.ITEM_GET_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 單一停用
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Disable")]
        [AuthenticationMethod(AuthenticationType.Delete)]
        [Logger(nameof(Item_lang.ITEM_DISABLE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Disable([Required]int? ID)
        {
            try
            {
                var con = new MSSQLCondition<ITEM>(x => x.ID == ID);

                con.ActionModify(x =>
                {
                    x.IS_ENABLED = false;
                    x.UPDATE_DATETIME = DateTime.Now;
                    x.UPDATE_USERNAME = ContextUtility.GetUserIdentity().Name;
                });

                _MasterAggregate.Item_T1_T2_.Update(con);

                return await new JsonResult(
                    Item_lang.ITEM_DISABLE_SUCCESS, true)
                    .Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                   ex.PrefixDevMessage(Item_lang.ITEM_DISABLE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Item_lang.ITEM_DISABLE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 批次停用
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("DisableRange")]
        [AuthenticationMethod(AuthenticationType.Admin)]
        [Logger(nameof(Item_lang.ITEM_DISABLE_RANGE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> DisableRange(List<ItemListViewModel> model)
        {
            try
            {
                var con = new MSSQLCondition<ITEM>();

                model.ForEach(g => con.Or(x => x.ID == g.ID));

                con.ActionModify(x =>
                {
                    x.IS_ENABLED = false;
                    x.UPDATE_DATETIME = DateTime.Now;
                    x.UPDATE_USERNAME = ContextUtility.GetUserIdentity().Name;
                });

                _MasterAggregate.Item_T1_T2_.UpdateRange(con);

                return await new JsonResult(
                 Item_lang.ITEM_DISABLE_RANGE_SUCCESS, true)
                 .Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(Item_lang.ITEM_DISABLE_RANGE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Item_lang.ITEM_DISABLE_RANGE_SUCCESS), false)
                    .Async();
            }
        }

        /// <summary>
        /// 單一新增
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        [Route("Create")]
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Add)]
        [Logger(nameof(Item_lang.ITEM_CREATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Create(ItemDetailViewModel model)
        {
            try
            {
                var domain = model.ToDomain();

                var service = _ItemFactories[EssentialCache.BusinessKeyValue.COMMONBU];

                await service.Create<dynamic>(domain);

                return await new JsonResult(Item_lang.ITEM_CREATE_SUCCESS, true).Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(Item_lang.ITEM_CREATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Item_lang.ITEM_CREATE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 單一更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Update")]
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Update)]
        [Logger(nameof(Item_lang.ITEM_UPDATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Update(ItemDetailViewModel model)
        {
            try
            {
                var domain = model.ToDomain();

                var service = _ItemFactories[EssentialCache.BusinessKeyValue.COMMONBU];

                await service.Update<dynamic>(domain);

                return await new JsonResult(Item_lang.ITEM_UPDATE_SUCCESS, true).Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(Item_lang.ITEM_UPDATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Item_lang.ITEM_UPDATE_FAIL), false)
                    .Async();
            }
        }

        [Route("Upload")]
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Add)]
        [Logger(nameof(Item_lang.ITEM_UPDLOAD))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Upload(FileUploadViewModel data)
        {
            try
            {
                var file = data.File;
                var errorCollection = new List<Item>();

                string fileExtName = Path.GetExtension(file.FileName).ToLower();

                if (!fileExtName.Equals(".xls", StringComparison.OrdinalIgnoreCase)
                    &&
                    !fileExtName.Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    throw new Exception("請上傳 .xls 或 .xlsx 格式的檔案");
                }

                var ms = new MemoryStream(file.Buffer);

                using (IExcelDataReader excelReader = ExcelReaderFactory.CreateReader(ms))
                {
                    var _data = excelReader.AsDataSet();
                    var tablesDic = _data.Tables.TablesToDictionary();

                    using (TransactionScope scope = new TransactionScope())
                    {

                        ErrorProcessHelp.Invoker<Item>((context) =>
                        {
                            foreach (var table in tablesDic)
                            {
                                var service = _ItemFactories.TryGetService(table.Key, EssentialCache.BusinessKeyValue.COMMONBU);
                                service?.Import(table.Value, ref context);
                            }
                        });


                        // 整個 excel 沒有錯誤才能寫入 上面的 ErrorProcessHelp 如果 context 有資料會自動拋出例外
                        scope.Complete();

                    }

                    ms.Dispose();
                }

                
                return await new JsonResult(Item_lang.ITEM_UPDLOAD_SUCCESS, true).Async();
            }
            catch (OutputException<Item> ex)
            {
                var errorItems = ex.Errors.Select(x => new ItemExportViewMode() { ID = x.ID, BUName = x.NodeName }).ToList();

                return await new JsonResult(Item_lang.ITEM_UPDLOAD_FAIL, false) { extend = errorItems }.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(Item_lang.ITEM_UPDLOAD_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Item_lang.ITEM_UPDLOAD_FAIL), false)
                    .Async();
            }
        }

       
    }
}
