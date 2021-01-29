using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using SMARTII.Domain.Notification.Email;

namespace SMARTII.Domain.Notification
{
    public interface IEmailMailProtocolFactory
    {
        /// <summary>
        /// 協定登入方式
        /// </summary>
        /// <returns></returns>
        List<(OfficialEmailEffectivePayload, OfficialEmailHistory)> LoginMailProtocol(OfficialEmailGroup data, Dictionary<string, OfficialEmailHistory> historyList, string NodeKey);
    }
}
