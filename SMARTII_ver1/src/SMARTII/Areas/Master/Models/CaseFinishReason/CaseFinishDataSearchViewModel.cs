using System.Linq.Expressions;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Attribute;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Master.Models.CaseFinishReason
{
    public class CaseFinishDataSearchViewModel
    {
        /// <summary>
        /// 分類代號
        /// </summary>
        [MSSQLFilter(nameof(CASE_FINISH_REASON_DATA.CLASSIFICATION_ID),
        ExpressionType.Equal,
        PredicateType.And)]
        public int? ClassificationID { get; set; }

        /// <summary>
        /// 企業別代號
        /// </summary>
        [MSSQLFilter("CASE_FINISH_REASON_CLASSIFICATION.NODE_ID",
        ExpressionType.Equal,
        PredicateType.And)]
        public int? NodeID { get; set; }

        /// <summary>
        /// 組織型態定義
        /// </summary>
        [MSSQLFilter("CASE_FINISH_REASON_CLASSIFICATION.ORGANIZATION_TYPE",
        ExpressionType.Equal,
        PredicateType.And)]
        public OrganizationType? OrganizationType { get; set; }

        /// <summary>
        /// 處置名稱
        /// </summary>
        [MSSQLFilter(nameof(CASE_FINISH_REASON_DATA.TEXT),
        ExpressionType.Parameter,
        PredicateType.And)]
        public string Text { get; set; }
    }
}
