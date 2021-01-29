using System;
using System.Linq.Expressions;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Attribute;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Organization.Models.OrganizarionNodeDefinition
{
    public class NodeDefinitionSearchViewModel
    {
        public NodeDefinitionSearchViewModel()
        {
        }

        /// <summary>
        /// 識別值
        /// ※ 可能會因為不同NotificationType 中的組織結點進行拆分
        /// </summary>
        [MSSQLFilter(nameof(ORGANIZATION_NODE_DEFINITION.IDENTIFICATION_ID),
        ExpressionType.Equal,
        PredicateType.And)]
        public int? Identification { get; set; }

        /// <summary>
        /// 識別值定義名稱
        /// ※ 可能會因為不同NotificationType 中的組織結點進行拆分
        /// </summary>
        [MSSQLFilter(nameof(ORGANIZATION_NODE_DEFINITION.IDENTIFICATION_NAME),
        ExpressionType.Equal,
        PredicateType.And)]
        public string IdentificationName { get; set; }

        /// <summary>
        /// 組織型態
        /// </summary>
        [MSSQLFilter(nameof(ORGANIZATION_NODE_DEFINITION.ORGANIZATION_TYPE),
        ExpressionType.Equal,
        PredicateType.And)]
        public OrganizationType? OrganizationType { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        [MSSQLFilter(nameof(ORGANIZATION_NODE_DEFINITION.NAME),
        ExpressionType.Parameter,
        PredicateType.And)]
        public string Name { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        [MSSQLFilter(nameof(ORGANIZATION_NODE_DEFINITION.IS_ENABLED),
        ExpressionType.Equal,
        PredicateType.And)]
        public bool? IsEnabled { get; set; }
    }
}
