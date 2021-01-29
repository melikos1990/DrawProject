using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SMARTII.Domain.Authentication;
using SMARTII.Domain.Authentication.Service;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Resource.Tag;

namespace SMARTII.Assist.Authentication
{
    public class AuthenticationAttribute : AuthorizeAttribute
    {
        public IPrincipalManager _PrincipalManager { get; set; }

        public ITokenManager _TokenManager { get; set; }

        public IUserAuthenticationValidator _UserAuthenticationValidator { get; set; }
        
        public ICommonAggregate _CommonAggregate { get; set; }

        public override void OnAuthorization(HttpActionContext context)
        {
            try
            {
                //_CommonAggregate.Logger.Info("[AuthenticationAttribute] 開始驗證資訊");


                //_CommonAggregate.Logger.Info("[AuthenticationAttribute] 開始解析使用者");

                // 取得token User
                var tokenUser = _TokenManager.ParseTokenUser(context);


                //_CommonAggregate.Logger.Info($"[AuthenticationAttribute] 認證使用者 User: {tokenUser.Name}");
                // 使用者認證
                _UserAuthenticationValidator.Valid(tokenUser);


                //_CommonAggregate.Logger.Info("[AuthenticationAttribute] 產生憑證資訊");
                // 產生憑證資訊
                var principal = _PrincipalManager.Generator(context, tokenUser);


                //_CommonAggregate.Logger.Info("[AuthenticationAttribute] 回填憑證");
                // 回填憑證
                System.Threading.Thread.CurrentPrincipal = principal;

                if (HttpContext.Current != null)
                    HttpContext.Current.User = principal;
            }
            catch (UserVersionException ex)
            {
                _CommonAggregate.Logger.Error("[UserVersionException]");
                _CommonAggregate.Logger.Error(ex);
                
                context.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    Content = new ObjectContent<IHttpResult>(new JsonResult()
                    {
                        isSuccess = false,
                        message = ex.Message
                    },
                    new JsonMediaTypeFormatter())
                };
                

                _CommonAggregate.Logger.Error("[UserVersionException] END");
                return;
            }
            catch (SecurityTokenExpiredException ex)
            {
                _CommonAggregate.Logger.Error("[SecurityTokenExpiredException]");
                _CommonAggregate.Logger.Error(ex);


                context.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    Content = new ObjectContent<IHttpResult>(new JsonResult()
                    {
                        isSuccess = false,
                        message = $"{Common_lang.AUTH_TOKEN_TIMOUT} , {ex.Message}"
                    },
                   new JsonMediaTypeFormatter())
                };

                _CommonAggregate.Logger.Error("[SecurityTokenExpiredException] END");
                return;
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error("[Exception]");
                _CommonAggregate.Logger.Error(ex);


                context.Response = new HttpResponseMessage(HttpStatusCode.Forbidden)
                {
                    Content = new ObjectContent<IHttpResult>(new JsonResult()
                    {
                        isSuccess = false,
                        message = ex.Message
                    },
                    new JsonMediaTypeFormatter())
                };

                _CommonAggregate.Logger.Error("[Exception] END");
                return;
            }
        }
    }
}
