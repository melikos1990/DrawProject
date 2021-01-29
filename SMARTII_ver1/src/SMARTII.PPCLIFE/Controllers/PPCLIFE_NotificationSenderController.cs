using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Assist.Authentication;
using SMARTII.Assist.Logger;
using SMARTII.Assist.Web;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Thread;
using SMARTII.Domain.Master;
using SMARTII.PPCLIFE.Models.NotificationSender;
using SMARTII.PPCLIFE.Domain;
using System.Web.Http;
using SMARTII.Domain.Case;
using System.Net;
using SMARTII.Domain.Organization;
using System.ComponentModel.DataAnnotations;
using SMARTII.PPCLIFE.Service;
using System.Dynamic;
using SMARTII.Domain.Types;
using SMARTII.Resource.Tag;

namespace SMARTII.PPCLIFE.Controllers
{
    [Authentication]
    [RoutePrefix("Api/PPCLIFE/NotificationSender")]
    public class PPCLIFE_NotificationSenderController : BaseApiController
    {
        private readonly ICommonAggregate _CommonAggregate;
        private readonly INotificationAggregate _NotificationAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly ICaseAggregate _CaseAggregate;
        private readonly IMasterAggregate _MasterAggregate;
        private readonly PPCLIFENotificationService _PPCLIFENotificationService;

        public PPCLIFE_NotificationSenderController(ICommonAggregate CommonAggregate,
                                                    INotificationAggregate NotificationAggregate,
                                                    IOrganizationAggregate OrganizationAggregate,
                                                    ICaseAggregate CaseAggregate,
                                                    IMasterAggregate MasterAggregate,
                                                    PPCLIFENotificationService PPCLIFENotificationService)
        {
            _CommonAggregate = CommonAggregate;
            _NotificationAggregate = NotificationAggregate;
            _OrganizationAggregate = OrganizationAggregate;
            _CaseAggregate = CaseAggregate;
            _MasterAggregate = MasterAggregate;
            _PPCLIFENotificationService = PPCLIFENotificationService;
        }

        /// <summary>
        /// 統藥大量叫修 - 取得達標清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(PPCLIFE_Notification_lang.NOTIFICATION_GET_LIST))]
        [ModelValidator(false)]
        public async Task<PagingResponse> GetArrivedList(PagingRequest model)
        {
            try
            {
                var con = new MSSQLCondition<PPCLIFE_EFFECTIVE_SUMMARY>(model.pageIndex, model.pageSize);
                con.IncludeBy(x => x.CASE_PPCLIFE);
                con.OrderBy(model.sort, model.orderType);

                var list = _NotificationAggregate.PPCLifeEffectiveSummary_T1_T2_.GetPaging(con);

                var ui = list.Select(x => new NotificationSenderListViewModel(x));

                if (list.Count() != 0)
                {
                    var conitem = new MSSQLCondition<ITEM>();
                    var itemIDList = list.Select(c => c.ItemID);
                    conitem.And(x => itemIDList.Contains(x.ID));

                    var itemlist = _MasterAggregate.Item_T1_T2_Expendo_.GetList(conitem);

                    var uiTemp = new List<NotificationSenderListViewModel>();
                    //解析國際條碼
                    foreach (var item in ui)
                    {
                        var temp = itemlist.Where(x=>x.ID == item.ItemID).FirstOrDefault().Particular.CastTo<PPCLIFE_Item>();
                        temp.Name = itemlist.Where(x => x.ID == item.ItemID).FirstOrDefault().Name;
                        uiTemp.Add(GetItem(temp, item));
                    }
                    ui = uiTemp;
                }

                return await new PagingResponse<IEnumerable<NotificationSenderListViewModel>>(ui)
                {
                    isSuccess = true,
                    totalCount = list.TotalCount
                }.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(PPCLIFE_Notification_lang.NOTIFICATION_GET_LIST_FAIL));

                return await new PagingResponse()
                {
                    isSuccess = false,
                    message = ex.PrefixMessage(PPCLIFE_Notification_lang.NOTIFICATION_GET_LIST_FAIL)
                }.Async();
            }
        }

        /// <summary>
        /// 解析PPCLIFE ITEM 資料
        /// </summary>
        /// <param name="data"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private NotificationSenderListViewModel GetItem(PPCLIFE_Item data, NotificationSenderListViewModel list)
        {
            if (data != null)
            {
                list.InternationalBarcode = data.InternationalBarcode;
                list.CommodityName = data.Name;
            }
            return list;
        }

