using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac.Features.Indexed;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Areas.Case.Models;
using SMARTII.Assist.Authentication;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Case;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Report;
using SMARTII.Domain.Thread;
using SMARTII.Service.Case.Flow;
using SMARTII.Service.Case.Resolver;
using SMARTII.Service.Organization.Provider;
using System.IO;
using SMARTII.Domain.IO;
using SMARTII.Domain.Notification.Email;
using SMARTII.Service.Master.Resolver;
using SMARTII.Service.Cache;
using SMARTII.Assist.Web;
using SMARTII.Service.Organization.Resolver;
using SMARTII.Resource.Tag;
using SMARTII.Domain.Transaction;
using SMARTII.Domain.Cache;
using SMARTII.Assist.Logger;

namespace SMARTII.Areas.Case.Controllers
{
    [Authentication]
    public class CaseAssignmentController : CaseBaseApiController
    {
        private readonly IIndex<string, IFlow> _Flows;
        private readonly ICaseAggregate _CaseAggregate;
        private readonly ICommonAggregate _CommonAggregate;
        private readonly ICaseAssignmentService _CaseAssignmentService;
        private readonly ICaseAssignmentFacade _CaseAssignmentFacade;
        private readonly OrganizationNodeResolver _OrganizationNodeResolver;

        public CaseAssignmentController(
            IIndex<string, IFlow> Flows,
            ICaseAggregate CaseAggregate,
            ICommonAggregate CommonAggregate,
            ICaseAssignmentService CaseAssignmentService,
            ICaseAssignmentFacade CaseAssignmentFacade,
            OrganizationNodeResolver OrganizationNodeResolver)
        {
            _Flows = Flows;
            _CaseAggregate = CaseAggregate;
            _CommonAggregate = CommonAggregate;
            _CaseAssignmentService = CaseAssignmentService;
            _CaseAssignmentFacade = CaseAssignmentFacade;
            _OrganizationNodeResolver = OrganizationNodeResolver;
        }

        /// <summary>
        /// 更新轉派案件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Logger(nameof(Case_lang.CASE_ASSIGNMENT_UPDATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> SaveCaseAssignment(CaseAssignmentViewModel model)
        {
            try
            {
                var domain = model.ToDomain();


                var data = _CaseAssignmentService.Update(domain, GetProcessNodeJob(model.EditorNodeJobID));

                var userName = ContextUtility.GetUserIdentity()?.Name ?? GlobalizationCache.APName;

                using (var scope = TrancactionUtility.NoTransactionScope())
                {
                    // 新增轉派History
                    Task.Run(() => _CaseAssignmentFacade.CreateHistory(data, Case_lang.CASE_ASSIGNMENT_REPLY, userName));
                };

                var payload = new CaseAssignmentViewModel(data);

                var result = new JsonResult<CaseAssignmentViewModel>(payload, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(Case_lang.CASE_ASSIGNMENT_UPDATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Case_lang.CASE_ASSIGNMENT_UPDATE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 銷案案件 (廠商/門市)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Logger(nameof(Case_lang.CASE_HS_VENDOR_FINISH))]
        [ModelValidator(false)]
        public async Task<IHttpResult> ProcessedCaseAssignment(CaseAssignmentViewModel model)
        {
            try
            {
                var domain = model.ToDomain();

                var data = await _Flows[nameof(CaseAssignProcessedFlow)].Run(domain, GetProcessNodeJob(model.EditorNodeJobID));

                var payload = new CaseAssignmentViewModel((CaseAssignment)data);

                var result = new JsonResult<CaseAssignmentViewModel>(payload, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(Case_lang.CASE_HS_VENDOR_FINISH_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Case_lang.CASE_HS_VENDOR_FINISH_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 資料重填 (廠商/門市)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Logger(nameof(Case_lang.CASE_HS_VENDOR_SAVEREFILL))]
        [ModelValidator(false)]
        public async Task<IHttpResult> SaveRefill(CaseAssignmentViewModel model)
        {
            try
            {
                var domain = model.ToDomain();

                //驗證須由同一權責單位重填銷案內容
                if (domain.FinishNodeID != GetProcessNodeJob(model.EditorNodeJobID).NodeID)
                    throw new Exception(SysCommon_lang.CASE_ASSIGNMENT_RESPONES_FAIL);

                var data = await _Flows[nameof(CaseAssignRefillFlow)].Run(domain, GetProcessNodeJob(model.EditorNodeJobID));

                var payload = new CaseAssignmentViewModel((CaseAssignment)data);

                var result = new JsonResult<CaseAssignmentViewModel>(payload, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(Case_lang.CASE_HS_VENDOR_SAVEREFILL_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Case_lang.CASE_HS_VENDOR_SAVEREFILL_FAIL), false)
                    .Async();
            }
        }


        #region MISC

        /// <summary>
        /// 取得歷程回應
        /// </summary>
        /// <param name="caseID"></param>
        /// <param name="assignmentID"></param>
        /// <returns></returns>
        [HttpGet]
        [Logger(nameof(Case_lang.CASE_GETRESUMES))]
        [ModelValidator(false)]
        public async Task<IHttpResult> GetResumes(string caseID, int assignmentID)
        {
            try
            {

                var list = _CaseAggregate.CaseAssignmentResume_T1_T2_
                                         .GetList(x => x.CASE_ID == caseID && x.CASE_ASSIGNMENT_ID == assignmentID)
                                         .OrderBy(x => x.CreateDateTime);

                var index = 0;
                var payload = list.Select(x =>
                {
                    index++;
                    var model = new CaseAssignmentResumeViewModel(x)
                    {
                        Index = index
                    };
                    return model;
                }).ToList();

                var result = new JsonResult<List<CaseAssignmentResumeViewModel>>(payload, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(Case_lang.CASE_GETRESUMES_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Case_lang.CASE_GETRESUMES_FAIL), false)
                    .Async();
            }
        }


        #endregion



    }
}
