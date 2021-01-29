using System.Threading.Tasks;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Authentication.Service
{
    public interface IUserAuthenticationManager
    {
        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<User> LoginAsync(string account, string password, WebType type);

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        Task LogoutAsync();

        /// <summary>
        /// 使用者重新更換密碼
        /// </summary>
        /// <param name="account"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        Task ResetPasswordAsync(string account, string oldPassword, string newPassword);

        /// <summary>
        /// 重置密碼
        /// </summary>
        /// <param name="account"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        Task ResetDefaultPasswordAsync(string account);
    }
}
