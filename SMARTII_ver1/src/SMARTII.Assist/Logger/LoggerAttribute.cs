using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using SMARTII.Assist.Authentication;
using SMARTII.Assist.Culture;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.System;
using SMARTII.Domain.Thread;

namespace SMARTII.Assist.Logger
{
    public class LoggerAttribute : ActionFilterAttribute
    {
        public readonly string FeatureTag;

        public ICommonAggregate _ICommonAggregate { get; set; }

        public LoggerAttribute()
        {
        }

        public LoggerAttribute(string featureTag)
        {
            FeatureTag = featureTag;
        }

        public override void OnActionExecuting(HttpActionContext context)
        {
            var featureName = FeatureTag.GetSpecificLang(new CultureInfo("zh-TW", false));

            Type controllerType = context.GetControllerType();

            var actionName = (context.ActionDescriptor as ReflectedHttpActionDescriptor).MethodInfo?.Name;

            MethodInfo methodInfo = controllerType.GetMethod(actionName);

            var attributes = methodInfo?.GetCustomAttributes<AuthenticationMethodAttribute>().ToList();

            var attribute = attributes.FirstOrDefault();

            var apiController = (context.ControllerContext.Controller as ApiController);

            var userInfo = apiController.User.Identity as UserIdentity;

            var log = new SystemLog()
            {
                CreateDateTime = DateTime.Now,
                CreateUserName = userInfo == null ? null : userInfo.Name,
                CreateUserAccount = userInfo == null ? null : userInfo.Account,
                FeatureTag = FeatureTag,
                FeatureName = featureName,
                Content = context.ActionArguments.DisplayFromHttpParamter(),
                Operator = attribute == null ? AuthenticationType.None : attribute.AuthenticationType,
            };

            ((NLog.Logger)_ICommonAggregate
                .Loggers["Database"])
                .WithProperty("Data", new
                {
                    CreateDateTime = log.CreateDateTime,
                    CreateUserName = log.CreateUserName,
                    CreateUserAccount = log.CreateUserAccount,
                    FeatureTag = log.FeatureTag,
                    FeatureName = log.FeatureName,
                    Content = log.Content,
                    Operator = (int)log.Operator
                })

                .Error(string.Empty);

            base.OnActionExecuting(context);
        }
    }
}