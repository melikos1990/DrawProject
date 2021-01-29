using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Areas.Master.Models.CaseWarning;
using SMARTII.Assist.Authentication;
using SMARTII.Assist.Logger;
using SMARTII.Assist.Web;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;
using SMARTII.Service.Organization.Resolver;

namespace SMARTII.Areas.Master.Controllers
{
    [Authentication]
    public class CaseWarningController : BaseApiController
    {
        private readonly ICommonAggregate _CommonAggregate;
        private readonly IMasterAggregate _MasterAggregate;
        private readonly ICaseWarningFacade _CaseWarningFacade;
        private readonly OrganizationNodeResolver _OrganizationNodeResolver;

        public CaseWarningController(
            ICommonAggregate CommonAggregate,
            IMasterAggregate MasterAggregate,
            OrganizationNodeResolver OrganizationNodeResolver,
            ICaseWarningFacade CaseWarningFacade)
        {
            _CommonAggregate = CommonAggregate;
            _MasterAggregate = MasterAggregate;
            _OrganizationNodeResolver = OrganizationNodeResolver;
            _CaseWarningFacade = CaseWarningFacade;
        }

        /// <summary>
        /// 取得清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(CaseWarning_lang.CASE_WARNING_GETLIST))]
        [ModelValidator(false)]
        public async Task<PagingResponse> GetList(PagingRequest<CaseWarningSearchViewModel> model)
        {
            try
            {
                var con = new MSSQLCondition<CASE_WARNING>(
                   model.criteria,
                   model.pageIndex,
                   model.pageSize);
                con.And(x => x.ORGANIZATION_TYPE == (byte)OrganizationType.HeaderQuarter);

                con.OrderBy(model.sort, model.orderType);

                var list = _MasterAggregate.CaseWarning_T1_T2_.GetPaging(con);

                var ui = _OrganizationNodeResolver.ResolveCollection(list)
                                                  .Select(x => new CaseWarningListViewModel(x));

                return await new PagingResponse<IEnumerable<CaseWarningListViewModel>>(ui)
                {
                    isSuccess = true,
                    totalCount = list.TotalCount
                }.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(CaseWarning_lang.CASE_WARNING_GETLIST_FAIL));

                return await new PagingResponse()
                {
                    isSuccess = false,
                    message = ex.PrefixMessage(CaseWarning_lang.CASE_WARNING_GETLIST_FAIL)
                }.Async();
            }
        }

