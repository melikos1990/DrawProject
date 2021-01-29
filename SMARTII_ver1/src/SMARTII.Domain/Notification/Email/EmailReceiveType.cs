using System.ComponentModel;

namespace SMARTII.Domain.Notification.Email
{
    public enum EmailReceiveType
    {
        /// <summary>
        /// 收件人
        /// </summary>
        [Description("收件人")]
        Recipient,

        /// <summary>
        /// CC
        /// </summary>
        [Description("副本")]
        CC,

        /// <summary>
        /// 密件副本
        /// </summary>
        [Description("密件副本")]
        BCC
    }
}
