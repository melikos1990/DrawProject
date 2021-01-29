using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Areas.Master.Models.NotificationGroupSender;
using SMARTII.Assist.Authentication;
using SMARTII.Assist.Logger;
using SMARTII.Assist.Web;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;
using SMARTII.Service.Master.Resolver;
using SMARTII.Service.Organization.Resolver;

namespace SMARTII.Areas.Master.Controllers
{
    [Authentication]
    public class NotificationGroupSenderController : BaseApiController
    {
        private readonly ICommonAggregate _CommonAggregate;
        private readonly INotificationAggregate _NotificationAggregate;
        private readonly INotificationGroupService _NotificationGroupService;
        private readonly UserResolver _UserResolver;
        private readonly ItemResolver _ItemResolver;
        private readonly OrganizationNodeResolver _OrganizationNodeResolver;
        private readonly QuestionClassificationResolver _QuestionClassificationResolver;

        public NotificationGroupSenderController(UserResolver UserResolver,
                                                 ItemResolver ItemResolver,
                                                 OrganizationNodeResolver OrganizationNodeResolver,
                                                 QuestionClassificationResolver QuestionClassificationResolver,
                                                 INotificationGroupService NotificationGroupService,
                                                 INotificationAggregate NotificationAggregate,
                                                 ICommonAggregate CommonAggregate)
        {
            _UserResolver = UserResolver;
            _ItemResolver = ItemResolver;
            _OrganizationNodeResolver = OrganizationNodeResolver;
            _QuestionClassificationResolver = QuestionClassificationResolver;
            _CommonAggregate = CommonAggregate;
            _NotificationAggregate = NotificationAggregate;
            _NotificationGroupService = NotificationGroupService;
        }

