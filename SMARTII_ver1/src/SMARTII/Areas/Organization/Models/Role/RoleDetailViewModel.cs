using System;
using System.Collections.Generic;
using System.Linq;
using SMARTII.Areas.Organization.Models.User;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Data;

namespace SMARTII.Areas.Organization.Models.Role
{
    public class RoleDetailViewModel
    {
        public RoleDetailViewModel()
        {
        }

        public RoleDetailViewModel(Domain.Organization.Role role)
        {
            this.RoleID = role.ID;
            this.Name = role.Name;
            this.IsEnabled = role.IsEnabled;
            this.Feature = role.Feature;
            this.CreateUserName = role.CreateUserName;
            this.CreateDateTime = role.CreateDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.UpdateDateTime = role.UpdateDateTime.DisplayWhenNull(text: "無");
            this.UpdateUserName = role.UpdateUserName;
            this.Users = role.Users?
                             .Select(x => new UserListViewModel(x))?
                             .ToList();
        }

        /// <summary>
        /// 權限代號
        /// </summary>
        public int? RoleID { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 新增人人員
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 新增時間
        /// </summary>
        public string CreateDateTime { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public string UpdateDateTime { get; set; }

        /// <summary>
        /// 更新人員
        /// </summary>
        public string UpdateUserName { get; set; }

        /// <summary>
        /// 功能權限
        /// </summary>
        public List<PageAuth> Feature { get; set; } = new List<PageAuth>();

        /// <summary>
        /// 底下的使用者
        /// </summary>
        public List<UserListViewModel> Users { get; set; } = new List<UserListViewModel>();
    }
}
