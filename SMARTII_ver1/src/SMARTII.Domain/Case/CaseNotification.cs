using System;

namespace SMARTII.Domain.Case
{
    public class CaseNotification
    {
        public int ID { get; set; }

        public string UserID { get; set; }

        public string CaseID { get; set; }

        public string Content { get; set; }

        public string CreateUserName { get; set; }

        public DateTime CreateDateTime { get; set; }

        public CaseType CaseType { get; set; }

        public string ApplyUserID { get; set; }

        public string ApplyUserName { get; set; }

        public DateTime ApplyDateTime { get; set; }

        public CaseNotificationType CaseNotificationType { get; set; }
    }
}