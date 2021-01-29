using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMARTII.Domain.Notification
{
    public interface INotificationPersonalFacade
    {
        void BillBoardNotification();

        void NotifyWeb(string userId);
        
        void NotifyWebCollection(List<string> userIds);

    }
}
