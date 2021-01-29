using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac.Features.Indexed;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Assist.Authentication;
using SMARTII.Assist.Logger;
using SMARTII.Assist.Web;
using SMARTII.COMMON_BU.Models.Store;
using SMARTII.COMMON_BU.Models.User;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using SMARTII.Domain.System;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;
using SMARTII.Service.Organization.Resolver;

namespace SMARTII.COMMON_BU.Controllers
{
    [Authentication]
    [RoutePrefix("Api/COMMON_BU/Store")]
    public class COMMON_BUStoreController : BaseApiController
    {

        private readonly IStoreFacade _StoreFacade;
        private readonly ICommonAggregate _CommonAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly ISystemAggregate _SystemAggregate;
        private readonly IIndex<string, IStoreFactory> _StoreFactories; 
        private readonly OrganizationNodeResolver _OrganizationResolver;


        public COMMON_BUStoreController(IStoreFacade StoreFacade,
                                        ICommonAggregate CommonAggregate,
                                        IOrganizationAggregate OrganizationAggregate,
                                        ISystemAggregate SystemAggregate,
                                        OrganizationNodeResolver OrganizationResolver,
                                        IIndex<string, IStoreFactory> StoreFactories)
        {
            _StoreFacade = StoreFacade;
            _OrganizationResolver = OrganizationResolver;
            _OrganizationAggregate = OrganizationAggregate;
            _SystemAggregate = SystemAggregate;
            _StoreFactories = StoreFactories;
            _CommonAggregate = CommonAggregate;
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
                var searchTerm = model.criteria;

                var con = new MSSQLCondition<STORE>(
                    searchTerm,
                    model.pageIndex,
                    model.pageSize
                    );

                con.IncludeBy(x => x.HEADQUARTERS_NODE.HEADQUARTERS_NODE2);
                con.OrderBy(model.sort, model.orderType);
                
                var list = _OrganizationAggregate.Store_T1_T2_Expendo_.GetPaging(con);

                var ui = list.Select(x => new StoreListViewModel(x));

                return await new PagingResponse<IEnumerable<StoreListViewModel>>(ui)
                {
                    isSuccess = true,
                    totalCount = list.TotalCount
                }.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(Store_lang.STORE_GETLIST_FAIL));

                return await new PagingResponse()
                {
                    isSuccess = false,
                    message = ex.PrefixMessage(Store_lang.STORE_GETLIST_FAIL)
                }.Async();
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
        [Logger(nameof(Store_lang.STORE_GET))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Get([Required]int? ID, [Required]OrganizationType? OrganizationType)
        {
            try
            {


                var store = _StoreFacade.GetComplete<ExpandoObject>(ID.Value, OrganizationType.Value);


                var model = new StoreDetailViewModel(store);
                
                var con = new MSSQLCondition<SYSTEM_PARAMETER>(
                                x => x.ID == EssentialCache.LayoutValue.StoreDeatilTemplate && 
                                x.KEY == model.NodeKey);

                model.DynamicForm = _SystemAggregate.SystemParameter_T1_T2_.GetOfSpecific(con, x => x.VALUE);
                

                var result = new JsonResult<StoreDetailViewModel>(model, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                      ex.PrefixDevMessage(Store_lang.STORE_GET_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Store_lang.STORE_GET_FAIL), false)
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
        [Logger(nameof(Store_lang.STORE_UPDATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Update(StoreDetailViewModel model)
        {
            try
            {
                var domain = model.ToDomain();

                var service = _StoreFactories[EssentialCache.BusinessKeyValue.COMMONBU];

                await service.Update<dynamic>(domain);

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
