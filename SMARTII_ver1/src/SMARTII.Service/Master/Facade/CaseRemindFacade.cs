using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Case;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;

namespace SMARTII.Service.Master.Facade
{
    public class CaseRemindFacade : ICaseRemindFacade
    {

        private readonly ICaseAggregate _CaseAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        public CaseRemindFacade(ICaseAggregate CaseAggregate,
                                 IOrganizationAggregate OrganizationAggregate)
        {
            _CaseAggregate = CaseAggregate;
            _OrganizationAggregate = OrganizationAggregate;
        }
        // <summary>
        /// 新增案件追蹤
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task Create(Domain.Case.CaseRemind data)
        {


            if (data.ActiveStartDateTime <= DateTime.Now)
                throw new Exception(CaseRemind_lang.CASEREMIND_DATETIME_BELOW_NOW);


            #region 驗證案件編號/轉派編號


            // 驗證案件是否存在
            var _case = _CaseAggregate.Case_T1_T2_.Get(x => x.CASE_ID == data.CaseID);
            if (_case == null)
                throw new Exception(CaseRemind_lang.CASEREMIND_CASE_ID_NOT_FOUND);

           
            // 驗證轉派是否存在
            if (data.AssignmentID != null)
            {
                var isExisitCaseAssignment = _CaseAggregate.CaseAssignment_T1_.HasAny(x => x.ASSIGNMENT_ID == data.AssignmentID && x.CASE_ID == data.CaseID);
                if (isExisitCaseAssignment == false)
                    throw new Exception(CaseRemind_lang.CASEREMIND_ASSIGNMENT_ID_NOT_FOUND);
            }

            // 驗證使用者是否有未啟用的
            var userIDs = data?.UserIDs ?? new List<string>();
            var hasNotEnable = _OrganizationAggregate.User_T1_T2_.HasAny(x => userIDs.Contains(x.USER_ID) && x.IS_ENABLED == false);
            if (hasNotEnable)
                throw new Exception(CaseRemind_lang.CASEREMIND_USER_NOT_ISENABLED);

            #endregion

            data.NodeID = _case.NodeID;
            data.CreateDateTime = DateTime.Now;
            data.CreateUserName = ContextUtility.GetUserIdentity()?.Name;
            data.CreateUserID = ContextUtility.GetUserIdentity().Instance.UserID;

            var result = _CaseAggregate.CaseRemind_T1_T2_.Add(data);

            await result.Async();
        }
        /// <summary>
        /// 更新案件追蹤
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task Update(Domain.Case.CaseRemind data)
        {

            var con = new MSSQLCondition<CASE_REMIND>(x => x.ID == data.ID);


            // 啟用後不可編輯追蹤
            var caseRemind = _CaseAggregate.CaseRemind_T1_T2_.Get(con);
            if (caseRemind.ActiveStartDateTime <= DateTime.Now)
                throw new Exception(CaseRemind_lang.CASEREMIND_OVERACTIVESTARTTIME);


            if (data.ActiveStartDateTime <= DateTime.Now)
                throw new Exception(CaseRemind_lang.CASEREMIND_DATETIME_BELOW_NOW);

            #region 驗證案件編號/轉派編號


            // 驗證案件是否存在
            var isExitCaseNode = _CaseAggregate.Case_T1_T2_.HasAny(x => x.CASE_ID == data.CaseID);
            if (isExitCaseNode == false)
                throw new Exception(CaseRemind_lang.CASEREMIND_CASE_ID_NOT_FOUND);

            // 驗證轉派是否存在
            if (data.AssignmentID != null)
            {
                var isExitCaseAssignment = _CaseAggregate.CaseAssignment_T1_.HasAny(x => x.ASSIGNMENT_ID == data.AssignmentID && x.CASE_ID == data.CaseID);
                if (isExitCaseAssignment == false)
                    throw new Exception(CaseRemind_lang.CASEREMIND_ASSIGNMENT_ID_NOT_FOUND);
            }

            // 驗證使用者是否有未啟用的
            var userIDs = data?.UserIDs ?? new List<string>();
            var hasNotEnable = _OrganizationAggregate.User_T1_T2_.HasAny(x => userIDs.Contains(x.USER_ID) && x.IS_ENABLED == false);
            if (hasNotEnable)
                throw new Exception(CaseRemind_lang.CASEREMIND_USER_NOT_ISENABLED);


            #endregion


            con.ActionModify(x =>
            {
                x.CONTENT = data.Content;
                x.ACTIVE_START_DAETTIME = data.ActiveStartDateTime;
                x.ACTIVE_END_DAETTIME = data.ActiveEndDateTime;
                x.TYPE = (byte)data.Type;
                x.UPDATE_DATETIME = DateTime.Now;
                x.UPDATE_USERNAME = ContextUtility.GetUserIdentity()?.Name;


                x.USER_IDs = JsonConvert.SerializeObject(data.UserIDs);
                
            });

            var result = _CaseAggregate.CaseRemind_T1_T2_.Update(con);

            await result.Async();

        }
    }
}
