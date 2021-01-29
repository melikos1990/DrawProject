using System;
using System.Collections.Generic;
using System.Reflection;
using System.Transactions;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Case;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.IO;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Domain.Transaction;
using SMARTII.Resource.Tag;
using SMARTII.Service.Organization.Provider;

namespace SMARTII.Service.Case.Service
{
    public class CaseAssignmentService : ICaseAssignmentService
    {
        private readonly ICommonAggregate _CommonAggregate;
        private readonly ICaseAggregate _CaseAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly INotificationAggregate _NotificationAggregate;
        private readonly ICaseAssignmentFacade _CaseAssignmentFacade;
        private readonly HeaderQuarterNodeProcessProvider _HeaderQuarterNodeProcessProvider;

        public CaseAssignmentService(
            ICommonAggregate CommonAggregate,
            ICaseAggregate CaseAggregate,
            IOrganizationAggregate OrganizationAggregate,
            INotificationAggregate NotificationAggregate,
            ICaseAssignmentFacade CaseAssignmentFacade,
            HeaderQuarterNodeProcessProvider HeaderQuarterNodeProcessProvider)
        {
            _CommonAggregate = CommonAggregate;
            _CaseAggregate = CaseAggregate;
            _OrganizationAggregate = OrganizationAggregate;
            _NotificationAggregate = NotificationAggregate;
            _CaseAssignmentFacade = CaseAssignmentFacade;
            _HeaderQuarterNodeProcessProvider = HeaderQuarterNodeProcessProvider;
        }

        /// <summary>
        /// 更新轉派 (從案件)
        /// </summary>
        /// <param name="assignment"></param>
        /// <returns></returns>
        public CaseAssignment UpdateComplete(CaseAssignment assignment)
        {

            // 填充待更新物件
            assignment.UpdateDateTime = DateTime.Now;
            assignment.UpdateUserName = ContextUtility.GetUserIdentity()?.Name;
            assignment.CaseAssignmentConcatUsers?.ForEach(x =>
            {
                x.CaseID = assignment.CaseID;
                x.AssignmentID = assignment.AssignmentID;
            });
            assignment.CaseAssignmentUsers?.ForEach(x =>
            {

                x.CaseID = assignment.CaseID;
                x.AssignmentID = assignment.AssignmentID;
            });

            using (var scope = TrancactionUtility.TransactionScope())
            {
                new FileProcessInvoker(context =>
                {
                    // 更新轉派對象/通知對象
                    _CaseAssignmentFacade.BatchModifyUsers(assignment);

                    // 案件副檔
                    var pathArray = FileSaverUtility.SaveCaseAssignmentFiles(context, assignment, assignment.Files);
                    assignment.FilePath = pathArray?.ToArray();

                    // 結案副檔
                    var finishPathArray = FileSaverUtility.SaveCaseAssignmentFiles(context, assignment, assignment.FinishFiles);
                    assignment.FinishFilePath = finishPathArray?.ToArray();

                    var entity = AutoMapper.Mapper.Map<CASE_ASSIGNMENT>(assignment);

                    var con = new MSSQLCondition<CASE_ASSIGNMENT>(
                        x => x.ASSIGNMENT_ID == assignment.AssignmentID);

                    con.ActionModify(x =>
                    {
                        x.NOTIFICATION_BEHAVIORS = entity.NOTIFICATION_BEHAVIORS;
                        x.NOTICE_DATETIME = entity.NOTICE_DATETIME;
                        x.FINISH_CONTENT = entity.FINISH_CONTENT;
                        x.FINISH_FILE_PATH = x.FINISH_FILE_PATH.InsertArraySerialize(finishPathArray.ToArray());
                        x.FINISH_USERNAME = entity.FINISH_USERNAME;
                        x.FINISH_DATETIME = entity.FINISH_DATETIME;
                        x.FILE_PATH = x.FILE_PATH.InsertArraySerialize(pathArray?.ToArray());
                        x.CONTENT = entity.CONTENT;
                        x.UPDATE_DATETIME = entity.UPDATE_DATETIME;
                        x.REJECT_TYPE = entity.REJECT_TYPE;
                        x.CASE_ASSIGNMENT_TYPE = entity.CASE_ASSIGNMENT_TYPE;
                        x.UPDATE_USERNAME = entity.UPDATE_USERNAME;
                        x.UPDATE_DATETIME = entity.UPDATE_DATETIME;

                    });

                    assignment = _CaseAggregate.CaseAssignment_T1_T2_.Update(con);
                });

                scope.Complete();
            };

            return assignment;
        }

