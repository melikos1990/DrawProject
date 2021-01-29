#define Test

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Case;
using SMARTII.Domain.Master;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Domain.Transaction;
using SMARTII.Resource.Tag;
using SMARTII.Service.Cache;
using SMARTII.Service.Organization.Provider;

namespace SMARTII.Service.Case.Task
{
    /// <summary>
    /// 案件結案TASK
    /// </summary>
    public class CaseFinishedTask : TaskBase, IFlowableTask
    {
        private readonly ICaseService _CaseService;
        private readonly ICaseAggregate _CaseAggregate;
        private readonly IMasterAggregate _MasterAggregate;
        private readonly ICaseFacade _CaseFacade;
        private readonly HeaderQuarterNodeProcessProvider _HeaderQuarterNodeProcessProvider;

        public CaseFinishedTask(
            ICaseService CaseService,
            ICaseAggregate CaseAggregate,
            IOrganizationAggregate OrganizationAggregate,
            ICaseFacade CaseFacade,
            IMasterAggregate MasterAggregate,
            HeaderQuarterNodeProcessProvider HeaderQuarterNodeProcessProvider) : base(OrganizationAggregate)
        {
            _CaseService = CaseService;
            _CaseAggregate = CaseAggregate;
            _CaseFacade = CaseFacade;
            _MasterAggregate = MasterAggregate;
            _HeaderQuarterNodeProcessProvider = HeaderQuarterNodeProcessProvider;
        }

        public async Task<IFlowable> Execute(IFlowable flowable, params object[] args)
        {
            var data = (Domain.Case.Case)flowable;

            await Validator(flowable, args);

            data.CaseType = CaseType.Finished;
            data.FinishDateTime = DateTime.Now;
            data.FinishUserName = ContextUtility.GetUserIdentity()?.Name;


            // 更新案件整體
            // 更新已經包含了結案內容。
            var @case = _CaseService.UpdateComplete(data,true);

            var user = ContextUtility.GetUserIdentity()?.Instance;

            _CaseFacade.CreateResume(@case.CaseID, null, SysCommon_lang.CASE_FINISH_RESUME, Case_lang.CASE_FINISH, user);

            // 新增歷程後記錄
            using (var scope = TrancactionUtility.NoTransactionScope())
            {
                global::System.Threading.Tasks.Task.Run(() =>
                {
                    _CaseFacade.CreateHistory(@case.CaseID, @case, Case_lang.CASE_FINISH, user);
                });
            };

            return @case;
        }

