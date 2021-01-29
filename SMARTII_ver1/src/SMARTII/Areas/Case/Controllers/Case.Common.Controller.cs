using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Areas.Case.Models;
using SMARTII.Areas.Common.Models.Organization;
using SMARTII.Areas.Master.Models.CaseRemind;
using SMARTII.Areas.Master.Models.CaseTemplate;
using SMARTII.Areas.Master.Models.QuestionClassificationGuide;
using SMARTII.Areas.Model;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;
using SMARTII.Domain.IO;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Report;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;
using SMARTII.Service.Cache;
using SMARTII.Assist.Logger;

namespace SMARTII.Areas.Case.Controllers
{
    public partial class CaseController
    {
        /// <summary>
        /// 取得快速結案原因清單
        /// </summary>
        /// <param name="buID"></param>
        /// <returns></returns>
        [HttpGet]
        [Logger(nameof(Case_lang.CASE_GETLIST_FASTFINISHEDREASON))]
        [ModelValidator(false)]
        public async Task<IHttpResult> GetFastFinishedReasons(int? buID = null)
        {
            try
            {
                var con = new MSSQLCondition<CASE_TEMPLATE>(x => x.IS_FAST_FINISH);

                var list = _MasterAggregate.CaseTemplate_T1_T2_
                                           .GetList(x => x.NODE_ID == buID && x.IS_FAST_FINISH);

                var ui = list.Select(x => new CaseTemplateListViewModel(x))
                             .ToList();

                var result = new JsonResult<List<CaseTemplateListViewModel>>(ui, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(Case_lang.CASE_GETLIST_FASTFINISHEDREASON_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Case_lang.CASE_GETLIST_FASTFINISHEDREASON_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 取得預立案清單
        /// </summary>
        /// <param name="buID"></param>
        /// <returns></returns>
        [HttpGet]
        [Logger(nameof(Case_lang.CASE_GET_PREVENT_LIST))]
        [ModelValidator(false)]
        public async Task<IHttpResult> GetPreventionCaseList(int? buID = null)
        {
            try
            {
                var con = new MSSQLCondition<CASE_SOURCE>(x => x.NODE_ID == buID &&
                                                               x.ORGANIZATION_TYPE == (byte)OrganizationType.HeaderQuarter &&
                                                               x.IS_PREVENTION);

                con.IncludeBy(x => x.CASE_SOURCE_USER);

                var sourceList = _CaseAggregate.CaseSource_T1_T2_
                                               .GetList(con)
                                               .Select(x => new CaseSourcePreventionListViewModel(x))
                                               .ToList();

                var result = new JsonResult<IEnumerable<CaseSourcePreventionListViewModel>>(sourceList, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                 ex.PrefixDevMessage(Case_lang.CASE_GET_PREVENT_LIST_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Case_lang.CASE_GET_PREVENT_LIST_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 取得近期未結案件清單
        /// </summary>
        /// <param name="buID"></param>
        /// <returns></returns>
        [HttpPost]
        [Logger(nameof(Case_lang.CASE_GET_NEARLY_LIST))]
        [ModelValidator(false)]
        public async Task<IHttpResult> GetNearlyCaseList(ConcatableUserViewModel model, [FromUri] int? buID = null)
        {
            try
            {
                var term =
                    _HeaderQuarterNodeProcessProvider.GetTerm(buID.Value, OrganizationType.HeaderQuarter);

                var parameters = new BusinesssUnitParameters((HeaderQuarterTerm)term);

                var end = DateTime.Now;
                var start = DateTime.Now.AddDays(-parameters.CaseNearlyDays.Value);

                var con = new MSSQLCondition<CASE>(x => x.NODE_ID == buID &&
                                                        x.ORGANIZATION_TYPE == (byte)OrganizationType.HeaderQuarter &&
                                                        x.CREATE_DATETIME >= start &&
                                                        x.CREATE_DATETIME < end);

                con.And(x =>
                            x.CASE_CONCAT_USER.Any(g => g.UNIT_TYPE == (int)model.UnitType) ||
                            x.CASE_COMPLAINED_USER.Any(g => g.UNIT_TYPE == (int)model.UnitType)
                        );

                con.IncludeBy(x => x.CASE_COMPLAINED_USER);
                con.IncludeBy(x => x.CASE_CONCAT_USER);

                var sourceList = _CaseAggregate.Case_T1_T2_
                                               .GetList(con)
                                               .WhereObject(new User
                                               {
                                                   Name = model.UserName,
                                                   Mobile = model.Mobile,
                                                   Email = model.Email
                                               })
                                               .OrderByDescending(x => x.CreateDateTime)
                                               .Select(x => new CaseNearlyListViewModel(x))
                                               .ToList();

                var result = new JsonResult<IEnumerable<CaseNearlyListViewModel>>(sourceList, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                 ex.PrefixDevMessage(Case_lang.CASE_GET_NEARLY_LIST_SUCCESS));

                return await new JsonResult(
                    ex.PrefixMessage(Case_lang.CASE_GET_NEARLY_LIST_SUCCESS), false)
                    .Async();
            }
        }

        /// <summary>
        /// 取得案件底下的轉派清單
        /// </summary>
        /// <param name="caseID"></param>
        /// <returns></returns>
        [HttpGet]
        [Logger(nameof(Case_lang.CASE_GETLIST_ASSIGNMENT))]
        [ModelValidator(false)]
        public async Task<IHttpResult> GetCaseAssignmentAggregate(string caseID)
        {
            try
            {
                var assignments = _CaseAssignmentFacade.GetAssignmentAggregate(caseID);
                int index = 1;
                var list = assignments
                    .OrderBy(x => x.CreateDateTime)
                    .Select(x =>
                    {
                        var model = new CaseAssignmentOverviewViewModel(x);
                        model.Index = index;
                        index++;
                        return model;
                    })
                    .ToList();

                var result = new JsonResult<IEnumerable<CaseAssignmentOverviewViewModel>>(list, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                 ex.PrefixDevMessage(Case_lang.CASE_GETLIST_ASSIGNMENT_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Case_lang.CASE_GETLIST_ASSIGNMENT_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 找到流程引導
        /// </summary>
        /// <param name="questionClassificationID"></param>
        /// <param name="isSelfSetting"></param>
        /// <returns></returns>
        [HttpGet]
        [Logger(nameof(Case_lang.CASE_GET_QUESTIONCLASSIFICATIONGUIDE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> GetQuestionClassificationGuides(bool isSelfSetting, int? questionClassificationID)
        {
            try
            {
                var vwQuestionClassification = _MasterAggregate.VWQuestionClassification
                                                               .Get(x => x.ID == questionClassificationID &&
                                                                         x.ORGANIZATION_TYPE == (byte)OrganizationType.HeaderQuarter);

                // 是否依據個人參數設定顯示
                var userID = ContextUtility.GetUserIdentity()?.Instance.UserID ?? "";

                var con = new MSSQLCondition<USER>();
                con.And(x => x.USER_ID == userID);
                con.IncludeBy(x => x.USER_PARAMETER);
                var userStatus = _OrganizationAggregate.User_T1_T2_.Get(con);

                if (isSelfSetting && !userStatus.UserParameter.NavigateOfNewbie)
                {
                    var resultNull = new JsonResult<IEnumerable<QCGuideListViewModel>>(new List<QCGuideListViewModel>(), true);

                    return await resultNull.Async();
                }

                var parentNodes =
                    Array.ConvertAll(vwQuestionClassification.PARENT_PATH?.Split('@'), x => int.Parse(x));

                var list = _MasterAggregate.VWQuestionClassificationGuide_QuestionClassificationGuide_
                                           .GetList(x => parentNodes.Contains(x.CLASSIFICATION_ID));

                var ui = list.Select(x => new QCGuideListViewModel(x)).ToList();

                var result = new JsonResult<IEnumerable<QCGuideListViewModel>>(ui, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(Case_lang.CASE_GET_QUESTIONCLASSIFICATIONGUIDE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Case_lang.CASE_GET_QUESTIONCLASSIFICATIONGUIDE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 瀏覽反應單
        /// </summary>
        /// <param name="buID"></param>
        /// <param name="caseID"></param>
        /// <param name="invoiceID"></param>
        /// <returns></returns>
        [HttpPost]
        [Logger(nameof(Case_lang.CASE_PREVIEW_INVOICE))]
        [ModelValidator(false)]
        public async Task<HttpResponseMessage> GetPreviewComplaintInvoice(int? buID, string caseID, string invoiceID)
        {
            try
            {
                var report = _ReportService.GetComplaintReport(caseID, invoiceID);
                
                var bytes = ReportUtility.ConvertExcelBytesToPDF(report.Buffer);
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new ByteArrayContent(bytes);
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = report.FileName + ".pdf";
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

                return await response.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                response.ReasonPhrase = ex.Message.Encoding();                

                return await response.Async();
            }
        }

        /// <summary>
        /// 取得歷程清單
        /// </summary>
        /// <param name="buID"></param>
        /// <returns></returns>
        [HttpGet]
        [Logger(nameof(Case_lang.CASE_GET_ASSIGNMENT_RESUME))]
        [ModelValidator(false)]
        public async Task<IHttpResult> GetCaseResumeList(string caseID)
        {
            try
            {
                var con = new MSSQLCondition<CASE_RESUME>(x => x.CASE_ID == caseID);

                var sourceList = _CaseAggregate.CaseResume_T1_T2_
                                               .GetList(con)
                                               .OrderByDescending(x => x.CreateDateTime)
                                               .Select(x => new CaseResumeListViewModel(x))
                                               .ToList();

                var result = new JsonResult<IEnumerable<CaseResumeListViewModel>>(sourceList, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                 ex.PrefixDevMessage(Case_lang.CASE_GET_ASSIGNMENT_RESUME_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Case_lang.CASE_GET_ASSIGNMENT_RESUME_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 取得案件來源(依BU)
        /// </summary>
        /// <param name="buID"></param>
        /// <returns></returns>
        [HttpGet]
        [Logger(nameof(Case_lang.GET_SOURCE_NATIVE))]
        [ModelValidator(false)]
        public async Task<IHttpActionResult> GetCaseSourceType(int buID)
        {
            try
            {
                var caseSources = new List<SelectItem>();
                var nodeKey = _OrganizationAggregate.HeaderQuarterNode_T1_.GetOfSpecific(
                        new MSSQLCondition<HEADQUARTERS_NODE>(x => x.NODE_ID == buID),
                        x => x.NODE_KEY
                    );

                DataStorage.CaseSourceDict.TryGetValue(nodeKey, out caseSources);

                return Ok(caseSources);
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return Json(new List<SelectItem>());
            }
        }

        /// <summary>
        /// 取得案件追蹤(立案畫面用)
        /// </summary>
        /// <param name="caseIDs"></param>
        /// <returns></returns>
        [HttpPost]
        [Logger(nameof(Case_lang.CASE_GET_REMINDFROMCASEPAGE))]
        //[ModelValidator(false)]
        public async Task<IHttpActionResult> GetCaseRemindFromCasePage([FromUri]int[] caseRemindIDs)
        {
            try
            {
                var con = new MSSQLCondition<CASE_REMIND>();

                caseRemindIDs.ToList().ForEach(id => con.Or(x => x.ID == id));

                var list = _CaseAggregate.CaseRemind_T1_T2_.GetList(con);

                var ui = _OrganizationNodeResolver.ResolveCollection(list);

                var result = ui.Select(x => new CaseRemindListViewModel(x)).ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return Json(new List<int>());
            }
        }
    }
}
