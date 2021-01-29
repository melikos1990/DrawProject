using System;
using System.Web.Http.Controllers;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Authentication.Service
{
    public interface ITokenManager
    {
        string GetHeaderToken(HttpActionContext context);

        string Create<T>(T identity, DateTime? Expire = null) where T : class, new();

        TokenPair Create<T>(T identity, DateTime? accessExpire = null, DateTime? refreshExpire = null) where T : User;

        T Parse<T>(string token) where T : class, new();

        User ParseTokenUser(HttpActionContext context);
    }
}