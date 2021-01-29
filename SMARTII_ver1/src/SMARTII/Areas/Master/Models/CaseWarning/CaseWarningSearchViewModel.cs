using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Attribute;
using SMARTII.Database.SMARTII;

namespace SMARTII.Areas.Master.Models.CaseWarning
{
    public class CaseWarningSearchViewModel
    {
        /// <summary>
        /// 企業別代號
        /// </summary>
        [MSSQLFilter(nameof(CASE_WARNING.NODE_ID),
        ExpressionType.Equal,
        PredicateType.And)]
        public int? NodeID { get; set; }
    }
}
