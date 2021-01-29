using System;
using System.Linq.Expressions;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Attribute;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Authentication.Object;

namespace SMARTII.PPCLIFE.Models.NotificationSender
{
    public class NotificationSearchViewModel
    {
        /// <summary>
        /// 發送時間
        /// </summary>
        public string CreateTimeRange { get; set; }

        /// <summary>
        /// 發送時間起
        /// </summary>
        [MSSQLFilter(nameof(PPCLIFE_RESUME.CREATE_DATETIME),
        ExpressionType.GreaterThanOrEqual,
        PredicateType.And)]
        public DateTime? Start
        {
            get
            {
                if (string.IsNullOrEmpty(CreateTimeRange) == false)
                {
                    var str = CreateTimeRange.Split('-')[0]?.Trim();

                    return DateTime.Parse(str);
                }

                return null;
            }
        }

        /// <summary>
        /// 發送時間迄
        /// </summary>
        [MSSQLFilter(nameof(PPCLIFE_RESUME.CREATE_DATETIME),
        ExpressionType.LessThan,
        PredicateType.And)]
        public DateTime? End
        {
            get
            {
                if (string.IsNullOrEmpty(CreateTimeRange) == false)
                {
                    var str = CreateTimeRange.Split('-')[1]?.Trim();

                    return DateTime.Parse(str);
                }
                return null;
            }
        }
    }
}
