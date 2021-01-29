#define Test

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
    /// <summary>
    /// 轉派處理TASK
    /// </summary>
    public class AssignProcessedTask : IFlowableTask
    {
        private readonly ICaseAggregate _CaseAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly ICaseService _CaseService;
        private readonly ICaseAssignmentService _CaseAssignmentService;
        private readonly ICaseFacade _CaseFacade;
        private readonly ICaseAssignmentFacade _CaseAssignmentFacade;
        private readonly OrganizationNodeResolver _OrganizationResolver;
        private readonly HeaderQuarterNodeProcessProvider _HeaderQuarterNodeProcessProvider;



        public AssignProcessedTask(ICaseAggregate CaseAggregate,
                                   IOrganizationAggregate OrganizationAggregate,
                                   ICaseService CaseService,
                                   ICaseAssignmentService CaseAssignmentService,
                                   ICaseFacade CaseFacade,
                                   ICaseAssignmentFacade CaseAssignmentFacade,
                                   OrganizationNodeResolver OrganizationNodeResolver,
                                   HeaderQuarterNodeProcessProvider HeaderQuarterNodeProcessProvider)
        {
            _CaseAggregate = CaseAggregate;
            _OrganizationAggregate = OrganizationAggregate;
            _CaseService = CaseService;
            _CaseAssignmentService = CaseAssignmentService;
            _CaseFacade = CaseFacade;
            _CaseAssignmentFacade = CaseAssignmentFacade;
            _OrganizationResolver = OrganizationNodeResolver;
            _HeaderQuarterNodeProcessProvider = HeaderQuarterNodeProcessProvider;
        }

        public async Task<IFlowable> Execute(IFlowable flowable, params object[] args)
        {
            var data = (CaseAssignment)flowable;

            await Validator(data, args);

            var jobPosition = args[0] as JobPosition;
            
            // 更改為處理完畢
            data.CaseAssignmentType = CaseAssignmentType.Processed;
            data.FinishDateTime = DateTime.Now;
            data.FinishUserName = ContextUtility.GetUserIdentity()?.Name;
            data.FinishNodeID = jobPosition.NodeID;
            data.FinishNodeName = jobPosition.NodeName;
            data.FinishOrganizationType = jobPosition.OrganizationType;
            data.RejectType = RejectType.None;

            using (var scope = TrancactionUtility.TransactionScope())
            {

                // 一般更新
                _CaseAssignmentService.Update(data, jobPosition);

                scope.Complete();
            }

            var user = ContextUtility.GetUserIdentity()?.Instance;

            // 新增案件Resume
            _CaseFacade.CreateResume(data.CaseID, null, string.Format(SysCommon_lang.CASE_ASSIGNMENT_FINISH_RESUME, data.CaseID, data.AssignmentID), Case_lang.CASE_ASSIGNMENT_FINISH, user);

            // 新增轉派Resume & History
            _CaseAssignmentFacade.CreateResume(data, string.Format(SysCommon_lang.CASE_ASSIGNMENT_FINISH_ASSIGNMENT_RESUME, data.FinishContent), user.Name, jobPosition);

            using (var scope = TrancactionUtility.NoTransactionScope())
            {
                global::System.Threading.Tasks.Task.Run(() => 
                {
                    _CaseAssignmentFacade.CreateHistory(data, Case_lang.CASE_ASSIGNMENT_FINISH, user.Name);
                    _CaseAssignmentFacade.CreatePersonalNotification(data, user.Name, string.Format(SysCommon_lang.PERSONAL_NOTICE_CASE_ASSIGNMENT_UPDATE, data.FinishNodeName, data.FinishUserName));
                });
            };

            return data;

        }
        public async global::System.Threading.Tasks.Task Validator(IFlowable flowable, params object[] args)
        {
            var data = (CaseAssignment)flowable;

            if (args.Length == 0 || args[0] == null)
                throw new ArgumentNullException(Case_lang.CASE_NO_PASS_EDITOR_NODE_JOB_ID);

            if (_CaseAggregate.CaseAssignment_T1_T2_.HasAny(x=>x.ASSIGNMENT_ID == data.AssignmentID && x.CASE_ID == data.CaseID && x.CASE_ASSIGNMENT_TYPE == (byte)CaseAssignmentType.Processed))
                throw new IndexOutOfRangeException(Case_lang.CASE_ASSIGNMENT_HAS_FINISH);


        }
    }
}
