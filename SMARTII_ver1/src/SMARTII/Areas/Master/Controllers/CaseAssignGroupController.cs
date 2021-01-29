using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Areas.Master.Models.CaseAssignGroup;
using SMARTII.Assist.Authentication;
using SMARTII.Assist.Logger;
using SMARTII.Assist.Web;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Case;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;
using SMARTII.Service.Organization.Resolver;

namespace SMARTII.Areas.Master.Controllers
{
    [Authentication]
    public class CaseAssignGroupController : BaseApiController
    {
        private readonly ICaseAssignGroupFacade _CaseAssignGroupFacade;
        private readonly ICommonAggregate _CommonAggregate;
        private readonly ICaseAggregate _CaseAggregate;
        private readonly OrganizationNodeResolver _OrganizationResolver;

        public CaseAssignGroupController(
            ICaseAssignGroupFacade CaseAssignGroupFacade,
            ICommonAggregate CommonAggregate,
            ICaseAggregate MasterAggregate,
            OrganizationNodeResolver OrganizationResolver)
        {
            _CaseAssignGroupFacade = CaseAssignGroupFacade;
            _CommonAggregate = CommonAggregate;
            _CaseAggregate = MasterAggregate;
            _OrganizationResolver = OrganizationResolver;
        }

        /// <summary>
        /// 取得清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(CaseAssignGroup_lang.CASE_ASSIGN_GROUP_GETLIST))]
        [ModelValidator(false)]
        public async Task<PagingResponse> GetList(PagingRequest<CaseAssignGroupSearchViewModel> model)
        {
            try
            {
                var buIDs = UserIdentity.Instance.DownProviderBUDist.TryGetBuList(Domain.Organization.OrganizationType.CallCenter);

                var searchTerm = model.criteria;

                var con = new MSSQLCondition<CASE_ASSIGN_GROUP>(
                   searchTerm,
                   model.pageIndex,
                   model.pageSize);

                //若無指定BU查詢，將會撈出該人員負責之BU
                if (searchTerm.BuID == null)
                {
                    con.And(x => buIDs.Contains(x.NODE_ID));
                }

                con.OrderBy(model.sort, model.orderType);

                var list = _CaseAggregate.CaseAssignmentGroup_T1_T2_.GetPaging(con);

                var ui = _OrganizationResolver.ResolveCollection(list).Select(x => new CaseAssignGroupListViewModel(x));

                return await new PagingResponse<IEnumerable<CaseAssignGroupListViewModel>>(ui)
                {
                    isSuccess = true,
                    totalCount = list.TotalCount
                }.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(CaseAssignGroup_lang.CASE_ASSIGN_GROUP_GETLIST_FAIL));

                return await new PagingResponse()
                {
                    isSuccess = false,
                    message = ex.PrefixMessage(CaseAssignGroup_lang.CASE_ASSIGN_GROUP_GETLIST_FAIL)
                }.Async();
            }
        }

        /// <summary>
        /// 單一取得
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(CaseAssignGroup_lang.CASE_ASSIGN_GROUP_GET))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Get([Required]int? ID)
        {
            try
            {
                var con = new MSSQLCondition<CASE_ASSIGN_GROUP>(x => x.ID == ID.Value);
                con.IncludeBy(x => x.CASE_ASSIGN_GROUP_USER);

                var data = _CaseAggregate.CaseAssignmentGroup_T1_T2_.Get(con);

                var complete = _OrganizationResolver.Resolve(data);

                var result = new JsonResult<CaseAssignGroupDetailViewModel>(
                                   new CaseAssignGroupDetailViewModel(complete), true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(CaseAssignGroup_lang.CASE_ASSIGN_GROUP_GET_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(CaseAssignGroup_lang.CASE_ASSIGN_GROUP_GET_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 單一新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Add)]
        [Logger(nameof(CaseAssignGroup_lang.CASE_ASSIGN_GROUP_CREATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Create(CaseAssignGroupDetailViewModel model)
        {
            try
            {
                var domain = model.ToDomain();

                await _CaseAssignGroupFacade.Create(domain);

                var result = new JsonResult<string>(CaseAssignGroup_lang.CASE_ASSIGN_GROUP_CREATE_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(CaseAssignGroup_lang.CASE_ASSIGN_GROUP_CREATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(CaseAssignGroup_lang.CASE_ASSIGN_GROUP_CREATE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 單一更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Update)]
        [Logger(nameof(CaseAssignGroup_lang.CASE_ASSIGN_GROUP_UPDATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Update(CaseAssignGroupDetailViewModel model)
        {
            try
            {
                var domain = model.ToDomain();

                await _CaseAssignGroupFacade.Update(domain);

                var result = new JsonResult<string>(CaseAssignGroup_lang.CASE_ASSIGN_GROUP_UPDATE_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(CaseAssignGroup_lang.CASE_ASSIGN_GROUP_UPDATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(CaseAssignGroup_lang.CASE_ASSIGN_GROUP_UPDATE_FAIL), false)
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
        [Logger(nameof(CaseAssignGroup_lang.CASE_ASSIGN_GROUP_DELETE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Delete([Required]int? ID)
        {
            try
            {
                var con = new MSSQLCondition<CASE_ASSIGN_GROUP>();
                con.IncludeBy(x => x.CASE_ASSIGN_GROUP_USER);

                con.And(x => x.ID == ID);

                var isSuccess = _CaseAggregate.CaseAssignmentGroup_T1_T2_.Remove(con);

                if (isSuccess == false)
                    throw new Exception(Common_lang.DATABASE_DELETE_ERROR);

                return await new JsonResult(
                    CaseAssignGroup_lang.CASE_ASSIGN_GROUP_DELETE_SUCCESS, true)
                    .Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                   ex.PrefixDevMessage(CaseAssignGroup_lang.CASE_ASSIGN_GROUP_DELETE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(CaseAssignGroup_lang.CASE_ASSIGN_GROUP_DELETE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 批次刪除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Delete)]
        [Logger(nameof(CaseAssignGroup_lang.CASE_ASSIGN_GROUP_DELETE_RANGE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> DeleteRange(List<CaseAssignGroupListViewModel> model)
        {
            try
            {
                var con = new MSSQLCondition<CASE_ASSIGN_GROUP>();
                con.IncludeBy(x => x.CASE_ASSIGN_GROUP_USER);

                model.ForEach(g => con.Or(x => x.ID == g.ID));

                var isSuccess = _CaseAggregate.CaseAssignmentGroup_T1_T2_.RemoveRange(con);

                if (isSuccess == false)
                    throw new Exception(Common_lang.DATABASE_DELETE_ERROR);

                return await new JsonResult(
                 CaseAssignGroup_lang.CASE_ASSIGN_GROUP_DELETE_RANGE_SUCCESS, true)
                 .Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(CaseAssignGroup_lang.CASE_ASSIGN_GROUP_DELETE_RANGE_FAILED));

                return await new JsonResult(
                    ex.PrefixMessage(CaseAssignGroup_lang.CASE_ASSIGN_GROUP_DELETE_RANGE_FAILED), false)
                    .Async();
            }
        }
    }
}
