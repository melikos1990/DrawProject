using System;

namespace SMARTII.Domain.Notification
{
    public interface INotificationProvider
    {
        /// <summary>
        /// 推送訊息
        /// </summary>
        /// <param name=""></param>
        void Send(ISenderPayload payload, Action<object> beforeSend = null, Action<object> afterSend = null);
    }
}