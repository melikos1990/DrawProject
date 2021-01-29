using System;
using System.Linq.Expressions;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Attribute;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Authentication.Object;

namespace SMARTII.Areas.System.Models.SystemLog
{
    public class SystemLogSearchViewModel
    {
        public SystemLogSearchViewModel()
        {
        }

        /// <summary>
        /// 機能標籤
        /// </summary>
        [MSSQLFilter(nameof(SYSTEM_LOG.FEATURE_TAG),
        ExpressionType.Equal,
        PredicateType.And)]
        public string FeatureTag { get; set; }

        /// <summary>
        /// 功能權限
        /// </summary>
        public AuthenticationType? Operator { get; set; }

        /// <summary>
        /// 建立人員帳號
        /// </summary>
        [MSSQLFilter(nameof(SYSTEM_LOG.CREATE_USER_ACCOUNT),
        ExpressionType.Parameter,
        PredicateType.And)]
        public string Account { get; set; }

        /// <summary>
        /// 立案時間起迄
        /// </summary>
        public string CreateDateTimeRange { get; set; }

        /// <summary>
        /// 立案時間起
        /// </summary>
        [MSSQLFilter(nameof(SYSTEM_LOG.CREATE_DATETIME),
        ExpressionType.GreaterThanOrEqual,
        PredicateType.And)]
        public DateTime? Start
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

        /// <summary>
        /// 立案時間迄
        /// </summary>
        [MSSQLFilter(nameof(SYSTEM_LOG.CREATE_DATETIME),
        ExpressionType.LessThan,
        PredicateType.And)]
        public DateTime? End
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
