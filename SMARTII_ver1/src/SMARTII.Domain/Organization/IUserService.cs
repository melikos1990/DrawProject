using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMARTII.Domain.Organization
{
    public interface IUserService
    {
        /// <summary>
        /// 使用者新增
        /// </summary>
        /// <returns></returns>
        Task CreateAsync(User user, int[] roleIDs);

        /// <summary>
        /// 使用者更新
        /// </summary>
        /// <returns></returns>
        Task UpdateAsync(User user, int[] roleIDs);

        /// <summary>
        /// 使用者權限更新
        /// </summary>
        /// <param name="role"></param>
        /// <param name="userIDs"></param>
        /// <returns></returns>
        Task UpdateRoleAsync(Role role, string[] userIDs);

        /// <summary>
        /// 使用者權限新增
        /// </summary>
        /// <param name="role"></param>
        /// <param name="userIDs"></param>
        /// <returns></returns>
        Task CreateRoleAsync(Role role, string[] userIDs);

        /// <summary>
        /// 取得Excel報表
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<byte[]> GetReport(List<User> user, UserSearchCondition condition);
    }
}
