using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Areas.Common.Models.Master;
using SMARTII.Areas.Master.Models.CaseAssignGroup;
using SMARTII.Areas.Master.Models.CaseTemplate;
using SMARTII.Areas.Master.Models.QuestionClassificationAnswer;
using SMARTII.Assist.Authentication;
using SMARTII.Assist.Web;
using SMARTII.COMMON_BU.Models.Item;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Case;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using SMARTII.Domain.System;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;
using SMARTII.Service.Cache;
using SMARTII.Service.Master.Resolver;
using SMARTII.Service.Organization.Resolver;

namespace SMARTII.Areas.Select.Controllers
{
    [Authentication]
    public class MasterController : BaseApiController
    {
        private readonly ICaseAggregate _CaseAggregate;
        private readonly ICommonAggregate _CommonAggregate;
        private readonly ISystemAggregate _SystemAggregate;
        private readonly IMasterAggregate _MasterAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly ICaseTemplateService _CaseTemplateService;
        private readonly UserResolver _UserResolver;
        private readonly QuestionClassificationResolver _QuestionClassificationResolver;

        public MasterController(ICaseAggregate CaseAggregate,
                                ISystemAggregate SystemAggregate,
                                ICommonAggregate CommonAggregate,
                                IMasterAggregate MasterAggregate,
                                IOrganizationAggregate OrganizationAggregate,
                                ICaseTemplateService CaseTemplateService,
                                UserResolver UserResolver,
                                QuestionClassificationResolver QuestionClassificationResolver)
        {
            _SystemAggregate = SystemAggregate;
            _CommonAggregate = CommonAggregate;
            _MasterAggregate = MasterAggregate;
            _OrganizationAggregate = OrganizationAggregate;
            _CaseAggregate = CaseAggregate;
            _CaseTemplateService = CaseTemplateService;
            _UserResolver = UserResolver;
            _QuestionClassificationResolver = QuestionClassificationResolver;
        }

        /// <summary>
        /// 取得範本類別清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetClassification")]
        public async Task<IHttpActionResult> GetClassificationAsync()
        {
            try
            {
                var con = new MSSQLCondition<SYSTEM_PARAMETER>(x => x.ID == EssentialCache.LayoutValue.CaseTemplate);
                con.OrderBy(x => x.CREATE_DATETIME, OrderType.Desc);

                var result = _SystemAggregate.SystemParameter_T1_T2_.GetPaging(con).ToList();

                var select2 = new Select2Response<SystemParameter>()
                {
                    items = Select2Response<SystemParameter>.ToSelectItems(result, x => x.Key, x => x.Text, x => x)
                };

                return Ok(select2);
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return await StatusCode(HttpStatusCode.InternalServerError).Async();
            }
        }

        /// <summary>
        /// 取得範本主檔(結案)
        /// </summary>
        /// <param name="buID"></param>
        /// <returns></returns>
        [HttpGet]
        [ModelValidator(false)]
        public async Task<IHttpResult> GetCaseTemplate(int? buID, string key)
        {
            try
            {
                var list = _MasterAggregate.CaseTemplate_T1_T2_
                                           .GetList(x => x.NODE_ID == buID &&
                                                         x.CLASSIFIC_KEY == key);

                var result = list?.Select(x => new CaseTemplateListViewModel(x))
                                  .ToList();

                return await new JsonResult<List<CaseTemplateListViewModel>>(result, true).Async();

            }
            catch (Exception ex)
            {

                _CommonAggregate.Logger.Error(
                 ex.PrefixDevMessage(""));

                return await new JsonResult(
                    ex.PrefixMessage(""), false)
                    .Async();
            }
        }

        /// <summary>
        /// 取得問題分類清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetQuestionClassificationList")]
        public async Task<IHttpActionResult> GetQuestionClassificationListAsync(Select2Request<QuestionClassificationSelectViewModel> model)
        {
            try
            {
                var criteria = model.criteria;

                var con = new MSSQLCondition<VW_QUESTION_CLASSIFICATION_NESTED>(
                    criteria,
                    model.pageIndex,
                    model.size
                );

                con.And(x => x.ORGANIZATION_TYPE == (byte)OrganizationType.HeaderQuarter && x.PARENT_ID == model.parentID);

                if (!string.IsNullOrEmpty(model.keyword))
                    con.And(x => x.NAME.Contains(model.keyword));//20201022加入keyword識別

                con.OrderBy(x => x.ORDER, OrderType.Asc);

                //20201022用途不明，先行註解
                //var collection = _MasterAggregate.VWQuestionClassification_QuestionClassification_
                //                                 .GetPaging(con);

                var result = _MasterAggregate.VWQuestionClassification_QuestionClassification_
                                             .GetPaging(con)
                                             .ToList();

                var select2 = new Select2Response<QuestionClassification>()
                {
                    items = Select2Response<QuestionClassification>.ToSelectItems(result, x => x.ID.ToString(), x => x.Name, x => x)
                };

                return Ok(select2);
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return Json(new Select2Response()
                {
                    items = new List<SelectItem>()
                });
            }
        }

