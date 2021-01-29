using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Case;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Substitute;
using SMARTII.Domain.Thread;
using SMARTII.Domain.Transaction;
using SMARTII.Resource.Tag;

namespace SMARTII.Service.Substitute.Facade
{
    public class CaseApplyFacade : ICaseApplyFacade
    {
        private readonly IUserFacade _UserFacade;
        private readonly ICaseFacade _CaseFacade;
        private readonly ICaseAggregate _CaseAggregate;
        private readonly INotificationAggregate _NotificationAggregate;
        private readonly INotificationPersonalFacade _NotificationPersonalFacade;

        public CaseApplyFacade(IUserFacade UserFacade,
                               ICaseFacade CaseFacade,
                               ICaseAggregate CaseAggregate,
                               INotificationAggregate NotificationAggregate,
                               INotificationPersonalFacade NotificationPersonalFacade)
        {
            _UserFacade = UserFacade;
            _CaseFacade = CaseFacade;
            _CaseAggregate = CaseAggregate;
            _NotificationAggregate = NotificationAggregate;
            _NotificationPersonalFacade = NotificationPersonalFacade;
        }

        /// <summary>
        /// 分派案件
        /// </summary>
        /// <param name="user"></param>
        /// <param name="caseIDs"></param>
        /// <returns></returns>
        public async Task Apply(User user, List<string> caseIDs)
        {
            var con = new MSSQLCondition<CASE>(x => caseIDs.Contains(x.CASE_ID));
            var caseList = _CaseAggregate.Case_T1_T2_.GetList(con).ToList();

            #region 檢查相關權限

            //檢查是否有指派人員
            if (user == null)
                throw new Exception(Common_lang.USER_UNDEFIND);

            //檢查該人員有無此Group權限

            caseList.ForEach(x =>
            {
                if (x == null)
                    throw new Exception(Common_lang.MODEL_INVALID);

                int groupId = x.GroupID ?? throw new Exception(Common_lang.MODEL_INVALID);
                _CaseFacade.CheckGroupAuth(groupId, user);
            });

            #endregion

            var nowDateTime = DateTime.Now;

            con.ActionModify(x =>
            {
                x.UPDATE_DATETIME = nowDateTime;
                x.UPDATE_USERNAME = ContextUtility.GetUserIdentity()?.Name;
                //負責人
                x.APPLY_USERNAME = user.Name;
                x.APPLY_USER_ID = user.UserID;
                x.APPLY_DAETTIME = DateTime.Now;
            });

            var personalNotification = new PersonalNotification()
            {
                UserID = user.UserID,
                CreateDateTime = nowDateTime,
                CreateUserName = ContextUtility.GetUserIdentity()?.Name,
                PersonalNotificationType = PersonalNotificationType.CaseAssign,
                Content = string.Format(SysCommon_lang.CASE_APPLY_PERSONAL_NOTICE, ContextUtility.GetUserIdentity()?.Name, caseList.Count().ToString())
            };

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                _CaseAggregate.Case_T1_T2_.UpdateRange(con);


                foreach (var item in caseList)
                {
                    #region 新增案件歷程
                    var caseResume = new CaseResume()
                    {
                        CaseID = item.CaseID,
                        CreateDateTime = nowDateTime,
                        CreateUserName = ContextUtility.GetUserIdentity()?.Name,
                        Content = string.Format(SysCommon_lang.CASE_APPLY_CASE_RESUME, ContextUtility.GetUserIdentity()?.Name, user.Name),
                        CaseType = item.CaseType
                    };

                    _CaseAggregate.CaseResume_T1_T2_.Add(caseResume);
                    #endregion

                    #region 新增CASE_HISTORY
                    _CaseFacade.CreateHistory(item.CaseID, null, CaseApply_lang.CASE_APPLY_APPLY, user);
                    #endregion

                    #region 新增至通知
                    var caseNoticeCon = new MSSQLCondition<CASE_NOTICE>(x => x.CASE_ID == item.CaseID && x.CASE_NOTICE_TYPE == (byte)CaseNoticeType.CaseApply);
                    var isExist = _CaseAggregate.CaseNotice_T1_T2_.HasAny(caseNoticeCon);

                    if (isExist)
                        _CaseAggregate.CaseNotice_T1_T2_.Remove(caseNoticeCon);

                    var caseNotice = new CaseNotice()
                    {
                        CaseID = item.CaseID,
                        ApplyUserID = user.UserID,
                        CreateDateTime = DateTime.Now,
                        CreateUserName = ContextUtility.GetUserIdentity()?.Name,
                        CaseNoticeType = CaseNoticeType.CaseApply
                    };

                    _CaseAggregate.CaseNotice_T1_T2_.Add(caseNotice);
                    #endregion
                }

                #region 新增個人通知

                _NotificationAggregate.PersonalNotification_T1_T2_.Add(personalNotification);

                _NotificationPersonalFacade.NotifyWeb(user.UserID);

                #endregion



                scope.Complete();
            }
        }
    }
}
