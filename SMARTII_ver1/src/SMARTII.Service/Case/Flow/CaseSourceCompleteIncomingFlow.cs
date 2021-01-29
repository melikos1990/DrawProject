using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMARTII.Domain.Case;
using SMARTII.Domain.Transaction;

namespace SMARTII.Service.Case.Flow
{
    /// <summary>
    /// 案件來源流程(包含案件)
    /// 這邊只能注入 Task / Flow, 其餘服務不得注入
    /// 原因為屆時需擴充為 workflow pattern
    /// </summary>
    public class CaseSourceCompleteIncomingFlow : IFlow
    {

        private readonly CaseSourceIncomingFlow _CaseIncomingFlow;
        private readonly CaseFillingFlow _CaseFillingFlow;

        public CaseSourceCompleteIncomingFlow(CaseSourceIncomingFlow CaseIncomingFlow,
                                        CaseFillingFlow CaseFillingFlow)
        {
            _CaseIncomingFlow = CaseIncomingFlow;
            _CaseFillingFlow = CaseFillingFlow;
        }

        public async Task<IFlowable> Run(IFlowable flowable, params object[] args)
        {
            CaseSource result = null;

            using (var scope = TrancactionUtility.TransactionScope())
            {

                // 當案件來源時 , 進行新增
                result = (await _CaseIncomingFlow.Run(flowable, args)) as CaseSource;

                // 回填案件編號
                var @case = ((CaseSource)flowable).Cases[0];
                @case.SourceID = result.SourceID;

                // 當案件來源包含案件時時 , 進行新增
                result.Cases.Add((Domain.Case.Case)await _CaseFillingFlow.Run(@case, args));

                scope.Complete();
            }

            return result;
        }
    }
}
