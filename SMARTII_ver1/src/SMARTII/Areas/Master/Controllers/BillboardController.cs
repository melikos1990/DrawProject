using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Areas.Master.Models.Billboard;
using SMARTII.Assist.Authentication;
using SMARTII.Assist.Logger;
using SMARTII.Assist.Web;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Master;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;
using SMARTII.Service.Organization.Resolver;

namespace SMARTII.Areas.Master.Controllers
{
    [Authentication]
    public class BillboardController : BaseApiController
    {
        private readonly IBillboardFacade _BillboardFacade;
        private readonly ICommonAggregate _CommonAggregate;
        private readonly IMasterAggregate _MasterAggregate;
        private readonly INotificationAggregate _NotificationAggregate;
        private readonly UserResolver _UserResolver;

        public BillboardController(
            IBillboardFacade BillboardFacade,
            ICommonAggregate CommonAggregate,
            IMasterAggregate MasterAggregate,
            INotificationAggregate NotificationAggregate,
            UserResolver UserResolver)
        {
            _BillboardFacade = BillboardFacade;
            _CommonAggregate = CommonAggregate;
            _MasterAggregate = MasterAggregate;
            _NotificationAggregate = NotificationAggregate;
            _UserResolver = UserResolver;
        }

        /// <summary>
        /// 取消個人通知
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(Billboard_lang.BILLBOARD_CLEAR_NOTIFICATION))]
        [ModelValidator(false)]
        public async Task<IHttpResult> ClearNotification()
        {
            try
            {
                var userID = UserIdentity.Instance?.UserID;

                var con = new MSSQLCondition<PERSONAL_NOTIFICATION>(
                                x => x.USER_ID == userID &&
                                x.PERSONAL_NOTIFICATION_TYPE == (byte)PersonalNotificationType.Billboard);

                _NotificationAggregate.PersonalNotification_T1_T2_.RemoveRange(con);

                var result = new JsonResult(Billboard_lang.BILLBOARD_CLEAR_NOTIFICATION, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                ex.PrefixDevMessage(Billboard_lang.BILLBOARD_CLEAR_NOTIFICATION_FAIL));

                return await new JsonResult(
                ex.PrefixMessage(Billboard_lang.BILLBOARD_CLEAR_NOTIFICATION_FAIL), false)
                  .Async();
            }
        }

        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(Billboard_lang.BILLBOARD_GET_LIST))]
        [ModelValidator(false)]
        public async Task<PagingResponse> GetList(PagingRequest<BillboardSearchViewModel> model)
        {
            try
            {
                var searchTerm = model.criteria;

                var con = new MSSQLCondition<BILL_BOARD>(
                   searchTerm,
                   model.pageIndex,
                   model.pageSize);

                con.OrderBy(model.sort, model.orderType);

                var list = _MasterAggregate.Billboard_T1_T2_.GetPaging(con);

                var ui = list.Select(x => new BillboardListViewModel(x));

                return await new PagingResponse<IEnumerable<BillboardListViewModel>>(ui)
                {
                    isSuccess = true,
                    totalCount = list.TotalCount
                }.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(Billboard_lang.BILLBOARD_GET_LIST_FAIL));

                return await new PagingResponse()
                {
                    isSuccess = false,
                    message = ex.PrefixMessage(Billboard_lang.BILLBOARD_GET_LIST_FAIL)
                }.Async();
            }
        }

        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(Billboard_lang.BILLBOARD_GET_OWN_LIST))]
        [ModelValidator(true)]
        public async Task<IHttpResult> GetOwnList(BillboardSearchViewModel model)
        {
            try
            {
                var userID = UserIdentity.Instance?.UserID;

                var con = new MSSQLCondition<BILL_BOARD>(model ?? new BillboardSearchViewModel());

                con.And(x => x.USER_IDs.Contains(userID));
                con.And(x => x.ACTIVE_DATE_START <= DateTime.Now);
                con.And(x => x.ACTIVE_DATE_END > DateTime.Now);

                con.OrderBy(x => x.ACTIVE_DATE_START, OrderType.Desc);
                con.OrderBy(x => x.WARNING_TYPE, OrderType.Desc);
               

                var list = _MasterAggregate.Billboard_T1_T2_.GetList(con);

                var ui = _UserResolver.GetUserImageResolve(list).Select(x => new BillboardListViewModel(x));

                var result = new JsonResult<IEnumerable<BillboardListViewModel>>(ui, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(Billboard_lang.BILLBOARD_GET_OWN_LIST_FAIL));

                return await new JsonResult(
                  ex.PrefixMessage(Billboard_lang.BILLBOARD_GET_OWN_LIST_FAIL), false)
                  .Async();
            }
        }

        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(Billboard_lang.BILLBOARD_GET))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Get([Required]int? ID)
        {
            try
            {
                var item = await _BillboardFacade.Get(ID.Value);

                var result = new JsonResult<BillboardDetailViewModel>(
                                   new BillboardDetailViewModel(item), true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(Billboard_lang.BILLBOARD_GET_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Billboard_lang.BILLBOARD_GET_FAIL), false)
                    .Async();
            }
        }

        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Add)]
        [Logger(nameof(Billboard_lang.BILLBOARD_CREATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Create(BillboardDetailViewModel model)
        {
            try
            {
                await _BillboardFacade.Create(model.ToDomain());

                return await new JsonResult(
                    Billboard_lang.BILLBOARD_CREATE_SUCCESS, true)
                    .Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(Billboard_lang.BILLBOARD_CREATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Billboard_lang.BILLBOARD_CREATE_FAIL), false)
                    .Async();
            }
        }

        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Update)]
        [Logger(nameof(Billboard_lang.BILLBOARD_UPDATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Update(BillboardDetailViewModel model)
        {
            try
            {
                await _BillboardFacade.Update(model.ToDomain());

                return await new JsonResult(
                    Billboard_lang.BILLBOARD_UPDATE_SUCCESS, true)
                    .Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(Billboard_lang.BILLBOARD_UPDATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Billboard_lang.BILLBOARD_UPDATE_FAIL), false)
                    .Async();
            }
        }

        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Delete)]
        [Logger(nameof(Billboard_lang.BILLBOARD_DELETE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Delete([Required]int? ID)
        {
            try
            {
                await _BillboardFacade.Delete(ID.Value);

                return await new JsonResult(
                    Billboard_lang.BILLBOARD_DELETE_SUCCESS, true)
                    .Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(Billboard_lang.BILLBOARD_DELETE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Billboard_lang.BILLBOARD_DELETE_FAIL), false)
                    .Async();
            }
        }

        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Delete)]
        [Logger(nameof(Billboard_lang.BILLBOARD_DELETE_RANGE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> DeleteRange(List<BillboardListViewModel> model)
        {
            try
            {
                var ids = model.Select(x => x.ID)
                               .ToArray();

                await _BillboardFacade.DeleteRange(ids);

                return await new JsonResult(
                    Billboard_lang.BILLBOARD_DELETE_RANGE_SUCCESS, true)
                    .Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(Billboard_lang.BILLBOARD_DELETE_RANGE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Billboard_lang.BILLBOARD_DELETE_RANGE_FAIL), false)
                    .Async();
            }
        }
    }
}
