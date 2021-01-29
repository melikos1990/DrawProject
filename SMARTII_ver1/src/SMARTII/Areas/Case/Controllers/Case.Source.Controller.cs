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
using Newtonsoft.Json;
using System.Collections;

namespace SMARTII.Areas.Case.Controllers
{
    public partial class CaseController
    {
        /// <summary>
        /// 新增案件來源
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Logger(nameof(Case_lang.CASE_SOURCE_SAVE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> SaveCaseSource(CaseSourceDetailViewModel model)
        {
            try
            {
                // 特為新增來源時特殊行為 , 因此先將案件清空
                model.Cases = null;

                var domain = model.ToDomain();

                var data = await _Flows[nameof(CaseSourceIncomingFlow)].Run(domain);

                var result = new JsonResult<CaseSourceDetailViewModel>(
                        new CaseSourceDetailViewModel((CaseSource)data), true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(Case_lang.CASE_SOURCE_SAVE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Case_lang.CASE_SOURCE_SAVE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 編輯案件來源
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Logger(nameof(Case_lang.CASE_SOURCE_UPDATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> UpdateCaseSource(CaseSourceDetailViewModel model)
        {
            try
            {
                // 特為新增來源時特殊行為 , 因此先將案件清空
                model.Cases = null;

                var domain = model.ToDomain();

                var data = _CaseSourceService.UpdateComplete(domain);

                var result = new JsonResult<CaseSourceDetailViewModel>(
                       new CaseSourceDetailViewModel(data), true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(Case_lang.CASE_SOURCE_UPDTAE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Case_lang.CASE_SOURCE_UPDTAE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 新增案件來源 (包含案件)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Logger(nameof(Case_lang.CASE_SOURCECOMPLETE_SAVE))]
        [ModelValidator(true)]
        public async Task<IHttpResult> SaveCaseSourceComplete(CaseSourceDetailViewModel model, [FromUri]bool? isFastFinished = null)
        {
            try
            {
                var domain = model.ToDomain();

                var data = await _Flows[nameof(CaseSourceCompleteIncomingFlow)].Run(domain, isFastFinished);

                var result = new JsonResult<CaseSourceDetailViewModel>(
                        new CaseSourceDetailViewModel((CaseSource)data), true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                   ex.PrefixDevMessage(Case_lang.CASE_SOURCECOMPLETE_SAVE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Case_lang.CASE_SOURCECOMPLETE_SAVE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 取得案件來源資料
        /// </summary>
        /// <param name="caseID"></param>
        /// <returns></returns>
        [HttpGet]
        [Logger(nameof(Case_lang.GET_SOURCE_NATIVE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> GetCaseSource(string caseID)
        {
            try
            {
                var con = new MSSQLCondition<CASE_SOURCE>(
                              x => x.CASE.Any(g => g.CASE_ID == caseID));

                con.IncludeBy(x => x.CASE_SOURCE_USER);

                // 這邊刻意不JOIN 案件 , 目的為了讓兩區塊彼此區隔
                // con.IncludeBy(x => x.CASE);

                var data = _CaseSourceResolver.Resolve
                           (
                            _CaseAggregate.CaseSource_T1_T2_.Get(con)
                           );

                var result = new JsonResult<CaseSourceDetailViewModel>(new CaseSourceDetailViewModel(data), true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(Case_lang.GET_SOURCE_NATIVE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Case_lang.GET_SOURCE_NATIVE_FAIL), false)
                    .Async();
            }
        }

        [HttpGet]
        [Logger(nameof(Case_lang.GET_SOURCE_NATIVE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> GetCaseSourceCheck(string caseID, string sourceTab)
        {
            try
            {
                var caseTabList = JsonConvert.DeserializeObject<List<CaseTabViewModel>>(sourceTab);        

                var con = new MSSQLCondition<CASE_SOURCE>(
                              x => x.CASE.Any(g => g.CASE_ID == caseID));

                con.IncludeBy(x => x.CASE_SOURCE_USER);

                // 這邊刻意不JOIN 案件 , 目的為了讓兩區塊彼此區隔
                // con.IncludeBy(x => x.CASE);

                var data = _CaseSourceResolver.Resolve
                           (
                            _CaseAggregate.CaseSource_T1_T2_.Get(con)
                           );

                if (caseTabList.Any(x => x.id.Contains(data.SourceID)))
                    return await new JsonResult(string.Format(Case_lang.GET_SOURCE_REPEAT, data.SourceID), false).Async();

                var result = new JsonResult<CaseSourceDetailViewModel>(new CaseSourceDetailViewModel(data), true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(Case_lang.GET_SOURCE_NATIVE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Case_lang.GET_SOURCE_NATIVE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 取得案件來源資料
        /// </summary>
        /// <param name="caseID"></param>
        /// <returns></returns>
        [HttpGet]
        [Logger(nameof(Case_lang.GET_SOURCE_NATIVE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> GetNativeCaseSource(string caseSourceID)
        {
            try
            {
                var con = new MSSQLCondition<CASE_SOURCE>(x => x.SOURCE_ID == caseSourceID);

                con.IncludeBy(x => x.CASE_SOURCE_USER);
                con.IncludeBy(x => x.CASE);

                var data = _CaseSourceResolver.Resolve
                           (
                            _CaseAggregate.CaseSource_T1_T2_.Get(con)
                           );

                var result = new JsonResult<CaseSourceDetailViewModel>(new CaseSourceDetailViewModel(data), true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(Case_lang.GET_SOURCE_NATIVE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Case_lang.GET_SOURCE_NATIVE_FAIL), false)
                    .Async();
            }
        }
    }
}