        /// <summary>
        /// 單一取得
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(CaseWarning_lang.CASE_WARNING_GET))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Get([Required]int? ID)
        {
            try
            {
                var con = new MSSQLCondition<CASE_WARNING>(x => x.ID == ID);

                var data = _MasterAggregate.CaseWarning_T1_T2_.Get(con);

                var result = new JsonResult<CaseWarningDetailViewModel>(
                                   new CaseWarningDetailViewModel(data), true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(CaseWarning_lang.CASE_WARNING_GET_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(CaseWarning_lang.CASE_WARNING_GET_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 單一新增明細
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Add)]
        [Logger(nameof(CaseWarning_lang.CASE_WARNING_CREATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Create(CaseWarningDetailViewModel model)
        {
            try
            {
                var domain = model.ToDomain();

                await _CaseWarningFacade.Create(domain);

                var result = new JsonResult<string>(CaseWarning_lang.CASE_WARNING_CREATE_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(CaseWarning_lang.CASE_WARNING_CREATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(CaseWarning_lang.CASE_WARNING_CREATE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 單一更新明細
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Update)]
        [Logger(nameof(CaseWarning_lang.CASE_WARNING_UPDATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Update(CaseWarningDetailViewModel model)
        {
            try
            {
                var domain = model.ToDomain();

                await _CaseWarningFacade.Update(domain);

                var result = new JsonResult<string>(KMClassification_lang.KMCLASSIFICATION_UPDATE_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(KMClassification_lang.KMCLASSIFICATION_UPDATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(KMClassification_lang.KMCLASSIFICATION_UPDATE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 單一停用
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Delete)]
        [Logger(nameof(CaseWarning_lang.CASE_WARNING_DISABLED))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Disabled([Required]int? ID)
        {
            try
            {
                var con = new MSSQLCondition<CASE_WARNING>(x => x.ID == ID);
                con.ActionModify(x =>
                {
                    x.IS_ENABLED = false;
                    x.UPDATE_DATETIME = DateTime.Now;
                    x.UPDATE_USERNAME = UserIdentity.Instance.Name;
                });

                _MasterAggregate.CaseWarning_T1_T2_.Update(con);

                var result = new JsonResult<string>(CaseWarning_lang.CASE_WARNING_DISABLED_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                     ex.PrefixDevMessage(CaseWarning_lang.CASE_WARNING_DISABLED_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(CaseWarning_lang.CASE_WARNING_DISABLED_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 批次停用
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Delete)]
        [Logger(nameof(CaseWarning_lang.CASE_WARNING_DISABLED_RANGE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> DisableRange(List<CaseWarningListViewModel> model)
        {
            try
            {
                var con = new MSSQLCondition<CASE_WARNING>();

                model.ForEach(g => con.Or(x => x.ID == g.ID));

                con.ActionModify(x =>
                {
                    x.IS_ENABLED = false;
                    x.UPDATE_DATETIME = DateTime.Now;
                    x.UPDATE_USERNAME = ContextUtility.GetUserIdentity().Name;
                });

                _MasterAggregate.CaseWarning_T1_T2_.UpdateRange(con);

                var result = new JsonResult<string>(CaseWarning_lang.CASE_WARNING_DISABLED_RANGE_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                     ex.PrefixDevMessage(CaseWarning_lang.CASE_WARNING_DISABLED_RANGE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(CaseWarning_lang.CASE_WARNING_DISABLED_RANGE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 取得排序清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(CaseWarning_lang.CASE_WARNING_GETLIST))]
        [ModelValidator(false)]
        public async Task<IHttpActionResult> GetDataList(int? NodeID)
        {
            try
            {
                var con = new MSSQLCondition<CASE_WARNING>(x => x.NODE_ID == NodeID);

                con.OrderBy(x => x.ORDER, OrderType.Asc);

                var result = _MasterAggregate.CaseWarning_T1_T2_
                                             .GetList(con)
                                             .ToList();

                var select2 = new Select2Response<CaseWarningListViewModel>()
                {
                    items = Select2Response<CaseWarningListViewModel>
                            .ToSelectItems(
                                result,
                                x => x.ID.ToString(),
                                x => x.Name,
                                x => new CaseWarningListViewModel(x))
                };

                return Ok(select2);
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return Json(new Select2Response()
                {
                    items = new List<SelectItem>()
                });
            }
        }

        /// <summary>
        /// 更新明細排序
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Update)]
        [Logger(nameof(CaseWarning_lang.CASE_WARNING_ORDER))]
        [ModelValidator(false)]
        public async Task<IHttpResult> OrderBy(List<SorterListResponse<CaseWarningListViewModel>> model)
        {
            try
            {
                _MasterAggregate.CaseWarning_T1_T2_.Operator(x =>
                {
                    var dbContext = (SMARTIIEntities)x;

                    foreach (var item in model)
                    {
                        var id = int.Parse(item.id);

                        var entity = dbContext.CASE_WARNING.First(g => g.ID == id);

                        entity.ORDER = item.order;
                    }
                    dbContext.SaveChanges();
                });

                var result = new JsonResult<string>(CaseWarning_lang.CASE_WARNING_ORDER_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(CaseWarning_lang.CASE_WARNING_ORDER_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(CaseWarning_lang.CASE_WARNING_ORDER_FAIL), false)
                    .Async();
            }
        }
    }
}
