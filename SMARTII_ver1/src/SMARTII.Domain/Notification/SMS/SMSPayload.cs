using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMARTII.Domain.Notification.SMS
{
    public class SMSPayload : ISenderPayload
    {
        /// <summary>
        /// 簡訊內容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 對象電話
        /// </summary>
        public List<string> PhoneNumbers { get; set; }

       
    }
}
