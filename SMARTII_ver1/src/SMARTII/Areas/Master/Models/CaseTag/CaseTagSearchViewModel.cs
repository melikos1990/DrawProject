using System.Linq.Expressions;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Attribute;
using SMARTII.Database.SMARTII;

namespace SMARTII.Areas.Master.Models.CaseTag
{
    public class CaseTagSearchViewModel
    {
        public CaseTagSearchViewModel()
        {
        }

        /// <summary>
        /// 企業別
        /// </summary>
        [MSSQLFilter(nameof(CASE_TAG.NODE_ID),
        ExpressionType.Equal,
        PredicateType.And)]
        public int? BuID { get; set; }
    }
}
