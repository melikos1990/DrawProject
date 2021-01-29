using SMARTII.Domain.Notification.Email;

namespace SMARTII.Domain.Notification
{
    public interface INotificationGroupService
    {
        /// <summary>
        /// 排程計算大量叫修數量
        /// </summary>
        void Calculate();

        /// <summary>
        /// 不通知
        /// </summary>
        /// <param name="groupID"></param>
        void NoSend(int groupID);

        /// <summary>
        /// 進行通知
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="payload"></param>
        void Send(int groupID, EmailPayload payload);
    }
}
