using System.Linq.Expressions;
using Ptc.Data.Condition2.Mssql.Attribute;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Master.Models.KMClassification
{
    public class KMSearchViewModel
    {
        public KMSearchViewModel(){ }

        /// <summary>
        /// 企業代號
        /// </summary>
        [MSSQLFilter("KM_CLASSIFICATION.NODE_ID",
        ExpressionType.Equal)]
        public int BuID { get; set; }
        /// <summary>
        /// 組織型態
        /// </summary>
        [MSSQLFilter("KM_CLASSIFICATION.ORGANIZATION_TYPE",
        ExpressionType.Equal)]
        public OrganizationType OrganizationType { get; set; }
        /// <summary>
        /// 問題分類代號
        /// </summary>
        [MSSQLFilter("KM_CLASSIFICATION.ID",
        ExpressionType.Equal)]
        public int? ClassificationID { get; set; }
        /// <summary>
        /// 關鍵字
        /// </summary>
        [MSSQLFilter(
        "x=> x.KM_CLASSIFICATION.NAME.Contains(Value) || " +
        "    x.TITLE.Contains(Value) || " +
        "    x.CONTENT.Contains(Value)")]
        public string Keyword { get; set; }
    }
}
