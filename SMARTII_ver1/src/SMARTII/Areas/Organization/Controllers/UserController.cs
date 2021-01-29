using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Areas.Organization.Models.User;
using SMARTII.Assist.Authentication;
using SMARTII.Assist.Logger;
using SMARTII.Assist.Web;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Authentication;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Authentication.Service;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;

namespace SMARTII.Areas.Organization.Controllers
{
    [Authentication]
    public class UserController : BaseApiController
    {
        private readonly IUserService _UserService;
        private readonly ICommonAggregate _CommonAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly IAuthenticationAggregate _AuthenticationAggregate;
        private readonly IUserAuthenticationManager _UserAuthenticationManager;
        private readonly IUserFacade _UserFacade;

        public UserController(
            IUserService UserService,
            ICommonAggregate CommonAggregate,
            IOrganizationAggregate OrganizationAggregate,
            IAuthenticationAggregate AuthenticationAggregate,
            IUserAuthenticationManager UserAuthenticationManager,
            IUserFacade UserFacade)
        {
            _UserService = UserService;
            _CommonAggregate = CommonAggregate;
            _OrganizationAggregate = OrganizationAggregate;
            _AuthenticationAggregate = AuthenticationAggregate;
            _UserAuthenticationManager = UserAuthenticationManager;
            _UserFacade = UserFacade;
        }

        /// <summary>
        /// 取得清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(User_lang.USER_GETLIST))]
        [ModelValidator(false)]
        public async Task<PagingResponse> GetList(PagingRequest<UserSearchViewModel> model)
        {
            try
            {
                var searchTerm = model.criteria;

                var con = new MSSQLCondition<USER>(
                   searchTerm,
                   model.pageIndex,
                   model.pageSize);

                con.IncludeBy(x => x.ROLE);
                con.OrderBy(model.sort, model.orderType);

                var list = _OrganizationAggregate.User_T1_T2_.GetPaging(con);

                var ui = list.Select(x => new UserListViewModel(x));

                return await new PagingResponse<IEnumerable<UserListViewModel>>(ui)
                {
                    isSuccess = true,
                    totalCount = list.TotalCount
                }.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(User_lang.USER_GETLIST_FAIL));

                return await new PagingResponse()
                {
                    isSuccess = false,
                    message = ex.PrefixMessage(User_lang.USER_GETLIST_FAIL)
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
        [Logger(nameof(User_lang.USER_GET))]
        [ModelValidator]
        public async Task<IHttpResult> Get([Required]string UserID)
        {
            try
            {
                var con = new MSSQLCondition<USER>(x => x.USER_ID == UserID);
                con.IncludeBy(x => x.ROLE);

                var item = _OrganizationAggregate.User_T1_T2_.Get(con);

                if (item == null)
                    throw new Exception(Common_lang.NOT_FOUND_DATA);

                var result = new JsonResult<UserDetailViewModel>(
                                   new UserDetailViewModel(item), true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(User_lang.USER_GET_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(User_lang.USER_GET_FAIL), false)
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
        [Logger(nameof(User_lang.USER_CREATE))]
        [ModelValidator]
        public async Task<IHttpResult> Create(UserDetailViewModel model)
        {
            try
            {
                var user = new User()
                {
                    LastChangePasswordDateTime = null,
                    UserID = Guid.NewGuid().ToString(),
                    Email = model.Email,
                    Telephone = model.Telephone,
                    Mobile = model.Mobile,
                    IsEnabled = model.IsEnable,
                    Name = model.Name,
                    Feature = model.Feature,
                    IsSystemUser = model.IsSystemUser,
                    Ext = model.Ext
                };

                if (user.IsSystemUser)
                {
                    user.Account = model.Account;
                    user.IsAD = model.IsAD;
                    user.ActiveStartDateTime = model.ActiveStartDateTime ?? DateTime.Now;
                    user.ActiveEndDateTime = model.ActiveEndDateTime ?? DateTime.MaxValue;
                }

                await _UserService.CreateAsync(user, model.RoleIDs ?? new int[] { });

                return await new JsonResult(
                   User_lang.USER_CREATE_SUCCESS, true)
                   .Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                 ex.PrefixDevMessage(User_lang.USER_CREATE_FAIL));

                return await new JsonResult(ex.PrefixMessage(User_lang.USER_CREATE_FAIL), false)
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
        [Logger(nameof(User_lang.USER_UPDATE))]
        [ModelValidator]
        public async Task<IHttpResult> Update(UserDetailViewModel model)
        {
            try
            {
                var user = new User()
                {
                    UserID = model.UserID,
                    Email = model.Email,
                    Telephone = model.Telephone,
                    Mobile = model.Mobile,
                    IsEnabled = model.IsEnable,
                    Name = model.Name,
                    Feature = model.Feature,
                    IsSystemUser = model.IsSystemUser,
                    Ext = model.Ext
                };

                if (user.IsSystemUser)
                {
                    user.Account = model.Account;
                    user.Password = model.Password;
                    user.IsAD = model.IsAD;
                    user.ActiveStartDateTime = model.ActiveStartDateTime ?? DateTime.Now;
                    user.ActiveEndDateTime = model.ActiveEndDateTime ?? DateTime.MaxValue;
                }

                await _UserService.UpdateAsync(user, model.RoleIDs ?? new int[] { });

                return await new JsonResult(
                   User_lang.USER_UPDATE_SUCCESS, true)
                   .Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                 ex.PrefixDevMessage(User_lang.USER_UPDATE_FAIL));
                return await new JsonResult(ex.PrefixMessage(User_lang.USER_UPDATE_FAIL), false)
                                 .Async();
            }
        }

        /// <summary>
        /// 單一停用
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Delete)]
        [Logger(nameof(User_lang.USER_DISABLE))]
        [ModelValidator]
        public async Task<IHttpResult> Disable([Required]string UserID)
        {
            try
            {
                var con = new MSSQLCondition<USER>(x => x.USER_ID == UserID);

                con.ActionModify(x => x.IS_ENABLED = false);

                _OrganizationAggregate.User_T1_.Update(con);

                return await new JsonResult(
                  User_lang.USER_DISABLE_SUCCESS, true)
                  .Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
               ex.PrefixDevMessage(User_lang.USER_DISABLE_FAIL));
                return await new JsonResult(ex.PrefixMessage(User_lang.USER_DISABLE_FAIL), false)
                                 .Async();
            }
        }

        /// <summary>
        /// 批次停用
        /// </summary>
        /// <param name="UserIDs"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Delete)]
        [Logger(nameof(User_lang.USER_DISABLE_RANGE))]
        [ModelValidator]
        public async Task<IHttpResult> DisableRange([Required]string[] UserIDs)
        {
            try
            {
                var con = new MSSQLCondition<USER>(x => UserIDs.Contains(x.USER_ID));

                con.ActionModify(x => x.IS_ENABLED = false);

                _OrganizationAggregate.User_T1_.UpdateRange(con);

                return await new JsonResult(
                  User_lang.USER_DISABLE_RANGE_SUCCESS, true)
                  .Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex.PrefixDevMessage(User_lang.USER_DISABLE_RANGE_FAIL));

                return await new JsonResult(ex.PrefixMessage(User_lang.USER_DISABLE_RANGE_FAIL), false)
                                 .Async();
            }
        }

        /// <summary>
        /// 驗證AD 帳戶
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Logger(nameof(User_lang.USER_VALID_AD_PASSWORD))]
        [ModelValidator(false)]
        public async Task<IHttpResult> ValidADPassword(ADViewModel model)
        {
            try
            {
                bool isAD = _AuthenticationAggregate.AD.IsADUser(model.Account);

                if (!isAD)
                    throw new Exception(Common_lang.AD_USER_NULL);

                return await new JsonResult(User_lang.USER_VALID_AD_PASSWORD_FAIL, true).Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex.PrefixDevMessage(User_lang.USER_VALID_AD_PASSWORD_FAIL));

                return await new JsonResult(ex.PrefixMessage(User_lang.USER_VALID_AD_PASSWORD_FAIL), false)
                                 .Async();
            }
        }

