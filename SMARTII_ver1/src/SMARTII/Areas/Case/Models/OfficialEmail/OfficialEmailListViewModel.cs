using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMARTII.Domain.Notification.Email;

namespace SMARTII.Areas.Case.Models.OfficialEmail
{
    public class OfficialEmailListViewModel
    {
        public OfficialEmailListViewModel()
        {
        }

        public OfficialEmailListViewModel(OfficialEmailEffectivePayload data)
        {
            this.BuID = data.NodeID;
            this.Body = data.Body;
            this.FromAddress = data.FromAddress;
            this.FromName = data.FromName;
            this.MessageID = data.MessageID;
            this.Subject = data.Subject;
            this.CaseID = data.CaseID;
            this.FilePath = data.FilePath;
            this.ReceivedDateTime = data.ReceivedDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.SourceAddress = data.Account;
            this.HasAttachment = data.HasAttachment;
        }
        public bool HasAttachment { get; set; }

        public string Body { get; set; }

        public string FromAddress { get; set; }

        public string FromName { get; set; }

        public string MessageID { get; set; }

        public string Subject { get; set; }

        public string CaseID { get; set; }

        public string FilePath { get; set; }

        public string ReceivedDateTime { get; set; }

        public string SourceAddress { get; set; }

        public int BuID { get; set; }
    }
}
