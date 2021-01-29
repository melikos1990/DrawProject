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
using SMARTII.Domain.Notification;
using SMARTII.Domain.Notification.Email;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Report;
using SMARTII.Domain.Thread;
using SMARTII.Domain.Transaction;
using SMARTII.Resource.Tag;
using SMARTII.Service.Cache;
using SMARTII.Service.Case.Flow;
using SMARTII.Service.Case.Task;
using SMARTII.Assist.Logger;
using SMARTII.Domain.Authentication.Object;

namespace SMARTII.Areas.Case.Controllers
{
    public partial class CaseController
    {

        /// <summary>
        /// 取得案件資料
        /// </summary>
        /// <param name="caseID"></param>
        /// <returns></returns>
        [HttpGet]
        [Logger(nameof(Case_lang.CASE_GET_CASE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> GetCase(string caseID)
        {
            try
            {
                var con = new MSSQLCondition<CASE>(x => x.CASE_ID == caseID);

                con.IncludeBy(x => x.CASE_COMPLAINED_USER);
                con.IncludeBy(x => x.CASE_CONCAT_USER);
                con.IncludeBy(x => x.CASE_TAG);
                con.IncludeBy(x => x.CASE_WARNING);
                con.IncludeBy(x => x.CASE_ASSIGNMENT);
                con.IncludeBy(x => x.CASE_ASSIGNMENT_COMPLAINT_INVOICE);
                con.IncludeBy(x => x.CASE_ASSIGNMENT_COMPLAINT_NOTICE);
                con.IncludeBy(x => x.CASE_FINISH_REASON_DATA);
                con.IncludeBy(x => x.CASE_REMIND);

                var data = _QuestionClassificationResolver.Resolve(_CaseAggregate.Case_T1_T2_.Get(con));

                data.CaseComplainedUsers = _StoreResolver.ResolveCollection(data.CaseComplainedUsers);

                var result = new JsonResult<CaseDetailViewModel>(new CaseDetailViewModel(data), true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(Case_lang.CASE_GET_CASE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Case_lang.CASE_GET_CASE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 新增案件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Logger(nameof(Case_lang.CASE_SAVE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> SaveCase(CaseDetailViewModel model)
        {
            try
            {
                var domain = model.ToDomain();

                var data = await _Flows[nameof(CaseFillingFlow)].Run(domain, false);

                var result = new JsonResult<CaseDetailViewModel>(
                        new CaseDetailViewModel((Domain.Case.Case)data), true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(Case_lang.CASE_SAVE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Case_lang.CASE_SAVE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 編輯案件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Logger(nameof(Case_lang.CASE_UPDATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> UpdateCase(CaseDetailViewModel model, [FromUri]int? roleID)
        {
            try
            {
                var con = new MSSQLCondition<CASE>(x => x.CASE_ID == model.CaseID);
                con.IncludeBy(x => x.CASE_CONCAT_USER);
                con.IncludeBy(x => x.CASE_COMPLAINED_USER);
                var beforeCase = _QuestionClassificationResolver.Resolve(_CaseAggregate.Case_T1_T2_.Get(con));

                if (beforeCase == null)
                    throw new Exception(Case_lang.CASE_UPDATE_FAIL);

                if (beforeCase.CaseType == CaseType.Finished)
                {
                    var roles = _OrganizationAggregate.Role_T1_T2_.Get(x => x.ID == roleID);
                    var userID = ContextUtility.GetUserIdentity()?.Instance.UserID;
                    var users = _OrganizationAggregate.User_T1_T2_.Get(x => x.USER_ID == userID);
                    var merged = _AuthHelper.GetCompleteMergedPageAuth(roles.Feature, users.Feature).Where(x => x.Feature == "C1").FirstOrDefault();
                    if (merged == null || (int)merged.AuthenticationType < (int)AuthenticationType.Admin)
                        throw new Exception(Case_lang.CASE_ALREADY_FINISH);
                }

                var domain = model.ToDomain();

                if (domain.AtLeastOneRespinsibility() == false)
                    throw new Exception(Case_lang.CASE_COMPLAINTED_USER_AT_LEAST_ONE_RESPONSEBLILITY);

                var data = _CaseService.UpdateComplete(domain, false);

                var user = ContextUtility.GetUserIdentity()?.Instance;


                _CaseFacade.CreateResume(data.CaseID, beforeCase, null, Case_lang.CASE_UPDATE, user);

                // 新增歷程後記錄&通知
                using (var scope = TrancactionUtility.NoTransactionScope())
                {
                    Task.Run(() =>
                    {
                        _CaseFacade.CreateHistory(data.CaseID, data, Case_lang.CASE_UPDATE, user);
                        _CaseFacade.CreatePersonalNotification(data.CaseID, PersonalNotificationType.CaseModify, user);
                    });
                };


                var result = new JsonResult<CaseDetailViewModel>(
                       new CaseDetailViewModel(data), true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(Case_lang.CASE_UPDATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Case_lang.CASE_UPDATE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 案件結案
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Logger(nameof(Case_lang.CASE_FINISH))]
        [ModelValidator(false)]
        public async Task<IHttpResult> FinishCase(CaseDetailViewModel model)
        {
            try
            {
                var domain = model.ToDomain();

                var data = await _Flows[nameof(CaseFinishedFlow)].Run(domain);

                var result = new JsonResult<CaseDetailViewModel>(
                    new CaseDetailViewModel((Domain.Case.Case)data), true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(Case_lang.CASE_FINISH_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Case_lang.CASE_FINISH_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 解鎖案件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Logger(nameof(Case_lang.CASE_UNLOCK))]
        [ModelValidator(false)]
        public async Task<IHttpResult> UnlockCase(CaseDetailViewModel model)
        {
            try
            {
                var domain = model.ToDomain();

                var data = await _Flows[nameof(CaseUnlockFlow)].Run(domain);

                var result = new JsonResult<CaseDetailViewModel>(
                    new CaseDetailViewModel((Domain.Case.Case)data), true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(Case_lang.CASE_UNLOCK_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Case_lang.CASE_UNLOCK_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 取得案件編號清單
        /// </summary>
        /// <param name="sourceID"></param>
        /// <returns></returns>
        [HttpGet]
        [Logger(nameof(Case_lang.CASE_GET_CASE_IDs))]
        [ModelValidator(false)]
        public async Task<IHttpResult> GetCaseIDs(string sourceID)
        {
            try
            {
                var con = new MSSQLCondition<CASE>(x => x.SOURCE_ID == sourceID);

                var ids = _CaseAggregate.Case_T1_T2_.GetListOfSpecific(con, x => x.CASE_ID);

                var result = new JsonResult<List<string>>(ids, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(Case_lang.CASE_GET_CASE_IDs_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Case_lang.CASE_GET_CASE_IDs_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 驗證勾稽案件存在與否
        /// </summary>
        /// <param name="caseID"></param>
        /// <returns></returns>
        [HttpGet]
        [Logger(nameof(Case_lang.CASE_CHECKCASE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> CheckCase(string caseID, int buID, string sourceID)
        {
            try
            {
                var value = caseID.Trim();

                var @case = _CaseAggregate.Case_T1_T2_.Get(x => x.CASE_ID == value);

                if (@case == null)
                    return await new JsonResult(string.Format(Case_lang.CASE_SOURCE_RELATE_NOEXIST, caseID), false).Async();

                if (@case.NodeID == buID ? false : true)
                    return await new JsonResult(Case_lang.CASE_SOURCE_RELATE_NOT_SAME_BU, false).Async();

                var con = new MSSQLCondition<CASE_SOURCE>(x => x.SOURCE_ID == sourceID);
                con.IncludeBy(x => x.CASE);
                con.And(x => x.CASE.Select(y => y.CASE_ID).Contains(caseID));

                if (_CaseAggregate.CaseSource_T1_T2_.HasAny(con))
                    return await new JsonResult(Case_lang.CASE_SOURCE_RELATE_SOURCE_REPEAT, false).Async();

                var result = new JsonResult<string>(caseID, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                   ex.PrefixDevMessage(Case_lang.CASE_CHECKCASE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Case_lang.CASE_CHECKCASE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 案件結案回信
        /// </summary>
        /// <param name="caseID"></param>
        /// <param name="emailPayload"></param>
        /// <returns></returns>
        [HttpPost]
        [Logger(nameof(Case_lang.CASE_FINISHEDMAILREPLY))]
        [ModelValidator(false)]
        public async Task<IHttpResult> CaseFinishedMailReply([FromUri]string caseID, EmailPayload emailPayload)
        {
            try
            {
                _CaseService.CaseFinishedMailReply(caseID, emailPayload);

                var result = new JsonResult<string>(Case_lang.CASE_FINISHEDMAILREPLY_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(Case_lang.CASE_FINISHEDMAILREPLY_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Case_lang.CASE_FINISHEDMAILREPLY_FAIL), false)
                    .Async();
            }
        }

    }
}
