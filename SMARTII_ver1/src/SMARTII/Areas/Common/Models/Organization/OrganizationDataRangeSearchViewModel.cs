using System.Linq.Expressions;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Attribute;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Common.Models.Organization
{
    public class OrganizationDataRangeSearchViewModel
    {
        public OrganizationDataRangeSearchViewModel()
        {
        }

        /// <summary>
        /// 是否為自身權限
        /// </summary>
        public bool IsSelf { get; set; }

        /// <summary>
        /// 指定特定節點
        /// </summary>
        public int? NodeID { get; set; }

        /// <summary>
        /// 查看角度
        /// </summary>
        public OrganizationType Goal { get; set; }

        /// <summary>
        /// 組織定義 KEY
        /// </summary>
        public string DefKey { get; set; }

        /// <summary>
        /// 是否找到底下節點
        /// </summary>
        public bool IsStretch { get; set; }

        /// <summary>
        /// 排除特定定義
        /// </summary>
        public string[] NotIncludeDefKey { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        [MSSQLFilter("IS_ENABLED",
        ExpressionType.Equal,
        PredicateType.And)]
        public bool? IsEnabled { get; set; }
    }
}
