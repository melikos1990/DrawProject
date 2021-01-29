using System;
using System.Web;
using System.Web.Http.Controllers;
using SMARTII.Domain.Authentication.Object;

namespace SMARTII.Domain.Thread
{
    public static class ContextUtility
    {
        public static UserIdentity GetUserIdentity()
        {
            var context = HttpContext.Current;

            if (context == null) return null;

            if (context.User == null || context.User.Identity == null) return null;

            return context.User.Identity as UserIdentity;
        }

        public static Type GetControllerType(this HttpActionContext context)
        {
            return context.ControllerContext.Controller.GetType();
        }
    }
}