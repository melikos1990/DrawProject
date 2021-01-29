#define Test

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using ClosedXML.Excel;
using MultipartDataMediaFormatter.Infrastructure;
using Nito.AsyncEx;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Case;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.DI;
using SMARTII.Domain.IO;
using SMARTII.Domain.Master;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Notification.Email;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Report;
using SMARTII.Domain.System;
using SMARTII.Domain.Thread;
using SMARTII.Domain.Transaction;
using SMARTII.Resource.Tag;
using SMARTII.Service.Cache;
using SMARTII.Service.Case.Flow;
using SMARTII.Service.Master.Resolver;
using SMARTII.Service.Organization.Provider;
using static SMARTII.Domain.Cache.EssentialCache;

namespace SMARTII.Service.Case.Service
{
    public class CaseService : ICaseService
    {

        private readonly AsyncLock _mutex = new AsyncLock();

        private readonly ICaseFacade _CaseFacade;
        private readonly IIndex<string, IFlow> _Flows;
        private readonly ICaseAggregate _CaseAggregate;
        private readonly IMasterAggregate _MasterAggregate;
        private readonly ICommonAggregate _CommonAggregate;
        private readonly ISystemAggregate _SystemAggregate;
        private readonly ICaseSourceFacade _CaseSourceFacade;
        private readonly IIndex<string, IEmailParser> _EmailParser;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly INotificationAggregate _NotificationAggregate;
        private readonly IIndex<string, ICaseSpecificFactory> _CaseSpecificFactory;
        private readonly QuestionClassificationResolver _QuestionClassificationResolver;
        private readonly HeaderQuarterNodeProcessProvider _HeaderQuarterNodeProcessProvider;
        private readonly OfficeEmailGroupResolver _OfficeEmailGroupResolver;
        private readonly IIndex<NotificationType, INotificationProvider> _NotificationProviders;
        private readonly INotificationPersonalFacade _NotificationPersonalFacade;


        public CaseService(ICaseFacade CaseFacade,
                           IIndex<string, IFlow> Flows,
                           ICaseAggregate CaseAggregate,
                           IMasterAggregate MasterAggregate,
                           ICommonAggregate CommonAggregate,
                           ISystemAggregate SystemAggregate,
                           ICaseSourceFacade CaseSourceFacade,
                           ICaseSearchProvider CaseSearchProvider,
                           IIndex<string, IEmailParser> EmailParser,
                           INotificationAggregate NotificationAggregate,
                           IOrganizationAggregate OrganizationAggregate,
                           IIndex<string, ICaseSpecificFactory> CaseSpecificFactory,
                           QuestionClassificationResolver QuestionClassificationResolver,
                           HeaderQuarterNodeProcessProvider HeaderQuarterNodeProcessProvider,
                           OfficeEmailGroupResolver OfficeEmailGroupResolver,
                           IIndex<NotificationType, INotificationProvider> NotificationProviders,
                           INotificationPersonalFacade NotificationPersonalFacade)
        {
            _Flows = Flows;
            _CaseFacade = CaseFacade;
            _EmailParser = EmailParser;
            _CaseAggregate = CaseAggregate;
            _MasterAggregate = MasterAggregate;
            _CommonAggregate = CommonAggregate;
            _SystemAggregate = SystemAggregate;
            _CaseSourceFacade = CaseSourceFacade;
            _CaseSpecificFactory = CaseSpecificFactory;
            _NotificationProviders = NotificationProviders;
            _OrganizationAggregate = OrganizationAggregate;
            _NotificationAggregate = NotificationAggregate;
            _QuestionClassificationResolver = QuestionClassificationResolver;
            _HeaderQuarterNodeProcessProvider = HeaderQuarterNodeProcessProvider;
            _OfficeEmailGroupResolver = OfficeEmailGroupResolver;
            _NotificationPersonalFacade = NotificationPersonalFacade;

        }

        #region Case's Behavior

        /// <summary>
        /// 案件結案回信
        /// </summary>
        /// <param name="case"></param>
        /// <returns></returns>
        public void CaseFinishedMailReply(string caseID, EmailPayload emailPayload)
        {
            // 取得既有案件
            var existData = _CaseAggregate.Case_T1_T2_.Get(x => x.CASE_ID == caseID);

            if (existData == null)
                throw new NullReferenceException(Common_lang.NOT_FOUND_DATA);

            // 寄出郵件
            new FileProcessInvoker((context) =>
            {
                using (var scope = TrancactionUtility.TransactionScope())
                {
                    //設定Email內容 20201027 共通規則:回覆顧客有原始來信內容
                    emailPayload.Content = GetEmailBody(existData, emailPayload.Content, existData.EMLFilePath);
                    emailPayload.IsHtmlBody = true;

                    // 進行發信
                    _NotificationProviders[NotificationType.Email].Send(
                        payload: emailPayload,
                        afterSend: AfterSenderHanlder(existData, context));

                    // 更新案件結案附件路徑
                    var con = new MSSQLCondition<CASE>(x => x.CASE_ID == caseID);

                    con.ActionModify(x =>
                    {
                        x.FINISH_EML_FILE_PATH = existData.FinishEMLFilePath;
                        x.FINISH_REPLY_DATETIME = DateTime.Now;
                        x.UPDATE_DATETIME = DateTime.Now;
                        x.UPDATE_USERNAME = ContextUtility.GetUserIdentity()?.Name;
                    });

                    _CaseAggregate.Case_T1_T2_.Update(con);

                    // TODO 寫入歷程
                    scope.Complete();
                }
            });
        }

