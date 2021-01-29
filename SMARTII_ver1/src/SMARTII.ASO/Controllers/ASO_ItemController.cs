using System;
using System.Threading.Tasks;
using System.Web.Http;
using SMARTII.Assist.Authentication;
using SMARTII.Assist.Web;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Thread;

namespace SMARTII.ASO.Controllers
{
    [Authentication]
    [RoutePrefix("Api/ASO/Item")]
    public class ASO_ItemController : BaseApiController
    {
        private readonly ICommonAggregate _CommonAggregate;

        public ASO_ItemController(ICommonAggregate CommonAggregate)
        {
            this._CommonAggregate = CommonAggregate;
        }

        [Route("GetList")]
        [HttpPost]
        public async Task<PagingResponse> GetList(PagingRequest<dynamic> model)
        {
            try
            {
                //var buIDs = UserIdentity.Instance.DownProviderBUDist.TryGetBuList(Domain.Organization.OrganizationType.CallCenter);

                //var con = new MSSQLCondition<ITEM>();

                //var searchTerm = model.criteria;

                //var con = new MSSQLCondition<ITEM>(
                //    searchTerm,
                //    model.pageIndex,
                //    model.pageSize
                //    );

                ////若無指定BU查詢，將會撈出該人員負責之BU
                //if (searchTerm.BuID == null)
                //{
                //    con.And(x => buIDs.Contains(x.NODE_ID));
                //}

                //con.OrderBy(model.sort, model.orderType);

                //var list = _ItemFactory.GetPaging(con);

                //var ui = list.Select(x => new SystemLogListViewModel(x));

                //return await new PagingResponse<IEnumerable<SystemLogListViewModel>>(ui)
                //{
                //    isSuccess = true,
                //    totalCount = list.TotalCount
                //}.Async();

                return null;
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error("error");

                return await new PagingResponse()
                {
                    isSuccess = false,
                    message = ex.PrefixMessage("success")
                }.Async();
            }
        }
    }
}
