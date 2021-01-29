#define Test

using System;
using System.Threading.Tasks;
using System.Transactions;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;
using SMARTII.Domain.IO;
using SMARTII.Domain.Thread;
using SMARTII.Domain.Transaction;
using SMARTII.Resource.Tag;

namespace SMARTII.Service.Case.Task
{
    /// <summary>
    /// 銷案駁回Task
    /// </summary>
    public class AssignRejectTask : IFlowableTask
    {
        private readonly ICaseAssignmentService _CaseAssignmentService;
        private readonly ICaseAggregate _CaseAggregate;
        private readonly ICaseService _CaseService;
        private readonly ICaseFacade _CaseFacade;
        private readonly ICaseAssignmentFacade _CaseAssignmentFacade;

        public AssignRejectTask(
            ICaseAssignmentService CaseAssignmentService,
            ICaseAggregate CaseAggregate,
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



            var exist = _CaseAggregate.CaseAssignment_T1_T2_
                                      .Get(x => x.CASE_ID == data.CaseID &&
                                                x.ASSIGNMENT_ID == data.AssignmentID);

            await Validator(flowable, exist);

            // 判斷駁回原因 , 如果是重填 , 則需將結案時間
            if (data.RejectType == RejectType.Undo)
            {
                data.FinishDateTime = null;
                data.FinishUserName = null;
                data.FinishNodeID = null;
                data.FinishNodeName = null;
                data.FinishOrganizationType = null;
                data.CaseAssignmentType = CaseAssignmentType.Assigned;
            }

            if (data.RejectType == RejectType.FillContent)
            {
                data.FinishUserName = exist.FinishUserName;
                data.FinishDateTime = exist.FinishDateTime;
                data.FinishNodeID = exist.FinishNodeID;
                data.FinishNodeName = exist.FinishNodeName;
                data.FinishOrganizationType = exist.FinishOrganizationType;
            }

            data.UpdateDateTime = DateTime.Now;
            data.UpdateUserName = ContextUtility.GetUserIdentity()?.Name;

            _CaseAssignmentService.Update(data);       

            var user = ContextUtility.GetUserIdentity()?.Instance;

            // 新增歷程後記錄
            _CaseFacade.CreateResume(data.CaseID, null, string.Format(SysCommon_lang.CASE_ASSIGNMENT_REJECT_RESUME, data.CaseID, data.AssignmentID, data.RejectType.GetDescription(), data.RejectReason), Case_lang.CASE_ASSIGNMENT_REJECT, user);

            // 新增轉派Resume & History
            _CaseAssignmentFacade.CreateResume(data, string.Format(SysCommon_lang.CASE_ASSIGNMENT_REJECT_ASSIGNMENT_RESUME, data.RejectType.GetDescription(), data.RejectReason), user.Name);
            using (var scope = TrancactionUtility.NoTransactionScope())
            {
                global::System.Threading.Tasks.Task.Run(() =>
                {
                    _CaseAssignmentFacade.CreateHistory(data, Case_lang.CASE_ASSIGNMENT_REJECT, user.Name);
                });
            };

            return data;

        }

        public async global::System.Threading.Tasks.Task Validator(IFlowable flowable, params object[] args)
        {

            var data = (CaseAssignment)flowable;

            var exist = args[0] as CaseAssignment;

            if (exist == null)
                throw new NullReferenceException(Common_lang.NOT_FOUND_DATA);

            if (exist.CaseAssignmentType != CaseAssignmentType.Processed)
                throw new NullReferenceException(Case_lang.CASE_ASSIGNMENT_TYPE_ERROR_WHEN_REJECT);

            if (data.RejectType == RejectType.None)
                throw new NullReferenceException(Case_lang.CASE_NO_REJECT_TYPE);


        }
    }
}
