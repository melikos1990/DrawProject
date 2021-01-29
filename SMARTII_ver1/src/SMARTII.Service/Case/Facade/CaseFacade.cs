#define Test

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Autofac.Features.Indexed;
using MoreLinq;
using MultipartDataMediaFormatter.Infrastructure;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Case;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.IO;
using SMARTII.Domain.Master;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Organization;
using SMARTII.Domain.System;
using SMARTII.Domain.Report;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;
using SMARTII.Service.Cache;
using SMARTII.Service.Case.Flow;
using SMARTII.Service.Case.Parser;
using SMARTII.Service.Case.Strategy;
using SMARTII.Service.Organization.Provider;
using SMARTII.Domain.Notification.Email;
using SMARTII.Service.Master.Resolver;
using System.Reflection;
using Newtonsoft.Json;
using SMARTII.Service.Notification.Base;

namespace SMARTII.Service.Case.Facade
{
    public class CaseFacade : ICaseFacade
    {
        private static int _Seed = 1;
        private static object _Lock = new object();

        private readonly IIndex<string, IFlow> _Flows;
        private readonly IIndex<string, ICaseAssignmentHSIntegrationData> _CaseAssignmentHSIntegrationData;
        private readonly IIndex<string, ICaseAssignmentCallCenterIntegrationData> _CaseAssignmentIntegrationData;
        private readonly IIndex<string, ICaseAssignmentVendorIntegrationData> _CaseAssignmentVendorIntegrationData;
        private readonly ICaseAggregate _CaseAggregate;
        private readonly IMasterAggregate _MasterAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly INotificationAggregate _NotificationAggregate;
        private readonly ISystemAggregate _SystemAggregate;
        private readonly ICommonAggregate _CommonAggregate;
        private readonly HeaderQuarterNodeProcessProvider _HeaderQuarterNodeProcessProvider;
        private readonly QuestionClassificationResolver _QuestionClassificationResolver;
        private readonly INotificationPersonalFacade _NotificationPersonalFacade;

        public CaseFacade(IIndex<string, IFlow> Flows,
                          IIndex<string, ICaseAssignmentCallCenterIntegrationData> CaseAssignmentIntegrationData,
                          IIndex<string, ICaseAssignmentHSIntegrationData> CaseAssignmentHSIntegrationData,
                          IIndex<string, ICaseAssignmentVendorIntegrationData> CaseAssignmentVendorIntegrationData,
                          ICaseAggregate CaseAggregate,
                          IMasterAggregate MasterAggregate,
                          IOrganizationAggregate OrganizationAggregate,
                          INotificationAggregate NotificationAggregate,
                          ISystemAggregate SystemAggregate,
                          ICommonAggregate CommonAggregate,
                          HeaderQuarterNodeProcessProvider HeaderQuarterNodeProcessProvider,
                          QuestionClassificationResolver QuestionClassificationResolver,
                          INotificationPersonalFacade NotificationPersonalFacade)
        {
            _Flows = Flows;
            _CaseAggregate = CaseAggregate;
            _MasterAggregate = MasterAggregate;
            _OrganizationAggregate = OrganizationAggregate;
            _NotificationAggregate = NotificationAggregate;
            _CaseAssignmentIntegrationData = CaseAssignmentIntegrationData;
            _SystemAggregate = SystemAggregate;
            _HeaderQuarterNodeProcessProvider = HeaderQuarterNodeProcessProvider;
            _CaseAssignmentHSIntegrationData = CaseAssignmentHSIntegrationData;
            _CaseAssignmentVendorIntegrationData = CaseAssignmentVendorIntegrationData;
            _CommonAggregate = CommonAggregate;
            _QuestionClassificationResolver = QuestionClassificationResolver;
            _NotificationPersonalFacade = NotificationPersonalFacade;
        }


        bool isNew(int id) => id == 0;


