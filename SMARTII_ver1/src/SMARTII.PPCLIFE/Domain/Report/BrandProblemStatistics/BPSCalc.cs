using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMARTII.Domain.Case;

namespace SMARTII.PPCLIFE.Domain
{
    /// <summary>
    /// 品牌商品與問題歸類-數據統計報表 (彙整)
    /// </summary>
    public class BPSCalc
    {
        /// <summary>
        /// 品牌ID
        /// </summary>
        public int? NodeID { get; set; }
        /// <summary>
        /// 品牌\問題\件數
        /// </summary>
        public string NodeName { get; set; }
        /// <summary>
        /// 總案件數
        /// </summary>
        public int TotalCaseCount { get; set; }
        /// <summary>
        ///  一般案件數
        /// </summary>
        public int GeneralCaseCount { get; set; }
        /// <summary>
        /// 開單件數
        /// </summary>
        public int Billed { get; set; }
        /// <summary>
        /// 客訴單
        /// </summary>
        public int ComplaintInvoiceCount { get; set; }

        /// <summary>
        /// 回覆顧客方式清單
        /// </summary>
        public List<CaseFinishReasonData> ReplyCustomerList { get; set; }

        /// <summary>
        /// 問題要因
        /// </summary>
        public List<CaseFinishReasonData> CauseProblemList { get; set; }   
    }
}
