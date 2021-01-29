using System.Threading.Tasks;

namespace SMARTII.Domain.Organization
{
    public interface IUserFacade
    {
        /// <summary>
        /// 取得完整使用者資訊
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        Task<User> GetUserAuthFromAccountAsync(string account);

        /// <summary>
        /// 取得完整使用者資訊
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        Task<User> GetUserGroupFromIDAsync(string account);

        /// <summary>
        /// 刪除使用者圖片
        /// </summary>
        /// <param name="id"></param>
        /// <param name="key"></param>
        void DeleteImage(string id, string key);
    }
}
