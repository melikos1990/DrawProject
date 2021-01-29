using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Transactions;
using Newtonsoft.Json;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Case;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.IO;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Organization;
using SMARTII.Resource.Tag;
using SMARTII.Service.Master.Resolver;
using SMARTII.Service.Organization.Resolver;

namespace SMARTII.Service.Case.Facade
{
    public class CaseAssignmentFacade : ICaseAssignmentFacade
    {
        private static readonly object _Lock = new object();
        private static readonly int _Seed = 1;

        private readonly ICaseAggregate _CaseAggregate;
        private readonly ICommonAggregate _CommonAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly INotificationAggregate _NotificationAggregate;
        private readonly OrganizationNodeResolver _OrganizationNodeResolver;
        private readonly CaseRemindResolver _CaseRemindResolver;
        private readonly INotificationPersonalFacade _NotificationPersonalFacade;

        public CaseAssignmentFacade(ICaseAggregate CaseAggregate,
                                    ICommonAggregate CommonAggregate,
                                    IOrganizationAggregate OrganizationAggregate,
                                    INotificationAggregate NotificationAggregate,
                                    OrganizationNodeResolver OrganizationNodeResolver,
                                    CaseRemindResolver CaseRemindResolver,
                                    INotificationPersonalFacade NotificationPersonalFacade)
        {
            _CaseAggregate = CaseAggregate;
            _CommonAggregate = CommonAggregate;
            _OrganizationAggregate = OrganizationAggregate;
            _NotificationAggregate = NotificationAggregate;
            _OrganizationNodeResolver = OrganizationNodeResolver;
            _CaseRemindResolver = CaseRemindResolver;
            _NotificationPersonalFacade = NotificationPersonalFacade;
        }      

        /// <summary>
        /// 編碼規則 (反應單)
        /// </summary>
        /// <param name="date"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private string GeneratorInvoiceCode(string type, string buCode, string yearMonth, int index)
            => $"{type}{buCode}{yearMonth}{index.ToString().PadLeft(3, '0')}";

        /// <summary>
        ///  取得來源編號 , 並更新滾號檔
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public string GetInvoiceCode(string type, string buCode, DateTime? date = null)
        {
            var result = string.Empty;

            date = date ?? DateTime.Now.Date;

            lock (_Lock)
            {
                var key = date.Value.ToString("yyMM");

                _CaseAggregate.CaseAssignmentComplaintInvoiceCode_T1_.Operator(x =>
              {
                  var context = (SMARTIIEntities)x;

                  var query = context.CASE_ASSIGNMENT_COMPLAINT_INVOICE_CODE
                                     .FirstOrDefault(g => g.DATE == key &&
                                                          g.BU_CODE == buCode);

                  if (query == null)
                  {
                      context.CASE_ASSIGNMENT_COMPLAINT_INVOICE_CODE.Add(new CASE_ASSIGNMENT_COMPLAINT_INVOICE_CODE()
                      {
                          DATE = key,
                          BU_CODE = buCode,
                          SERIAL_CODE = _Seed
                      });

                      result = GeneratorInvoiceCode(type, buCode, key, _Seed);

                      context.SaveChanges();
                  }
                  else
                  {
                      query.SERIAL_CODE = query.SERIAL_CODE + 1;

                      context.SaveChanges();

                      result = GeneratorInvoiceCode(type, buCode, query.DATE, query.SERIAL_CODE);
                  }
              });
            }

            return result;
        }

        /// <summary>
        /// 取得轉派流水號
        /// </summary>
        /// <param name="caseID"></param>
        /// <returns></returns>
        public int GetAssignmentCode(string caseID)
        {

            var con = new MSSQLCondition<CASE_ASSIGNMENT>(x => x.CASE_ID == caseID);
            var list = _CaseAggregate.CaseAssignment_T1_T2_.GetListOfSpecific(con, x => x.CASE_ID);

            if (list == null || list.Any() == false)
            {
                return _Seed;
            }
            else
            {
                return (list.Count + 1);
            }

        }

