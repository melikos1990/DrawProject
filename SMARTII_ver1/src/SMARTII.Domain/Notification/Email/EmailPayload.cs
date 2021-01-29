using System.Collections.Generic;
using MultipartDataMediaFormatter.Infrastructure;
using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Notification.Email
{
    public class EmailPayload : ISenderPayload
    {
        /// <summary>
        /// 附件檔案
        /// </summary>
        public List<HttpFile> Attachments { get; set; } = new List<HttpFile>();

        /// <summary>
        /// 附件檔案(路徑)
        /// </summary>
        public List<string> FilePaths { get; set; } = new List<string>();

        /// <summary>
        /// CC(副本)
        /// </summary>
        public List<ConcatableUser> Cc { get; set; }

        /// <summary>
        /// BCC(密件副本)
        /// </summary>
        public List<ConcatableUser> Bcc { get; set; }

        /// <summary>
        /// 收件人
        /// </summary>
        public List<ConcatableUser> Receiver { get; set; }

        /// <summary>
        /// 寄信人
        /// </summary>
        public ConcatableUser Sender { get; set; }

        /// <summary>
        /// 主旨
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 內容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 是否夾帶案件的附件
        /// </summary>
        public bool IsAddCaseAttachment { get; set; }
        /// <summary>
        /// 是否已經是html 內文(回覆顧客信件會先行設定html內文) 
        /// </summary>
        public bool IsHtmlBody { get; set; }

        /// <summary>
        /// 聯絡人員
        /// </summary>
        public List<ConcatableUser> ConcatableUsers
        {
            get
            {
                var users = new List<ConcatableUser>();

                users.AddRange(Cc ?? new List<ConcatableUser>());
                users.AddRange(Bcc ?? new List<ConcatableUser>());
                users.AddRange(Receiver ?? new List<ConcatableUser>());
                return users;
            }
        }
    }
}
