using System.Linq.Expressions;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Attribute;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Common.Models.Organization
{
    public class NodeDefinitionSearchViewModel
    {
        public NodeDefinitionSearchViewModel()
        {
        }

        /// <summary>
        /// 組織型態
        /// </summary>
        [MSSQLFilter("ORGANIZATION_TYPE",
        ExpressionType.Equal,
        PredicateType.And)]
        public OrganizationType? OrganizationType { get; set; }

        /// <summary>
        /// 組織型態
        /// </summary>
        [MSSQLFilter("IDENTIFICATION_ID",
        ExpressionType.Equal,
        PredicateType.And)]
        public int? BUID { get; set; }

        /// <summary>
        /// 載入根定義 (BU/VENDER/CALLCENTER)
        /// </summary>
        public string IncludeDefKey { get; set; }
    }
}
