using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using MoreLinq.Extensions;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Areas.Common.Models.Notification;
using SMARTII.Areas.Master.Models.Billboard;
using SMARTII.Areas.Master.Models.CaseRemind;
using SMARTII.Areas.Master.Models.NotificationGroupSender;
using SMARTII.Areas.Master.Models.PersonalNotification;
using SMARTII.Assist.Authentication;
using SMARTII.Assist.Web;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Case;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Master;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;
using SMARTII.Service.Organization.Resolver;

namespace SMARTII.Areas.Common.Controllers
{
    [Authentication]
    public class NotificationController : BaseApiController
    {
        private readonly ICaseAggregate _CaseAggregate;
        private readonly IMasterAggregate _MasterAggregate;
        private readonly ICommonAggregate _CommonAggregate;
        private readonly INotificationAggregate _NotificationAggregate;
        private readonly OrganizationNodeResolver _OrganizationResolver;

        public NotificationController(
           ICaseAggregate CaseAggregate,
            IMasterAggregate MasterAggregate,
            ICommonAggregate CommonAggregate,
            INotificationAggregate NotificationAggregate,
            OrganizationNodeResolver OrganizationResolver)
        {
            _CommonAggregate = CommonAggregate;
            _NotificationAggregate = NotificationAggregate;
            _CaseAggregate = CaseAggregate;
            _MasterAggregate = MasterAggregate;
            _OrganizationResolver = OrganizationResolver;
        }

        /// <summary>
        /// 依照企業別取得官網來信
        /// </summary>
        /// <param name="BuID"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetOfficialWebMailList")]
        public async Task<IHttpActionResult> GetOfficialWebMailListAsync(int? buID)
        {
            try
            {
                var con = new MSSQLCondition<OFFICIAL_EMAIL_GROUP>(x => x.NODE_ID == buID &&
                                                                        x.ORGANIZATION_TYPE == (byte)OrganizationType.HeaderQuarter);

                var group = _NotificationAggregate.OfficialEmailGroup_T1_T2_.GetList(con);

                return Ok(group?.Select(x => new { Email = x.MailAddress, OfficialDisplayName = x.MailDisplayName }));
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return await StatusCode(HttpStatusCode.InternalServerError).Async();
            }
        }

        /// <summary>
        /// 依據BU取得提醒群組清單
        /// </summary>
        /// <param name="nodeID"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetNotificationGroup")]
        public async Task<IHttpActionResult> GetNotificationGroupAsync(int nodeID)
        {
            try
            {
                var con = new MSSQLCondition<NOTIFICATION_GROUP>(x => x.NODE_ID == nodeID);
                con.OrderBy(x => x.ID, OrderType.Asc);

                var result = _NotificationAggregate.NotificationGroup_T1_T2_.GetPaging(con).ToList();

                var select2 = new Select2Response()
                {
                    items = Select2Response.ToSelectItems(result, x => x.ID.ToString(), x => x.Name)
                };

                return Ok(select2);
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return await StatusCode(HttpStatusCode.InternalServerError).Async();
            }
        }

        /// <summary>
        /// 取得提醒群組的人員清單
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetNotificationGroupUser")]
        public async Task<IHttpActionResult> GetNotificationGroupUserAsync(int? groupID)
        {
            try
            {
                var con = new MSSQLCondition<NOTIFICATION_GROUP_USER>(x => x.GROUP_ID == groupID);

                var result = _NotificationAggregate.NotificationGroupUser_T1_T2_
                                                   .GetPaging(con)
                                                   .Select(x => new NotificationGroupUserListViewModel(x));

                return Ok(result);
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return await StatusCode(HttpStatusCode.InternalServerError).Async();
            }
        }

