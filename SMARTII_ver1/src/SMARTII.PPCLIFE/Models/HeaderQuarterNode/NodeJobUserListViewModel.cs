﻿using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;

namespace SMARTII.PPCLIFE.Models.NotificationSender
{
    public class NodeJobUserListViewModel
    {
        public NodeJobUserListViewModel()
        {
        }

        public NodeJobUserListViewModel(User user)
        {
            this.UserID = user.UserID;
            this.IsEnabled = user.IsEnabled.DisplayBit();
            this.IsAD = user.IsAD.DisplayBit();
            this.UserName = user.Name;
            this.Account = user.Account;
            this.CreateUserName = user.CreateUserName;
            this.CreateDateTime = user.CreateDateTime.ToString("yyyy/MM/dd HH:mm:ss");
        }

        /// <summary>
        /// 使用者代號
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        public string IsEnabled { get; set; }

        /// <summary>
        /// 是否為內部登入人員
        /// </summary>
        public string IsAD { get; set; }

        /// <summary>
        /// 使用者名稱
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 使用者帳號
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 建立人員
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public string CreateDateTime { get; set; }
    }
}