        /// <summary>
        /// 批次異動人員
        /// </summary>
        /// <param name="caseAssignment"></param>
        public void BatchModifyUsers(CaseAssignment caseAssignment)
        {
            using (var scope = new TransactionScope(
             TransactionScopeOption.Required,
             TransactionScopeAsyncFlowOption.Enabled))
            {
                // 須改為比對方式進行更新
                _CaseAggregate.CaseAssignmentUser_T1_T2_.RemoveRange(g => g.ASSIGNMENT_ID == caseAssignment.AssignmentID);

                _CaseAggregate.CaseAssignmentUser_T1_T2_.AddRange(caseAssignment.CaseAssignmentUsers);

                // 須改為比對方式進行更新
                _CaseAggregate.CaseAssignmentConcatUser_T1__T1_T2_.RemoveRange(g => g.ASSIGNMENT_ID == caseAssignment.AssignmentID);

                _CaseAggregate.CaseAssignmentConcatUser_T1__T1_T2_.AddRange(caseAssignment.CaseAssignmentConcatUsers);

                scope.Complete();
            }
        }

        /// <summary>
        /// 取得歷程清單
        /// -> 反應單
        /// -> 轉派
        /// -> 一般通知
        /// -> 一般溝通
        /// </summary>
        /// <param name="caseID"></param>
        /// <returns></returns>
        public List<CaseAssignmentBase> GetAssignmentAggregate(string caseID)
        {
            var result = new List<CaseAssignmentBase>();

            var con = new MSSQLCondition<CASE_ASSIGNMENT>(x => x.CASE_ID == caseID);
            con.IncludeBy(x => x.CASE_ASSIGNMENT_USER);

            var cons = new MSSQLCondition<CASE_ASSIGNMENT_COMPLAINT_INVOICE>(x => x.CASE_ID == caseID);
            cons.IncludeBy(x => x.CASE_ASSIGNMENT_COMPLAINT_INVOICE_USER);
            var conn = new MSSQLCondition<CASE_ASSIGNMENT_COMPLAINT_NOTICE>(x => x.CASE_ID == caseID);
            conn.IncludeBy(x => x.CASE_ASSIGNMENT_COMPLAINT_NOTICE_USER);

            var coni = new MSSQLCondition<CASE_ASSIGNMENT_COMMUNICATE>(x => x.CASE_ID == caseID);
            

            var assignments = _CaseRemindResolver.ResolveCollection(_CaseAggregate.CaseAssignment_T1_T2_.GetList(con));
            var invoices = _CaseAggregate.CaseAssignmentComplaintInvoice_T1_T2_.GetList(cons);
            var notices = _CaseAggregate.CaseAssignmentComplaintNotice_T1_T2_.GetList(conn);
            var communicated = _CaseAggregate.CaseAssignmentCommunicate_T1_T2_.GetList(coni);

            result.AddRange(assignments ?? new List<CaseAssignment>());
            result.AddRange(invoices ?? new List<CaseAssignmentComplaintInvoice>());
            result.AddRange(notices ?? new List<CaseAssignmentComplaintNotice>());
            result.AddRange(communicated ?? new List<CaseAssignmentCommunicate>());

            return result;

        }



        #region OTHER

        /// <summary>
        /// 刪除反應單附件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="key"></param>
        public void DeleteInvoiceFileWithUpdate(int id, string key)
        {
            var con = new MSSQLCondition<CASE_ASSIGNMENT_COMPLAINT_INVOICE>(x => x.ID == id);
            con.ActionModify(x =>
            {
                x.FILE_PATH = x.FILE_PATH.RemoveArraySerialize(key);

            });

            using (TransactionScope scope = new TransactionScope())
            {
                var data = _CaseAggregate.CaseAssignmentComplaintInvoice_T1_T2_.Update(con);

                var path = FilePathFormatted.GetCaseComplaintInvoicePhysicalFilePath(
                        data.NodeID,
                        data.CreateDateTime.ToString("yyyyMMdd"),
                        data.CaseID,
                        data.InvoiceID,
                        key);

                FileUtility.DeleteFile(path);

                scope.Complete();
            }
        }

        /// <summary>
        /// 刪除一般通知附件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="key"></param>
        public void DeleteNoticeFileWithUpdate(int id, string key)
        {
            var con = new MSSQLCondition<CASE_ASSIGNMENT_COMPLAINT_NOTICE>(x => x.ID == id);
            con.ActionModify(x =>
            {
                x.FILE_PATH = x.FILE_PATH.RemoveArraySerialize(key);

            });

            using (TransactionScope scope = new TransactionScope())
            {
                var data = _CaseAggregate.CaseAssignmentComplaintNotice_T1_T2_.Update(con);

                var path = FilePathFormatted.GetCaseComplaintNoticePhysicalFilePath(
                        data.NodeID,
                        data.CreateDateTime.ToString("yyyyMMdd"),
                        data.CaseID,
                        data.ID,
                        key);

                FileUtility.DeleteFile(path);

                scope.Complete();
            }
        }
        
