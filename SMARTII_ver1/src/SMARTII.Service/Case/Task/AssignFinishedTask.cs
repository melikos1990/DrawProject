#define Test

using System;
using System.Threading.Tasks;
using System.Transactions;
using SMARTII.Domain.Case;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Domain.Transaction;
using SMARTII.Resource.Tag;

namespace SMARTII.Service.Case.Task
{
    /// <summary>
    /// 轉派銷案TASK
    /// </summary>
    public class AssignFinishedTask : IFlowableTask
    {
        private readonly ICaseAggregate _CaseAggregate;
        private readonly ICaseAssignmentService _CaseAssignmentService;
        private readonly ICaseService _CaseService;
        private readonly ICaseFacade _CaseFacade;
        private readonly ICaseAssignmentFacade _CaseAssignmentFacade;

        public AssignFinishedTask(ICaseAggregate CaseAggregate,
                                  ICaseAssignmentService CaseAssignmentService,
                                  ICaseService CaseService,
                                  ICaseFacade CaseFacade,
                                  ICaseAssignmentFacade CaseAssignmentFacade)
        {
            _CaseAggregate = CaseAggregate;
            _CaseAssignmentService = CaseAssignmentService;
            _CaseService = CaseService;
            _CaseFacade = CaseFacade;
            _CaseAssignmentFacade = CaseAssignmentFacade;
        }

        public async Task<IFlowable> Execute(IFlowable flowable, params object[] args)
        {
            var data = (CaseAssignment)flowable;

            await Validator(data, args);

            // 組合更新欄位
            data.CaseAssignmentType = CaseAssignmentType.Finished;
            
            using (var scope = TrancactionUtility.TransactionScope())
            {
                // 一般更新
                data = _CaseAssignmentService.Update(data);

                // TODO : 寫入結案通知

                scope.Complete();
            }

            var user = ContextUtility.GetUserIdentity()?.Instance;
            // 新增歷程後記錄
            _CaseFacade.CreateResume(data.CaseID, null, string.Format(SysCommon_lang.CASE_ASSIGNMENT_CC_FINISH_RESUME, data.CaseID, data.AssignmentID), Case_lang.CASE_ASSIGNMENT_CC_FINISH, user);

            // 新增轉派Resume & History
            _CaseAssignmentFacade.CreateResume(data, Case_lang.CASE_ASSIGNMENT_CC_FINISH, user.Name);
            using (var scope = TrancactionUtility.NoTransactionScope())
            {
                global::System.Threading.Tasks.Task.Run(() => 
                {
                    _CaseAssignmentFacade.CreateHistory(data, Case_lang.CASE_ASSIGNMENT_CC_FINISH, user.Name);
                });
            };

            return data;
        }

        public async global::System.Threading.Tasks.Task Validator(IFlowable flowable, params object[] args)
        {
            var data = (CaseAssignment)flowable;


            var exist = _CaseAggregate.CaseAssignment_T1_T2_.Get(x => x.CASE_ID == data.CaseID && 
                                                                      x.ASSIGNMENT_ID == data.AssignmentID);

            if (exist == null)
                throw new NullReferenceException(Common_lang.NOT_FOUND_DATA);

            if (exist.CaseAssignmentType == CaseAssignmentType.Finished)
                throw new IndexOutOfRangeException(Case_lang.CASE_ASSIGNMENT_CASE_TYPE_ERROR);
        }

    }
}
