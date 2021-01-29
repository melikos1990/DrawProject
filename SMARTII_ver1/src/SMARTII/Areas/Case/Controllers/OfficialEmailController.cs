using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac.Features.Indexed;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Areas.Case.Models;
using SMARTII.Assist.Authentication;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Case;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Report;
using SMARTII.Domain.Thread;
using SMARTII.Service.Case.Flow;
using SMARTII.Service.Case.Resolver;
using SMARTII.Service.Organization.Provider;
using System.IO;
using SMARTII.Domain.IO;
using SMARTII.Domain.Notification.Email;
using SMARTII.Service.Master.Resolver;
using SMARTII.Service.Cache;
using SMARTII.Assist.Logger;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Resource.Tag;
using SMARTII.Areas.Case.Models.OfficialEmail;
using SMARTII.Domain.Notification;
using System.ComponentModel.DataAnnotations;
using SMARTII.Assist.Web;

namespace SMARTII.Areas.Case.Controllers
{
    [Authentication]
    public class OfficialEmailController : BaseApiController
    {
        private readonly IIndex<string, IFlow> _Flows;
        private readonly ICaseFacade _CaseFacade;
        private readonly ICaseService _CaseService;
        private readonly INotificationAggregate _NotificationAggregate;
        private readonly ICommonAggregate _CommonAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly OfficeEmailGroupResolver _OfficeEmailGroupResolver;

        public OfficialEmailController(IIndex<string, IFlow> Flows,
                                       ICaseFacade CaseFacade,
                                       ICaseService CaseService,
                                       INotificationAggregate NotificationAggregate,
                                       ICommonAggregate CommonAggregate,
                                       IOrganizationAggregate OrganizationAggregate,
                                       OfficeEmailGroupResolver OfficeEmailGroupResolver
                                       )
        {
            _Flows = Flows;
            _CaseFacade = CaseFacade;
            _CaseService = CaseService;
            _NotificationAggregate = NotificationAggregate;
            _CommonAggregate = CommonAggregate;
            _OrganizationAggregate = OrganizationAggregate;
            _OfficeEmailGroupResolver = OfficeEmailGroupResolver;
        }


        /// <summary>
        /// 取得清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(OfficialEmail_lang.OFFICIAL_EMAIL_GETLIST))]
        [ModelValidator(false)]
        public async Task<PagingResponse> GetList(PagingRequest<OfficialEmailSearchViewModel> model)
        {
            try
            {
                var searchTerm = model.criteria;

                var con = new MSSQLCondition<OFFICIAL_EMAIL_EFFECTIVE_DATA>(
                   searchTerm);

                con.OrderBy(model.sort, model.orderType);

                var filterList = _NotificationAggregate.OfficialEmailEffectivePayload_T1_T2_.GetPaging(con).ToList();

                if (!string.IsNullOrEmpty(model.criteria.Subject))
                {
                    filterList = filterList.Where(x => x.Subject.Contains(model.criteria.Subject) || x.Body.Contains(model.criteria.Subject)).ToList();
                }
                if (!string.IsNullOrEmpty(searchTerm.DateRange))
                {
                    filterList = filterList.Where(x => x.ReceivedDateTime >= searchTerm.StarDate && x.ReceivedDateTime <= searchTerm.EndDate).ToList();
                }

                int totalCount = filterList.Count();

                if (model.pageIndex >= 0 && model.pageSize >= 0)
                {
                    filterList = filterList.Skip(model.pageIndex * model.pageSize).Take(model.pageSize).ToList();
                }

                var ui = _OfficeEmailGroupResolver.ResolveCollection(filterList).Select(x => new OfficialEmailListViewModel(x));

                return await new PagingResponse<IEnumerable<OfficialEmailListViewModel>>(ui)
                {
                    isSuccess = true,
                    totalCount = totalCount
                }.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(OfficialEmail_lang.OFFICIAL_EMAIL_GETLIST_FAIL));

                return await new PagingResponse()
                {
                    isSuccess = false,
                    message = ex.PrefixMessage(OfficialEmail_lang.OFFICIAL_EMAIL_GETLIST_FAIL)
                }.Async();
            }
        }

