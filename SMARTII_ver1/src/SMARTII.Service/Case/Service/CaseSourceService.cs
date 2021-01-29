using System;
using System.Linq;
using System.Reflection;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Case;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;
using SMARTII.Domain.System;
using SMARTII.Domain.Thread;
using SMARTII.Domain.Transaction;
using SMARTII.Resource.Tag;

namespace SMARTII.Service.Case.Service
{

    public class CaseSourceService : ICaseSourceService
    {
        private readonly ICaseAggregate _CaseAggregate;
        private readonly ICommonAggregate _CommonAggregate;
        private readonly ISystemAggregate _SystemAggregate;
        private readonly ICaseSourceFacade _CaseSourceFacade;
        private readonly IOrganizationAggregate _OrganizationAggregate;

        public CaseSourceService(ICaseAggregate CaseAggregate,
                                 ICommonAggregate CommonAggregate,
                                 ISystemAggregate SystemAggregate,
                                 ICaseSourceFacade CaseSourceFacade,
                                 IOrganizationAggregate OrganizationAggregate)
        {
            _CaseAggregate = CaseAggregate;
            _CommonAggregate = CommonAggregate;
            _SystemAggregate = SystemAggregate;
            _CaseSourceFacade = CaseSourceFacade;
            _OrganizationAggregate = OrganizationAggregate;
        }

        /// <summary>
        /// 更新案件來源
        /// </summary>
        /// <param name="caseSource"></param>
        /// <returns></returns>
        public CaseSource UpdateComplete(CaseSource caseSource)
        {

            CaseSource result = null;

            // 填充待更新物件
            caseSource.UpdateDateTime = DateTime.Now;
            caseSource.UpdateUserName = ContextUtility.GetUserIdentity().Name;

            using (var scope = TrancactionUtility.TransactionScope())
            {
                // 計算勾稽編號 , 預立案的需要更新
                _CaseSourceFacade.CancelPreventTagsFromCaseIDs(caseSource.RelationCaseIDs);

                var con = new MSSQLCondition<CASE_SOURCE>(x => x.SOURCE_ID == caseSource.SourceID);
                con.IncludeBy(x => x.CASE_SOURCE_USER);
                con.IncludeBy(x => x.CASE);

                var entity = AutoMapper.Mapper.Map<CASE_SOURCE>(caseSource);

                con.ActionModify(x =>
                {
                    x.UPDATE_DATETIME = entity.UPDATE_DATETIME;
                    x.UPDATE_USERNAME = entity.UPDATE_USERNAME;
                    x.REMARK = entity.REMARK;
                    x.IS_TWICE_CALL = entity.IS_TWICE_CALL;
                    x.RELATION_CASE_IDs = entity.RELATION_CASE_IDs;
                    x.RELATION_CASE_SOURCE_ID = entity.RELATION_CASE_SOURCE_ID;
                    x.IS_PREVENTION = entity.IS_PREVENTION;

                    if (entity.IS_PREVENTION && x.CASE.Any())
                        throw new IndexOutOfRangeException(Case_lang.CASE_SOURCE_IS_REVENTION_UNDEFIND);


                    if (x.CASE_SOURCE_USER == null)
                    {
                        x.CASE_SOURCE_USER = entity.CASE_SOURCE_USER;
                    }
                    else
                    {

                        x.CASE_SOURCE_USER.ADDRESS = entity.CASE_SOURCE_USER.ADDRESS;
                        x.CASE_SOURCE_USER.BU_ID = entity.CASE_SOURCE_USER.BU_ID;
                        x.CASE_SOURCE_USER.BU_NAME = entity.CASE_SOURCE_USER.BU_NAME;
                        x.CASE_SOURCE_USER.EMAIL = entity.CASE_SOURCE_USER.EMAIL;
                        x.CASE_SOURCE_USER.GENDER = entity.CASE_SOURCE_USER.GENDER;
                        x.CASE_SOURCE_USER.JOB_ID = entity.CASE_SOURCE_USER.JOB_ID;
                        x.CASE_SOURCE_USER.JOB_NAME = entity.CASE_SOURCE_USER.JOB_NAME;
                        x.CASE_SOURCE_USER.MOBILE = entity.CASE_SOURCE_USER.MOBILE;
                        x.CASE_SOURCE_USER.NODE_ID = entity.CASE_SOURCE_USER.NODE_ID;
                        x.CASE_SOURCE_USER.NODE_NAME = entity.CASE_SOURCE_USER.NODE_NAME;
                        x.CASE_SOURCE_USER.ORGANIZATION_TYPE = entity.CASE_SOURCE_USER.ORGANIZATION_TYPE;
                        x.CASE_SOURCE_USER.PARENT_PATH_NAME = entity.CASE_SOURCE_USER.PARENT_PATH_NAME;
                        x.CASE_SOURCE_USER.TELEPHONE = entity.CASE_SOURCE_USER.TELEPHONE;
                        x.CASE_SOURCE_USER.TELEPHONE_BAK = entity.CASE_SOURCE_USER.TELEPHONE_BAK;
                        x.CASE_SOURCE_USER.UNIT_TYPE = entity.CASE_SOURCE_USER.UNIT_TYPE;
                        x.CASE_SOURCE_USER.USER_ID = entity.CASE_SOURCE_USER.USER_ID;
                        x.CASE_SOURCE_USER.USER_NAME = entity.CASE_SOURCE_USER.USER_NAME;
                    }
                });

                result = _CaseAggregate.CaseSource_T1_T2_.Update(con);

                scope.Complete();

            }

            var user = ContextUtility.GetUserIdentity()?.Instance;
            //紀錄來源History
            using (var scope = TrancactionUtility.NoTransactionScope())
            {
                global::System.Threading.Tasks.Task.Run(() => _CaseSourceFacade.CreateHistory(caseSource, Case_lang.CASE_SOURCE_UPDATE, user));
            };
            return result;
        }

