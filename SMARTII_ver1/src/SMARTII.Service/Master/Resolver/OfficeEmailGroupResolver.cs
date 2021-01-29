using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMARTII.Domain.Master;
using SMARTII.Domain.Notification;
using MoreLinq;
using SMARTII.Domain.Notification.Email;

namespace SMARTII.Service.Master.Resolver
{
    public class OfficeEmailGroupResolver
    {
        private readonly INotificationAggregate _NotificationAggregate;

        public OfficeEmailGroupResolver(INotificationAggregate NotificationAggregate)
        {
            _NotificationAggregate = NotificationAggregate;
        }
        public IEnumerable<T> ResolveCollection<T>(IEnumerable<T> data) where T : IOfficialEailGroupRelationship, new()
        {

            IDictionary<string, OfficialEmailGroup> dist = new Dictionary<string, OfficialEmailGroup>();

            var group = data.GroupBy(x => new
            {
                ID = x.Email_Group_ID
            });

            group.ForEach(pair =>
            {
                var officialEmailGroup = _NotificationAggregate.OfficialEmailGroup_T1_T2_
                                                                        .Get(x => x.ID == pair.Key.ID);

                dist.Add($"{pair.Key.ID}", officialEmailGroup);
            });

            data.ForEach(item =>
            {

                var officialEmailGroup = dist[$"{item.Email_Group_ID}"];
                item.Account = officialEmailGroup?.Account;
                item.BuMailAccount = officialEmailGroup?.MailAddress;
                item.MailDisplayName = officialEmailGroup.MailDisplayName;
            });


            return data;
        }
    }
}
