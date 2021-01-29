using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Organization;
using SMARTII.Service.Notification.Provider;

namespace SMARTII.Service.Notification.Base
{
    public class NotificationBase
    {
        private readonly INotificationAggregate _NotificationAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly SignalRProvider _SignalRProvider;

        public NotificationBase(
            INotificationAggregate NotificationAggregate, 
            IOrganizationAggregate OrganizationAggregate)
        {
            _NotificationAggregate = NotificationAggregate;
            _OrganizationAggregate = OrganizationAggregate;
            _SignalRProvider = (SignalRProvider)_NotificationAggregate.Providers[NotificationType.UI];
        }


        public void RefrachNotificationCount(List<string> userIDs)
        {
            var con = new MSSQLCondition<USER>(x => userIDs.Contains(x.USER_ID));

            var accounts = _OrganizationAggregate.User_T1_T2_.GetListOfSpecific(con, x => x.ACCOUNT);
            
            accounts.ForEach(account => _SignalRProvider.RefrachNotificationCount(account));

        }
    }
}
