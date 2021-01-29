using System;
using System.Threading.Tasks;
using System.Transactions;
using SMARTII.Domain.Case;
using SMARTII.Domain.Transaction;
using SMARTII.Service.Case.Task;

namespace SMARTII.Service.Case.Flow
{
    /// <summary>
    /// 案件結案流程
    /// 這邊只能注入 Task / Flow, 其餘服務不得注入
    /// 原因為屆時需擴充為 workflow pattern
    /// </summary>
    public class CaseFinishedFlow : IFlow
    {
        private readonly CaseFinishedTask _CaseFinishedTask;

        public CaseFinishedFlow(CaseFinishedTask CaseFinishedTask)
        {
            _CaseFinishedTask = CaseFinishedTask;
        }

        public async Task<IFlowable> Run(IFlowable flowable, params object[] args)
        {

            IFlowable result = null;

            using (var scope = TrancactionUtility.TransactionScope())
            {
                // 進行案件結案 Task
                result = await _CaseFinishedTask.Execute(flowable , false);

                // 更新資料
                scope.Complete();

            }

            return result;
        }
    }
}
