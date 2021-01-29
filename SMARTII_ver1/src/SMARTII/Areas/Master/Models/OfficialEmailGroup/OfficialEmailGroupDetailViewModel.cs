using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMARTII.Domain.Data;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Master.Models.OfficialEmailGroup
{
    public class OfficialEmailGroupDetailViewModel
    {
        public OfficialEmailGroupDetailViewModel()
        {
        }
        public OfficialEmailGroupDetailViewModel(Domain.Notification.OfficialEmailGroup data)
        {
            this.BuID = data.NodeID;
            this.BuName = data.NodeName;
            this.ID = data.ID;
            this.KeepDay = data.KeepDay;
            this.MailAddress = data.MailAddress;
            this.Account = data.Account;
            this.Password = data.Password;
            this.UpdateDateTime = data.UpdateDateTime.DisplayWhenNull();
            this.UpdateUserName = data.UpdateUserName.DisplayWhenNull();
            this.CreateDateTime = data.CreateDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.CreateUserName = data.CreateUserName;
            this.Protocol = data.MailProtocolType;
            this.IsEnabled = data.IsEnabled;
            this.AllowReceive = data.AllowReceive;
            this.MailDisplayName = data.MailDisplayName;
            this.OfficialEmail = data.OfficialEmail;
            this.Users = data.User.Select(x=> new OfficialEmailGroupUserListViewModel()
            {
                UserID = x.UserID,
                Account = x.Account,
                UserName = x.Name

            }).ToList();
            this.HostName = data.HostName;
        }
        public Domain.Notification.OfficialEmailGroup ToDomain()
        {
            var result = new Domain.Notification.OfficialEmailGroup();
            result.ID = this.ID;
            result.NodeID = this.BuID;
            result.OrganizationType = OrganizationType.HeaderQuarter;
            result.MailAddress = this.MailAddress;
            result.Account = this.Account;
            result.Password = this.Password;
            result.KeepDay = this.KeepDay;
            result.MailProtocolType = this.Protocol;
            result.IsEnabled = this.IsEnabled;
            result.AllowReceive = this.AllowReceive;
            result.MailDisplayName = this.MailDisplayName;
            result.OfficialEmail = this.OfficialEmail;
            result.User = this.Users.Select(x => new User()
            {
                UserID = x.UserID
            }).ToList();
            result.HostName = this.HostName;
            return result;
        }
        /// <summary>
        /// 企業別代號
        /// </summary>
        public int BuID { get; set; }
        /// <summary>
        /// 企業別名稱
        /// </summary>
        public string BuName { get; set; }
        /// <summary>
        /// 識別ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 保留天數
        /// </summary>
        public int KeepDay { get; set; }
        /// <summary>
        /// 來源信箱
        /// </summary>
        public string MailAddress { get; set; }
        /// <summary>
        /// 帳號
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 密碼
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 協定
        /// </summary>
        public MailProtocolType Protocol { get; set; }
        /// <summary>
        /// 建立時間
        /// </summary>
        public string CreateDateTime { get; set; }
        /// <summary>
        /// 建立人員
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 更新時間
        /// </summary>
        public string UpdateDateTime { get; set; }
        /// <summary>
        /// 更新人員
        /// </summary>
        public string UpdateUserName { get; set; }
        /// <summary>
        /// 提醒人員清單
        /// </summary>
        public List<OfficialEmailGroupUserListViewModel> Users { get; set; }
        /// <summary>
        /// 信件伺服器
        /// </summary>
        public string HostName { get; set; }
        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool IsEnabled { get; set; }


        /// <summary>
        /// 來源信箱
        /// </summary>
        public string OfficialEmail { get; set; }


        /// <summary>
        /// 寄信顯示名稱
        /// </summary>
        public string MailDisplayName { get; set; }


        /// <summary>
        /// 是否允許收信(Batch)
        /// </summary>
        public bool AllowReceive { get; set; }

    }
}
