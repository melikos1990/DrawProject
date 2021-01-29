using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac.Features.Indexed;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Areas.Case.Models;
using SMARTII.Areas.Common.Models.Organization;
using SMARTII.Areas.Master.Models.CaseTemplate;
using SMARTII.Areas.Master.Models.QuestionClassificationGuide;
using SMARTII.Areas.Model;
using SMARTII.Assist.Authentication;
using SMARTII.Assist.Web;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Case;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.IO;
using SMARTII.Domain.Master;
using SMARTII.Domain.Notification.Email;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Report;
using SMARTII.Domain.Thread;
using SMARTII.Service.Cache;
using SMARTII.Service.Case.Flow;
using SMARTII.Service.Case.Task;
using SMARTII.Assist.Logger;
using SMARTII.Resource.Tag;

namespace SMARTII.Areas.Case.Controllers
{
    public partial class CaseController
    {
        /// <summary>
        /// 新增歷程 (轉派)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Logger(nameof(Case_lang.CASE_ASSIGNMENT_SAVE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> SaveCaseAssignment(CaseAssignmentViewModel model)
        {
            try
            {
                var domain = model.ToDomain();

                var data = await _Flows[nameof(CaseAssignFlow)].Run(domain);

                var result = new JsonResult<CaseAssignmentViewModel>(new CaseAssignmentViewModel((CaseAssignment)data), true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(Case_lang.CASE_ASSIGNMENT_SAVE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Case_lang.CASE_ASSIGNMENT_SAVE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 寄出 轉派
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Logger(nameof(Case_lang.CASE_ASSIGNMENT_INVOICE_SEND))]
        [ModelValidator(false)]
        public async Task<IHttpResult> SendCaseAssignment([FromUri]int assignmentID, [FromUri]string caseID, EmailPayload emailPayload)
        {
            try
            {
                var domain = new CaseAssignment()
                {
                    AssignmentID = assignmentID,
                    CaseID = caseID
                };

                var data = await _Flows[nameof(CaseAssignmentSenderFlow)].Run(domain, emailPayload);

                var result = new JsonResult<string>(Case_lang.CASE_ASSIGNMENT_INVOICE_SEND_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(Case_lang.CASE_ASSIGNMENT_INVOICE_SEND_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Case_lang.CASE_ASSIGNMENT_INVOICE_SEND_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 新增歷程 (開立反應單)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Logger(nameof(Case_lang.CASE_ASSIGNMENT_INVOICE_SAVE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> SaveCaseAssignmentInvoice(CaseAssignmentInvoiceViewModel model)
        {
            try
            {
                var domain = model.ToDomain();

                var data = await _Flows[nameof(CaseAssignFlow)].Run(domain);

                var result = new JsonResult<CaseAssignmentComplaintInvoice>(((CaseAssignmentComplaintInvoice)data), true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(Case_lang.CASE_ASSIGNMENT_INVOICE_SAVE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Case_lang.CASE_ASSIGNMENT_INVOICE_SAVE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 新增歷程 (溝通歷程)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Logger(nameof(Case_lang.CASE_ASSIGNMENT_COMMUNICATION))]
        [ModelValidator(false)]
        public async Task<IHttpResult> SaveCaseAssignmentCommunicate(CaseAssignmentCommunicateViewModel model)
        {
            try
            {
                var domain = model.ToDomain();

                var data = await _Flows[nameof(CaseAssignmentCommunicationFlow)].Run(domain);

                var result = new JsonResult<CaseAssignmentCommunicate>(((CaseAssignmentCommunicate)data), true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(Case_lang.CASE_ASSIGNMENT_COMMUNICATION_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Case_lang.CASE_ASSIGNMENT_COMMUNICATION_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 更新反應單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Logger(nameof(Case_lang.CASE_ASSIGNMENT_INVOICE_UPDATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> UpdateCaseAssignmentInvoice(CaseAssignmentInvoiceViewModel model)
        {
            try
            {
                var domain = model.ToDomain();

                var data = _CaseAssignmentService.UpdateInvoice(domain);

                var result = new JsonResult<string>(Case_lang.CASE_ASSIGNMENT_INVOICE_UPDATE_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(Case_lang.CASE_ASSIGNMENT_INVOICE_UPDATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Case_lang.CASE_ASSIGNMENT_INVOICE_UPDATE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 寄出反應單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Logger(nameof(Case_lang.CASE_ASSIGNMENT_INVOICE_SEND))]
        [ModelValidator(false)]
        public async Task<IHttpResult> SendCaseAssignmentInvoice([FromUri]int identityID, EmailPayload emailPayload)
        {
            try
            {
                var domain = new CaseAssignmentComplaintInvoice()
                {
                    ID = identityID
                };

                var data = await _Flows[nameof(CaseComplaintInvoiceSenderFlow)].Run(domain, emailPayload);

                var result = new JsonResult<string>(Case_lang.CASE_ASSIGNMENT_INVOICE_SEND_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(Case_lang.CASE_ASSIGNMENT_INVOICE_SEND_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Case_lang.CASE_ASSIGNMENT_INVOICE_SEND_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 反應單重送
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Logger(nameof(Case_lang.CASE_ASSIGNMENT_INVOICE_RESEND))]
        [ModelValidator(false)]
        public async Task<IHttpResult> ResendCaseAssignmentInvoice([FromUri]int identityID, EmailPayload emailPayload)
        {
            try
            {
                var domain = new CaseAssignmentComplaintInvoice();
                domain.ID = identityID;

                var data = await _Flows[nameof(CaseComplaintInvoiceResendFlow)].Run(domain, emailPayload);

                var result = new JsonResult<string>(((CaseAssignmentComplaintInvoice)data).InvoiceID, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(Case_lang.CASE_ASSIGNMENT_INVOICE_RESEND_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Case_lang.CASE_ASSIGNMENT_INVOICE_RESEND_FAIL), false)
                    .Async();
            }
        }


        /// <summary>
        /// 新增歷程 (一般通知)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Logger(nameof(Case_lang.CASE_ASSIGNMENT_NOTICE_SAVE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> SaveCaseAssignmentNotice(CaseAssignmentNoticeViewModel model)
        {
            try
            {
                var domain = model.ToDomain();

                var data = await _Flows[nameof(CaseAssignFlow)].Run(domain, model.EmailPayload);

                var result = new JsonResult<string>(Case_lang.CASE_ASSIGNMENT_NOTICE_SAVE_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(Case_lang.CASE_ASSIGNMENT_NOTICE_SAVE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Case_lang.CASE_ASSIGNMENT_NOTICE_SAVE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 更新一般通知
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Logger(nameof(Case_lang.CASE_ASSIGNMENT_NOTICE_UPDATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> UpdateCaseAssignmentNotice(CaseAssignmentNoticeViewModel model)
        {
            try
            {
                var domain = model.ToDomain();

                var data = _CaseAssignmentService.UpdateNotice(domain);

                var result = new JsonResult<string>(Case_lang.CASE_ASSIGNMENT_NOTICE_UPDATE_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(Case_lang.CASE_ASSIGNMENT_NOTICE_UPDATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Case_lang.CASE_ASSIGNMENT_NOTICE_UPDATE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 銷案駁回
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Logger(nameof(Case_lang.CASE_ASSIGNMENT_REJECT))]
        [ModelValidator(false)]
        public async Task<IHttpResult> RejectCaseAssigment(CaseAssignmentViewModel model)
        {
            try
            {
                var domain = model.ToDomain();

                var data = _Flows[nameof(CaseAssignRejectFlow)].Run(domain);

                var result = new JsonResult<string>(Case_lang.CASE_ASSIGNMENT_REJECT_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(Case_lang.CASE_ASSIGNMENT_REJECT_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Case_lang.CASE_ASSIGNMENT_REJECT_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 銷案案件 (客服)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Logger(nameof(Case_lang.CASE_ASSIGNMENT_CC_FINISH))]
        [ModelValidator(false)]
        public async Task<IHttpResult> FinishedCaseAssignment(CaseAssignmentViewModel model)
        {
            try
            {
                var domain = model.ToDomain();

                var data = await _Flows[nameof(CaseAssignFinishFlow)].Run(domain);

                var payload = new CaseAssignmentViewModel((CaseAssignment)data);

                var result = new JsonResult<CaseAssignmentViewModel>(payload, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(Case_lang.CASE_ASSIGNMENT_CC_FINISH_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Case_lang.CASE_ASSIGNMENT_CC_FINISH_FAIL), false)
                    .Async();
            }
        }



        /// <summary>
        /// 取得一般通知資料
        /// </summary>
        /// <param name="caseID"></param>
        /// <returns></returns>
        [HttpGet]
        [Logger(nameof(Case_lang.CASE_ASSIGNMENT_NOTICE_GET))]
        [ModelValidator(false)]
        public async Task<IHttpResult> GetCaseAssignmentNotice(int noticeID)
        {
            try
            {
                var con = new MSSQLCondition<CASE_ASSIGNMENT_COMPLAINT_NOTICE>(x => x.ID == noticeID);

                con.IncludeBy(x => x.CASE_ASSIGNMENT_COMPLAINT_NOTICE_USER);
                con.IncludeBy(x => x.CASE);
                con.IncludeBy(x => x.CASE.CASE_COMPLAINED_USER);
                con.IncludeBy(x => x.CASE.CASE_CONCAT_USER);

                var data = _CaseAggregate.CaseAssignmentComplaintNotice_T1_T2_.Get(con);

                var model = new CaseAssignmentNoticeViewModel(data);

                model.Case =
                   new CaseDetailViewModel(_QuestionClassificationResolver.Resolve(data.Case));

                var result = new JsonResult<CaseAssignmentNoticeViewModel>(model, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(Case_lang.CASE_ASSIGNMENT_NOTICE_GET_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Case_lang.CASE_ASSIGNMENT_NOTICE_GET_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 取得反應單資料
        /// </summary>
        /// <param name="identityID"></param>
        /// <returns></returns>
        [HttpGet]
        [Logger(nameof(Case_lang.CASE_ASSIGNMENT_INVOICE_GET))]
        [ModelValidator(false)]
        public async Task<IHttpResult> GetCaseAssignmentInvoice(int identityID)
        {
            try
            {
                var con = new MSSQLCondition<CASE_ASSIGNMENT_COMPLAINT_INVOICE>(x => x.ID == identityID);

                con.IncludeBy(x => x.CASE_ASSIGNMENT_COMPLAINT_INVOICE_USER);
                con.IncludeBy(x => x.CASE);
                con.IncludeBy(x => x.CASE.CASE_COMPLAINED_USER);
                con.IncludeBy(x => x.CASE.CASE_CONCAT_USER);

                var data = _CaseAggregate.CaseAssignmentComplaintInvoice_T1_T2_.Get(con);

                // 取得BU三碼
                var term = (HeaderQuarterTerm)
                    _HeaderQuarterNodeProcessProvider.GetTerm(data.NodeID, OrganizationType.HeaderQuarter);

                var model = new CaseAssignmentInvoiceViewModel(data);

                if (DataStorage.CaseAssignmentComplaintInvoiceTypeDict.TryGetValue(term.NodeKey, out var items))
                    model.InvoiceTypeName = items.FirstOrDefault(x => x.id == model.InvoiceType)?.text;

                model.Case =
                   new CaseDetailViewModel(_QuestionClassificationResolver.Resolve(data.Case));

                var result = new JsonResult<CaseAssignmentInvoiceViewModel>(model, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(Case_lang.CASE_ASSIGNMENT_INVOICE_GET_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Case_lang.CASE_ASSIGNMENT_INVOICE_GET_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 取得轉派資料
        /// </summary>
        /// <param name="caseID"></param>
        /// <param name="assignmentID"></param>
        /// <returns></returns>
        [HttpGet]
        [Logger(nameof(Case_lang.CASE_ASSIGNMENT_GET))]
        [ModelValidator(false)]
        public async Task<IHttpResult> GetCaseAssignment(string caseID, int assignmentID)
        {
            try
            {
                var con = new MSSQLCondition<CASE_ASSIGNMENT>(x => x.CASE_ID == caseID &&
                                                                   x.ASSIGNMENT_ID == assignmentID);

                con.IncludeBy(x => x.CASE_ASSIGNMENT_CONCAT_USER);
                con.IncludeBy(x => x.CASE_ASSIGNMENT_USER);
                con.IncludeBy(x => x.CASE);
                con.IncludeBy(x => x.CASE.CASE_COMPLAINED_USER);
                con.IncludeBy(x => x.CASE.CASE_CONCAT_USER);

                var data = _CaseAggregate.CaseAssignment_T1_T2_.Get(con);

                var model = new CaseAssignmentViewModel(data);

                model.Case =
                    new CaseDetailViewModel(_QuestionClassificationResolver.Resolve(data.Case));
                
                var result = new JsonResult<CaseAssignmentViewModel>(model, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(Case_lang.CASE_ASSIGNMENT_GET_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Case_lang.CASE_ASSIGNMENT_GET_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 取得單位溝通資料
        /// </summary>
        /// <param name="communicateID"></param>
        /// <returns></returns>
        [HttpGet]
        [Logger(nameof(Case_lang.CASE_ASSIGNMENT_COMMUNICATION_GET))]
        [ModelValidator(false)]
        public async Task<IHttpResult> GetCaseAssignmentCommunicate(int communicateID)
        {
            try
            {
                var con = new MSSQLCondition<CASE_ASSIGNMENT_COMMUNICATE>(x => x.ID == communicateID);

                con.IncludeBy(x => x.CASE);
                con.IncludeBy(x => x.CASE.CASE_COMPLAINED_USER);
                con.IncludeBy(x => x.CASE.CASE_CONCAT_USER);

                var data = _CaseAggregate.CaseAssignmentCommunicate_T1_T2_.Get(con);

                var model = new CaseAssignmentCommunicateViewModel(data);

                model.Case =
                   new CaseDetailViewModel(_QuestionClassificationResolver.Resolve(data.Case));

                var result = new JsonResult<CaseAssignmentCommunicateViewModel>(model, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(Case_lang.CASE_ASSIGNMENT_COMMUNICATION_GET_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Case_lang.CASE_ASSIGNMENT_COMMUNICATION_GET_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 取得單位溝通資料
        /// </summary>
        /// <param name="communicateID"></param>
        /// <returns></returns>
        [HttpGet]
        [Logger(nameof(Case_lang.CASE_INVOICE_CANCEL))]
        [ModelValidator(false)]
        public async Task<IHttpResult> CancelInovice(int InvoiceIdentityID)
        {
            try
            {
                var con = new MSSQLCondition<CASE_ASSIGNMENT_COMPLAINT_INVOICE>(x => x.ID == InvoiceIdentityID);

                con.ActionModify(x => x.TYPE = (byte)CaseAssignmentComplaintInvoiceType.Cancel);

                _CaseAggregate.CaseAssignmentComplaintInvoice_T1_T2_.Update(con);

                #region 歷程建立
                con.ClearFilters();
                con.And(x => x.ID == InvoiceIdentityID);
                con.IncludeBy(x => x.CASE);
                var casei = _CaseAggregate.CaseAssignmentComplaintInvoice_T1_T2_.Get(con);

                var caseResume = new CaseResume()
                {
                    CaseID = casei.CaseID,
                    CreateDateTime = DateTime.Now,
                    CreateUserName = ContextUtility.GetUserIdentity()?.Name,
                    Content = string.Format(SysCommon_lang.INOVICE_CANCEL, casei.InvoiceID),
                    CaseType = casei.Case.CaseType
                };
                _CaseAggregate.CaseResume_T1_T2_.Add(caseResume);
                #endregion




                var result = new JsonResult<string>() { message = "取消成功", isSuccess = true};

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(Case_lang.CASE_INVOICE_CANCEL_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Case_lang.CASE_INVOICE_CANCEL_FAIL), false)
                    .Async();
            }
        }
    }
}
