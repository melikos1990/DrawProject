using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Data;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Notification.UI;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;

namespace SMARTII.Areas.Case.Controllers
{
    public partial class CaseController
    {
        

        [HttpGet]
        public async Task<IHttpResult> JoinRoom(string caseID)
        {
            try
            {

                var currentUser = new User()
                {
                    Account = UserIdentity.Instance.Account,
                    Name = UserIdentity.Instance.Name,
                };

                KeyValueInstance<string, User>.Room.Add(caseID, currentUser);

                _NotificationAggregate.Providers[NotificationType.UI].Send(
                    new UIPayload<string>(caseID) {
                        ClientMethod = "CurrentLockUpUsers"
                    }
                );

                var result = new JsonResult<string>("", true);

                return await result.Async();
            }
            catch (Exception ex)
            {

                _CommonAggregate.Logger.Error(
                ex.PrefixDevMessage(""));

                return await new JsonResult(
                    ex.PrefixMessage(""), false)
                    .Async();
            }
        }

        [HttpGet]
        public async Task<IHttpResult> LeaveAllRoom(string sourceID)
        {
            try
            {

                var con = new MSSQLCondition<CASE>(x => x.SOURCE_ID == sourceID);

                var caseIDs = _CaseAggregate.Case_T1_T2_.GetListOfSpecific(con, x => x.CASE_ID);

                var currentUser = new User()
                {
                    Account = UserIdentity.Instance.Account,
                    Name = UserIdentity.Instance.Name
                };

                caseIDs?.ForEach(caseID =>
                {
                    KeyValueInstance<string, User>.Room.Remove(caseID, currentUser, x => x.Account != currentUser.Account);

                    _NotificationAggregate.Providers[NotificationType.UI].Send(
                        new UIPayload<string>(caseID)
                        {
                            ClientMethod = "CurrentLockUpUsers"
                        }
                    );
                });

                

                var result = new JsonResult<string>("", true);

                return await result.Async();
            }
            catch (Exception ex)
            {

                _CommonAggregate.Logger.Error(
                ex.PrefixDevMessage(""));

                return await new JsonResult(
                    ex.PrefixMessage(""), false)
                    .Async();
            }
        }

        [HttpGet]
        public async Task<IHttpResult> LeaveRoom(string caseID)
        {
            try
            {

                var currentUser = new User()
                {
                    Account = UserIdentity.Instance.Account,
                    Name = UserIdentity.Instance.Name
                };

                KeyValueInstance<string, User>.Room.Remove(caseID, currentUser, x => x.Account != currentUser.Account);

                _NotificationAggregate.Providers[NotificationType.UI].Send(
                    new UIPayload<string>(caseID)
                    {
                        ClientMethod = "CurrentLockUpUsers"
                    }
                );

                var result = new JsonResult<string>("", true);

                return await result.Async();
            }
            catch (Exception ex)
            {

                _CommonAggregate.Logger.Error(
                ex.PrefixDevMessage(""));

                return await new JsonResult(
                    ex.PrefixMessage(""), false)
                    .Async();
            }
        }

        [HttpGet]
        public async Task<IHttpResult> LeaveAll()
        {
            try
            {

                var joinedRooms = KeyValueInstance<string, User>.Room.UserJoinedRooms(x => x.Account == UserIdentity.Instance.Account);

                KeyValueInstance<string, User>.Room.Remove(x => x.Account != UserIdentity.Instance.Account);

                joinedRooms.ForEach(caseID =>
                {
                    _NotificationAggregate.Providers[NotificationType.UI].Send(
                        new UIPayload<string>(caseID)
                        {
                            ClientMethod = "CurrentLockUpUsers"
                        }
                    );
                });


                var result = new JsonResult<string>("", true);

                return await result.Async();
            }
            catch (Exception ex)
            {

                _CommonAggregate.Logger.Error(
                ex.PrefixDevMessage(""));

                return await new JsonResult(
                    ex.PrefixMessage(""), false)
                    .Async();
            }
        }
    }
}
