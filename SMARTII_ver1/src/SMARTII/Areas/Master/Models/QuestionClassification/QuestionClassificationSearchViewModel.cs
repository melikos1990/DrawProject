using System.Linq.Expressions;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Attribute;
using SMARTII.Database.SMARTII;

namespace SMARTII.Areas.Master.Models.QuestionClassification
{
    public class QuestionClassificationSearchViewModel
    {
        [MSSQLFilter("NODE_ID",
        ExpressionType.Equal,
        PredicateType.And)]
        public int? BuID { get; set; }

        public byte OrganizationType { get; set; }

        [MSSQLFilter("ID",
        ExpressionType.Equal,
        PredicateType.And)]
        public int? QestionID { get; set; }

        [MSSQLFilter(nameof(VW_QUESTION_CLASSIFICATION_NESTED.PARENT_PATH),
        ExpressionType.Parameter,
        PredicateType.And)]
        public string ParnetIDPath { get; set; }
        [MSSQLFilter("IS_ENABLED",
        ExpressionType.Equal,
        PredicateType.And)]
        public bool? IsEnable { get; set; }
    }
}
