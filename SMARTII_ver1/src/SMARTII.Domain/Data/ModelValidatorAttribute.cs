using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using SMARTII.Resource.Tag;

namespace SMARTII.Domain.Data
{
    public class ModelValidatorAttribute : ActionFilterAttribute
    {
        private readonly bool _AllowNullable;

        public ModelValidatorAttribute(bool allowNullable = true)
        {
            _AllowNullable = allowNullable;
        }

        public override void OnActionExecuting(HttpActionContext context)
        {
            if (_AllowNullable == false)
            {
                if (context.ActionArguments == null)
                    context.Response = context.Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"{ Common_lang.MODEL_INVALID }");

                foreach (var item in context.ActionArguments.Values)
                {
                    if (item == null)
                    {
                        context.Response = context.Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"{ Common_lang.MODEL_INVALID }");
                        return;
                    }
                }
            }

            if (context.ModelState.IsValid == false)
            {
                context.Response = context.Request
                                          .CreateErrorResponse(
                                            HttpStatusCode.BadRequest,
                                            $"{ Common_lang.MODEL_INVALID }-{context.ModelState.GetErrorMessage("\n")}");
            }
        }
    }
}