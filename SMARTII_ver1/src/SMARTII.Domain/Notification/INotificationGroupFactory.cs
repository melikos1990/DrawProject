using System.Collections.Generic;

namespace SMARTII.Domain.Notification
{
    public interface INotificationGroupFactory
    {
        void Execute(IEnumerable<Case.Case> cases, NotificationGroup group);
    }
}