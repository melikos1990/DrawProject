using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using AutoMapper;
using MultipartDataMediaFormatter.Infrastructure;
using Newtonsoft.Json;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Master;
using SMARTII.Domain.Master.Parser;
using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Case
{


    public class Case : DynamicSerializeObject, IOrganizationRelationship, IQuestionClassificationRelationship, IFlowable
    {
        public Case()
        {
        }
        /// <summary>
        ///案件編號
        /// </summary>
        [Replace("CaseID")]
        [Description("案件編號")]
        public string CaseID { get; set; }

        /// <summary>
        ///來源代號
        /// </summary>
        [Description("來源代號")]
        public string SourceID { get; set; }

        /// <summary>
        /// KPI 群組 (CC_NODE)
        /// </summary>
        [Description("KPI 群組 (CC_NODE)")]
        public int? GroupID { get; set; }

        /// <summary>
        ///案件負責人代號
        /// </summary>
        [Description("案件負責人代號")]
        public string ApplyUserID { get; set; }

        /// <summary>
        ///認養時間
        /// </summary>
        [Description("認養時間")]
        public DateTime ApplyDateTime { get; set; }

        /// <summary>
        /// 認養人姓名
        /// </summary>
        [Description("認養人姓名")]
        public string ApplyUserName { get; set; }

        /// <summary>
        /// 反應內容
        /// </summary>
        [Replace("CaseContent")]
        [Description("案件內容")]
        [Custom("案件內容")]
        public string Content { get; set; }

        /// <summary>
        /// 案件狀態(0 : 立案 ; 1 : 處理中 ; 2 : 結案)
        /// </summary>
        [Description("案件狀態")]
        public CaseType CaseType { get; set; }

        /// <summary>
        ///應完成時間
        /// </summary>
        [Description("應完成時間")]
        public DateTime PromiseDateTime { get; set; }

        /// <summary>
        ///客戶期望完成時間
        /// </summary>
        [Description("客戶期望完成時間")]
        public DateTime? ExpectDateTime { get; set; }

        /// <summary>
        /// 案件附件路徑
        /// </summary>
        [Description("案件附件路徑")]
        public string[] FilePath { get; set; }

        /// <summary>
        /// 案件附件
        /// </summary>
        [Description("案件附件")]
        public List<HttpFile> Files { get; set; }

        /// <summary>
        /// 結案內容
        /// </summary>
        [Description("結案內容")]
        [Custom("結案內容")]
        public string FinishContent { get; set; }

        /// <summary>
        /// 結案附件路徑
        /// </summary>
        [Description("結案附件路徑")]
        public string[] FinishFilePath { get; set; }

        /// <summary>
        /// 結案附件
        /// </summary>
        [Description("結案附件")]
        public List<HttpFile> FinishFiles { get; set; }

        /// <summary>
        ///結案時間
        /// </summary>
        [Description("結案時間")]
        public DateTime? FinishDateTime { get; set; }

        /// <summary>
        /// 結案人
        /// </summary>
        [Description("結案人")]
        public string FinishUserName { get; set; }

        /// <summary>
        /// 案件結案回覆時間
        /// </summary>
        [Description("案件結案回覆時間")]
        public DateTime? FinishReplyDateTime { get; set; }

        /// <summary>
        /// 結案附件留存路徑
        /// </summary>
        [Description("案件編號")]
        public string FinishEMLFilePath { get; set; }

        /// <summary>
        ///是否列入日報
        /// </summary>
        [Description("是否列入日報")]
        public bool IsReport { get; set; }

        /// <summary>
        ///是否待關注
        /// </summary>
        [Description("是否待關注")]
        public bool IsAttension { get; set; }

        /// <summary>
        ///問題分類代號
        /// </summary>
        [Description("問題分類代號")]
        public int QuestionClassificationID { get; set; }

        /// <summary>
        ///緊急程度代號
        /// </summary>
        [Description("緊急程度代號")]
        public int CaseWarningID { get; set; }

        /// <summary>
        /// 相關案件代號清單
        /// </summary>
        [Description("相關案件代號清單")]
        public string[] RelationCaseIDs { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        [Replace("NotifyTime", typeof(CaseTemplateParser), nameof(CaseTemplateParser.NotifyDateTimeParing))] // 通知時間 *Template 解析時的當下時間*
        [Replace("CreateDateTime", typeof(CaseTemplateParser), nameof(CaseTemplateParser.CreateDateTimeParing))]
        [Description("建立時間")]
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 建立人員
        /// </summary>
        [Replace("CreateUserName", typeof(CaseTemplateParser), nameof(CaseTemplateParser.CreateUserNameParing))] // 建立人員 *Template 解析時的當下客服人員姓名*
        [Description("建立人員")]
        public string CreateUserName { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        [Description("更新時間")]
        public DateTime? UpdateDateTime { get; set; }

        /// <summary>
        /// 更新人員
        /// </summary>
        [Description("更新人員")]
        public string UpdateUserName { get; set; }


        /// <summary>
        /// 認養Email來源信件
        /// </summary>
        [Replace("SourceEmailTitle", typeof(CaseTemplateParser), nameof(CaseTemplateParser.CaseEmailTitleParing))]
        [Description("認養Email來源信件")]
        public string EMLFilePath { get; set; }

        #region reference
        /// <summary>
        /// 案件緊急程度
        /// </summary>
        [Description("案件緊急程度")]
        public CaseWarning CaseWarning { get; set; }

        /// <summary>
        /// 案件來源
        /// </summary>
        [Description("案件來源")]
        public CaseSource CaseSource { get; set; }

        /// <summary>
        /// 案件轉派
        /// </summary>
        [Replace("AssignmentContent", typeof(CaseTemplateParser), nameof(CaseTemplateParser.CaseAssignmenContentParing))]
        [Replace("AssignmentNotifyUsers", typeof(CaseTemplateParser), nameof(CaseTemplateParser.CaseAssignmenNoticeUserParing))]
        [Replace("AssignmentUsers", typeof(CaseTemplateParser), nameof(CaseTemplateParser.AssignmentUsersParing))]
        [Replace("UnitCode", typeof(CaseTemplateParser), nameof(CaseTemplateParser.UnitCodeParing))]
        [Description("案件轉派")]
        public List<CaseAssignment> CaseAssignments { get; set; }

        /// <summary>
        /// 一般通知
        /// </summary>
        [Replace("ComplaintNoticeUsers", typeof(CaseTemplateParser), nameof(CaseTemplateParser.CaseAssignmenNoticeParing))]
        [Description("一般通知")]
        public List<CaseAssignmentComplaintNotice> ComplaintNotice { get; set; }

        /// <summary>
        /// 反應單
        /// </summary>
        [Replace("ComplaintInvoiceUsers", typeof(CaseTemplateParser), nameof(CaseTemplateParser.CaseAssignmenInvoiceParing))]
        [Replace("InvoiceID", typeof(CaseTemplateParser), nameof(CaseTemplateParser.CaseAssignmenInvoiceIDParing))]
        [Description("反應單")]
        public List<CaseAssignmentComplaintInvoice> ComplaintInvoice { get; set; }

        /// <summary>
        /// 案件客訴對象
        /// </summary>
        [Replace("ComplaintedUser", typeof(CaseTemplateParser), nameof(CaseTemplateParser.CaseComplainedUsersParing))]
        [Replace("ComplaintedUserInfo", typeof(CaseTemplateParser), nameof(CaseTemplateParser.CaseComplainedUsersInfoParing))]
        [Replace("InvoiceTitle21NodeName", typeof(CaseTemplateParser), nameof(CaseTemplateParser.InvoiceTitle21NodeNameParing))]
        [Replace("InvoiceTitleNodeName", typeof(CaseTemplateParser), nameof(CaseTemplateParser.InvoiceTitleNodeNameParing))]

        [Description("案件客訴對象")]
        public List<CaseComplainedUser> CaseComplainedUsers { get; set; }

        /// <summary>
        /// 案件標籤
        /// </summary>
        [Description("案件標籤")]
        public List<CaseTag> CaseTags { get; set; }

        /// <summary>
        /// 案件聯絡人
        /// </summary>
        [Replace("ConcatUser", typeof(CaseTemplateParser), nameof(CaseTemplateParser.CaseConcatUsersParing))]
        [Replace("ConcatUserName", typeof(CaseTemplateParser), nameof(CaseTemplateParser.CaseConcatUsersNameParing))]
        [Replace("ConcatUserPhone", typeof(CaseTemplateParser), nameof(CaseTemplateParser.CaseConcatUsersPhoneParing))]
        [Description("反應者資訊")]
        [Custom("反應者資訊")]
        public List<CaseConcatUser> CaseConcatUsers { get; set; }

        /// <summary>
        /// 案件商品
        /// </summary>
        [Description("案件商品")]
        public List<CaseItem> Items { get; set; }

        /// <summary>
        /// 結案原因
        /// </summary>
        [Description("結案原因")]
        public List<CaseFinishReasonData> CaseFinishReasonDatas { get; set; }

        /// <summary>
        /// 單位溝通
        /// </summary>
        [Description("單位溝通")]
        public List<CaseAssignmentCommunicate> CaseAssignmentCommunicates { get; set; }


        /// <summary>
        /// 案件追蹤
        /// </summary>
        [Description("案件追蹤")]
        public List<CaseRemind> CaseReminds { get; set; }


        #endregion reference

        #region impl Node
        /// <summary>
        /// 組織型態
        /// </summary>
        public OrganizationType OrganizationType { get; set; } = OrganizationType.HeaderQuarter;

        /// <summary>
        /// 組織節點代號(企業別)
        /// </summary>
        public int NodeID { get; set; }

        /// <summary>
        /// 組織名稱
        /// </summary>
        public string NodeName { get; set; }

        /// <summary>
        /// 組織
        /// </summary>
        public IOrganizationNode Node { get; set; }

        #endregion impl Node

        #region impl classification


        [Description("問題分類名稱")]
        [Custom("問題分類")]
        public string QuestionClassificationName { get; set; }

        [Replace("QuestionClassifiction", typeof(CaseTemplateParser), nameof(CaseTemplateParser.QuestionClassifiction))]
        [Replace("QuestionClassifictionLevel1", typeof(CaseTemplateParser), nameof(CaseTemplateParser.QuestionClassifictionLevel1))]
        [Replace("QuestionClassifictionLevel2", typeof(CaseTemplateParser), nameof(CaseTemplateParser.QuestionClassifictionLevel2))]
        [Replace("QuestionClassifictionLevel3", typeof(CaseTemplateParser), nameof(CaseTemplateParser.QuestionClassifictionLevel3))]
        [Replace("QuestionClassifictionLevel4", typeof(CaseTemplateParser), nameof(CaseTemplateParser.QuestionClassifictionLevel4))]
        public string QuestionClassificationParentNames { get; set; }
        public string QuestionClassificationParentPath { get; set; }
        public string[] QuestionClassificationParentNamesByArray { get; set; }
        public string[] QuestionClassificationParentPathByArray { get; set; }

        #endregion


    }
}