        /// <summary>
        /// 依BU取得處置原因
        /// 並顯示勾選清單
        /// </summary>
        /// <param name="BuID"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetFinishReasonClassificationChecked")]
        public async Task<IHttpActionResult> GetFinishReasonClassificationCheckedAsync(int? BuID)
        {
            try
            {
                var con = new MSSQLCondition<CASE_FINISH_REASON_CLASSIFICATION>(x => x.IS_ENABLED);

                if (BuID != null) con.And(x => x.NODE_ID == BuID && x.ORGANIZATION_TYPE == (int)OrganizationType.HeaderQuarter);

                con.IncludeBy(x => x.CASE_FINISH_REASON_DATA);
                con.OrderBy(x => x.ORDER, OrderType.Asc);

                var finishResaons = this._MasterAggregate.CaseFinishReasonClassification_T1_T2_.GetList(con).Select(x => new CaseFinishReasonListViewModel(x));

                return Ok(finishResaons);
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return Json(new CaseFinishReasonListViewModel());
            }
        }

        /// <summary>
        /// 依BU取得處置原因
        /// </summary>
        /// <param name="BuID"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetFinishReasonClassification")]
        public async Task<IHttpActionResult> GetFinishReasonClassificationAsync(int? nodeID)
        {
            try
            {
                var con = new MSSQLCondition<CASE_FINISH_REASON_CLASSIFICATION>();

                if (nodeID.HasValue)
                    con.And(x => x.NODE_ID == nodeID && x.ORGANIZATION_TYPE == (int)OrganizationType.HeaderQuarter);

                con.OrderBy(x => x.ORDER, OrderType.Asc);

                var result =
                    _MasterAggregate.CaseFinishReasonClassification_T1_T2_
                    .GetList(con)
                    .Select(x => new CaseFinishReasonListViewModel(x))
                    .ToList();

                var select2 = new Select2Response()
                {
                    items = Select2Response.ToSelectItems<CaseFinishReasonListViewModel>(result,
                    x => x.ClassificationID.ToString(),
                    x => x.ClassificationName.ToString())
                };

                return Ok(select2);
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return Json(new CaseFinishReasonListViewModel());
            }
        }

        /// <summary>
        /// 依據BU取得派工群組清單
        /// </summary>
        /// <param name="nodeID"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetCaseAssignGroup")]
        public async Task<IHttpActionResult> GetCaseAssignGroupAsync(int nodeID, CaseAssignGroupType? caseAssignGroupType)
        {
            try
            {
                var con = new MSSQLCondition<CASE_ASSIGN_GROUP>(x => x.NODE_ID == nodeID);
                if (caseAssignGroupType != null)
                    con.And(x => x.TYPE == (int)caseAssignGroupType);
                con.OrderBy(x => x.ID, OrderType.Asc);

                var result = _CaseAggregate.CaseAssignmentGroup_T1_T2_.GetPaging(con).ToList();

                var select2 = new Select2Response()
                {
                    items = Select2Response.ToSelectItems(result, x => x.ID.ToString(), x => x.Name)
                };

                return Ok(select2);
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return await StatusCode(HttpStatusCode.InternalServerError).Async();
            }
        }

        /// <summary>
        /// 取得派工群組的人員清單
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetCaseAssignGroupUser")]
        public async Task<IHttpActionResult> GetCaseAssignGroupUserAsync(int? groupID)
        {
            try
            {
                var con = new MSSQLCondition<CASE_ASSIGN_GROUP_USER>(x => x.GROUP_ID == groupID);
                con.OrderBy(x => x.NOTIFICATION_REMARK, OrderType.Asc);


                var result = _UserResolver.ResolveCollection(_CaseAggregate.CaseAssignmentGroupUser_T1_T2_.GetPaging(con))
                                           .Select(x => new CaseAssignGroupUserListViewModel(x));

                return Ok(result);
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return await StatusCode(HttpStatusCode.InternalServerError).Async();
            }
        }

