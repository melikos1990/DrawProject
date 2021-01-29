#define Test

using System;
using System.Threading.Tasks;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Case;
using SMARTII.Domain.Common;
using SMARTII.Domain.Thread;

namespace SMARTII.Service.Case.Task
{
    /// <summary>
    /// 案件處理TASK
    /// </summary>
    public class CaseProcessedTask : IFlowableTask
    {
        private readonly ICaseAggregate _CaseAggregate;

        public CaseProcessedTask(ICaseAggregate CaseAggregate)
        {
            _CaseAggregate = CaseAggregate;
        }

        public async Task<IFlowable> Execute(IFlowable flowable, params object[] args)
        {
            var data = (CaseAssignmentBase)flowable;

            var hasAnyAssignment = _CaseAggregate.CaseAssignment_T1_.HasAny(x => x.CASE_ID == data.CaseID);
            //var hasAnyInvoice = _CaseAggregate.CaseAssignmentComplaintInvoice_T1_.HasAny(x => x.CASE_ID == data.CaseID);
            //var hasNotice = _CaseAggregate.CaseAssignmentComplaintNotice_T1_.HasAny(x => x.CASE_ID == data.CaseID);


            if (hasAnyAssignment == false)
            {
                return data;
            }

            await Validator(flowable);

            var con = new MSSQLCondition<CASE>(x => x.CASE_ID == data.CaseID);

            con.ActionModify(x =>
            {
                x.CASE_TYPE = (byte)CaseType.Process;
                x.UPDATE_DATETIME = DateTime.Now;
                x.UPDATE_USERNAME = ContextUtility.GetUserIdentity().Name;

            });          

            return _CaseAggregate.Case_T1_T2_.Update(con);
        }

        public async global::System.Threading.Tasks.Task Validator(IFlowable flowable, params object[] args)
        {
        }
    }
}
