using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Attribute;
using SMARTII.Areas.Organization.Models;
using SMARTII.Domain.Case;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Case.Models
{
    public class CaseHSSearchViewModel
    {
        public CaseHSSearchViewModel()
        {
        }
        public CaseHSCondition ToDomain()
        {
            var result = new CaseHSCondition();
            result.NodeID = this.NodeID;
            result.NodeName = this.NodeName;
            result.CaseType = this.CaseType;
            result.CaseWarningID = this.CaseWarningID;
            result.CaseWarningName = this.CaseWarningName;
            result.CreateTimeRange = this.CreateTimeRange;
            result.CaseID = this.CaseID;
            result.CaseSourceType = this.CaseSourceType;
            result.CaseConcatUnitType = (UnitType?)this.CaseConcatUnitType;
            result.ConcatName = this.ConcatName;
            result.ConcatTelephone = this.ConcatTelephone;
            result.ConcatEmail = this.ConcatEmail;
            result.ConcatStoreName = this.ConcatStoreName;
            result.ConcatStoreNo = this.ConcatStoreNo;
            result.ConcatNodeName = this.ConcatNodeName;
            result.CaseComplainedUnitType = (UnitType?)this.CaseComplainedUnitType;
            result.CaseComplainedStoreName = this.CaseComplainedStoreName;
            result.CaseComplainedStoreNo = this.CaseComplainedStoreNo;
            result.CaseComplainedNodeName = this.CaseComplainedNodeName;
            result.CaseContent = this.CaseContent;
            result.FinishContent = this.FinishContent;
            result.InvoiceID = this.InvoiceID;
            result.CaseTagList = this.CaseTagList;
            result.CaseTagIDList = this.CaseTagIDList;
            result.ReasonID = this.ReasonIDs;
            result.ReasonName = this.ReasonNames;
            result.ClassificationID = this.ClassificationID;
            if (this.ConcatNode != null && this.ConcatNode.Any())
            {
                result.ConcatNode = this.ConcatNode.Select(x => new OrganizationNodeBase()
                {
                    Name = x.Name,
                    LeftBoundary = x.LeftBoundary,
                    RightBoundary = x.RightBoundary
                }).ToList();


            }
            if (this.ComplainedNode != null && this.ComplainedNode.Any())
            {
                result.ComplainedNode = this.ComplainedNode.Select(x => new OrganizationNodeBase()
                {
                    Name = x.Name,
                    LeftBoundary = x.LeftBoundary,
                    RightBoundary = x.RightBoundary
                }).ToList();
            }
            result.IsBusinessAll = this.IsBusinessAll;
            return result;
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
        /// 案件狀態
        /// </summary>
        public int? CaseType { get; set; }
        /// <summary>
        /// 案件等級
        /// </summary>
        public int? CaseWarningID { get; set; }
        /// <summary>
        /// 案件等級名稱
        /// </summary>
        public string CaseWarningName { get; set; }
        /// <summary>
        /// 立案時間
        /// </summary>
        public string CreateTimeRange { get; set; }
        /// <summary>
        /// 案件編號
        /// </summary>
        public string CaseID { get; set; }
        /// <summary>
        /// 案件來源
        /// </summary>
        public Nullable<int> CaseSourceType { get; set; }
        /// <summary>
        /// 結案時間
        /// </summary>
        public string FinishDateTimeRange { get; set; }
        /// <summary>
        /// 連絡者類型
        /// </summary>
        public int? CaseConcatUnitType { get; set; }
        /// <summary>
        /// 連絡者姓名
        /// </summary>
        public string ConcatName { get; set; }
        /// <summary>
        /// 連絡者電話
        /// </summary>
        public string ConcatTelephone { get; set; }
        /// <summary>
        /// 連絡者信箱
        /// </summary>
        public string ConcatEmail { get; set; }
        /// <summary>
        /// 連絡者店名
        /// </summary>
        public string ConcatStoreName { get; set; }
        /// <summary>
        /// 連絡者店號
        /// </summary>
        public string ConcatStoreNo { get; set; }
        /// <summary>
        /// 連絡者單位名稱
        /// </summary>
        public string ConcatNodeName { get; set; }
        /// <summary>
        /// 被反應者類型
        /// </summary>
        public int? CaseComplainedUnitType { get; set; }
        /// <summary>
        /// 被反應者店名
        /// </summary> 
        public string CaseComplainedStoreName { get; set; }
        /// <summary>
        /// 被反應者店號
        /// </summary>
        public string CaseComplainedStoreNo { get; set; }
        /// <summary>
        /// 被反應者單位名稱
        /// </summary>
        public string CaseComplainedNodeName { get; set; }
        /// <summary>
        /// 案件內容
        /// </summary>
        public string CaseContent { get; set; }
        /// <summary>
        /// 結案內容
        /// </summary>
        public string FinishContent { get; set; }
        /// <summary>
        /// 反應單號
        /// </summary>
        public string InvoiceID { get; set; }
        /// <summary>
        /// 分類代號
        /// </summary>
        public int? ClassificationID { get; set; }
        /// <summary>
        /// 標籤名稱
        /// </summary>
        public List<string> CaseTagList { get; set; }
        /// <summary>
        /// 標籤ID
        /// </summary>
        public List<int> CaseTagIDList { get; set; }
        /// <summary>
        /// 結案處置
        /// </summary>
        public List<int> ReasonIDs { get; set; }
        /// <summary>
        /// 結案處置名稱
        /// </summary>
        public List<string> ReasonNames { get; set; }
        /// <summary>
        /// 連絡者 區
        /// </summary>
        public List<OrganizationNodeViewModel> ConcatNode { get; set; }

        /// <summary>
        /// 被反應者 區
        /// </summary>
        public List<OrganizationNodeViewModel> ComplainedNode { get; set; }
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
        ///結案開始時間
        /// </summary>
        public DateTime? FinishDateStarTime
        {
            get
            {
                return String.IsNullOrEmpty(this.FinishDateTimeRange) ?
                    default(DateTime?) :
                    Convert.ToDateTime(this.FinishDateTimeRange.Split('-')[0].Trim());
            }
        }
        /// <summary>
        ///結案結束時間
        /// </summary>
        public DateTime? FinishDateEndTime
        {
            get
            {
                return String.IsNullOrEmpty(this.FinishDateTimeRange) ?
                    default(DateTime?) :
                    Convert.ToDateTime(this.FinishDateTimeRange.Split('-')[1].Trim());
            }
        }
        /// <summary>
        /// 是否是否顯示該BU所有案件(同客服查詢結果，不inner join被反應者Table(BU查詢功能))
        /// </summary>
        public bool IsBusinessAll { get; set; }
    }
}
