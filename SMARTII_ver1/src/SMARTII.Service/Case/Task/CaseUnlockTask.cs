using System;
using System.Threading.Tasks;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Case;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Domain.Transaction;
using SMARTII.Resource.Tag;

namespace SMARTII.Service.Case.Task
{
    /// <summary>
    /// 案件解鎖Task
    /// </summary>
    public class CaseUnlockTask : TaskBase, IFlowableTask
    {
        private readonly ICaseAggregate _CaseAggregate;
        private readonly ICaseSourceFacade _CaseSourceFacade;
        private readonly ICaseService _CaseService;
        private readonly ICaseFacade _CaseFacade;

        public CaseUnlockTask(ICaseAggregate CaseAggregate,
                              IOrganizationAggregate OrganizationAggregate,
                              ICaseSourceFacade CaseSourceFacade,
                              ICaseService CaseService,
                              ICaseFacade CaseFacade) : base(OrganizationAggregate)
        {
            _CaseAggregate = CaseAggregate;
            _CaseSourceFacade = CaseSourceFacade;
            _CaseService = CaseService;
            _CaseFacade = CaseFacade;
        }

        public async Task<IFlowable> Execute(IFlowable flowable, params object[] args)
        {
            var data = (Domain.Case.Case)flowable;

            await Validator(flowable, args);

            var hasAssignment = _CaseAggregate.CaseAssignment_T1_T2_.HasAny(x => x.CASE_ID == data.CaseID);

            // 更新案件結案狀態與結案時間
            var con = new MSSQLCondition<CASE>(x => x.CASE_ID == data.CaseID);

            con.ActionModify(x =>
            {
                x.CASE_TYPE = hasAssignment ? (byte)CaseType.Process : (byte)CaseType.Filling;
                x.FINISH_DATETIME = null;
            });

            flowable = _CaseAggregate.Case_T1_T2_.Update(con);

            var user = ContextUtility.GetUserIdentity()?.Instance;

            _CaseFacade.CreateResume(((Domain.Case.Case)flowable).CaseID, null, SysCommon_lang.CASE_UNCLOSE_RESUME, Case_lang.CASE_UNLOCK, user);
            // 新增歷程後記錄
            using (var scope = TrancactionUtility.NoTransactionScope())
            {
                global::System.Threading.Tasks.Task.Run(() =>
                {
                    _CaseFacade.CreateHistory(((Domain.Case.Case)flowable).CaseID, ((Domain.Case.Case)flowable), Case_lang.CASE_UNLOCK, user);
                });
            };
            

            return flowable;
        }

        public async global::System.Threading.Tasks.Task Validator(IFlowable flowable, params object[] args)
        {
            var data = (Domain.Case.Case)flowable;

            if (data.CaseType != CaseType.Finished)
                throw new IndexOutOfRangeException(Case_lang.CASE_TYPE_ERROR);
        }
    }
}
