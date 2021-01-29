using System.Linq.Expressions;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Attribute;
using SMARTII.Database.SMARTII;

namespace SMARTII.Areas.Master.Models.CaseAssignGroup
{
    public class CaseAssignGroupSearchViewModel
    {
        public CaseAssignGroupSearchViewModel()
        {
        }

        /// <summary>
        /// 企業別
        /// </summary>
        [MSSQLFilter(nameof(CASE_ASSIGN_GROUP.NODE_ID),
        ExpressionType.Equal,
        PredicateType.And)]
        public int? BuID { get; set; }
    }
}