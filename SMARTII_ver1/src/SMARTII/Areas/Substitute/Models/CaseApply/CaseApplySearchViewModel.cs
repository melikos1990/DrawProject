using System;
using System.Linq.Expressions;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Attribute;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Case;

namespace SMARTII.Areas.Substitute.Models.CaseApply
{
    public class CaseApplySearchViewModel
    {
        public CaseApplySearchViewModel()
        {
        }

        [MSSQLFilter(nameof(CASE.APPLY_USER_ID),
        ExpressionType.Equal,
        PredicateType.And)]
        public string ApplyUserID { get; set; }

        [MSSQLFilter(nameof(CASE.CASE_ID),
        ExpressionType.Parameter,
        PredicateType.And)]
        public string CaseID { get; set; }

        [MSSQLFilter(nameof(CASE.CASE_TYPE),
        ExpressionType.Equal,
        PredicateType.And)]
        public CaseType? CaseType { get; set; }

        [MSSQLFilter(nameof(CASE.CASE_WARNING_ID),
        ExpressionType.Equal,
        PredicateType.And)]
        public int? CaseWarningID { get; set; }

        [MSSQLFilter(nameof(CASE.NODE_ID),
        ExpressionType.Equal,
        PredicateType.And)]
        public int? NodeID { get; set; }

        /// <summary>
        /// 立案時間起迄
        /// </summary>
        public string CreateDateTimeRange { get; set; }

        [MSSQLFilter(nameof(CASE.CREATE_DATETIME),
        ExpressionType.GreaterThanOrEqual,
        PredicateType.And)]
        /// <summary>
        /// 立案時間起
        /// </summary>
        public DateTime? CreateDateTimeEnd
        {
            get
            {
                if (string.IsNullOrEmpty(CreateDateTimeRange) == false)
                {
                    var str = CreateDateTimeRange.Split('-')[0]?.Trim();

                    return DateTime.Parse(str);
                }

                return null;
            }
        }

        [MSSQLFilter(nameof(CASE.CREATE_DATETIME),
        ExpressionType.LessThan,
        PredicateType.And)]
        /// <summary>
        /// 立案時間迄
        /// </summary>
        public DateTime? CreateDateTimeStart
        {
            get
            {
                if (string.IsNullOrEmpty(CreateDateTimeRange) == false)
                {
                    var str = CreateDateTimeRange.Split('-')[1]?.Trim();

                    return DateTime.Parse(str);
                }
                return null;
            }
        }
    }
}