        /// <summary>
        /// 重新設置密碼
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Update)]
        [Logger(nameof(User_lang.USER_RESET_DEFAULT_PASSWORD))]
        [ModelValidator]
        public async Task<IHttpResult> ResetPassword([Required]string account)
        {
            try
            {
                await _UserAuthenticationManager.ResetDefaultPasswordAsync(account);

                return await new JsonResult(User_lang.USER_RESET_DEFAULT_PASSWORD_SUCCESS, true).Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex.PrefixDevMessage(User_lang.USER_RESET_DEFAULT_PASSWORD_FAIL));

                return await new JsonResult(ex.PrefixMessage(User_lang.USER_RESET_DEFAULT_PASSWORD_FAIL), false)
                                 .Async();
            }
        }

        /// <summary>
        /// 檢查是否重複使用者姓名
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(User_lang.USER_CHECK_NAME))]
        [ModelValidator(false)]
        public async Task<IHttpResult> CheckName(string ID, [Required]string name)
        {
            try
            {
                var con = new MSSQLCondition<USER>();
                con.And(x => x.NAME == name);

                if (!string.IsNullOrEmpty(ID))
                    con.And(x => x.USER_ID != ID);

                var data = _OrganizationAggregate.User_T1_T2_.Get(con);


                if (data == null)
                {
                    return new JsonResult<bool>(false, "", true);
                }

                return new JsonResult<bool>(true, data.Name, true);
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
        /// <summary>
        /// 匯出Excel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Report)]
        [Logger(nameof(User_lang.USER_GETLIST))]
        [ModelValidator(false)]
        public async Task<HttpResponseMessage> GetReport(UserSearchViewModel model)
        {
            try
            {

                var con = new MSSQLCondition<USER>(model);

                con.IncludeBy(x => x.ROLE);

                var condition = model.ToDomain();

                var list = _OrganizationAggregate.User_T1_T2_.GetList(con).ToList();

                var bytes = await _UserService.GetReport(list, condition);

                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new ByteArrayContent(bytes);

                response.Content.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");


                response.Content.Headers.ContentDisposition =
                   new ContentDispositionHeaderValue("attachment") { FileName = "使用者管理報表.xlsx".Encoding() };


                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.ms-excel");

                return response;
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                   ex.PrefixDevMessage(Report_lang.REPORT_CREATE_FAIL));

                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}
