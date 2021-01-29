using System.ComponentModel.DataAnnotations;
using SMARTII.Domain.Authentication.Object;

namespace SMARTII.Models.Account
{
    public class AccountLoginViewModel
    {
        public AccountLoginViewModel()
        {
        }

        [Required(ErrorMessage = "人員帳號 欄位為必要項目")]
        [MaxLength(20, ErrorMessage = "人員帳號 欄位長度需要 ≦ 20")]
        public string Account { get; set; }

        [Required(ErrorMessage = "人員密碼 欄位為必要項目")]
        public string Password { get; set; }
    
        /// <summary>
        /// 內/外網 狀態
        /// </summary>
        public WebType Type { get; set; }

    }
}
