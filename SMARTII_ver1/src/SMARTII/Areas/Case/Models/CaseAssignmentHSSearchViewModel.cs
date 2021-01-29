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
    public class CaseAssignmentHSSearchViewModel
    {
        public CaseAssignmentHSSearchViewModel()
        {
        }
        public CaseAssignmentHSCondition ToDomain()
        {
            var result = new CaseAssignmentHSCondition();
            result.NodeID = this.NodeID;
            result.NodeName = this.NodeName;
            result.AssignmentUser = this.AssignmentUsers?.Select(x => x.ToDoamin()).ToList();
            result.AssignmentType = this.AssignmentType;
            result.Type = this.Type;
            result.TypeName = this.TypeName;
            result.CaseID = this.CaseID;
            result.CreateTimeRange = this.CreateTimeRange;
            result.NoticeDateTimeRange = this.NoticeDateTimeRange;
            result.NoticeContent = this.NoticeContent;
            result.CaseContent = this.CaseContent;
            result.InvoiceID = this.InvoiceID;
            result.CaseConcatUnitType = this.CaseConcatUnitType;
            result.ConcatName = this.ConcatName;
            result.ConcatTelephone = this.ConcatTelephone;
            result.ConcatEmail = this.ConcatEmail;
            result.ConcatStoreName = this.ConcatStoreName;
            result.ConcatStoreNo = this.ConcatStoreNo;
            result.ConcatNodeName = this.ConcatNodeName;
            result.CaseComplainedUnitType = this.CaseComplainedUnitType;
            result.CaseComplainedStoreName = this.CaseComplainedStoreName;
            result.CaseComplainedStoreNo = this.CaseComplainedStoreNo;
            result.CaseComplainedNodeName = this.CaseComplainedNodeName;
            result.InvoiceType = this.InvoiceType;
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
        /// 轉派對象
        /// </summary>
        public List<CaseAssignmentUserViewModel> AssignmentUsers { get; set; }
        /// <summary>
        /// 轉派對象ID
        /// </summary>
        public List<int> AssignmentUserID { get; set; }
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
        public string InvoiceID { get; set; }
        /// <summary>
        /// 反應者類型
        /// </summary>
        public UnitType? CaseConcatUnitType { get; set; }
        /// <summary>
        /// 連絡者姓名
        /// </summary>
        public string ConcatName { get; set; }
        /// <summary>
        /// 反應者電話
        /// </summary>
        public string ConcatTelephone { get; set; }
        /// <summary>
        /// 反應者信箱
        /// </summary>
        public string ConcatEmail { get; set; }
        /// <summary>
        /// 反應者店名
        /// </summary>
        public string ConcatStoreName { get; set; }
        /// <summary>
        /// 反應者店號
        /// </summary>
        public string ConcatStoreNo { get; set; }
        /// <summary>
        /// 反應者單位名稱
        /// </summary>
        public string ConcatNodeName { get; set; }
        /// <summary>
        /// 被反應者類型
        /// </summary>
        public UnitType? CaseComplainedUnitType { get; set; }
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
        /// 反應類別
        /// </summary>
        public string InvoiceType { get; set; }
        /// <summary>
        /// 分類代號
        /// </summary>
        public int? ClassificationID { get; set; }
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
        /// 通知時間開始時間
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
        /// 通知時間結束時間
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
