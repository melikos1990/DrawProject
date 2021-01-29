using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Areas.Master.Models.CaseFinishReason;
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
    public class CaseFinishReasonController : BaseApiController
    {
        private readonly ICommonAggregate _CommonAggregate;
        private readonly IMasterAggregate _MasterAggregate;
        private readonly ICaseFinishReasonFacade _CaseFinishReasonFacade;
        private readonly OrganizationNodeResolver _OrganizationNodeResolver;

        public CaseFinishReasonController(
            ICommonAggregate CommonAggregate,
            IMasterAggregate MasterAggregate,
            OrganizationNodeResolver OrganizationNodeResolver,
            ICaseFinishReasonFacade CaseFinishReasonFacade)
        {
            _CommonAggregate = CommonAggregate;
            _MasterAggregate = MasterAggregate;
            _OrganizationNodeResolver = OrganizationNodeResolver;
            _CaseFinishReasonFacade = CaseFinishReasonFacade;
        }

        /// <summary>
        /// 取得明細清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(CaseFinishReason_lang.CASEFINISHREASON_GETLIST))]
        [ModelValidator(false)]
        public async Task<PagingResponse> GetList(PagingRequest<CaseFinishDataSearchViewModel> model)
        {
            try
            {
                var con = new MSSQLCondition<CASE_FINISH_REASON_DATA>(
                   model.criteria,
                   model.pageIndex,
                   model.pageSize);
                con.IncludeBy(x => x.CASE_FINISH_REASON_CLASSIFICATION);

                con.OrderBy(model.sort, model.orderType);

                var list = _MasterAggregate.CaseFinishReasonData_T1_T2_.GetPaging(con);

                var ui = _OrganizationNodeResolver.ResolveCollection(list)
                                                  .Select(x => new CaseFinishDataListViewModel(x));

                return await new PagingResponse<IEnumerable<CaseFinishDataListViewModel>>(ui)
                {
                    isSuccess = true,
                    totalCount = list.TotalCount
                }.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(CaseFinishReason_lang.CASEFINISHREASON_GETLIST_FAIL));

                return await new PagingResponse()
                {
                    isSuccess = false,
                    message = ex.PrefixMessage(CaseFinishReason_lang.CASEFINISHREASON_GETLIST_FAIL)
                }.Async();
            }
        }

        /// <summary>
        /// 單一取得明細
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(CaseFinishReason_lang.CASEFINISHREASON_GET))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Get([Required]int? ID)
        {
            try
            {
                var con = new MSSQLCondition<CASE_FINISH_REASON_DATA>(x => x.ID == ID);
                con.IncludeBy(x => x.CASE_FINISH_REASON_CLASSIFICATION);
                var data = _OrganizationNodeResolver.Resolve(_MasterAggregate.CaseFinishReasonData_T1_T2_.Get(con));

                var result = new JsonResult<CaseFinishDataDetailViewModel>(
                                   new CaseFinishDataDetailViewModel(data), true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(CaseFinishReason_lang.CASEFINISHREASON_GET_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(CaseFinishReason_lang.CASEFINISHREASON_GET_FAIL), false)
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
        [Logger(nameof(CaseFinishReason_lang.CASEFINISHREASON_UPDATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Update(CaseFinishDataDetailViewModel model)
        {
            try
            {
                var domain = model.ToDomain();

                await _CaseFinishReasonFacade.Update(domain);

                var result = new JsonResult<string>(CaseFinishReason_lang.CASEFINISHREASON_UPDATE_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(CaseFinishReason_lang.CASEFINISHREASON_UPDATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(CaseFinishReason_lang.CASEFINISHREASON_UPDATE_FAIL), false)
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
        [Logger(nameof(CaseFinishReason_lang.CASEFINISHREASON_CREATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Create(CaseFinishDataDetailViewModel model)
        {
            try
            {
                var domain = model.ToDomain();

                await _CaseFinishReasonFacade.Create(domain);

                var result = new JsonResult<string>(CaseFinishReason_lang.CASEFINISHREASON_CREATE_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(CaseFinishReason_lang.CASEFINISHREASON_CREATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(CaseFinishReason_lang.CASEFINISHREASON_CREATE_FAIL), false)
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
        [Logger(nameof(CaseFinishReason_lang.CASEFINISHREASON_DISABLED))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Disabled([Required]int? ID)
        {
            try
            {
                var con = new MSSQLCondition<CASE_FINISH_REASON_DATA>(x => x.ID == ID);
                con.ActionModify(x => x.IS_ENABLED = false);

                _MasterAggregate.CaseFinishReasonData_T1_T2_.Update(con);

                var result = new JsonResult<string>(CaseFinishReason_lang.CASEFINISHREASON_DISABLED_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                     ex.PrefixDevMessage(CaseFinishReason_lang.CASEFINISHREASON_DISABLED_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(CaseFinishReason_lang.CASEFINISHREASON_DISABLED_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 單一新增分類
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Add)]
        [Logger(nameof(CaseFinishReason_lang.CASEFINISHREASON_CREATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> CreateClassification(CaseFinishClassificationDetailViewModel model)
        {
            try
            {
                var domain = model.ToDomain();

                await _CaseFinishReasonFacade.CreateClassification(domain);

                var result = new JsonResult<string>(CaseFinishReason_lang.CASEFINISHREASON_CREATE_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(CaseFinishReason_lang.CASEFINISHREASON_CREATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(CaseFinishReason_lang.CASEFINISHREASON_CREATE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 單一更新分類
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Update)]
        [Logger(nameof(CaseFinishReason_lang.CASEFINISHREASON_UPDATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> UpdateClassification(CaseFinishClassificationDetailViewModel model)
        {
            try
            {
                var domain = model.ToDomain();

                await _CaseFinishReasonFacade.UpdateClassification(domain);

                var result = new JsonResult<string>(CaseFinishReason_lang.CASEFINISHREASON_UPDATE_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(CaseFinishReason_lang.CASEFINISHREASON_UPDATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(CaseFinishReason_lang.CASEFINISHREASON_UPDATE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 單一取得分類
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(CaseFinishReason_lang.CASEFINISHREASON_CLASSIFICATION_GET))]
        [ModelValidator(false)]
        public async Task<IHttpResult> GetClassification([Required]int? ID)
        {
            try
            {
                var con = new MSSQLCondition<CASE_FINISH_REASON_CLASSIFICATION>(x => x.ID == ID);
                var data = _OrganizationNodeResolver.Resolve(
                    _MasterAggregate.CaseFinishReasonClassification_T1_T2_.Get(con));

                var result = new JsonResult<CaseFinishClassificationDetailViewModel>(
                                   new CaseFinishClassificationDetailViewModel(data), true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(CaseFinishReason_lang.CASEFINISHREASON_CLASSIFICATION_GET_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(CaseFinishReason_lang.CASEFINISHREASON_CLASSIFICATION_GET_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 是否有單選有設定預選資料
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(CaseFinishReason_lang.CASEFINISHREASON_CHECK_SINGLE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> CheckSingle([Required]int? ID)
        {
            try
            {
                var result = await _CaseFinishReasonFacade.CheckExistDefault(ID.Value);

                return new JsonResult<bool>(result.Item1, result.Item2?.Text, true);
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(CaseFinishReason_lang.CASEFINISHREASON_CHECK_SINGLE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(CaseFinishReason_lang.CASEFINISHREASON_CHECK_SINGLE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 取得分類清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(CaseFinishReason_lang.CASEFINISHREASON_GETLIST))]
        [ModelValidator(false)]
        public async Task<IHttpActionResult> GetClassificationList(int? nodeID)
        {
            try
            {
                var con = new MSSQLCondition<CASE_FINISH_REASON_CLASSIFICATION>(x =>
                                x.NODE_ID == nodeID &&
                                x.ORGANIZATION_TYPE == (byte)OrganizationType.HeaderQuarter);

                con.OrderBy(x => x.ORDER, OrderType.Asc);

                var result = _MasterAggregate.CaseFinishReasonClassification_T1_T2_
                                             .GetList(con)
                                             .ToList();

                var select2 = new Select2Response<CaseFinishClassificationListViewModel>()
                {
                    items = Select2Response<CaseFinishClassificationListViewModel>
                            .ToSelectItems(
                                result,
                                x => x.ID.ToString(),
                                x => x.Title,
                                x => new CaseFinishClassificationListViewModel(x))
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
        /// 取得明細清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(CaseFinishReason_lang.CASEFINISHREASON_GETLIST))]
        [ModelValidator(false)]
        public async Task<IHttpActionResult> GetDataList(int? classificationID)
        {
            try
            {
                var con = new MSSQLCondition<CASE_FINISH_REASON_DATA>(x => x.CLASSIFICATION_ID == classificationID);

                con.OrderBy(x => x.ORDER, OrderType.Asc);
                con.IncludeBy(x => x.CASE_FINISH_REASON_CLASSIFICATION);

                var result = _MasterAggregate.CaseFinishReasonData_T1_T2_
                                             .GetList(con)
                                             .ToList();

                var select2 = new Select2Response<CaseFinishDataListViewModel>()
                {
                    items = Select2Response<CaseFinishDataListViewModel>
                            .ToSelectItems(
                                result,
                                x => x.ID.ToString(),
                                x => x.Text,
                                x => new CaseFinishDataListViewModel(x))
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
        /// 更新分類排序
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Update)]
        [Logger(nameof(CaseFinishReason_lang.CASEFINISHREASON_ORDER))]
        [ModelValidator(false)]
        public async Task<IHttpResult> OrderBy(List<SorterListResponse<CaseFinishDataListViewModel>> model)
        {
            try
            {
                _MasterAggregate.CaseFinishReasonClassification_T1_T2_.Operator(x =>
                {
                    var dbContext = (SMARTIIEntities)x;

                    foreach (var item in model)
                    {
                        var id = int.Parse(item.id);

                        var entity = dbContext.CASE_FINISH_REASON_DATA.First(g => g.ID == id);
                        entity.ORDER = item.order;
                    }
                    dbContext.SaveChanges();
                });
                var result = new JsonResult<string>(CaseFinishReason_lang.CASEFINISHREASON_ORDER_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(CaseFinishReason_lang.CASEFINISHREASON_ORDER_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(CaseFinishReason_lang.CASEFINISHREASON_ORDER_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 更新明細排序
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Update)]
        [Logger(nameof(CaseFinishReason_lang.CASEFINISHREASON_ORDER_CLASSIFICATION))]
        [ModelValidator(false)]
        public async Task<IHttpResult> OrderByClassification(List<SorterListResponse<CaseFinishClassificationListViewModel>> model)
        {
            try
            {
                _MasterAggregate.CaseFinishReasonClassification_T1_T2_.Operator(x =>
                {
                    var dbContext = (SMARTIIEntities)x;

                    foreach (var item in model)
                    {
                        var id = int.Parse(item.id);

                        var entity = dbContext.CASE_FINISH_REASON_CLASSIFICATION.First(g => g.ID == id);

                        entity.ORDER = item.order;
                    }
                    dbContext.SaveChanges();
                });

                var result = new JsonResult<string>(CaseFinishReason_lang.CASEFINISHREASON_ORDER_CLASSIFICATION_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(CaseFinishReason_lang.CASEFINISHREASON_ORDER_CLASSIFICATION_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(CaseFinishReason_lang.CASEFINISHREASON_ORDER_CLASSIFICATION_FAIL), false)
                    .Async();
            }
        }
    }
}
