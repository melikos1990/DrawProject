#define Test

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Case;
using SMARTII.Domain.Common;
using SMARTII.Domain.IO;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Domain.Transaction;
using SMARTII.Resource.Tag;
using SMARTII.Service.Organization.Provider;

namespace SMARTII.Service.Case.Task
{
    /// <summary>
    /// 建立轉派TASK
    /// </summary>
    public class AssignTask : IFlowableTask
    {
        private readonly object _lock = new object();

        private readonly ICaseAggregate _CaseAggregate;
        private readonly ICommonAggregate _CommonAggregate;
        private readonly ICaseService _CaseService;
        private readonly ICaseAssignmentService _CaseAssignmentService;
        private readonly ICaseFacade _CaseFacade;
        private readonly ICaseAssignmentFacade _CaseAssignmentFacade;
        private readonly HeaderQuarterNodeProcessProvider _HeaderQuarterNodeProcessProvider;

        public AssignTask(ICaseAggregate CaseAggregate,
                          ICommonAggregate CommonAggregate,
                          ICaseService CaseService,
                          ICaseAssignmentService CaseAssignmentService,
                          ICaseFacade CaseFacade,
                          ICaseAssignmentFacade CaseAssignmentFacade,
                          HeaderQuarterNodeProcessProvider HeaderQuarterNodeProcessProvider)
        {
            _CaseAggregate = CaseAggregate;
            _CommonAggregate = CommonAggregate;
            _CaseService = CaseService;
            _CaseFacade = CaseFacade;
            _CaseAssignmentService = CaseAssignmentService;
            _CaseAssignmentFacade = CaseAssignmentFacade;
            _HeaderQuarterNodeProcessProvider = HeaderQuarterNodeProcessProvider;
        }

        public async Task<IFlowable> Execute(IFlowable flowable, params object[] args)
        {

            var data = (CaseAssignment)flowable;

            // 組入新增物件
            await Validator(data);

            using (var transactionscope = TrancactionUtility.NoTransactionScope())
            {
                // 發生時間 , 為了避免立案時間與相關的計算基準一致
                // 因此需先記錄在變數中。
                var announceDateTime = DateTime.Now;

                // 取得BU識別值 , 如(003 => ASO) ...
                // 此欄位為系統原則 , 若無法取得 , 則需拋出例外 。
                var term = (HeaderQuarterTerm)_HeaderQuarterNodeProcessProvider.GetTerm(
                            data.NodeID,
                            data.OrganizationType);

           

                data.CreateDateTime = DateTime.Now;
                data.CreateUserName = ContextUtility.GetUserIdentity()?.Name;
                data.CaseAssignmentType = CaseAssignmentType.Assigned;

            }

            new FileProcessInvoker(context =>
            {
                using (var scope = TrancactionUtility.TransactionScope())
                {

                

                    lock (_lock)
                    {
                        data.AssignmentID = _CaseAssignmentFacade.GetAssignmentCode(data.CaseID);

                        // 建立轉派附件
                        var pathArray = FileSaverUtility.SaveCaseAssignmentFiles(context, data, data.Files);

                        data.FilePath = pathArray?.ToArray();

                        data.CaseAssignmentConcatUsers?.ForEach(x =>
                        {
                            x.CaseID = data.CaseID;
                            x.AssignmentID = data.AssignmentID;
                        });
                        data.CaseAssignmentUsers?.ForEach(x =>
                        {
                            x.CaseID = data.CaseID;
                            x.AssignmentID = data.AssignmentID;
                        });

                        // 一般新增
                        data = _CaseAggregate.CaseAssignment_T1_T2_.Add(data);


                    }                 

                    scope.Complete();
                }


            });


            var responsibilityUsers = this.combineAssignUser(data.CaseAssignmentUsers?.Where(x => x.CaseComplainedUserType == CaseComplainedUserType.Responsibility));

            var noticeUsers = this.combineAssignUser(data.CaseAssignmentUsers?.Where(x => x.CaseComplainedUserType == CaseComplainedUserType.Notice), Common_lang.NONE);
            
            var user = ContextUtility.GetUserIdentity()?.Instance;

            // 新增案件Resume
            _CaseFacade.CreateResume(data.CaseID, null, string.Format(SysCommon_lang.CASE_ASSIGNMENT_RESUME, data.CaseID, data.AssignmentID, responsibilityUsers, noticeUsers), Case_lang.CASE_ASSIGNMENT_SAVE, user);

            // 新增轉派Resume & History
            _CaseAssignmentFacade.CreateResume(data, string.Format(SysCommon_lang.CASE_ASSIGNMENT_RESUME, data.CaseID, data.AssignmentID, responsibilityUsers, noticeUsers), user.Name);

            using (var scope = TrancactionUtility.NoTransactionScope())
            {
                global::System.Threading.Tasks.Task.Run(() =>
                {
                    _CaseAssignmentFacade.CreateHistory(data, Case_lang.CASE_ASSIGNMENT_SAVE, user.Name);
                });
            };

            #region 將案件狀態改為處理中
            var con = new MSSQLCondition<CASE>(x => x.CASE_ID == data.CaseID);

            con.ActionModify(x =>
            {
                x.CASE_TYPE = (byte)CaseType.Process;
                x.UPDATE_DATETIME = DateTime.Now;
                x.UPDATE_USERNAME = ContextUtility.GetUserIdentity().Name;

            });

            var caseResult = _CaseAggregate.Case_T1_T2_.Update(con);

            _CommonAggregate.Logger.Info("派工壓案件狀態成功====>", caseResult.CaseType);
            #endregion

            return data;
        }

        public async global::System.Threading.Tasks.Task Validator(IFlowable flowable, params object[] args)
        {
            var data = (CaseAssignment)flowable;

            if (_CaseAggregate.Case_T1_T2_.HasAny(x => x.CASE_ID == data.CaseID && x.CASE_TYPE == (byte)CaseType.Finished))
                throw new NullReferenceException(Case_lang.CASE_ALREADY_FINISH);
        }


        private string combineAssignUser(IEnumerable<CaseAssignmentUser> users, string defaultText = "")
        {

            if (users == null || !users.Any()) return defaultText;


            return string.Join(",", users.Select(x => x.UserName).ToList());
        }

    }
}
