using System;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Newtonsoft.Json;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Authentication;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Authentication.Service;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Security;
using SMARTII.Domain.System;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;

namespace SMARTII.Assist.Authentication
{
    public class UserAuthenticationManager : IUserAuthenticationManager
    {
        private readonly IUserFacade _UserFacade;
        private readonly IIndex<UserType, IAccountFactory> _AccountFactorties;
        private readonly IAuthenticationAggregate _AuthenticationAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly ISystemAggregate _SystemAggregate;

        public UserAuthenticationManager(IUserFacade UserFacade,
                                         IIndex<UserType, IAccountFactory> AccountFactorties,
                                         IOrganizationAggregate OrganizationAggregate,
                                         IAuthenticationAggregate AuthenticationAggregate,
                                         ISystemAggregate SystemAggregate)

        {
            _UserFacade = UserFacade;
            _AccountFactorties = AccountFactorties;
            _AuthenticationAggregate = AuthenticationAggregate;
            _OrganizationAggregate = OrganizationAggregate;
            _SystemAggregate = SystemAggregate;
        }

        /// <summary>
        /// 使用者登入
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<User> LoginAsync(string account, string password, WebType webType)
        {
            var con = new MSSQLCondition<USER>(x => x.ACCOUNT == account);

            var existUser = _OrganizationAggregate.User_T1_T2_.GetFirstOrDefault(con);

            var canExtranetLogin = _SystemAggregate.SystemParameter_T1_T2_.GetOfSpecific
                (new MSSQLCondition<SYSTEM_PARAMETER>(x => x.ID == EssentialCache.LoginValue.SYSTEM_SETTING && x.KEY == EssentialCache.LoginValue.OUTSIDE_AD_ALLOW), 
                x => x.VALUE);
            

            if (existUser == null)
                throw new Exception(Common_lang.USER_LOGIN_ERROR);
            
            if(existUser.IsSystemUser == false)
                throw new Exception(Common_lang.NOT_SYSTEM_USER);
            
            if (existUser.IsEnabled == false)
                throw new Exception(Common_lang.USER_DISABLED);

            if (existUser.LockoutDateTime.HasValue)
                throw new Exception(Common_lang.USER_LOCK);

            if (existUser.ActiveStartDateTime.HasValue == false || existUser.ActiveEndDateTime.HasValue == false)
                throw new Exception(Common_lang.USER_ACTIVEDATE_ERROR);

            if (DateTime.Now < existUser.ActiveStartDateTime || DateTime.Now > existUser.ActiveEndDateTime)
                throw new Exception(Common_lang.USER_UNACTIVE);


            var type = existUser.IsAD ? UserType.AD : UserType.System;

            // 判斷 目前系統可否讓 外部網站用 AD 登入
            if (existUser.IsAD && webType == WebType.Extranet && Convert.ToBoolean(int.Parse(canExtranetLogin)) == false)
                throw new Exception(Common_lang.NOT_ALLOW_EXTRANET_LOGIN);

            _AccountFactorties[type].Login(existUser, password);

            var user = await _UserFacade.GetUserAuthFromAccountAsync(account);

            return await user.Async();
        }

        /// <summary>
        /// 使用者登出
        /// </summary>
        /// <returns></returns>
        public async Task LogoutAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 使用者變更密碼
        /// </summary>
        /// <param name="account"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public async Task ResetPasswordAsync(string account, string oldPassword, string newPassword)
        {
            var con = new MSSQLCondition<USER>(x => x.ACCOUNT == account);
            User user = _OrganizationAggregate.User_T1_T2_.Get(con);

            var defaultPass = _SystemAggregate.SystemParameter_T1_T2_.Get(x =>
                        x.KEY == EssentialCache.UserPasswordKeyValue.USER_DEFAULTCODE &&
                        x.ID == EssentialCache.UserPasswordKeyValue.SYSTEM_SETTING).Value;

            if (user.IsAD)
                throw new Exception(Common_lang.AD_DENY_CHANGE_PASSWORD);
            if (String.Equals(newPassword.ToLower(), account.ToLower()))
                throw new Exception(Common_lang.NEWPASSWORD_EQUAL_ACCOUNT);

            if (String.Equals(newPassword, oldPassword))
                throw new Exception(Common_lang.NEWPASSWORD_EQUAL_OLDPASSWORD);

            if (String.Equals(user.Password, oldPassword.Md5Hash()) == false)
                throw new Exception(Common_lang.OLDPASSWORD_ERROR);

            if (user.PastPasswordQueue.HasAny(newPassword.Md5Hash()))
                throw new Exception(Common_lang.NEWPASSWORD_INSIDE_FIVE_GROUPS);

            if (String.Equals(defaultPass, oldPassword) == false)
                user.PastPasswordQueue.Insert(oldPassword.Md5Hash());

            con.ActionModify(x =>
            {
                x.PASSWORD = newPassword.Md5Hash();
                x.LAST_CHANGE_PASSWORD_DATETIME = DateTime.Now;
                x.LOCKOUT_DATETIME = null;
                x.UPDATE_DATETIME = DateTime.Now;
                x.UPDATE_USERNAME = GlobalizationCache.APName;
                x.PAST_PASSWORD_RECORD = JsonConvert.SerializeObject(user.PastPasswordQueue.ToArray());
            });

            _OrganizationAggregate.User_T1_T2_.Update(con);
        }

        /// <summary>
        /// 重置密碼
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task ResetDefaultPasswordAsync(string account)
        {
            var con = new MSSQLCondition<USER>(x => x.ACCOUNT == account);

            User user = _OrganizationAggregate.User_T1_T2_.Get(con);

            var defaultPass = _SystemAggregate.SystemParameter_T1_T2_.Get(x => 
                        x.KEY == EssentialCache.UserPasswordKeyValue.USER_DEFAULTCODE && 
                        x.ID == EssentialCache.UserPasswordKeyValue.SYSTEM_SETTING).Value;

            //  系統管理者重新設定密碼 , 原先密碼需要先放入歷程堆疊中。
            if (String.Equals(defaultPass, user.Password) == false)
                user.PastPasswordQueue.Insert(user.Password);

            con.ActionModify(x =>
            {
                x.PAST_PASSWORD_RECORD = JsonConvert.SerializeObject(user.PastPasswordQueue.ToArray());
                x.PASSWORD = _SystemAggregate.SystemParameter_T1_T2_.Get(y => y.KEY == EssentialCache.UserPasswordKeyValue.USER_DEFAULTCODE && y.ID == EssentialCache.UserPasswordKeyValue.SYSTEM_SETTING).Value.Md5Hash();
                x.LAST_CHANGE_PASSWORD_DATETIME = null;
                x.LOCKOUT_DATETIME = null;
                x.UPDATE_DATETIME = DateTime.Now;
                x.UPDATE_USERNAME = ContextUtility.GetUserIdentity()?.Name;
            });

            _OrganizationAggregate.User_T1_T2_.Update(con);
        }
    }
}