        /// <summary>
        /// 取消個人通知
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(NotificationGroupSender_lang.NOTIFICATION_GROUP_SENDER_CLEAR_NOTIFICATION))]
        [ModelValidator(false)]
        public async Task<IHttpResult> ClearNotification()
        {
            try
            {
                var userID = UserIdentity.Instance?.UserID;

                var con = new MSSQLCondition<PERSONAL_NOTIFICATION>(
                                x => x.USER_ID == userID &&
                                x.PERSONAL_NOTIFICATION_TYPE == (byte)PersonalNotificationType.NotificationGroup);

                _NotificationAggregate.PersonalNotification_T1_T2_.RemoveRange(con);

                var result = new JsonResult(NotificationGroupSender_lang.NOTIFICATION_GROUP_SENDER_CLEAR_NOTIFICATION_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                ex.PrefixDevMessage(NotificationGroupSender_lang.NOTIFICATION_GROUP_SENDER_CLEAR_NOTIFICATION_FAIL));

                return await new JsonResult(
                ex.PrefixMessage(NotificationGroupSender_lang.NOTIFICATION_GROUP_SENDER_CLEAR_NOTIFICATION_FAIL), false)
                  .Async();
            }
        }

        /// <summary>
        /// 取得達標清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(NotificationGroupSender_lang.NOTIFICATION_GROUP_SENDER_GET_LIST))]
        [ModelValidator(false)]
        public async Task<PagingResponse> GetArrivedList(PagingRequest model)
        {
            try
            {
                var con = new MSSQLCondition<NOTIFICATION_GROUP>(model.pageIndex, model.pageSize);

                con.OrderBy(model.sort, model.orderType);
                con.And(x => x.IS_ARRIVE);

                var list = _NotificationAggregate.NotificationGroup_T1_T2_.GetPaging(con);

                var ui = _QuestionClassificationResolver.ResolveNullableCollection
                            (
                               _ItemResolver.ResolveNullableCollection
                               (
                                    _OrganizationNodeResolver.ResolveCollection(list)
                               )
                            )
                         .Select(x => new NotificationGroupSenderListViewModel(x));

                return await new PagingResponse<IEnumerable<NotificationGroupSenderListViewModel>>(ui)
                {
                    isSuccess = true,
                    totalCount = list.TotalCount
                }.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(NotificationGroupSender_lang.NOTIFICATION_GROUP_SENDER_GET_LIST_FAIL));

                return await new PagingResponse()
                {
                    isSuccess = false,
                    message = ex.PrefixMessage(NotificationGroupSender_lang.NOTIFICATION_GROUP_SENDER_GET_LIST_FAIL)
                }.Async();
            }
        }

        /// <summary>
        /// 取得人員清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(NotificationGroupSender_lang.NOTIFICATION_GROUP_SENDER_GET_USER_LIST))]
        public async Task<IHttpResult> GetUserList([Required]int? groupID)
        {
            try
            {
                var con = new MSSQLCondition<NOTIFICATION_GROUP_USER>(x => x.GROUP_ID == groupID);

                var list = _NotificationAggregate.NotificationGroupUser_T1_T2_.GetList(con);

                var ui = list.Select(x => new NotificationGroupUserListViewModel(x));

                return await new JsonResult<IEnumerable<NotificationGroupUserListViewModel>>(ui, true).Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(NotificationGroupSender_lang.NOTIFICATION_GROUP_SENDER_GET_USER_LIST_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(NotificationGroupSender_lang.NOTIFICATION_GROUP_SENDER_GET_USER_LIST_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 取得歷程清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(NotificationGroupSender_lang.NOTIFICATION_GROUP_SENDER_GET_RESUME_LIST))]
        [ModelValidator(false)]
        public async Task<PagingResponse> GetResumeList(PagingRequest<NotificationGroupSenderResumeSearchViewModel> model)
        {
            try
            {
                var buIDs = UserIdentity.Instance.DownProviderBUDist.TryGetBuList(Domain.Organization.OrganizationType.CallCenter);

                var searchTerm = model.criteria;

                var con = new MSSQLCondition<NOTIFICATION_GROUP_RESUME>(
                    searchTerm,
                    model.pageIndex,
                    model.pageSize);

                //若無指定BU查詢，將會撈出該人員負責之BU
                if (searchTerm.NodeID == null)
                {
                    con.And(x => buIDs.Contains(x.NODE_ID));
                }

                con.OrderBy(model.sort, model.orderType);
                con.IncludeBy(x => x.NOTIFICATION_GROUP);

                var list = _NotificationAggregate.NotificationGroupResume_T1_T2_.GetPaging(con);

                var ui = list.Select(x => new NotificationGroupSenderResumeListViewModel(x));

                return await new PagingResponse<IEnumerable<NotificationGroupSenderResumeListViewModel>>(ui)
                {
                    isSuccess = true,
                    totalCount = list.TotalCount
                }.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(NotificationGroupSender_lang.NOTIFICATION_GROUP_SENDER_GET_RESUME_LIST_FAIL));

                return await new PagingResponse()
                {
                    isSuccess = false,
                    message = ex.PrefixMessage(NotificationGroupSender_lang.NOTIFICATION_GROUP_SENDER_GET_RESUME_LIST_FAIL)
                }.Async();
            }
        }

        /// <summary>
        /// 不通知對象
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(NotificationGroupSender_lang.NOTIFICATION_GROUP_SENDER_NO_SEND))]
        [ModelValidator(true)]
        public async Task<IHttpResult> NoSend([Required]int? groupID)
        {
            try
            {
                _NotificationGroupService.NoSend(groupID.Value);

                var result = new JsonResult(NotificationGroupSender_lang.NOTIFICATION_GROUP_SENDER_NO_SEND_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(NotificationGroupSender_lang.NOTIFICATION_GROUP_SENDER_NO_SEND_FAIL));

                return await new JsonResult(
                ex.PrefixMessage(NotificationGroupSender_lang.NOTIFICATION_GROUP_SENDER_NO_SEND_FAIL), false)
                  .Async();
            }
        }

        /// <summary>
        /// 通知對象
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(NotificationGroupSender_lang.NOTIFICATION_GROUP_SENDER_SEND))]
        [ModelValidator(true)]
        public async Task<IHttpResult> Send(NotificationGroupSenderExecuteViewModel model)
        {
            try
            {
                _NotificationGroupService.Send(model.GroupID, model.Payload);

                var result = new JsonResult<string>(NotificationGroupSender_lang.NOTIFICATION_GROUP_SENDER_SEND_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(NotificationGroupSender_lang.NOTIFICATION_GROUP_SENDER_SEND_FAIL));

                return await new JsonResult(
                ex.PrefixMessage(NotificationGroupSender_lang.NOTIFICATION_GROUP_SENDER_SEND_FAIL), false)
               .Async();
            }
        }
    }
}
