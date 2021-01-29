using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMARTII.Domain.Case;
using SMARTII.Domain.Transaction;
using SMARTII.Service.Case.Task;

namespace SMARTII.Service.Case.Flow
{
    public class CaseAssignmentSenderFlow : IFlow
    {

        private readonly AssignmentSenderTask _AssignmentSenderTask;

        public CaseAssignmentSenderFlow(AssignmentSenderTask AssignmentSenderTask)
        {
            _AssignmentSenderTask = AssignmentSenderTask;
        }

        public async Task<IFlowable> Run(IFlowable flowable, params object[] args)
        {
            
            IFlowable result = null;

            using (var scope = TrancactionUtility.TransactionScope())
            {
                // 寄出反應單 Task
                result = await _AssignmentSenderTask.Execute(flowable, args);

                // 更新資料
                scope.Complete();
            }

            return result;
        }
    }
}
