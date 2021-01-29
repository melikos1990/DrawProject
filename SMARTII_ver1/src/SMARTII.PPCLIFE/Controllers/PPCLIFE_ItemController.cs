using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac.Features.Indexed;
using MultipartDataMediaFormatter.Infrastructure;
using Newtonsoft.Json;
using Ptc.Data.Condition2.Mssql.Class;
using Ptc.Data.Condition2.Mssql.Repository;
using SMARTII.Assist.Authentication;
using SMARTII.Assist.Logger;
using SMARTII.Assist.Web;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.DI;
using SMARTII.Domain.Master;
using SMARTII.Domain.Thread;
using SMARTII.PPCLIFE.Domain;
using SMARTII.PPCLIFE.Models.Item;
using SMARTII.Resource.Tag;
using SMARTII.Service.Organization.Resolver;

namespace SMARTII.PPCLIFE.Controllers
{
    [Authentication]
    [RoutePrefix("Api/PPCLIFE/Item")]
    public class PPCLIFE_ItemController : BaseApiController
    {
        private readonly IIndex<string, IItemFactory> _ItemFactories;
        private readonly ICommonAggregate _CommonAggregate;
        private readonly IMasterAggregate _MasterAggregate;
        private readonly OrganizationNodeResolver _OrganizationNodeResolver;

        private readonly IMSSQLRepository<ITEM, Item<PPCLIFE_Item>> _ItemRepo;

        public PPCLIFE_ItemController(IMasterAggregate MasterAggregate,
                                      ICommonAggregate CommonAggregate,
                                      IIndex<string, IItemFactory> ItemFactories,
                                      IMSSQLRepository<ITEM, Item<PPCLIFE_Item>> ItemRepo,
                                      OrganizationNodeResolver OrganizationNodeResolver)
        {
            _ItemRepo = ItemRepo;
            this._MasterAggregate = MasterAggregate;
            this._ItemFactories = ItemFactories;
            this._CommonAggregate = CommonAggregate;
            this._OrganizationNodeResolver = OrganizationNodeResolver;
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
                var particular = model?.Particular;

                var con = new MSSQLCondition<ITEM>();

                if (model.NodeID != null)
                    con.And(x => x.NODE_ID == model.NodeID);
                if (model.Code != null)
                    con.And(x => x.CODE.Contains(model.Code));
                if (model.Name != null)
                    con.And(x => x.NAME.Contains(model.Name));

                var items = _ItemRepo.GetList(con).Where(x => x.Particular != null).ToList();

                var jContentCon = new MSSQLCondition<Item<PPCLIFE_Item>>(particular);

                items = items.Query(jContentCon).ToList();

                var list = _OrganizationNodeResolver.ResolveCollection(items).Select(x => new ItemsListViewModel(x));

                var result = new JsonResult<IEnumerable<ItemsListViewModel>>(list, true);

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

                IItemFactory service = _ItemFactories.TryGetService(model.NodeKey, EssentialCache.BusinessKeyValue.COMMONBU);

                await service.Create<PPCLIFE_Item>(domain);

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

                var service = _ItemFactories.TryGetService(model.NodeKey, EssentialCache.BusinessKeyValue.COMMONBU);

                await service.Update<PPCLIFE_Item>(domain);

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

        

    }
}
