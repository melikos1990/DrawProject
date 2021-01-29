using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMARTII.Domain.Data;
using SMARTII.Domain.Common;
using SMARTII.Domain.Security;
using Ptc.Data.Condition2.Mssql.Attribute;
using Ptc.Data.Condition2.Interface.Type;
using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Case
{
    public class SP_GetCaseList : DynamicSerializeObject
    {
        /// <summary>
        /// 總筆數
        /// </summary>
        public int Overall_Count { get; set; }
        /// <summary>
        /// 流水號
        /// </summary>
        public int RowNo { get; set; }
        /// <summary>
        /// 企業別代號
        /// </summary>
        public string NodeKey {get;set;}
        /// <summary>
        /// 企業名稱
        /// </summary>
        public string NodeName { get; set; }
        /// <summary>
        /// 案件來源
        /// </summary>
        public CaseSourceType? SourceType { get; set; }
        /// <summary>
        /// 案件編號
        /// </summary>
        public string CaseID { get; set; }
        /// <summary>
        /// 立案時間
        /// </summary>
        public DateTime CreateDateTime { get; set; }
        /// <summary>
        /// 來源時間
        /// </summary>
        public DateTime? IncomingDateTime { get; set; }
        /// <summary>
        /// 期望期限
        /// </summary>
        public DateTime? ExpectDateTime { get; set; }
        /// <summary>
        /// 負責人
        /// </summary>
        public string ApplyUserName { get; set; }
        /// <summary>
        /// 案件狀態
        /// </summary>
        public CaseType? CaseType { get; set; }
        /// <summary>
        /// 案件等級
        /// </summary>
        public string CaseWarningName { get; set; }
        /// <summary>
        /// 預立案
        /// </summary>
        public bool IsPrevention { get; set; }
        /// <summary>
        /// 關注案件
        /// </summary>
        public bool IsAttension { get; set; }
        /// <summary>
        /// 案件內容
        /// </summary>
        public string CaseContent { get; set; }
        /// <summary>
        /// 應完成時間 案件期限
        /// </summary>
        public DateTime? PromiseDateTime { get; set; }
        /// <summary>
        /// 結案內容
        /// </summary>
        public string FinishContent { get; set; }
        /// <summary>
        /// 結案時間
        /// </summary>
        public DateTime? FinishDateTime { get; set; }
        /// <summary>
        /// 分類代號
        /// </summary>
        public int? ClassificationID { get; set; }
        /// <summary>
        /// 分類父層
        /// </summary>
        public string ClassificationParentName { get; set; }
        /// <summary>
        /// 連絡者
        /// </summary>
        public List<CaseConcatUser> CaseConcatUsersList { get; set; }
        /// <summary>
        /// 被反應者
        /// </summary>
        public List<CaseComplainedUser> CaseComplainedUsersList { get; set; }
        /// <summary>
        /// 案件標籤
        /// </summary>
        public List<CaseTag> CaseTagList { get; set; }
        /// <summary>
        /// 結案原因處置
        /// </summary>
        public List<CaseFinishReasonData> CaseFinishReasonDatas { get; set; }
        /// <summary>
        /// 反應單
        /// </summary>
        public List<CaseAssignmentComplaintInvoice> ComplaintInvoice { get; set; }
        /// <summary>
        /// 其他資訊
        /// </summary>
        public List<CaseItem> CaseItemList { get; set; }
    }
}
