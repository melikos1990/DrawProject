using System.Collections.Generic;
using System.Threading.Tasks;
using SMARTII.Domain.Case;

namespace SMARTII.Domain.Notification
{
    public interface INotificationGroupFacade
    {
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        Task Update(NotificationGroup group);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        Task Create(NotificationGroup group);
    }
}