        /// <summary>
        /// 信件認養
        /// </summary>
        /// <param name="messageID"></param>
        /// <returns></returns>
        public async Task<string> Adopt(string messageID, int buID, int groupID)
        {
            var result = string.Empty;

            // 檢查人員相關權限
            // 檢核GroupID 是否在個人資料範圍之中
            _CaseFacade.CheckGroupAuth(groupID, ContextUtility.GetUserIdentity()?.Instance);

            // 檢查該信件狀態
            // 回傳是否已結案後 , 進行後續邏輯上的處理
            var checkResult = await _CaseFacade.CheckExistCaseIsUnclose(messageID, buID);

            // 取得案件內容與檢檢核結果
            var email = checkResult.officialEmailEffective;

            // 回填服務群組代號
            email.GroupID = groupID;

            // 執行續鎖定
            // 為支援非同步處理 , 因此使用 (InterLock)
            using (await _mutex.LockAsync())
            {

                // 尚未結案 , 並建立歷程
                if (checkResult.isUnclosed)
                {
                    var customerEmailContent = "\r\n" + email.Subject + "\r\n" + email.Body;
                    var caseAssignmentBase = new CaseAssignmentBase()
                    {
                        CaseID = email.CaseID,
                        NodeID = email.NodeID,
                        OrganizationType = email.OrganizationType,
                        Content = SysCommon_lang.EMAIL_ADOPT_CASE_ASSIGNMENT + customerEmailContent,
                        EMLFilePath = email.FilePath,
                    };

                    // 背後實際上其實是進行歷程的新增
                    // 依據系統參數進行一般通知或是單位溝通的歷程
                    var caseAssignmentBaseResult = (CaseAssignmentBase)await _Flows[nameof(OfficialEmailAdoptCaseAssignmentFlow)].Run(caseAssignmentBase, email);

                    // 回傳須為案件編號
                    result = caseAssignmentBaseResult.CaseID;
                }
                else
                {

                    // 取得該組織節點設定的預設案件時效
                    var caseWarning = GetDefaultCaseWarning(email.NodeID);
                    var nodeKey = EssentialCache.BusinessKeyValue.COMMONBU;

                    DataStorage.NodeKeyDict.TryGetValue(buID, out nodeKey);

                    // 將信件內容轉為案件來源
                    var source = _EmailParser.TryGetService(nodeKey, EssentialCache.BusinessKeyValue.COMMONBU)
                                    .ConvertToCaseSource(email, caseWarning, ContextUtility.GetUserIdentity()?.Instance);

                    // 進行來源建立
                    // 背後是建立歷來遠與案件 (單一案件)
                    var caseSource = (CaseSource)await _Flows[nameof(OfficialEmailAdoptCaseFlow)].Run(source, email, false);

                    // 回傳須為案件編號
                    result = caseSource.Cases.First().CaseID;
                }
            }

            return await result.Async();
        }

        /// <summary>
        /// 管理者指派
        /// </summary>
        /// <param name="messageIDs"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public async Task<CounterResult> AdminOrder(string[] messageIDs, int buID, string userID, int groupID)
        {

            // 回傳處理結果
            // 成功數與失敗數
            var result = new CounterResult()
            {
                SuccessCount = 0,
                FailCount = 0
            };

            #region 檢查相關權限

            // 驗證人員存在與否
            var condition = new MSSQLCondition<USER>(x => x.USER_ID == userID);
            condition.IncludeBy(x => x.NODE_JOB);
            var user = _OrganizationAggregate.User_T1_T2_.Get(condition);

            if (user == null)
                throw new Exception(Common_lang.USER_UNDEFIND);

            // 檢查人員相關權限
            // 檢核GroupID 是否在個人資料範圍之中
            _CaseFacade.CheckGroupAuth(groupID, user);

            // 透過從前端傳回的信件識別值 (messageIDs)
            // 找到資料庫中的信件資訊
            var emails = _NotificationAggregate.OfficialEmailEffectivePayload_T1_T2_
                                               .GetList(x => messageIDs.Contains(x.MESSAGE_ID) && x.NODE_ID == buID);

            // 逐一驗證信件內容是否吻合規則
            // ※ 需要案件編號是空的
            HasCaseID(emails.ToList());

            #endregion 檢查相關權限

            //取得Bu Name
            var conNode = new MSSQLCondition<HEADQUARTERS_NODE>(x => x.NODE_ID == buID);
            var nodeName = _OrganizationAggregate.HeaderQuarterNode_T1_T2_.GetOfSpecific(conNode, x => x.NAME);

            // 執行續鎖定
            // 為支援非同步處理 , 因此使用 (InterLock)
            using (await _mutex.LockAsync())
            {
                foreach (var email in emails)
                {
                    email.GroupID = groupID;

                    try
                    {
                        // 取得該組織節點設定的預設案件時效
                        var caseWarning = GetDefaultCaseWarning(email.NodeID);

                        // 將信件內容轉為案件來源
                        var source = _EmailParser[EssentialCache.BusinessKeyValue.COMMONBU]
                                        .ConvertToCaseSource(email, caseWarning, user);

                        // 進行流程處理
                        // 背後主要建立案件來源 , 且刪除信件內容
                        await _Flows[nameof(OfficialEmailAdoptCaseFlow)].Run(source, email, true);

                        //轉一般案件 成功+1
                        result.SuccessCount += 1;
                    }
                    catch (Exception ex)
                    {
                        //該筆失敗 失敗+1
                        result.FailCount += 1;

                        _CommonAggregate.Logger.Error(
                                ex.PrefixMessage(OfficialEmail_lang.OFFICIAL_EMAIL_ADMIN_ORDER_FAILED));

                        continue;
                    }
                };


                // 建立通知行為
                if (result.SuccessCount > 0)
                    CreatePersonalNotification(user, result.SuccessCount, nodeName);


            }

            return await result.Async();
        }

