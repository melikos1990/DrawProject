using System.Linq.Expressions;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Attribute;
using SMARTII.Database.SMARTII;

namespace SMARTII.Areas.Select.Models.Organization
{
    public class HeaderQuarterNodeSearchViewModel
    {
        public HeaderQuarterNodeSearchViewModel()
        {
        }

        [MSSQLFilter(nameof(HEADQUARTERS_NODE.BU_ID),
        ExpressionType.Equal,
        PredicateType.And)]
        public int? BUID { get; set; }

        [MSSQLFilter(nameof(HEADQUARTERS_NODE.DEPTH_LEVEL),
        ExpressionType.Equal,
        PredicateType.And)]
        public int Level { get; set; }

        [MSSQLFilter(nameof(HEADQUARTERS_NODE.LEFT_BOUNDARY),
        ExpressionType.GreaterThanOrEqual,
        PredicateType.And)]
        public int? LeftBoundary { get; set; }

        [MSSQLFilter(nameof(HEADQUARTERS_NODE.RIGHT_BOUNDARY),
        ExpressionType.LessThanOrEqual,
        PredicateType.And)]
        public int? RightBoundary { get; set; }

        /// <summary>
        /// 是否為個人權限下
        /// </summary>
        public bool IsSelf { get; set; }
    }
}
