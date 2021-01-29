using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Attribute;
using SMARTII.Database.SMARTII;

namespace SMARTII.Areas.Master.Models.OfficialEmailGroup
{
    public class OfficialEmailGroupSearchViewModel
    {
        /// <summary>
        /// 企業別代號
        /// </summary>
        [MSSQLFilter(nameof(OFFICIAL_EMAIL_GROUP.NODE_ID),
        ExpressionType.Equal,
        PredicateType.And)]
        public int? BuID { get; set; }
    }
}
