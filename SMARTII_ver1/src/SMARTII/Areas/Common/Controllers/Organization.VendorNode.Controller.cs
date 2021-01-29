using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using MoreLinq;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Areas.Common.Models.Organization;
using SMARTII.Areas.Organization.Models.OrganizarionNodeDefinition;
using SMARTII.Areas.Organization.Models.User;
using SMARTII.Areas.Organization.Models.VendorNode;
using SMARTII.Areas.Select.Models.Organization;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;

namespace SMARTII.Areas.Common.Controllers
{
    public partial class OrganizationController
    {
        /// <summary>
        /// 取得廠商組織樹
        /// </summary>
        /// <param name="isSelf"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetVendorNodeTree")]
        public async Task<IHttpActionResult> GetVendorNodeTreeAsync(OrganizationDataRangeSearchViewModel model)
        {
            try
            {
                var con = new MSSQLCondition<VENDOR_NODE>();
                con.IncludeBy(x => x.VENDOR_NODE2);

                if (model.IsSelf)
                    con.FilterVendorTreeNodeRange(model.Goal);

                //if (model.NodeID.HasValue)
                //{
                //    if (model.IsStretch)
                //    {
                //        con.FilterSpecifyNodeField(x => x.NODE_ID == model.NodeID, PredicateType.Or);
                //        con.FilterSpecifyNodeField(x => x.PARENT_PATH.Contains(model.NodeID.ToString()), PredicateType.Or);
                //    }
                //    else
                //    {
                //        con.FilterSpecifyNodeField(x => x.NODE_ID == model.NodeID);
                //    }
                //}

                if (!string.IsNullOrEmpty(model.DefKey))
                    con.FilterSpecifyNodeField(x => x.NODE_TYPE_KEY == model.DefKey);

                con.IncludeBy(x => x.ORGANIZATION_NODE_DEFINITION);

                var nodes = _OrganizationAggregate.VendorNode_T1_IOrganizationNode_.GetList(con);

                var nested = (VendorNode)nodes.AsNestedNSM();

                var result = new JsonResult<VendorNodeViewModel>(
                    new VendorNodeViewModel(nested), true);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return await StatusCode(HttpStatusCode.InternalServerError).Async();
            }
        }

        /// <summary>
        /// 取得廠商組織樹(查詢用)
        /// </summary>
        /// <param name="isSelf"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetVendorNodeTreeForSearch")]
        public async Task<IHttpActionResult> GetVendorNodeTreeForSearchAsync(OrganizationDataRangeSearchViewModel model)
        {
            try
            {
                var con = new MSSQLCondition<VENDOR_NODE>();
                con.And(x => x.HEADQUARTERS_NODE.Any(y => y.NODE_ID == model.NodeID));
                con.IncludeBy(x => x.HEADQUARTERS_NODE);

                var nodes = _OrganizationAggregate.VendorNode_T1_T2_.GetList(con);

                var vCon = new MSSQLCondition<VENDOR_NODE>();

                vCon.IncludeBy(x => x.VENDOR_NODE2);
                vCon.IncludeBy(x => x.ORGANIZATION_NODE_DEFINITION);

                nodes.ForEach(node =>
                {
                    vCon.Or(x => x.LEFT_BOUNDARY <= node.LeftBoundary &&
                            x.RIGHT_BOUNDARY >= node.RightBoundary);
                });

                vCon.Or(x => x.LEFT_BOUNDARY == 1);

                var allNodes = _OrganizationAggregate.VendorNode_T1_T2_.GetList(vCon);

                var nested = allNodes.AsNestedNSM();                

                var result = new JsonResult<VendorNodeViewModel>(
                    new VendorNodeViewModel(nested), true);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return await StatusCode(HttpStatusCode.InternalServerError).Async();
            }
        }

        /// <summary>
        /// 取得廠商根節點
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetVendorRootNodes")]
        public async Task<IHttpActionResult> GetVendorRootNodesAsync(bool isSelf = true)
        {
            try
            {
                var con = new MSSQLCondition<VENDOR_NODE>(x => x.ORGANIZATION_NODE_DEFINITION.KEY == EssentialCache.NodeDefinitionValue.Vendor);

                if (isSelf)
                {
                    var user = this.UserIdentity.Instance;

                    var vendorIDs = user.JobPositions?
                                        .GetOwnerRootNodeIDs<VendorJobPosition>();

                    con.And(x => vendorIDs.Contains(x.VENDER_ID));
                }

                con.IncludeBy(x => x.ORGANIZATION_NODE_DEFINITION);
                con.OrderBy(x => x.CREATE_DATETIME, OrderType.Desc);

                var result = _OrganizationAggregate.VendorNode_T1_T2_.GetPaging(con).ToList();

                var select2 = new Select2Response<VendorNode>()
                {
                    items = Select2Response<VendorNode>.ToSelectItems(result, x => x.NodeID.ToString(), x => x.Name, x => x)
                };

                return Ok(select2);
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return await StatusCode(HttpStatusCode.InternalServerError).Async();
            }
        }

