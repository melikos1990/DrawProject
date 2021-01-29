using System.ComponentModel.DataAnnotations;
using SMARTII.Domain.Authentication.Object;

namespace SMARTII.Models.Account
{
    public class ChangePasswordViewModel
    {
        public ChangePasswordViewModel()
        {
        }

        [Required(ErrorMessage = "人員帳號 欄位為必要項目")]
        public string Account { get; set; }

        [Required(ErrorMessage = "人員原密碼 欄位為必要項目")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "人員新密碼 欄位為必要項目")]
        [PasswordValidator(
         typeof(PasswordUtility),
         nameof(PasswordUtility.Validate))]
        public string NewPassword { get; set; }
    }
}
