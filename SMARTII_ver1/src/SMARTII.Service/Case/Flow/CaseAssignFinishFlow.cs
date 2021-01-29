using System;
using System.Threading.Tasks;
using System.Transactions;
using SMARTII.Domain.Case;
using SMARTII.Domain.Transaction;
using SMARTII.Service.Case.Task;

namespace SMARTII.Service.Case.Flow
{
    /// <summary>
    /// 轉派銷案流程
    /// 這邊只能注入 Task , 其餘服務不得注入
    /// 原因為屆時需擴充為 workflow pattern
    /// </summary>
    public class CaseAssignFinishFlow : IFlow
    {
        private readonly AssignFinishedTask _AssignFinishedTask;

        public CaseAssignFinishFlow(AssignFinishedTask AssignFinishedTask)
        {
            _AssignFinishedTask = AssignFinishedTask;
        }

        public async Task<IFlowable> Run(IFlowable flowable, params object[] args)
        {
            IFlowable result = null;


            using (var scope = TrancactionUtility.TransactionScope())
            {
                // 轉派結案處理 Task
                result = await _AssignFinishedTask.Execute(flowable , args);

                // 更新資料
                scope.Complete();

            }

            return result;
        }

    
    }
}
