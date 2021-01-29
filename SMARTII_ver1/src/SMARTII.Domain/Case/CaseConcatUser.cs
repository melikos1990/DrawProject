using System.ComponentModel;
using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Case
{
    /// <summary>
    /// 通知對象
    /// </summary>
    public class CaseConcatUser : ConcatableUser
    {
        /// <summary>
        /// 流水號
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 案件編號
        /// </summary>
        [Description("案件編號")]
        public string CaseID { get; set; }

        /// <summary>
        /// 所屬案件
        /// </summary>
        public Case Case { get; set; }
    }
}
