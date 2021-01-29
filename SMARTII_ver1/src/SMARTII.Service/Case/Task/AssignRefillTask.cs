using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Case;
using SMARTII.Domain.IO;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Domain.Transaction;
using SMARTII.Resource.Tag;
using SMARTII.Service.Organization.Provider;
using SMARTII.Service.Organization.Resolver;

namespace SMARTII.Service.Case.Task
{
    public class AssignRefillTask : IFlowableTask
    {
        private readonly ICaseAssignmentService _CaseAssignmentService;
        private readonly ICaseAssignmentFacade _CaseAssignmentFacade;
        private readonly OrganizationNodeResolver _OrganizationResolver;

        public AssignRefillTask(ICaseAssignmentService CaseAssignmentService,
                                ICaseAssignmentFacade CaseAssignmentFacade,
                                OrganizationNodeResolver OrganizationNodeResolver)
        {
            _CaseAssignmentService = CaseAssignmentService;
            _CaseAssignmentFacade = CaseAssignmentFacade;
            _OrganizationResolver = OrganizationNodeResolver;
        }

        public async Task<IFlowable> Execute(IFlowable flowable, params object[] args)
        {
            var data = (CaseAssignment)flowable;
            var user = ContextUtility.GetUserIdentity()?.Instance;

            await Validator(data, args);

            var jobPosition = args[0] as JobPosition;
            data.RejectType = RejectType.None;
            data.UpdateDateTime = DateTime.Now;
            data.UpdateUserName = user.Name;

            data = _CaseAssignmentService.Update(data, jobPosition);

            data = _OrganizationResolver.Resolve(data);
            using (var scope = TrancactionUtility.NoTransactionScope())
            {
                global::System.Threading.Tasks.Task.Run(() =>
                {
                    _CaseAssignmentFacade.CreateHistory(data, Case_lang.CASE_ASSIGNMENT_REFILL, user.Name);
                    _CaseAssignmentFacade.CreatePersonalNotification(data, user.Name, string.Format(SysCommon_lang.CASE_ASSIGNMENT_FINISH_RESPONES_FAIL_PERSONAL_NOTIFY, data.FinishNodeName, user.Name));
                });
            };

            return data;
        }
        public async global::System.Threading.Tasks.Task Validator(IFlowable flowable, params object[] args)
        {


        }
    }
}
