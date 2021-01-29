using System;
using System.Collections.Generic;
using System.Dynamic;
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
using SMARTII.Domain.Types;
using SMARTII.PPCLIFE.Domain;
using SMARTII.Resource.Tag;
using SMARTII.Service.Master.Resolver;

namespace SMARTII.PPCLIFE.Service
{
    public class PPCLIFENotificationService : PPCLIFENotificationBaseService, IPPCLIFENotificationService
    {
        private readonly ICaseAggregate _CaseAggregate;
        private readonly IMasterAggregate _MasterAggregate;
        private readonly ICommonAggregate _CommonAggregate;
        private readonly INotificationAggregate _NotificationAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly IIndex<PPCLifeArriveType, IPPCLifeNotificationFactory> _PPCLifeNotificationFactories;
        private readonly IPPCLifeNotificationFacade _PPCLIFEFacade;
        private readonly IIndex<NotificationType, INotificationProvider> _NotificationProviders;
        private readonly QuestionClassificationResolver _QuestionClassificationResolver;
        public PPCLIFENotificationService(ICaseAggregate CaseAggregate,
                                        IMasterAggregate MasterAggregate,
                                        ICommonAggregate CommonAggregate,
                                        INotificationAggregate NotificationAggregate,
                                        IOrganizationAggregate OrganizationAggregate,
                                        IPPCLifeNotificationFacade PPCLIFEFacade,
                                        IIndex<NotificationType, INotificationProvider> NotificationProviders,
                                        QuestionClassificationResolver QuestionClassificationResolver,
                                        IIndex<PPCLifeArriveType, IPPCLifeNotificationFactory> PPCLifeNotificationFactories)
        {
            _CaseAggregate = CaseAggregate;
            _CommonAggregate = CommonAggregate;
            _MasterAggregate = MasterAggregate;
            _NotificationAggregate = NotificationAggregate;
            _OrganizationAggregate = OrganizationAggregate;
            _PPCLifeNotificationFactories = PPCLifeNotificationFactories;
            _PPCLIFEFacade = PPCLIFEFacade;
            _NotificationProviders = NotificationProviders;
            _QuestionClassificationResolver = QuestionClassificationResolver;
        }

        /// <summary>
        /// (PPCLIFE 客制)排程計算大量叫修數量
        /// </summary>
        public void PPCLifeCalculate()
        {
            try
            {
                var now = DateTime.Now;

                _CommonAggregate.Logger.Info($"【統藥大量叫修通知】  準備進行排程 , 時間 : {now.ToString()}。");

                #region 找到該月案件清單

                #region 找到統藥NODE_ID
                var ppcLifeID = _OrganizationAggregate.HeaderQuarterNode_T1_T2_.Get(x => x.NODE_KEY == EssentialCache.BusinessKeyValue.PPCLIFE);

                if (ppcLifeID == null)
                {
                    _CommonAggregate.Logger.Info($"【統藥大量叫修通知】  未設定統藥識別值 , 排程結束。");
                    return;
                }
                #endregion

                _CommonAggregate.Logger.Info($"【統藥大量叫修通知】  準備撈取案件。");

                var firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddMilliseconds(-1); ;

                // 特殊問題分類
                var specficQusetionIDs = _MasterAggregate.QuestionClassification_T1_T2_.GetList(
                                                                        x => EssentialCache.PPCLifeCustomerValue.All.Contains(x.CODE) &&
                                                                        x.NODE_ID == ppcLifeID.NodeID).Select(q => q.ID).ToList();

                var cCon = new MSSQLCondition<CASE_ITEM>();
                cCon.IncludeBy(x => x.CASE);
                cCon.And(x => x.CASE.NODE_ID == ppcLifeID.NodeID);
                cCon.And(x => x.CASE.CREATE_DATETIME <= lastDayOfMonth);
                cCon.And(x => x.CASE.CREATE_DATETIME >= firstDayOfMonth);
                cCon.And(x => specficQusetionIDs.Contains(x.CASE.QUESION_CLASSIFICATION_ID));

                var cases = _CaseAggregate.CaseItem_T1_T2_.GetList(cCon);

                if (cases.Count() == 0)
                {
                    _CommonAggregate.Logger.Info($"【統藥大量叫修通知】  {now.ToString("yyyyMM")} 無統藥案件 , 排程結束。");
                    return;
                }

                _CommonAggregate.Logger.Info($"【統藥大量叫修通知】  撈取案件完畢 , 共 {cases.Count()} 筆。");
                #endregion

                //#region 清出非當月的資料 20200911確認不需刪除
                //_CommonAggregate.Logger.Info($"【統藥大量叫修通知】  準備清除 CASE_PPCLife 非 {now.ToString("yyyyMM")} 之案件。");

                //var rCon = new MSSQLCondition<CASE_PPCLIFE>();
                //rCon.Or(x => x.CREATE_DATETIME > lastDayOfMonth);
                //rCon.Or(x => x.CREATE_DATETIME < firstDayOfMonth);
                //_CaseAggregate.CasePPCLife_T1_T2_.RemoveRange(rCon);

                //_CommonAggregate.Logger.Info($"【統藥大量叫修通知】  清除非 {now.ToString("yyyyMM")} 之案件完畢。");
                //#endregion

                #region 比對現有的案件&商品

                // 比對現有的案件&商品
                _PPCLIFEFacade.PPCLifeCalculateCaseRepeat(cases.ToList());


                #endregion

                _CommonAggregate.Logger.Info($"【統藥大量叫修通知】  開始計算特定規則。");

                // 重算客制案件規則
                PPCLifeRuleReset();

                _CommonAggregate.Logger.Info($"【統藥大量叫修通知】  計算特定規則完畢。");
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex.Message);
                _CommonAggregate.Loggers["Email"].Error($"【統藥大量叫修通知】整批失敗, 原因 : {ex.Message}");
            }
        }