        #region BATCH

        /// <summary>
        /// 預立案件自動結案
        /// </summary>
        public void ClosePreventionCaseSource()
        {
            _CommonAggregate.Logger.Info("【預立案自動結案】  開始排程");

            try
            {
                var conSys = new MSSQLCondition<SYSTEM_PARAMETER>(x => x.ID == EssentialCache.LayoutValue.PrecaseOverDueDeleteTemplate);
                var buTimeList = _SystemAggregate.SystemParameter_T1_.GetList(conSys);

                if (buTimeList == null || buTimeList.Count() == 0)
                {
                    _CommonAggregate.Logger.Info($"【預立案自動結案】  並未撈取有設參數的BU清單 , 故不往下執行。");
                    return;
                }

                _CommonAggregate.Logger.Info($"【預立案自動結案】  撈出共 {buTimeList.Count()} 個BU , 有設系統參數。");
                _CommonAggregate.Logger.Info($"【預立案自動結案】  準備進行各BU案件刪除。");

                foreach (var item in buTimeList)
                {
                    try
                    {
                        CloseCaseSource(item);
                    }
                    catch (Exception ex)
                    {
                        _CommonAggregate.Logger.Error(ex.Message);
                        _CommonAggregate.Loggers["Email"].Error($"【預立案自動結案】失敗 , ID代號 : {item.ID} ,BU代號 : {item.KEY}, 原因 : {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex.Message);
                _CommonAggregate.Loggers["Email"].Error($"【預立案自動結案】整批失敗, 原因 : {ex.Message}");
            }

            _CommonAggregate.Logger.Info("【預立案自動結案】  結束排程");
        }

        /// <summary>
        /// 刪除各BU的來源
        /// </summary>
        /// <param name="item"></param>
        private void CloseCaseSource(SYSTEM_PARAMETER item)
        {
            try
            {
                //轉換型別
                var isNumber = int.TryParse(item.VALUE, out var deleteTime);
                var deleteDay = DateTime.Now.AddDays(-deleteTime);

                //撈出NODEID，並判斷該BU是否存在
                var conNode = new MSSQLCondition<HEADQUARTERS_NODE>(x => x.NODE_KEY == item.KEY && x.NODE_TYPE_KEY == EssentialCache.NodeDefinitionValue.BusinessUnit);
                var buId = _OrganizationAggregate.HeaderQuarterNode_T1_.GetOfSpecific(conNode, x => x.NODE_ID);
                if (buId == 0)
                {
                    _CommonAggregate.Logger.Info($"【預立案自動結案】  此BU不存在 , BU代號 : {item.KEY}。");
                    return;
                }

                //取得以有勾稽的來源編號
                var hCon = new MSSQLCondition<CASE_SOURCE>();
                hCon.And(x => x.RELATION_CASE_SOURCE_ID != null);
                var sourceHasList = _CaseAggregate.CaseSource_T1_.GetListOfSpecific(hCon, x => x.RELATION_CASE_SOURCE_ID);

                var con = new MSSQLCondition<CASE_SOURCE>();
                con.IncludeBy(x => x.CASE);
                con.IncludeBy(x => x.CASE_SOURCE_USER);
                con.And(x => x.CASE.Any() == false);
                con.And(x => x.IS_PREVENTION == true);
                con.And(x => x.CREATE_DATETIME < deleteDay);
                con.And(x => x.NODE_ID == buId);

                //過濾已有勾稽的來源編號
                if (sourceHasList != null)
                {
                    con.And(x => !sourceHasList.Contains(x.SOURCE_ID));
                }

                var sourceList = _CaseAggregate.CaseSource_T1_.GetList(con);

                if (sourceList == null || sourceList.Count() == 0)
                {
                    _CommonAggregate.Logger.Info($"【預立案自動結案】  該BU代號 : {item.KEY} , 無過時之預立案 , 故不往下執行。");
                    return;
                }

                _CommonAggregate.Logger.Info($"【預立案自動結案】  該BU代號 : {item.KEY}  , 撈出共 {sourceList.Count()} 筆。");
                _CommonAggregate.Logger.Info($"【預立案自動結案】  準備進行該BU來源案件刪除。");

                foreach (var itemSource in sourceList)
                {
                    try
                    {
                        _CommonAggregate.Logger.Info($"【預立案自動結案】  來源代號 : {itemSource.SOURCE_ID} , 建立人員 : {itemSource.CREATE_USERNAME} , 備註 : {itemSource.REMARK} , 音檔位置 : {itemSource.VOICE_LOCATOR}。");
                        _CommonAggregate.Logger.Info($"【預立案自動結案】  準備進行來源案件刪除。");

                        _CaseAggregate.CaseSource_T1_T2_.Remove(x => x.SOURCE_ID == itemSource.SOURCE_ID);
                    }
                    catch (Exception ex)
                    {
                        _CommonAggregate.Logger.Error(ex.Message);
                        _CommonAggregate.Loggers["Email"].Error($"【預立案自動結案】該BU節點 : {itemSource.NODE_ID} , 刪除失敗, 原因 : {ex.Message}");
                    }
                }

                con.ClearFilters();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex.Message);
                _CommonAggregate.Loggers["Email"].Error($"【預立案自動結案】整批失敗, 原因 : {ex.Message}");
            }
        }

        #endregion


    }
}