        /// <summary>
        /// 取得序號 , 並更新滾號檔
        /// </summary>
        /// <param name="nodeKey"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public string GetCaseCode(string nodeKey, DateTime? date = null)
        {
            var result = string.Empty;

            date = date ?? DateTime.Now.Date;

            lock (_Lock)
            {
                var key = date.Value.ToString("yyMMdd");

                _CaseAggregate.CaseCode_T1_.Operator(x =>
                {
                    var context = (SMARTIIEntities)x;

                    var query = context.CASE_CODE
                                       .FirstOrDefault(g => g.DATE == key && g.BU_CODE == nodeKey);

                    if (query == null)
                    {
                        context.CASE_CODE.Add(new CASE_CODE()
                        {
                            BU_CODE = nodeKey,
                            DATE = key,
                            SERIAL_CODE = _Seed
                        });

                        result = GeneratorCode(nodeKey, key, _Seed);

                        context.SaveChanges();
                    }
                    else
                    {
                        query.SERIAL_CODE = query.SERIAL_CODE + 1;

                        context.SaveChanges();

                        result = GeneratorCode(query.BU_CODE, query.DATE, query.SERIAL_CODE);
                    }

                });
            }

            return result;
        }

        /// <summary>
        /// 更新/新增案件標籤
        /// 於案件更新/新增時 , 需要對標籤同步進行維護
        /// </summary>
        /// <returns></returns>
        public void UpdateOrCreateTags(Domain.Case.Case @case)
        {


            if (@case.CaseTags == null) return;

            // 依照是否既有的 , 進行更新或是新增 * true = "新增的tag(於畫面直接新增)"  false = "既有的Tag" *
            var createNorUpdate = @case.CaseTags
                                       .ToLookup(x => x.Target);

            // 找到準備新增的 , 並回填資料
            createNorUpdate[true].ForEach(x => RefillTag(x, @case));

            createNorUpdate[true]?.ToList().ForEach(x =>
            {
                var isExist = _MasterAggregate.CaseTag_T1_T2_.HasAny(y => y.NAME == x.Name && y.NODE_ID == x.NodeID);

                if (isExist)
                    throw new Exception(string.Format(Case_lang.CASE_TAG_NAME_REPEAT, x.Name));
            });

            using (var scope = new TransactionScope(
                TransactionScopeOption.Required,
                TransactionScopeAsyncFlowOption.Enabled))
            {

                var ids = new List<int>();

                // 進行新增
                var newTags = _MasterAggregate.CaseTag_T1_T2_.AddRange(createNorUpdate[true]?.ToList());

                // 新增完後 , 得到ID回填 
                // 準備更新回案件上 。
                ids.AddRange(newTags.Select(x => x.ID));

                var oldTags = createNorUpdate[false]?.ToList();

                ids.AddRange(oldTags.Select(x => x.ID));

                _CaseAggregate.Case_T1_T2_.Operator(x =>
                {
                    var context = (SMARTIIEntities)x;

                    var existCase = context.CASE
                                           .Include("CASE_TAG")
                                           .FirstOrDefault(g => g.CASE_ID == @case.CaseID);

                    var existCaseTags = context.CASE_TAG
                                               .Where(g => ids.Contains(g.ID))
                                               .ToList();

                    existCase.CASE_TAG = existCaseTags;

                    context.SaveChanges();

                });

                scope.Complete();
            }


        }

        /// <summary>
        /// 對於案件通知對象進行異動
        /// </summary>
        /// <param name="case"></param>
        public void BatchModifyUsers(Domain.Case.Case @case)
        {

            using (var scope = new TransactionScope(
               TransactionScopeOption.Required,
               TransactionScopeAsyncFlowOption.Enabled))
            {
                // 須改為比對方式進行更新
                MigrationConcatUser(@case.CaseID, @case.CaseConcatUsers ?? new List<CaseConcatUser>());
                //_CaseAggregate.CaseConcatUser_T1_T2_.RemoveRange(g => g.CASE_ID == @case.CaseID);

                //_CaseAggregate.CaseConcatUser_T1_T2_.AddRange(@case.CaseConcatUsers ?? new List<CaseConcatUser>());

                // 須改為比對方式進行更新
                MigrationComplaintedUser(@case.CaseID, @case.CaseComplainedUsers ?? new List<CaseComplainedUser>());
                //_CaseAggregate.CaseComplainedUser_T1_T2_.RemoveRange(g => g.CASE_ID == @case.CaseID);

                //_CaseAggregate.CaseComplainedUser_T1_T2_.AddRange(@case.CaseComplainedUsers ?? new List<CaseComplainedUser>());

                scope.Complete();
            }

        }

