using System.ComponentModel;

namespace SMARTII.Domain.Notification
{
    public enum MailProtocolType
    {
        /// <summary>
        /// POP3
        /// </summary>
        [Description("POP3")]
        POP3 = 0,

        /// <summary>
        /// OFFICE365
        /// </summary>
        [Description("OFFICE365")]
        OFFICE365 = 1
    }
}
