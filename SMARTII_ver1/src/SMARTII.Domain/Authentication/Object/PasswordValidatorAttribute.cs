using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SMARTII.Domain.Authentication.Object
{
    public class PasswordValidatorAttribute : ValidationAttribute
    {
        private Type Type { get; set; }
        private string MethodName { get; set; }

        public PasswordValidatorAttribute(Type type, string methodName)
        {
            Type = type;
            MethodName = methodName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string password = (string)value;
            MethodInfo methodInfo = this.Type.GetMethod(this.MethodName);
            bool validated = (Boolean)methodInfo.Invoke(this.Type, new object[] { password });

            if (validated)
                return ValidationResult.Success;
            else
                return new ValidationResult(Resource.Tag.Common_lang.VALID_PASSWORD_STRENGTH_FAIL);
        }
    }
}