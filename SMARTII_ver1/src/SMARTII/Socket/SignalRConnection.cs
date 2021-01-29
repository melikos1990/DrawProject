using System.Collections.Generic;
using System.Linq;
using SMARTII.Domain.Cache;
using SMARTII.Domain.IO;
using SMARTII.Domain.Organization;

namespace SMARTII.Socket
{
    public class SignalRConnection : ISignalRConnection
    {
        private readonly SignalRHub _hub;

        public SignalRConnection(SignalRHub hub)
        {
            this._hub = hub;
        }

        public void PushAllUser(string mesg)
        {
            if (_hub.Context != null)
                _hub.Clients.All.ShowMessage(mesg);
        }

        public void CurrentLockUpUsers(string caseID, List<string> accounts, List<User> lockUsers)
        {
            if (_hub.Context != null)
            {
                accounts.ForEach(account =>
                {

                    var conids = KeyValueInstance<string, string>.SignalRConnections.GetConnections(account).ToList();

                    conids.ForEach(conid => _hub.Clients.Client(conid).CurrentLockUpUsers(new KeyValuePair<string, List<User>>(caseID, lockUsers)));
                    
                });
                
            }
        }

        public void RefrachNotificationCount(string account)
        {
            if (_hub.Context != null)
            {
                var conid = KeyValueInstance<string, string>.SignalRConnections.GetConnections(account).FirstOrDefault();

                if(string.IsNullOrEmpty(conid) == false)  _hub.Clients.Client(conid).RefrachNotificationCount();
            }
        }
    }
}
