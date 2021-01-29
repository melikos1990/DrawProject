using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Newtonsoft.Json;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Master;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;
using SMARTII.Service.Notification.Base;
using SMARTII.Service.Notification.Provider;

namespace SMARTII.Service.Notification.Facade
{
    public class NotificationPersonalFacade : NotificationBase, INotificationPersonalFacade
    {

        private readonly IMasterAggregate _MasterAggregate;
        private readonly INotificationAggregate _NotificationAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;

        public NotificationPersonalFacade(
            IMasterAggregate MasterAggregate, 
            INotificationAggregate NotificationAggregate, 
            IOrganizationAggregate OrganizationAggregate): base(NotificationAggregate, OrganizationAggregate)
        {
            _MasterAggregate = MasterAggregate;
            _NotificationAggregate = NotificationAggregate;
            _OrganizationAggregate = OrganizationAggregate;
        }
        
        /// <summary>
        /// 布告欄寫入通知
        /// </summary>
        public void BillBoardNotification()
        {
            var now = DateTime.Now;
            var con = new MSSQLCondition<BILL_BOARD>();

            con.And(x => x.IS_NOTIFICATED == false && x.ACTIVE_DATE_START <= now && x.ACTIVE_DATE_END >= now);

            var billboards = _MasterAggregate.Billboard_T1_T2_.GetList(con).ToList();

            var personals = new List<PersonalNotification>();

            billboards.ForEach(billboard =>
            {
                var datas = billboard.UserIDs.Select(userId => new PersonalNotification()
                {
                    Content = billboard.Content,
                    UserID = userId,
                    Extend = JsonConvert.SerializeObject(billboard),
                    PersonalNotificationType = PersonalNotificationType.Billboard,
                    CreateDateTime = DateTime.Now,
                    CreateUserName = GlobalizationCache.APName
                });

                personals.AddRange(datas);
            });


            con.ClearFilters();

            con.ActionModify(x => x.IS_NOTIFICATED = true);
            
            billboards.ForEach(x => con.Or(g => g.ID == x.ID));

            using (TransactionScope scope = new TransactionScope())
            {

                _NotificationAggregate.PersonalNotification_T1_T2_.AddRange(personals);

                #region 更新佈告欄

                if(billboards.Count > 0)
                    _MasterAggregate.Billboard_T1_T2_.UpdateRange(con);

                #endregion

                scope.Complete();
            }



            #region 通知畫面 更新數字

            var userIDs = billboards.SelectMany(x => x.UserIDs).Distinct().ToList();

            base.RefrachNotificationCount(userIDs);

            #endregion

        }

        /// <summary>
        /// 通知使用者(UI)
        /// </summary>
        /// <param name="userId"></param>
        public void NotifyWeb(string userId)
        {
            base.RefrachNotificationCount(new List<string>() { userId });
        }

        /// <summary>
        /// 通知多個使用者(UI)
        /// </summary>
        /// <param name="userId"></param>

        public void NotifyWebCollection(List<string> userIds)
        {
            base.RefrachNotificationCount(userIds);
        }
    }
}
