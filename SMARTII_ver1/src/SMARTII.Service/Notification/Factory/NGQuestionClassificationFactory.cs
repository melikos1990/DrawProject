using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Newtonsoft.Json;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Common;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Organization;
using SMARTII.Resource.Tag;

namespace SMARTII.Service.Notification.Factory
{
    public class NGQuestionClassificationFactory : NotificationGroupBaseFactory, INotificationGroupFactory
    {
        private readonly ICommonAggregate _CommonAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly INotificationAggregate _NotificationAggregate;
        private readonly INotificationPersonalFacade _NotificationPersonalFacade;

        public NGQuestionClassificationFactory(ICommonAggregate CommonAggregate,
                                               IOrganizationAggregate OrganizationAggregate,
                                               INotificationAggregate NotificationAggregate,
                                               INotificationPersonalFacade NotificationPersonalFacade)
        {
            _CommonAggregate = CommonAggregate;
            _OrganizationAggregate = OrganizationAggregate;
            _NotificationAggregate = NotificationAggregate;
            _NotificationPersonalFacade = NotificationPersonalFacade;
        }

        public void Execute(IEnumerable<Domain.Case.Case> cases, NotificationGroup group)
        {
            _CommonAggregate.Logger.Info($"【提醒群組通知】 《問題分類行為》");
            _CommonAggregate.Logger.Info($"【提醒群組通知】  計算是否達標");

            // 此次達標的總案件
            var allActualCases = cases.Where(x => x.QuestionClassificationID == group.QuestionClassificationID)
                                   .ToList();

            // 撈取已計算過的案件
            var existSummary = _NotificationAggregate.NotificationGroupEffectiveSummary_T1_T2_.GetList(x => x.NOTIFICATION_GROUP_ID == group.ID && x.IS_PROCESS == true).ToList();
            var existCases = existSummary.SelectMany(x => x.CaseIDs).Distinct().ToList();

            // 過濾已計算過的案件
            var actualCases = allActualCases.Where(x => !existCases.Contains(x.CaseID)).ToList();

            bool isArrive = (actualCases.Count() >= group.AlertCount);

            _CommonAggregate.Logger.Info($"【提醒群組通知】  計算完畢 , 實際達標數 : {actualCases.Count()} , 預計達標數 : {group.AlertCount} , 是否達標 : {isArrive}");

            if (!isArrive)
            {
                // 取消原達標標的
                cancelGroup(group);
                return;
            }

            // 組合彙總歷程
            var summary = base.GenerateSummary(group, actualCases);

            // 更新案件群組(達標數/數量)
            var con = base.GenerateCondition(group, actualCases, isArrive);

            _CommonAggregate.Logger.Info($"【提醒群組通知】  準備寫入對應資料");

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                if (group.IsArrive)
                {
                    // 原已達標，更新原達標資料
                    updateEffectiveTarget(group, actualCases);
                }
                else
                {
                    // 原無達標，新增達標資料
                    _NotificationAggregate.NotificationGroupEffectiveSummary_T1_T2_.Add(summary);
                }

                // 沒通知過且達標 , 才進行通知
                if (group.IsNotificated == false && isArrive)
                {
                    // 組合通知物件
                    var questionClassificationParentNames = group.QuestionClassificationParentNames.Replace("@", "-");

                    var personalNotifications = GeneratePersonalNotifications(group, actualCases, questionClassificationParentNames);

                    _NotificationAggregate.PersonalNotification_T1_T2_.AddRange(personalNotifications);
                    

                    _NotificationPersonalFacade.NotifyWebCollection(base.GetUserIDs(group));
                }

                _NotificationAggregate.NotificationGroup_T1_T2_.Update(con);

                scope.Complete();
            }

            _CommonAggregate.Logger.Info($"【提醒群組通知】  寫入完畢");
        }

