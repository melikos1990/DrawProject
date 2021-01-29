using System;
using System.Collections.Generic;
using System.Linq;
using MultipartDataMediaFormatter.Infrastructure;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Case.Models
{
    public class CaseAssignmentViewModel : CaseAssignmentBaseViewModel
    {
        public CaseAssignmentViewModel()
        {

        }
        public CaseAssignmentViewModel(CaseAssignment assignment)
        {

            if (assignment == null) return;

            this.AssignmentID = assignment.AssignmentID;
            this.CaseAssignmentConcatUsers = assignment.CaseAssignmentConcatUsers?
                                                       .Select(x => new CaseAssignmentConcatUserViewModel(x))
                                                       .ToList();

            this.CaseAssignmentUsers = assignment.CaseAssignmentUsers?
                                                 .Select(x => new CaseAssignmentUserViewModel(x))?
                                                 .ToList();



            this.CaseAssignmentProcessType = assignment.CaseAssignmentProcessType;

            this.CaseAssignmentWorkType = assignment.CaseAssignmentWorkType;
            this.CaseAssignmentWorkTypeName = assignment.CaseAssignmentWorkType?.GetDescription();

            this.CaseAssignmentProcessTypeName = assignment.CaseAssignmentProcessType.GetDescription();
            this.CaseAssignmentType = assignment.CaseAssignmentType;
            this.CaseAssignmentTypeName = assignment.CaseAssignmentType.GetDescription();
            this.CaseID = assignment.CaseID;
            this.Content = assignment.Content;
            this.CreateDateTime = assignment.CreateDateTime;
            this.CreateUserName = assignment.CreateUserName;
            this.Files = assignment.Files;
            this.FilePath = assignment.FilePath;
            this.FinishContent = assignment.FinishContent;
            this.FinishDateTime = assignment.FinishDateTime;
            this.FinishFiles = assignment.FinishFiles;
            this.FinishFilePath = assignment.FinishFilePath;
            this.FinishUserName = assignment.FinishUserName;
            this.FinishNodeID = assignment.FinishNodeID;
            this.FinishNodeName = assignment.FinishNodeName;
            this.FinishOrganizationType = assignment.FinishOrganizationType;
            this.NodeID = assignment.NodeID;
            this.NotificationUsers = string.Join(",", assignment.NoticeUsers ?? new string[] { });
            this.NotificationBehaviors = assignment.NotificationBehaviors;
            this.NotificationDateTime = assignment.NotificationDateTime;
            this.OrganizationType = assignment.OrganizationType;
            this.RejectType = assignment.RejectType;
            this.RejectReason = assignment.RejectReason;
            this.UpdateDateTime = assignment.UpdateDateTime;
            this.UpdateUserName = assignment.UpdateUserName;
            this.EMLFilePath = assignment.EMLFilePath;

        }

        public CaseAssignment ToDomain()
        {
            var assignment = new CaseAssignment();


            assignment.AssignmentID = this.AssignmentID;
            assignment.CaseAssignmentProcessType = this.CaseAssignmentProcessType;
            assignment.CaseAssignmentType = this.CaseAssignmentType;
            assignment.CaseID = this.CaseID;
            assignment.Content = this.Content;
            assignment.CreateDateTime = this.CreateDateTime;
            assignment.CreateUserName = this.CreateUserName;
            assignment.Files = this.Files;
            assignment.FinishContent = this.FinishContent;
            assignment.FinishDateTime = this.FinishDateTime;
            assignment.FinishFiles = this.FinishFiles;
            assignment.FinishUserName = this.FinishUserName;
            assignment.FinishNodeID = this.FinishNodeID;
            assignment.FinishNodeName = this.FinishNodeName;
            assignment.FinishOrganizationType = this.FinishOrganizationType;
            assignment.NodeID = this.NodeID;
            assignment.NoticeUsers = this.NotificationUsers?.Split(',');
            assignment.NotificationBehaviors = this.NotificationBehaviors;
            assignment.NotificationDateTime = this.NotificationDateTime;
            assignment.OrganizationType = this.OrganizationType;
            assignment.RejectType = this.RejectType;
            assignment.RejectReason = this.RejectReason;
            assignment.UpdateDateTime = this.UpdateDateTime;
            assignment.UpdateUserName = this.UpdateUserName;
            assignment.ReplyContent = this.ReplyContent;
            assignment.CaseAssignmentUsers = this.CaseAssignmentUsers?
                                                 .Select(x => x.ToDoamin())
                                                 .ToList();

            assignment.CaseAssignmentConcatUsers = this.CaseAssignmentConcatUsers?
                                                       .Select(x => x.ToDoamin())
                                                       .ToList();

            return assignment;

        }


        /// <summary>
        /// 轉派代號
        /// </summary>
        public int AssignmentID { get; set; }

        /// <summary>
        /// 銷案內容
        /// </summary>
        public string FinishContent { get; set; }

        /// <summary>
        /// 銷案附件路徑
        /// </summary>
        public string[] FinishFilePath { get; set; }

        /// <summary>
        /// 檔案實體
        /// </summary>
        public List<HttpFile> FinishFiles { get; set; }

        /// <summary>
        /// 銷案時間
        /// </summary>
        public DateTime? FinishDateTime { get; set; }

        /// <summary>
        /// 銷案人員
        /// </summary>
        public string FinishUserName { get; set; }

        /// <summary>
        /// 銷案人員組織名稱
        /// </summary>
        public string FinishNodeName { get; set; }

        /// <summary>
        /// 銷案人員組織代號
        /// </summary>
        public int? FinishNodeID { get; set; }

        /// <summary>
        /// 銷案人員組織型態
        /// </summary>
        public OrganizationType? FinishOrganizationType { get; set; }


        /// <summary>
        /// 銷案狀態 (0 : 已派工 ; 1 : 處理完成 ; 2 : 銷案)
        /// </summary>
        public CaseAssignmentType CaseAssignmentType { get; set; }

        /// <summary>
        /// 銷案狀態 (0 : 已派工 ; 1 : 處理完成 ; 2 : 銷案) 名稱
        /// </summary>
        public string CaseAssignmentTypeName { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime? UpdateDateTime { get; set; }

        /// <summary>
        /// 更新人員
        /// </summary>
        public string UpdateUserName { get; set; }

        /// <summary>
        /// 駁回型態 (0 : 無 , 1 : 資料重填 , 2 : 重新處理)
        /// </summary>
        public RejectType RejectType { get; set; }

        /// <summary>
        /// 駁回原因
        /// </summary>
        public string RejectReason { get; set; }

        /// <summary>
        /// 銷案模式
        /// </summary>
        public CaseAssignmentWorkType? CaseAssignmentWorkType { get; set; }

        /// <summary>
        /// 銷案模式名稱
        /// </summary>
        public string CaseAssignmentWorkTypeName { get; set; }

        /// <summary>
        /// 案件轉派通知對象
        /// </summary>
        public List<CaseAssignmentConcatUserViewModel> CaseAssignmentConcatUsers { get; set; }

        /// <summary>
        /// 案件轉派對象
        /// </summary>
        public List<CaseAssignmentUserViewModel> CaseAssignmentUsers { get; set; }

        #region 操作組織相關

        /// <summary>
        /// 操作人員組織
        /// </summary>
        public int EditorNodeJobID { get; set; }

        #endregion


    }
}
