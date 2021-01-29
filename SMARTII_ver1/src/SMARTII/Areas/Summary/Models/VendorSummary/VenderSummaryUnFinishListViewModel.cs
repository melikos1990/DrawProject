using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;

namespace SMARTII.Areas.Summary.Models.VendorSummary
{
    public class VenderSummaryUnFinishListViewModel
    {
        public VenderSummaryUnFinishListViewModel(Domain.Case.CaseAssignment caseAssignment)
        {
            var concatUser = caseAssignment.Case.CaseConcatUsers?.FirstOrDefault() ?? null;
            var complainedUser = caseAssignment.Case.CaseComplainedUsers?.FirstOrDefault() ?? null;

            this.NoticeDateTime = caseAssignment.NotificationDateTime.DisplayWhenNull(text: "無");
            this.CaseID = caseAssignment.Case.CaseID;
            this.SN = caseAssignment.AssignmentID;
            this.Mode = CaseAssignmentProcessType.Assignment.GetDescription();
            this.Type = caseAssignment.CaseAssignmentType.GetDescription();
            this.CaseAssignmentUser = caseAssignment.CaseAssignmentUsers == null ? "" : string.Join(" / ", caseAssignment.CaseAssignmentUsers.Select(x=>x.UserName).ToArray());

            this.ConcatUserName = concatUser.GetTableConcataAndComplained();
            this.ComplainedUserName = complainedUser.GetTableConcataAndComplained();
            this.ComplainedUserParentNamePath = complainedUser?.ParentPathName ?? "無";
            this.CaseContent = caseAssignment.Case.Content;
            this.NoticeContent = caseAssignment.Content;
        }


        /// <summary>
        /// 通知時間
        /// </summary>
        public string NoticeDateTime { get; set; }
        /// <summary>
        /// 案件編號
        /// </summary>
        public string CaseID { get; set; }
        /// <summary>
        /// 序號
        /// </summary>
        public int SN { get; set; }
        /// <summary>
        /// 歷程模式
        /// </summary>
        public string Mode { get; set; }
        /// <summary>
        /// 歷程狀態
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 轉派對象
        /// </summary>
        public string CaseAssignmentUser { get; set; }
        /// <summary>
        /// 反應者
        /// </summary>
        public string ConcatUserName { get; set; }
        /// <summary>
        /// 被反應者
        /// </summary>
        public string ComplainedUserName { get; set; }
        /// <summary>
        /// 被反應者組織
        /// </summary>
        public string ComplainedUserParentNamePath { get; set; }
        /// <summary>
        /// 案件內容
        /// </summary>
        public string CaseContent { get; set; }
        /// <summary>
        /// 通知內容
        /// </summary>
        public string NoticeContent { get; set; }

    }
}
