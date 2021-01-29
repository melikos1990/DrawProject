using System;
using System.Linq.Expressions;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Attribute;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Organization.Models.User
{
    public class UserSearchViewModel
    {
        public UserSearchViewModel()
        {
        }

        public UserSearchCondition ToDomain()
        {
            var result = new UserSearchCondition();
            result.IsAD = this.IsAD == null ? "" : this.IsAD == true ? "是" : "否";
            result.IsEnable = this.IsEnable == null ? "" : this.IsEnable == true ? "是" : "否";
            result.Name = this.Name;
            result.Account = this.Account;
            result.RoleNames = this.RoleNames == null ? "" : string.Join("/", this.RoleNames);
            result.IsSystemUser = this.IsSystemUser == null ? "" : this.IsSystemUser == true ? "是" : "否";
            return result;
        }

        /// <summary>
        /// 是否為內網登入人員
        /// </summary>
        [MSSQLFilter(nameof(USER.IS_AD),
        ExpressionType.Equal,
        PredicateType.And)]
        public Boolean? IsAD { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        [MSSQLFilter(nameof(USER.IS_ENABLED),
        ExpressionType.Equal,
        PredicateType.And)]
        public Boolean? IsEnable { get; set; }

        /// <summary>
        /// 使用者名稱
        /// </summary>
        [MSSQLFilter(nameof(USER.NAME),
        ExpressionType.Parameter,
        PredicateType.And)]
        public string Name { get; set; }

        /// <summary>
        /// 使用者帳號
        /// </summary>
        [MSSQLFilter(nameof(USER.ACCOUNT),
        ExpressionType.Parameter,
        PredicateType.And)]
        public string Account { get; set; }

        /// <summary>
        /// 角色代號清單
        /// </summary>
        [MSSQLFilter("x => x.ROLE.Any(g => Value.Contains(g.ID))")]
        public int[] RoleIDs { get; set; }

        /// <summary>
        /// 角色名稱清單
        /// </summary>
        public string[] RoleNames { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        [MSSQLFilter(nameof(USER.IS_SYSTEM_USER),
        ExpressionType.Equal,
        PredicateType.And)]
        public Boolean? IsSystemUser { get; set; }
    }
}
