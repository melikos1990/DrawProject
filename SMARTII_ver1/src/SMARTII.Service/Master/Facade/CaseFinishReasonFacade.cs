using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Case;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;

namespace SMARTII.Service.Master.Facade
{
    public class CaseFinishReasonFacade : ICaseFinishReasonFacade
    {
        private readonly IMasterAggregate _MasterAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;

        public CaseFinishReasonFacade(IMasterAggregate MasterAggregate,
                                      IOrganizationAggregate OrganizationAggregate)
        {
            _MasterAggregate = MasterAggregate;
            _OrganizationAggregate = OrganizationAggregate;
        }

        /// <summary>
        /// 單一新增明細
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public async Task Create(CaseFinishReasonData data)
        {
            #region 驗證名稱

            var isExistName = _MasterAggregate.CaseFinishReasonData_T1_
                                                 .HasAny(x => x.CLASSIFICATION_ID == data.ClassificationID &&
                                                              x.TEXT == data.Text);
            if (isExistName)
                throw new Exception(CaseFinishReason_lang.CASEFINISHREASON_DUPLICATE_TEXT);

            #endregion

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                UpdateWhenExistDefault(data);

                data.CreateDateTime = DateTime.Now;
                data.CreateUserName = ContextUtility.GetUserIdentity()?.Name;

                _MasterAggregate.CaseFinishReasonData_T1_T2_.Add(data);

                scope.Complete();

            }

        }

