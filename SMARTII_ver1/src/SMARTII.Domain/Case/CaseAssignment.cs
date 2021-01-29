using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using AutoMapper;
using MultipartDataMediaFormatter.Infrastructure;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Case
{
    public class CaseAssignment : CaseAssignmentBase, ICaseRemindRelationship
    {
        public CaseAssignment() { }

        /// <summary>
        /// 轉派代號
        /// </summary>
        [Description("轉派代號")]
        public int AssignmentID { get; set; }

        /// <summary>
        /// 銷案內容
        /// </summary>
        [Description("銷案內容")]
        public string FinishContent { get; set; }

        /// <summary>
        /// 銷案附件路徑
        /// </summary>
        [Description("銷案附件路徑")]
        public string[] FinishFilePath { get; set; }

        /// <summary>
        /// 檔案實體
        /// </summary>
        [Description("檔案實體")]
        public List<HttpFile> FinishFiles { get; set; }

        /// <summary>
        /// 銷案時間
        /// </summary>
        [Description("銷案時間")]
        public DateTime? FinishDateTime { get; set; }

        /// <summary>
        /// 銷案人員
        /// </summary>
        [Description("銷案人員")]
        public string FinishUserName { get; set; }


        /// <summary>
        /// 銷案狀態 (0 : 已派工 ; 1 : 處理完成 ; 2 : 銷案)
        /// </summary>
        [Description("銷案狀態")]
        public CaseAssignmentType CaseAssignmentType { get; set; }



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
        /// 駁回型態 (0 : 無 , 1 : 資料重填 , 2 : 重新處理)
        /// </summary>
        [Description("駁回型態")]
        public RejectType RejectType { get; set; }

        /// <summary>
        /// 駁回原因
        /// </summary>
        [Description("駁回原因")]
        public string RejectReason { get; set; }

        /// <summary>
        /// 催修次數
        /// </summary>
        [Description("催修次數")]
        public int RecallTimes { get; set; }

        /// <summary>
        /// 結案單位
        /// </summary>
        [Description("結案單位")]
        public int? FinishNodeID { get; set; }

        /// <summary>
        /// 結案組織 (單位父節點)
        /// </summary>
        [Description("結案組織")]
        public string FinishNodeName { get; set; }

        /// <summary>
        /// 結案組織型態
        /// </summary>
        [Description("結案組織型態")]
        public OrganizationType? FinishOrganizationType { get; set; }

        /// <summary>
        /// 案件轉派通知對象
        /// </summary>
        [Description("案件轉派通知對象")]
        public List<CaseAssignmentConcatUser> CaseAssignmentConcatUsers { get; set; }

        /// <summary>
        /// 案件轉派對象
        /// </summary>
        [Description("案件轉派對象")]
        public List<CaseAssignmentUser> CaseAssignmentUsers { get; set; }

        /// <summary>
        /// 回應內容
        /// </summary>
        [Description("回應內容")]
        public string ReplyContent { get; set; }

        /// <summary>
        /// 銷按模式
        /// </summary>
        [IgnoreMap]
        [Description("銷按模式")]
        public CaseAssignmentWorkType? CaseAssignmentWorkType
        {
            get
            {
                if (this.CaseAssignmentUsers.Any() == false) return null;

                return (this.CaseAssignmentUsers.Count(x => x.CaseComplainedUserType == CaseComplainedUserType.Responsibility) > 1) ?
                        Domain.Case.CaseAssignmentWorkType.Accompanied :
                        Domain.Case.CaseAssignmentWorkType.General;
            }
        }


        #region override

        public override CaseAssignmentProcessType CaseAssignmentProcessType { get; set; } =
                        CaseAssignmentProcessType.Assignment;

        #endregion

        #region impl

        public List<CaseRemind> CaseReminds { get; set; }


        #endregion

    }
}
