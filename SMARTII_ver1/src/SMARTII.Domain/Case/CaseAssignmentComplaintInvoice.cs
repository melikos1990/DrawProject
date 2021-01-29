using System.Collections.Generic;
using System.ComponentModel;

namespace SMARTII.Domain.Case
{
    public class CaseAssignmentComplaintInvoice : CaseAssignmentBase
    {
        public CaseAssignmentComplaintInvoice()
        {
        }

        /// <summary>
        /// 系統流水號
        /// </summary>
        [Description("系統流水號")]
        public int ID { get; set; }

        /// <summary>
        /// 反應單號
        /// </summary>
        [Description("反應單號")]
        public string InvoiceID { get; set; }

        /// <summary>
        /// 反應單型態 (行銷/...)
        /// </summary>
        [Description("反應單型態")]
        public string InvoiceType { get; set; }

        /// <summary>
        /// 反應單狀態
        /// </summary>
        [Description("反應單狀態")]
        public CaseAssignmentComplaintInvoiceType CaseAssignmentComplaintInvoiceType { get; set; }

        /// <summary>
        /// 是否需回電
        /// </summary>
        [Description("是否需回電")]
        public bool IsRecall { get; set; }

        /// <summary>
        /// 底下的人員
        /// </summary>
        [Description("底下的人員")]
        public List<CaseAssignmentComplaintInvoiceUser> Users { get; set; }

        #region override

        public override CaseAssignmentProcessType CaseAssignmentProcessType { get; set; } =
                        CaseAssignmentProcessType.Invoice;

        #endregion override
    }
}