        /// <summary>
        /// 自動分派
        /// </summary>
        /// <param name="eachPersonMail"></param>
        /// <param name="userIDs"></param>
        /// <returns></returns>
        public async Task<CounterResult> AutoOrder(int eachPersonMail, string[] userIDs, int buID, int groupID)
        {
            // 回傳處理結果
            // 成功數與失敗數
            var result = new CounterResult()
            {
                SuccessCount = 0,
                FailCount = 0
            };

            #region 檢查人員相關權限

            // 驗證人員存在與否
            var uCon = new MSSQLCondition<USER>(x => userIDs.Contains(x.USER_ID));
            uCon.IncludeBy(x => x.NODE_JOB);
            var users = _OrganizationAggregate.User_T1_T2_.GetList(uCon).ToArray();


            if (users == null)
                throw new Exception(Common_lang.USER_UNDEFIND);

            // 檢查人員相關權限
            // 檢核GroupID 是否在個人資料範圍之中
            _CaseFacade.CheckGroupAuth(groupID, users);

            #endregion 檢查人員相關權限

            //取得Bu Name
            var conNode = new MSSQLCondition<HEADQUARTERS_NODE>(x => x.NODE_ID == buID);
            var nodeName = _OrganizationAggregate.HeaderQuarterNode_T1_T2_.GetOfSpecific(conNode, x => x.NAME);

            // 執行續鎖定
            // 為支援非同步處理 , 因此使用 (InterLock)
            using (await _mutex.LockAsync())
            {
                foreach (var user in users)
                {
                    try
                    {
                        var userSuccess = 0;

                        // 取得每個人分配到的Mail 資料數
                        var oCon = new MSSQLCondition<OFFICIAL_EMAIL_EFFECTIVE_DATA>(0, eachPersonMail);

                        oCon.And(x => x.NODE_ID == buID &&
                                      x.CASE_ID == null);

                        oCon.OrderBy(x => x.RECEIVED_DATETIME, OrderType.Asc);

                        var emails = _NotificationAggregate.OfficialEmailEffectivePayload_T1_T2_
                                                           .GetPaging(oCon)
                                                           .ToList();

                        foreach (var email in emails)
                        {
                            email.GroupID = groupID;

                            try
                            {
                                // 取得案件的預設時效
                                var caseWarning = GetDefaultCaseWarning(email.NodeID);

                                var source = _EmailParser[EssentialCache.BusinessKeyValue.COMMONBU]
                                                .ConvertToCaseSource(email, caseWarning, user);


                                // 進行流程處理
                                // 背後主要建立案件來源 , 且刪除信件內容
                                await _Flows[nameof(OfficialEmailAdoptCaseFlow)].Run(source, email, true);

                                //轉一般案件 成功+1
                                result.SuccessCount += 1;
                                userSuccess += 1;
                            }
                            catch (Exception ex)
                            {
                                //該筆失敗 失敗+1
                                result.FailCount += 1;

                                _CommonAggregate.Logger.Error(
                                        ex.PrefixMessage(OfficialEmail_lang.OFFICIAL_EMAIL_AUTO_ORDER_FAILED));

                                continue;
                            }
                        };


                        // 建立通知行為
                        if (userSuccess > 0)
                            CreatePersonalNotification(user, userSuccess, nodeName);

                    }
                    catch (Exception ex)
                    {
                        _CommonAggregate.Logger.Error(
                                ex.PrefixMessage(OfficialEmail_lang.OFFICIAL_EMAIL_AUTO_ORDER_FAILED));
                    }
                };
            }

            return await result.Async();
        }

