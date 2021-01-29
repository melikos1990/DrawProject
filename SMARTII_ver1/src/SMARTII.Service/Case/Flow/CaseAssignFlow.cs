using System;
using System.Threading.Tasks;
using System.Transactions;
using SMARTII.Domain.Case;
using SMARTII.Domain.Thread;
using SMARTII.Domain.Transaction;
using SMARTII.Service.Case.Task;

namespace SMARTII.Service.Case.Flow
{
    /// <summary>
    /// 轉派流程
    /// ※這邊分流三種 , 分別為
    ///  -> 一般通知
    ///  -> 反應單開立
    ///  -> 轉派處理
    /// 這邊只能注入 Task/flow , 其餘服務不得注入
    /// 原因為屆時需擴充為 workflow pattern
    /// </summary>
    public class CaseAssignFlow : IFlow
    {
        private readonly AssignTask _AssignTask;
        private readonly CaseProcessedTask _CaseProcessTask;
        private readonly ComplaintInvoiceTask _ComplaintInvoiceTask;
        private readonly AssignNotificationTask _AssignNotificationTask;

        public CaseAssignFlow(AssignTask AssignTask,
                              CaseProcessedTask CaseProcessedTask,
                              ComplaintInvoiceTask ComplaintInvoiceTask,
                              AssignNotificationTask AssignNotificationTask)
        {
            _AssignTask = AssignTask;
            _CaseProcessTask = CaseProcessedTask;
            _ComplaintInvoiceTask = ComplaintInvoiceTask;
            _AssignNotificationTask = AssignNotificationTask;
        }

        public async Task<IFlowable> Run(IFlowable flowable, params object[] args)
        {

            var data = (CaseAssignmentBase)flowable;

            IFlowable result = null;

            using (var scope = TrancactionUtility.TransactionScope())
            {
                
                switch (data.CaseAssignmentProcessType)
                {
                    case CaseAssignmentProcessType.Notice:
                        result = await _AssignNotificationTask.Execute(flowable, args);
                        break;
                    case CaseAssignmentProcessType.Invoice:
                        result = await _ComplaintInvoiceTask.Execute(flowable, args);
                        break;
                    case CaseAssignmentProcessType.Assignment:
                        result = await _AssignTask.Execute(flowable, args);
                        break;
                    default:
                        break;
                }

                // 進行完轉派後 , 執行案件處理中之流程 , 因僅有派工需要 故移至派工task處理
                //await _CaseProcessTask.Execute(flowable);

                scope.Complete();

            }

            return result;


        }
    }
}
