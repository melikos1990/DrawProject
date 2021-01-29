using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;

namespace SMARTII.Areas.Case.Models
{
    public class CaseAssignmentOverviewViewModel
    {
        public CaseAssignmentOverviewViewModel()
        {

        }
        public CaseAssignmentOverviewViewModel(CaseAssignmentBase @base)
        {

            this.CaseID = @base.CaseID;
            this.CaseAssignmentProcessType = @base.CaseAssignmentProcessType;
            this.CaseAssignmentProcessTypeName = @base.CaseAssignmentProcessType.GetDescription();
            this.NoticeDateTime = @base.NotificationDateTime.DisplayWhenNull();
            this.NotifyContent = @base.Content;
            this.NotificationBehaviors = @base.NotificationBehaviors;

            switch (@base.CaseAssignmentProcessType)
            {
                case CaseAssignmentProcessType.Notice:

                    var notice = ((CaseAssignmentComplaintNotice)@base);

                    //this.ComplaintNodeNames = string.Join("/", notice.Users?
                    //                                                 .Select(x => x.NodeName ?? x.UserName)
                    //                                                 .ToArray());

                    this.AssignmentType = (byte)notice.CaseAssignmentComplaintNoticeType;
                    this.AssignmentTypeName = notice.CaseAssignmentComplaintNoticeType.GetDescription();
                    this.NoticeID = notice.ID;


                    break;
                case CaseAssignmentProcessType.Invoice:

                    var invoice = ((CaseAssignmentComplaintInvoice)@base);


                    //this.ComplaintNodeNames = string.Join("/", invoice.Users?
                    //                                                  .Select(x => x.NodeName ?? x.UserName)
                    //                                                  .ToArray());

                    this.AssignmentType = (byte)invoice.CaseAssignmentComplaintInvoiceType;
                    this.AssignmentTypeName = invoice.CaseAssignmentComplaintInvoiceType.GetDescription();

                    this.Identifier = invoice.InvoiceID;
                    this.InvoiceIdentityID = invoice.ID;


                    break;
                case CaseAssignmentProcessType.Assignment:

                    var assignment = ((CaseAssignment)@base);


                    this.ComplaintNodeNames = string.Join("/", assignment.CaseAssignmentUsers?
                                                                         .Where(x => x.CaseComplainedUserType == CaseComplainedUserType.Responsibility)
                                                                         .Select(x => x.NodeName ?? x.UserName)
                                                                         .ToArray());

                    this.AssignmentType = (byte)assignment.CaseAssignmentType;
                    this.AssignmentTypeName = assignment.CaseAssignmentType.GetDescription();

                    this.Identifier = assignment.AssignmentID.ToString();

                    this.FinishedContent = assignment.FinishContent.DisplayWhenNull();
                    this.FinishNodeName = assignment.FinishNodeName;
                    this.FinishedUserName = assignment.FinishUserName.DisplayWhenNull();
                    this.FinishedDateTime = assignment.FinishDateTime.DisplayWhenNull();
                    this.AssignmentID = assignment.AssignmentID;
                    this.CaseRemindIDs = assignment.CaseReminds?.Select(x => x.ID).ToList() ?? new List<int>();


                    this.CaseAssignmentProcessTypeName = (assignment.CaseAssignmentUsers?
                                                                   .Where(x => x.CaseComplainedUserType == CaseComplainedUserType.Responsibility)
                                                                   .Count() > 1) 
                                                                   ? CaseAssignmentWorkType.Accompanied.GetDescription() : CaseAssignmentWorkType.General.GetDescription();

                    break;
                case CaseAssignmentProcessType.Communication:

                    var communicate = ((CaseAssignmentCommunicate)@base);
                    this.CommunicateID = communicate.ID;

                    break;


                default:
                    break;
            }


        }

        #region COMMON

        /// <summary>
        /// 歷程模式
        /// </summary>
        public CaseAssignmentProcessType CaseAssignmentProcessType { get; set; }

        /// <summary>
        /// 歷程模式名稱
        /// </summary>
        public string CaseAssignmentProcessTypeName { get; set; }

        /// <summary>
        /// 聯絡時間
        /// </summary>
        public string NoticeDateTime { get; set; }

        /// <summary>
        /// 案件代號
        /// </summary>
        public string CaseID { get; set; }


        /// <summary>
        /// 通知內容
        /// </summary>
        public string NotifyContent { get; set; }

        /// <summary>
        /// 通知行為
        /// </summary>
        public string[] NotificationBehaviors { get; set; }


        #endregion

        #region ASSIGNMENT
        /// <summary>
        /// 銷案內容
        /// </summary>
        public string FinishedContent { get; set; }
        /// <summary>
        /// 銷案單位
        /// </summary>
        public string FinishNodeName { get; set; }
        /// <summary>
        /// 銷案人
        /// </summary>
        public string FinishedUserName { get; set; }
        /// <summary>
        /// 銷案時間
        /// </summary>
        public string FinishedDateTime { get; set; }

        /// <summary>
        /// 轉派序號
        /// </summary>
        public int? AssignmentID { get; set; }

        /// <summary>
        /// 案件追蹤
        /// </summary>
        public List<int> CaseRemindIDs { get; set; }

        #endregion

        #region INVOICE

        /// <summary>
        /// 識別碼(反應單號/轉派序號)
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// 反應單識別值
        /// </summary>
        public int? InvoiceIdentityID { get; set; }

        #endregion

        #region NOTICE

        /// <summary>
        /// 通知識別值
        /// </summary>
        public int? NoticeID { get; set; }

        #endregion

        #region COMMUNICATE

        /// <summary>
        /// 溝通識別值
        /// </summary>
        public int? CommunicateID { get; set; }

        #endregion

        /// <summary>
        /// 項次
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 處理單位
        /// </summary>
        public string ComplaintNodeNames { get; set; }

        /// <summary>
        /// 狀態名稱
        /// </summary>
        public string AssignmentTypeName { get; set; }

        /// <summary>
        /// 狀態
        /// 因為有三種格式 , 因此以 byte 取代之
        /// </summary>
        public byte AssignmentType { get; set; }


    }
}
