using System;
using System.Collections.Generic;
using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Notification
{
    public class OfficialEmailGroup: IOrganizationRelationship
    {
        /// <summary>
        /// 流水號
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 節點ID
        /// </summary>
        public int NodeID { get; set; }

        /// <summary>
        /// 組織型態
        /// </summary>
        public OrganizationType OrganizationType { get; set; }

        /// <summary>
        /// BU信箱
        /// </summary>
        public string MailAddress { get; set; }

        /// <summary>
        /// 寄信顯示名稱
        /// </summary>
        public string MailDisplayName { get; set; }

        /// <summary>
        /// 來源信箱
        /// </summary>
        public string OfficialEmail { get; set; }

        /// <summary>
        /// 帳號
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 密碼
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 是否允許收信(Batch)
        /// </summary>
        public bool AllowReceive { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateDateTime { get; set; }
        

        /// <summary>
        /// 建立人員
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 建立人員
        /// </summary>
        public DateTime? UpdateDateTime { get; set; }

        /// <summary>
        /// 更新人員
        /// </summary>
        public string UpdateUserName { get; set; }

        /// <summary>
        /// 傳輸協定
        /// </summary>
        public MailProtocolType MailProtocolType { get; set; }

        /// <summary>
        /// 資料保存期限
        /// </summary>
        public int KeepDay { get; set; }


        /// <summary>
        /// HOSTNAME
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 人員
        /// </summary>
        public List<User> User { get; set; }
        public string NodeName { get ; set; }
        public IOrganizationNode Node { get ; set; }
    }
}
