using System.Linq.Expressions;
using Ptc.Data.Condition2.Mssql.Attribute;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Select.Models.Organization
{
    public class NodeUserSearchViewModel
    {
        [MSSQLFilter(nameof(Database.SMARTII.USER.NAME),
        ExpressionType.Parameter)]
        public string UserName { get; set; }

        /// <summary>
        /// 組織結點
        /// </summary>
        public int? NodeID { get; set; }

        /// <summary>
        /// 組織型態
        /// </summary>
        public OrganizationType OrganizationType { get; set; }

        /// <summary>
        /// 是否選擇啟用人員
        /// </summary>
        [MSSQLFilter(nameof(Database.SMARTII.USER.IS_ENABLED),
        ExpressionType.Equal)]
        public bool? IsUserEnabled { get; set; }
    }
}