        /// <summary>
        /// (PPCLIFE 客制)重算客制案件規則
        /// </summary>
        public void PPCLifeRuleReset()
        {
            #region 計算達標案件

            // 撈出未忽略且在同批號同產品下未通知過的案件商品
            var con = new MSSQLCondition<CASE_PPCLIFE>();

            var casePPCLives = _CaseAggregate.CasePPCLife_T1_T2_.GetList(con).ToList();

            _CommonAggregate.Logger.Info($"【統藥大量叫修通知】  計算特定規則完畢。");

            if (casePPCLives.Count == 0)
            {
                _CommonAggregate.Logger.Info($"【統藥大量叫修通知】  無案件比對。");
                return;
            }

            var pPCLifeEffectiveSummariesNew = new List<PPCLifeEffectiveSummary>();

            foreach (PPCLifeArriveType pPCLifeArriveType in Enum.GetValues(typeof(PPCLifeArriveType)))
            {
                var service = _PPCLifeNotificationFactories.TryGetService(pPCLifeArriveType);

                var pPCLifeEffectiveSummaries = service.Execute(casePPCLives);

                pPCLifeEffectiveSummariesNew.AddRange(pPCLifeEffectiveSummaries);
            }

            #endregion

            #region 比對達標標的與現有標的並寫入至達標表

            // 比對達標標的
            _PPCLIFEFacade.PPCLifeCalculateSubjectRepeat(pPCLifeEffectiveSummariesNew);

            #endregion
        }

        #region 統藥大量叫修按鍵事件