        /// <summary>
        /// 依BU取得案件標籤
        /// </summary>
        /// <param name="BuID"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetHashTags")]
        public async Task<IHttpActionResult> GetHashTagsAsync(bool? isEnabled, int? BuID)
        {
            try
            {
                if (BuID == null)
                {
                    return Ok(Select2Response.ToSelectItems<CaseTag>(new List<CaseTag>(), x => x.ID.ToString(), x => x.Name));
                }

                var con = new MSSQLCondition<CASE_TAG>();

                if (isEnabled != null)
                {
                    con.And(x => x.IS_ENABLED == isEnabled);
                }

                if (BuID != null) con.And(x => x.NODE_ID == BuID && x.ORGANIZATION_TYPE == (int)OrganizationType.HeaderQuarter);

                var hashTags = this._MasterAggregate.CaseTag_T1_T2_.GetList(con).ToList();

                return Ok(Select2Response.ToSelectItems<CaseTag>(hashTags, x => x.ID.ToString(), x => x.Name));
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return Json(new List<SelectItem>());
            }
        }

        /// <summary>
        /// 依BU取得案件時效
        /// </summary>
        /// <param name="BuID"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetCaseWarning")]
        public async Task<IHttpActionResult> GetCaseWarningAsync(int? BuID, bool? Enabled)
        {
            try
            {
                var con = new MSSQLCondition<CASE_WARNING>();

                if (Enabled != null)
                {
                    con.And(x => x.IS_ENABLED);
                }

                if (BuID != null) con.And(x => x.NODE_ID == BuID && x.ORGANIZATION_TYPE == (int)OrganizationType.HeaderQuarter);

                var caseWarning = this._MasterAggregate.CaseWarning_T1_T2_.GetList(con).OrderBy(x => x.Order).ToList();

                return Ok(Select2Response.ToSelectItems<CaseWarning>(caseWarning, x => x.ID.ToString(), x => x.Name));
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return Json(new List<SelectItem>());
            }
        }

        /// <summary>
        /// 取得商品資訊清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetItemList")]
        public async Task<IHttpActionResult> GetItemListAsync(Select2Request<ItemSearchViewModel> model)
        {
            try
            {
                var searchTerm = model.criteria;

                var con = new MSSQLCondition<ITEM>(
                    searchTerm,
                    model.pageIndex,
                    model.size
                );

                if (string.IsNullOrEmpty(model.keyword) == false)
                {
                    con.And(x => x.NAME.Contains(model.keyword));
                }

                con.OrderBy(x => x.NAME, OrderType.Asc);

                var result = _MasterAggregate.Item_T1_T2_
                                             .GetPaging(con)
                                             .ToList();

                var select2 = new Select2Response()
                {
                    items = Select2Response.ToSelectItems(result, x => x.ID.ToString(), x => x.Name)
                };

                return Ok(select2);
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return Json(new Select2Response()
                {
                    items = new List<SelectItem>()
                });
            }
        }

        /// <summary>
        /// 依照BU 問題分類取得常用語清單
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetAnswerList")]
        public async Task<PagingResponse> GetAnswerListAsync([Required]int? classificationID)
        {
            try
            {
                var con = new MSSQLCondition<VW_QUESTION_CLASSIFICATION_ANSWER_NESTED>(x => x.CLASSIFICATION_ID == classificationID);

                con.OrderBy(x => x.INDEX, OrderType.Asc);

                var list = _MasterAggregate.VWQuestionClassificationAnswer_QuestionClassificationAnswer_.GetPaging(con);

                var ui = list.Select(x => new QuestionClassificationAnswerListViewModel(x));

                return await new PagingResponse<IEnumerable<QuestionClassificationAnswerListViewModel>>(ui)
                {
                    isSuccess = true,
                    totalCount = list.TotalCount
                }.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                ex.PrefixDevMessage(QuestionClassificationAnswer_lang.QUESTION_CLASSIFICATION_ANSWER_GETLIST_FAIL));

                return await new PagingResponse()
                {
                    isSuccess = false,
                    message = ex.PrefixMessage(QuestionClassificationAnswer_lang.QUESTION_CLASSIFICATION_ANSWER_GETLIST_FAIL)
                }.Async();
            }
        }