        /// <summary>
        /// 整理案件聯絡人
        /// </summary>
        /// <param name="caseID"></param>
        /// <param name="users"></param>
        public void MigrationConcatUser(string caseID, List<CaseConcatUser> users)
        {
            var con = new MSSQLCondition<CASE_CONCAT_USER>(x => x.CASE_ID == caseID);

            var existIDs = _CaseAggregate.CaseConcatUser_T1_T2_.GetListOfSpecific(con, x => x.ID);

            var newIDs = users.Select(x => x.ID);

            var deleteIDs = existIDs.Except(newIDs);

            var addItems = users.Where(x => isNew(x.ID))
                                .ToList();
            var updateIDs = newIDs.Intersect(existIDs);


            // 移除舊的
            _CaseAggregate.CaseConcatUser_T1_T2_.RemoveRange(g => deleteIDs.Contains(g.ID));

            // 建立新的
            _CaseAggregate.CaseConcatUser_T1_T2_.AddRange(addItems);

            con.ClearFilters();

            // 更新兩邊都有的
            foreach (var updateID in updateIDs)
            {

                var updateItem = users.FirstOrDefault(x => x.ID == updateID);
                var entity = AutoMapper.Mapper.Map<CASE_CONCAT_USER>(updateItem);

                con.And(x => x.ID == updateID);

                con.ActionModify(x =>
                {

                    x.ADDRESS = entity.ADDRESS;
                    x.BU_ID = entity.BU_ID;
                    x.BU_NAME = entity.BU_NAME;
                    x.CASE_ID = entity.CASE_ID;
                    x.EMAIL = entity.EMAIL;
                    x.GENDER = entity.GENDER;
                    x.JOB_ID = entity.JOB_ID;
                    x.JOB_NAME = entity.JOB_NAME;
                    x.MOBILE = entity.MOBILE;
                    x.NODE_ID = entity.NODE_ID;
                    x.NODE_NAME = entity.NODE_NAME;
                    x.NOTIFICATION_BEHAVIOR = entity.NOTIFICATION_BEHAVIOR;
                    x.NOTIFICATION_KIND = entity.NOTIFICATION_KIND;
                    x.NOTIFICATION_REMARK = entity.NOTIFICATION_REMARK;
                    x.ORGANIZATION_TYPE = entity.ORGANIZATION_TYPE;
                    x.PARENT_PATH_NAME = entity.PARENT_PATH_NAME;
                    x.STORE_NO = entity.STORE_NO;
                    x.TELEPHONE = entity.TELEPHONE;
                    x.TELEPHONE_BAK = entity.TELEPHONE_BAK;
                    x.UNIT_TYPE = entity.UNIT_TYPE;
                    x.USER_ID = entity.USER_ID;
                    x.USER_NAME = entity.USER_NAME;

                });

                _CaseAggregate.CaseConcatUser_T1_T2_.Update(con);

                con.ClearModifies();
                con.ClearFilters();
            }


        }

