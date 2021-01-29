using System.Collections.Generic;
using Newtonsoft.Json;

namespace SMARTII.Domain.Notification.FCM
{
    public class FCMPPayload : ISenderPayload
    {
        /// <summary>
        /// User Registration Token (service work)
        /// </summary>
        public string token { get; set; }

        /// <summary>
        /// 額外資訊
        /// </summary>
        public IDictionary<string, string> data { get; set; }

        /// <summary>
        /// 推播物件
        /// </summary>
        public FCMNotification notification { get; set; }

        /// <summary>
        /// 推播平台 Web/Android/IOS
        /// </summary>
        public string platform { get; set; }

        public string toJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}