using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Case;
using SMARTII.Domain.Master;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;

namespace SMARTII.Service.Notification.Facade
{
    public class CaseAssignGroupFacade : ICaseAssignGroupFacade
    {
        private readonly IMasterAggregate _MasterAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly ICaseAggregate _CaseAggregate;

        public CaseAssignGroupFacade(IMasterAggregate MasterAggregate,
                                       IOrganizationAggregate OrganizationAggregate,
                                       ICaseAggregate CaseAggregate)
        {
            _MasterAggregate = MasterAggregate;
            _OrganizationAggregate = OrganizationAggregate;
            _CaseAggregate = CaseAggregate;
        }

        /// <summary>
        /// 更新-派工群組設定
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public async Task Update(CaseAssignGroup group)
        {
            #region 驗證名稱/既有單位
            var isExitHeadquarters = _OrganizationAggregate.HeaderQuarterNode_T1_
                                                 .HasAny(x => x.BU_ID == group.NodeID);
            if (!isExitHeadquarters)
                throw new Exception(CaseAssignGroup_lang.CASE_ASSIGN_GROUP_BU_FAIL);


            var isExistName = _CaseAggregate.CaseAssignmentGroup_T1_
                                                    .HasAny(x => x.ID != group.ID &&
                                                                 x.NODE_ID == group.NodeID &&
                                                                 x.NAME == group.Name &&
                                                                 x.TYPE == (byte)group.CaseAssignGroupType);

            if (isExistName)
                throw new Exception(CaseAssignGroup_lang.CASE_ASSIGN_GROUP_DUPLICATE_NAME);
            #endregion

            // 驗證人員資訊
            ValidUsers(group.CaseAssignGroupUsers);

            // 組合 con 物件
            var con = new MSSQLCondition<CASE_ASSIGN_GROUP>(x => x.ID == group.ID);

            con.ActionModify(x =>
            {
                x.NAME = group.Name;
                x.NODE_ID = group.NodeID;
                x.TYPE = (byte)group.CaseAssignGroupType;
                x.UPDATE_DATETIME = DateTime.Now;
                x.UPDATE_USERNAME = ContextUtility.GetUserIdentity()?.Name;
            });
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                _CaseAggregate.CaseAssignmentGroup_T1_T2_.Update(con);

                _CaseAggregate.CaseAssignmentGroupUser_T1_T2_.RemoveRange(x => x.GROUP_ID == group.ID);

                _CaseAggregate.CaseAssignmentGroupUser_T1_T2_.AddRange(group.CaseAssignGroupUsers);

                scope.Complete();
            }
        }

        /// <summary>
        /// 新增-派工群組設定
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public async Task Create(CaseAssignGroup group)
        {
            #region 驗證名稱/既有單位
            var isExitHeadquarters = _OrganizationAggregate.HeaderQuarterNode_T1_
                                                 .HasAny(x => x.BU_ID == group.NodeID);
            if (!isExitHeadquarters)
                throw new Exception(CaseAssignGroup_lang.CASE_ASSIGN_GROUP_BU_FAIL);

            var isExistName = _CaseAggregate.CaseAssignmentGroup_T1_
                                                   .HasAny(x => x.NODE_ID == group.NodeID &&
                                                                x.NAME == group.Name &&
                                                                x.TYPE == (byte)group.CaseAssignGroupType);
            if (isExistName)
                throw new Exception(CaseAssignGroup_lang.CASE_ASSIGN_GROUP_DUPLICATE_NAME);
            #endregion

            // 驗證人員資訊
            ValidUsers(group.CaseAssignGroupUsers);

            group.CreateDateTime = DateTime.Now;
            group.CreateUserName = ContextUtility.GetUserIdentity()?.Name;
            var result = _CaseAggregate.CaseAssignmentGroup_T1_T2_.Add(group);
            await result.Async();
        }

        /// <summary>
        /// 驗證人員是否啟用
        /// </summary>
        /// <param name="users"></param>
        private void ValidUsers(List<CaseAssignGroupUser> users)
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