        /// <summary>
        /// 批次回覆
        /// </summary>
        /// <param name="QestionID"></param>
        /// <param name="EmailContent"></param>
        /// <param name="FinishContent"></param>
        /// <param name="MessageIDs"></param>
        /// <returns></returns>
        public async Task<CounterResult> ReplyRange(int questionID, string emailContent, string finishContent, string[] messageIDs, int buID, int groupID)
        {
            // 回傳處理結果
            // 成功數與失敗數
            var result = new CounterResult()
            {
                SuccessCount = 0,
                FailCount = 0
            };

            #region 檢查人員相關權限

            // 檢查人員相關權限
            // 檢核GroupID 是否在個人資料範圍之中
            _CaseFacade.CheckGroupAuth(groupID, ContextUtility.GetUserIdentity()?.Instance);

            // 透過從前端傳回的信件識別值 (messageIDs)
            // 找到資料庫中的信件資訊
            var emails = _NotificationAggregate.OfficialEmailEffectivePayload_T1_T2_
                                               .GetList(x => messageIDs.Contains(x.MESSAGE_ID) && x.NODE_ID == buID);

            emails = _OfficeEmailGroupResolver.ResolveCollection(emails);

            // 逐一驗證信件內容是否吻合規則
            // ※ 需要案件編號是空的
            HasCaseID(emails.ToList());

            #endregion 檢查人員相關權限

            // 執行續鎖定
            // 為支援非同步處理 , 因此使用 (InterLock)
            using (await _mutex.LockAsync())
            {
                foreach (var email in emails)
                {
                    email.GroupID = groupID;

                    try
                    {

                        #region 寄件初始化

                        var receivers = new List<ConcatableUser>()
                        {
                            new ConcatableUser()
                            {
                                Email = email.FromAddress
                            }
                        };

                        var sender = new ConcatableUser()
                        {
                            Email = email.BuMailAccount,
                            UserName = email.MailDisplayName
                        };

                        var mail = new EmailPayload()
                        {
                            Receiver = receivers,
                            Sender = sender,
                            Title = GlobalizationCache.ReplyMailTilie + email.Subject
                        };

                        #endregion 寄件初始化

                        #region 來源初始化

                        var caseWarning = GetDefaultCaseWarning(email.NodeID);

                        var source = _EmailParser[EssentialCache.BusinessKeyValue.COMMONBU]
                                           .ConvertToCaseSource(email, caseWarning, ContextUtility.GetUserIdentity()?.Instance);

                        #endregion 來源初始化


                        // 寄出郵件
                        new FileProcessInvoker((context) =>
                        {
                            using (var scope = TrancactionUtility.TransactionScope())
                            {
                                #region 立案

                                var caseSource = (CaseSource)_Flows[nameof(OfficialEmailAdoptCaseFlow)].Run(source, email, false).Result;

                                var existCase = caseSource.Cases.First();

                                #endregion 立案                                

                                #region 寄信

                                ////解析信件內容動態欄位
                                //var stringFormatMailContent = existCase.ParsingTemplate(emailContent);
                                //mail.Content = stringFormatMailContent;

                                //設定Email內容 20201027 共通規則:回覆顧客有原始來信內容
                                mail.Content = GetEmailBody(existCase, emailContent, email.FilePath);
                                mail.IsHtmlBody = true;
                                // 進行發信
                                _NotificationProviders[NotificationType.Email].Send(
                                                                                    payload: mail,
                                                                                    afterSend: AfterSenderHanlder(existCase, context));

                                #endregion 寄信

                                #region 結案
                                var conCase = new MSSQLCondition<CASE>();
                                conCase.And(x => x.CASE_ID == existCase.CaseID);
                                conCase.IncludeBy(x => x.CASE_CONCAT_USER);
                                conCase.IncludeBy(x => x.CASE_COMPLAINED_USER);
                                conCase.IncludeBy(x => x.CASE_TAG);

                                var @case = _CaseAggregate.Case_T1_T2_
                                                          .Get(conCase);

                                //解析結案內容動態欄位
                                var stringFormatFinishContent = existCase.ParsingTemplate(finishContent);


                                //撈取預設結案處置原因
                                var caseFinishReasonDatas = new List<CaseFinishReasonData>();

                                var fCon = new MSSQLCondition<CASE_FINISH_REASON_CLASSIFICATION>();
                                fCon.And(x => x.NODE_ID == existCase.NodeID);
                                fCon.And(x => x.IS_REQUIRED == true);
                                fCon.IncludeBy(x => x.CASE_FINISH_REASON_DATA);

                                var defaultCaseFinishReasonClassifications = _MasterAggregate.CaseFinishReasonClassification_T1_T2_.GetList(fCon).ToList();

                                defaultCaseFinishReasonClassifications.ForEach(x =>
                                {
                                    caseFinishReasonDatas.AddRange(x.CaseFinishReasonDatas.Where(y => y.Default == true));
                                });

                                //結案案件資訊
                                @case.QuestionClassificationID = questionID;
                                @case.FinishContent = stringFormatFinishContent;
                                @case.FinishEMLFilePath = existCase.FinishEMLFilePath;
                                @case.CaseFinishReasonDatas = caseFinishReasonDatas;

                                @case = (Domain.Case.Case)_Flows[nameof(CaseFinishedFlow)].Run(@case).Result;

                                var con = new MSSQLCondition<CASE>(x => x.CASE_ID == @case.CaseID);

                                con.ActionModify(x =>
                                {
                                    x.FINISH_EML_FILE_PATH = existCase.FinishEMLFilePath;
                                    x.FINISH_REPLY_DATETIME = DateTime.Now;
                                });

                                _CaseAggregate.Case_T1_T2_.Update(con);

                                #endregion 結案                             

                                scope.Complete();
                            };
                        });

                        //轉一般案件 成功+1
                        result.SuccessCount += 1;
                    }
                    catch (Exception ex)
                    {
                        //該筆失敗 失敗+1
                        result.FailCount += 1;

                        _CommonAggregate.Logger.Error(
                                ex.PrefixMessage(OfficialEmail_lang.OFFICIAL_EMAIL_REPLY_RANGE_FAILED));

                        continue;
                    }
                };
            }
            return await result.Async();
        }

        #endregion Case's Behavior

        #region CRUD

