#define Test

using System;
using System.Threading.Tasks;
using System.Transactions;
using SMARTII.Domain.Case;
using SMARTII.Domain.IO;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Domain.Transaction;
using SMARTII.Resource.Tag;
using SMARTII.Service.Cache;
using SMARTII.Service.Organization.Provider;

namespace SMARTII.Service.Case.Task
{
    /// <summary>
    /// 反應單建立TASK
    /// </summary>
    public class ComplaintInvoiceTask : IFlowableTask
    {
        private readonly ICaseAggregate _CaseAggregate;
        private readonly IMasterAggregate _MasterAggregate;
        private readonly ICaseAssignmentFacade _CaseAssignmentFacade;
        private readonly ICaseFacade _CaseFacade;
        private readonly HeaderQuarterNodeProcessProvider _HeaderQuarterNodeProcessProvider;

        public ComplaintInvoiceTask(ICaseAggregate CaseAggregate,
                                    IMasterAggregate MasterAggregate,
                                    ICaseAssignmentFacade CaseAssignmentFacade,
                                    ICaseFacade CaseFacade,
                                    HeaderQuarterNodeProcessProvider HeaderQuarterNodeProcessProvider)
        {
            _CaseAggregate = CaseAggregate;
            _MasterAggregate = MasterAggregate;
            _CaseAssignmentFacade = CaseAssignmentFacade;
            _CaseFacade = CaseFacade;
            _HeaderQuarterNodeProcessProvider = HeaderQuarterNodeProcessProvider;
        }

        public async Task<IFlowable> Execute(IFlowable flowable, params object[] args)
        {
            var data = (CaseAssignmentComplaintInvoice)flowable;

            IFlowable result = null;
            using (var transactionscope = TrancactionUtility.NoTransactionScope())
            {
                // 發生時間 , 為了避免立案時間與相關的計算基準一致
                // 因此需先記錄在變數中。
                var announceDateTime = DateTime.Now;

                // 取得 BU 三碼
                var term = (HeaderQuarterTerm)_HeaderQuarterNodeProcessProvider
                                .GetTerm(data.NodeID, data.OrganizationType);

                await Validator(flowable, term);

                // 計算反應單單號
                data.InvoiceID = _CaseAssignmentFacade.GetInvoiceCode(data.InvoiceType, term.NodeKey);
                data.CreateDateTime = DateTime.Now;
                data.CreateUserName = ContextUtility.GetUserIdentity()?.Name;
                data.CaseAssignmentComplaintInvoiceType = CaseAssignmentComplaintInvoiceType.Created;
                data.Users?.ForEach(x => x.CaseID = data.CaseID);


            }

            new FileProcessInvoker((context) =>
            {

                using (var transactionscope = new TransactionScope(
                TransactionScopeOption.Required,
                TransactionScopeAsyncFlowOption.Enabled))
                {

                    // 建立附件
                    var pathArray = FileSaverUtility.SaveAssignmentInvoiceFiles(context, data);
                    data.FilePath = pathArray?.ToArray();

                    // 寫入資料
                    result = _CaseAggregate.CaseAssignmentComplaintInvoice_T1_T2_.Add(data);

                    // TODO 寫入歷程
                    var user = ContextUtility.GetUserIdentity()?.Instance;
                    _CaseFacade.CreateResume(data.CaseID, null, string.Format(SysCommon_lang.CASE_ASSIGNMENT_INVOICE_RESUME, data.InvoiceID), Case_lang.CASE_ASSIGNMENT_INVOICE_SAVE, user);

                    transactionscope.Complete();
                }
            });

            return result;
        }

        public async global::System.Threading.Tasks.Task Validator(IFlowable flowable, params object[] args)
        {
            var term = (HeaderQuarterTerm)args[0];

            var data = (CaseAssignmentBase)flowable;

            var inovicedata = (CaseAssignmentComplaintInvoice)flowable;

            if (_CaseAggregate.Case_T1_T2_.HasAny(x => x.CASE_ID == data.CaseID && x.CASE_TYPE == (byte)CaseType.Finished))
                throw new NullReferenceException(Case_lang.CASE_ALREADY_FINISH);

            // 如果是不允許多開 , 就需要彈出錯誤
            if (DataStorage.CaseMutipleTicketTypeDict.TryGetValue(term.NodeKey, out var type) == false && 
                inovicedata.CaseAssignmentComplaintInvoiceType != CaseAssignmentComplaintInvoiceType.Cancel)
                throw new NullReferenceException(Case_lang.CASE_NO_MUTIPLE_ASSIGN_TICKET);

            // 如果是只能允許一個反應單開立 , 則需檢驗是否已經開立過了反應單
            if (type == CaseMutipleTicketType.One)
            {
                var isOpend = _CaseAggregate.CaseAssignmentComplaintInvoice_T1_.HasAny(x => x.CASE_ID == data.CaseID && x.TYPE != 2);

                if (isOpend)
                    throw new NullReferenceException(Case_lang.CASE_TICKET_OUT_OF_INDEX);
            }
        }
    }
}
