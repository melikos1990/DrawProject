using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Ptc.Data.Condition2.Mssql.Attribute;

namespace SMARTII.Areas.Master.Models.WorkSchedule
{
    public class WorkScheduleSearchViewModel
    {
        /// <summary>
        /// 企業代號
        /// </summary>
        [MSSQLFilter(nameof(Database.SMARTII.WORK_SCHEDULE.NODE_ID),
        ExpressionType.Equal)]
        public int BuID { get; set; }
        /// <summary>
        /// 年度
        /// </summary>
        public string YearTime { get; set; }
    }
}
