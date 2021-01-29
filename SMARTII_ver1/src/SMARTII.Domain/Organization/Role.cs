using System;
using System.Collections.Generic;
using SMARTII.Domain.Authentication.Object;

namespace SMARTII.Domain.Organization
{
    public class Role
    {
        /// <summary>
        /// 操作權限代號
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 操作權限名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 更新人員
        /// </summary>
        public string UpdateUserName { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 建立人員
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime? UpdateDateTime { get; set; }

        /// <summary>
        /// 功能清單JSON
        /// </summary>
        public List<PageAuth> Feature { get; set; }

        /// <summary>
        /// 人員
        /// </summary>
        public List<User> Users { get; set; }
    }
}
