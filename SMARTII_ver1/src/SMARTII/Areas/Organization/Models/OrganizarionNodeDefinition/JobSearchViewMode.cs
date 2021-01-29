using System.Linq.Expressions;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Attribute;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Organization.Models.OrganizarionNodeDefinition
{
    public class JobSearchViewModel
    {
        public JobSearchViewModel()
        {
        }

        /// <summary>
        /// 識別值
        /// 組織定義代號
        /// </summary>
        [MSSQLFilter("DEFINITION_ID",
        ExpressionType.Equal,
        PredicateType.And)]
        public int? DefinitionID { get; set; }

        /// <summary>
        /// 組織型態
        /// </summary>
        [MSSQLFilter("ORGANIZATION_TYPE",
        ExpressionType.Equal,
        PredicateType.And)]
        public OrganizationType? OrganizationType { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        [MSSQLFilter("NAME",
        ExpressionType.Parameter,
        PredicateType.And)]
        public string Name { get; set; }
    }
}