        /// <summary>
        /// 統藥大量叫修 - 取得案件資訊
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetCaseList")]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(PPCLIFE_Notification_lang.NOTIFICATION_GET_LIST))]
        [ModelValidator(false)]
        public async Task<IHttpResult> GetCaseList([Required]int EffectiveID)
        {
            try
            {
                var con = new MSSQLCondition<PPCLIFE_EFFECTIVE_SUMMARY>(x => x.ID == EffectiveID);
                con.IncludeBy(x => x.CASE_PPCLIFE);

                var list = _NotificationAggregate.PPCLifeEffectiveSummary_T1_T2_.Get(con);

                var caselist = list.CasePPCLifes.Where(x=>x.IsIgnore == false).ToList();

                if (caselist.Count != 0)
                {
                    var conCase = new MSSQLCondition<CASE>();
                    var caseIDList = caselist.Select(c => c.CaseID);
                    conCase.And(x => caseIDList.Contains(x.CASE_ID));
                    var temp = _CaseAggregate.Case_T1_T2_.GetList(conCase);

                    foreach (var item in caselist)
                    {
                        var @case = temp.Where(x => x.CaseID == item.CaseID).FirstOrDefault();
                        item.CaseContent = @case.Content;
                    }
                }

                var ui = caselist.Select(x => new NotificationSenderCaseListViewModel(x, list.ID));

                return await new JsonResult<IEnumerable<NotificationSenderCaseListViewModel>>(ui, true).Async();

            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(PPCLIFE_Notification_lang.NOTIFICATION_GET_LIST_FAIL));


                return await new JsonResult()
                {
                    isSuccess = false,
                    message = ex.PrefixMessage(PPCLIFE_Notification_lang.NOTIFICATION_GET_LIST_FAIL)
                }.Async();
            }
        }


        /// <summary>
        /// 統藥大量叫修 - 取得歷程清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetResumeList")]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(PPCLIFE_Notification_lang.NOTIFICATION_GET_LIST))]
        [ModelValidator(false)]
        public async Task<PagingResponse> GetResumeList(PagingRequest<NotificationSearchViewModel> model)
        {
            try
            {
                var searchTerm = model.criteria;

                var con = new MSSQLCondition<PPCLIFE_RESUME>(
                    searchTerm,
                    model.pageIndex,
                    model.pageSize);

                con.OrderBy(model.sort, model.orderType);

                var list = _NotificationAggregate.PPCLifeResume_T1_T2_.GetPaging(con);

                var ui = list.Select(x => new NotificationSenderResumeListViewModel(x));

                return await new PagingResponse<IEnumerable<NotificationSenderResumeListViewModel>>(ui)
                {
                    isSuccess = true,
                    totalCount = list.TotalCount
                }.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(PPCLIFE_Notification_lang.NOTIFICATION_GET_LIST_FAIL));

                return await new PagingResponse()
                {
                    isSuccess = false,
                    message = ex.PrefixMessage(PPCLIFE_Notification_lang.NOTIFICATION_GET_LIST_FAIL)
                }.Async();
            }
        }
        

        /// <summary>
        /// 不通知對象
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("NoSend")]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(PPCLIFE_Notification_lang.NOTIFICATION_WHITOUT_NOTICE))]
        [ModelValidator(true)]
        public async Task<IHttpResult> NoSend([Required]int? effectiveID)
        {
            try
            {
                _PPCLIFENotificationService.NoSend(effectiveID.Value);

                var result = new JsonResult(PPCLIFE_Notification_lang.NOTIFICATION_WHITOUT_NOTICE_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(PPCLIFE_Notification_lang.NOTIFICATION_WHITOUT_NOTICE_FAIL));

                return await new JsonResult(
                ex.PrefixMessage(PPCLIFE_Notification_lang.NOTIFICATION_WHITOUT_NOTICE_FAIL), false)
                  .Async();
            }
        }

        /// <summary>
        /// 通知對象
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Send")]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(PPCLIFE_Notification_lang.NOTIFICATION_NOTICE))]
        [ModelValidator(true)]
        public async Task<IHttpResult> Send(NotificationSenderExecuteViewModel model)
        {
            try
            {
                _PPCLIFENotificationService.Send(model.EffectiveID, model.Payload);

                var result = new JsonResult<string>(PPCLIFE_Notification_lang.NOTIFICATION_NOTICE_SUCCESS, true);
               

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(PPCLIFE_Notification_lang.NOTIFICATION_NOTICE_FAIL));

                return await new JsonResult(
                ex.PrefixMessage(PPCLIFE_Notification_lang.NOTIFICATION_NOTICE_FAIL), false)
               .Async();

            }
        }

        /// <summary>
        /// 無視案件
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Disregard")]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(PPCLIFE_Notification_lang.NOTIFICATION_DISREGARD))]
        [ModelValidator(true)]
        public async Task<IHttpResult> Disregard(List<NotificationSenderCaseListViewModel> model)
        {
            try
            {
                var data = model.Select(x => x.ToDomain()).ToList();

                _PPCLIFENotificationService.Disregard(data);

                var result = new JsonResult(PPCLIFE_Notification_lang.NOTIFICATION_DISREGARD_SUCCESS, true);
                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(PPCLIFE_Notification_lang.NOTIFICATION_DISREGARD_FAIL));

                return await new JsonResult(
                ex.PrefixMessage(PPCLIFE_Notification_lang.NOTIFICATION_DISREGARD_FAIL), false)
                  .Async();
            }
        }
    }
}
