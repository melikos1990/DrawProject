using System;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;


namespace SMARTII.Areas.Case.Models
{
    public class CaseResumeListViewModel
    {
        public CaseResumeListViewModel()
        {
        }
        public CaseResumeListViewModel(Domain.Case.CaseResume @case)
        {
            this.CaseID = @case.CaseID;
            this.CreateDateTime = @case.CreateDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.Content = @case.Content;
            this.CaseType = @case.CaseType.GetDescription();
            this.CreateUserName = @case.CreateUserName;
        }
        /// <summary>
        /// 案件編號
        /// </summary>
        public string CaseID { get; set; }

        /// <summary>
        /// 案件型態
        /// </summary>
        public string CaseType { get; set; }

        /// <summary>
        /// 異動內容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 立案時間
        /// </summary>
        public string CreateDateTime { get; set; }

        /// <summary>
        /// 立案人員
        /// </summary>
        public string CreateUserName { get; set; }
    }
}
