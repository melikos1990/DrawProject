using System.Threading.Tasks;
using SMARTII.Domain.Case;

namespace SMARTII.Domain.Notification
{
    public interface ICaseAssignGroupFacade
    {
        /// <summary>
        /// 更新-派工群組設定
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        Task Update(CaseAssignGroup group);

        /// <summary>
        /// 新增-派工群組設定
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        Task Create(CaseAssignGroup group);
    }
}