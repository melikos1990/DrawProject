using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMARTII.Domain.Common;
using SMARTII.Domain.Notification;

namespace SMARTII.Service.Notification.Service
{
    public class NotificationPersonalService : INotificationPersonalService
    {
        
        private readonly INotificationPersonalFacade _NotificationPersonalFacade;
        private readonly ICommonAggregate _CommonAggregate;

        public NotificationPersonalService(INotificationPersonalFacade NotificationPersonalFacade, ICommonAggregate CommonAggregate)
        {
            _NotificationPersonalFacade = NotificationPersonalFacade;
            _CommonAggregate = CommonAggregate;
        }


        /// <summary>
        /// [Batch] 佈告欄提醒通知
        /// </summary>
        public void BillBoardNotification()
        {
            _CommonAggregate.Logger.Info("[佈告欄提醒] 開始執行");
            try
            {
                
                _NotificationPersonalFacade.BillBoardNotification();


                _CommonAggregate.Logger.Info("[佈告欄提醒] 執行完成");
            }
            catch (Exception ex)
            {

                _CommonAggregate.Logger.Error("[佈告欄提醒] 執行錯誤");
                _CommonAggregate.Logger.Error(ex.Message);

            }
        }
    }
}
