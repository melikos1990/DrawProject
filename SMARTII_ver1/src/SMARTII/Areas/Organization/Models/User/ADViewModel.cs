using System.ComponentModel.DataAnnotations;

namespace SMARTII.Areas.Organization.Models.User
{
    public class ADViewModel
    {
        public ADViewModel()
        {
        }

        /// <summary>
        /// 帳號
        /// </summary>
        [Required(ErrorMessage = "人員帳號 欄位為必要項目")]
        public string Account { get; set; }

        /// <summary>
        /// 密碼
        /// </summary>
        [Required(ErrorMessage = "人員密碼 欄位為必要項目")]
        public string Password { get; set; }
    }
}