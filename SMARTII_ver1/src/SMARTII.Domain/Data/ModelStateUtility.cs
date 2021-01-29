using System.Linq;
using System.Web.Http.ModelBinding;

namespace SMARTII.Domain.Data
{
    public static class ModelStateUtility
    {
        public static string GetErrorMessage(this ModelStateDictionary data, string splitString = "\n")
        {
            string message = string.Join(splitString, data.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x =>
                                        {
                                            if (string.IsNullOrEmpty(x.ErrorMessage))
                                                return x.Exception.Message;

                                            return x.ErrorMessage;
                                        }));

            return message;
        }
    }
}