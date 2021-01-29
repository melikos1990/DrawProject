using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Areas.Master.Models.CaseRemind;
using SMARTII.Assist.Authentication;
using SMARTII.Assist.Logger;
using SMARTII.Assist.Web;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Case;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;
using SMARTII.Service.Organization.Resolver;

namespace SMARTII.Areas.Master.Controllers
{
    [Authentication]
    public class CaseRemindController : BaseApiController
    {
        private readonly ICaseRemindFacade _CaseRemindFacade;
        private readonly ICommonAggregate _CommonAggregate;
        private readonly ICaseAggregate _CaseAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly OrganizationNodeResolver _OrganizationResolver;

        public CaseRemindController(
            ICaseRemindFacade CaseRemindFacade,
            ICommonAggregate CommonAggregate,
            ICaseAggregate CaseAggregate,
            IOrganizationAggregate OrganizationAggregate,
            OrganizationNodeResolver OrganizationResolver)
        {
            _CaseRemindFacade = CaseRemindFacade;
            _CommonAggregate = CommonAggregate;
            _CaseAggregate = CaseAggregate;
            _OrganizationAggregate = OrganizationAggregate;
            _OrganizationResolver = OrganizationResolver;
        }

        /// <summary>
        /// 單一新增案件追蹤
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Add)]
        [Logger(nameof(CaseRemind_lang.CASEREMIND_CREATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Create(CaseRemindDetailViewModel model)
        {
            try
            {
                var domain = model.ToDomain();

                await _CaseRemindFacade.Create(domain);

                var result = new JsonResult<string>(CaseRemind_lang.CASEREMIND_CREATE_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(CaseRemind_lang.CASEREMIND_CREATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(CaseRemind_lang.CASEREMIND_CREATE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 單一刪除案件追蹤
        /// </summary>
        /// <param name="BuID"></param>
        /// <param name="ClassificKey"></param>
        /// <param name="Title"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Delete)]
        [Logger(nameof(CaseRemind_lang.CASEREMIND_DELETE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Delete([Required]int? ID)
        {
            try
            {
                var con = new MSSQLCondition<CASE_REMIND>();

                con.And(x => x.ID == ID);

                var isSuccess = _CaseAggregate.CaseRemind_T1_T2_.Remove(con);

                if (isSuccess == false)
                    throw new Exception(Common_lang.DATABASE_DELETE_ERROR);

                return await new JsonResult(
                    CaseRemind_lang.CASEREMIND_DELETE_SUCCESS, true)
                    .Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                   ex.PrefixDevMessage(CaseRemind_lang.CASEREMIND_DELETE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(CaseRemind_lang.CASEREMIND_DELETE_FAIL), false)
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
        [Logger(nameof(CaseRemind_lang.CASEREMIND_GET))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Get([Required]int? ID)
        {
            try
            {
                var data = _CaseAggregate.CaseRemind_T1_T2_.Get(x => x.ID == ID);
                if (data == null)
                    throw new Exception(Common_lang.NOT_FOUND_DATA);

                var userIDs = data?.UserIDs ?? new List<string>();

                var users = _OrganizationAggregate.User_T1_T2_.GetList(x => userIDs.Contains(x.USER_ID));

                data.Users = users.ToList();

                var result = new JsonResult<CaseRemindDetailViewModel>(
                                   new CaseRemindDetailViewModel(data), true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(CaseRemind_lang.CASEREMIND_GET_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(CaseRemind_lang.CASEREMIND_GET_FAIL), false)
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
        [Logger(nameof(CaseRemind_lang.CASEREMIND_GETLIST))]
        [ModelValidator(false)]
        public async Task<PagingResponse> GetList(PagingRequest<CaseRemindSearchViewModel> model)
        {
            try
            {
                var searchTerm = model.criteria;

                var con = new MSSQLCondition<CASE_REMIND>(
                   searchTerm,
                   model.pageIndex,
                   model.pageSize);

                con.OrderBy(model.sort, model.orderType);

                var list = _CaseAggregate.CaseRemind_T1_T2_.GetPaging(con);

                var ui = _OrganizationResolver.ResolveCollection(list)
                         .Select(x => new CaseRemindListViewModel(x));

                return await new PagingResponse<IEnumerable<CaseRemindListViewModel>>(ui)
                {
                    isSuccess = true,
                    totalCount = list.TotalCount
                }.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(CaseRemind_lang.CASEREMIND_GETLIST_FAIL));

                return await new PagingResponse()
                {
                    isSuccess = false,
                    message = ex.PrefixMessage(CaseRemind_lang.CASEREMIND_GETLIST_FAIL)
                }.Async();
            }
        }

        /// <summary>
        /// 單一更新案件追蹤
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Update)]
        [Logger(nameof(CaseRemind_lang.CASEREMIND_UPDATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Update(CaseRemindDetailViewModel model)
        {
            try
            {
                var domain = model.ToDomain();

                await _CaseRemindFacade.Update(domain);

                var result = new JsonResult<string>(CaseRemind_lang.CASEREMIND_UPDATE_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(CaseRemind_lang.CASEREMIND_UPDATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(CaseRemind_lang.CASEREMIND_UPDATE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 完成案件追蹤
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Update)]
        [Logger(nameof(CaseRemind_lang.CASEREMIND_CONFIRM))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Confirm([Required] int id)
        {
            try
            {
                var con = new MSSQLCondition<CASE_REMIND>(x => x.ID == id);

                con.ActionModify(x =>
                {
                    x.IS_CONFIRM = true;
                    x.CONFIRM_DATETIME = DateTime.Now;
                    x.CONFIRM_USER_ID = ContextUtility.GetUserIdentity().Instance.UserID;
                    x.CONFIRM_USERNAME = ContextUtility.GetUserIdentity()?.Name;
                    x.UPDATE_DATETIME = DateTime.Now;
                    x.UPDATE_USERNAME = ContextUtility.GetUserIdentity()?.Name;
                });

                _CaseAggregate.CaseRemind_T1_T2_.Update(con);

                return await new JsonResult<string>(CaseRemind_lang.CASEREMIND_CONFIRM_SUCCESS, true).Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(CaseRemind_lang.CASEREMIND_CONFIRM_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(CaseRemind_lang.CASEREMIND_CONFIRM_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 檢查案件
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(CaseRemind_lang.CASEREMIND_CHECK_CASE_ID))]
        [ModelValidator(false)]
        public async Task<IHttpResult> CheckCaseID([Required]string CaseID)
        {
            try
            {
                var con = new MSSQLCondition<CASE>();
                con.And(x => x.CASE_ID == CaseID);

                var data = _CaseAggregate.Case_T1_T2_.HasAny(con);

                if (data)
                {
                    return new JsonResult<bool>(data, true);
                }

                return new JsonResult<bool>(data, Case_lang.CASE_GET_FAIL, false);
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(User_lang.USER_CHECK_NAME_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(User_lang.USER_CHECK_NAME_FAIL), false)
                    .Async();
            }
        }
    }
}
