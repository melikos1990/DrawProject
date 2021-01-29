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
    public class CaseComplaintInvoiceResendFlow : IFlow
    {
        private readonly ComplaintInvoiceResendTask _CompaintInvoiceResendTask;

        public CaseComplaintInvoiceResendFlow(ComplaintInvoiceResendTask CompaintInvoiceResendTask)
        {
            _CompaintInvoiceResendTask = CompaintInvoiceResendTask;
        }

        public async Task<IFlowable> Run(IFlowable flowable, params object[] args)
        {
            IFlowable result = null;


            using (var scope = TrancactionUtility.TransactionScope())
            {
                // 重寄反應單 Task
                result = await _CompaintInvoiceResendTask.Execute(flowable, args);

                // 更新資料
                scope.Complete();
            }

            return result;
        }
    }
}
