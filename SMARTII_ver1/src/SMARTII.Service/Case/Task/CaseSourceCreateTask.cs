#define Test

using System;
using System.Threading.Tasks;
using System.Transactions;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Domain.Transaction;
using SMARTII.Resource.Tag;


namespace SMARTII.Service.Case.Task
{
    /// <summary>
    /// 案件來源建立TASK
    /// </summary>
    public class CaseSourceCreateTask : TaskBase, IFlowableTask
    {
        private readonly ICaseAggregate _CaseAggregate;
        private readonly ICaseSourceFacade _CaseSourceFacade;
        private readonly ICaseSourceService _CaseSourceService;

        public CaseSourceCreateTask(ICaseAggregate CaseAggregate,
                                    IOrganizationAggregate OrganizationAggregate,
                                    ICaseSourceFacade CaseSourceFacade,
                                    ICaseSourceService CaseSourceService) : base(OrganizationAggregate)
        {
            _CaseAggregate = CaseAggregate;
            _CaseSourceFacade = CaseSourceFacade;
            _CaseSourceService = CaseSourceService;
        }

        public async Task<IFlowable> Execute(IFlowable flowable, params object[] args)
        {

            await Validator(flowable);

            var data = (CaseSource)flowable;

            // 計算來源編號(滾號檔)
            var sourceID = _CaseSourceFacade.GetSourceCode();

            // 回填來源編號 (人員/來源)
            data.SourceID = sourceID;
            data.CaseSourceUser.SourceID = sourceID;
            data.CreateDateTime = DateTime.Now;
            data.CreateUserName = ContextUtility.GetUserIdentity()?.Name;


            // 來源時間為空, 就為目前時間。
            // 情境在於當今天為手動立案時 , 於畫面並不會押上進線時間。
            data.IncomingDateTime = data.IncomingDateTime ?? DateTime.Now;

            // 計算勾稽編號 , 預立案的需要更新
            _CaseSourceFacade.CancelPreventTagsFromCaseIDs(data.RelationCaseIDs);

            var result = await _CaseAggregate.CaseSource_T1_T2_.Add(data).Async();

            var user = ContextUtility.GetUserIdentity()?.Instance;
            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted}))
            {
                //紀錄來源History
                global::System.Threading.Tasks.Task.Run(() => _CaseSourceFacade.CreateHistory(result, Case_lang.CASE_SOURCE_SAVE, user));
            };

            // 新增來源
            return result;
        }

        public async global::System.Threading.Tasks.Task Validator(IFlowable flowable, params object[] args)
        {
            var data = (CaseSource)flowable;

            // 必須要傳入來源人資訊
            if (data.CaseSourceUser == null)
                throw new NullReferenceException(Case_lang.CASE_SOURCE_UNDEFIND);


            // 驗證自己是否有權限能夠立案
            // 需檢核是否有該節點Group的權限 (DATARANGE)
            if (data.GroupID.HasValue && !base.HasGroupAuth(data.GroupID.Value))
                throw new NullReferenceException(Case_lang.CASE_NO_GROUP_AUTH);

            // 立案時需檢核該Group 是否服務此BU
            if (data.GroupID.HasValue && !base.HasGroupBU(data.NodeID, data.GroupID.Value))
                throw new NullReferenceException(Case_lang.CASE_GROUP_NO_BU);
        }
    }
}