        /// <summary>
        /// 依提醒人員取得案件提醒
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetCaseRemindList")]
        public async Task<IHttpActionResult> GetCaseRemindListAsync(string UserId)
        {
            try
            {
                var con = new MSSQLCondition<CASE_REMIND>();
                con.And(x => x.USER_IDs.Contains(UserId));
                con.And(x => x.ACTIVE_START_DAETTIME <= DateTime.Now);
                con.And(x => x.IS_CONFIRM == false);
                con.OrderBy(x => x.CREATE_DATETIME, OrderType.Desc);

                var caseReminds = _CaseAggregate.CaseRemind_T1_T2_.GetList(con);
                var items = _OrganizationResolver.ResolveCollection(caseReminds);

                #region 分開今天通知以及今天以前

                DateTime today = DateTime.Now.Date;

                var todoyList = items.Where(x => x.ActiveStartDateTime >= today)?
                                    .Select(x => new NotificationCaseRemindViewModel(x))
                                    .Cast<NotificationBaseViewModel>()
                                    .ToList();


                var beforeTodoyList = items.Where(x => x.ActiveStartDateTime < today)?
                                            .Select(x => new NotificationCaseRemindViewModel(x))
                                            .Cast<NotificationBaseViewModel>()
                                            .ToList();
                

                #endregion 分開今天通知以及今天以前

                SystemNotificationViewModel result = new SystemNotificationViewModel
                {
                    TodayNotification = new TodoyNotification
                    {
                        NotificationDatas = todoyList
                    },
                    BeforeNotification = new BeforeNotification
                    {
                        NotificationDatas = beforeTodoyList
                    }
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return await StatusCode(HttpStatusCode.InternalServerError).Async();
            }
        }

        /// <summary>
        /// 依人員取得系統通知清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetPersonalList")]
        public async Task<IHttpActionResult> GetPersonalListAsync(string userId)
        {
            try
            {

                #region 取得 PERSONAL_NOTIFICATION

                var con = new MSSQLCondition<PERSONAL_NOTIFICATION>();
                con.SystemNotificaitonFromPersonal();
                con.And(x => x.USER_ID == userId);


                con.OrderBy(x => x.CREATE_DATETIME, OrderType.Desc);

                var personaldata = _NotificationAggregate.PersonalNotification_T1_T2_.GetList(con);

                #endregion


                var todayData = new List<NotificationBaseViewModel>();
                var beforeData = new List<NotificationBaseViewModel>();


                #region 分開今天通知以及今天以前

                DateTime today = DateTime.Now.Date;

                todayData.AddRange(
                        personaldata.Where(x => x.CreateDateTime >= today)?.Select(x => new NotificationPersonalViewModel(x)) ?? new List<NotificationPersonalViewModel>()
                    );
                

                beforeData.AddRange(
                        personaldata.Where(x => x.CreateDateTime < today)?.Select(x => new NotificationPersonalViewModel(x)) ?? new List<NotificationPersonalViewModel>()
                    );
                
                #endregion 分開今天通知以及今天以前

                SystemNotificationViewModel result = new SystemNotificationViewModel
                {
                    TodayNotification = new TodoyNotification
                    {
                        NotificationDatas = todayData
                    },
                    BeforeNotification = new BeforeNotification
                    {
                        NotificationDatas = beforeData
                    }
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return await StatusCode(HttpStatusCode.InternalServerError).Async();
            }
        }

        /// <summary>
        /// 依人員取得官網來信
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetOfficialEmailList")]
        public async Task<IHttpActionResult> GetOfficialEmailListAsync(string userId)
        {
            try
            {

                var user = this.UserIdentity.Instance;
                
                var emailGroupIDs = _NotificationAggregate.OfficialEmailGroup_T1_T2_.GetListOfSpecific(
                                        new MSSQLCondition<OFFICIAL_EMAIL_GROUP>(x => x.USER.Any(g => g.USER_ID == user.UserID)), 
                                        x => x.ID
                                    );

                var emailDatas = _NotificationAggregate.OfficialEmailEffectivePayload_T1_T2_.GetList(
                                new MSSQLCondition<OFFICIAL_EMAIL_EFFECTIVE_DATA>(x => emailGroupIDs.Contains(x.EMAIL_GROUP_ID))
                            );

                
                var items = _OrganizationResolver.ResolveCollection(emailDatas).ToList();
                

                #region 分開今天通知以及今天以前

                DateTime today = DateTime.Now.Date;
                
                var todoyItems = emailDatas.Where(x => x.CreateDateTime >= today)?
                                                .Select(x => new NotificationOfficialEmailViewModel(x))
                                                .Cast<NotificationBaseViewModel>()
                                                .ToList() ?? new List<NotificationBaseViewModel>();

                var beforeItems = emailDatas.Where(x => x.CreateDateTime < today)?
                                                .Select(x => new NotificationOfficialEmailViewModel(x))
                                                .Cast<NotificationBaseViewModel>()
                                                .ToList() ?? new List<NotificationBaseViewModel>();

                #endregion 分開今天通知以及今天以前

                SystemNotificationViewModel result = new SystemNotificationViewModel
                {
                    TodayNotification = new TodoyNotification
                    {
                        NotificationDatas = todoyItems
                    },
                    BeforeNotification = new BeforeNotification
                    {
                        NotificationDatas = beforeItems
                    }
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return await StatusCode(HttpStatusCode.InternalServerError).Async();
            }
        }

        /// <summary>
        /// 依系統通知刪除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("DeletePersonal")]
        public async Task<IHttpResult> DeletePersonalAsync([Required]int? id)
        {
            try
            {
                var con = new MSSQLCondition<PERSONAL_NOTIFICATION>();
                con.And(x => x.ID == id);
                var result = _NotificationAggregate.PersonalNotification_T1_T2_.Remove(con);

                return await new JsonResult(true).Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return await new JsonResult(Common_lang.GET_DATA_FAIL, false).Async();
            }
        }

        /// <summary>
        /// 依系統通知刪除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("DeleteAllPersonal")]
        public async Task<IHttpResult> DeleteAllPersonalAsync([Required]string userID)
        {
            try
            {
                var con = new MSSQLCondition<PERSONAL_NOTIFICATION>();
                con.And(x => x.USER_ID == userID);

                var result = _NotificationAggregate.PersonalNotification_T1_T2_.RemoveRange(con);

                return await new JsonResult(true).Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return await new JsonResult<string>(Common_lang.GET_DATA_FAIL, false).Async();
            }
        }

        /// <summary>
        /// 依系統通知刪除(代辦事項)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("DeletePersonalCaseRemind")]
        public async Task<IHttpActionResult> DeletePersonalCaseRemindAsync([Required]int? id)
        {
            try
            {
                var con = new MSSQLCondition<CASE_REMIND>();
                con.And(x => x.ID == id);
                con.ActionModify(x => x.IS_CONFIRM = true);

                var result = _CaseAggregate.CaseRemind_T1_T2_.Update(con);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return await StatusCode(HttpStatusCode.InternalServerError).Async();
            }
        }

        /// <summary>
        /// 依人員取得系統通知數
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetNotificationCount")]
        public async Task<IHttpResult> GetNotificationCountAsync([Required] string userID)
        {
            try
            {


                #region 系統通知
                
                var notificationCon = new MSSQLCondition<PERSONAL_NOTIFICATION>();

                notificationCon.SystemNotificaitonFromPersonal();
                notificationCon.And(x => x.USER_ID == userID);
                var notificationCount = _NotificationAggregate.PersonalNotification_T1_T2_.Count(notificationCon);
                
                #endregion


                #region 案件追蹤


                var caseRemindCon = new MSSQLCondition<CASE_REMIND>(x => x.USER_IDs.Contains(userID));
                caseRemindCon.And(x => x.ACTIVE_START_DAETTIME <= DateTime.Now);
                caseRemindCon.And(x => x.IS_CONFIRM == false);
                var caseRemindCount = _CaseAggregate.CaseRemind_T1_T2_.Count(caseRemindCon);

                #endregion
                

                #region 官網來信

                var emailGroupIDs = _NotificationAggregate.OfficialEmailGroup_T1_T2_.GetListOfSpecific(
                                       new MSSQLCondition<OFFICIAL_EMAIL_GROUP>(x => x.USER.Any(g => g.USER_ID == userID)),
                                       x => x.ID
                                   );

                var officialEmailCount = _NotificationAggregate.OfficialEmailEffectivePayload_T1_T2_.Count(
                                new MSSQLCondition<OFFICIAL_EMAIL_EFFECTIVE_DATA>(x => emailGroupIDs.Contains(x.EMAIL_GROUP_ID))
                            );

                #endregion
                

                var result = new NotficationCalcViewModel()
                {
                    SystemNotificationCount = notificationCount,
                    CaseRemindCount = caseRemindCount,
                    OfficialEmailCount = officialEmailCount
                };

                return  await new JsonResult<NotficationCalcViewModel>(result, true).Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return await new JsonResult(ex.Message, false).Async();
            }
        }

    }
}
