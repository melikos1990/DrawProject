using System.Linq.Expressions;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Attribute;
using SMARTII.Database.SMARTII;

namespace SMARTII.Areas.Organization.Models.Enterprise
{
    public class EnterpriseSearchViewModel
    {
        public EnterpriseSearchViewModel()
        {
        }

        /// <summary>
        /// 企業名稱
        /// </summary>
        [MSSQLFilter(nameof(ENTERPRISE.NAME),
        ExpressionType.Parameter,
        PredicateType.And)]
        public string Name { get; set; }
    }
}