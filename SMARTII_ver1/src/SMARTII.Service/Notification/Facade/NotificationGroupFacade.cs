using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Case;
using SMARTII.Domain.Common;
using SMARTII.Domain.Master;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;

namespace SMARTII.Service.Notification.Facade
{
    public class NotificationGroupFacade : INotificationGroupFacade
    {
        private readonly IMasterAggregate _MasterAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly INotificationAggregate _NotificationAggregate;
        private readonly ICommonAggregate _CommonAggregate;

        public NotificationGroupFacade(IMasterAggregate MasterAggregate,
                                       IOrganizationAggregate OrganizationAggregate,
                                       INotificationAggregate NotificationAggregate,
                                       ICommonAggregate CommonAggregate)
        {
            _MasterAggregate = MasterAggregate;
            _OrganizationAggregate = OrganizationAggregate;
            _NotificationAggregate = NotificationAggregate;
            _CommonAggregate = CommonAggregate;
        }

        public async Task Update(NotificationGroup group)
        {
            var isExistName = _NotificationAggregate.NotificationGroup_T1_
                                                    .HasAny(x => x.ID != group.ID &&
                                                                 x.NODE_ID == group.NodeID &&
                                                                 x.NAME == group.Name);

            if (isExistName)
                throw new Exception(NotificationGroup_lang.NOTIFICATION_GROUP_DUPLICATE_NAME);

            // 驗證商品資訊
            ValidItem(group.ItemID);
            // 驗證問題分類資訊
            ValidQuestionClassification(group.QuestionClassificationID);
            // 驗證人員資訊
            ValidUsers(group.NotificationGroupUsers);

            // 組合 con 物件
            var con = new MSSQLCondition<NOTIFICATION_GROUP>(x => x.ID == group.ID);

            con.ActionModify(x =>
            {
                x.UPDATE_DATETIME = DateTime.Now;
                x.UPDATE_USERNAME = ContextUtility.GetUserIdentity()?.Name;
                x.NAME = group.Name;
                x.ALERT_COUNT = group.AlertCount;
                x.ALERT_CYCLE_DAY = group.AlertCycleDay;
            });

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                _NotificationAggregate.NotificationGroup_T1_T2_.Update(con);

                _NotificationAggregate.NotificationGroupUser_T1_T2_.RemoveRange(x => x.GROUP_ID == group.ID);

                _NotificationAggregate.NotificationGroupUser_T1_T2_.AddRange(group.NotificationGroupUsers);
                scope.Complete();
            }
        }

        public async Task Create(NotificationGroup group)
        {
            var isExistName = _NotificationAggregate.NotificationGroup_T1_
                                                   .HasAny(x => x.NODE_ID == group.NodeID &&
                                                                x.NAME == group.Name);

            if (isExistName)
                throw new Exception(NotificationGroup_lang.NOTIFICATION_GROUP_DUPLICATE_NAME);

            // 驗證商品資訊
            ValidItem(group.ItemID);
            // 驗證問題分類資訊
            ValidQuestionClassification(group.QuestionClassificationID);
            // 驗證人員資訊
            ValidUsers(group.NotificationGroupUsers);

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                group.CreateDateTime = DateTime.Now;
                group.CreateUserName = ContextUtility.GetUserIdentity()?.Name;

                var newGroup = _NotificationAggregate.NotificationGroup_T1_T2_.Add(group);

                scope.Complete();
            }
        }

        private void ValidItem(int? itemID)
        {
            if (itemID.HasValue)
            {
                var item = _MasterAggregate.Item_T1_T2_.Get(x => x.ID == itemID);

                if (item == null)
                    throw new Exception(Common_lang.NOT_FOUND_DATA);

                if (item.IsEnabled == false)
                    throw new Exception(Common_lang.ITEM_NOT_ENABLE);
            }
        }

        private void ValidQuestionClassification(int? quesionClassificationID)
        {
            if (quesionClassificationID.HasValue)
            {
                var questionClassification = _MasterAggregate.VWQuestionClassification_QuestionClassification_.Get(x => x.ID == quesionClassificationID);

                if (questionClassification == null)
                    throw new Exception(Common_lang.NOT_FOUND_DATA);

                if (questionClassification.IsEnabled == false)
                    throw new Exception(Common_lang.QUESTION_CLASSIFICATION_NOT_ENABLE);
            }
        }

        private void ValidUsers(List<NotificationGroupUser> users)
        {
            if (users != null && users.Count() > 0)
            {
                var userIDs = users.Select(x => x.UserID)
                                   .Where(x => string.IsNullOrEmpty(x))
                                   .ToArray();

                var existUsers = _OrganizationAggregate.User_T1_T2_.GetList(x => userIDs.Contains(x.USER_ID));

                if (!existUsers.All(UserUtility.ValidExpression()))
                    throw new Exception(Common_lang.USER_UNACTIVE);
            }
        }
    }
}
