using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMARTII.Domain.Data;

namespace SMARTII.Areas.Summary.Models.HeaderQuarterSummary
{
    public class HeaderQuarterSummaryUnCloseListViewModel
    {

        public HeaderQuarterSummaryUnCloseListViewModel(Domain.Case.Case @case)
        {
            var concatUser = @case.CaseConcatUsers?.FirstOrDefault() ?? null;
            var complainedUser = @case.CaseComplainedUsers?.FirstOrDefault() ?? null;
            
            this.CaseSourceType = @case.CaseSource.CaseSourceType.GetDescription();
            this.CreateTime = @case.CreateDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            this.CaseID = @case.CaseID;
            this.CaseWarning = @case.CaseWarning.Name;
            this.CaseType = @case.CaseType.GetDescription();
            this.ConcatUserName = @case.CaseConcatUsers.FirstOrDefault()?.UserName ?? "";
            this.ComplainedUserName = complainedUser?.UserName ?? "";
            this.CaseContent = @case.Content;
            this.ApplyUserName = @case.ApplyUserName;
            this.FirstClassification = @case.QuestionClassificationParentNamesByArray?.FirstOrDefault() ?? "";
            this.ComplainedUserParentNamePath = complainedUser?.ParentPathName ?? "";

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
        /// 案件編號
        /// </summary>
        public string CaseID { get; set; }
        /// <summary>
        /// 案件等級
        /// </summary>
        public string CaseWarning { get; set; }
        /// <summary>
        /// 案件狀態
        /// </summary>
        public string CaseType { get; set; }
        /// <summary>
        /// 連絡者
        /// </summary>
        public string ConcatUserName { get; set; }
        /// <summary>
        /// 被反應者
        /// </summary>
        public string ComplainedUserName { get; set; }
        /// <summary>
        /// 案件內容
        /// </summary>
        public string CaseContent { get; set; }
        /// <summary>
        /// 負責人
        /// </summary>
        public string ApplyUserName { get; set; }
        /// <summary>
        /// 問題分類1
        /// </summary>
        public string FirstClassification { get; set; }
        /// <summary>
        /// 結案內容
        /// </summary>
        public string FinishContent { get; set; }
        /// <summary>
        /// 被反應者組織
        /// </summary>
        public string ComplainedUserParentNamePath { get; set; }

    }
}
