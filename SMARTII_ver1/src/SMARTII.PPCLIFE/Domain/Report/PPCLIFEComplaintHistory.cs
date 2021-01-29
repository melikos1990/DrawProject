using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMARTII.Domain.Case;
using SMARTII.Domain.Master;

namespace SMARTII.PPCLIFE.Domain
{
    /// <summary>
    /// 統一來電紀錄、代理品牌、自有品牌、醫學美容
    /// 客訴紀錄-一般客訴
    /// 客訴紀錄-一般客訴(上個月未結案)
    /// 客訴紀錄-重大客訴
    /// 客訴紀錄-重大客訴(上個月未結案)
    /// </summary>
    public class PPCLIFEComplaintHistory
    {
        /// <summary>
        /// 立案時間
        /// </summary>
        public DateTime CreateDateTime { get; set; }
        /// <summary>
        /// 來源
        /// </summary>
        public CaseSourceType SourceType { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// 時間
        /// </summary>
        public string Time { get; set; }
        /// <summary>
        /// 反應品牌
        /// </summary>
        public string ComplainedNodeName { get; set; }
        /// <summary>
        /// 商品名稱
        /// </summary>
        public string CommodityName { get; set; }
        /// <summary>
        /// 國際條碼
        /// </summary>
        public string InternationalBarcode { get; set; }
        /// <summary>
        /// 批號
        /// </summary>
        public string BatchNo { get; set; }
        /// <summary>
        /// 分類ID
        /// </summary>
        public int? ClassificationID { get; set; }
        /// <summary>
        /// 問題
        /// </summary>
        public string CaseContent { get; set; }
        /// <summary>
        /// 回覆
        /// </summary>
        public string FinishContent { get; set; }
        /// <summary>
        /// 處理人員
        /// </summary>
        public string ApplyUserName { get; set; }
        /// <summary>
        /// 轉派單位人員
        /// </summary>
        public string AssignmentUserName { get; set; }
        /// <summary>
        /// 案件編號
        /// </summary>
        public string CaseID { get; set; }
        /// <summary>
        /// 問題分類
        /// </summary>
        public QuestionClassification ClassList { get; set; }
    }
}
