using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.ModelBinding;
using SMARTII.Assist.Authentication;
using SMARTII.Assist.Logger;
using SMARTII.Assist.Web;
using SMARTII.Domain.Authentication;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Authentication.Service;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.IO;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Models.Account;
using SMARTII.Resource.Tag;
using System.Drawing;
using System.Text;
using SMARTII.Domain.Security;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Web;
using System.Linq;
using System.Web.SessionState;
using System.Net.Http.Formatting;

namespace SMARTII.Controllers
{
    [AllowAnonymous]
    public class AccountController : BaseApiController
    {
        private readonly IVoiceFacade _VoiceFacade;
        private readonly IUserService _UserService;
        private readonly IUserFacade _UserFacade;
        private readonly ITokenManager _TokenManager;
        private readonly ICommonAggregate _CommonAggregate;
        private readonly IUserAuthenticationManager _UserAuthenticationManager;

        public AccountController(IVoiceFacade VoiceFacade,
                                 IUserService UserService,
                                 IUserFacade UserFacade,
                                 ITokenManager TokenManager,
                                 ICommonAggregate CommonAggAggregateService,
                                 IUserAuthenticationManager UserAuthenticationManager)
        {
            _UserFacade = UserFacade;
            _UserService = UserService;
            _VoiceFacade = VoiceFacade;
            _TokenManager = TokenManager;
            _CommonAggregate = CommonAggAggregateService;
            _UserAuthenticationManager = UserAuthenticationManager;
        }
        /// <summary>
        /// 使用者登入
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ActionName("Login")]
        [Logger(nameof(Account_lang.ACCOUNT_LOGIN))]
        [ModelValidator(false)]
        [HttpPost]
        public async Task<IHttpResult> LoginAsync(AccountLoginViewModel model)
        {
            try
            {
                User user = await _UserAuthenticationManager.LoginAsync(model.Account, model.Password, model.Type);

                var tokenPair = _TokenManager.Create(user,
                                   accessExpire: DateTime.UtcNow.AddMinutes(30),
                                   refreshExpire: DateTime.UtcNow.AddDays(7));

                var result = new AccountUserViewModel(user, tokenPair);

                var wrapper = new JsonResult<AccountUserViewModel>(result, true);
                return await wrapper.Async();
            }
            catch (ResetPasswordException ex)
            {
                var wrapper = new JsonResult<AccountUserViewModel>(
                    new AccountUserViewModel(ex.Message), ex.InnerException.Message, false);

                return await wrapper.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                   ex.PrefixDevMessage(Account_lang.ACCOUNT_LOGIN_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Account_lang.ACCOUNT_LOGIN_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 刷新token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        [ActionName("RefreshToken")]
        [AuthenticationMethod(AuthenticationType.None)]
        [Logger(nameof(Account_lang.ACCOUNT_REFRESH_TOKEN))]
        [ModelValidator(false)]
        [HttpGet]
        public async Task<IHttpResult> RefreshTokenAsync([Required]string refreshToken)
        {
            try
            {
                var tokenUser = _TokenManager.Parse<User>(refreshToken);

                var user = await _UserFacade.GetUserAuthFromAccountAsync(tokenUser.Account);

                if (user == null)
                    throw new Exception(Common_lang.USER_UNDEFIND);

                var tokenPair = _TokenManager.Create(user.AsTokenIdentity(),
                                    accessExpire: DateTime.UtcNow.AddMinutes(30),
                                    refreshExpire: DateTime.UtcNow.AddDays(7));

                var result = new AccountUserViewModel(user, tokenPair);

                var wrapper = new JsonResult<AccountUserViewModel>(result, true);

                return await wrapper.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                   ex.PrefixDevMessage(Account_lang.ACCOUNT_REFRESH_TOKEN_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Account_lang.ACCOUNT_REFRESH_TOKEN_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 重設密碼
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ActionName("ResetPassword")]
        [Logger(nameof(User_lang.USER_RESET_PASSWORD))]
        [ModelValidator(false)]
        [HttpPost]
        public async Task<IHttpResult> ResetPasswordAsync(ChangePasswordViewModel model)
        {
            try
            {
                await this._UserAuthenticationManager.ResetPasswordAsync(model.Account, model.OldPassword, model.NewPassword);

                var wrapper = new JsonResult(User_lang.USER_RESET_PASSWORD_SUCCESS, true);

                return await wrapper.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                   ex.PrefixDevMessage(User_lang.USER_RESET_PASSWORD_FAIL));
                return await new JsonResult(
                        ex.PrefixMessage(User_lang.USER_RESET_PASSWORD_FAIL), false)
                        .Async();
            }
        }
        [HttpGet]
        public async Task<HttpResponseMessage> VerificationCode()
        {

            try
            {
                //Response.ContentType = "image/gif";

                //建立 Bitmap 物件和繪製

                Bitmap basemap = new Bitmap(70, 46);

                Graphics graph = Graphics.FromImage(basemap);

                graph.FillRectangle(new SolidBrush(Color.White), 0, 0, 70, 46);

                Font font = new Font(FontFamily.GenericSerif, 16, FontStyle.Bold, GraphicsUnit.Pixel);


                // 英數
                string letters = "0123456789"; // ABCDEFGHIJKLMNPQRSTUVWXYZ

                string letter;
                StringBuilder sb = new StringBuilder();

                // 加入隨機6個字
                // 英文4 ~ 5字，中文2 ~ 3字
                for (int word = 0; word < 5; word++)
                {
                    letter = letters.Substring(SecurityUtility.Next(0, letters.Length - 1), 1);
                    sb.Append(letter);
                    // 繪製字串
                    graph.DrawString(letter, font, new SolidBrush(Color.Black), word * 12, 0);
                }

                // 混亂背景

                Pen linePen = new Pen(new SolidBrush(Color.Black), 1);

                for (int x = 0; x < 3; x++)

                {

                    graph.DrawLine(linePen, new Point(SecurityUtility.Next(1, 69), SecurityUtility.Next(1, 45)), new Point(SecurityUtility.Next(1, 69), SecurityUtility.Next(1, 45)));

                }

                ArraySegment<byte> bmpBytes;
                using (var ms = new MemoryStream())
                {
                    basemap.Save(ms, ImageFormat.Gif);
                    ms.TryGetBuffer(out bmpBytes);
                    basemap.Dispose();
                    ms.Dispose();
                }

                var base64 = $"data:image/jpg;base64,{ Convert.ToBase64String(bmpBytes.ToArray())}";

                var resp = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ObjectContent<IHttpResult>(new JsonResult<KeyValuePair>()
                    {
                        isSuccess = true,
                        element = new KeyValuePair
                        {
                            Key = sb.ToString(),
                            Value = base64
                        }
                    },
                   new JsonMediaTypeFormatter())
                };


                resp.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpg");
                return resp;
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}
