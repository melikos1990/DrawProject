using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Newtonsoft.Json;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Authentication;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Security;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;
using SMARTII.Domain.System;
using System.Collections.Generic;
using SMARTII.Domain.Report;

namespace SMARTII.Service.Organization.Service
{
    public class UserService : IUserService
    {
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly IAuthenticationAggregate _AuthenticationAggregate;
        private readonly ISystemAggregate _SystemAggregate;
        private readonly IUserReportProvider _UserReportProvider;

        public UserService(IOrganizationAggregate OrganizationAggregate,
                           IAuthenticationAggregate AuthenticationAggregate,
                           IIndex<OrganizationType, IOrganizationNodeProcessProvider> NodeProviders,
                           ISystemAggregate SystemAggregate,
                           IUserReportProvider UserReportProvider)
        {
            _OrganizationAggregate = OrganizationAggregate;
            _AuthenticationAggregate = AuthenticationAggregate;
            _SystemAggregate = SystemAggregate;
            _UserReportProvider = UserReportProvider;
        }

        /// <summary>
        /// 建立人員資訊
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleIDs"></param>
        /// <returns></returns>
        public async Task CreateAsync(User user, int[] roleIDs)
        {
            if (user.IsAD)
            {
                // 如果是AD 帳戶 , 就必須驗證合法性
                if (_AuthenticationAggregate.AD.IsADUser(user.Account) == false)
                    throw new Exception(Common_lang.AD_USER_NULL);
            }
            else
            {
                // 給予預設帳號
                // 使用者初次登入時需給予該組預設帳號進行設置
                //user.Password = SecurityCache.Instance.DefaultPassword.Md5Hash();
                user.Password = _SystemAggregate.SystemParameter_T1_T2_.Get(x => x.KEY == EssentialCache.UserPasswordKeyValue.USER_DEFAULTCODE && x.ID == EssentialCache.UserPasswordKeyValue.SYSTEM_SETTING).Value.Md5Hash();

                // 重設註記, 標註鎖定時間
                user.LastChangePasswordDateTime = null;
            }

            if (user.IsSystemUser && string.IsNullOrEmpty(user.Account))
                throw new Exception(Common_lang.USER_ACCOUNT_NULL);

            _OrganizationAggregate.User_T1_T2_.Operator(context =>
            {
                var db = (SMARTIIEntities)context;
                db.Configuration.LazyLoadingEnabled = false;

                if (user.IsSystemUser && db.USER.Any(x => x.ACCOUNT.ToLower() == user.Account.ToLower()))
                    throw new Exception(Common_lang.ACCOUNT_EXIST);

                var entity = AutoMapper.Mapper.Map<USER>(user);
                entity.CREATE_DATETIME = DateTime.Now;
                entity.CREATE_USERNAME = ContextUtility.GetUserIdentity()?.Name;
                entity.VERSION = DateTime.Now;

                // 找到使用者操作權限
                var rolesEntity = db.ROLE.Where(x => roleIDs.Contains(x.ID)).ToList();

                // 綁定人員
                entity.ROLE = rolesEntity;

                db.USER.Add(entity);

                db.SaveChanges();
            });
        }

