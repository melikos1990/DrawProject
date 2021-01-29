using System;
using System.Linq.Expressions;
using Ptc.Data.Condition2.Mssql.Attribute;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Master;

namespace SMARTII.Areas.Master.Models.Billboard
{
    public class BillboardSearchViewModel
    {
        public BillboardSearchViewModel()
        {
        }

        /// <summary>
        /// 主旨或內容
        /// </summary>
        [MSSQLFilter("x => x.CONTENT.Contains(Value) || x.TITLE.Contains(Value)")]
        public string Content { get; set; }

        /// <summary>
        /// 公告開始查詢時間區間
        /// </summary>
        public string FirstActivateDateTimeRange { get; set; }

        /// <summary>
        /// 緊急程度
        /// </summary>
        [MSSQLFilter(nameof(BILL_BOARD.WARNING_TYPE), ExpressionType.Equal)]
        public BillboardWarningType? BillboardWarningType { get; set; }

        /// <summary>
        ///  公告開始時間
        /// </summary>
        [MSSQLFilter(nameof(BILL_BOARD.ACTIVE_DATE_START), ExpressionType.GreaterThanOrEqual)]
        public DateTime? FirstActivateStartDateTime
        {
            get
            {
                return String.IsNullOrEmpty(this.FirstActivateDateTimeRange) ?
                    default(DateTime?) :
                    Convert.ToDateTime(this.FirstActivateDateTimeRange.Split('-')[0].Trim());
            }
        }

        /// <summary>
        /// 公告結束時間
        /// </summary>
        [MSSQLFilter(nameof(BILL_BOARD.ACTIVE_DATE_END), ExpressionType.LessThan)]
        public DateTime? FirstActivateEndDateTime
        {
            get
            {
                return String.IsNullOrEmpty(this.FirstActivateDateTimeRange) ?
                    default(DateTime?) :
                    Convert.ToDateTime(this.FirstActivateDateTimeRange.Split('-')[1].Trim());
            }
        }
    }
}
