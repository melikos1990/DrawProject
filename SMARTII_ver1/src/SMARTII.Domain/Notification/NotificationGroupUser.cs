using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Notification
{
    public class NotificationGroupUser : ConcatableUser
    {
        /// <summary>
        /// 流水號
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 通知群組代號
        /// </summary>
        public int GroupID { get; set; }

        /// <summary>
        /// 所屬提醒群組
        /// </summary>
        public NotificationGroup NotificationGroup { get; set; }
    }
}
