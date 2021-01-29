using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Areas.Case.Models;
using SMARTII.Areas.Common.Models.Organization;
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
using SMARTII.Assist.Logger;


namespace SMARTII.Areas.Case.Controllers
{
    public partial class CaseController
    {

        /// <summary>
        /// 取得案件查詢(客服)清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Logger(nameof(Case_lang.CASE_CALLCENTER_GETLIST))]
        [ModelValidator(false)]
        public async Task<IHttpResult> GetCaseForCustomerList(CaseCallCenterSearchViewModel model)
        {
            try
            {
                
                var condition = model.ToDomain();
                var list = await _CaseSearchService.GetCaseForCustomerLists(condition);

                //組合顯示內容
                var ui = list.Select(x => new CaseCallCenterListViewModel(x) { }).ToList().OrderBy(x=>x.CaseID);

                return await new JsonResult<IEnumerable<CaseCallCenterListViewModel>>(ui, true).Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(Case_lang.CASE_CALLCENTER_GETLIST_ERROR));


                return await new JsonResult(ex.PrefixMessage(Case_lang.CASE_CALLCENTER_GETLIST_ERROR), false).Async();
            }
        }
        /// <summary>
        /// 案件查詢-匯出Excel(客服)
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpPost]
        [Logger(nameof(Case_lang.CASE_CALLCENTER_GET_EXCEL))]
        [ModelValidator(false)]
        public async Task<HttpResponseMessage> GetExcelCaseForCustomer(CaseCallCenterSearchViewModel model)
        {
            try
            {
                var condition = model.ToDomain();
                var excelByte = await _CaseSearchService.ExcelCaseForCustomer(condition);
                var resp = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(excelByte)
                };
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_案件查詢(客服使用).xlsx";

                resp.Content.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");
                resp.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = fileName.Encoding() };
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.ms-excel");

                return resp;
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                 ex.PrefixDevMessage(Case_lang.CASE_CALLCENTER_GET_EXCEL_ERROR));

                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
        /// <summary>
        /// 取得案件查詢(總部、門市)清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Logger(nameof(Case_lang.CASE_CALLCENTER_HS_GETLIST))]
        [ModelValidator(false)]
        public async Task<IHttpResult> GetCaseForHSList(CaseHSSearchViewModel model)
        {

            try
            {
                var list = await _CaseSearchService.GetCaseForHSLists(model.ToDomain());
                //組合顯示內容
                var ui = list.Select(x => new CaseHSListViewModel(x) { }).ToList().OrderBy(x => x.CaseID);

                return await new JsonResult<IEnumerable<CaseHSListViewModel>>(ui, true).Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(Case_lang.CASE_CALLCENTER_HS_GETLIST_ERROR));


                return await new JsonResult(ex.PrefixMessage(Case_lang.CASE_CALLCENTER_HS_GETLIST_ERROR), false).Async();
            }
        }
        /// <summary>
        /// 案件查詢-匯出Excel(總部、門市)
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpPost]
        [Logger(nameof(Case_lang.CASE_CALLCENTER_HS_GET_EXCEL))]
        [ModelValidator(false)]
        public async Task<HttpResponseMessage> GetExcelCaseForHS(CaseHSSearchViewModel model)
        {
            try
            {
                var excelByte = await _CaseSearchService.ExcelCaseForHS(model.ToDomain());
                var resp = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(excelByte)
                };
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_案件查詢(總部 門市).xlsx";
                resp.Content.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");
                resp.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = fileName.Encoding() };
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.ms-excel");
                return resp;
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                 ex.PrefixDevMessage(Case_lang.CASE_CALLCENTER_HS_GET_EXCEL_ERROR));

                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
        /// <summary>
        /// 取得轉派案件查詢(客服)清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Logger(nameof(Case_lang.CASE_ASSIGNMENT_GETLIST))]
        [ModelValidator(false)]
        public async Task<IHttpResult> GetCaseAssignmentForCustomerList(CaseAssignmentCallCenteSearchViewModel model)
        {
            try
            {
                var list = await _CaseSearchService.GetCaseAssignmentForCustomerLists(model.ToDomain());

                //顯示內容
                var ui = list.Select(x => new CaseAssignmentCallCenterListViewModel(x)).ToList().OrderBy(x => x.SN).OrderBy(x => x.CaseID);
                return await new JsonResult<IEnumerable<CaseAssignmentCallCenterListViewModel>>(ui, true).Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(Case_lang.CASE_ASSIGNMENT_GETLIST_ERROR));

                return await new JsonResult(ex.PrefixMessage(Case_lang.CASE_ASSIGNMENT_GETLIST_ERROR), false).Async();
            }
        }
        /// <summary>
        /// 轉派案件查詢-匯出Excel(客服)
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpPost]
        [Logger(nameof(Case_lang.CASE_ASSIGNMENT_GET_EXCEL))]
        [ModelValidator(false)]
        public async Task<HttpResponseMessage> GetExcelCaseAssignmentForCustomer(CaseAssignmentCallCenteSearchViewModel model)
        {
            try
            {
                var excelByte = await _CaseSearchService.ExcelCaseAssignmentForCustomer(model.ToDomain());
                var resp = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(excelByte)
                };
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_轉派查詢(客服使用).xlsx";
                resp.Content.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");
                resp.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = fileName.Encoding() };
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.ms-excel");
                return resp;
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                 ex.PrefixDevMessage(Case_lang.CASE_ASSIGNMENT_GET_EXCEL_ERROR));

                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
        /// <summary>
        /// 取得轉派案件查詢(總部、門市)清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Logger(nameof(Case_lang.CASE_ASSIGNMENT_HS_GETLIST))]
        [ModelValidator(false)]
        public async Task<IHttpResult> GetCaseAssignmentForHSList(CaseAssignmentHSSearchViewModel model)
        {
            try
            {

                var list = await _CaseSearchService.GetCaseAssignmentForHSLists(model.ToDomain(), OrganizationType.HeaderQuarter);

                //顯示內容
                var ui = list.Select(x => new CaseAssignmentHSListViewModel(x)).ToList().OrderBy(x => x.SN).OrderBy(x => x.CaseID);


                return await new JsonResult<IEnumerable<CaseAssignmentHSListViewModel>>(ui, true).Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(Case_lang.CASE_ASSIGNMENT_HS_GETLIST_ERROR));


                return await new JsonResult(ex.PrefixMessage(Case_lang.CASE_ASSIGNMENT_HS_GETLIST_ERROR), false).Async();
            }
        }
        /// <summary>
        /// 轉派案件查詢-匯出Excel(總部、門市)
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpPost]
        [Logger(nameof(Case_lang.CASE_ASSIGNMENT_HS_GET_EXCEL))]
        [ModelValidator(false)]
        public async Task<HttpResponseMessage> GetExcelCaseAssignmentForHS(CaseAssignmentHSSearchViewModel model)
        {
            try
            {
                var excelByte = await _CaseSearchService.ExcelCaseAssignmentForHS(model.ToDomain(), OrganizationType.HeaderQuarter);
                var resp = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(excelByte)
                };
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_轉派查詢(總部 門市).xlsx";
                resp.Content.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");
                resp.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = fileName.Encoding() };
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.ms-excel");
                return resp;
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                 ex.PrefixDevMessage(Case_lang.CASE_ASSIGNMENT_HS_GET_EXCEL_ERROR));

                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
        /// <summary>
        /// 取得轉派案件查詢(廠商)清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Logger(nameof(Case_lang.CASE_ASSIGNMENT_VENDOR_GETLIST))]
        [ModelValidator(false)]
        public async Task<IHttpResult> GetCaseAssignmentForVendorList(CaseAssignmentHSSearchViewModel model)
        {
            try
            {
                var list = await _CaseSearchService.GetCaseAssignmentForVendorLists(model.ToDomain(), OrganizationType.Vendor);

                //顯示內容
                var ui = list.Select(x => new CaseAssignmentVendorListViewModel(x)).ToList().OrderBy(x => x.SN).OrderBy(x => x.CaseID);


                return await new JsonResult<IEnumerable<CaseAssignmentVendorListViewModel>>(ui, true).Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(Case_lang.CASE_ASSIGNMENT_VENDOR_GETLIST_ERROR));


                return await new JsonResult(ex.PrefixMessage(Case_lang.CASE_ASSIGNMENT_VENDOR_GETLIST_ERROR), false).Async();
            }
        }
        /// <summary>
        /// 轉派案件查詢-匯出Excel(廠商)
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpPost]
        [Logger(nameof(Case_lang.CASE_ASSIGNMENT_VENDOR_GET_EXCEL))]
        [ModelValidator(false)]
        public async Task<HttpResponseMessage> GetExcelCaseAssignmentForVendor(CaseAssignmentHSSearchViewModel model)
        {
            try
            {
                var excelByte = await _CaseSearchService.ExcelCaseAssignmentForVendor(model.ToDomain(), OrganizationType.Vendor);
                var resp = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(excelByte)
                };
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_轉派查詢(廠商).xlsx";

                resp.Content.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");
                resp.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = fileName.Encoding() };
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.ms-excel");
                return resp;
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                 ex.PrefixDevMessage(Case_lang.CASE_ASSIGNMENT_VENDOR_GET_EXCEL_ERROR));

                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}
