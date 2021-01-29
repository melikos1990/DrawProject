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
    public class CaseUnlockFlow : IFlow
    {
  
        private readonly CaseUnlockTask _CaseUnlockTask;

        public CaseUnlockFlow(CaseUnlockTask CaseUnlockTask)
        {
            _CaseUnlockTask = CaseUnlockTask;

        }

        public async Task<IFlowable> Run(IFlowable flowable, params object[] args)
        { 
            IFlowable result = null;


            using (var scope = TrancactionUtility.TransactionScope())
            {
                // 案件解鎖
                result = await _CaseUnlockTask.Execute(flowable, args);

                // 更新資料
                scope.Complete();
            }

            return result;
        }
    }
}
