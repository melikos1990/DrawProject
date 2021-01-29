using System;
using System.Linq.Expressions;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Attribute;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Case;

namespace SMARTII.Areas.Master.Models.CaseRemind
{
    public class CaseRemindSearchViewModel
    {
        /// <summary>
        /// 案件編號
        /// </summary>
        [MSSQLFilter("x => x.CASE_ID.Contains(Value)",
        PredicateType.And)]
        public string CaseID { get; set; }

        /// <summary>
        /// 轉派編號
        /// </summary>
        [MSSQLFilter(nameof(CASE_REMIND.ASSIGNMENT_ID),
        ExpressionType.Equal,
        PredicateType.And)]
        public int? CaseAssignmentID { get; set; }

        /// <summary>
        /// 通知狀態
        /// </summary>
        [MSSQLFilter(nameof(CASE_REMIND.IS_CONFIRM),
        ExpressionType.Equal,
        PredicateType.And)]
        public bool? Isconfirm { get; set; }

        /// <summary>
        /// 生效時間
        /// </summary>
        public string ActiveDateTimeRange { get; set; }

        /// <summary>
        /// 開始生效日
        /// </summary>
        [MSSQLFilter("x => x.ACTIVE_END_DAETTIME >= (Value)",
        PredicateType.And)]
        public DateTime? ActiveStartDateTime
        {
            get
            {
                return String.IsNullOrEmpty(this.ActiveDateTimeRange) ?
                    default(DateTime?) :
                    Convert.ToDateTime(this.ActiveDateTimeRange.Split('-')[0].Trim());
            }
        }

        /// <summary>
        /// 結束生效日
        /// </summary>
        [MSSQLFilter("x => x.ACTIVE_START_DAETTIME <= (Value)",
        PredicateType.And)]
        public DateTime? ActiveEndDateTime
        {
            get
            {
                return String.IsNullOrEmpty(this.ActiveDateTimeRange) ?
                    default(DateTime?) :
                    Convert.ToDateTime(this.ActiveDateTimeRange.Split('-')[1].Trim());
            }
        }

        /// <summary>
        /// 被通知者
        /// </summary>
        [MSSQLFilter("x => x.USER_IDs.Contains(Value)",
        PredicateType.And)]
        public string UserIDs { get; set; }

        /// <summary>
        /// 通知等級
        /// </summary>
        [MSSQLFilter(nameof(CASE_REMIND.TYPE),
        ExpressionType.Equal,
        PredicateType.And)]
        public CaseRemindType? Level { get; set; }

        /// <summary>
        /// 企業別
        /// </summary>
        [MSSQLFilter(nameof(CASE_REMIND.NODE_ID),
        ExpressionType.Equal,
        PredicateType.And)]
        public int NodeID { get; set; }

        /// <summary>
        /// 通知內容
        /// </summary>
        [MSSQLFilter(nameof(CASE_REMIND.CONTENT),
        ExpressionType.Parameter,
        PredicateType.And)]
        public string Content { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public string CreateDateTimeRange { get; set; }

        /// <summary>
        /// 開始建立日
        /// </summary>
        [MSSQLFilter("x => x.CREATE_DATETIME >= (Value)",
        PredicateType.And)]
        public DateTime? CreateStartTime
        {
            get
            {
                return String.IsNullOrEmpty(this.CreateDateTimeRange) ?
                    default(DateTime?) :
                    Convert.ToDateTime(this.CreateDateTimeRange.Split('-')[0].Trim());
            }
        }

        /// <summary>
        /// 結束建立日
        /// </summary>
        [MSSQLFilter("x => x.CREATE_DATETIME <= (Value)",
        PredicateType.And)]
        public DateTime? CreateEndDateTime
        {
            get
            {
                return String.IsNullOrEmpty(this.CreateDateTimeRange) ?
                    default(DateTime?) :
                    Convert.ToDateTime(this.CreateDateTimeRange.Split('-')[1].Trim());
            }
        }

        /// <summary>
        /// 建立者
        /// </summary>
        [MSSQLFilter(nameof(CASE_REMIND.CREATE_USER_ID),
        ExpressionType.Equal,
        PredicateType.And)]
        public string CreateUserID { get; set; }
    }
}
