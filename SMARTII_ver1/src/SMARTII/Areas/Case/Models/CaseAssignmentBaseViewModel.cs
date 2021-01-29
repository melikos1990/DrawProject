using System;
using System.Collections.Generic;
using MultipartDataMediaFormatter.Infrastructure;
using SMARTII.Domain.Case;
using SMARTII.Domain.Notification.Email;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Case.Models
{
    public class CaseAssignmentBaseViewModel
    {
        /// <summary>
        /// 組織節點代號
        /// </summary>
        public int NodeID { get; set; }
        /// <summary>
        /// 企業別代號
        /// </summary>
        public OrganizationType OrganizationType { get; set; }

        /// <summary>
        ///  案件代號
        /// </summary>
        public string CaseID { get; set; }

        /// <summary>
        /// 歷程模式
        /// </summary>
        public virtual CaseAssignmentProcessType CaseAssignmentProcessType { get; set; }

        /// <summary>
        /// 歷程模式名稱
        /// </summary>
        public virtual string CaseAssignmentProcessTypeName { get; set; }

        /// <summary>
        /// 附檔路徑
        /// </summary>
        public string[] FilePath { get; set; }

        /// <summary>
        /// 副檔
        /// </summary>
        public List<HttpFile> Files { get; set; }

        /// <summary>
        /// 案件內容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 通知行為
        /// </summary>
        public string[] NotificationBehaviors { get; set; }

        /// <summary>
        /// 通知對象
        /// ※ 報表資訊
        /// </summary>
        public string NotificationUsers { get; set; }

        /// <summary>
        /// 通知時間
        /// </summary>
        public DateTime? NotificationDateTime { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 建立人員
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 寄出信件備份路徑
        /// </summary>
        public string EMLFilePath { get; set; }

        /// <summary>
        /// 準備通知的郵件
        /// </summary>
        public EmailPayload EmailPayload { get; set; }
       
        /// <summary>
        /// 所屬的案件
        /// </summary>
        public CaseDetailViewModel Case { get; set; }

        /// <summary>
        /// 回應內容
        /// </summary>
        public string ReplyContent { get; set; }

    }
}