        /// <summary>
        /// 刪除轉派附件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="key"></param>
        public void DeleteAssignmentFinishFileWithUpdate(string caseID, int id, string key)
        {
            var con = new MSSQLCondition<CASE_ASSIGNMENT>(x => x.ASSIGNMENT_ID == id &&  
                                                               x.CASE_ID == caseID);
            con.ActionModify(x =>
            {
                x.FINISH_FILE_PATH = x.FILE_PATH.RemoveArraySerialize(key);

            });

            using (TransactionScope scope = new TransactionScope())
            {
                var data = _CaseAggregate.CaseAssignment_T1_T2_.Update(con);

                var path = FilePathFormatted.GetCaseAssignmentPhysicalFilePath(
                        data.NodeID,
                        data.CreateDateTime.ToString("yyyyMMdd"),
                        data.CaseID,
                        data.AssignmentID,
                        key);

                FileUtility.DeleteFile(path);

                scope.Complete();
            }
        }

        #endregion

        #region CRUD

        /// <summary>
        /// 新增轉派歷程CaseAssignmentResume
        /// </summary>
        /// <param name="beforeCaseAssignment"></param>
        /// <param name="caseAssignmentResumeContent"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async void CreateResume(CaseAssignment beforeCaseAssignment, string caseAssignmentResumeContent, string userName, JobPosition jobPosition = null)
        {
            try
            {
                if (beforeCaseAssignment == null)
                    throw new Exception(SysCommon_lang.CASE_ASSIGNMENT_INTEGRAE_NOTICE_INIT_FAIL);

                var con = new MSSQLCondition<CASE_ASSIGNMENT>(x => x.CASE_ID == beforeCaseAssignment.CaseID && x.ASSIGNMENT_ID == beforeCaseAssignment.AssignmentID);

                var afterCaseAssignment = _CaseAggregate.CaseAssignment_T1_T2_.Get(con);

                if (afterCaseAssignment == null)
                    throw new Exception(SysCommon_lang.CASE_ASSIGNMENT_INTEGRAE_NOTICE_GET_FAIL);

                var caseAssignmentResume = new CaseAssignmentResume()
                {
                    CaseID = afterCaseAssignment.CaseID,
                    CaseAssignmentID = afterCaseAssignment.AssignmentID,
                    CaseAssignmentType = afterCaseAssignment.CaseAssignmentType,
                    Content = caseAssignmentResumeContent,
                    CreateDateTime = DateTime.Now,
                    CreateUserName = userName,
                    CreateOrganizationType = jobPosition?.OrganizationType,
                    CreateNodeName = jobPosition?.NodeName,
                    CreateNodeID = jobPosition?.NodeID,
                    IsReply = jobPosition != null,
                };

                _CaseAggregate.CaseAssignmentResume_T1_T2_.Add(caseAssignmentResume);

            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);
                _CommonAggregate.Logger.Error(beforeCaseAssignment.GetObjectContentFromDescription(MethodBase.GetCurrentMethod().Name));
            }
        }

