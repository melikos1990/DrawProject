using System.ComponentModel.DataAnnotations;
using SMARTII.Domain.Data;

namespace SMARTII.Areas.Organization.Models.Enterprise
{
    public class EnterpriseDetailViewModel
    {
        public EnterpriseDetailViewModel()
        {
        }

        public EnterpriseDetailViewModel(Domain.Organization.Enterprise enterprise)
        {
            this.EnterpriseID = enterprise.ID;
            this.Name = enterprise.Name;
            this.CreateDateTime = enterprise.CreateDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.CreateUserName = enterprise.CreateUserName;
            this.UpdateDateTime = enterprise.UpdateDateTime.DisplayWhenNull(text: "無");
            this.UpdateUserName = enterprise.UpdateUserName;
            this.IsEnabled = enterprise.IsEnabled;
        }

        /// <summary>
        /// 定義代號
        /// </summary>
        public int EnterpriseID { get; set; }

        /// <summary>
        /// 關鍵值
        /// </summary>
        [Required]
        [MaxLength(50, ErrorMessage = "關鍵值 欄位長度需要小於50")]
        public string Name { get; set; }

        /// <summary>
        /// 建立者
        /// </summary>
        public string CreateDateTime { get; set; }

        /// <summary>
        /// 建立者
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public string UpdateDateTime { get; set; }

        /// <summary>
        /// 更新者
        /// </summary>
        public string UpdateUserName { get; set; }
        /// <summary>
        /// 啟用/停用
        /// </summary>
        public bool IsEnabled { get; set; }
    }
}
