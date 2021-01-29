using System;

namespace SMARTII.Domain.Case
{
    public class CaseAssignmentResume
    {
        /// <summary>
        /// 識別直
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 案件編號
        /// </summary>
        public string CaseID { get; set; }

        /// <summary>
        /// 轉派序號
        /// </summary>
        public int CaseAssignmentID { get; set; }

        /// <summary>
        /// 異動內容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 當前轉派狀態
        /// </summary>
        public CaseAssignmentType CaseAssignmentType { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 建立人員
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 建立單位代號
        /// </summary>
        public int? CreateNodeID { get; set; }   

        /// <summary>
        /// 建立單位組織 (單位父節點)
        /// </summary>
        public string CreateNodeName { get; set; }

        /// <summary>
        /// 建立組織型態
        /// </summary>
        public Organization.OrganizationType? CreateOrganizationType { get; set; }

        /// <summary>
        /// 是否為回覆資料 (需區隔變更以及回應)
        /// </summary>
        public bool IsReply { get; set; }
    }
}
