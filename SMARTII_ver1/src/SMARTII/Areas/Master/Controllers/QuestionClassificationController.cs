using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net.Http;
using System.Net;
using MoreLinq.Extensions;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Areas.Master.Models.QuestionClassification;
using SMARTII.Assist.Authentication;
using SMARTII.Assist.Logger;
using SMARTII.Assist.Web;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;
using SMARTII.Service.Organization.Resolver;
using SMARTII.Domain.Report;
using System.Net.Http.Headers;

namespace SMARTII.Areas.Master.Controllers
{
    [Authentication]
    public class QuestionClassificationController : BaseApiController
    {
        private readonly ICommonAggregate _CommonAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly IMasterAggregate _IMasterAggregate;
        private readonly IQuestionClassificationFacade _QuestionClassificationFacade;
        private readonly IQuestionClassificationReportProvider _QuestionClassificationReportProvider;
        private readonly OrganizationNodeResolver _OrganizationResolver;

        public QuestionClassificationController(
            ICommonAggregate CommonAggregate,
            IMasterAggregate IMasterAggregate,
            IOrganizationAggregate OrganizationAggregate,
            IQuestionClassificationFacade QuestionClassificationFacade,
            IQuestionClassificationReportProvider QuestionClassificationReportProvider,
            OrganizationNodeResolver OrganizationResolver
        )
        {
            _CommonAggregate = CommonAggregate;
            _IMasterAggregate = IMasterAggregate;
            _OrganizationAggregate = OrganizationAggregate;
            _QuestionClassificationFacade = QuestionClassificationFacade;
            _QuestionClassificationReportProvider = QuestionClassificationReportProvider;
            _OrganizationResolver = OrganizationResolver;
        }

