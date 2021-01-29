using System;
using System.Linq.Expressions;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Attribute;

namespace SMARTII.Areas.Organization.Models.Role
{
    public class RoleSearchViewModel
    {
        public RoleSearchViewModel()
        {
        }

        /// <summary>
        /// 是否啟用
        /// </summary>
        [MSSQLFilter("IS_ENABLED",
        ExpressionType.Equal,
        PredicateType.And)]
        public Boolean? IsEnable { get; set; }

        /// <summary>
        /// 權限名稱
        /// </summary>
        [MSSQLFilter("NAME",
        ExpressionType.Parameter,
        PredicateType.And)]
        public string Name { get; set; }
    }
}
