using System;
using System.Collections.Generic;
using System.Linq;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Master;

namespace SMARTII.Domain.Organization
{
    public class User 
    {
        /// <summary>
        /// 使用者代號
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 密碼
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 帳號
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 建立人
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 電子信箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 是否AD登入
        /// </summary>
        public Boolean IsAD { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        public Boolean IsEnabled { get; set; }

        /// <summary>
        /// 最後修改密碼時間
        /// </summary>
        public DateTime? LastChangePasswordDateTime { get; set; }

        /// <summary>
        /// 鎖定時間
        /// </summary>
        public DateTime? LockoutDateTime { get; set; }

        /// <summary>
        /// 過去修改過的密碼
        /// </summary>
        public PasswordQueue PastPasswordQueue { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 推播ID
        /// </summary>
        public string MobilePushID { get; set; }

        /// <summary>
        /// 手機號碼
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 連絡電話
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// 更新人員
        /// </summary>
        public string UpdateUserName { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime? UpdateDateTime { get; set; }

        /// <summary>
        /// 圖片實體檔案
        /// </summary>
        public Byte[] Picture { get; set; }

        /// <summary>
        /// 圖片路徑
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// 版本號
        /// </summary>
        public DateTime? Version { get; set; }

        /// <summary>
        /// 生效起始日期
        /// </summary>
        public DateTime? ActiveStartDateTime { get; set; }

        /// <summary>
        /// 生效結束日期
        /// </summary>
        public DateTime? ActiveEndDateTime { get; set; }

        /// <summary>
        /// 是否為系統人員
        /// </summary>
        public bool IsSystemUser { get; set; }

        /// <summary>
        /// 分機號碼
        /// </summary>
        public string Ext { get; set; }

        /// <summary>
        /// 所擁有的權限清單
        /// </summary>
        public List<Role> Roles { get; set; } = new List<Role>();

        /// <summary>
        /// 功能權限
        /// </summary>
        public List<PageAuth> Feature { get; set; } = new List<PageAuth>();

        /// <summary>
        /// 職稱定義
        /// </summary>
        public List<JobPosition> JobPositions { get; set; } = new List<JobPosition>();

        /// <summary>
        /// 個人化參數
        /// </summary>
        public UserParameter UserParameter { get; set; } = new UserParameter();

        #region calc

        /// <summary>
        /// 找到廠商/CC/總部服務之BU 節點
        /// 廠商  : 包含自身以及自身以下所服務之BU
        /// CC   : 包含自身以及自身以下所服務之BU
        /// 總部  : 目前無使用
        /// </summary>
        public IDictionary<OrganizationType, int[]> DownProviderBUDist = new Dictionary<OrganizationType, int[]>();

        #endregion calc

        #region extension

        public User AsTokenIdentity()
        {
            return new User()
            {
                Account = this.Account,
                Name = this.Name,
                Email = this.Email,
                ImagePath = this.UserParameter?.ImagePath ?? null,
                Version = this.Version,
                UserID = this.UserID,
                DownProviderBUDist = this.DownProviderBUDist
            };
        }

        #endregion extension


    }
}
