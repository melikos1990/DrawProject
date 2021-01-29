using System.Collections.Generic;
using SMARTII.Domain.Organization;

namespace SMARTII.Domain.IO
{
    /// <summary>
    /// 該介面為 AP 端呼叫Client 端的邏輯
    /// </summary>
    public interface ISignalRConnection
    {
        void PushAllUser(string message);

        void RefrachNotificationCount(string userID);
        
        void CurrentLockUpUsers(string caseID, List<string> connIDs, List<User> lockUsers);

    }
}
