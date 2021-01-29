using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;

namespace SMARTII.Service.Notification.Facade
{
    public class OfficialEmailGroupFacade : IOfficialEmailGroupFacade
    {

        private readonly INotificationAggregate _NotificationAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;

        public OfficialEmailGroupFacade(INotificationAggregate NotificationAggregate, IOrganizationAggregate OrganizationAggregate)
        {
            _NotificationAggregate = NotificationAggregate;
            _OrganizationAggregate = OrganizationAggregate;
        }

        /// <summary>
        /// 單一新增官網來信提醒
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task Create(Domain.Notification.OfficialEmailGroup data)
        {

            #region 驗證信箱

            // 驗證收件信箱是否存在
            var isExitMailAddress = _NotificationAggregate.OfficialEmailGroup_T1_.HasAny(x => x.ACCOUNT == data.Account);
            if (isExitMailAddress)
                throw new Exception(OfficialEmailGroup_lang.OFFICIAL_EMAIL_GROUP_DUPLICATE_EMAIL);

            // 驗證使用者是否有未啟用的
            var userEnableExep = UserUtility.ValidExpressionByER(data.User.Select(x => x.UserID).ToList());
            var allUserEnable = _OrganizationAggregate.User_T1_T2_.HasAny(userEnableExep);
            if (allUserEnable == false)
                throw new Exception(OfficialEmailGroup_lang.OFFICIAL_EMAIL_GROUP_USER_NOT_ISENABLED);

            #endregion


            _NotificationAggregate.OfficialEmailGroup_T1_T2_.Operator(x =>
            {
                var context = (SMARTIIEntities)x;

                var query = context.OFFICIAL_EMAIL_GROUP;

                var ef = new OFFICIAL_EMAIL_GROUP()
                {
                    NODE_ID = data.NodeID,
                    ORGANIZATION_TYPE = (byte)data.OrganizationType,
                    MAIL_ADDRESS = data.MailAddress,
                    ACCOUNT = data.Account,
                    PASSWORD = data.Password,
                    CREATE_DATETIME = DateTime.Now,
                    CREATE_USERNAME = ContextUtility.GetUserIdentity().Name,
                    KEEP_DAY = data.KeepDay,
                    PROTOCOL = (byte)data.MailProtocolType,
                    HOSTNAME = data.HostName,
                    IS_ENABLED = data.IsEnabled,
                    ALLOW_RECEIVE = data.AllowReceive,
                    OFFICIAL_EMAIL = data.OfficialEmail,
                    MAIL_DISPLAY_NAME = data.MailDisplayName
                };
                var userList = data.User.Select(y => y.UserID);
                //需先尋找實體User
                var existuUserList = context.USER
                                           .Where(g => userList.Contains(g.USER_ID))
                                           .ToList();

                ef.USER = existuUserList;

                context.OFFICIAL_EMAIL_GROUP.Add(ef);

                context.SaveChanges();
            });
        }
        /// <summary>
        /// 單一更新官網來信提醒
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task Update(Domain.Notification.OfficialEmailGroup data)
        {
            #region 驗證信箱
            // 驗證收件信箱是否存在
            var isExitMailAddress = _NotificationAggregate.OfficialEmailGroup_T1_.HasAny(x =>x.ID != data.ID && x.ACCOUNT == data.Account);
            if (isExitMailAddress)
                throw new Exception(OfficialEmailGroup_lang.OFFICIAL_EMAIL_GROUP_DUPLICATE_EMAIL);

            // 驗證使用者是否有未啟用的
            var userEnableExep = UserUtility.ValidExpressionByER(data.User.Select(x => x.UserID).ToList());
            var allUserEnable = _OrganizationAggregate.User_T1_T2_.HasAny(userEnableExep);
            if (allUserEnable == false)
                throw new Exception(OfficialEmailGroup_lang.OFFICIAL_EMAIL_GROUP_USER_NOT_ISENABLED);

            #endregion
            _NotificationAggregate.OfficialEmailGroup_T1_T2_.Operator(x =>
            {
                var context = (SMARTIIEntities)x;
                context.Configuration.LazyLoadingEnabled = false;

                var entity = context.OFFICIAL_EMAIL_GROUP.Include("USER").First(g => g.ID == data.ID);

                entity.UPDATE_DATETIME = DateTime.Now;
                entity.UPDATE_USERNAME = ContextUtility.GetUserIdentity().Name;
                entity.MAIL_ADDRESS = data.MailAddress;
                entity.ACCOUNT = data.Account;
                entity.PASSWORD = data.Password;
                entity.KEEP_DAY = data.KeepDay;
                entity.PROTOCOL = (byte)data.MailProtocolType;
                entity.HOSTNAME = data.HostName;
                entity.IS_ENABLED = data.IsEnabled;
                entity.ALLOW_RECEIVE = data.AllowReceive;
                entity.OFFICIAL_EMAIL = data.OfficialEmail;
                entity.MAIL_DISPLAY_NAME = data.MailDisplayName;


                var userList = data.User.Select(y => y.UserID).ToArray();

                var users = context.USER.Where(g => userList.Contains(g.USER_ID)).ToList();
                entity.USER = users;




                context.SaveChanges();
            });
        }
    }
}