        /// <summary>
        /// 批次刪除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Delete)]
        [Logger(nameof(OfficialEmail_lang.OFFICIAL_EMAIL_DELETE_RANGE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> DeleteRange(List<OfficialEmailListViewModel> model)
        {
            try
            {
                var con = new MSSQLCondition<OFFICIAL_EMAIL_EFFECTIVE_DATA>();

                model.ForEach(g => con.Or(x => x.MESSAGE_ID == g.MessageID && x.NODE_ID == g.BuID));

                var isSuccess = _NotificationAggregate.OfficialEmailEffectivePayload_T1_T2_.RemoveRange(con);

                if (isSuccess == false)
                    throw new Exception(Common_lang.DATABASE_DELETE_ERROR);

                return await new JsonResult(
                 OfficialEmail_lang.OFFICIAL_EMAIL_DELETE_RANGE_SUCCESS, true)
                 .Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(OfficialEmail_lang.OFFICIAL_EMAIL_DELETE_RANGE_FAILED));

                return await new JsonResult(
                    ex.PrefixMessage(OfficialEmail_lang.OFFICIAL_EMAIL_DELETE_RANGE_FAILED), false)
                    .Async();
            }
        }

        /// <summary>
        /// 信件認養
        /// </summary>
        /// <param name="messageID"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Update)]
        [Logger(nameof(OfficialEmail_lang.OFFICIAL_EMAIL_ADOPT))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Adopt(OfficialEmailAdoptViewModel officialEmailAdoptViewModel)
        {
            try
            {
                var result = await _CaseService.Adopt(officialEmailAdoptViewModel.MessageID, officialEmailAdoptViewModel.BuID, officialEmailAdoptViewModel.GroupID);
                return await new JsonResult<string>(result, OfficialEmail_lang.OFFICIAL_EMAIL_ADOPT_SUCCESS, true).Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(OfficialEmail_lang.OFFICIAL_EMAIL_ADOPT_FAILED));

                return await new JsonResult(
                    ex.PrefixMessage(OfficialEmail_lang.OFFICIAL_EMAIL_ADOPT_FAILED), false)
                    .Async();
            }
        }

        /// <summary>
        /// 管理者指派
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Update)]
        [Logger(nameof(OfficialEmail_lang.OFFICIAL_EMAIL_ADMIN_ORDER))]
        [ModelValidator(true)]
        public async Task<IHttpResult> AdminOrder(OfficialEmailAdminOrderViewModel model)
        {
            try
            {
                //指定某些案件給特定的人
                var result = await _CaseService.AdminOrder(model.MessageIDs, model.BuID, model.UserID, model.GroupID);

                return await new JsonResult<CounterResult>(result, OfficialEmail_lang.OFFICIAL_EMAIL_ADMIN_ORDER_SUCCESS, true).Async();

            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(OfficialEmail_lang.OFFICIAL_EMAIL_ADMIN_ORDER_FAILED));

                return await new JsonResult(
                ex.PrefixMessage(OfficialEmail_lang.OFFICIAL_EMAIL_ADMIN_ORDER_FAILED), false)
               .Async();
            }
        }

        /// <summary>
        /// 自動分派
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Update)]
        [Logger(nameof(OfficialEmail_lang.OFFICIAL_EMAIL_AUTO_ORDER))]
        [ModelValidator(true)]
        public async Task<IHttpResult> AutoOrder(OfficialEmailAutoOrderViewModel model)
        {
            try
            {
                //分派案件，立來源&案件
                var result = await _CaseService.AutoOrder(model.EachPersonMail.Value, model.UserIDs, model.BuID, model.GroupID);

                return await new JsonResult<CounterResult>(result, OfficialEmail_lang.OFFICIAL_EMAIL_AUTO_ORDER_SUCCESS, true).Async();

            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(OfficialEmail_lang.OFFICIAL_EMAIL_AUTO_ORDER_FAILED));

                return await new JsonResult(
                ex.PrefixMessage(OfficialEmail_lang.OFFICIAL_EMAIL_AUTO_ORDER_FAILED), false)
               .Async();
            }
        }

        /// <summary>
        /// 批次回覆     
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Update)]
        [Logger(nameof(OfficialEmail_lang.OFFICIAL_EMAIL_REPLY_RANGE))]
        [ModelValidator(true)]
        public async Task<IHttpResult> ReplyRenge(OfficialEmailReplyRengeViewModel model)
        {
            try
            {
                //先立來源+案件，並結案 再回信給消費者
                var result = await _CaseService.ReplyRange(model.QuestionID, model.EmailContent, model.FinishContent, model.MessageIDs, model.BuID, model.GroupID);

                return await new JsonResult<CounterResult>(result, OfficialEmail_lang.OFFICIAL_EMAIL_REPLY_RANGE_SUCCESS, true).Async();

            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(OfficialEmail_lang.OFFICIAL_EMAIL_REPLY_RANGE_FAILED));

                return await new JsonResult(
                ex.PrefixMessage(OfficialEmail_lang.OFFICIAL_EMAIL_REPLY_RANGE_FAILED), false)
               .Async();
            }
        }

        /// <summary>
        /// 批次信件認養
        /// </summary>
        /// <param name="messageID"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Update)]
        [Logger(nameof(OfficialEmail_lang.OFFICIAL_EMAIL_BATCHADOPT))]
        [ModelValidator(false)]
        public async Task<IHttpResult> BatchAdopt(OfficialEmailBatchAdoptViewModel model)
        {
            try
            {
                var ss = new OfficialEmailBatchAdoptViewModel()
                {
                    BuID = 10
                };
                var result = new List<string>();

                foreach (var msgitem in model.MessageIDs)
                {
                    var sn = await _CaseService.Adopt(msgitem, model.BuID, model.GroupID);
                    result.Add(sn);
                }
                return await new JsonResult(OfficialEmail_lang.OFFICIAL_EMAIL_BATCHADOPT_SUCCESS, true).Async();

                //return await new JsonResult<string>(result, OfficialEmail_lang.OFFICIAL_EMAIL_ADOPT_SUCCESS, true).Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(OfficialEmail_lang.OFFICIAL_EMAIL_BATCHADOPT_FAILED));

                return await new JsonResult(
                    ex.PrefixMessage(OfficialEmail_lang.OFFICIAL_EMAIL_BATCHADOPT_FAILED), false)
                    .Async();
            }
        }
    }
}
