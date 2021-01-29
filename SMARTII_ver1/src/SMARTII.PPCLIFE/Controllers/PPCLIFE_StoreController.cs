using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac.Features.Indexed;
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
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.PPCLIFE.Domain;
using SMARTII.PPCLIFE.Models.Store;
using SMARTII.Resource.Tag;
using static SMARTII.Domain.Cache.EssentialCache;

namespace SMARTII.PPCLIFE.Controllers
{
    [Authentication]
    [RoutePrefix("Api/PPCLIFE/Store")]
    public class PPCLIFE_StoreController : BaseApiController
    {
        private readonly IIndex<string, IStoreFactory> _StoreFactories;
        private readonly ICommonAggregate _CommonAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly IMSSQLRepository<STORE, Store<PPCLIFE_Store>> _StoreRepo;

        public PPCLIFE_StoreController(IOrganizationAggregate OrganizationAggregate,
                                      ICommonAggregate CommonAggregate,
                                      IIndex<string, IStoreFactory> StoreFactories,
                                      IMSSQLRepository<STORE, Store<PPCLIFE_Store>> StoreRepo)
        {
            this._OrganizationAggregate = OrganizationAggregate;
            this._StoreFactories = StoreFactories;
            this._CommonAggregate = CommonAggregate;
            this._StoreRepo = StoreRepo;
        }

        /// <summary>
        /// 取得清單
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(Store_lang.STORE_GETLIST))]
        [ModelValidator(false)]
        public async Task<PagingResponse> GetList(PagingRequest<StoreSearchViewModel> model)
        {
            try
            {
                var particular = model?.criteria.Particular;
                var con = new MSSQLCondition<STORE>(model.criteria);
                con.IncludeBy(x => x.HEADQUARTERS_NODE.HEADQUARTERS_NODE2);

                var stores = _StoreRepo.GetList(con).Where(x => x.Particular != null).ToList();

                var jContentCon = new MSSQLCondition<Store<PPCLIFE_Store>>(particular);

                stores = stores.Query(jContentCon).ToList();


                var list = stores.Select(x => new StoreListViewModel(x));
                
                return await new PagingResponse<IEnumerable<COMMON_BU.Models.Store.StoreListViewModel>>(list)
                {
                    isSuccess = true,
                    totalCount = list.Count()
                }.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(Item_lang.ITEM_CREATE_FAIL));

                return await new PagingResponse()
                {
                    isSuccess = false,
                    message = ex.PrefixMessage(CaseRemind_lang.CASEREMIND_GETLIST_FAIL)
                }.Async();
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
        [Logger(nameof(Store_lang.STORE_UPDATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Update(StoreDetailViewModel model)
        {
            try
            {
                var domain = model.ToDomain();

                var service = _StoreFactories.TryGetService(model.NodeKey, BusinessKeyValue.COMMONBU);

                await service.Update<PPCLIFE_Store>(domain);

                return await new JsonResult(Store_lang.STORE_UPDATE_SUCCESS, true).Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(Store_lang.STORE_UPDATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Store_lang.STORE_UPDATE_FAIL), false)
                    .Async();
            }
        }
    }
}
