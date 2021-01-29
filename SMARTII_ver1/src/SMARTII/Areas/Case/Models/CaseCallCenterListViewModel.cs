using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Attribute;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Case.Models
{
    public class CaseCallCenterListViewModel
    {

        public CaseCallCenterListViewModel()
        {

        }

        public CaseCallCenterListViewModel(SP_GetCaseList sp_Data)
        {
            var concatTarget = sp_Data.CaseConcatUsersList.FirstOrDefault();
            var complainedTarget = sp_Data.CaseComplainedUsersList.FirstOrDefault(y => y.CaseComplainedUserType == CaseComplainedUserType.Responsibility);

            CaseSourceType = sp_Data.SourceType.GetDescription();  
            CreateTime = sp_Data.CreateDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            CaseID = sp_Data.CaseID;
            CaseWarning = sp_Data.CaseWarningName; 
            CaseType = sp_Data.CaseType.GetDescription();
            CaseContent = sp_Data.CaseContent;
            ApplyUserName = sp_Data.ApplyUserName;
            FinishContent = sp_Data.FinishContent;
            FirstClassification = string.IsNullOrWhiteSpace(sp_Data.ClassificationParentName) ? "" : sp_Data.ClassificationParentName;
            ComplainedUserParentNamePath = complainedTarget?.ParentPathName ?? "";

            ConcatUserName = concatTarget.GetTableConcataAndComplained();
            
            CaseComplainedUserName = complainedTarget.GetTableConcataAndComplained();
            
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
        /// 反應者
        /// </summary>
        public string ConcatUserName { get; set; }
        /// <summary>
        /// 被反應者
        /// </summary>
        public string CaseComplainedUserName { get; set; }
        /// <summary>
        /// 案件內容
        /// </summary>
        public string CaseContent { get; set; }
        /// <summary>
        /// 案件分類1
        /// </summary>
        public string FirstClassification { get; set; }
        /// <summary>
        /// 負責人
        /// </summary>
        public string ApplyUserName { get; set; }
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
