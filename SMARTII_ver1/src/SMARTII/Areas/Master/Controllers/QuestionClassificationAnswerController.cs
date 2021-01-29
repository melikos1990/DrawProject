using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Areas.Master.Models.QuestionClassificationAnswer;
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
    public class QuestionClassificationAnswerController : BaseApiController
    {
        private readonly ICommonAggregate _CommonAggregate;
        private readonly IMasterAggregate _MasterAggregate;
        private readonly OrganizationNodeResolver _OrganizationResolver;
        private readonly IQuestionClassificationAnswerFacade _QuestionClassificationAnswerFacade;

        public QuestionClassificationAnswerController(ICommonAggregate CommonAggregate,
                                                      IMasterAggregate MasterAggregate,
                                                      OrganizationNodeResolver OrganizationResolver,
                                                      IQuestionClassificationAnswerFacade QuestionClassificationAnswerFacade)
        {
            _CommonAggregate = CommonAggregate;
            _MasterAggregate = MasterAggregate;
            _OrganizationResolver = OrganizationResolver;
            _QuestionClassificationAnswerFacade = QuestionClassificationAnswerFacade;
        }

        // <summary>
        /// 單一新增常用語
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Add)]
        [Logger(nameof(QuestionClassificationAnswer_lang.QUESTION_CLASSIFICATION_ANSWER_CREATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Create(List<QuestionClassificationAnswerDetailViewModel> model)
        {
            try
            {
                var domain = model.Select(x => x.ToDomain()).ToList();

                await _QuestionClassificationAnswerFacade.Create(domain);

                var result = new JsonResult<string>(QuestionClassificationAnswer_lang.QUESTION_CLASSIFICATION_ANSWER_CREATE_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(QuestionClassificationAnswer_lang.QUESTION_CLASSIFICATION_ANSWER_CREATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(QuestionClassificationAnswer_lang.QUESTION_CLASSIFICATION_ANSWER_CREATE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 單一刪除
        /// </summary>
        /// <param name="BuID"></param>
        /// <param name="ClassificKey"></param>
        /// <param name="Title"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Delete)]
        [Logger(nameof(QuestionClassificationAnswer_lang.QUESTION_CLASSIFICATION_ANSWER_DELETE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Delete([Required]int? ID)
        {
            try
            {
                var con = new MSSQLCondition<QUESTION_CLASSIFICATION_ANSWER>();

                con.And(x => x.ID == ID);

                var isSuccess = _MasterAggregate.QuestionClassificationAnswer_T1_T2_.Remove(con);

                if (isSuccess == false)
                    throw new Exception(Common_lang.DATABASE_DELETE_ERROR);

                return await new JsonResult(
                    QuestionClassificationAnswer_lang.QUESTION_CLASSIFICATION_ANSWER_DELETE_SUCCESS, true)
                    .Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                   ex.PrefixDevMessage(QuestionClassificationAnswer_lang.QUESTION_CLASSIFICATION_ANSWER_DELETE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(QuestionClassificationAnswer_lang.QUESTION_CLASSIFICATION_ANSWER_DELETE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 確認是否存在問題分類
        /// </summary>
        /// <param name="BuID"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthenticationMethod(AuthenticationType.None)]
        [Logger(nameof(QuestionClassificationAnswer_lang.QUESTION_CLASSIFICATION_ANSWER_CHECK))]
        [ModelValidator(false)]
        public async Task<IHttpResult> CheckQuestionCategory([Required]int? BuID)
        {
            try
            {
                var con = new MSSQLCondition<QUESTION_CLASSIFICATION>();

                con.And(x => x.NODE_ID == BuID);

                var hasAny = _MasterAggregate.QuestionClassification_T1_.HasAny(con);

                return await new JsonResult<Boolean>( hasAny , true).Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                   ex.PrefixDevMessage(QuestionClassificationAnswer_lang.QUESTION_CLASSIFICATION_ANSWER_DELETE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(QuestionClassificationAnswer_lang.QUESTION_CLASSIFICATION_ANSWER_DELETE_FAIL), false)
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
        [Logger(nameof(QuestionClassificationAnswer_lang.QUESTION_CLASSIFICATION_ANSWER_GET))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Get([Required]int? ID)
        {
            try
            {
                var data = _MasterAggregate.VWQuestionClassificationAnswer_QuestionClassificationAnswer_.Get(x => x.ANSWER_ID == ID);
                if (data == null)
                    throw new Exception(Common_lang.NOT_FOUND_DATA);

                var result = new JsonResult<QuestionClassificationAnswerDetailViewModel>(
                                   new QuestionClassificationAnswerDetailViewModel(data), true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(QuestionClassificationAnswer_lang.QUESTION_CLASSIFICATION_ANSWER_GET_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(QuestionClassificationAnswer_lang.QUESTION_CLASSIFICATION_ANSWER_GET_FAIL), false)
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
        [Logger(nameof(QuestionClassificationAnswer_lang.QUESTION_CLASSIFICATION_ANSWER_GETLIST))]
        [ModelValidator(false)]
        public async Task<PagingResponse> GetList(PagingRequest<QuestionClassificationAnswerSearchViewModel> model)
        {
            try
            {
                var searchTerm = model.criteria;

                var con = new MSSQLCondition<VW_QUESTION_CLASSIFICATION_ANSWER_NESTED>(
                   searchTerm,
                   model.pageIndex,
                   model.pageSize);

                con.OrderBy(model.sort, model.orderType);

                var list = _MasterAggregate.VWQuestionClassificationAnswer_QuestionClassificationAnswer_.GetPaging(con);

                var ui = _OrganizationResolver.ResolveCollection(list)
                         .Select(x => new QuestionClassificationAnswerListViewModel(x));

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
        /// 單一更新常用語
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Update)]
        [Logger(nameof(QuestionClassificationAnswer_lang.QUESTION_CLASSIFICATION_ANSWER_UPDATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Update(QuestionClassificationAnswerDetailViewModel model)
        {
            try
            {
                var domain = model.ToDomain();

                await _QuestionClassificationAnswerFacade.Update(domain);

                var result = new JsonResult<string>(QuestionClassificationAnswer_lang.QUESTION_CLASSIFICATION_ANSWER_UPDATE_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(QuestionClassificationAnswer_lang.QUESTION_CLASSIFICATION_ANSWER_UPDATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(QuestionClassificationAnswer_lang.QUESTION_CLASSIFICATION_ANSWER_UPDATE_FAIL), false)
                    .Async();
            }
        }
    }
}
