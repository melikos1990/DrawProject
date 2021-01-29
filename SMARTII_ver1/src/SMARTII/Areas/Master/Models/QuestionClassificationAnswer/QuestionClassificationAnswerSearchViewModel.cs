using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Attribute;

namespace SMARTII.Areas.Master.Models.QuestionClassificationAnswer
{
    public class QuestionClassificationAnswerSearchViewModel
    {

        /// <summary>
        /// 企業別代號
        /// </summary>
        [MSSQLFilter(nameof(Database.SMARTII.VW_QUESTION_CLASSIFICATION_ANSWER_NESTED.NODE_ID),
        ExpressionType.Equal,
        PredicateType.And)]
        public int? NodeID { get; set; }

        /// <summary>
        /// 分類代號
        /// </summary>
        [MSSQLFilter(nameof(Database.SMARTII.VW_QUESTION_CLASSIFICATION_ANSWER_NESTED.CLASSIFICATION_ID),
        ExpressionType.Equal,
        PredicateType.And)]
        public int? ClassificationID { get; set; }

        /// <summary>
        /// 主旨/內容
        /// </summary>
        [MSSQLFilter("x => x.CONTENT.Contains(Value) || x.TITLE.Contains(Value)")]
        public string Keyword { get; set; }
    }
}
