using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Areas.Master.Models.OfficialEmailGroup;
using SMARTII.Assist.Authentication;
using SMARTII.Assist.Logger;
using SMARTII.Assist.Web;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;
using SMARTII.Service.Organization.Resolver;

namespace SMARTII.Areas.Master.Controllers
{
    [Authentication]
    public class OfficialEmailGroupController : BaseApiController
    {
        private readonly IOfficialEmailGroupFacade _OfficialEmailGroup;
        private readonly ICommonAggregate _CommonAggregate;
        private readonly INotificationAggregate _NotificationAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly OrganizationNodeResolver _OrganizationResolver;

        public OfficialEmailGroupController(IOfficialEmailGroupFacade OfficialEmailGroup,
            ICommonAggregate CommonAggregate,
            INotificationAggregate NotificationAggregate,
            IOrganizationAggregate OrganizationAggregate,
            OrganizationNodeResolver OrganizationResolver)
        {
            _OfficialEmailGroup = OfficialEmailGroup;
            _CommonAggregate = CommonAggregate;
            _NotificationAggregate = NotificationAggregate;
            _OrganizationAggregate = OrganizationAggregate;
            _OrganizationResolver = OrganizationResolver;
        }

        /// <summary>
        /// 單一新增官網來信
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Add)]
        [Logger(nameof(OfficialEmailGroup_lang.OFFICIAL_EMAIL_GROUP_CREATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Create(OfficialEmailGroupDetailViewModel model)
        {
            try
            {
                var domain = model.ToDomain();

                await _OfficialEmailGroup.Create(domain);

                var result = new JsonResult<string>(OfficialEmailGroup_lang.OFFICIAL_EMAIL_GROUP_CREATE_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(OfficialEmailGroup_lang.OFFICIAL_EMAIL_GROUP_CREATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(OfficialEmailGroup_lang.OFFICIAL_EMAIL_GROUP_CREATE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 單一取得
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(OfficialEmailGroup_lang.OFFICIAL_EMAIL_GROUP_GET))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Get([Required]int? ID)
        {
            try
            {
                var con = new MSSQLCondition<OFFICIAL_EMAIL_GROUP>(x => x.ID == ID);
                con.IncludeBy(x => x.USER);
                var data = _NotificationAggregate.OfficialEmailGroup_T1_T2_.Get(con);
                if (data == null)
                    throw new Exception(Common_lang.NOT_FOUND_DATA);

                var complete = _OrganizationResolver.Resolve(data);

                var result = new JsonResult<OfficialEmailGroupDetailViewModel>(
                                   new OfficialEmailGroupDetailViewModel(complete), true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(OfficialEmailGroup_lang.OFFICIAL_EMAIL_GROUP_GET_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(OfficialEmailGroup_lang.OFFICIAL_EMAIL_GROUP_GET_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 取得清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(OfficialEmailGroup_lang.OFFICIAL_EMAIL_GROUP_GETLIST))]
        [ModelValidator(false)]
        public async Task<PagingResponse> GetList(PagingRequest<OfficialEmailGroupSearchViewModel> model)
        {
            try
            {
                var searchTerm = model.criteria;

                var con = new MSSQLCondition<OFFICIAL_EMAIL_GROUP>(
                   searchTerm,
                   model.pageIndex,
                   model.pageSize);
                con.IncludeBy(x => x.USER);

                //若無指定BU查詢，將會撈出該人員負責之BU
                if (searchTerm.BuID == null)
                {
                    var buIDs = UserIdentity.Instance.DownProviderBUDist.TryGetBuList(Domain.Organization.OrganizationType.CallCenter).Cast<int?>();
                    con.And(x => buIDs.Contains(x.NODE_ID));
                }

                con.OrderBy(model.sort, model.orderType);

                var list = _NotificationAggregate.OfficialEmailGroup_T1_T2_.GetPaging(con);

                var ui = _OrganizationResolver.ResolveCollection(list)
                         .Select(x => new OfficialEmailGroupListViewModel(x));

                return await new PagingResponse<IEnumerable<OfficialEmailGroupListViewModel>>(ui)
                {
                    isSuccess = true,
                    totalCount = list.TotalCount
                }.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(OfficialEmailGroup_lang.OFFICIAL_EMAIL_GROUP_GETLIST_FAIL));

                return await new PagingResponse()
                {
                    isSuccess = false,
                    message = ex.PrefixMessage(OfficialEmailGroup_lang.OFFICIAL_EMAIL_GROUP_GETLIST_FAIL)
                }.Async();
            }
        }

        /// <summary>
        /// 單一更新官網來信
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Update)]
        [Logger(nameof(OfficialEmailGroup_lang.OFFICIAL_EMAIL_GROUP_UPDATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Update(OfficialEmailGroupDetailViewModel model)
        {
            try
            {
                var domain = model.ToDomain();

                await _OfficialEmailGroup.Update(domain);

                var result = new JsonResult<string>(OfficialEmailGroup_lang.OFFICIAL_EMAIL_GROUP_UPDATE_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(OfficialEmailGroup_lang.OFFICIAL_EMAIL_GROUP_UPDATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(OfficialEmailGroup_lang.OFFICIAL_EMAIL_GROUP_UPDATE_FAIL), false)
                    .Async();
            }
        }

        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Delete)]
        [Logger(nameof(OfficialEmailGroup_lang.OFFICIAL_EMAIL_GROUP_DISABLE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Disable([Required]int? ID)
        {
            try
            {
                var con = new MSSQLCondition<OFFICIAL_EMAIL_GROUP>(x => x.ID == ID);
                con.ActionModify(x => x.IS_ENABLED = false);

                _NotificationAggregate.OfficialEmailGroup_T1_T2_.Update(con);

                var result = new JsonResult<string>(OfficialEmailGroup_lang.OFFICIAL_EMAIL_GROUP_DISABLE_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(OfficialEmailGroup_lang.OFFICIAL_EMAIL_GROUP_DISABLE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(OfficialEmailGroup_lang.OFFICIAL_EMAIL_GROUP_DISABLE_FAIL), false)
                    .Async();
            }
        }
    }
}
