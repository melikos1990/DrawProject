using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Data;
using SMARTII.Domain.Thread;

namespace SMARTII.Areas.Common.Controllers
{
    public partial class OrganizationController
    {
        /// <summary>
        /// 取得操作權限清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetAppRoles")]
        public async Task<IHttpActionResult> GetAppRolesAsync(Select2Request model)
        {
            try
            {
                var con = new MSSQLCondition<ROLE>(
                  null,
                  model.start,
                  model.size
                  );

                if (string.IsNullOrEmpty(model.keyword) == false)
                {
                    con.And(x => x.NAME.Contains(model.keyword));
                }

                con.OrderBy(x => x.ID, OrderType.Asc);

                var result = _OrganizationAggregate.Role_T1_T2_.GetPaging(con).ToList();

                var select2 = new Select2Response()
                {
                    items = Select2Response.ToSelectItems(result, x => x.ID.ToString(), x => x.Name)
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