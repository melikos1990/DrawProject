using System.Collections.Generic;
using SMARTII.Domain.Organization;

namespace SMARTII.Socket
{
    /// <summary>
    /// 提供給 Client 做Signalr 訂閱
    /// ※ 該介面不會在AP實作
    /// </summary>
    public interface IClinetDelegate
    {
        void ShowMessage(string mesg);

        void RefrachNotificationCount();

        void CurrentLockUpUsers(KeyValuePair<string, List<User>> payload);
    }
}
