using System;
using System.ComponentModel.DataAnnotations;
using SMARTII.Domain.Data;

namespace SMARTII.Areas.System.Models.SystemParameter
{
    public class SystemParameterDetailViewModel
    {
        public SystemParameterDetailViewModel()
        {
        }

        public SystemParameterDetailViewModel(Domain.System.SystemParameter systemParameter)
        {
            this.ID = systemParameter.ID;
            this.Key = systemParameter.Key;
            this.Text = systemParameter.Text;
            this.Value = systemParameter.Value;
            this.NextValue = systemParameter.NextValue;
            this.ActiveDateTime = systemParameter.ActiveDateTime.DisplayWhenNull(text: "");
            this.UpdateDateTime = systemParameter.UpdateDateTime.DisplayWhenNull(text: "無");
            this.UpdateUserName = systemParameter.UpdateUserName;
            this.CreateDateTime = systemParameter.CreateDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.CreateUserName = systemParameter.CreateUserName;
        }

        /// <summary>
        /// 定義代號
        /// </summary>
        [Required]
        [MaxLength(50, ErrorMessage = "定義代號 欄位長度需要小於50")]
        public string ID { get; set; }

        /// <summary>
        /// 關鍵值
        /// </summary>
        [Required]
        [MaxLength(50, ErrorMessage = "關鍵值 欄位長度需要小於50")]
        public string Key { get; set; }

        /// <summary>
        /// 代表文字
        /// </summary>
        [Required]
        public string Text { get; set; }

        /// <summary>
        /// 實際資料
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        ///下次值 
        /// </summary>
        public string NextValue { get; set; }

        /// <summary>
        /// 生效日
        /// </summary>
        public string ActiveDateTime { get; set; }

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
    }
}
