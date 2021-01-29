using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Areas.Master.Models.WorkSchedule;
using SMARTII.Assist.Authentication;
using SMARTII.Assist.Logger;
using SMARTII.Assist.Web;
using SMARTII.Configuration;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;
using SMARTII.Service.Cache;
using SMARTII.Service.Organization.Resolver;

namespace SMARTII.Areas.Master.Controllers
{
    [Authentication]
    public class WorkScheduleController : BaseApiController
    {
        private readonly IWorkScheduleFacade _WorkScheduleFacade;
        private readonly ICommonAggregate _CommonAggregate;
        private readonly IMasterAggregate _MasterAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly OrganizationNodeResolver _OrganizationResolver;

        public WorkScheduleController(IWorkScheduleFacade WorkScheduleFacade,
                                          ICommonAggregate CommonAggregate,
                                          IMasterAggregate MasterAggregate,
                                          IOrganizationAggregate OrganizationAggregate,
                                          OrganizationNodeResolver OrganizationResolver)
        {
            _CommonAggregate = CommonAggregate;
            _MasterAggregate = MasterAggregate;
            _OrganizationAggregate = OrganizationAggregate;
            _WorkScheduleFacade = WorkScheduleFacade;
            _OrganizationResolver = OrganizationResolver;
        }
        /// <summary>
        /// 取得特定假日清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(WorkSchedule_lang.WORK_SCHEDULE_GETLIST))]
        [ModelValidator(false)]
        public async Task<PagingResponse> GetList(PagingRequest<WorkScheduleSearchViewModel> model)
        {
            try
            {
                var con = new MSSQLCondition<WORK_SCHEDULE>(
                   model.criteria ?? new WorkScheduleSearchViewModel(),
                   model.pageIndex,
                   model.pageSize);

                con.OrderBy(model.sort, model.orderType);

                if (!string.IsNullOrEmpty(model.criteria.YearTime))
                {
                    DateTime k;
                    CultureInfo culTW = new CultureInfo("zh-TW", true);
                    string[] sTwDtPattern = new string[] { "yyyy" };

                    if (DateTime.TryParseExact(model.criteria.YearTime, sTwDtPattern, culTW, DateTimeStyles.None, out k))
                    {
                        DateTime startTime = new DateTime(int.Parse(model.criteria.YearTime), 1, 1);
                        DateTime endTime = new DateTime(int.Parse(model.criteria.YearTime), 12, 31);

                        con.And(x => x.DATE >= startTime && x.DATE < endTime);

                    }
                    else
                    {
                        throw new Exception(WorkSchedule_lang.WORK_SCHEDULE_PARSE_DATE_FAIL);
                    }
                }

                var list = _MasterAggregate.WorkSchedule_T1_T2_.GetPaging(con);

                var ui = _OrganizationResolver.ResolveCollection(list)
                        .Select(x => new WorkScheduleListViewModel(x));

                return await new PagingResponse<IEnumerable<WorkScheduleListViewModel>>(ui)
                {
                    isSuccess = true,
                    totalCount = list.TotalCount
                }.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(WorkSchedule_lang.WORK_SCHEDULE_GETLIST_FAIL));

                return await new PagingResponse()
                {
                    isSuccess = false,
                    message = ex.PrefixMessage(WorkSchedule_lang.WORK_SCHEDULE_GETLIST_FAIL)
                }.Async();
            }
        }