        /// <summary>
        /// 更新人員資訊
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleIDs"></param>
        /// <returns></returns>
        public async Task UpdateAsync(User user, int[] roleIDs)
        {
            _OrganizationAggregate.User_T1_T2_.Operator(context =>
            {
                var db = (SMARTIIEntities)context;
                db.Configuration.LazyLoadingEnabled = false;

                var query = db.USER.Include("ROLE")
                                   .Where(x => x.USER_ID == user.UserID)
                                   .FirstOrDefault();

                if (user.IsSystemUser)
                {
                    if (string.IsNullOrEmpty(user.Account))
                        throw new Exception(Common_lang.USER_ACCOUNT_NULL);

                    if (db.USER.Any(x => x.USER_ID != user.UserID && x.ACCOUNT.ToLower() == user.Account.ToLower()))
                        throw new Exception(Common_lang.ACCOUNT_EXIST);

                    if (query.IS_AD == false && user.IsAD)
                    {
                        // 如果改為AD 帳戶 , 需確認有效性
                        if (_AuthenticationAggregate.AD.IsADUser(user.Account) == false)
                            throw new Exception(Common_lang.AD_USER_NULL);
                    }

                    if (query.IS_AD && user.IsAD == false)
                    {
                        // 如果原本是AD帳戶而被修正為非AD 帳戶
                        // 密碼重新配置為預設的 , 並且進行重設鎖定
                        //query.PASSWORD = SecurityCache.Instance.DefaultPassword.Md5Hash();
                        query.PASSWORD = _SystemAggregate.SystemParameter_T1_T2_.Get(x => x.KEY == EssentialCache.UserPasswordKeyValue.USER_DEFAULTCODE && x.ID == EssentialCache.UserPasswordKeyValue.SYSTEM_SETTING).Value.Md5Hash();

                        // 重設註記 , 將最後修改時間重置
                        query.LAST_CHANGE_PASSWORD_DATETIME = null;
                    }
                }

                query.ACCOUNT = user.Account;
                query.IS_AD = user.IsAD;
                query.NAME = user.Name;
                query.TELEPHONE = user.Telephone;
                query.MOBILE = user.Mobile;
                query.VERSION = DateTime.Now;
                query.UPDATE_DATETIME = DateTime.Now;
                query.UPDATE_USERNAME = ContextUtility.GetUserIdentity()?.Name;
                query.IS_ENABLED = user.IsEnabled;
                query.FEATURE = JsonConvert.SerializeObject(user.Feature);
                query.ACTIVE_START_DATETIME = user.ActiveStartDateTime;
                query.ACTIVE_END_DATETIME = user.ActiveEndDateTime;
                query.EXT = user.Ext;
                query.EMAIL = user.Email;
                query.IS_SYSTEM_USER = user.IsSystemUser;

                // 找到使用者操作權限
                var rolesEntity = db.ROLE.Where(x => roleIDs.Contains(x.ID)).ToList();

                // 綁定人員
                query.ROLE = rolesEntity;
 
                #region 修正相關table資訊(非關連類)

                //提醒群組成員
                var notificationGroupUsers = db.NOTIFICATION_GROUP_USER.Where(x => x.USER_ID == user.UserID).ToList();
                notificationGroupUsers.ForEach(x =>
                {
                    x.USER_NAME = user.Name;
                    x.TELEPHONE = user.Telephone;
                    x.MOBILE = user.Mobile;
                    x.ADDRESS = user.Address;
                    x.EMAIL = user.Email;
                });

                //派工群組成員
                var caseAssignGroupUsers = db.CASE_ASSIGN_GROUP_USER.Where(x => x.USER_ID == user.UserID).ToList();
                caseAssignGroupUsers.ForEach(x =>
                {
                    x.USER_NAME = user.Name;
                    x.TELEPHONE = user.Telephone;
                    x.MOBILE = user.Mobile;
                    x.ADDRESS = user.Address;
                    x.EMAIL = user.Email;
                });

                #endregion

                db.SaveChanges();
            });
        }

        /// <summary>
        /// 建立功能權限
        /// </summary>
        /// <param name="role"></param>
        /// <param name="userIDs"></param>
        /// <returns></returns>
        public async Task CreateRoleAsync(Role role, string[] userIDs)
        {
            var checkName = _OrganizationAggregate.Role_T1_T2_.HasAny(x => x.NAME == role.Name);

            if (checkName)
                throw new Exception(Role_lang.ROLE_NAME_REPEAT);

            _OrganizationAggregate.Role_T1_.Operator(context =>
            {
                var db = (SMARTIIEntities)context;
                db.Configuration.LazyLoadingEnabled = false;

                var entity = AutoMapper.Mapper.Map<ROLE>(role);
                entity.CREATE_DATETIME = DateTime.Now;
                entity.CREATE_USERNAME = ContextUtility.GetUserIdentity()?.Name;

                var usersEntity = db.USER.Where(x => userIDs.Contains(x.USER_ID)).ToList();

                entity.USER = usersEntity;

                db.ROLE.Add(entity);

                db.SaveChanges();
            });
        }

        /// <summary>
        /// 更新功能權限
        /// </summary>
        /// <param name="role"></param>
        /// <param name="userIDs"></param>
        /// <returns></returns>
        public async Task UpdateRoleAsync(Role role, string[] userIDs)
        {
            var checkName = _OrganizationAggregate.Role_T1_T2_.HasAny(x => x.ID != role.ID && x.NAME == role.Name);

            if (checkName)
                throw new Exception(Role_lang.ROLE_NAME_REPEAT);

            _OrganizationAggregate.Role_T1_.Operator(context =>
            {
                var db = (SMARTIIEntities)context;
                db.Configuration.LazyLoadingEnabled = false;

                var query = db.ROLE.Include("USER")
                                   .Where(x => x.ID == role.ID)
                                   .FirstOrDefault();

                if (query == null)
                    throw new Exception(Common_lang.NOT_FOUND_DATA);

                query.FEATURE = JsonConvert.SerializeObject(role.Feature);
                query.IS_ENABLED = role.IsEnabled;
                query.NAME = role.Name;
                query.UPDATE_DATETIME = DateTime.Now;
                query.UPDATE_USERNAME = ContextUtility.GetUserIdentity()?.Name;

                var usersEntity = db.USER.Where(x => userIDs.Contains(x.USER_ID)).ToList();

                query.USER = usersEntity;

                db.SaveChanges();
            });
        }

        public async Task<byte[]> GetReport(List<User> user, UserSearchCondition condition)
        {
            byte[] @byte = await _UserReportProvider.GetAuthorityReport(condition ,user);
            
            return @byte;
        }
    }
}
