using System.Collections.Generic;
using System.Linq;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;
using SMARTII.Service.Cache;

namespace SMARTII.Areas.Common.Models.Organization
{
    public class BusinesssUnitParameters
    {
        public BusinesssUnitParameters()
        {
        }

        public BusinesssUnitParameters(HeaderQuarterTerm term)
        {
            this.BuID = term.NodeID;
            this.NodeKey = term.NodeKey;
            this.CanFastFinished = DataStorage.CanFastFinishedCaseArray.Contains(term.NodeKey);
            this.CanFinishReturn = DataStorage.CaseCanFinishReturnArray.Contains(term.NodeKey);
            this.CanGetAssignmentUser = DataStorage.CaseCanGetAssignmentUser.Contains(term.NodeKey);

            if (DataStorage.CaseMutipleTicketTypeDict.TryGetValue(term.NodeKey ?? string.Empty, out var mutipleTicketType))
            {
                this.MutipleTicketType = mutipleTicketType;
            }
            if (DataStorage.CaseAssignmentComplaintInvoiceTypeDict.TryGetValue(term.NodeKey ?? string.Empty, out var CaseComplaintInvoiceTypes))
            {
                this.CaseComplaintInvoiceTypes = CaseComplaintInvoiceTypes;
            }
            if (DataStorage.CaseNearlyDaysDict.TryGetValue(term.NodeKey ?? string.Empty, out var days))
            {
                this.CaseNearlyDays = days ?? 0;
            }
        }


        /// <summary>
        /// 企業別三碼
        /// </summary>
        public string NodeKey { get; set; }

        /// <summary>
        /// 能否快速結案
        /// </summary>
        public bool CanFastFinished { get; set; }

        /// <summary>
        /// 能否使用結案回上頁 註記
        /// </summary>
        public bool CanFinishReturn { get; set; }

        /// <summary>
        /// 能否新增轉派時，將轉派對象節點人員帶入Mail清單 註記
        /// </summary>
        public bool CanGetAssignmentUser { get; set; }

        /// <summary>
        /// 是否能多開
        /// </summary>
        public CaseMutipleTicketType MutipleTicketType { get; set; }

        /// <summary>
        /// 反應單型態
        /// </summary>
        public List<SelectItem> CaseComplaintInvoiceTypes { get; set; }

        /// <summary>
        /// 最近的案件間隔天數
        /// </summary>
        public int? CaseNearlyDays { get; set; }

        /// <summary>
        /// 企業業別ID
        /// </summary>
        public int BuID { get; set; }

    }
}
