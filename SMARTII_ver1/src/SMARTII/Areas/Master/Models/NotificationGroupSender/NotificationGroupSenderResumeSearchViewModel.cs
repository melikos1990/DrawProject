using System;
using System.Linq.Expressions;
using Ptc.Data.Condition2.Mssql.Attribute;

namespace SMARTII.Areas.Master.Models.NotificationGroupSender
{
    public class NotificationGroupSenderResumeSearchViewModel
    {
        public NotificationGroupSenderResumeSearchViewModel()
        {
        }

        [MSSQLFilter(nameof(Database.SMARTII.NOTIFICATION_GROUP_RESUME.NODE_ID),
        ExpressionType.Equal)]
        public int? NodeID { get; set; }

        [MSSQLFilter(nameof(Database.SMARTII.NOTIFICATION_GROUP_RESUME.GROUP_ID),
        ExpressionType.Equal)]
        public int? GroupID { get; set; }

        public string CreateTimeRange { get; set; }

        [MSSQLFilter(nameof(Database.SMARTII.NOTIFICATION_GROUP_RESUME.CREATE_DATETIME),
         ExpressionType.GreaterThanOrEqual)]
        public DateTime? Start
        {
            get
            {
                return String.IsNullOrEmpty(this.CreateTimeRange) ?
                    default(DateTime?) :
                    Convert.ToDateTime(this.CreateTimeRange.Split('-')[0].Trim());
            }
        }

        [MSSQLFilter(nameof(Database.SMARTII.NOTIFICATION_GROUP_RESUME.CREATE_DATETIME),
         ExpressionType.LessThan)]
        public DateTime? End
        {
            get
            {
                return String.IsNullOrEmpty(this.CreateTimeRange) ?
                    default(DateTime?) :
                    Convert.ToDateTime(this.CreateTimeRange.Split('-')[1].Trim());
            }
        }
    }
}
