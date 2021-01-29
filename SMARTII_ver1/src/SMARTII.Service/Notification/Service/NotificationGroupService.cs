using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Transactions;
using Autofac.Features.Indexed;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Case;
using SMARTII.Domain.Common;
using SMARTII.Domain.DI;
using SMARTII.Domain.IO;
using SMARTII.Domain.Master;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Notification.Email;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Service.Master.Resolver;
using SMARTII.Service.Organization.Resolver;

namespace SMARTII.Service.Notification.Service
{
    public class NotificationGroupService : INotificationGroupService
    {
        private readonly ICaseAggregate _CaseAggregate;
        private readonly IMasterAggregate _MasterAggregate;
        private readonly ICommonAggregate _CommonAggregate;
        private readonly INotificationAggregate _NotificationAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly IIndex<string, INotificationGroupFactory> _NotificationFactories;
        private readonly IIndex<NotificationType, INotificationProvider> _NotificationProviders;
        private readonly INotificationGroupFacade _NotificationGroupFacade;
        private readonly ItemResolver _ItemResolver;
        private readonly OrganizationNodeResolver _OrganizationNodeResolver;
        private readonly QuestionClassificationResolver _QuestionClassificationResolver;

        public NotificationGroupService(ICaseAggregate CaseAggregate,
                                        IMasterAggregate MasterAggregate,
                                        ICommonAggregate CommonAggregate,
                                        INotificationAggregate NotificationAggregate,
                                        IOrganizationAggregate OrganizationAggregate,
                                        INotificationGroupFacade NotificationGroupFacade,
                                        ItemResolver ItemResolver,
                                        OrganizationNodeResolver OrganizationNodeResolver,
                                        QuestionClassificationResolver QuestionClassificationResolver,
                                        IIndex<string, INotificationGroupFactory> NotificationFactories,
                                        IIndex<NotificationType, INotificationProvider> NotificationProviders)
        {
            _ItemResolver = ItemResolver;
            _CaseAggregate = CaseAggregate;
            _CommonAggregate = CommonAggregate;
            _MasterAggregate = MasterAggregate;
            _NotificationAggregate = NotificationAggregate;
            _OrganizationAggregate = OrganizationAggregate;
            _NotificationFactories = NotificationFactories;
            _NotificationProviders = NotificationProviders;
            _NotificationGroupFacade = NotificationGroupFacade;
            _OrganizationNodeResolver = OrganizationNodeResolver;
            _QuestionClassificationResolver = QuestionClassificationResolver;
        }



        #region Batch