        /// <summary>
        /// 單一刪除
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Delete)]
        [Logger(nameof(QuestionClassification_lang.QUESTIONCLASSIFICATION_DELETE))]
        [ModelValidator]
        public async Task<IHttpResult> Disable(int id)
        {
            try
            {
                await _QuestionClassificationFacade.DeleteAsync(id);

                return await new JsonResult(true).Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(QuestionClassification_lang.QUESTIONCLASSIFICATION_DELETE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(QuestionClassification_lang.QUESTIONCLASSIFICATION_DELETE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 批次刪除
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Delete)]
        [Logger(nameof(QuestionClassification_lang.QUESTIONCLASSIFICATION_DELETE_RANGE))]
        public async Task<IHttpResult> DisableRanage(List<QuestionClassificationDetailViewModel> model)
        {
            try
            {
                var ids = model.Select(x => x.ID).ToArray();

                await _QuestionClassificationFacade.DeleteRangeAsync(ids);

                return await new JsonResult(true).Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(QuestionClassification_lang.QUESTIONCLASSIFICATION_DELETE_RANGE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(QuestionClassification_lang.QUESTIONCLASSIFICATION_DELETE_RANGE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 單一新增
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Add)]
        [Logger(nameof(QuestionClassification_lang.QUESTIONCLASSIFICATION_CREATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Create(QuestionClassificationDetailViewModel viewModel)
        {
            try
            {
                var user = this.UserIdentity.Instance;
                var now = DateTime.Now;
                var domain = new QuestionClassification()
                {
                    Name = viewModel.Name,
                    NodeID = viewModel.BuID,
                    ParentID = viewModel.ParentNodeID,
                    Level = viewModel.Level,
                    IsEnabled = viewModel.IsEnable,
                    OrganizationType = OrganizationType.HeaderQuarter,
                    CreateUserName = user.Name,
                    CreateDateTime = now
                };

                await _QuestionClassificationFacade.CreateAsync(domain);

                return await new JsonResult(true).Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(QuestionClassification_lang.QUESTIONCLASSIFICATION_CREATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(QuestionClassification_lang.QUESTIONCLASSIFICATION_CREATE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 單一更新
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Update)]
        [Logger(nameof(QuestionClassification_lang.QUESTIONCLASSIFICATION_UPDATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Update(QuestionClassificationDetailViewModel viewModel)
        {
            try
            {
                var user = this.UserIdentity.Instance;
                var now = DateTime.Now;

                var question = new QuestionClassification()
                {
                    ID = viewModel.ID,
                    NodeID = viewModel.BuID,
                    Name = viewModel.Name,
                    Level = viewModel.Level,
                    IsEnabled = viewModel.IsEnable,
                    ParentID = viewModel.ParentNodeID,
                    UpdateDateTime = now,
                    UpdateUserName = user.Name
                };

                await _QuestionClassificationFacade.UpdateAsync(question);

                return await new JsonResult(true).Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(QuestionClassification_lang.QUESTIONCLASSIFICATION_UPDATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(QuestionClassification_lang.QUESTIONCLASSIFICATION_UPDATE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 單一取得
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(QuestionClassification_lang.QUESTIONCLASSIFICATION_GET))]
        [ModelValidator]
        public async Task<IHttpResult> Get(int nodeID, int id, int organizationType)
        {
            try
            {
                var con = new MSSQLCondition<VW_QUESTION_CLASSIFICATION_NESTED>(x => x.NODE_ID == nodeID && x.ID == id && x.ORGANIZATION_TYPE == (byte)organizationType);

                var question = _IMasterAggregate.VWQuestionClassification_QuestionClassification_.Get(con);

                if (question == null) throw new Exception(Common_lang.NOT_FOUND_DATA);

                var parentPath = Enumerable.Zip(question.ParentPathByArray.SkipLast(1), question.ParentNamePathByArray.SkipLast(1), (x, y) => new SelectItem
                {
                    id = x,
                    text = y
                }).ToList();

                var detail = new QuestionClassificationDetailViewModel(question, parentPath);

                return await new JsonResult<QuestionClassificationDetailViewModel>(detail, true).Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(QuestionClassification_lang.QUESTIONCLASSIFICATION_GET_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(QuestionClassification_lang.QUESTIONCLASSIFICATION_GET_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 取得清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(QuestionClassification_lang.QUESTIONCLASSIFICATION_GETLIST))]
        [ModelValidator(false)]
        public async Task<PagingResponse> GetList(PagingRequest<QuestionClassificationSearchViewModel> model)
        {
            try
            {
                var criteria = model.criteria;
                var user = this.UserIdentity.Instance;
                var con = new MSSQLCondition<VW_QUESTION_CLASSIFICATION_NESTED>(
                    criteria,
                    model.pageIndex,
                    model.pageSize
                );
                con.And(x => x.ORGANIZATION_TYPE == (byte)OrganizationType.HeaderQuarter);

                con.OrderBy(model.sort, model.orderType);

                var paging = _IMasterAggregate.VWQuestionClassification_QuestionClassification_.GetPaging(con);

                var ui = _OrganizationResolver.ResolveCollection(paging).Select(x => new QuestionClassificationListViewModel(x)).ToList();

                return await new PagingResponse<List<QuestionClassificationListViewModel>>(ui)
                {
                    isSuccess = true,
                    totalCount = paging.Count
                }.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(QuestionClassification_lang.QUESTIONCLASSIFICATION_GETLIST_FAIL));

                return await new PagingResponse()
                {
                    isSuccess = false,
                    message = ex.PrefixMessage(QuestionClassification_lang.QUESTIONCLASSIFICATION_GETLIST_FAIL)
                }.Async();
            }
        }

        /// <summary>
        /// 取得問題分類 階層總數
        /// </summary>
        /// <param name="BuID"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(QuestionClassification_lang.QUESTIONCLASSIFICATION_GET_MAXLEVEL))]
        public async Task<IHttpResult> GetQuestionMaxLevel(int BuID)
        {
            try
            {
                var con = new MSSQLCondition<QUESTION_CLASSIFICATION>(x =>
                    x.NODE_ID == BuID &&
                    x.ORGANIZATION_TYPE == (byte)OrganizationType.HeaderQuarter);

                con.OrderBy(x => x.LEVEL, OrderType.Desc);

                var maxLevel = _IMasterAggregate.QuestionClassification_T1_T2_.GetOfSpecific(con, x => x.LEVEL);

                return await new JsonResult<int>(maxLevel, true).Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(QuestionClassification_lang.QUESTIONCLASSIFICATION_GET_MAXLEVEL_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(QuestionClassification_lang.QUESTIONCLASSIFICATION_GET_MAXLEVEL_FAIL), false)
                    .Async();
            }
        }

        private List<QUESTION_CLASSIFICATION> RecursivelyChildren(QUESTION_CLASSIFICATION question)
        {
            var result = new List<QUESTION_CLASSIFICATION>();

            if (question.QUESTION_CLASSIFICATION1 != null && question.QUESTION_CLASSIFICATION1.Count > 0)
            {
                question.QUESTION_CLASSIFICATION1.ToList().ForEach(child =>
                {
                    result.AddRange(this.RecursivelyChildren(child));
                });
            }

            result.Insert(0, question);

            return result;
        }

        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Update)]
        [Logger(nameof(QuestionClassification_lang.QUESTIONCLASSIFICATION_EDITORDER))]
        public async Task<IHttpResult> EditOrder(List<SorterListResponse<QuestionClassificationOrderViewModel>> model)
        {
            try
            {
                _IMasterAggregate.QuestionClassification_T1_T2_.Operator(context =>
                {
                    var db = (SMARTIIEntities)context;
                    db.Configuration.LazyLoadingEnabled = false;

                    model.ForEach(x =>
                    {
                        var id = int.Parse(x.id);

                        var entity = db.QUESTION_CLASSIFICATION.First(g => g.ID == id);

                        entity.ORDER = x.order;
                    });

                    db.SaveChanges();
                });

                return await new JsonResult<string>(QuestionClassification_lang.QUESTIONCLASSIFICATION_EDITORDER_SUCCESS, true).Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(QuestionClassification_lang.QUESTIONCLASSIFICATION_EDITORDER_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(QuestionClassification_lang.QUESTIONCLASSIFICATION_EDITORDER_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 案件查詢-匯出Excel(客服)
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpPost]
        [ModelValidator(false)]
        public async Task<HttpResponseMessage> GetExcelForQuestionClassification(QuestionClassificationSearchViewModel model)
        {
            try
            {
                var con = new MSSQLCondition<VW_QUESTION_CLASSIFICATION_NESTED>(
                    model
                );
                con.And(x => x.ORGANIZATION_TYPE == (byte)OrganizationType.HeaderQuarter);

                var paging = _IMasterAggregate.VWQuestionClassification_QuestionClassification_.GetList(con);

                var list = _OrganizationResolver.ResolveCollection(paging).Select(x => new QuestionClassificationForExcel(x)).ToList();

                var result = _QuestionClassificationReportProvider.CreateQuestionClassificationExcel(list);


                var resp = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(result)
                };
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_問題分類.xlsx";

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
    }
}
