using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Areas.Organization.Models.Role;
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
    public class RoleController : BaseApiController
    {
        private readonly IUserService _UserService;
        private readonly ICommonAggregate _CommonAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;

        public RoleController(
            IUserService UserService,
            ICommonAggregate CommonAggregate,
            IOrganizationAggregate OrganizationAggregate)
        {
            _UserService = UserService;
            _CommonAggregate = CommonAggregate;
            _OrganizationAggregate = OrganizationAggregate;
        }

        /// <summary>
        /// 取得清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(Role_lang.ROLE_GETLIST))]
        [ModelValidator(false)]
        public async Task<PagingResponse> GetList(PagingRequest<RoleSearchViewModel> model)
        {
            try
            {
                var con = new MSSQLCondition<Database.SMARTII.ROLE>(
                   model.criteria,
                   model.pageIndex,
                   model.pageSize
                   );

                con.OrderBy(model.sort, model.orderType);

                var list = _OrganizationAggregate.Role_T1_T2_.GetPaging(con);

                var ui = list.Select(x => new RoleListViewModel(x));

                return await new PagingResponse<IEnumerable<RoleListViewModel>>(ui)
                {
                    isSuccess = true,
                    totalCount = list.TotalCount
                }.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(Role_lang.ROLE_GETLIST_FAIL));

                return await new PagingResponse()
                {
                    isSuccess = false,
                    message = ex.PrefixMessage(Role_lang.ROLE_GETLIST_FAIL)
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
        [Logger(nameof(Role_lang.ROLE_GET))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Get([Required]int? RoleID)
        {
            try
            {
                var con = new MSSQLCondition<ROLE>(x => x.ID == RoleID);
                con.IncludeBy(x => x.USER);

                var item = _OrganizationAggregate.Role_T1_T2_.Get(con);

                if (item == null)
                    throw new Exception(Common_lang.NOT_FOUND_DATA);

                var result = new JsonResult<RoleDetailViewModel>(
                                   new RoleDetailViewModel(item), true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(Role_lang.ROLE_GET_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Role_lang.ROLE_GET_FAIL), false)
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
        [Logger(nameof(Role_lang.ROLE_CREATE))]
        [ModelValidator]
        public async Task<IHttpResult> Create(RoleDetailViewModel model)
        {
            try
            {
                var role = new Role()
                {
                    Feature = model.Feature,
                    IsEnabled = model.IsEnabled,
                    Name = model.Name,
                };

                var userIDs = model.Users?.Select(x => x.UserID).ToArray();

                await _UserService.CreateRoleAsync(role, userIDs);

                return await new JsonResult(
                   Role_lang.ROLE_CREATE_SUCCESS, true)
                   .Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                 ex.PrefixDevMessage(Role_lang.ROLE_CREATE_FAIL));

                return await new JsonResult(ex.PrefixMessage(Role_lang.ROLE_CREATE_FAIL), false)
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
        [Logger(nameof(Role_lang.ROLE_UPDATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Update(RoleDetailViewModel model)
        {
            try
            {
                var role = new Role()
                {
                    ID = model.RoleID.Value,
                    Feature = model.Feature,
                    IsEnabled = model.IsEnabled,
                    Name = model.Name,
                };

                var userIDs = model.Users?.Select(x => x.UserID).ToArray();

                await _UserService.UpdateRoleAsync(role, userIDs);

                return await new JsonResult(
                   Role_lang.ROLE_UPDATE_SUCCESS, true)
                   .Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                 ex.PrefixDevMessage(Role_lang.ROLE_UPDATE_FAIL));
                return await new JsonResult(ex.PrefixMessage(Role_lang.ROLE_UPDATE_FAIL), false)
                                 .Async();
            }
        }

        /// <summary>
        /// 單一停用
        /// </summary>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Delete)]
        [Logger(nameof(Role_lang.ROLE_DISABLE))]
        [ModelValidator]
        public async Task<IHttpResult> Disable([Required]int? RoleID)
        {
            try
            {
                var con = new MSSQLCondition<ROLE>(x => x.ID == RoleID);

                con.ActionModify(x =>
                {
                    x.IS_ENABLED = false;
                    x.UPDATE_DATETIME = DateTime.Now;
                    x.UPDATE_USERNAME = ContextUtility.GetUserIdentity()?.Name;
                });

                _OrganizationAggregate.Role_T1_T2_.Update(con);

                return await new JsonResult(Role_lang.ROLE_DISABLE_SUCCESS, true)
                  .Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
               ex.PrefixDevMessage(Role_lang.ROLE_DISABLE_FAIL));
                return await new JsonResult(ex.PrefixMessage(Role_lang.ROLE_DISABLE_FAIL), false)
                                 .Async();
            }
        }

        /// <summary>
        /// 批次停用
        /// </summary>
        /// <param name="RoleIDs"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Delete)]
        [Logger(nameof(Role_lang.ROLE_DISABLE_RANGE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> DisableRange(int[] RoleIDs)
        {
            try
            {
                var con = new MSSQLCondition<ROLE>(x => RoleIDs.Contains(x.ID));

                con.ActionModify(x =>
                {
                    x.IS_ENABLED = false;
                    x.UPDATE_DATETIME = DateTime.Now;
                    x.UPDATE_USERNAME = ContextUtility.GetUserIdentity()?.Name;
                });

                _OrganizationAggregate.Role_T1_T2_.UpdateRange(con);

                return await new JsonResult(
                    Role_lang.ROLE_DISABLE_RANGE_SUCCESS, true)
                    .Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(Role_lang.ROLE_DISABLE_RANGE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Role_lang.ROLE_DISABLE_RANGE_FAIL), false)
                    .Async();
            }
        }
    }
}
