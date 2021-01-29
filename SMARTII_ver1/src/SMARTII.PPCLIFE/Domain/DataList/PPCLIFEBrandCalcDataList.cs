using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMARTII.Domain.Case;

namespace SMARTII.PPCLIFE.Domain.DataList
{
    public class PPCLIFEBrandCalcDataList
    {
        /// <summary>
        /// 品牌商品與問題歸類-數據統計報表 (彙整)
        /// </summary>
        public List<BPSCalc> BrandSummaryCalc { get;set;}
        /// <summary>
        /// 品牌商品與問題歸類-數據統計報表 (明細)
        /// </summary>
        public List<BPSDetail> BrandSummaryDetail { get; set; }
        /// <summary>
        /// 回覆顧客方式、問題要因
        /// </summary>
        public List<CaseFinishReasonData> FieldList { get; set; }
        /// <summary>
        /// 回覆顧客方式ID
        /// </summary>
        public int ReplyID { get; set; }

        /// <summary>
        /// 問題要因ID
        /// </summary>
        public int FactorsID { get; set; }
    }
}
