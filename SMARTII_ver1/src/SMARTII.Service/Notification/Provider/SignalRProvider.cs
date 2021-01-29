using System;
using System.Linq;
using System.Reflection;
using SMARTII.Domain.Cache;
using SMARTII.Domain.IO;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Notification.UI;
using SMARTII.Domain.Organization;

namespace SMARTII.Service.Notification.Provider
{
    public class SignalRProvider : INotificationProvider
    {
        private readonly ISignalRConnection _Connection;

        public SignalRProvider(ISignalRConnection Connection)
        {
            _Connection = Connection;
        }

        public void Send(ISenderPayload payload, Action<object> beforeSend = null, Action<object> afterSend = null)
        {
            // cast payload
            var _payload = (UIPayload)payload;

            var method = this.GetType().GetMethod(_payload.ClientMethod);

            method.Invoke(this,
                        BindingFlags.Public | BindingFlags.InvokeMethod,
                        null,
                        new object[] { payload },
                        null);

            //var c = KeyValueInstance<string, string>.SignalRConnections.GetConnections();

            //this._Connection.PushAllUser("push");
        }



        /// <summary>
        /// 重整系統通知數
        /// </summary>
        /// <param name=""></param>
        public void RefrachNotificationCount(string userID)
        {
           
            this._Connection.RefrachNotificationCount(userID);
        }


        public void CurrentLockUpUsers(ISenderPayload payload)
        {
            var _payload = (UIPayload<string>)payload;

            var lockUsers = KeyValueInstance<string, User>.Room.GetLookupUsers(_payload.data).ToList();
            var accounts = lockUsers.Select(x => x.Account).ToList();

            this._Connection.CurrentLockUpUsers(_payload.data, accounts, lockUsers);

        }

    }
}
