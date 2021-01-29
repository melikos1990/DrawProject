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
    public class CaseAssignmentHSCondition
    {


        /// <summary>
        /// 企業別ID
        /// </summary>
        public Nullable<int> NodeID { get; set; }
        /// <summary>
        /// 企業別名稱
        /// </summary>
        public string NodeName { get; set; }
        /// <summary>
        /// 轉派對象
        /// </summary>
        [MSSQLFilter("y => Value !=null && y.CaseAssignmentUsersList!=null &&  y.CaseAssignmentUsersList.Any(c=> c.NodeID!=null && Value.Any(g => g.ID == (int)c.NodeID && g.OrganizationType == c.OrganizationType))"
            , PredicateType.And)]
        public List<CaseAssignmentUser> AssignmentUser { get; set; }
        /// <summary>
        /// 歷程模式
        /// </summary>
        public CaseAssignmentProcessType? AssignmentType { get; set; }
        /// <summary>
        /// 歷程狀態
        /// </summary>
        public int? Type { get; set; }
        /// <summary>
        /// 歷程狀態名稱
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 案件編號
        /// </summary>
        public string CaseID { get; set; }
        /// <summary>
        /// 立案時間
        /// </summary>
        public string CreateTimeRange { get; set; }
        /// <summary>
        /// 通知時間
        /// </summary>
        public string NoticeDateTimeRange { get; set; }
        /// <summary>
        /// 通知內容
        /// </summary>
        public string NoticeContent { get; set; }
        /// <summary>
        /// 案件內容
        /// </summary>
        public string CaseContent { get; set; }
        /// <summary>
        /// 反應單號
        /// </summary>
        [MSSQLFilter("y => Value !=null && y.InvoiceID.Contains(Value)"
        , PredicateType.And)]
        public string InvoiceID { get; set; }
        /// <summary>
        /// 反應者類型
        /// </summary>
        [MSSQLFilter("y => Value !=null && y.CaseConcatUsersList.Any(c=>c.UnitType ==Value)"
        , PredicateType.And)]
        public UnitType? CaseConcatUnitType { get; set; }
        /// <summary>
        /// 反應者姓名
        /// </summary>
        [MSSQLFilter("y => Value !=null && y.CaseConcatUsersList.Any(c=> c.UserName!= null && c.UserName.Contains(Value))"
        , PredicateType.And)]
        public string ConcatName { get; set; }
        /// <summary>
        /// 反應者電話
        /// </summary>
        [MSSQLFilter("y => Value !=null && (y.CaseConcatUsersList.Any(c=> c.Mobile!= null && c.Mobile.Contains(Value)) || y.CaseConcatUsersList.Any(c=> c.Telephone!= null && c.Telephone.Contains(Value)) || y.CaseConcatUsersList.Any(c=> c.TelephoneBak!= null && c.TelephoneBak.Contains(Value)))"
        , PredicateType.And)]
        public string ConcatTelephone { get; set; }
        /// <summary>
        /// 連絡者信箱
        /// </summary>
        [MSSQLFilter("y => Value !=null && y.CaseConcatUsersList.Any(c=> c.Email!= null &&  c.Email.Contains(Value))"
        , PredicateType.And)]
        public string ConcatEmail { get; set; }
        /// <summary>
        /// 連絡者店名
        /// </summary>
        [MSSQLFilter("y => Value !=null &&  y.CaseConcatUsersList.Any(c=>c.NodeName!= null && c.NodeName.Contains(Value))"
        , PredicateType.And)]
        public string ConcatStoreName { get; set; }
        /// <summary>
        /// 連絡者店號
        /// </summary>
        [MSSQLFilter("y => Value !=null && y.CaseConcatUsersList.Any(c=>c.StoreNo!= null && c.StoreNo.Contains(Value))"
        , PredicateType.And)]
        public string ConcatStoreNo { get; set; }
        /// <summary>
        /// 連絡者單位名稱
        /// </summary>
        [MSSQLFilter("y => Value !=null && y.CaseConcatUsersList.Any(c=>c.NodeName!= null && c.NodeName.Contains(Value))"
        , PredicateType.And)]
        public string ConcatNodeName { get; set; }
        /// <summary>
        /// 被反應者類型
        /// </summary>
        [MSSQLFilter("y => Value !=null && y.CaseComplainedUsersList.Any(c=>c.UnitType ==Value)"
        , PredicateType.And)]
        public UnitType? CaseComplainedUnitType { get; set; }
        /// <summary>
        /// 被反應者店名
        /// </summary> 
        [MSSQLFilter("y => Value !=null && y.CaseComplainedUsersList.Any(c=>c.NodeName!= null && c.NodeName.Contains(Value))"
        , PredicateType.And)]
        public string CaseComplainedStoreName { get; set; }
        /// <summary>
        /// 被反應者店號
        /// </summary>
        [MSSQLFilter("y => Value !=null && y.CaseComplainedUsersList.Any(c=>c.StoreNo!= null && c.StoreNo.Contains(Value))"
        , PredicateType.And)]
        public string CaseComplainedStoreNo { get; set; }
        /// <summary>
        /// 被反應者單位名稱
        /// </summary>
        [MSSQLFilter("y => Value !=null && y.CaseComplainedUsersList.Any(c=>c.NodeName!= null && c.NodeName.Contains(Value))"
        , PredicateType.And)]
        public string CaseComplainedNodeName { get; set; }
        /// <summary>
        /// 反應類別
        /// </summary>
        [MSSQLFilter("y => Value !=null && y.InvoiceType == (Value)"
        , PredicateType.And)]
        public string InvoiceType { get; set; }
        /// <summary>
        /// 分類代號
        /// </summary>
        public int? ClassificationID { get; set; }
        /// <summary>
        /// 該分類代號底下分類ID
        /// </summary>
        public string ClassificationIDGroup { get; set; }
        /// <summary>
        /// 連絡者 區域
        /// </summary>
        public List<OrganizationNodeBase> ConcatNode { get; set; }
        /// <summary>
        /// 連絡者 區域篩選
        /// </summary>
        [MSSQLFilter(" y => Value !=null && y.CaseConcatUsersList.Any(c=> Value.Contains(c.NodeID))"
        , PredicateType.And)]
        public List<int?> ConcatNodeRange { get; set; }
        /// <summary>
        /// 被反應者 區 
        /// </summary>
        public List<OrganizationNodeBase> ComplainedNode { get; set; }
        /// <summary>
        /// 被反應者 區域篩選
        /// </summary>
        [MSSQLFilter(" y => Value !=null && y.CaseComplainedUsersList.Any(c=> Value.Contains(c.NodeID))"
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
        public DateTime? NoticeDateStarTime
        {
            get
            {
                return String.IsNullOrEmpty(this.NoticeDateTimeRange) ?
                    default(DateTime?) :
                    Convert.ToDateTime(this.NoticeDateTimeRange.Split('-')[0].Trim());
            }
        }
        /// <summary>
        /// 期望期限結束時間
        /// </summary>
        public DateTime? NoticeDateEndTime
        {
            get
            {
                return String.IsNullOrEmpty(this.NoticeDateTimeRange) ?
                    default(DateTime?) :
                    Convert.ToDateTime(this.NoticeDateTimeRange.Split('-')[1].Trim());
            }
        }
        /// <summary>
        /// 是否是否顯示該BU所有案件(同客服查詢結果，不inner join被反應者Table-(BU查詢功能))
        /// </summary>
        public bool IsBusinessAll { get; set; }
    }
}