        /// <summary>
        /// 整理被反應者資訊
        /// </summary>
        /// <param name="caseID"></param>
        /// <param name="users"></param>
        public void MigrationComplaintedUser(string caseID, List<CaseComplainedUser> users)
        {
            var con = new MSSQLCondition<CASE_COMPLAINED_USER>(x => x.CASE_ID == caseID);

            var existIDs = _CaseAggregate.CaseComplainedUser_T1_T2_.GetListOfSpecific(con, x => x.ID);

            var newIDs = users.Select(x => x.ID);

            var deleteIDs = existIDs.Except(newIDs);

            var addItems = users.Where(x => isNew(x.ID))
                                .ToList();

            var updateIDs = newIDs.Intersect(existIDs);


            // 移除舊的
            _CaseAggregate.CaseComplainedUser_T1_T2_.RemoveRange(g => deleteIDs.Contains(g.ID));

            // 建立新的
            _CaseAggregate.CaseComplainedUser_T1_T2_.AddRange(addItems);

            con.ClearFilters();

            // 更新兩邊都有的
            foreach (var updateID in updateIDs)
            {

                var updateItem = users.FirstOrDefault(x => x.ID == updateID);
                var entity = AutoMapper.Mapper.Map<CASE_COMPLAINED_USER>(updateItem);

                con.And(x => x.ID == updateID);

                con.ActionModify(x =>
                {

                    x.ADDRESS = entity.ADDRESS;
                    x.BU_ID = entity.BU_ID;
                    x.BU_NAME = entity.BU_NAME;
                    x.CASE_ID = entity.CASE_ID;
                    x.COMPLAINED_USER_TYPE = entity.COMPLAINED_USER_TYPE;
                    x.EMAIL = entity.EMAIL;
                    x.GENDER = entity.GENDER;
                    x.JOB_ID = entity.JOB_ID;
                    x.JOB_NAME = entity.JOB_NAME;
                    x.MOBILE = entity.MOBILE;
                    x.NODE_ID = entity.NODE_ID;
                    x.NODE_NAME = entity.NODE_NAME;
                    x.NOTIFICATION_BEHAVIOR = entity.NOTIFICATION_BEHAVIOR;
                    x.NOTIFICATION_KIND = entity.NOTIFICATION_KIND;
                    x.NOTIFICATION_REMARK = entity.NOTIFICATION_REMARK;
                    x.ORGANIZATION_TYPE = entity.ORGANIZATION_TYPE;
                    x.OWNER_JOB_NAME = entity.OWNER_JOB_NAME;
                    x.OWNER_USERNAME = entity.OWNER_USERNAME;
                    x.OWNER_USER_PHONE = entity.OWNER_USER_PHONE;
                    x.PARENT_PATH_NAME = entity.PARENT_PATH_NAME;
                    x.STORE_NO = entity.STORE_NO;
                    x.SUPERVISOR_JOB_NAME = entity.SUPERVISOR_JOB_NAME;
                    x.SUPERVISOR_NODE_NAME = entity.SUPERVISOR_NODE_NAME;
                    x.SUPERVISOR_USERNAME = entity.SUPERVISOR_USERNAME;
                    x.SUPERVISOR_USER_PHONE = entity.SUPERVISOR_USER_PHONE;
                    x.TELEPHONE = entity.TELEPHONE;
                    x.TELEPHONE_BAK = entity.TELEPHONE_BAK;
                    x.UNIT_TYPE = entity.UNIT_TYPE;
                    x.USER_ID = entity.USER_ID;
                    x.USER_NAME = entity.USER_NAME;

                });

                _CaseAggregate.CaseComplainedUser_T1_T2_.Update(con);

                con.ClearModifies();
                con.ClearFilters();
            }


        }

        /// <summary>
        /// 對案件捷案內容進行異動
        /// </summary>
        /// <param name="case"></param>
        public void BatchModifyFinishedReasons(Domain.Case.Case @case)
        {

            var ids = @case.CaseFinishReasonDatas?
                           .Select(x => x.ID) ?? new List<int>();

            _CaseAggregate.Case_T1_T2_.Operator(x =>
            {
                var context = (SMARTIIEntities)x;

                var existCase = context.CASE
                                       .Include("CASE_FINISH_REASON_DATA")
                                       .FirstOrDefault(g => g.CASE_ID == @case.CaseID);


                var existFinihedReasons = context.CASE_FINISH_REASON_DATA
                                                 .Where(g => ids.Contains(g.ID))
                                                 .ToList();


                existCase.CASE_FINISH_REASON_DATA = existFinihedReasons;

                context.SaveChanges();


            });
        }

        /// <summary>
        /// 取得應完成時間
        /// </summary>
        /// <param name="case"></param>
        /// <returns></returns>
        public DateTime GetPromiseDateTime(Domain.Case.Case @case, HeaderQuarterTerm term, DateTime announceDateTime)
        {

            // 取得工作小時
            var con = new MSSQLCondition<CASE_WARNING>(x => x.ID == @case.CaseWarningID);

            var workHour = _MasterAggregate.CaseWarning_T1_T2_
                                           .GetOfSpecific(con, x => x.WORK_HOUR);
            // 計算案件時效
            var promiseDateTime =
                new PromiseDateTimeStrategy(term).Calculator(workHour, announceDateTime);

            return promiseDateTime;
        }

        /// <summary>
        /// 取得完整案件資訊
        /// </summary>
        /// <param name="CaseID"></param>
        /// <returns></returns>
        public Domain.Case.Case GetComplete(string caseID)
        {
            var con = new MSSQLCondition<CASE>(x => x.CASE_ID == caseID);

            con.IncludeBy(x => x.CASE_ASSIGNMENT
                                .Select(g => g.CASE_ASSIGNMENT_CONCAT_USER));
            con.IncludeBy(x => x.CASE_ASSIGNMENT
                                .Select(g => g.CASE_ASSIGNMENT_USER));
            con.IncludeBy(x => x.CASE_SOURCE.CASE_SOURCE_USER);
            con.IncludeBy(x => x.CASE_ASSIGNMENT_COMPLAINT_INVOICE
                                .Select(g => g.CASE_ASSIGNMENT_COMPLAINT_INVOICE_USER));
            con.IncludeBy(x => x.CASE_ASSIGNMENT_COMPLAINT_NOTICE
                                .Select(g => g.CASE_ASSIGNMENT_COMPLAINT_NOTICE_USER));

            return _CaseAggregate.Case_T1_T2_.Get(con);

        }

