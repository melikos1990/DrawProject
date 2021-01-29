using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Web.Http.Controllers;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Authentication.Service;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;
using SMARTII.Resource.Tag;

namespace SMARTII.Assist.Authentication
{
    public class TokenManager : ITokenManager
    {
        private static JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        private static byte[] securityKey = SecurityCache.Instance.TokenSecurityKey.GetBytes();

        private static TokenValidationParameters validationParameters = new TokenValidationParameters()
        {
            ValidAudience = "https://www.mywebsite.com",
            IssuerSigningKeys = new List<SecurityKey>()
        {
        (SecurityKey) new SymmetricSecurityKey(securityKey)
        },
            ValidAudiences = new List<string>()
        {
        "https://www.mywebsite.com"
        },
            ValidIssuer = "self"
        };

        private static DateTime defaultExpire = DateTime.UtcNow.AddMinutes(30);

        public T Parse<T>(string token) where T : class, new()
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                    throw new NullReferenceException(Common_lang.NO_AUTH_HEADER_TOKEN);

                SecurityToken validatedToken;

                Claim claim = tokenHandler.ValidateToken(token, validationParameters, out validatedToken)
                                          .Claims
                                          .FirstOrDefault<Claim>();
                if (claim == null)
                    throw new NullReferenceException(Common_lang.AUTH_TOKEN_INVALID);

                return JsonConvert.DeserializeObject<T>(claim.Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Create<T>(T identity, DateTime? expire = null) where T : class, new()
        {
            try
            {
                SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor();
                securityTokenDescriptor.Subject = new ClaimsIdentity(new Claim[1]
                {
                    new Claim("user", JsonConvert.SerializeObject((object) identity, Formatting.Indented, new JsonSerializerSettings()
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                }), "http://www.w3.org/2001/XMLSchema#string", "(local)")
                });
                securityTokenDescriptor.Issuer = "self";
                securityTokenDescriptor.Audience = "https://www.mywebsite.com";
                securityTokenDescriptor.Expires = new DateTime?(expire.HasValue ? expire.Value : defaultExpire);
                securityTokenDescriptor.SigningCredentials = new SigningCredentials((SecurityKey)new SymmetricSecurityKey(securityKey), "HS256");
                SecurityTokenDescriptor tokenDescriptor = securityTokenDescriptor;
                SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public TokenPair Create<T>(T identity, DateTime? accessExpire = null, DateTime? refreshExpire = null) where T : User
        {
            return new TokenPair()
            {
                AccessToken = Create(identity.AsTokenIdentity(),
                    accessExpire.HasValue ? accessExpire.Value : defaultExpire),
                RefreshToken = Create(identity.AsTokenIdentity(),
                    refreshExpire.HasValue ? refreshExpire.Value : defaultExpire),
            };
        }

        public string GetHeaderToken(HttpActionContext context)
        {
            var auth = context.Request.Headers.Authorization;

            if (auth == null)
                throw new Exception(Common_lang.NO_AUTH_HEADER);

            if (string.IsNullOrEmpty(auth.Scheme) || auth.Scheme != "Bearer")
                throw new Exception(Common_lang.NO_AUTH_HEADER_TOKEN);

            return auth.Parameter;
        }

        public User ParseTokenUser(HttpActionContext context)
        {
            var token = GetHeaderToken(context);

            return Parse<User>(token);
        }
    }
}