        /// <summary>
        /// 單一取得特定假日
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(WorkSchedule_lang.WORK_SCHEDULE_GET))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Get([Required]int id)
        {
            try
            {
                var con = new MSSQLCondition<WORK_SCHEDULE>();
                con.And(x => x.ID == id);

                var data = _MasterAggregate.WorkSchedule_T1_T2_.Get(con);


                var complete = _OrganizationResolver.Resolve(data);

                var result = new JsonResult<WorkScheduleDetailViewModel>(
                                   new WorkScheduleDetailViewModel(complete), true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(WorkSchedule_lang.WORK_SCHEDULE_GET_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(WorkSchedule_lang.WORK_SCHEDULE_GET_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 單一新增特定假日
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Add)]
        [Logger(nameof(WorkSchedule_lang.WORK_SCHEDULE_CREATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Create(List<WorkScheduleDetailViewModel> model)
        {
            try
            {
                var domain = model.Select(x => x.ToDomain()).ToList();

                //CheckkDate(domain);

                await _WorkScheduleFacade.Create(domain);

                var result = new JsonResult(WorkSchedule_lang.WORK_SCHEDULE_CREATE_SUCCESS, true);

                return await result.Async();
            }
            catch (OutputException<WorkSchedule> ex)
            {
                var errors = new List<WorkScheduleErrorViewModel>();

                if (ex.Errors != null)
                    errors = _OrganizationResolver.ResolveCollection<WorkSchedule>(ex.Errors)
                    .Select(x => new WorkScheduleErrorViewModel(x)).ToList();


                return await new JsonResult(ex.Message, false) { extend = errors }.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(WorkSchedule_lang.WORK_SCHEDULE_CREATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(WorkSchedule_lang.WORK_SCHEDULE_CREATE_FAIL), false)
                    .Async();
            }
        }

        private static void CheckDate(List<WorkSchedule> domain)
        {
            foreach (var item in domain)
            {
                if (item.Date < DateTime.Now)
                    throw new OutputException<WorkSchedule>(WorkSchedule_lang.WORK_SCHEDULE_DATE_ILLEGAL);
            }
        }

        /// <summary>
        /// 單一更新特定假日明細
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Update)]
        [Logger(nameof(WorkSchedule_lang.WORK_SCHEDULE_UPDATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Update(WorkScheduleDetailViewModel model)
        {
            try
            {
                var domain = model.ToDomain();

                if (domain.Date < DateTime.Now)
                    throw new Exception(WorkSchedule_lang.WORK_SCHEDULE_DATE_ILLEGAL);

                await _WorkScheduleFacade.Update(domain);


                // 需異動 static 
                // 當 IS_UNDELETABLE
                DataStorage.Refresh();

                var result = new JsonResult<string>(WorkSchedule_lang.WORK_SCHEDULE_UPDATE_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(WorkSchedule_lang.WORK_SCHEDULE_UPDATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(WorkSchedule_lang.WORK_SCHEDULE_UPDATE_FAIL), false)
                    .Async();
            }
        }
        /// <summary>
        /// 單一刪除特定假日
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Delete)]
        [Logger(nameof(WorkSchedule_lang.WORK_SCHEDULE_DELETE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Delete(WorkScheduleListViewModel model)
        {
            try
            {
                if (Convert.ToDateTime(model.Date) < DateTime.Now.Date.AddDays(1))
                    throw new Exception(WorkSchedule_lang.WORK_SCHEDULE_BEGIN_NOWTIME);


                if (await _WorkScheduleFacade.ChackIncorporatedCase(model.ID))
                    throw new Exception(WorkSchedule_lang.WORK_SCHEDULE_INCORPORATED_CASE);

                var con = new MSSQLCondition<WORK_SCHEDULE>();
                con.And(x => x.ID == model.ID);

                var data = _MasterAggregate.WorkSchedule_T1_T2_.Remove(con);


                // 需異動 static 
                // 當 IS_UNDELETABLE
                DataStorage.Refresh();

                var result = new JsonResult(WorkSchedule_lang.WORK_SCHEDULE_DELETE_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(WorkSchedule_lang.WORK_SCHEDULE_DELETE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(WorkSchedule_lang.WORK_SCHEDULE_DELETE_FAIL), false)
                    .Async();
            }
        }
        /// <summary>
        /// 批次刪除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Delete)]
        [Logger(nameof(WorkSchedule_lang.WORK_SCHEDULE_DELETE_RANGE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> DeleteRange(List<WorkScheduleListViewModel> model)
        {
            try
            {
                //刪除資料早於系統日期
                DateTime now = DateTime.Now.Date.AddDays(1);

                ErrorProcessHelp.Invoker<WorkScheduleListViewModel>(context =>
                {
                    var ovreTime = model.Where(x => Convert.ToDateTime(x.Date) < now);
                    if (ovreTime.Any())
                    {
                        context.AddRange(ovreTime.ToList());
                        context.Message = WorkSchedule_lang.WORK_SCHEDULE_BEGIN_NOWTIME;
                    }
                    

                });


                // 驗證 是否有案件已被計算時效
                ErrorProcessHelp.Invoker<WorkScheduleListViewModel>(context =>
                {
                    context.Message = WorkSchedule_lang.WORK_SCHEDULE_INCORPORATED_CASE;

                    model.ForEach(async data =>
                    {
                        if (await _WorkScheduleFacade.ChackIncorporatedCase(data.ID))
                        {
                            context.Add(data);
                        }
                    });
                });

                var con = new MSSQLCondition<WORK_SCHEDULE>();

                model.ForEach(g => con.Or(x => x.ID == g.ID));

                var isSuccess = _MasterAggregate.WorkSchedule_T1_T2_.RemoveRange(con);

                if (isSuccess == false)
                    throw new Exception(Common_lang.DATABASE_DELETE_ERROR);


                // 需異動 static 
                // 當 IS_UNDELETABLE
                DataStorage.Refresh();

                return await new JsonResult(
                 WorkSchedule_lang.WORK_SCHEDULE_DELETE_RANGE_SUCCESS, true)
                 .Async();
            }
            catch (OutputException<WorkScheduleListViewModel> ex)
            {
                var workList = ex.Errors.Select(x => new WorkSchedule() { NodeID = x.NodeID }).ToList();

                var errors = _OrganizationResolver.ResolveCollection<WorkSchedule>(workList)
                    .Zip(ex.Errors, (domain, viewModel) => new WorkScheduleErrorViewModel(domain) { Date = viewModel.Date })
                    .ToList();


                return await new JsonResult(ex.Message, false) { extend = errors }.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(WorkSchedule_lang.WORK_SCHEDULE_DELETE_RANGE_FAILED));

                return await new JsonResult(
                    ex.PrefixMessage(WorkSchedule_lang.WORK_SCHEDULE_DELETE_RANGE_FAILED), false)
                    .Async();
            }
        }
    }
}
