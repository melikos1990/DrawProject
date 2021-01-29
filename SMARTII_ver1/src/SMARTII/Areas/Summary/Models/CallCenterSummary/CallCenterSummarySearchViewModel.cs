using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Attribute;
using SMARTII.Database.SMARTII;

namespace SMARTII.Areas.Summary.Models.CallCenterSummary
{
    public class CallCenterSummarySearchViewModel
    {
        [MSSQLFilter(nameof(CASE.NODE_ID),
        ExpressionType.Equal,
        PredicateType.And)]
        public int BuID { get; set; }

        [MSSQLFilter(nameof(CASE.IS_ATTENSION),
        ExpressionType.Equal,
        PredicateType.And)]
        public bool? IsAttention { get; set; }


        public bool IsSelf { get; set; }
    }
}
