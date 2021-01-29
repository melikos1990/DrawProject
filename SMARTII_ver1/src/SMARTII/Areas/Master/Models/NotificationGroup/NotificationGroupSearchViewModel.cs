using System.Linq.Expressions;
using Ptc.Data.Condition2.Mssql.Attribute;
using SMARTII.Domain.Notification;

namespace SMARTII.Areas.Master.Models.NotificationGroup
{
    public class NotificationGroupSearchViewModel
    {
        public NotificationGroupSearchViewModel()
        {
        }

        /// <summary>
        /// 企業別代號
        /// </summary>
        [MSSQLFilter(nameof(Database.SMARTII.NOTIFICATION_GROUP.NODE_ID), ExpressionType.Equal)]
        public int? NodeID { get; set; }

        /// <summary>
        /// 示警類型
        /// </summary>
        [MSSQLFilter(nameof(Database.SMARTII.NOTIFICATION_GROUP.CALC_MODE), ExpressionType.Equal)]
        public NotificationCalcType? CalcMode { get; set; }
    }
}
