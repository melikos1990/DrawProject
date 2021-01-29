using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Areas.System.Models.SystemLog;
using SMARTII.Assist.Authentication;
using SMARTII.Assist.Logger;
using SMARTII.Assist.Web;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.System;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;

namespace SMARTII.Areas.System.Controllers
{
    [Authentication]
    public class SystemLogController : BaseApiController
    {
        private readonly ICommonAggregate _CommonAggregate;
        private readonly ISystemAggregate _SystemAggregate;

        public SystemLogController(ISystemAggregate SystemAggregate,
                                   ICommonAggregate CommonAggAggregate)
        {
            _SystemAggregate = SystemAggregate;
            _CommonAggregate = CommonAggAggregate;
        }

        /// <summary>
        /// 取得清單
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(SystemLog_lang.SYSTEM_LOG_GETLIST))]
        [ModelValidator(false)]
        public async Task<PagingResponse> GetList(PagingRequest<SystemLogSearchViewModel> model)
        {
            try
            {
                var searchTerm = model.criteria;

                var con = new MSSQLCondition<SYSTEM_LOG>(
                    searchTerm,
                    model.pageIndex,
                    model.pageSize
                    );

                con.OrderBy(model.sort, model.orderType);

                if (searchTerm.Operator.HasValue)
                    con.And(x => x.OPERATOR == (int)searchTerm.Operator);

                var list = _SystemAggregate.SystemLog_T1_T2_.GetPaging(con);

                var ui = list.Select(x => new SystemLogListViewModel(x));

                return await new PagingResponse<IEnumerable<SystemLogListViewModel>>(ui)
                {
                    isSuccess = true,
                    totalCount = list.TotalCount
                }.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(SystemLog_lang.SYSTEM_LOG_GETLIST_FAIL));

                return await new PagingResponse()
                {
                    isSuccess = false,
                    message = ex.PrefixMessage(SystemLog_lang.SYSTEM_LOG_GETLIST_FAIL)
                }.Async();
            }
        }
    }
}
