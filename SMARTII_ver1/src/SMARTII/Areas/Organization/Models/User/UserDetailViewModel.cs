using System;
using System.Collections.Generic;
using System.Linq;
using MultipartDataMediaFormatter.Infrastructure;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Data;

namespace SMARTII.Areas.Organization.Models.User
{
    public class UserDetailViewModel
    {
        public UserDetailViewModel()
        {
        }

        public UserDetailViewModel(Domain.Organization.User user)
        {
            this.UserID = user.UserID;
            this.Name = user.Name;
            this.Email = user.Email;
            this.Account = user.Account;
            this.CreateUserName = user.CreateUserName;
            this.CreateDateTime = user.CreateDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.UpdateDateTime = user.UpdateDateTime.DisplayWhenNull(text: "無");
            this.UpdateUserName = user.UpdateUserName;
            this.Telephone = user.Telephone;
            this.Mobile = user.Mobile;
            this.IsAD = user.IsAD;
            this.IsEnable = user.IsEnabled;
            this.ImagePath = user.UserParameter?.ImagePath;
            this.Feature = user.Feature;
            this.RoleIDs = user.Roles?.Select(x => x.ID).ToArray();
            this.Ext = user.Ext;
            this.IsSystemUser = user.IsSystemUser;
            this.EnableDateTime = StringUtility.ToDateRangePicker(user.ActiveStartDateTime, user.ActiveEndDateTime);
        }

        /// <summary>
        /// 使用者代號
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 是否為內部登入人員
        /// </summary>
        public Boolean IsAD { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        public Boolean IsEnable { get; set; }

        /// <summary>
        /// 使用者名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 使用者信箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 使用者帳號
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 使用者密碼
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 新增人人員
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 新增時間
        /// </summary>
        public string CreateDateTime { get; set; }

        /// <summary>
        /// 電話號碼
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// 手機號碼
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public string UpdateDateTime { get; set; }

        /// <summary>
        /// 更新人員
        /// </summary>
        public string UpdateUserName { get; set; }

        /// <summary>
        /// 檔案
        /// </summary>
        public HttpFile[] Picture { get; set; }

        /// <summary>
        /// 檔案路徑(preview)
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// 角色清單
        /// </summary>
        public int[] RoleIDs { get; set; }

        /// <summary>
        /// 功能權限
        /// </summary>
        public List<PageAuth> Feature { get; set; } = new List<PageAuth>();

        /// <summary>
        /// 系統使用者
        /// </summary>
        public bool IsSystemUser { get; set; }

        /// <summary>
        /// 分機號碼
        /// </summary>
        public string Ext { get; set; }

        /// <summary>
        /// 啟用時間
        /// </summary>
        public string EnableDateTime { get; set; }

        public DateTime? ActiveStartDateTime
        {
            get
            {
                return String.IsNullOrEmpty(this.EnableDateTime) ? default(DateTime?) : Convert.ToDateTime(this.EnableDateTime.Split('-')[0].Trim());
            }
        }

        public DateTime? ActiveEndDateTime
        {
            get
            {
                return String.IsNullOrEmpty(this.EnableDateTime) ? default(DateTime?) : Convert.ToDateTime(this.EnableDateTime.Split('-')[1].Trim());
            }
        }
    }
}