        /// <summary>
        /// 更新明細
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public async Task Update(CaseFinishReasonData data)
        {

            #region 驗證名稱

            string Text = data.Text.Trim();


            var isExistName = _MasterAggregate.CaseFinishReasonData_T1_T2_
                                       .HasAny(x => x.ID != data.ID &&
                                                 x.CLASSIFICATION_ID == data.ClassificationID &&
                                                 x.TEXT == Text);

            if (isExistName)
                throw new Exception(CaseFinishReason_lang.CASEFINISHREASON_DUPLICATE_TEXT);

            #endregion

            var con = new MSSQLCondition<CASE_FINISH_REASON_DATA>(x => x.ID == data.ID);

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                UpdateWhenExistDefault(data);

                con.ActionModify(x =>
                {
                    x.TEXT = data.Text;
                    x.IS_ENABLED = data.IsEnabled;
                    x.DEFAULT = data.Default;
                    x.UPDATE_DATETIME = DateTime.Now;
                    x.UPDATE_USERNAME = ContextUtility.GetUserIdentity()?.Name;
                });

                _MasterAggregate.CaseFinishReasonData_T1_T2_.Update(con);

                scope.Complete();

            }

        }

        /// <summary>
        /// 新增分類
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task CreateClassification(CaseFinishReasonClassification data)
        {
            #region 驗證名稱/既有單位
            var isExitHeadquarters = _OrganizationAggregate.HeaderQuarterNode_T1_
                                                 .HasAny(x => x.BU_ID == data.NodeID);
            if (!isExitHeadquarters)
                throw new Exception(CaseFinishReason_lang.CASEFINISHREASON_BU_FAIL);

            var isExistName = _MasterAggregate.CaseFinishReasonClassification_T1_
                                                 .HasAny(x => x.NODE_ID == data.NodeID &&
                                                              x.TITLE == data.Title);
            if (isExistName)
                throw new Exception(CaseFinishReason_lang.CASEFINISHREASON_DUPLICATE_TITLE);
            #endregion

            data.CreateDateTime = DateTime.Now;
            data.CreateUserName = ContextUtility.GetUserIdentity()?.Name;

            var result = _MasterAggregate.CaseFinishReasonClassification_T1_T2_.Add(data);

            await result.Async();
        }

        /// <summary>
        /// 更新分類
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task UpdateClassification(CaseFinishReasonClassification data)
        {

            #region 驗證標題/既有單位
            var isExitHeadquarters = _OrganizationAggregate.HeaderQuarterNode_T1_
                                                 .HasAny(x => x.BU_ID == data.NodeID);
            if (!isExitHeadquarters)
                throw new Exception(CaseFinishReason_lang.CASEFINISHREASON_BU_FAIL);

            string Title = data.Title.Trim();

            var existName = _MasterAggregate.CaseFinishReasonClassification_T1_T2_
                                       .HasAny(x => x.ID != data.ID &&
                                                 x.NODE_ID == data.NodeID &&
                                                 x.TITLE == Title);

            if (existName)
                throw new Exception(CaseFinishReason_lang.CASEFINISHREASON_DUPLICATE_TITLE);

            #endregion


            var con = new MSSQLCondition<CASE_FINISH_REASON_CLASSIFICATION>(x => x.ID == data.ID);

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                // 如果是由多選 , 變為單選 , 清空細項的預設選項
                UpdateWhenMultipleToSingle(data);

                con.ActionModify(x =>
                {
                    x.TITLE = data.Title;
                    x.IS_ENABLED = data.IsEnabled;
                    x.IS_MULTIPLE = data.IsMultiple;
                    x.IS_REQUIRED = data.IsRequired;
                    x.UPDATE_DATETIME = DateTime.Now;
                    x.UPDATE_USERNAME = ContextUtility.GetUserIdentity()?.Name;
                });

                _MasterAggregate.CaseFinishReasonClassification_T1_T2_.Update(con);

                scope.Complete();

            }

        }

        public async Task<(bool, CaseFinishReasonData)> CheckExistDefault(int ID)
        {
            var con = new MSSQLCondition<CASE_FINISH_REASON_CLASSIFICATION>();
            con.IncludeBy(x => x.CASE_FINISH_REASON_DATA);
            con.And(x => x.ID == ID);

            var data = _MasterAggregate.CaseFinishReasonClassification_T1_T2_.Get(con);


            if (data.IsMultiple == false)
            {
                var target = data.CaseFinishReasonDatas
                                 .FirstOrDefault(x => x.Default == true);

                return await (target != null, target).Async();
            }

            return default((bool, CaseFinishReasonData));
        }

        /// <summary>
        /// 更新同分類中的其他細項
        /// </summary>
        /// <param name="data"></param>
        private void UpdateWhenExistDefault(CaseFinishReasonData data)
        {

            var con = new MSSQLCondition<CASE_FINISH_REASON_DATA>(x => x.CLASSIFICATION_ID == data.ClassificationID);

            con.IncludeBy(x => x.CASE_FINISH_REASON_CLASSIFICATION);

            // 如果傳入項目為預設勾選的
            if (data.Default)
            {

                var exists = _MasterAggregate.CaseFinishReasonData_T1_T2_.GetList(con);

                var target = exists.FirstOrDefault();

                if (target == null) return;

                var existClassification = target.CaseFinishReasonClassification;

                // 如果為單選 , 其他的細項需改成 "不預設"
                if (existClassification.IsMultiple == false)
                {

                    con.ActionModify(x => x.DEFAULT = false);
                    _MasterAggregate.CaseFinishReasonData_T1_T2_.UpdateRange(con);

                }

            }
        }
        /// <summary>
        /// 在多選變為單選時 , 需註銷其他細項的預設項目
        /// </summary>
        /// <param name="data"></param>
        private void UpdateWhenMultipleToSingle(CaseFinishReasonClassification data)
        {
            var exist = _MasterAggregate.CaseFinishReasonClassification_T1_T2_
                                        .Get(x => x.ID == data.ID);

            // 由多選 改為單選
            if (exist.IsMultiple == true && data.IsMultiple == false)
            {
                var con = new MSSQLCondition<CASE_FINISH_REASON_DATA>(x => x.CLASSIFICATION_ID == data.ID);
                con.ActionModify(x => x.DEFAULT = false);
                _MasterAggregate.CaseFinishReasonData_T1_T2_.UpdateRange(con);

            }


        }

    }
}
