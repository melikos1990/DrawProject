using System;
using System.Threading.Tasks;
using System.Transactions;
using SMARTII.Domain.Case;
using SMARTII.Domain.Transaction;
using SMARTII.Service.Case.Task;

namespace SMARTII.Service.Case.Flow
{
    public class CaseAssignRefillFlow : IFlow
    {
        private readonly AssignRefillTask _AssignRefillTask;

        public CaseAssignRefillFlow(AssignRefillTask AssignRefillTask)
        {
            _AssignRefillTask = AssignRefillTask;
        }

        public async Task<IFlowable> Run(IFlowable flowable, params object[] args)
        {
            IFlowable result = null;


            using (var scope = TrancactionUtility.TransactionScope())
            {
                // 轉派進行處理 Task
                result = await _AssignRefillTask.Execute(flowable, args);

                // 更新資料
                scope.Complete();

            }

            return result;
        }
    }
}