        /// <summary>
        /// 更新案件
        /// </summary>
        /// <param name="case"></param>
        /// <returns></returns>
        public Domain.Case.Case UpdateComplete(Domain.Case.Case @case, bool IsFinish)
        {
            Domain.Case.Case existCase = null;

            var term = default(HeaderQuarterTerm);

            // 填充待更新物件

            @case.CaseConcatUsers?.ForEach(x => x.CaseID = @case.CaseID);
            @case.CaseComplainedUsers?.ForEach(x => x.CaseID = @case.CaseID);
            @case.UpdateDateTime = DateTime.Now;
            @case.UpdateUserName = ContextUtility.GetUserIdentity()?.Name;

            // 取得BU識別值 , 如(003 => ASO) ...
            // 此欄位為系統原則 , 若無法取得 , 則需拋出例外 。
            term = (HeaderQuarterTerm)_HeaderQuarterNodeProcessProvider.GetTerm(
                   @case.NodeID,
                   @case.OrganizationType);

            existCase = _CaseAggregate.Case_T1_T2_.Get(x => x.CASE_ID == @case.CaseID);

            using (var scope = TrancactionUtility.TransactionScope())
            {
                new FileProcessInvoker(context =>
                {
                    // 更新反應者/被反應者
                    _CaseFacade.BatchModifyUsers(@case);

                    // 計算勾稽編號 , 預立案的需要更新
                    _CaseSourceFacade.CancelPreventTagsFromCaseIDs(@case.RelationCaseIDs);


                    // 案件副檔
                    var pathArray = FileSaverUtility.SaveCaseFiles(context, @case, @case.Files);
                    @case.FilePath = pathArray?.ToArray();

                    // 結案副檔
                    var finishPathArray = FileSaverUtility.SaveCaseFiles(context, @case, @case.FinishFiles);
                    @case.FinishFilePath = finishPathArray?.ToArray();

                    // 更新案件時效
                    @case.PromiseDateTime = (existCase.CaseWarningID != @case.CaseWarningID) ?
                            _CaseFacade.GetPromiseDateTime(@case, term, @case.CreateDateTime) : existCase.PromiseDateTime;


                    var entity = AutoMapper.Mapper.Map<CASE>(@case);

                    // 更新標籤/新增標籤
                    _CaseFacade.UpdateOrCreateTags(@case);

                    var con = new MSSQLCondition<CASE>(x => x.CASE_ID == existCase.CaseID);

                    con.ActionModify(x =>
                    {
                        x.IS_ATTENSION = entity.IS_ATTENSION;
                        x.IS_REPORT = entity.IS_REPORT;
                        x.RELATION_CASE_IDs = entity.RELATION_CASE_IDs;

                        if (IsFinish)
                        {
                            x.CASE_TYPE = entity.CASE_TYPE;
                            x.FINISH_USERNAME = entity.FINISH_USERNAME;
                            x.FINISH_DATETIME = entity.FINISH_DATETIME;
                        }

                        x.FINISH_CONTENT = entity.FINISH_CONTENT;
                        x.FINISH_FILE_PATH = x.FINISH_FILE_PATH.InsertArraySerialize(finishPathArray?.ToArray());
                        x.FILE_PATH = x.FILE_PATH.InsertArraySerialize(pathArray?.ToArray());
                        x.CONTENT = entity.CONTENT;
                        x.EXPECT_DATETIME = entity.EXPECT_DATETIME;
                        x.PROMISE_DATETIME = entity.PROMISE_DATETIME;
                        x.QUESION_CLASSIFICATION_ID = entity.QUESION_CLASSIFICATION_ID;
                        x.CASE_WARNING_ID = entity.CASE_WARNING_ID;
                        x.UPDATE_DATETIME = entity.UPDATE_DATETIME;
                        x.UPDATE_USERNAME = entity.UPDATE_USERNAME;
                        x.J_CONTENT = entity.J_CONTENT;
                    });

                    _CaseAggregate.Case_T1_T2_.Update(con);

                    // 更新案件其他資訊
                    _CaseSpecificFactory.TryGetService(
                        term.NodeKey,
                        EssentialCache.BusinessKeyValue.COMMONBU).Update(@case);

                    // 更新結案處置原因
                    _CaseFacade.BatchModifyFinishedReasons(@case);

                });

                scope.Complete();
            };

            return @case;
        }

        #endregion CRUD

        #region BATCH

        /// <summary>
        /// 逾時案件未結案通知
        /// </summary>
        public void CaseTimeoutNotice()
        {
            _CommonAggregate.Logger.Info("【逾時未結案通知】  開始排程");

            // 找到未結案案件 , 且應完成時間小於目前時間的
            DateTime now = DateTime.Now;
            var caseType = (byte)CaseType.Finished;

            var con = new MSSQLCondition<CASE>(x => x.CASE_TYPE != caseType && x.PROMISE_DATETIME < now);
            con.IncludeBy(x => x.CASE_CONCAT_USER);
            con.IncludeBy(x => x.CASE_WARNING);
            con.IncludeBy(x => x.CASE_COMPLAINED_USER);
            con.IncludeBy(x => x.CASE_ASSIGNMENT);
            con.IncludeBy(x => x.CASE_SOURCE);

            var caseTimeoutList = _CaseAggregate.Case_T1_T2_.GetList(con);

            _CommonAggregate.Logger.Info($"【逾時未結案通知】，執行時間:{DateTime.Now}，撈出共 {caseTimeoutList.Count()} 個逾時案件。");

            var buGroupList = caseTimeoutList.GroupBy(x => x.NodeID);
            var caseTimeoutfilterSysList = new List<Domain.Case.Case>();
            foreach (var buGroup in buGroupList)
            {
                var nodeKey = EssentialCache.BusinessKeyValue.COMMONBU;
                DataStorage.NodeKeyDict.TryGetValue(buGroup.Key, out nodeKey);
                var sCon = new MSSQLCondition<SYSTEM_PARAMETER>(x => x.ID == CaseFinishTimeValue.CASE_UNCLOSENOTIFICATION && x.KEY == nodeKey);
                var buTime = _SystemAggregate.SystemParameter_T1_T2_.GetOfSpecific(sCon, x => x.VALUE);
                var limitTime = now.AddDays((string.IsNullOrEmpty(buTime) ? 0 : int.Parse(buTime)));
                foreach (var item in buGroup)
                {
                    if (item.PromiseDateTime < limitTime)
                        caseTimeoutfilterSysList.Add(item);
                }
            }


            var groupList = caseTimeoutfilterSysList.GroupBy(x => x.GroupID);

            var conCallCenter = new MSSQLCondition<CALLCENTER_NODE>();
            var list = groupList.Select(x => x.Key).ToList();
            conCallCenter.And(x => list.Contains(x.NODE_ID));
            var callCenterList = _OrganizationAggregate.CallCenterNode_T1_T2_.GetList(conCallCenter);

            try
            {
                // 依照GroupID 做分群 並且迭代
                foreach (var item in groupList)
                {
                    _CommonAggregate.Logger.Info($"【逾時未結案通知】，檢查是協同或者是單人。");
                    //檢查是協同或者是單人
                    if (callCenterList.First(x => x.NodeID == item.Key).WorkProcessType == WorkProcessType.Individual)
                    {
                        _CommonAggregate.Logger.Info($"【逾時未結案通知】，尋找負責人信箱。");
                        var singleGroup = item.GroupBy(x => x.ApplyUserID);
                        //查詢負責人信箱
                        var conUser = new MSSQLCondition<USER>();

                        var appList = singleGroup.Select(x => x.Key);
                        conUser.And(x => appList.Contains(x.USER_ID));
                        var applyMail = _OrganizationAggregate.User_T1_T2_.GetList(conUser).ToList();

                        // 依負責人 GroupBy Case
                        foreach (var applyitem in singleGroup)
                        {
                            string mail = applyMail.Where(x => x.UserID == applyitem.Key).First().Email;
                            List<string> mailList = new List<string>();
                            mailList.Add(mail);

                            _CommonAggregate.Logger.Info($"【逾時未結案通知】，開始產生Excel檔案。");
                            CaseTimeoutMailSender(applyitem.ToList(), mailList);
                        }
                    }
                    else if (callCenterList.First(x => x.NodeID == item.Key).WorkProcessType == WorkProcessType.Accompanied)
                    {
                        _CommonAggregate.Logger.Info($"【逾時未結案通知】，尋找群組信箱。");
                        // 依據 CallCenterNode , 找到底下人員 , 撈出Email清單 (準備組入寄送清單)
                        var jcon = new MSSQLCondition<NODE_JOB>(x => x.NODE_ID == item.Key && x.ORGANIZATION_TYPE == (byte)OrganizationType.CallCenter);
                        jcon.IncludeBy(x => x.USER);
                        jcon.IncludeBy(x => x.JOB);
                        var jobs = _OrganizationAggregate.JobPosition_T1_T2_.GetList(jcon);

                        var jobMailList = jobs.SelectMany(x => x.Users).Select(y => y.Email).ToList();

                        List<string> mailList = new List<string>();
                        mailList.AddRange(jobMailList);
                        _CommonAggregate.Logger.Info($"【逾時未結案通知】，開始產生Excel檔案。");
                        // 案件清單 , 透過CaseReport Provider , 產出 excel
                        CaseTimeoutMailSender(item.ToList(), mailList);
                    }
                }
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);
                _CommonAggregate.Logger.Info("[逾時未結案通知] 寄信失敗");
            }
        }