        #region OTHER

        /// <summary>
        /// 刪除附件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="key"></param>
        public void DeleteFileWithUpdate(string id, string key, CaseType caseType)
        {
            var con = new MSSQLCondition<CASE>(x => x.CASE_ID == id);
            con.ActionModify(x =>
            {
                if (caseType == CaseType.Finished)
                {
                    x.FINISH_FILE_PATH = x.FILE_PATH.RemoveArraySerialize(key);
                }
                else
                {
                    x.FILE_PATH = x.FILE_PATH.RemoveArraySerialize(key);
                }

            });

            using (TransactionScope scope = new TransactionScope())
            {
                var data = _CaseAggregate.Case_T1_T2_.Update(con);

                var path = FilePathFormatted.GetCasePhysicalFilePath(
                        data.NodeID,
                        data.CreateDateTime.ToString("yyyyMMdd"),
                        data.CaseID,
                        key);

                FileUtility.DeleteFile(path);

                scope.Complete();
            }
        }

        #endregion


        #region PRIVATE

        /// <summary>
        /// 編碼規則
        /// </summary>
        /// <param name="nodeKey"></param>
        /// <param name="date"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private string GeneratorCode(string nodeKey, string dateKey, int index)
            => $"{nodeKey}{dateKey}{index.ToString().PadLeft(5, '0')}";

        /// <summary>
        /// 回填案件標籤資訊
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="case"></param>
        private void RefillTag(CaseTag tag, Domain.Case.Case @case)
        {
            tag.CreateDateTime = DateTime.Now;
            tag.CreateUserName = ContextUtility.GetUserIdentity()?.Name;
            tag.IsEnabled = true;
            tag.NodeID = @case.NodeID;
            tag.OrganizationType = @case.OrganizationType;
        }

        #endregion

        #region OFFICAL EMAIL ADOPT

        /// <summary>
        /// 檢查信件是否結案
        /// </summary>
        /// <param name="messageID"></param>
        /// <returns></returns>
        public async Task<(bool isUnclosed, OfficialEmailEffectivePayload officialEmailEffective, Domain.Case.Case @case)> CheckExistCaseIsUnclose(string messageID, int buID)
        {
            var @case = new Domain.Case.Case();

            var existData = _NotificationAggregate.OfficialEmailEffectivePayload_T1_T2_
                                                  .Get(x => x.MESSAGE_ID == messageID && x.NODE_ID == buID);

            //檢查信件是否存在
            if (existData == null)
                throw new Exception(OfficialEmail_lang.OFFICIAL_EMAIL_EMAIL_ALREADY_ADOPT);

            //無案件編號
            if (string.IsNullOrEmpty(existData.CaseID))
                return await (false, existData, @case).Async();

            var caseID = existData.CaseID.Trim();

            @case = _CaseAggregate.Case_T1_T2_
                                  .Get(x => x.CASE_ID == caseID);

            //檢查案件是否存在
            if (@case == null)
                return await (false, existData, @case).Async();

            //檢查信件與案件的BU是否一致
            if (@case.NodeID != existData.NodeID)
                throw new Exception("此信件與案件的BU歸屬有誤");

            return await ((@case.CaseType != CaseType.Finished), existData, @case).Async();

        }

        /// <summary>
        /// 檢查該人員有無此Group權限
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="users"></param>
        /// <returns></returns>
        public bool CheckGroupAuth(int groupID, params User[] users)
        {
            foreach (var user in users)
            {
                if (user == null)
                    throw new Exception(Common_lang.USER_UNDEFIND);

                bool hasAuth = user.JobPositions
                                   .Any(x => x.NodeID == groupID &&
                                             x.OrganizationType == OrganizationType.CallCenter);

                if (hasAuth == false)
                    throw new Exception(Common_lang.USER_NOT_SERVICE);
            }

            return true;
        }

        #endregion

