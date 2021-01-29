using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;

namespace SMARTII.Areas.Summary.Models.CallCenterSummary
{
    public class CallCenterSummaryListViewModel
    {

        public CallCenterSummaryListViewModel(Domain.Case.Case @case)
        {
            var complained = @case.CaseComplainedUsers.FirstOrDefault(x => x.CaseComplainedUserType == CaseComplainedUserType.Responsibility) ?? null;
            var concatUser = @case.CaseConcatUsers.FirstOrDefault();

            this.CaseSourceType = @case.CaseSource.CaseSourceType.GetDescription();
            this.CreateTime = @case.CreateDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            this.CaseID = @case.CaseID;
            this.CaseWarning = @case.CaseWarning.Name;
            this.CaseType = @case.CaseType.GetDescription();
            this.ConcatUserName = concatUser.GetTableConcataAndComplained();

            this.CaseComplainedUserName = complained.GetTableConcataAndComplained();
            this.CaseContent = @case.Content;
            this.ApplyUserName = @case.ApplyUserName;
            this.FirstClassification = @case.QuestionClassificationParentNamesByArray?.FirstOrDefault() ?? "";
            this.ComplainedUserParentNamePath = complained?.ParentPathName ?? "";
        }

        public string CaseSourceType { get; set; }

        public string CreateTime { get; set; }

        public string CaseID { get; set; }

        public string CaseWarning { get; set; }

        public string CaseType { get; set; }

        public string ConcatUserName { get; set; }

        public string CaseComplainedUserName { get; set; }

        public string CaseContent { get; set; }

        public string ApplyUserName { get; set; }

        public string FirstClassification { get; set; }

        public string ComplainedUserParentNamePath { get; set; }
    }
}