        /// <summary>
        /// 更新轉派 (從轉派)
        /// </summary>
        /// <param name="assignment"></param>
        /// <returns></returns>
        public CaseAssignment Update(CaseAssignment assignment, JobPosition jobPosition = null)
        {
   
            using (var scope = TrancactionUtility.TransactionScope())
            {
                new FileProcessInvoker(context =>
                {


                    // 如果有新增回應 , 則需要新增
                    if (string.IsNullOrEmpty(assignment.ReplyContent) == false)
                    {
                        var resume = new CaseAssignmentResume()
                        {
                            CaseAssignmentID = assignment.AssignmentID,
                            CaseAssignmentType = assignment.CaseAssignmentType,
                            CaseID = assignment.CaseID,
                            Content = SysCommon_lang.CASE_ASSIGNMENT_REPLY_RESUME + assignment.ReplyContent,
                            CreateDateTime = DateTime.Now,
                            CreateUserName = ContextUtility.GetUserIdentity()?.Name,
                            CreateOrganizationType = jobPosition?.OrganizationType,
                            CreateNodeName = jobPosition?.NodeName,
                            CreateNodeID = jobPosition?.NodeID,
                            IsReply = jobPosition != null,
                        };

                        _CaseAggregate.CaseAssignmentResume_T1_T2_.Add(resume);
                    }

                    // 案件副檔
                    var pathArray = FileSaverUtility.SaveCaseAssignmentFiles(context, assignment, assignment.Files);
                    assignment.FilePath = pathArray?.ToArray();

                    // 結案副檔
                    var finishPathArray = FileSaverUtility.SaveCaseAssignmentFiles(context, assignment, assignment.FinishFiles);
                    assignment.FinishFilePath = finishPathArray?.ToArray();

                    var entity = AutoMapper.Mapper.Map<CASE_ASSIGNMENT>(assignment);

                    var con = new MSSQLCondition<CASE_ASSIGNMENT>(
                        x => x.ASSIGNMENT_ID == assignment.AssignmentID && 
                             x.CASE_ID == assignment.CaseID);

                    con.ActionModify(x =>
                    {
                        x.NOTIFICATION_BEHAVIORS = entity.NOTIFICATION_BEHAVIORS;
                        x.NOTICE_DATETIME = entity.NOTICE_DATETIME;
                        x.FINISH_CONTENT = entity.FINISH_CONTENT;
                        x.FINISH_FILE_PATH = x.FINISH_FILE_PATH.InsertArraySerialize(finishPathArray.ToArray());
                        x.FINISH_USERNAME = entity.FINISH_USERNAME;
                        x.FINISH_DATETIME = entity.FINISH_DATETIME;
                        x.FINISH_NODE_ID = entity.FINISH_NODE_ID;
                        x.FINISH_NODE_NAME = entity.FINISH_NODE_NAME;
                      
                        x.FINISH_ORGANIZATION_TYPE = entity.FINISH_ORGANIZATION_TYPE;
                        x.FILE_PATH = x.FILE_PATH.InsertArraySerialize(pathArray?.ToArray());
                        x.CONTENT = entity.CONTENT;
                        x.CASE_ASSIGNMENT_TYPE = entity.CASE_ASSIGNMENT_TYPE;
                        x.UPDATE_DATETIME = entity.UPDATE_DATETIME;
                        x.REJECT_TYPE = entity.REJECT_TYPE;
                        x.REJECT_REASON = entity.REJECT_REASON;
                        x.UPDATE_USERNAME = entity.UPDATE_USERNAME;
                        x.UPDATE_DATETIME = entity.UPDATE_DATETIME;

                    });

                    assignment = _CaseAggregate.CaseAssignment_T1_T2_.Update(con);

                

                });

                scope.Complete();
            };


            return assignment;
        }

        /// <summary>
        /// 更新反應單資訊
        /// </summary>
        /// <param name="invoice"></param>
        /// <returns></returns>
        public CaseAssignmentComplaintInvoice UpdateInvoice(CaseAssignmentComplaintInvoice invoice)
        {

            using (var scope = new TransactionScope(
            TransactionScopeOption.Required,
            TransactionScopeAsyncFlowOption.Enabled))
            {
                new FileProcessInvoker(context =>
                {

                    // 案件副檔
                    var pathArray = FileSaverUtility.SaveAssignmentInvoiceFiles(context, invoice);
                    invoice.FilePath = pathArray?.ToArray();


                    var entity = AutoMapper.Mapper.Map<CASE_ASSIGNMENT_COMPLAINT_INVOICE>(invoice);

                    var con = new MSSQLCondition<CASE_ASSIGNMENT_COMPLAINT_INVOICE>(
                        x => x.ID == invoice.ID);

                    con.ActionModify(x =>
                    {

                        x.FILE_PATH = x.FILE_PATH.InsertArraySerialize(pathArray?.ToArray());
                        x.CONTENT = entity.CONTENT;

                    });

                    invoice = _CaseAggregate.CaseAssignmentComplaintInvoice_T1_T2_.Update(con);
                });

                scope.Complete();
            };

            return invoice;

        }

        /// <summary>
        /// 更新通知一般通知資訊
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        public CaseAssignmentComplaintNotice UpdateNotice(CaseAssignmentComplaintNotice notice)
        {
            using (var scope = new TransactionScope(
                 TransactionScopeOption.Required,
                 TransactionScopeAsyncFlowOption.Enabled))
            {
                new FileProcessInvoker(context =>
                {

                    // 案件副檔
                    var pathArray = FileSaverUtility.SaveAssignmentNoticeFiles(context, notice);
                    notice.FilePath = pathArray?.ToArray();


                    var entity = AutoMapper.Mapper.Map<CASE_ASSIGNMENT_COMPLAINT_NOTICE>(notice);

                    var con = new MSSQLCondition<CASE_ASSIGNMENT_COMPLAINT_NOTICE>(
                        x => x.ID == notice.ID);

                    con.ActionModify(x =>
                    {

                        x.FILE_PATH = x.FILE_PATH.InsertArraySerialize(pathArray?.ToArray());
                        x.CONTENT = entity.CONTENT;

                    });

                    notice = _CaseAggregate.CaseAssignmentComplaintNotice_T1_T2_.Update(con);
                });

                scope.Complete();
            };

            return notice;
        }



    }
}