        /// <summary>
        /// 根據查詢條件 , 找到範本後進行解析
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("ParseCaseTemplate")]
        public async Task<IHttpResult> ParseCaseTemplateAsync(CaseTemplateParseViewModel model)
        {
            try
            {

                var con = new MSSQLCondition<CASE_TEMPLATE>(model);

                var template = _MasterAggregate.CaseTemplate_T1_T2_.Get(con);

                if (template == null)
                    throw new NullReferenceException(Common_lang.NOT_FOUND_DATA);

                var @case = this.GetCase(model.CaseID);

                // 後須 篩選特定條件
                if (string.IsNullOrEmpty(model.InvoicID) == false)
                {
                    var invoice = @case.ComplaintInvoice.Where(x => x.InvoiceID.Trim() == model.InvoicID.Trim()).ToList();
                    @case.ComplaintInvoice = invoice;
                }

                // 後須 篩選特定條件
                if (model.AssignmentID != null)
                {
                    var assigment = @case.CaseAssignments.Where(x => x.AssignmentID == model.AssignmentID).ToList();
                    @case.CaseAssignments = assigment;
                }

                var contentTemplate =
                    await _CaseTemplateService.ParseTemplateUseExist(template.Content, () => @case);

                var emailTemplate = !string.IsNullOrEmpty(template.EmailTitle) ?
                     await _CaseTemplateService.ParseTemplateUseExist(template.EmailTitle, () => @case) : "";


                emailTemplate = this.GetDefaultEmail(emailTemplate, @case.NodeID);

                var result = new CaseTemplateParseResultViewModel()
                {
                    Content = contentTemplate,
                    EmailTitle = emailTemplate
                };


                return await new JsonResult<CaseTemplateParseResultViewModel>(result, true).Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex.PrefixDevMessage(CaseTemplate_lang.CASE_TEMPLATE_PARSING_FAIL));

                return await new JsonResult(ex.Message, false).Async();
            }
        }

        /// <summary>
        /// 取得預設範本(不須解析)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetCaseTemplate")]
        public async Task<IHttpResult> GetCaseTemplateAsync(CaseTemplateParseViewModel model)
        {
            try
            {
                var con = new MSSQLCondition<CASE_TEMPLATE>(model);

                var template = _MasterAggregate.CaseTemplate_T1_T2_.Get(con);

                if (template == null)
                    throw new NullReferenceException(Common_lang.NOT_FOUND_DATA);

                var result = new CaseTemplateParseResultViewModel()
                {
                    Content = template.Content,
                    EmailTitle = template.EmailTitle
                };

                return await new JsonResult<CaseTemplateParseResultViewModel>(result, true).Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex.PrefixDevMessage(CaseTemplate_lang.CASE_TEMPLATE_PARSING_FAIL));

                return await new JsonResult(ex.Message, false).Async();
            }
        }

        /// <summary>
        /// 取得解析範本
        /// </summary>
        /// <param name="template"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("ParseCaseTemplateUseExist")]
        public async Task<IHttpResult> ParseCaseTemplateUseExistAsync([FromBody]string template, [FromUri]string key)
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                    return await new JsonResult<string>(template, true).Async();

                var @case = this.GetCase(key);

                var result = await _CaseTemplateService.ParseTemplateUseExist<Domain.Case.Case>(template, () => @case);

                result = this.GetDefaultEmail(result, @case.NodeID);

                return await new JsonResult<string>(result, true).Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex.PrefixDevMessage(CaseTemplate_lang.CASE_TEMPLATE_PARSING_FAIL));

                return await new JsonResult(ex.Message, false).Async();
            }
        }

        #region MISC

        private Domain.Case.Case GetCase(string caseID, Action<MSSQLCondition<CASE>> action = null)
        {
            var con = new MSSQLCondition<CASE>();

            con.And(x => x.CASE_ID == caseID);
            con.IncludeBy(x => x.CASE_ASSIGNMENT_COMPLAINT_INVOICE);
            con.IncludeBy(x => x.CASE_ASSIGNMENT_COMPLAINT_NOTICE);
            con.IncludeBy(x => x.CASE_ASSIGNMENT);
            con.IncludeBy(x => x.CASE_ASSIGNMENT.Select(g => g.CASE_ASSIGNMENT_USER));
            con.IncludeBy(x => x.CASE_CONCAT_USER);
            con.IncludeBy(x => x.CASE_COMPLAINED_USER);

            Domain.Case.Case @case = _CaseAggregate.Case_T1_T2_.Get(con);

            _QuestionClassificationResolver.Resolve<Domain.Case.Case>(@case);

            return @case;
        }


        private string GetDefaultEmail(string template, int nodeID)
        {
            var _template = template;
            if (template.Contains("{{SourceEmailTitle}}"))
            {
                var defaultEmail = DataStorage.EmailDefaultTitleDict[DataStorage.NodeKeyDict[nodeID]];
                _template = template.Replace("{{SourceEmailTitle}}", defaultEmail);
            }

            return _template;
        }

        #endregion


    }
}
