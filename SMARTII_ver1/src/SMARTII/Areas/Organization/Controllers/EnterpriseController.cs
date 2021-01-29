using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Areas.Organization.Models.Enterprise;
using SMARTII.Assist.Authentication;
using SMARTII.Assist.Logger;
using SMARTII.Assist.Web;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;

namespace SMARTII.Areas.Organization.Controllers
{
    [Authentication]
    public class EnterpriseController : BaseApiController
    {
        private readonly ICommonAggregate _CommonAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly IEnterpriseFacade _EnterpriseFacade;

        public EnterpriseController(
            ICommonAggregate CommonAggregate,
            IOrganizationAggregate OrganizationAggregate,
            IEnterpriseFacade EnterpriseFacade)
        {
            _CommonAggregate = CommonAggregate;
            _OrganizationAggregate = OrganizationAggregate;
            _EnterpriseFacade = EnterpriseFacade;
        }

        /// <summary>
        /// 取得清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(Enterprise_lang.ENTERPRISE_GETLIST))]
        [ModelValidator(false)]
        public async Task<PagingResponse> GetList(PagingRequest<EnterpriseSearchViewModel> model)
        {
            try
            {
                var searchTerm = model.criteria;

                var con = new MSSQLCondition<ENTERPRISE>(
                   searchTerm,
                   model.pageIndex,
                   model.pageSize);

                con.OrderBy(model.sort, model.orderType);

                var list = _OrganizationAggregate.Enterprise_T1_T2_.GetPaging(con);

                var ui = list.Select(x => new EnterpriseListViewModel(x));

                return await new PagingResponse<IEnumerable<EnterpriseListViewModel>>(ui)
                {
                    isSuccess = true,
                    totalCount = list.TotalCount
                }.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(Enterprise_lang.ENTERPRISE_GETLIST_FAIL));

                return await new PagingResponse()
                {
                    isSuccess = false,
                    message = ex.PrefixMessage(Enterprise_lang.ENTERPRISE_GETLIST_FAIL)
                }.Async();
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
        [Logger(nameof(Enterprise_lang.ENTERPRISE_GET))]
        [ModelValidator]
        public async Task<IHttpResult> Get([Required]int? EnterpriseID)
        {
            try
            {
                var item = _OrganizationAggregate.Enterprise_T1_T2_
                                                  .Get(x => x.ID == EnterpriseID);

                if (item == null)
                    throw new Exception(Common_lang.NOT_FOUND_DATA);

                var result = new JsonResult<EnterpriseDetailViewModel>(
                                   new EnterpriseDetailViewModel(item), true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(Enterprise_lang.ENTERPRISE_GET_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Enterprise_lang.ENTERPRISE_GET_FAIL), false)
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
        [Logger(nameof(Enterprise_lang.ENTERPRISE_CREATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Create(EnterpriseDetailViewModel Model)
        {
            try
            {
                var domain = new Enterprise()
                {
                    Name = Model.Name,
                    IsEnabled = Model.IsEnabled,
                    CreateDateTime = DateTime.Now,
                    CreateUserName = UserIdentity.Name
                };

                var result = await _EnterpriseFacade.Create(domain);

                return await new JsonResult(
                    Enterprise_lang.ENTERPRISE_CREATE_SUCCESS, true)
                    .Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(Enterprise_lang.ENTERPRISE_CREATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Enterprise_lang.ENTERPRISE_CREATE_FAIL), false)
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
        [Logger(nameof(Enterprise_lang.ENTERPRISE_UPDATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Update(EnterpriseDetailViewModel Model)
        {
            try
            {
                var domain = new Enterprise()
                {
                    ID = Model.EnterpriseID,
                    Name = Model.Name,
                    IsEnabled = Model.IsEnabled,
                };

                var result = await _EnterpriseFacade.Update(domain);

                return await new JsonResult(
                    Enterprise_lang.ENTERPRISE_UPDATE_SUCCESS, true)
                    .Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(Enterprise_lang.ENTERPRISE_UPDATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Enterprise_lang.ENTERPRISE_UPDATE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 單一刪除
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Delete)]
        [Logger(nameof(Enterprise_lang.ENTERPRISE_DELETE))]
        [ModelValidator]
        public async Task<IHttpResult> Delete([Required]int? EnterpriseID)
        {
            try
            {
                var isSuccess = _OrganizationAggregate.Enterprise_T1_T2_
                                                       .Remove(x => x.ID == EnterpriseID);

                if (isSuccess == false)
                    throw new Exception(Common_lang.DATABASE_DELETE_ERROR);

                return await new JsonResult(
                    Enterprise_lang.ENTERPRISE_DELETE_SUCCESS, true)
                    .Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                   ex.PrefixDevMessage(Enterprise_lang.ENTERPRISE_DELETE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Enterprise_lang.ENTERPRISE_DELETE_FAIL), false)
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
        [Logger(nameof(Enterprise_lang.ENTERPRISE_DELETE_RANGE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> DeleteRange([Required]int[] EnterpriseIDs)
        {
            try
            {
                var con = new MSSQLCondition<ENTERPRISE>(x => EnterpriseIDs.Contains(x.ID));

                var isSuccess = _OrganizationAggregate.Enterprise_T1_.RemoveRange(con);

                if (isSuccess == false)
                    throw new Exception(Common_lang.DATABASE_DELETE_ERROR);

                return await new JsonResult(
                 Enterprise_lang.ENTERPRISE_DELETE_SUCCESS, true)
                 .Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(Enterprise_lang.ENTERPRISE_DELETE_RANGE_FAILED));

                return await new JsonResult(
                    ex.PrefixMessage(Enterprise_lang.ENTERPRISE_DELETE_RANGE_SUCCESS), false)
                    .Async();
            }
        }

        /// <summary>
        /// 單一停用
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Delete)]
        [Logger(nameof(Enterprise_lang.ENTERPRISE_DISABLED))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Disabled([Required]int? ID)
        {
            try
            {
                var con = new MSSQLCondition<ENTERPRISE>(x => x.ID == ID);
                con.ActionModify(x =>
                {
                    x.IS_ENABLED = false;
                    x.UPDATE_DATETIME = DateTime.Now;
                    x.UPDATE_USERNAME = UserIdentity.Instance.Name;
                });

                _OrganizationAggregate.Enterprise_T1_.Update(con);

                var result = new JsonResult<string>(Enterprise_lang.ENTERPRISE_DISABLED_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                     ex.PrefixDevMessage(Enterprise_lang.ENTERPRISE_DISABLED_FAILED));

                return await new JsonResult(
                    ex.PrefixMessage(Enterprise_lang.ENTERPRISE_DISABLED_FAILED), false)
                    .Async();
            }
        }
    }
}
