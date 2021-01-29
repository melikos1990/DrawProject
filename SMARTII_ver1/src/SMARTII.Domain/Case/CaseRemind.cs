using System;
using System.Collections.Generic;
using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Case
{
    public class CaseRemind : IOrganizationRelationship
    {
        /// <summary>
        /// 流水號
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 案件編號
        /// </summary>
        public string CaseID { get; set; }

        /// <summary>
        /// 生效時間(起)
        /// </summary>
        public DateTime ActiveStartDateTime { get; set; }

        /// <summary>
        /// 生效時間(迄)
        /// </summary>
        public DateTime ActiveEndDateTime { get; set; }

        /// <summary>
        /// 內容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 人員代號
        /// </summary>
        public List<string> UserIDs { get; set; }
        /// <summary>
        /// 通知人員清單
        /// ※ 該欄位透過二次查詢求得
        /// </summary>
        public List<User> Users { get; set; }
        /// <summary>
        /// 是否確認
        /// </summary>
        public bool IsConfirm { get; set; }

        /// <summary>
        /// 確認人員代號
        /// </summary>
        public string ConfirmUserID { get; set; }

        /// <summary>
        /// 確認人員名稱
        /// </summary>
        public string ConfirmUserName { get; set; }

        /// <summary>
        /// 確認時間
        /// </summary>
        public DateTime? ConfirmDateTime { get; set; }

        /// <summary>
        /// 緊急程度
        /// </summary>
        public CaseRemindType Type { get; set; }

        /// <summary>
        /// 建立人代號
        /// </summary>
        public string CreateUserID { get; set; }

        /// <summary>
        /// 建立人名稱
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 更新人名稱
        /// </summary>
        public string UpdateUserName { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime? UpdateDateTime { get; set; }

        /// <summary>
        /// 轉派代號
        /// </summary>
        public int? AssignmentID { get; set; }

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
        /// 組織
        /// </summary>
        public IOrganizationNode Node { get; set; }

        #endregion impl
    }
}
