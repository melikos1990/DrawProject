using System.Threading.Tasks;
using System.Transactions;
using SMARTII.Domain.Case;
using SMARTII.Domain.Transaction;
using SMARTII.Service.Case.Task;

namespace SMARTII.Service.Case.Flow
{
    /// <summary>
    /// 案件來源流程
    /// 這邊只能注入 Task / Flow, 其餘服務不得注入
    /// 原因為屆時需擴充為 workflow pattern
    /// </summary>
    public class CaseSourceIncomingFlow : IFlow
    {
        private readonly CaseSourceCreateTask _CaseSourceCreateTask;

        public CaseSourceIncomingFlow(CaseSourceCreateTask caseSourceCreateTask)
        {
            _CaseSourceCreateTask = caseSourceCreateTask;
        }

        public async Task<IFlowable> Run(IFlowable flowable, params object[] args)
        {
            IFlowable result = null;

            using (var scope = TrancactionUtility.TransactionScope())
            {

                // 當案件來源時 , 進行新增
                result = await _CaseSourceCreateTask.Execute(flowable);

                scope.Complete();
            }

            return result;
        }
    }
}
