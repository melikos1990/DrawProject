using System.ComponentModel;
using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Case
{
    public class CaseAssignmentComplaintInvoiceUser : ConcatableUser
    {
        /// <summary>
        /// 識別編號
        /// </summary>
        [Description("識別編號")]
        public int ID { get; set; }

        /// <summary>
        /// 反應單代號
        /// </summary>
        [Description("反應單代號")]
        public int InvoiceIdentityID { get; set; }

        /// <summary>
        /// 反應單號
        /// </summary>
        [Description("反應單號")]
        public string InvoiceID { get; set; }

        /// <summary>
        /// 案件代號
        /// </summary>
        [Description("案件代號")]
        public string CaseID { get; set; }

        /// <summary>
        /// 所屬反應單
        /// </summary>
        public CaseAssignmentComplaintInvoice Invoice { get; set; }
    }
}