        /// <summary>
        /// 新增轉派CaseAssignmentHistory
        /// </summary>
        /// <param name="caseAssignment"></param>
        /// <param name="caseAssignmentHistoryPreFix"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async void CreateHistory(CaseAssignment caseAssignment, string caseAssignmentHistoryPreFix, string userName)
        {
            try
            {
                if (caseAssignment == null)
                    throw new Exception(SysCommon_lang.CASE_ASSIGNMENT_INTEGRAE_NOTICE_INIT_FAIL);

                var con = new MSSQLCondition<CASE_ASSIGNMENT>(x => x.CASE_ID == caseAssignment.CaseID && x.ASSIGNMENT_ID == caseAssignment.AssignmentID);
                con.IncludeBy(x => x.CASE_ASSIGNMENT_CONCAT_USER);
                con.IncludeBy(x => x.CASE_ASSIGNMENT_USER);
                var afterCaseAssignment = _CaseAggregate.CaseAssignment_T1_T2_.Get(con);

                if (afterCaseAssignment == null)
                    throw new Exception(SysCommon_lang.CASE_ASSIGNMENT_INTEGRAE_NOTICE_GET_FAIL);

                var caseAssignmentHistory = new CaseAssignmentHistory()
                {
                    CaseID = afterCaseAssignment.CaseID,
                    AssignemtID = afterCaseAssignment.AssignmentID,
                    Content = afterCaseAssignment.GetObjectContentFromDescription(caseAssignmentHistoryPreFix),
                    CaseAssignmentType = afterCaseAssignment.CaseAssignmentType,
                    CreateDateTime = DateTime.Now,
                    CreateUserName = userName
                };

                _CaseAggregate.CaseAssignmentHistory_T1_T2_.Add(caseAssignmentHistory);

            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);
                _CommonAggregate.Logger.Error(caseAssignment.GetObjectContentFromDescription(MethodBase.GetCurrentMethod().Name));
            }
        }

        /// <summary>
        /// 轉派銷案通知PersonalNotification
        /// </summary>
        /// <param name="caseAssignment"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async void CreatePersonalNotification(CaseAssignment caseAssignment, string userName, string content = null)
        {
            try
            {
                if (caseAssignment == null)
                    throw new Exception(SysCommon_lang.CASE_ASSIGNMENT_INTEGRAE_NOTICE_INIT_FAIL);

                var newCaseAssignment = _OrganizationNodeResolver.Resolve(caseAssignment);

                var con = new MSSQLCondition<CASE>(x => x.CASE_ID == newCaseAssignment.CaseID);
                var afterCase = _CaseAggregate.Case_T1_T2_.Get(con);

                if (afterCase == null)
                    throw new Exception(SysCommon_lang.CASE_ASSIGNMENT_INTEGRAE_NOTICE_GET_FAIL);

                var personalNotificationList = new List<PersonalNotification>();
                var notifyUsers = new List<string>();

                // 找出案件所屬group
                var groupNode = _OrganizationAggregate.CallCenterNode_T1_T2_.Get(x => x.NODE_ID == afterCase.GroupID);

                if (groupNode == null)
                    throw new Exception(Common_lang.GROUP_NOT_FOUND);

                if (groupNode.IsWorkProcessNotice)
                {
                    switch (groupNode.WorkProcessType)
                    {
                        case WorkProcessType.Individual:

                            var personalNotificationIndividual = new PersonalNotification()
                            {
                                UserID = afterCase.ApplyUserID,
                                Content = content ?? "",
                                CreateDateTime = DateTime.Now,
                                CreateUserName = userName,
                                PersonalNotificationType = PersonalNotificationType.CaseAssignmentFinish,
                                Extend = JsonConvert.SerializeObject(newCaseAssignment)
                            };

                            notifyUsers.Add(afterCase.ApplyUserID);
                            personalNotificationList.Add(personalNotificationIndividual);

                            break;

                        case WorkProcessType.Accompanied:

                            var jCon = new MSSQLCondition<NODE_JOB>(x => x.NODE_ID == afterCase.GroupID && x.ORGANIZATION_TYPE == (byte)OrganizationType.CallCenter);
                            jCon.IncludeBy(x => x.USER);
                            jCon.IncludeBy(x => x.JOB);
                            var jobsUsers = _OrganizationAggregate.JobPosition_T1_T2_.GetList(jCon).SelectMany(x => x.Users).Select(x => x.UserID).Distinct().ToList();

                            jobsUsers.ForEach(x =>
                            {
                                var personalNotificationAccompanied = new PersonalNotification()
                                {
                                    UserID = x,
                                    Content = content ?? "",
                                    CreateDateTime = DateTime.Now,
                                    CreateUserName = userName,
                                    PersonalNotificationType = PersonalNotificationType.CaseAssignmentFinish,
                                    Extend = JsonConvert.SerializeObject(newCaseAssignment)
                                };


                                notifyUsers.Add(x);
                                personalNotificationList.Add(personalNotificationAccompanied);
                            });

                            break;
                    }

                    _NotificationAggregate.PersonalNotification_T1_T2_.AddRange(personalNotificationList);

                    _NotificationPersonalFacade.NotifyWebCollection(notifyUsers);
                }

            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);
                _CommonAggregate.Logger.Error(caseAssignment.GetObjectContentFromDescription(MethodBase.GetCurrentMethod().Name));
            }
        }

        #endregion
    }
}
