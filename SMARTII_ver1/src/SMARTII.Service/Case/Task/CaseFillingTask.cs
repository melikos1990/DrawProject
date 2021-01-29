#define Test

using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Case;
using SMARTII.Domain.DI;
using SMARTII.Domain.IO;
using SMARTII.Domain.Master;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Domain.Transaction;
using SMARTII.Resource.Tag;
using SMARTII.Service.Organization.Provider;

namespace SMARTII.Service.Case.Task
{
    /// <summary>
    /// 案件立案TASK
    /// </summary>
    public class CaseFillingTask : TaskBase, IFlowableTask
    {
        private readonly ICaseFacade _CaseFacade;
        private readonly ICaseService _CaseService;
        private readonly ICaseAggregate _CaseAggregate;
        private readonly IMasterAggregate _MasterAggregate;
        private readonly ICaseSourceFacade _CaseSourceFacade;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly IIndex<string, ICaseSpecificFactory> _CaseSpecificFactory;
        private readonly HeaderQuarterNodeProcessProvider _HeaderQuarterNodeProcessProvider;

        public CaseFillingTask(ICaseFacade CaseFacade,
                               ICaseService CaseService,
                               ICaseAggregate CaseAggregate,
                               IMasterAggregate MasterAggregate,
                               ICaseSourceFacade CaseSourceFacade,
                               IOrganizationAggregate OrganizationAggregate,
                               IIndex<string, ICaseSpecificFactory> CaseSpecificFactory,
                               HeaderQuarterNodeProcessProvider HeaderQuarterNodeProcessProvider) : base(OrganizationAggregate)
        {
            _CaseFacade = CaseFacade;
            _CaseService = CaseService;
            _CaseAggregate = CaseAggregate;
            _MasterAggregate = MasterAggregate;
            _CaseSourceFacade = CaseSourceFacade;
            _CaseSpecificFactory = CaseSpecificFactory;
            _OrganizationAggregate = OrganizationAggregate;
            _HeaderQuarterNodeProcessProvider = HeaderQuarterNodeProcessProvider;
        }

        public async Task<IFlowable> Execute(IFlowable flowable, params object[] args)
        {
            var data = (Domain.Case.Case)flowable;

            await Validator(data);


            // 發生時間 , 為了避免立案時間與相關的計算基準一致
            // 因此需先記錄在變數中。
            var announceDateTime = DateTime.Now;

            // 取得BU識別值 , 如(003 => ASO) ...
            // 此欄位為系統原則 , 若無法取得 , 則需拋出例外 。
            var term = (HeaderQuarterTerm)_HeaderQuarterNodeProcessProvider.GetTerm(
                        data.NodeID,
                        data.OrganizationType);

            // 計算案件編號(滾號檔)
            var caseID = _CaseFacade.GetCaseCode(term.NodeKey, announceDateTime.Date);

            // 計算案件時效
            var promiseDateTime = _CaseFacade.GetPromiseDateTime(data, term, announceDateTime);

            data.CaseID = caseID;
            data.PromiseDateTime = promiseDateTime;
            data.CaseConcatUsers?.ForEach(x => x.CaseID = caseID);
            data.CaseComplainedUsers?.ForEach(x => x.CaseID = caseID);
            data.ApplyDateTime = DateTime.Now;
            data.CreateDateTime = DateTime.Now;
            data.CaseType = CaseType.Filling;
            data.ApplyUserID = data.ApplyUserID ?? ContextUtility.GetUserIdentity()?.Instance.UserID;
            data.ApplyUserName = data.ApplyUserName ?? ContextUtility.GetUserIdentity()?.Name;
            data.CreateUserName = ContextUtility.GetUserIdentity()?.Name;


            new FileProcessInvoker(context =>
            {
                using (var scope = TrancactionUtility.TransactionScope())
                {
                    // 建立附件
                    var pathArray = FileSaverUtility.SaveCaseFiles(context, data, data.Files);

                    data.FilePath = pathArray?.ToArray();

                    // 計算勾稽編號 , 預立案的需要更新
                    _CaseSourceFacade.CancelPreventTagsFromCaseIDs(data.RelationCaseIDs);


                    // 一般新增
                    _CaseAggregate.Case_T1_T2_.Add(data);

                    // 特殊欄位更新
                    _CaseSpecificFactory.TryGetService(
                                      term.NodeKey,
                                      EssentialCache.BusinessKeyValue.COMMONBU).Update(data);
                    // 更新標籤
                    _CaseFacade.UpdateOrCreateTags(data);

                    scope.Complete();
                }
            });

            var user = ContextUtility.GetUserIdentity()?.Instance;

            _CaseFacade.CreateResume(data.CaseID, null, string.Format(SysCommon_lang.CASE_SAVE_SUCCESS_RESUME, data.CaseID), Case_lang.CASE_SAVE, user);

            // 新增案件後記錄
            using (var scope = TrancactionUtility.NoTransactionScope())
            {
                global::System.Threading.Tasks.Task.Run(() =>
                {
                    _CaseFacade.CreateHistory(data.CaseID, data, Case_lang.CASE_SAVE, user);
                });
            };
            

            return data;
        }

        public async global::System.Threading.Tasks.Task Validator(IFlowable flowable, params object[] args)
        {
            // 驗證來源
            var data = (Domain.Case.Case)flowable;

            var soruce = _CaseAggregate.CaseSource_T1_T2_.Get(x => x.SOURCE_ID == data.SourceID);

            if (soruce == null)
                throw new NullReferenceException(Case_lang.CASE_SOURCE_UNDEFIND);

            if (soruce.IsPrevention)
                throw new IndexOutOfRangeException(Case_lang.CASE_SOURCE_IS_REVENTION_UNDEFIND);

            if (data.AtLeastOneRespinsibility() == false)
                throw new NullReferenceException(Case_lang.CASE_COMPLAINTED_USER_AT_LEAST_ONE_RESPONSEBLILITY);

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
