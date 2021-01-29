using System.Linq.Expressions;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Attribute;
using SMARTII.Database.SMARTII;

namespace SMARTII.Areas.Master.Models.CaseTemplate
{
    public class CaseTemplateSearchViewModel
    {
        public CaseTemplateSearchViewModel()
        {
        }

        /// <summary>
        /// 內容
        /// </summary>
        [MSSQLFilter("x => x.CONTENT.Contains(Value) || x.TITLE.Contains(Value)")]
        public string Content { get; set; }

        /// <summary>
        /// 企業別
        /// </summary>
        [MSSQLFilter(nameof(CASE_TEMPLATE.NODE_ID),
        ExpressionType.Equal,
        PredicateType.And)]
        public int? BuID { get; set; }

        /// <summary>
        /// 類別
        /// </summary>
        [MSSQLFilter(nameof(CASE_TEMPLATE.CLASSIFIC_KEY),
        ExpressionType.Equal,
        PredicateType.And)]
        public string ClassificKey { get; set; }
    }
}
