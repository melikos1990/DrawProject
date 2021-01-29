using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Notification.UI;
using SMARTII.Domain.Organization;

namespace SMARTII.Socket
{
    public class SignalRHub : Hub<IClinetDelegate>
    {

        private readonly INotificationAggregate _NotificationAggregate;

        public SignalRHub(INotificationAggregate NotificationAggregate)
        {
            _NotificationAggregate = NotificationAggregate;
        }

        public override Task OnConnected()
        {
            var account = Context.Request.QueryString["Account"];
            var conId = Context.ConnectionId;

            this.MappinUser(account, conId);

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var account = Context.Request.QueryString["Account"];
            var conId = Context.ConnectionId;


            var caseIDs = KeyValueInstance<string, User>.Room.UserJoinedRooms((user) => user.Account == account);

            KeyValueInstance<string, User>.Room.Remove( x => x.Account != account);
            
            if (caseIDs != null && caseIDs.Count > 0)
            {
                caseIDs.ForEach(caseID =>
                {
                    _NotificationAggregate.Providers[NotificationType.UI].Send(
                        new UIPayload<string>(caseID)
                        {
                            ClientMethod = "CurrentLockUpUsers"
                        }
                    );
                });
            }

            this.UnMappinUser(conId);

            return base.OnDisconnected(stopCalled);
        }

        public void MappinUser(string Account, string ConnectionId)
        {
            KeyValueInstance<string, string>.SignalRConnections.Add(Account, ConnectionId);
        }

        public void UnMappinUser(string conId)
        {
            KeyValueInstance<string, string>.SignalRConnections.Remove(conId);
        }
    }
}
