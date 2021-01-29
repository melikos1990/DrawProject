using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Areas.Organization.Models.User;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Data;
using SMARTII.Domain.Thread;

namespace SMARTII.Areas.Common.Controllers
{
    public partial class OrganizationController
    {
        /// <summary>
        /// 取得使用者清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetAppUsers")]
        public async Task<IHttpActionResult> GetAppUsersAsync(Select2Request model)
        {
            try
            {
                var con = new MSSQLCondition<USER>(
                  null,
                  model.start,
                  model.size
                  );

                con.OrderBy(x => x.USER_ID, OrderType.Asc);

                if (string.IsNullOrEmpty(model.keyword) == false)
                {
                    con.And(x => x.NAME.Contains(model.keyword));
                }

                var result = _OrganizationAggregate.User_T1_T2_.GetPaging(con).ToList();

                var select2 = new Select2Response<UserListViewModel>()
                {
                    items = Select2Response<UserListViewModel>.ToSelectItems(result, x => x.UserID, x => $"{x.Name}({x.Account})", x => new UserListViewModel(x))
                };

                return Ok(select2);
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return await StatusCode(HttpStatusCode.InternalServerError).Async();
            }
        }
    }
}
