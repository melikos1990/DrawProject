using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Areas.Master.Models.QuestionClassificationGuide;
using SMARTII.Assist.Authentication;
using SMARTII.Assist.Logger;
using SMARTII.Assist.Web;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Master;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;
using SMARTII.Service.Organization.Resolver;

namespace SMARTII.Areas.Master.Controllers
{
    [Authentication]
    public class QuestionClassificationGuideController : BaseApiController
    {
        private readonly ICommonAggregate _CommonAggregate;
        private readonly IMasterAggregate _MasterAggregate;
        private readonly IQuestionClassificationGuideFacade _QuestionClassificationGuideFacade;
        private readonly OrganizationNodeResolver _OrganizationResolver;

        public QuestionClassificationGuideController(
            ICommonAggregate CommonAggregate,
            IMasterAggregate MasterAggregate,
            IQuestionClassificationGuideFacade QuestionClassificationGuideFacade,
            OrganizationNodeResolver OrganizationResolver)
        {
            _CommonAggregate = CommonAggregate;
            _MasterAggregate = MasterAggregate;
            _QuestionClassificationGuideFacade = QuestionClassificationGuideFacade;
            _OrganizationResolver = OrganizationResolver;
        }

        // <summary>
        /// 單一新增流程引導
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Add)]
        [Logger(nameof(QuestionClassificationGuide_lang.QUESTION_CLASSIFICATION_GUIDE_CREATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Create(QCGuideDetailViewModel model)
        {
            try
            {
                var domain = model.ToDomain();

                await _QuestionClassificationGuideFacade.Create(domain);

                var result = new JsonResult<string>(QuestionClassificationGuide_lang.QUESTION_CLASSIFICATION_GUIDE_CREATE_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(QuestionClassificationGuide_lang.QUESTION_CLASSIFICATION_GUIDE_CREATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(QuestionClassificationGuide_lang.QUESTION_CLASSIFICATION_GUIDE_CREATE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 單一刪除
        /// </summary>
        /// <param name="ClassificcationID"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Delete)]
        [Logger(nameof(QuestionClassificationGuide_lang.QUESTION_CLASSIFICATION_GUIDE_DELETE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Delete([Required]int? ID)
        {
            try
            {
                var con = new MSSQLCondition<QUESTION_CLASSIFICATION_GUIDE>();

                con.And(x => x.CLASSIFICATION_ID == ID);

                var isSuccess = _MasterAggregate.QuestionClassificationGuide_T1_T2_.Remove(con);

                if (isSuccess == false)
                    throw new Exception(Common_lang.DATABASE_DELETE_ERROR);

                return await new JsonResult(
                    QuestionClassificationGuide_lang.QUESTION_CLASSIFICATION_GUIDE_DELETE_SUCCESS, true)
                    .Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                   ex.PrefixDevMessage(QuestionClassificationGuide_lang.QUESTION_CLASSIFICATION_GUIDE_DELETE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(QuestionClassificationGuide_lang.QUESTION_CLASSIFICATION_GUIDE_DELETE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 單一取得
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(QuestionClassificationGuide_lang.QUESTION_CLASSIFICATION_GUIDE_GET))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Get([Required]int? ID)
        {
            try
            {
                var data = _MasterAggregate.VWQuestionClassificationGuide_QuestionClassificationGuide_.Get(x => x.ID == ID);
                if (data == null)
                    throw new Exception(Common_lang.NOT_FOUND_DATA);

                var result = new JsonResult<QCGuideDetailViewModel>(
                                   new QCGuideDetailViewModel(data), true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(QuestionClassificationGuide_lang.QUESTION_CLASSIFICATION_GUIDE_GET_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(QuestionClassificationGuide_lang.QUESTION_CLASSIFICATION_GUIDE_GET_FAIL), false)
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
        [Logger(nameof(QuestionClassificationGuide_lang.QUESTION_CLASSIFICATION_GUIDE_GETLIST))]
        [ModelValidator(false)]
        public async Task<PagingResponse> GetList(PagingRequest<QCGuideSearchViewModel> model)
        {
            try
            {
                var searchTerm = model.criteria;

                var con = new MSSQLCondition<VW_QUESTION_CLASSIFICATION_GUIDE_NESTED>(
                   model.pageIndex,
                   model.pageSize);

                con.And(c => c.NODE_ID == searchTerm.NodeID);

                if (model.criteria.ClassificationID != null) {
                    con.And(c => c.NODE_ID == searchTerm.NodeID && c.PARENT_PATH.Contains(searchTerm.ClassificationID.ToString()));
                }

                con.OrderBy(model.sort, model.orderType);

                var list = _MasterAggregate.VWQuestionClassificationGuide_QuestionClassificationGuide_.GetPaging(con);

                var ui = _OrganizationResolver.ResolveCollection(list)
                         .Select(x => new QCGuideListViewModel(x));

                return await new PagingResponse<IEnumerable<QCGuideListViewModel>>(ui)
                {
                    isSuccess = true,
                    totalCount = list.TotalCount
                }.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(QuestionClassificationGuide_lang.QUESTION_CLASSIFICATION_GUIDE_GETLIST_FAIL));

                return await new PagingResponse()
                {
                    isSuccess = false,
                    message = ex.PrefixMessage(QuestionClassificationGuide_lang.QUESTION_CLASSIFICATION_GUIDE_GETLIST_FAIL)
                }.Async();
            }
        }

        /// <summary>
        /// 單一更新流程引導
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Update)]
        [Logger(nameof(QuestionClassificationGuide_lang.QUESTION_CLASSIFICATION_GUIDE_UPDATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Update(QCGuideDetailViewModel model)
        {
            try
            {
                var domain = model.ToDomain();

                await _QuestionClassificationGuideFacade.Update(domain);

                var result = new JsonResult<string>(QuestionClassificationGuide_lang.QUESTION_CLASSIFICATION_GUIDE_UPDATE_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(QuestionClassificationGuide_lang.QUESTION_CLASSIFICATION_GUIDE_UPDATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(QuestionClassificationGuide_lang.QUESTION_CLASSIFICATION_GUIDE_UPDATE_FAIL), false)
                    .Async();
            }
        }
    }
}
