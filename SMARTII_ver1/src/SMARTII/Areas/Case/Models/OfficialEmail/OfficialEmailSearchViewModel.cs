using System;
using System.Linq.Expressions;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Attribute;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;

namespace SMARTII.Areas.Case.Models.OfficialEmail
{
    public class OfficialEmailSearchViewModel
    {
        public OfficialEmailSearchViewModel()
        {
        }

        /// <summary>
        /// 企業別 寄件日期、寄件者、寄件MAIL、主旨、內容
        /// </summary>
        [MSSQLFilter(nameof(OFFICIAL_EMAIL_EFFECTIVE_DATA.NODE_ID),
        ExpressionType.Equal,
        PredicateType.And)]
        public int? BuID { get; set; }

        [MSSQLFilter(nameof(OFFICIAL_EMAIL_EFFECTIVE_DATA.CASE_ID),
        ExpressionType.Parameter,
        PredicateType.And)]
        public string CaseID { get; set; }

        [MSSQLFilter(nameof(OFFICIAL_EMAIL_EFFECTIVE_DATA.FROM_NAME),
        ExpressionType.Parameter,
        PredicateType.And)]
        public string FromName { get; set; }

        [MSSQLFilter(nameof(OFFICIAL_EMAIL_EFFECTIVE_DATA.FROM_ADDRESS),
        ExpressionType.Parameter,
        PredicateType.And)]
        public string FromAddress { get; set; }

        public string Subject { get; set; }

        public string DateRange { get; set; }

        public DateTime? StarDate {
            get { return this.DateRange.StarTime(); }
        }

        public DateTime? EndDate {
            get { return this.DateRange.EndTime(); }
        }

    }
}
