using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Summary.Models.HeaderQuarterSummary
{
    public class HeaderQuarterSummaryUnFinishListViewModel
    {
        public HeaderQuarterSummaryUnFinishListViewModel(Domain.Case.CaseAssignment caseAssignment)
        {
            var concatUser = caseAssignment.Case.CaseConcatUsers?.FirstOrDefault() ?? null;
            var complainedUser = caseAssignment.Case.CaseComplainedUsers?.FirstOrDefault() ?? null;

            this.CaseSourceType = caseAssignment.Case.CaseSource.CaseSourceType.GetDescription();
            this.CreateTime = caseAssignment.Case.CreateDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            this.NoticeDateTime = caseAssignment.NotificationDateTime.DisplayWhenNull(text: "");
            this.CaseID = caseAssignment.Case.CaseID;
            this.SN = caseAssignment.AssignmentID;
            this.Mode = CaseAssignmentProcessType.Assignment.GetDescription();
            this.Type = caseAssignment.CaseAssignmentType.GetDescription();
            

            this.ConcatUserName = concatUser.GetTableConcataAndComplained();
            this.ComplainedUserName = complainedUser?.GetTableConcataAndComplained() ?? "";
            this.ComplainedUserParentNamePath = complainedUser?.ParentPathName ?? "";
            this.CaseContent = caseAssignment.Case.Content;
            this.NoticeContent = caseAssignment.Content;

            this.CaseWarning = caseAssignment.Case.CaseWarning.Name;
            this.CaseType = caseAssignment.Case.CaseType.GetDescription();
            this.ApplyUserName = caseAssignment.Case.ApplyUserName;
            this.FirstClassification = caseAssignment.Case.QuestionClassificationParentNamesByArray?.FirstOrDefault() ?? "";

            var CaseAssignmentUser = caseAssignment.CaseAssignmentUsers?.Select(x =>
            {
                return x.DisplayCaseUserOrOrganization();
            }).ToList() ?? new List<string>();

            this.CaseAssignmentUser = string.Join("/", CaseAssignmentUser);
        }

        /// <summary>
        /// 案件來源
        /// </summary>
        public string CaseSourceType { get; set; }
        /// <summary>
        /// 立案時間
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 案件等級
        /// </summary>
        public string CaseWarning { get; set; }
        /// <summary>
        /// 案件狀態
        /// </summary>
        public string CaseType { get; set; }
        /// <summary>
        /// 負責人
        /// </summary>
        public string ApplyUserName { get; set; }
        /// <summary>
        /// 問題分類1
        /// </summary>
        public string FirstClassification { get; set; }
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
