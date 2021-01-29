using System;
using System.Linq.Expressions;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Attribute;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Case;

namespace SMARTII.Areas.Substitute.Models.CaseNotice
{
    public class CaseNoticeSearchViewModel
    {
        public CaseNoticeSearchViewModel()
        {
        }

        /// <summary>
        /// 通知型態
        /// </summary>
        [MSSQLFilter(nameof(CASE_NOTICE.CASE_NOTICE_TYPE),
        ExpressionType.Equal,
        PredicateType.And)]
        public CaseNoticeType? CaseNoticeType { get; set; }
    }
}