        /// <summary>
        /// 取得廠商組織底下使用者
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetVendorNodeUsers")]
        public async Task<PagingResponse> GetVendorNodeUsersAsync(PagingRequest<NodeUserSearchViewModel> model)
        {
            try
            {
                var searchTerm = model.criteria;

                var con = new MSSQLCondition<USER>(
                   searchTerm,
                   model.pageIndex,
                   model.pageSize);

                con.OrderBy(model.sort, model.orderType);

                con.IncludeBy(x => x.NODE_JOB.Select(
                    g => g.JOB
                ));

                con.And(x => x.NODE_JOB.Any(
                    c =>
                    c.NODE_ID == searchTerm.NodeID &&
                    c.ORGANIZATION_TYPE == (byte)searchTerm.OrganizationType
                ));


                var ocon = new MSSQLCondition<VENDOR_NODE>(x =>
                     x.NODE_ID == searchTerm.NodeID &&
                     x.ORGANIZATION_TYPE == (byte)searchTerm.OrganizationType);

                ocon.IncludeBy(x => x.VENDOR_NODE2);

                var node = _OrganizationAggregate.VendorNode_T1_T2_.Get(ocon);

                var list = _OrganizationAggregate.User_T1_T2_.GetPaging(con);

                var ui = list.Select(x => new VendorUserListViewModel(x, node));

                return await new PagingResponse<IEnumerable<VendorUserListViewModel>>(ui)
                {
                    isSuccess = true,
                    totalCount = list.TotalCount
                }.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return await new PagingResponse(false).Async();
            }
        }

        /// <summary>
        /// 取得廠商節點下的所有人員執掌
        /// </summary>
        /// <param name="nodeID"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetVendorNodeUsersFromID")]
        public async Task<IHttpActionResult> GetVendorNodeUsersFromIDAsync(int nodeID)
        {
            try
            {
                var ocon = new MSSQLCondition<VENDOR_NODE>(x => x.NODE_ID == nodeID);

                var organization = _OrganizationAggregate.VendorNode_T1_IOrganizationNode_.Get(ocon);

                var con = new MSSQLCondition<NODE_JOB>(x =>
                    x.NODE_ID == nodeID &&
                    x.ORGANIZATION_TYPE == (byte)OrganizationType.Vendor
                 );

                con.IncludeBy(x => x.USER);
                con.IncludeBy(x => x.JOB);
                con.OrderBy(x => x.JOB.LEVEL, OrderType.Asc);

                var jobPosition = _OrganizationAggregate.JobPosition_T1_T2_.GetList(con);

                var users = jobPosition.SelectMany(c => c.Users);

                var list = users.Select(x => new UserListViewModel(x, organization));

                return Ok(list);
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return await StatusCode(HttpStatusCode.InternalServerError).Async();
            }
        }

        /// <summary>
        /// 取得廠商節點下的所有職稱定義與職稱
        /// </summary>
        /// <param name="nodeID"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetVendorNodeDefJobsFromID")]
        public async Task<IHttpActionResult> GetVendorNodeDefJobsFromIDAsync(int nodeID)
        {
            try
            {
                var con = new MSSQLCondition<NODE_JOB>(x =>
                    x.IDENTIFICATION_ID == nodeID &&
                    x.ORGANIZATION_TYPE == (byte)OrganizationType.Vendor);

                con.IncludeBy(x => x.JOB.ORGANIZATION_NODE_DEFINITION);

                var nodeJobs = _OrganizationAggregate.JobPosition_T1_T2_.GetList(con);

                var jobs = nodeJobs.Select(x => x.Job)
                                   .DistinctBy(x => x.ID);

                var list = jobs?.Select(x => new JobListViewModel(x));

                return Ok(list);
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return await StatusCode(HttpStatusCode.InternalServerError).Async();
            }
        }
    }
}
