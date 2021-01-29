using System;
using System.Threading.Tasks;
using System.Transactions;
using SMARTII.Domain.Case;
using SMARTII.Domain.Transaction;
using SMARTII.Service.Case.Task;

namespace SMARTII.Service.Case.Flow
{
    /// <summary>
    /// 轉派反應單流程
    /// </summary>
    public class CaseComplaintInvoiceSenderFlow : IFlow
    {
        private readonly ComplaintInvoiceSenderTask _CompaintInvoiceSenderTask;

        public CaseComplaintInvoiceSenderFlow(ComplaintInvoiceSenderTask CompaintInvoiceSenderTask)
        {
            _CompaintInvoiceSenderTask = CompaintInvoiceSenderTask;
        }

        public async Task<IFlowable> Run(IFlowable flowable, params object[] args)
        {
            IFlowable result = null;

        
            using (var scope = TrancactionUtility.TransactionScope())
            {
                // 寄出反應單 Task
                result = await _CompaintInvoiceSenderTask.Execute(flowable , args);

                // 更新資料
                scope.Complete();
            }

            return result;
        }
    }
}