        /// <summary>
        /// 取消原達標標的
        /// </summary>
        /// <param name="group"></param>
        private void cancelGroup(NotificationGroup group)
        {
            var summaries = _NotificationAggregate.NotificationGroupEffectiveSummary_T1_T2_.GetList(x => x.NOTIFICATION_GROUP_ID == group.ID && x.IS_PROCESS == false).ToList();

            summaries.ForEach(x =>
            {
                // 移除已達標標的
                _NotificationAggregate.NotificationGroupEffectiveSummary_T1_T2_.Remove(y => y.ID == x.ID);
            });

            // 修改標的狀態
            var con = new MSSQLCondition<NOTIFICATION_GROUP>(x => x.ID == group.ID);
            con.ActionModify(x =>
            {
                x.ACTUAL_COUNT = 0;
                x.IS_NOTIFICATED = false;
                x.IS_ARRIVE = false;
                x.UPDATE_DATETIME = DateTime.Now;
                x.UPDATE_USERNAME = GlobalizationCache.APName;
            });

            _NotificationAggregate.NotificationGroup_T1_T2_.Update(con);
        }

        /// <summary>
        /// 更新未通知的達標標的
        /// </summary>
        /// <param name="group"></param>
        /// <param name="actualCases"></param>
        private void updateEffectiveTarget(NotificationGroup group, List<Domain.Case.Case> actualCases)
        {
            var con = new MSSQLCondition<NOTIFICATION_GROUP_EFFECTIVE_SUMMARY>(x => x.NOTIFICATION_GROUP_ID == group.ID && x.IS_PROCESS == false);
            con.ActionModify(x =>
            {
                x.ACTUAL_ARRIVE_COUNT = actualCases.Count();
                x.EXPECT_ARRIVE_COUNT = group.AlertCount;
                x.CASE_IDs = JsonConvert.SerializeObject(actualCases.Select(y => y.CaseID)?.ToArray());
                x.UPDATE_DATETIME = DateTime.Now;
                x.UPDATE_USERNAME = GlobalizationCache.APName;
            });

            _NotificationAggregate.NotificationGroupEffectiveSummary_T1_T2_.Update(con);
        }


        /// <summary>
        /// 組合通知物件
        /// </summary>
        /// <param name="group"></param>
        /// <param name="actualCases"></param>
        /// <param name="targetName"></param>
        /// <returns></returns>
        private List<PersonalNotification> GeneratePersonalNotifications(NotificationGroup group, List<Domain.Case.Case> actualCases, string targetName)
        {
            var personalNotificationList = new List<PersonalNotification>();

            foreach (var actualCase in actualCases)
            {
                // 找出案件所屬group
                var groupNode = _OrganizationAggregate.CallCenterNode_T1_T2_.Get(x => x.NODE_ID == actualCase.GroupID);

                if (groupNode == null)
                    throw new Exception(Common_lang.GROUP_NOT_FOUND);

                switch (groupNode.WorkProcessType)
                {
                    case WorkProcessType.Individual:
                        var personalNotificationIndividual = new PersonalNotification()
                        {
                            UserID = actualCase.ApplyUserID,
                            Content = string.Format(NotificationFormatted.NotificationGroup, group.NodeName, targetName, group.Name),
                            CreateDateTime = DateTime.Now,
                            CreateUserName = GlobalizationCache.APName,
                            PersonalNotificationType = PersonalNotificationType.NotificationGroup,
                        };

                        personalNotificationList.Add(personalNotificationIndividual);

                        break;

                    case WorkProcessType.Accompanied:

                        var jCon = new MSSQLCondition<NODE_JOB>(x => x.NODE_ID == actualCase.GroupID && x.ORGANIZATION_TYPE == (byte)OrganizationType.CallCenter);
                        jCon.IncludeBy(x => x.USER);
                        jCon.IncludeBy(x => x.JOB);
                        var jobsUsers = _OrganizationAggregate.JobPosition_T1_T2_.GetList(jCon).SelectMany(x => x.Users).Select(x => x.UserID).Distinct().ToList();

                        jobsUsers.ForEach(x =>
                        {
                            var personalNotificationAccompanied = new PersonalNotification()
                            {
                                UserID = x,
                                Content = string.Format(NotificationFormatted.NotificationGroup, group.NodeName, targetName, group.Name),
                                CreateDateTime = DateTime.Now,
                                CreateUserName = GlobalizationCache.APName,
                                PersonalNotificationType = PersonalNotificationType.NotificationGroup,
                            };

                            personalNotificationList.Add(personalNotificationAccompanied);
                        });

                        break;
                }
            }
            //過濾掉重複的人
            personalNotificationList = personalNotificationList.GroupBy(x => x.UserID).Select(x => x.First()).ToList();

            return personalNotificationList;
        }
    }
}
