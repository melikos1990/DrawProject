#define Test
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMARTII.Domain.Case;
using SMARTII.Domain.Thread;
using SMARTII.Domain.Transaction;
using SMARTII.Resource.Tag;

namespace SMARTII.Service.Case.Task
{

    /// <summary>
    /// 單位溝通Task
    /// </summary>
    public class AssignCommunicateTask : IFlowableTask
    {
        private readonly ICaseAggregate _CaseAggregate;

        public AssignCommunicateTask(ICaseAggregate CaseAggregate)
        {
            _CaseAggregate = CaseAggregate;

        }

        public async Task<IFlowable> Execute(IFlowable flowable, params object[] args)
        {
            var data = (CaseAssignmentCommunicate)flowable;

            await Validator(data, args);

            // 組合更新欄位
            data.NotificationDateTime = DateTime.Now;
            data.CreateDateTime = DateTime.Now;
            data.CreateUserName = ContextUtility.GetUserIdentity()?.Name;

            using (var scope = TrancactionUtility.TransactionScope())
            {               

                data = _CaseAggregate.CaseAssignmentCommunicate_T1_T2_.Add(data);

                // TODO : 寫入結案通知

                scope.Complete();
            }

            return data;
        }

        public async global::System.Threading.Tasks.Task Validator(IFlowable flowable, params object[] args)
        {
            var data = (CaseAssignmentCommunicate)flowable;

            if (_CaseAggregate.Case_T1_T2_.HasAny(x => x.CASE_ID == data.CaseID && x.CASE_TYPE == (byte)CaseType.Finished))
                throw new NullReferenceException(Case_lang.CASE_ALREADY_FINISH);

        }
    }
}
