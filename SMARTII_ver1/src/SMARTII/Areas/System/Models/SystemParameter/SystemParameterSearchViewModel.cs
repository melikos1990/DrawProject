using System.Linq.Expressions;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Attribute;
using SMARTII.Database.SMARTII;

namespace SMARTII.Areas.System.Models.SystemParameter
{
    public class SystemParameterSearchViewModel
    {
        [MSSQLFilter(nameof(SYSTEM_PARAMETER.ID),
        ExpressionType.Parameter,
        PredicateType.And)]
        public string ID { get; set; }

        [MSSQLFilter(nameof(SYSTEM_PARAMETER.KEY),
        ExpressionType.Parameter,
        PredicateType.And)]
        public string Key { get; set; }

        [MSSQLFilter(nameof(SYSTEM_PARAMETER.TEXT),
        ExpressionType.Parameter,
        PredicateType.And)]
        public string Text { get; set; }

        [MSSQLFilter(nameof(SYSTEM_PARAMETER.VALUE),
        ExpressionType.Parameter,
        PredicateType.And)]
        public string Value { get; set; }
    }
}