        #region CRUD

        /// <summary>
        /// 新增案件CaseResume
        /// </summary>
        /// <param name="caseID"></param>
        /// <param name="beforeCase"></param>
        /// <param name="caseResumeContent"></param>
        /// <param name="caseHistoryPreFix"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async void CreateResume(string caseID, Domain.Case.Case beforeCase, string caseResumeContent, string caseHistoryPreFix, User user)
        {
            try
            {
                if (string.IsNullOrEmpty(caseID) || user == null)
                    throw new Exception(SysCommon_lang.CASE_INTEGRAE_NOTICE_INIT_FAIL);

                var con = new MSSQLCondition<CASE>(x => x.CASE_ID == caseID);
                con.IncludeBy(x => x.CASE_CONCAT_USER);
                con.IncludeBy(x => x.CASE_COMPLAINED_USER);
                var nowCase = _CaseAggregate.Case_T1_T2_.Get(con);

                if (nowCase == null)
                    throw new Exception(SysCommon_lang.CASE_INTEGRAE_NOTICE_GET_FAIL);

                var afterCase = (nowCase.QuestionClassificationID == 0) ? nowCase : _QuestionClassificationResolver.Resolve(nowCase);

                var caseResume = new CaseResume()
                {
                    CaseID = afterCase.CaseID,
                    CaseType = afterCase.CaseType,
                    Content = caseResumeContent,
                    CreateDateTime = DateTime.Now,
                    CreateUserName = user.Name
                };

                // 寫入差異比對
                if (string.IsNullOrEmpty(caseResumeContent))
                {
                    caseResume.Content = beforeCase.CustomerDiffFormatter(afterCase, "異動");

                    if (string.IsNullOrEmpty(caseResume.Content.Replace("\r\n", string.Empty)))
                        return;
                }

                _CaseAggregate.CaseResume_T1_T2_.Add(caseResume);
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);
                _CommonAggregate.Logger.Error(beforeCase.GetObjectContentFromDescription(MethodBase.GetCurrentMethod().Name));
            }
        }

