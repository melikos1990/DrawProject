using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMARTII.Domain.Notification;
using System.ServiceModel.Channels;
using SMARTII.Domain.Notification.Email;

namespace SMARTII.Service.Notification.Factory
{
    public class EmailOffice365Factory: IEmailMailProtocolFactory
    {
        /// <summary>
        /// Office365協定登入方式
        /// </summary>
        /// <returns></returns>
        public List<(OfficialEmailEffectivePayload, OfficialEmailHistory)> LoginMailProtocol(OfficialEmailGroup data, Dictionary<string, OfficialEmailHistory> historyList, string NodeKey)
        {
            return new List<(OfficialEmailEffectivePayload, OfficialEmailHistory)>();
        }
    }
}
