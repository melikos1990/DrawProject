using System;
using System.Threading.Tasks;
using System.Transactions;
using SMARTII.Domain.Case;
using SMARTII.Domain.Transaction;
using SMARTII.Service.Case.Task;

namespace SMARTII.Service.Case.Flow
{
    /// <summary>
    /// 案件立案流程
    /// 這邊只能注入 Task / Flow, 其餘服務不得注入
    /// 原因為屆時需擴充為 workflow pattern
    /// </summary>
    public class CaseFillingFlow : IFlow
    {
        private readonly CaseFillingTask _CaseFillingTask;
        private readonly CaseFinishedTask _CaseFinishedTask;

        public CaseFillingFlow(CaseFillingTask caseFillingTask,
                               CaseFinishedTask CaseFinishedTask)
        {
            _CaseFinishedTask = CaseFinishedTask;
            _CaseFillingTask = caseFillingTask;
        }

        public async Task<IFlowable> Run(IFlowable flowable, params object[] args)
        {

            IFlowable result = null;

            // 傳入可選參數
            // 是否為快速結案
            Boolean? isFastFinish = args[0] as Boolean?;

            using (var scope = TrancactionUtility.TransactionScope())
            {
                // 先進行立案。
                var flowFilling = await _CaseFillingTask.Execute(flowable);

                // 如果是快速結案 , 就直接進行結案了。
                result = (isFastFinish == true) ?
                        await _CaseFinishedTask.Execute(flowFilling , isFastFinish) : flowFilling;

                scope.Complete();

            }

            return result;

        }
    }
}
