using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMARTII.Domain.Notification
{
    public interface IOfficialEmailGroupFacade
    {
        /// <summary>
        /// 單一新增官網來信提醒
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task Create(Domain.Notification.OfficialEmailGroup data);
        /// <summary>
        /// 單一更新官網來信提醒
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task Update(Domain.Notification.OfficialEmailGroup data);
    }
}
