using System;
using System.Collections.Generic;
using System.ComponentModel;
using MultipartDataMediaFormatter.Infrastructure;
using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Case
{
    public class CaseAssignmentBase : IFlowable , IOrganizationRelationship
    {
        /// <summary>
        ///  案件代號
        /// </summary>
        [Description("案件代號")]
        public string CaseID { get; set; }

        /// <summary>
        /// 歷程模式
        /// </summary>
        [Description("歷程模式")]
        public virtual CaseAssignmentProcessType CaseAssignmentProcessType { get; set; }

        /// <summary>
        /// 附檔路徑
        /// </summary>
        [Description("附檔路徑")]
        public string[] FilePath { get; set; }

        /// <summary>
        /// 副檔
        /// </summary>
        [Description("副檔")]
        public List<HttpFile> Files { get; set; }

        /// <summary>
        /// 案件內容
        /// </summary>
        [Description("案件內容")]
        public string Content { get; set; }

        /// <summary>
        /// 通知行為
        /// </summary>
        [Description("通知行為")]
        public string[] NotificationBehaviors { get; set; }

        /// <summary>
        /// 通知對象
        /// ※ 報表資訊
        /// </summary>
        [Description("通知對象")]
        public string[] NoticeUsers { get; set; }

        /// <summary>
        /// 通知時間
        /// </summary>
        [Description("通知時間")]
        public DateTime? NotificationDateTime { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        [Description("建立時間")]
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 建立人員
        /// </summary>
        [Description("建立人員")]
        public string CreateUserName { get; set; }

        /// <summary>
        /// 寄出信件備份路徑
        /// </summary>
        [Description("寄出信件備份路徑")]
        public string EMLFilePath { get; set; }


        /// <summary>
        /// 所屬案件
        /// </summary>
        public Case Case { get; set; }

        #region impl

        /// <summary>
        /// 組織節點代號(企業別)
        /// </summary>
        public int NodeID { get; set; }

        /// <summary>
        /// 組織型態
        /// </summary>
        public OrganizationType OrganizationType { get; set; }

        /// <summary>
        /// 組織名稱
        /// </summary>
        public string NodeName { get; set; }

        /// <summary>
        /// 組織節點
        /// </summary>
        public IOrganizationNode Node { get; set; }

        #endregion
    }
}
