using System;
using MultipartDataMediaFormatter.Infrastructure;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Master;
using SMARTII.Domain.Data;

namespace SMARTII.Domain.Notification.Email
{
    public class OfficialEmailEffectivePayload : DynamicSerializeObject, IReceivePayload, IOfficialEailGroupRelationship, IOrganizationRelationship
    {

        /// <summary>
        /// 組織節點
        /// </summary>
        public int NodeID { get; set; }

        /// <summary>
        /// 組織型態
        /// </summary>
        public OrganizationType OrganizationType { get; set; }

        /// <summary>
        /// Email ID
        /// </summary>
        public string MessageID { get; set; }

        /// <summary>
        /// 主旨
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 寄信者地址
        /// </summary>
        public string FromAddress { get; set; }

        /// <summary>
        /// 寄信者名稱
        /// </summary>
        public string FromName { get; set; }

        /// <summary>
        /// 信件內容
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 接收時間(來源信件的時間)
        /// </summary>
        public DateTime ReceivedDateTime { get; set; }

        /// <summary>
        /// 來源Email檔案 實體路徑
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 資料建立時間
        /// </summary>
        public DateTime CreateDateTime { get; set;}

        /// <summary>
        /// 案件編號
        /// </summary>
        public string CaseID { get; set; }

        /// <summary>
        /// 群組代號
        /// </summary>
        public int? GroupID { get; set; }

        /// <summary>
        /// 信件實體檔
        /// </summary>
        public HttpFile Mail { get; set; }

        /// <summary>
        /// 來源信箱ID
        /// </summary>
        public int Email_Group_ID { get; set; }

        /// <summary>
        /// 是否有附件
        /// </summary>
        public bool HasAttachment { set; get; }
        
        /// <summary>
        /// 來源信箱
        /// </summary>

        #region impl

        public string Account { get; set; }

        public string BuMailAccount { get; set; }

        public string NodeName { get; set; }

        public IOrganizationNode Node { get; set; }
        
        public string MailDisplayName { get; set; }

        #endregion

    }
}
