using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Attribute;
using SMARTII.Database.SMARTII;

namespace SMARTII.Areas.Master.Models.QuestionClassificationGuide
{
    public class QCGuideSearchViewModel
    {
        /// <summary>
        /// 問題分類ID
        /// </summary>
        [MSSQLFilter(nameof(VW_QUESTION_CLASSIFICATION_GUIDE_NESTED.CLASSIFICATION_ID),
        ExpressionType.Equal,
        PredicateType.And)]
        public int? ClassificationID { get; set; }
        /// <summary>
        /// 企業別代號
        /// </summary>
        [MSSQLFilter(nameof(VW_QUESTION_CLASSIFICATION_GUIDE_NESTED.NODE_ID),
        ExpressionType.Equal,
        PredicateType.And)]
        public int NodeID { get; set; }

    }
}