        /// <summary>
        /// 新增案件CaseHistory
        /// </summary>
        /// <param name="caseID"></param>
        /// <param name="caseHistoryPreFix"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async void CreateHistory(string caseID, Domain.Case.Case @case, string caseHistoryPreFix, User user)
        {
            try
            {
                if (string.IsNullOrEmpty(caseID) || user == null)
                    throw new Exception(SysCommon_lang.CASE_INTEGRAE_NOTICE_INIT_FAIL);

                var con = new MSSQLCondition<CASE>(x => x.CASE_ID == caseID);
                con.IncludeBy(x => x.CASE_CONCAT_USER);
                con.IncludeBy(x => x.CASE_COMPLAINED_USER);
                con.IncludeBy(x => x.CASE_TAG);
                con.IncludeBy(x => x.CASE_ITEM);
                con.IncludeBy(x => x.CASE_FINISH_REASON_DATA);
                con.IncludeBy(x => x.CASE_ASSIGNMENT);
                con.IncludeBy(x => x.CASE_ASSIGNMENT.Select(g => g.CASE_ASSIGNMENT_CONCAT_USER));
                con.IncludeBy(x => x.CASE_ASSIGNMENT.Select(g => g.CASE_ASSIGNMENT_USER));
                con.IncludeBy(x => x.CASE_ASSIGNMENT_COMMUNICATE);
                con.IncludeBy(x => x.CASE_ASSIGNMENT_COMPLAINT_INVOICE);
                con.IncludeBy(x => x.CASE_ASSIGNMENT_COMPLAINT_INVOICE.Select(g => g.CASE_ASSIGNMENT_COMPLAINT_INVOICE_USER));
                con.IncludeBy(x => x.CASE_ASSIGNMENT_COMPLAINT_NOTICE);
                con.IncludeBy(x => x.CASE_ASSIGNMENT_COMPLAINT_NOTICE.Select(g => g.CASE_ASSIGNMENT_COMPLAINT_NOTICE_USER));
                var nowCase = _CaseAggregate.Case_T1_T2_.Get(con);

                if (nowCase == null)
                    throw new Exception(SysCommon_lang.CASE_INTEGRAE_NOTICE_GET_FAIL);

                var afterCase = (nowCase.QuestionClassificationID == 0) ? nowCase : _QuestionClassificationResolver.Resolve(nowCase);

                var caseHistory = new CaseHistory()
                {
                    CaseID = afterCase.CaseID,
                    Content = afterCase.GetObjectContentFromDescription(caseHistoryPreFix),
                    CaseType = afterCase.CaseType,
                    CreateDateTime = DateTime.Now,
                    CreateUserName = user.Name
                };

                _CaseAggregate.CaseHistory_T1_T2_.Add(caseHistory);
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);
                _CommonAggregate.Logger.Error(@case.GetObjectContentFromDescription(MethodBase.GetCurrentMethod().Name));
            }
        }

        /// <summary>
        /// 新增案件通知PersonalNotification
        /// </summary>
        /// <param name="caseID"></param>
        /// <param name="personalNotificationType"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async void CreatePersonalNotification(string caseID, PersonalNotificationType personalNotificationType, User user)
        {
            try
            {
                if (string.IsNullOrEmpty(caseID) || user == null)
                    throw new Exception(SysCommon_lang.CASE_INTEGRAE_NOTICE_INIT_FAIL);

                var con = new MSSQLCondition<CASE>(x => x.CASE_ID == caseID);
                var afterCase = _CaseAggregate.Case_T1_T2_.Get(con);

                if (afterCase == null)
                    throw new Exception(SysCommon_lang.CASE_INTEGRAE_NOTICE_GET_FAIL);

                var conNode = new MSSQLCondition<HEADQUARTERS_NODE>(x=>x.NODE_ID == afterCase.NodeID);
                var nodeName = _OrganizationAggregate.HeaderQuarterNode_T1_T2_.GetOfSpecific(conNode, node => node.NAME);

                var personalNotificationList = new List<PersonalNotification>();

                // 找出案件所屬group
                var groupNode = _OrganizationAggregate.CallCenterNode_T1_T2_.Get(x => x.NODE_ID == afterCase.GroupID);

                if (groupNode == null)
                    throw new Exception(Common_lang.GROUP_NOT_FOUND);

                if (groupNode.IsWorkProcessNotice)
                {
                    switch (groupNode.WorkProcessType)
                    {
                        case WorkProcessType.Individual:
                            //非自己時才通知
                            if (afterCase.ApplyUserID != user.UserID)
                            {
                                var personalNotificationIndividual = new PersonalNotification()
                                {
                                    UserID = afterCase.ApplyUserID,
                                    Content = string.Format(SysCommon_lang.PERSONAL_NOTICE_CASE_UPDATE, nodeName, user.Name),
                                    CreateDateTime = DateTime.Now,
                                    CreateUserName = user.Name,
                                    Extend = JsonConvert.SerializeObject(new Domain.Case.Case() { CaseID = caseID })
                                };

                                personalNotificationList.Add(personalNotificationIndividual);
                            }

                            break;

                        case WorkProcessType.Accompanied:

                            var jCon = new MSSQLCondition<NODE_JOB>(x => x.NODE_ID == afterCase.GroupID && x.ORGANIZATION_TYPE == (byte)OrganizationType.CallCenter);
                            jCon.IncludeBy(x => x.USER);
                            jCon.IncludeBy(x => x.JOB);
                            var jobsUsers = _OrganizationAggregate.JobPosition_T1_T2_.GetList(jCon).SelectMany(x => x.Users).Select(x => x.UserID).Distinct().ToList();

                            jobsUsers.ForEach(x =>
                            {
                                //非自己時才通知
                                if (x != user.UserID)
                                {
                                    var personalNotificationAccompanied = new PersonalNotification()
                                    {
                                        UserID = x,
                                        Content = string.Format(SysCommon_lang.PERSONAL_NOTICE_CASE_UPDATE, nodeName, user.Name),
                                        CreateDateTime = DateTime.Now,
                                        CreateUserName = user.Name,
                                        Extend = JsonConvert.SerializeObject(new Domain.Case.Case() { CaseID = caseID })
                                    };

                                    personalNotificationList.Add(personalNotificationAccompanied);
                                }
                            });

                            break;
                    }

                    personalNotificationList.ForEach(x =>
                    {
                        x.PersonalNotificationType = personalNotificationType;
                    });

                    _NotificationAggregate.PersonalNotification_T1_T2_.AddRange(personalNotificationList);

                    _NotificationPersonalFacade.NotifyWeb(user.UserID);

                }
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);
            }
        }

        #endregion
    }
}
