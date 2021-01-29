using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Areas.Master.Models.NotificationGroup;
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
    public class NotificationGroupController : BaseApiController
    {
        private readonly INotificationGroupFacade _NotificationGroupFacade;
        private readonly INotificationAggregate _NotificationAggregate;
        private readonly ICommonAggregate _CommonAggregate;
        private readonly UserResolver _UserResolver;
        private readonly ItemResolver _ItemResolver;
        private readonly OrganizationNodeResolver _OrganizationNodeResolver;
        private readonly QuestionClassificationResolver _QuestionClassificationResolver;

        public NotificationGroupController(UserResolver UserResolver,
                                           ItemResolver ItemResolver,
                                           OrganizationNodeResolver OrganizationNodeResolver,
                                           QuestionClassificationResolver QuestionClassificationResolver,
                                           ICommonAggregate CommonAggregate,
                                           INotificationAggregate NotificationAggregate,
                                           INotificationGroupFacade NotificationGroupFacade)

        {
            _UserResolver = UserResolver;
            _ItemResolver = ItemResolver;
            _CommonAggregate = CommonAggregate;
            _NotificationAggregate = NotificationAggregate;
            _NotificationGroupFacade = NotificationGroupFacade;
            _OrganizationNodeResolver = OrganizationNodeResolver;
            _QuestionClassificationResolver = QuestionClassificationResolver;
        }

        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(NotificationGroup_lang.NOTIFICATION_GROUP_GET_LIST))]
        [ModelValidator(false)]
        public async Task<PagingResponse> GetList(PagingRequest<NotificationGroupSearchViewModel> model)
        {
            try
            {
                var buIDs = UserIdentity.Instance.DownProviderBUDist.TryGetBuList(Domain.Organization.OrganizationType.CallCenter);

                var searchTerm = model.criteria;

                var con = new MSSQLCondition<NOTIFICATION_GROUP>(
                   searchTerm,
                   model.pageIndex,
                   model.pageSize);

                //若無指定BU查詢，將會撈出該人員負責之BU
                if (searchTerm.NodeID == null)
                {
                    con.And(x => buIDs.Contains(x.NODE_ID));
                }

                con.OrderBy(model.sort, model.orderType);

                var list = _NotificationAggregate.NotificationGroup_T1_T2_.GetPaging(con);

                var ui = _QuestionClassificationResolver.ResolveNullableCollection
                            (
                               _ItemResolver.ResolveNullableCollection
                               (
                                    _OrganizationNodeResolver.ResolveCollection(list)
                               )
                            )
                         .Select(x => new NotificationGroupListViewModel(x))
                         .ToList();

                return await new PagingResponse<IEnumerable<NotificationGroupListViewModel>>(ui)
                {
                    isSuccess = true,
                    totalCount = list.TotalCount
                }.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(NotificationGroup_lang.NOTIFICATION_GROUP_GET_LIST_FAIL));

                return await new PagingResponse()
                {
                    isSuccess = false,
                    message = ex.PrefixMessage(NotificationGroup_lang.NOTIFICATION_GROUP_GET_LIST_FAIL)
                }.Async();
            }
        }

        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(NotificationGroup_lang.NOTIFICATION_GROUP_GET))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Get([Required]int? ID)
        {
            try
            {
                var con = new MSSQLCondition<NOTIFICATION_GROUP>(x => x.ID == ID.Value);
                con.IncludeBy(x => x.NOTIFICATION_GROUP_USER);

                var data = _NotificationAggregate.NotificationGroup_T1_T2_.Get(con);

                var complete = _QuestionClassificationResolver.ResolveNullable
                            (
                               _ItemResolver.ResolveNullable
                               (
                                    _OrganizationNodeResolver.Resolve(data)
                               )
                            );

                var result = new JsonResult<NotificationGroupDetailViewModel>(
                                 new NotificationGroupDetailViewModel(complete), true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(NotificationGroup_lang.NOTIFICATION_GROUP_GET_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(NotificationGroup_lang.NOTIFICATION_GROUP_GET_FAIL), false)
                    .Async();
            }
        }

        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Update)]
        [Logger(nameof(NotificationGroup_lang.NOTIFICATION_GROUP_UPDATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Update(NotificationGroupDetailViewModel model)
        {
            try
            {
                var domain = model.ToDomain();

                await _NotificationGroupFacade.Update(domain);

                var result = new JsonResult<string>(NotificationGroup_lang.NOTIFICATION_GROUP_UPDATE_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                   ex.PrefixDevMessage(NotificationGroup_lang.NOTIFICATION_GROUP_UPDATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(NotificationGroup_lang.NOTIFICATION_GROUP_UPDATE_FAIL), false)
                    .Async();
            }
        }

        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Add)]
        [Logger(nameof(NotificationGroup_lang.NOTIFICATION_GROUP_CREATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Create(NotificationGroupDetailViewModel model)
        {
            try
            {
                var domain = model.ToDomain();

                await _NotificationGroupFacade.Create(domain);

                var result = new JsonResult<string>(NotificationGroup_lang.NOTIFICATION_GROUP_CREATE_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                   ex.PrefixDevMessage(NotificationGroup_lang.NOTIFICATION_GROUP_CREATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(NotificationGroup_lang.NOTIFICATION_GROUP_CREATE_FAIL), false)
                    .Async();
            }
        }

        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Delete)]
        [Logger(nameof(NotificationGroup_lang.NOTIFICATION_GROUP_DELETE_RANGE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> DeleteRange(List<NotificationGroupListViewModel> model)
        {
            try
            {
                var ids = model?.Select(x => x.ID).ToList();

                var con = new MSSQLCondition<NOTIFICATION_GROUP>(x => ids.Contains(x.ID));
                con.IncludeBy(x => x.NOTIFICATION_GROUP_RESUME);
                con.IncludeBy(x => x.NOTIFICATION_GROUP_USER);

                _NotificationAggregate.NotificationGroup_T1_T2_.RemoveRange(con);

                var result = new JsonResult<string>(NotificationGroup_lang.NOTIFICATION_GROUP_DELETE_RANGE_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                   ex.PrefixDevMessage(NotificationGroup_lang.NOTIFICATION_GROUP_DELETE_RANGE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(NotificationGroup_lang.NOTIFICATION_GROUP_DELETE_RANGE_FAIL), false)
                    .Async();
            }
        }

        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Delete)]
        [Logger(nameof(NotificationGroup_lang.NOTIFICATION_GROUP_DELETE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Delete(int? ID)
        {
            try
            {
                var con = new MSSQLCondition<NOTIFICATION_GROUP>(x => x.ID == ID);

                con.IncludeBy(x => x.NOTIFICATION_GROUP_RESUME);
                con.IncludeBy(x => x.NOTIFICATION_GROUP_USER);

                _NotificationAggregate.NotificationGroup_T1_T2_.Remove(con);

                var result = new JsonResult<string>(NotificationGroup_lang.NOTIFICATION_GROUP_DELETE_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                   ex.PrefixDevMessage(NotificationGroup_lang.NOTIFICATION_GROUP_DELETE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(NotificationGroup_lang.NOTIFICATION_GROUP_DELETE_FAIL), false)
                    .Async();
            }
        }
    }
}