        #endregion BATCH

        #region PRIVATE

        /// <summary>
        /// 逾時未結案通知行為
        /// </summary>
        /// <param name="cases"></param>
        /// <param name="mails"></param>
        private void CaseTimeoutMailSender(List<Domain.Case.Case> cases, List<string> mails)
        {
            // 案件清單 , 透過CaseReport Provider , 產出 excel
            var @byte = CaseTimeoutNoticeReport(cases);

            _CommonAggregate.Logger.Info($"【逾時未結案通知】，組合發信內容。");
            //組合寄信內容
            EmailPayload emailPayload = new EmailPayload()
            {
                Attachments = new List<HttpFile>()
                                {
                                    new HttpFile(){
                                        Buffer = @byte,
                                        FileName = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "逾時案件未結案通知.xlsx"
                                    }
                                },
                Cc = new List<ConcatableUser>()
                            {
                            new ConcatableUser(){ Email = GlobalizationCache.Instance.AdminMailReceiverAddress }
                            },
                Sender = new ConcatableUser() { Email = GlobalizationCache.Instance.AdminMailAddress, UserName = GlobalizationCache.APName },
                Receiver = new List<ConcatableUser>(),
                Title = DateTime.Now.ToString("yyyy-MM-dd") + "逾時案件未結案通知",
                Content = "逾時案件明細 , 如附件 , 請參閱"
            };
            mails.ForEach(x =>
                 emailPayload.Receiver.Add(new ConcatableUser()
                 {
                     //Email = "carylin@ptc-nec.com.tw"
                     Email = x
                 })
                );
            _CommonAggregate.Logger.Info($"【逾時未結案通知】，發信。");

            //寄信
            _NotificationProviders[NotificationType.Email].Send(
           payload: emailPayload);
        }

