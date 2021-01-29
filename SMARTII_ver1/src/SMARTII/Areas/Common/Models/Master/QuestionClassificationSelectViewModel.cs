using System.Linq.Expressions;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Attribute;

namespace SMARTII.Areas.Common.Models.Master
{
    public class QuestionClassificationSelectViewModel
    {
        /// <summary>
        /// 階層等級
        /// </summary>
        [MSSQLFilter("LEVEL",
        ExpressionType.Equal,
        PredicateType.And)]
        public int? Level { get; set; }

        /// <summary>
        /// BU代碼
        /// </summary>
        [MSSQLFilter("NODE_ID",
        ExpressionType.Equal,
        PredicateType.And)]
        public int BuID { get; set; }

        /// <summary>
        /// 過濾的問題分類
        /// </summary>
        [MSSQLFilter("ID",
        ExpressionType.NotEqual,
        PredicateType.And)]
        public int? FilterID { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        [MSSQLFilter("IS_ENABLED",
        ExpressionType.Equal)]
        public bool? IsEnabled { get; set; }
    }
}