        /// <summary>
        /// 不通知對象
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public void NoSend(int effectiveID)
        {
            var con = new MSSQLCondition<PPCLIFE_EFFECTIVE_SUMMARY>();
            con.And(x => x.ID == effectiveID);
            con.IncludeBy(x => x.CASE_PPCLIFE);
            var list = _NotificationAggregate.PPCLifeEffectiveSummary_T1_T2_.Get(con);

            if (list == null)
                new Exception(PPCLIFE_Notification_lang.NOTIFICATION_WHITOUT_NOTICE_FAIL_DATA_CHANG);
            //檢查規則是否符合
            var result = CheckCaseCount(list);
            if (!result)
                new Exception(PPCLIFE_Notification_lang.NOTIFICATION_WHITOUT_NOTICE_RULE_COUNT_FAIL);

            var conitem = new MSSQLCondition<ITEM>();

            conitem.And(x => list.ItemID == x.ID);

            var itemlist = _MasterAggregate.Item_T1_T2_Expendo_.GetList(conitem);

            var item = itemlist.First().Particular.CastTo<PPCLIFE_Item>();

            var temp = new PPCLifeResume();
            //組合資料，已便新增歷程
            item.Name = itemlist.First().Name;
            temp = GetItem(item, list, string.Empty, false);
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                //刪除規則
                DeletePPClifeSummary(list);

                //更新 CASE_PPCLIFE 
                UpdateCasePPClife(list);

                //新增歷程
                CreatPPClifeResume(temp);

                scope.Complete();
            }

        }


        /// <summary>
        /// 通知對象
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public void Send(int effectiveID, EmailPayload emailPayload)
        {
            var con = new MSSQLCondition<PPCLIFE_EFFECTIVE_SUMMARY>();
            con.And(x => x.ID == effectiveID);
            con.IncludeBy(x => x.CASE_PPCLIFE);
            var list = _NotificationAggregate.PPCLifeEffectiveSummary_T1_T2_.Get(con);

            if (list == null)
                new Exception(PPCLIFE_Notification_lang.NOTIFICATION_NOTICE_RULE_COUNT_FAIL);
            //檢查規則是否符合
            var result = CheckCaseCount(list);
            if (!result)
                new Exception(PPCLIFE_Notification_lang.NOTIFICATION_NOTICE_RULE_COUNT_FAIL);

            var conitem = new MSSQLCondition<ITEM>();

            conitem.And(x => list.ItemID == x.ID);

            var itemlist = _MasterAggregate.Item_T1_T2_Expendo_.GetList(conitem);

            var item = itemlist.First().Particular.CastTo<PPCLIFE_Item>();

            var temp = new PPCLifeResume();
            //組合資料，已便新增歷程
            item.Name = itemlist.First().Name;
            temp = GetItem(item, list, emailPayload.Content, true);

            new FileProcessInvoker((context) =>
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {

                        // 進行發信，然後將檔案路徑放進temp EMLFilePath
                        _NotificationProviders[NotificationType.Email].Send(
                                payload: emailPayload,
                                afterSend: AfterSenderHanlder(temp, context));

                        //刪除規則
                        DeletePPClifeSummary(list);

                        //更新 CASE_PPCLIFE 
                        UpdateCasePPClife(list);

                        //新增歷程
                        CreatPPClifeResume(temp);


                    scope.Complete();
                }
            });
        }

        /// <summary>
        /// 無視案件(更新IS_IGNOR為 1)
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public void Disregard(List<CasePPCLife> data)
        {

            int effectiveID = data.First().EffectiveID;
            var con = new MSSQLCondition<PPCLIFE_EFFECTIVE_SUMMARY>();
            con.And(x => x.ID == effectiveID);
            con.IncludeBy(x => x.CASE_PPCLIFE);
            var list = _NotificationAggregate.PPCLifeEffectiveSummary_T1_T2_.Get(con);
            if (list == null)
                new Exception(PPCLIFE_Notification_lang.NOTIFICATION_DISREGARD_FAIL_DATA_CHANG);

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {

                //加上無視狀態
                var conCase = new MSSQLCondition<CASE_PPCLIFE>();
                conCase.ActionModify(x =>
                {
                    x.IS_IGNORE = true;
                });

                data.ForEach(x =>
                {
                    conCase.And(y => y.CASE_ID == x.CaseID && y.ITEM_ID == x.ItemID);
                    _CaseAggregate.CasePPCLife_T1_T2_.Update(conCase);
                    conCase.ClearFilters();
                });
                scope.Complete();
            }
            // (PPCLIFE 客制)重算客制案件規則
            PPCLifeRuleReset();

        }

        #region 統藥大量叫修 更新/新增/計算規則
        /// <summary>
        /// 刪除規則
        /// </summary>
        /// <param name="ID"></param>
        private void DeletePPClifeSummary(PPCLifeEffectiveSummary data)
        {
            _NotificationAggregate.PPCLifeEffectiveSummary_T1_T2_.Operator(context =>
            {
                var db = (SMARTIIEntities)context;
                db.Configuration.LazyLoadingEnabled = false;

                var rule = db.PPCLIFE_EFFECTIVE_SUMMARY
                             .Include("CASE_PPCLIFE")
                             .Where(x => x.ARRIVE_TYPE == (byte)data.PPCLifeArriveType &&
                                         x.ITEM_ID == data.ItemID &&
                                         x.BATCH_NO == data.BatchNo).FirstOrDefault();

                rule.CASE_PPCLIFE.Clear();

                db.PPCLIFE_EFFECTIVE_SUMMARY.Remove(rule);
                db.SaveChanges();
            });
        }


        /// <summary>
        /// 新增PPCLIFE_RESUME 歷程
        /// </summary>
        /// <param name="data"></param>
        private void CreatPPClifeResume(PPCLifeResume data)
        {
            var con = new MSSQLCondition<PPCLIFE_RESUME>();
            _NotificationAggregate.PPCLifeResume_T1_T2_.Add(data);
        }

        /// <summary>
        /// 更新Case PPCLIFE 通知狀態
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private void UpdateCasePPClife(PPCLifeEffectiveSummary list)
        {
            var conCase = new MSSQLCondition<CASE_PPCLIFE>();
            conCase.ActionModify(x =>
            {
                if (list.PPCLifeArriveType == PPCLifeArriveType.AllSame)
                    x.ALLSAME_FINISH = true;
                if (list.PPCLifeArriveType == PPCLifeArriveType.DiffBatcNo)
                    x.DIFF_BATCHNO_FINISH = true;
                if (list.PPCLifeArriveType == PPCLifeArriveType.NothingBatchNo)
                    x.NOTHINE_BATCHNO_FINISH = true;
            });

            list.CasePPCLifes.ForEach(x =>
            {
                conCase.And(y => y.CASE_ID == x.CaseID && y.ITEM_ID == x.ItemID);
                _CaseAggregate.CasePPCLife_T1_T2_.Update(conCase);
                conCase.ClearFilters();
            });
        }
        /// <summary>
        /// 整合新增歷程資料
        /// </summary>
        /// <param name="data"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private PPCLifeResume GetItem(PPCLIFE_Item data, PPCLifeEffectiveSummary list, string Content, bool notice)
        {

            var result = new PPCLifeResume();
            //解析國際條碼
            if (data != null)
            {
                result.ItemCode = data.InternationalBarcode;
                result.ItemName = data.Name;
            }
            result.PPCLifeArriveType = list.PPCLifeArriveType;
            result.Content = Content;
            result.NotificationGroupResultType = notice ? NotificationGroupResultType.Finish : NotificationGroupResultType.NoSend;
            result.BatchNo = list.BatchNo;
            result.CreateDateTime = DateTime.Now;
            result.CreateUserName = ContextUtility.GetUserIdentity()?.Name;
            return result;
        }

        /// <summary>
        /// 檢查規則案件數是否符合
        /// </summary>
        /// <param name="effectiveID"></param>
        /// <returns></returns>
        private bool CheckCaseCount(PPCLifeEffectiveSummary list)
        {
            bool result = false;
            switch (list.PPCLifeArriveType)
            {
                case PPCLifeArriveType.AllSame:
                    if (list.CasePPCLifes.Count() >= 3)
                        result = true;
                    break;
                case PPCLifeArriveType.DiffBatcNo:
                    if (list.CasePPCLifes.Count() >= 5)
                        result = true;
                    break;
                case PPCLifeArriveType.NothingBatchNo:
                    if (list.CasePPCLifes.Count() >= 5)
                        result = true;
                    break;
                default:
                    return false;
            }
            return result;
        }

        private Action<Object> AfterSenderHanlder(PPCLifeResume resume, FileProcessContext context)
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

        #endregion 

        #endregion
    }
}