        /// <summary>
        /// 排程計算大量叫修數量
        /// </summary>
        public void Calculate()
        {
            try
            {
                var now = DateTime.Now;

                _CommonAggregate.Logger.Info($"【提醒群組通知】  準備進行排程 , 時間 : {now.ToString()}。");

                // 撈取規則清單
                var con = new MSSQLCondition<NOTIFICATION_GROUP>();
                con.IncludeBy(x => x.NOTIFICATION_GROUP_USER);

                var seed = _NotificationAggregate.NotificationGroup_T1_T2_.GetList(con);

                if (seed == null || seed.Count() == 0)
                {
                    _CommonAggregate.Logger.Info($"【提醒群組通知】  並未撈出通知規則 , 故不往下執行。");
                    return;
                }

                _CommonAggregate.Logger.Info($"【提醒群組通知】  撈出規則共 {seed.Count()} 筆。");
                _CommonAggregate.Logger.Info($"【提醒群組通知】  準備進行資料回填。");

                var groups =
                    _QuestionClassificationResolver.ResolveNullableCollection
                    (
                       _ItemResolver.ResolveNullableCollection
                       (
                            _OrganizationNodeResolver.ResolveCollection(seed)
                       )
                    );

                _CommonAggregate.Logger.Info($"【提醒群組通知】  準備進行資料完畢 , 準備計算規則。");

                // 找到最大天數
                var maxDateRange = groups.Max(x => x.AlertCycleDay);
                _CommonAggregate.Logger.Info($"【提醒群組通知】  找到最大天數 : {maxDateRange}。");

                // 找到計算起始日
                var start = now.AddDays(-maxDateRange);

                _CommonAggregate.Logger.Info($"【提醒群組通知】  案件撈取起始時間 : {start.ToString()}。");
                _CommonAggregate.Logger.Info($"【提醒群組通知】  案件撈取結束時間 : {now.ToString()}。");
                _CommonAggregate.Logger.Info($"【提醒群組通知】  準備撈取案件。");

                // 找到案件清單
                var cCon = new MSSQLCondition<CASE>(x => x.CREATE_DATETIME >= start &&
                                                         x.CREATE_DATETIME < now);

                cCon.IncludeBy(x => x.CASE_ITEM);
                //cCon.IncludeBy(x => x.QUESTION_CLASSIFICATION);

                var cases = _CaseAggregate.Case_T1_T2_.GetList(cCon);

                _CommonAggregate.Logger.Info($"【提醒群組通知】  撈取案件完畢 , 共 {cases.Count()} 筆。");
                _CommonAggregate.Logger.Info($"【提醒群組通知】  撈取案件完畢 , 準備進行資料的寫入。");

                // 迭代服務清單
                foreach (var group in groups)
                {
                    try
                    {
                        // 找到該群計算起始日
                        var startGroup = now.AddDays(-group.AlertCycleDay);

                        var gruopCases = cases.Where(x => x.CreateDateTime >= startGroup &&
                                                          x.CreateDateTime < now).ToList();

                        DoCalculate(group, gruopCases);
                    }
                    catch (Exception ex)
                    {
                        _CommonAggregate.Logger.Error(ex.Message);
                        _CommonAggregate.Loggers["Email"].Error($"【提醒群組通知】失敗 , 群組代號 : {group.ID} ,群組名稱 : {group.Name}, 原因 : {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex.Message);
                _CommonAggregate.Loggers["Email"].Error($"【提醒群組通知】整批失敗, 原因 : {ex.Message}");
            }
        }

        /// <summary>
        /// 進行計算
        /// </summary>
        /// <param name="group"></param>
        /// <param name="cases"></param>
        private void DoCalculate(NotificationGroup group, List<Domain.Case.Case> cases)
        {
            var calcMode = group.CalcMode;
            string key = calcMode.ToString();

            var service = _NotificationFactories.TryGetService(key, EssentialCache.BusinessKeyValue.COMMONBU);

            service.Execute(cases, group);
        }




        #endregion

        /// <summary>
        /// 不進行通知
        /// </summary>
        public void NoSend(int groupID)
        {
            var group = _NotificationAggregate.NotificationGroup_T1_T2_.Get(x => x.ID == groupID);

            var seed =
                _QuestionClassificationResolver.ResolveNullable
                (
                   _ItemResolver.ResolveNullable
                   (
                        _OrganizationNodeResolver.Resolve(group)
                   )
                );

            try
            {
                // 將規則內的數字清空
                var con = new MSSQLCondition<NOTIFICATION_GROUP>(x => x.ID == groupID);
                con.ActionModify(x =>
                {
                    x.IS_ARRIVE = false;
                    x.IS_NOTIFICATED = false;
                    x.ACTUAL_COUNT = 0;
                    x.UPDATE_DATETIME = DateTime.Now;
                    x.UPDATE_USERNAME = ContextUtility.GetUserIdentity()?.Name;
                });

                // 將該筆達標通知押上已處理
                var effective = new MSSQLCondition<NOTIFICATION_GROUP_EFFECTIVE_SUMMARY>(x => x.NOTIFICATION_GROUP_ID == groupID && x.IS_PROCESS == false);
                effective.ActionModify(x =>
                {
                    x.IS_PROCESS = true;
                    x.UPDATE_DATETIME = DateTime.Now;
                    x.UPDATE_USERNAME = ContextUtility.GetUserIdentity()?.Name;
                });

                // 寫入推送歷程
                var resume = new NotificationGroupResume()
                {
                    Content = string.Empty,
                    CreateDateTime = DateTime.Now,
                    CreateUserName = ContextUtility.GetUserIdentity()?.Name,
                    NodeID = seed.NodeID,
                    NodeName = seed.NodeName,
                    Target = seed.TargetNames,
                    GroupID = seed.ID,
                    NotificationGroupResultType = NotificationGroupResultType.NoSend
                };

                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    _NotificationAggregate.NotificationGroup_T1_T2_.Update(con);

                    _NotificationAggregate.NotificationGroupEffectiveSummary_T1_T2_.Update(effective);

                    _NotificationAggregate.NotificationGroupResume_T1_T2_.Add(resume);

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                // 嘗試寫入推送歷程
                var resume = new NotificationGroupResume()
                {
                    Content = ex.Message,
                    CreateDateTime = DateTime.Now,
                    CreateUserName = ContextUtility.GetUserIdentity()?.Name,
                    NodeID = seed.NodeID,
                    NodeName = seed.NodeName,
                    Target = seed.TargetNames,
                    GroupID = seed.ID,
                    NotificationGroupResultType = NotificationGroupResultType.Error
                };

                _NotificationAggregate.NotificationGroupResume_T1_T2_.Add(resume);

                throw ex;
            }
        }

        /// <summary>
        /// 進行通知
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="payload"></param>
        public void Send(int groupID, EmailPayload payload)
        {
            var group = _NotificationAggregate.NotificationGroup_T1_T2_.Get(x => x.ID == groupID);

            var seed =
                _QuestionClassificationResolver.ResolveNullable
                (
                   _ItemResolver.ResolveNullable
                   (
                        _OrganizationNodeResolver.Resolve(group)
                   )
                );

            try
            {
                // 將規則內的數字清空
                var con = new MSSQLCondition<NOTIFICATION_GROUP>(x => x.ID == groupID);

                con.ActionModify(x =>
                {
                    x.IS_ARRIVE = false;
                    x.IS_NOTIFICATED = false;
                    x.ACTUAL_COUNT = 0;
                    x.UPDATE_DATETIME = DateTime.Now;
                    x.UPDATE_USERNAME = ContextUtility.GetUserIdentity()?.Name;
                });

                // 將該筆達標通知押上已處理
                var effective = new MSSQLCondition<NOTIFICATION_GROUP_EFFECTIVE_SUMMARY>(x => x.NOTIFICATION_GROUP_ID == groupID && x.IS_PROCESS == false);
                effective.ActionModify(x =>
                {
                    x.IS_PROCESS = true;
                    x.UPDATE_DATETIME = DateTime.Now;
                    x.UPDATE_USERNAME = ContextUtility.GetUserIdentity()?.Name;
                });

                // 寫入推送歷程
                var resume = new NotificationGroupResume()
                {
                    Content = payload.Content,
                    CreateDateTime = DateTime.Now,
                    CreateUserName = ContextUtility.GetUserIdentity()?.Name,
                    NodeID = seed.NodeID,
                    NodeName = seed.NodeName,
                    Target = seed.TargetNames,
                    GroupID = seed.ID,
                    NotificationGroupResultType = NotificationGroupResultType.Finish
                };

                new FileProcessInvoker((context) =>
                {
                    //// 更新人員
                    //var users = AutoMapper.Mapper.Map<List<NOTIFICATION_GROUP_USER>>(payload.ConcatableUsers);
                    //users.ForEach(x => x.GROUP_ID = groupID);

                    using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        // 進行發信
                        _NotificationProviders[NotificationType.Email].Send(
                        payload: payload,
                        afterSend: AfterSenderHanlder(resume, context));

                        _NotificationAggregate.NotificationGroupResume_T1_T2_.Add(resume);

                        _NotificationAggregate.NotificationGroupEffectiveSummary_T1_T2_.Update(effective);

                        _NotificationAggregate.NotificationGroup_T1_T2_.Update(con);

                        //_NotificationAggregate.NotificationGroupUser_T1_T2_.RemoveRange(x => x.GROUP_ID == groupID);

                        //_NotificationAggregate.NotificationGroupUser_T1_.AddRange(users);

                        scope.Complete();
                    }
                });

            }
            catch (Exception ex)
            {
                // 嘗試寫入推送歷程
                var resume = new NotificationGroupResume()
                {
                    Content = ex.Message,
                    CreateDateTime = DateTime.Now,
                    CreateUserName = ContextUtility.GetUserIdentity()?.Name,
                    NodeID = seed.NodeID,
                    NodeName = seed.NodeName,
                    Target = seed.TargetNames,
                    GroupID = seed.ID,
                    NotificationGroupResultType = NotificationGroupResultType.Error
                };

                _NotificationAggregate.NotificationGroupResume_T1_T2_.Add(resume);

                throw ex;
            }
        }


        private Action<Object> AfterSenderHanlder(NotificationGroupResume resume, FileProcessContext context)
        {
            return (object obj) =>
            {

                var fileName = $"{Guid.NewGuid().ToString()}.eml";

                var emailBytes = (obj as byte[]);

                var physicalDirPath = FilePathFormatted.GetEmailSenderPhysicalDirPath();

                var virtualPath = FilePathFormatted.GetEmailSenderVirtualFilePath(fileName);

                var path = emailBytes.SaveAsFilePath(physicalDirPath, fileName);

                resume.EMLFilePath = virtualPath;

                context.Paths.Add(path);

            };
        }

    }
}