        public async global::System.Threading.Tasks.Task Validator(IFlowable flowable, params object[] args)
        {

            bool isFastFinished = (bool)args?[0];

            var data = (Domain.Case.Case)flowable;
            var user = ContextUtility.GetUserIdentity().Instance;
            var message = string.Empty;

            if(_CaseAggregate.Case_T1_T2_.HasAny(x=>x.CASE_ID == data.CaseID && x.CASE_TYPE == (byte)CaseType.Finished))
                throw new IndexOutOfRangeException(Case_lang.CASE_ALREADY_FINISH);


            if (string.IsNullOrEmpty(data.FinishContent))
                throw new IndexOutOfRangeException(Case_lang.CASE_FINISH_CONTENT_FAIL);

            var con = new MSSQLCondition<CASE_ASSIGNMENT>(x => x.CASE_ID == data.CaseID);


            // 取得BU三碼        
            var term = (HeaderQuarterTerm)
                _HeaderQuarterNodeProcessProvider.GetTerm(data.NodeID, data.OrganizationType);

            if (isFastFinished && DataStorage.CanFastFinishedCaseArray.Contains(term.NodeKey) == false)
                throw new IndexOutOfRangeException(Case_lang.CASE_ASSIGNMENT_NOT_ALL_CLOSED);


            // 判斷底下是否有未結案之轉派
            var hasAnyProcessAssign = _CaseAggregate.CaseAssignment_T1_T2_.HasAny(
                                        x => x.CASE_ID == data.CaseID &&
                                        x.CASE_ASSIGNMENT_TYPE != (byte)CaseAssignmentType.Finished);

            if (hasAnyProcessAssign)
                throw new IndexOutOfRangeException(Case_lang.CASE_ASSIGNMENT_NOT_ALL_CLOSED);

            //結案分類 必選檢查
            var conCf = new MSSQLCondition<CASE_FINISH_REASON_CLASSIFICATION>(x => x.NODE_ID == data.NodeID && x.IS_ENABLED && x.IS_REQUIRED);
            conCf.IncludeBy(x => x.CASE_FINISH_REASON_DATA);

            var cfcList = _MasterAggregate.CaseFinishReasonClassification_T1_T2_.GetList(conCf).ToList();

            //沒有必選 不用檢查
            if (cfcList.Any())
            {
                var checkList = data.CaseFinishReasonDatas?.Select(x => x.ID).ToList() ?? new List<int>();

                cfcList.ForEach(x => {
                    var ids = x.CaseFinishReasonDatas.Select(c => c.ID);
                    if (ids.Intersect(checkList).Any() == false)
                    {
                        throw new IndexOutOfRangeException($"{x.Title} => " + Case_lang.CASE_FINISH_REASON_CLASSIFICATION_FAIL);
                    }
                });
            }

            if (this.SpecialValidator(term, data, out message) == false)
            {
                throw new Exception(message);
            }

            // 驗證自己是否有權限能夠立案
            // 需檢核是否有該節點Group的權限 (DATARANGE)
            if (data.GroupID.HasValue)
            {
                if (!base.HasGroupAuth(data.GroupID.Value))
                    throw new NullReferenceException(Case_lang.CASE_NO_GROUP_AUTH);

                // 檢驗該Group 是否為協同銷案 , 如果不是就需要檢核負責人代號
                var groupNode = _OrganizationAggregate.CallCenterNode_T1_T2_.Get(x => x.NODE_ID == data.GroupID);

                if (groupNode == null)
                    throw new NullReferenceException(Common_lang.NOT_FOUND_DATA);

                if(groupNode.WorkProcessType == WorkProcessType.Individual && data.ApplyUserID != user.UserID)
                    throw new NullReferenceException(Case_lang.CASE_FINISH_ERROR_USER_NOT_MATCH_ID);


            }

         
          
        }


        private bool SpecialValidator(HeaderQuarterTerm term, IFlowable flowable, out string message)
        {

            message = string.Empty;

            var data = (Domain.Case.Case)flowable;


            if (term.NodeKey == EssentialCache.BusinessKeyValue.PPCLIFE)
            {
                #region  選擇特定問題分類 驗證結案原因


                var questionIDs = _MasterAggregate.QuestionClassification_T1_.GetListOfSpecific(
                        new MSSQLCondition<QUESTION_CLASSIFICATION>(x => EssentialCache.PPCLifeCustomerValue.SpecialClassification.Contains(x.CODE)), 
                        x => x.ID).ToList();

                // 沒有選擇到特定問題分類時就返回
                if (!questionIDs.Any(x => x == data.QuestionClassificationID)) return true;
                
                // 找出 處置原因 => 問題要因 
                var con = new MSSQLCondition<CASE_FINISH_REASON_CLASSIFICATION>(x => 
                    x.KEY == EssentialCache.ReasonClassValue.FACTORS && 
                    x.IS_ENABLED);

                con.IncludeBy(x => x.CASE_FINISH_REASON_DATA);

                var finishClassifcation = _MasterAggregate.CaseFinishReasonClassification_T1_T2_.GetOfSpecific(con, x => new { Title = x.TITLE, ReasonDatas = x.CASE_FINISH_REASON_DATA });

                // 取得 DB 和 UI 的處置原因集合
                var checkList = data.CaseFinishReasonDatas?.Select(x => x.ID).ToList() ?? new List<int>();
                var validaList = finishClassifcation.ReasonDatas.Select(x => x.ID).ToList();

                // 2個集合 如果沒有交集 就拋例外
                if (checkList.Intersect(validaList).Any() == false)
                {
                    message = $"{finishClassifcation.Title} => " + Case_lang.CASE_FINISH_REASON_CLASSIFICATION_FAIL;

                    return false;
                }

                #endregion

            }


            return true;
        }

    }
}