        /// <summary>
        /// 逾時未結案通知Excel
        /// </summary>
        /// <param name="cases"></param>
        /// <returns></returns>
        private byte[] CaseTimeoutNoticeReport(List<Domain.Case.Case> cases)
        {
            //找出最多分類數量
            var classList = _MasterAggregate.VWQuestionClassification_QuestionClassification_.GetList().ToList();
            int classificationCount = 0;

            foreach (var item in cases)
            {
                var list = classList.Where(x => x.ID == item.QuestionClassificationID).FirstOrDefault();
                if (list != null && list.Level > classificationCount)
                {
                    classificationCount = list.Level;
                }
            }
            var con = new MSSQLCondition<HEADQUARTERS_NODE>();
            var buList = _OrganizationAggregate.HeaderQuarterNode_T1_T2_.GetList(con);

            XLWorkbook book = new XLWorkbook();
            var ws = book.AddWorksheet("工作表1");

            #region 開頭

            ws.Cell(1, 1).Value = "企業別";
            ws.Cell(1, 2).Value = "來源編號";
            ws.Cell(1, 3).Value = "案件編號";
            ws.Cell(1, 4).Value = "案件來源";
            ws.Cell(1, 5).Value = "案件狀態";
            ws.Cell(1, 6).Value = "案件等級";
            ws.Cell(1, 7).Value = "關注案件";
            for (int i = 1; i <= classificationCount; i++)
            {
                ws.Cell(1, 7 + i).Value = "分類項目" + i.ToString();
            }
            int column = 7 + classificationCount;

            ws.Cell(1, column + 1).Value = "連絡者型態";
            ws.Cell(1, column + 2).Value = "連絡者";
            ws.Cell(1, column + 3).Value = "被反應者";
            ws.Cell(1, column + 4).Value = "組織";
            ws.Cell(1, column + 5).Value = "立案時間";
            ws.Cell(1, column + 6).Value = "期望期限";
            ws.Cell(1, column + 7).Value = "應完成時間";
            ws.Cell(1, column + 8).Value = "案件內容";
            ws.Cell(1, column + 9).Value = "未結案轉派";

            int allColumn = column + 8;

            #endregion 開頭

            int row = 2;
            foreach (var item in cases)
            {
                //企業別
                ws.Cell(row, 1).Value = buList.FirstOrDefault(x => x.BUID == item.NodeID).Name;
                //來源編號
                ws.Cell(row, 2).Value = "'" + item.SourceID.ToString();
                //案件編號
                ws.Cell(row, 3).Value = "'" + item.CaseID.ToString();
                //案件來源
                ws.Cell(row, 4).Value = item.CaseSource.CaseSourceType.GetDescription();
                //案件狀態
                ws.Cell(row, 5).Value = item.CaseType.GetDescription();
                //案件等級
                ws.Cell(row, 6).Value = item.CaseWarning.Name;
                //關注案件
                ws.Cell(row, 7).Value = item.IsAttension ? "是" : "否";
                //分類
                var list = classList.Where(x => x.ID == item.QuestionClassificationID).FirstOrDefault();
                for (int i = 1; i < classificationCount + 1; i++)
                {
                    if (list != null)
                    {
                        string erw = list.ParentNamePathByArray[i - 1];
                        ws.Cell(row, 7 + i).Value = list.ParentNamePathByArray[i - 1];

                        if (list.ParentNamePathByArray.Count() < i + 1)
                        {
                            break;
                        }
                    }
                }
                //聯絡者型態
                ws.Cell(row, column + 1).Value = item.CaseConcatUsers.FirstOrDefault().UnitType.GetDescription();
                //連絡者
                ws.Cell(row, column + 2).Value = item.CaseConcatUsers.FirstOrDefault().UserName + item.CaseConcatUsers.FirstOrDefault().Gender.GetDescription();
                //當權責單位有資料
                if (item.CaseComplainedUsers.Where(x => x.CaseComplainedUserType == CaseComplainedUserType.Responsibility).Count() != 0)
                {
                    //被反應者
                    ws.Cell(row, column + 3).Value = item.CaseComplainedUsers.Where(x => x.CaseComplainedUserType == CaseComplainedUserType.Responsibility).FirstOrDefault().NodeName;
                    //組織
                    var parentPathName = item.CaseComplainedUsers.Where(x => x.CaseComplainedUserType == CaseComplainedUserType.Responsibility).FirstOrDefault().ParentPathName;
                    if (parentPathName != null)
                    {
                        var parentPathNameArrray = parentPathName.Split('/');
                        ws.Cell(row, column + 4).Value = parentPathNameArrray[parentPathNameArrray.Length - 1];
                    }
                }
                //立案時間
                ws.Cell(row, column + 5).Value = item.ApplyDateTime.ToString("yyyy/MM/dd HH:mm:ss");
                //期望期限
                DateTime expectDateTime = item.ExpectDateTime ?? DateTime.Now;
                ws.Cell(row, column + 6).Value = item.ExpectDateTime == null ? "" : expectDateTime.ToString("yyyy/MM/dd HH:mm:ss");
                //應完成時間
                ws.Cell(row, column + 7).Value = item.PromiseDateTime.ToString("yyyy/MM/dd HH:mm:ss");
                //案件內容
                ws.Cell(row, column + 8).Value = item.Content;
                //未結案轉派
                ws.Cell(row, column + 9).Value = item.CaseAssignments.Where(x => x.CaseAssignmentType != 0).Count();

                row++;
            }
            //左右至中
            ws.Range(1, 1, row - 1, column + 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            //水平 至中
            ws.Range(1, 1, row - 1, column + 9).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range(1, 1, row - 1, column + 9);

            //自動對應欄位寬度
            ws.Rows().AdjustToContents();
            ws.Columns().AdjustToContents();

            //設定內容靠左與固定寬度
            ws.Range(2, column + 8, row - 1, column + 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Column(column + 8).Width = 100;
            //20200911 固定列高
            for (int i = 2; i < row; i++)
            {
                ws.Row(i).Height = 95;
            }
            //格線
            var rngTable1 = ws.Range(1, 1, row - 1, column + 9);
            rngTable1.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            rngTable1.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            var result = ReportUtility.ConvertBookToByte(book, "");

            return result;
        }

        /// <summary>
        /// 取出預設時效
        /// ※ 撈出該BU排序1的時效
        /// </summary>
        /// <param name="nodeID"></param>
        /// <returns></returns>
        private CaseWarning GetDefaultCaseWarning(int nodeID)
        {

            var caseWarning = _MasterAggregate.CaseWarning_T1_T2_.Get(x => x.NODE_ID == nodeID && x.ORDER == 1);

            if (caseWarning == null)
                throw new Exception(Common_lang.BUSINESSS_CASE_WARNING_NOT_FOUND);

            return caseWarning;

        }

        /// <summary>
        /// 更新案件附件
        /// </summary>
        /// <param name="case"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private Action<Object> AfterSenderHanlder(Domain.Case.Case @case, FileProcessContext context)
        {
            return (object obj) =>
            {
                var fileName = $"{Guid.NewGuid().ToString()}.eml";

                var emailBytes = (obj as byte[]);

                var physicalDirPath = FilePathFormatted.GetEmailSenderPhysicalDirPath();

                var virtualPath = FilePathFormatted.GetEmailSenderVirtualFilePath(fileName);

                var path = emailBytes.SaveAsFilePath(physicalDirPath, fileName);

                @case.FinishEMLFilePath = virtualPath;

                context.Paths.Add(path);
            };
        }




        #endregion PRIVATE

        #region OTHER

        /// <summary>
        /// 檢查該信件有無案編
        /// </summary>
        /// <param name="emails"></param>
        /// <returns></returns>
        private bool HasCaseID(List<OfficialEmailEffectivePayload> emails)
        {
            foreach (var email in emails)
            {
                if (email == null)
                    throw new Exception(OfficialEmail_lang.OFFICIAL_EMAIL_EMAIL_ALREADY_ADOPT);

                if (string.IsNullOrEmpty(email.CaseID) == false)
                    throw new Exception(OfficialEmail_lang.OFFICIAL_EMAIL_EMAIL_HAS_CASE);
            }

            return true;
        }

        /// <summary>
        /// 建立個人通知
        /// </summary>
        /// <param name="user"></param>
        /// <param name="count"></param>
        private void CreatePersonalNotification(User user, int count, string nodeName)
        {
            var personalNotice = new PersonalNotification()
            {
                UserID = user.UserID,
                Content = string.Format(SysCommon_lang.EMAIL_ADOPT_PERSONAL_NOTICE, nodeName, ContextUtility.GetUserIdentity()?.Name, count),
                CreateDateTime = DateTime.Now,
                CreateUserName = ContextUtility.GetUserIdentity()?.Name,
                PersonalNotificationType = PersonalNotificationType.MailAdopt
            };

            _NotificationAggregate.PersonalNotification_T1_T2_.Add(personalNotice);
            _NotificationPersonalFacade.NotifyWeb(user.UserID);
        }
        /// <summary>
        /// 依BU設定信件內容 - 共同規則:回信顧客需加入原信件內容
        /// </summary>
        /// <param name="Case"></param>
        /// <param name="EmailContent"></param>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        private string GetEmailBody(Domain.Case.Case Case, string EmailContent, string FilePath = "")
        {
            string result = "";

            result += "<HTML>";
            result += "<HEAD><META content=\"text/html; charset=utf8\" http-equiv=Content-Type>";
            result += "</HEAD><BODY>";

            var replyContent = Case.ParsingTemplate(EmailContent);

            result += replyContent?.ToHtmlFormat();

            if (!string.IsNullOrEmpty(FilePath))
            {
                var keyArray = ParsinRequestParam(FilePath);
                if (keyArray.Count == 3)
                {
                    string path = string.Format(FilePathFormatted.OfficialEmailPhysicalFilePath, keyArray[0], keyArray[1], Path.GetFileName(keyArray[2]));
                    var imgByte = File.ReadAllBytes(path);
                    var OrgMail = ActiveUp.Net.Mail.Parser.ParseMessage(imgByte);

                    result += "";
                    //回覆標頭區塊
                    result += "<DIV style=\"FONT: 10pt tahoma; FONT-WEIGHT: normal; COLOR: #000000; FONT-STYLE: normal; TEXT-DECORATION: none; DISPLAY: inline\"><DIV style=\"BACKGROUND: #f5f5f5\">";
                    result += "<DIV style=\"font-color: black\"><B>From:</B>" + OrgMail.From.Link + "</DIV>";
                    result += "<DIV><B>Sent:</B> " + OrgMail.DateString + "</DIV>";
                    result += "<DIV><B>To:</B> ";
                    foreach (var item in OrgMail.To)
                    {
                        result += item.Link + " ; ";
                    }

                    result += " </DIV>";

                    if (OrgMail.Cc.Count > 0)
                    {
                        result += "<DIV><B>Cc:</B> ";
                        foreach (var item in OrgMail.Cc)
                        {
                            result += item.Link + " ; ";
                        }
                        result += " </DIV>";
                    }

                    result += "<DIV><B>Subject:</B> " + OrgMail.Subject + " </DIV>";
                    result += "</DIV></DIV>";

                    string sOrgBodyHtml = string.IsNullOrEmpty(OrgMail.BodyHtml.Text) ? OrgMail.BodyText.Text : OrgMail.BodyHtml.Text;

                    result += sOrgBodyHtml;
                }

            }

            result += "</BODY></HTML>";

            return result;
        }

        private List<string> ParsinRequestParam(string req)
        {
            var result = new List<string>();

            if (!string.IsNullOrEmpty(req))
            {

                var _temp = req;
                var tag = string.Empty;

                // 取得 Keyword poisition
                var left_index = _temp.IndexOf('=');
                var right_index = _temp.IndexOf('&');


                // 表示最後一筆 request 參數
                if (right_index < 0)
                {
                    tag = _temp.Substring(left_index);
                    result.Add(tag.Replace("=", "").Replace("&", ""));
                    return result;
                }
                else
                {
                    // 計算 擷取字串長度
                    var offset = (right_index - left_index) + 1;

                    tag = _temp.Substring(left_index, offset);
                    result.Add(tag.Replace("=", "").Replace("&", ""));
                }



                // 清掉這次的 Tag 文字
                var subTemp = _temp.Substring(right_index + 1);

                result.AddRange(ParsinRequestParam(subTemp));

            }


            return result;
        }
        #endregion OTHER
    }
}
