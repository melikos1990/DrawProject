using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMARTII.Domain.Notification.Facebook
{
    public class FacebookPayload : IPayload
    {

        /// <summary>
        /// 推播訊息 基本格式: { text: "message" }
        /// </summary>
        public IDictionary<string, object> message { get; set; }

        // <summary>
        /// 推播對象 基本格式: { id: "userID" }
        /// </summary>
        public IDictionary<string, object> recipent { get; set; }

    }
}
