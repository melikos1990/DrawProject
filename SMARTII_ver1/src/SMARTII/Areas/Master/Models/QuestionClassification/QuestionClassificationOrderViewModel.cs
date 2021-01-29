using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Attribute;

namespace SMARTII.Areas.Master.Models.QuestionClassification
{
    public class QuestionClassificationOrderViewModel
    {
        [MSSQLFilter("PARENT_ID",
        ExpressionType.Equal,
        PredicateType.And)]
        public int? ParentID { get; set; }

        [MSSQLFilter("NODE_ID",
        ExpressionType.Equal,
        PredicateType.And)]
        public int BuID { get; set; }


        public List<QuestionClassificationDetailViewModel> Questions { get; set; }
    }
}
