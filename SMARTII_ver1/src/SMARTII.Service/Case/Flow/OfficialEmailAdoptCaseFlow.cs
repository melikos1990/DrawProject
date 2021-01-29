using System.Threading.Tasks;
using SMARTII.Domain.Case;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Notification.Email;
using SMARTII.Domain.Transaction;
using SMARTII.Service.Case.Task;

namespace SMARTII.Service.Case.Flow
{
    public class OfficialEmailAdoptCaseFlow : IFlow
    {
        private readonly CaseSourceCompleteIncomingFlow _CaseSourceCompleteIncomingFlow;
        private readonly OfficialEmailAdoptTask _OfficialEmailAdoptTask;


        public OfficialEmailAdoptCaseFlow(CaseSourceCompleteIncomingFlow CaseSourceCompleteIncomingFlow,
                                          OfficialEmailAdoptTask OfficialEmailAdoptTask)
        {
            _CaseSourceCompleteIncomingFlow = CaseSourceCompleteIncomingFlow;
            _OfficialEmailAdoptTask = OfficialEmailAdoptTask;
        }

        public async Task<IFlowable> Run(IFlowable flowable, params object[] args)
        {
            IFlowable result = null;

            var mailDomain = (OfficialEmailEffectivePayload)args[0];
            var isNeedCaseNotice = (bool)args[1];

            using (var scope = TrancactionUtility.TransactionScope())
            {
                var caseSource = (CaseSource)flowable;

                result = (await _CaseSourceCompleteIncomingFlow.Run(caseSource, false)) as CaseSource;

                // 新增認養信件相關通知&刪除信件
                await _OfficialEmailAdoptTask.Execute(result, mailDomain, isNeedCaseNotice);

                scope.Complete();
            }

            return result;
        }
    }
}

