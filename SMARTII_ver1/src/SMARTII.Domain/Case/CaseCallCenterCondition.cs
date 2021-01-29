using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Attribute;
using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Case
{
    public class CaseCallCenterCondition
    {
        public CaseCallCenterCondition()
        {
        }
        /// <summary>
        /// 企業別ID
        /// </summary>
        public Nullable<int> NodeID { get; set; }
        /// <summary>
        /// 企業別名稱
        /// </summary>
        public string NodeName { get; set; }
        /// <summary>
        /// 案件來源
        /// </summary>
        public int? CaseSourceType { get; set; }
        /// <summary>
        /// 案件編號
        /// </summary>
        public string CaseID { get; set; }
        /// <summary>
        /// 立案時間
        /// </summary>
        public string CreateTimeRange { get; set; }
        /// <summary>
        /// 連絡者類型
        /// </summary>
        [MSSQLFilter("y => y.CaseConcatUsersList !=null && y.CaseConcatUsersList.Any(c=>c.UnitType ==Value)"
        , PredicateType.And)]
        public UnitType? CaseConcatUnitType { get; set; }
        /// <summary>
        /// 連絡者姓名
        /// </summary>
        [MSSQLFilter("y => y.CaseConcatUsersList !=null && y.CaseConcatUsersList.Any(c=> c.UserName!= null && c.UserName.Contains(Value))"
        , PredicateType.And)]
        public string ConcatName { get; set; }
        /// <summary>
        /// 連絡者電話
        /// </summary>
        [MSSQLFilter("y => y.CaseConcatUsersList !=null && (y.CaseConcatUsersList.Any(c=> c.Mobile!= null && c.Mobile.Contains(Value)) || y.CaseConcatUsersList.Any(c=> c.Telephone!= null && c.Telephone.Contains(Value)) || y.CaseConcatUsersList.Any(c=> c.TelephoneBak!= null && c.TelephoneBak.Contains(Value)))"
        , PredicateType.And)]
        public string ConcatTelephone { get; set; }
        /// <summary>
        /// 連絡者信箱
        /// </summary>
        [MSSQLFilter("y => y.CaseConcatUsersList !=null && y.CaseConcatUsersList.Any(c=> c.Email!= null &&  c.Email.Contains(Value))"
        , PredicateType.And)]
        public string ConcatEmail { get; set; }
        /// <summary>
        /// 連絡者店名
        /// </summary>
        [MSSQLFilter("y => y.CaseConcatUsersList !=null &&  y.CaseConcatUsersList.Any(c=>c.NodeName!= null && c.NodeName.Contains(Value))"
        , PredicateType.And)]
        public string ConcatStoreName { get; set; }
        /// <summary>
        /// 連絡者店號
        /// </summary>
        [MSSQLFilter("y => y.CaseConcatUsersList !=null && y.CaseConcatUsersList.Any(c=>c.StoreNo!= null && c.StoreNo.Contains(Value))"
        , PredicateType.And)]
        public string ConcatStoreNo { get; set; }
        /// <summary>
        /// 連絡者單位名稱
        /// </summary>
        [MSSQLFilter("y => y.CaseConcatUsersList !=null && y.CaseConcatUsersList.Any(c=>c.NodeName!= null && c.NodeName.Contains(Value))"
        , PredicateType.And)]
        public string ConcatNodeName { get; set; }
        /// <summary>
        /// 案件內容
        /// </summary>
        public string CaseContent { get; set; }
        /// <summary>
        /// 結案內容
        /// </summary>
        public string FinishContent { get; set; }
        /// <summary>
        /// 負責人名稱
        /// </summary>
        public string ApplyUserName { get; set; }
        /// <summary>
        /// 負責人ID
        /// </summary>
        public string ApplyUserID { get; set; }
        /// <summary>
        /// 反應單號
        /// </summary>
        [MSSQLFilter("y => y.ComplaintInvoice !=null && y.ComplaintInvoice.Any(c=>c.InvoiceID.Contains(Value))"
        , PredicateType.And)]
        public string InvoiceID { get; set; }
        /// <summary>
        /// 期望期限
        /// </summary>
        public string ExpectDateTimeRange { get; set; }
        /// <summary>
        /// 案件等級
        /// </summary>
        public int? CaseWarningID { get; set; }
        /// <summary>
        /// 案件等級名稱
        /// </summary>
        public string CaseWarningName { get; set; }
        /// <summary>
        /// 案件狀態
        /// </summary>
        public int? CaseType { get; set; }
        /// <summary>
        /// 被反應者類型
        /// </summary>
        [MSSQLFilter("y => y.CaseComplainedUsersList !=null && y.CaseComplainedUsersList.Any(c=>c.UnitType ==Value)"
        , PredicateType.And)]
        public UnitType? CaseComplainedUnitType { get; set; }
        /// <summary>
        /// 被反應者店名
        /// </summary> 
        [MSSQLFilter("y => y.CaseComplainedUsersList !=null && y.CaseComplainedUsersList.Any(c=>c.NodeName!= null && c.NodeName.Contains(Value))"
        , PredicateType.And)]
        public string CaseComplainedStoreName { get; set; }
        /// <summary>
        /// 被反應者店號
        /// </summary>
        [MSSQLFilter("y => y.CaseComplainedUsersList !=null && y.CaseComplainedUsersList.Any(c=>c.StoreNo!= null && c.StoreNo.Contains(Value))"
        , PredicateType.And)]
        public string CaseComplainedStoreNo { get; set; }
        /// <summary>
        /// 被反應者單位名稱
        /// </summary>
        [MSSQLFilter("y => y.CaseComplainedUsersList !=null && y.CaseComplainedUsersList.Any(c=>c.NodeName!= null && c.NodeName.Contains(Value))"
        , PredicateType.And)]
        public string CaseComplainedNodeName { get; set; }
        /// <summary>
        /// 分類代號
        /// </summary>
        public int? ClassificationID { get; set; }
        /// <summary>
        /// 該分類代號底下分類ID
        /// </summary>
        public string ClassificationIDGroup { get; set; }
        /// <summary>
        /// 標籤名稱
        /// </summary>
        public List<string> CaseTagList { get; set; }
        /// <summary>
        /// 標籤ID
        /// </summary>
        [MSSQLFilter("y => y.CaseTagList !=null &&  y.CaseTagList.Any(c=> Value.Contains(c.ID))"
        , PredicateType.And)]
        public List<int> CaseTagIDList { get; set; }
        /// <summary>
        /// 結案處置ID
        /// </summary>
        [MSSQLFilter(" y => y.CaseFinishReasonDatas !=null && y.CaseFinishReasonDatas.Any(c=> Value.Contains(c.ID))"
        , PredicateType.And)]
        public List<int> ReasonID { get; set; }
        /// <summary>
        /// 結案處置名稱
        /// </summary>
        public List<string> ReasonName { get; set; }
        /// <summary>
        /// 連絡者 區域
        /// </summary>
        public List<OrganizationNodeBase> ConcatNode { get; set; }
        /// <summary>
        /// 連絡者 區域篩選
        /// </summary>
        [MSSQLFilter(" y => y.CaseConcatUsersList !=null && y.CaseConcatUsersList.Any(c=> Value.Contains(c.NodeID))"
        , PredicateType.And)]
        public List<int?> ConcatNodeRange { get; set; }
        /// <summary>
        /// 被反應者 區 
        /// </summary>
        public List<OrganizationNodeBase> ComplainedNode { get; set; }
        /// <summary>
        /// 被反應者 區域篩選
        /// </summary>
        [MSSQLFilter(" y => y.CaseComplainedUsersList !=null && y.CaseComplainedUsersList.Any(c=> Value.Contains(c.NodeID))"
        , PredicateType.And)]
        public List<int?> ComplainedRange { get; set; }
        /// <summary>
        ///立案開始時間
        /// </summary>
        public DateTime? CreateStarTime
        {
            get
            {
                return String.IsNullOrEmpty(this.CreateTimeRange) ?
                    default(DateTime?) :
                    Convert.ToDateTime(this.CreateTimeRange.Split('-')[0].Trim());
            }
        }
        /// <summary>
        ///立案結束時間
        /// </summary>
        public DateTime? CreateEndTime
        {
            get
            {
                return String.IsNullOrEmpty(this.CreateTimeRange) ?
                    default(DateTime?) :
                    Convert.ToDateTime(this.CreateTimeRange.Split('-')[1].Trim());
            }
        }
        /// <summary>
        /// 期望期限開始時間
        /// </summary>
        public DateTime? ExpectDateStarTime
        {
            get
            {
                return String.IsNullOrEmpty(this.ExpectDateTimeRange) ?
                    default(DateTime?) :
                    Convert.ToDateTime(this.ExpectDateTimeRange.Split('-')[0].Trim());
            }
        }
        /// <summary>
        /// 期望期限結束時間
        /// </summary>
        public DateTime? ExpectDateEndTime
        {
            get
            {
                return String.IsNullOrEmpty(this.ExpectDateTimeRange) ?
                    default(DateTime?) :
                    Convert.ToDateTime(this.ExpectDateTimeRange.Split('-')[1].Trim());
            }
        }
    }
}
