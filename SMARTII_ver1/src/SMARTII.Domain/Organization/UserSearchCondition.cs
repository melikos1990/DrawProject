using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMARTII.Domain.Organization
{
    public class UserSearchCondition
    {
        /// <summary>
        /// 是否為內網登入人員
        /// </summary>
        public string IsAD { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        public string IsEnable { get; set; }

        /// <summary>
        /// 使用者名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 使用者帳號
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 角色代號清單
        /// </summary>
        public string RoleNames { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        public string IsSystemUser { get; set; }
    }
}
