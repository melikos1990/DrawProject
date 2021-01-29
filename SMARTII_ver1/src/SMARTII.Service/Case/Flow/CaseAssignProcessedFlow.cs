using System;
using System.Threading.Tasks;
using System.Transactions;
using SMARTII.Domain.Case;
using SMARTII.Domain.Transaction;
using SMARTII.Service.Case.Task;

namespace SMARTII.Service.Case.Flow
{
    /// <summary>
    /// 轉派處理完畢流程
    /// 這邊只能注入 Task/Flow , 其餘服務不得注入
    /// 原因為屆時需擴充為 workflow pattern
    /// </summary>
    public class CaseAssignProcessedFlow : IFlow
    {
        private readonly AssignProcessedTask _AssignProcessedTask;

        public CaseAssignProcessedFlow(AssignProcessedTask AssignProcessedTask)
        {
            _AssignProcessedTask = AssignProcessedTask;
        }

        public async Task<IFlowable> Run(IFlowable flowable, params object[] args)
        {
            IFlowable result = null;


            using (var scope = TrancactionUtility.TransactionScope())
            {
                // 轉派進行處理 Task
                result = await _AssignProcessedTask.Execute(flowable , args);

                // 更新資料
                scope.Complete();

            }

            return result;
        }
    }
}
