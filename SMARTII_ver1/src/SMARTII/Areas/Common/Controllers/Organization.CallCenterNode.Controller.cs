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
using SMARTII.Areas.Organization.Models.CallCenterNode;
using SMARTII.Areas.Organization.Models.OrganizarionNodeDefinition;
using SMARTII.Areas.Organization.Models.User;
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
        /// 取得客服中心節點
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetCallCenterNode")]
        public async Task<IHttpActionResult> GetCallCenterNodeAsync(int nodeID)
        {
            try
            {
                var con = new MSSQLCondition<CALLCENTER_NODE>(
                    x => x.NODE_ID == nodeID &&
                         x.ORGANIZATION_TYPE == (byte)OrganizationType.CallCenter);

                con.IncludeBy(x => x.ORGANIZATION_NODE_DEFINITION);

                var data = _OrganizationAggregate.CallCenterNode_T1_T2_.Get(con);

                var result = new JsonResult<CallCenterNodeDetailViewModel>(
                    new CallCenterNodeDetailViewModel(data), true);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return await StatusCode(HttpStatusCode.InternalServerError).Async();
            }
        }

        /// <summary>
        /// 取得客服中心組織樹
        /// </summary>
        /// <param name="isSelf"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetCallCenterNodeTree")]
        public async Task<IHttpActionResult> GetCallCenterNodeTreeAsync(OrganizationDataRangeSearchViewModel model)
        {
            try
            {
                var con = new MSSQLCondition<CALLCENTER_NODE>();
                con.IncludeBy(x => x.CALLCENTER_NODE2);

                if (model.IsSelf)
                    con.FilterCallCenterTreeNodeRange(model.Goal);

                if (model.NodeID.HasValue)
                {
                    if (model.IsStretch)
                    {
                        con.FilterSpecifyNodeField(x => x.NODE_ID == model.NodeID, PredicateType.Or);
                        con.FilterSpecifyNodeField(x => x.PARENT_PATH.Contains(model.NodeID.ToString()), PredicateType.Or);
                    }
                    else
                    {
                        con.FilterSpecifyNodeField(x => x.NODE_ID == model.NodeID);
                    }
                }

                if (!string.IsNullOrEmpty(model.DefKey))
                    con.FilterSpecifyNodeField(x => x.NODE_TYPE_KEY == model.DefKey);

                con.IncludeBy(x => x.ORGANIZATION_NODE_DEFINITION);

                var nodes = _OrganizationAggregate.CallCenterNode_T1_IOrganizationNode_.GetList(con);

                var nested = (CallCenterNode)nodes.AsNestedNSM();

                var result = new JsonResult<CallCenterNodeViewModel>(
                    new CallCenterNodeViewModel(nested), true);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return await StatusCode(HttpStatusCode.InternalServerError).Async();
            }
        }

        /// <summary>
        /// 取得客服中心組織根結點
        /// </summary>
        /// <param name="isSelf"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetCallCenterRootNodes")]
        public async Task<IHttpActionResult> GetCallCenterRootNodesAsync(bool isSelf = true)
        {
            try
            {
                var con = new MSSQLCondition<CALLCENTER_NODE>(x => x.ORGANIZATION_NODE_DEFINITION.KEY == EssentialCache.NodeDefinitionValue.CallCenter);

                con.IncludeBy(x => x.ORGANIZATION_NODE_DEFINITION);

                if (isSelf)
                {
                    var user = this.UserIdentity.Instance;

                    var ccIDs = user.JobPositions?
                                    .GetOwnerRootNodeIDs<CallCenterJobPosition>();

                    con.And(x => ccIDs.Contains(x.CALLCENTER_ID));
                }

                con.OrderBy(x => x.CREATE_DATETIME, OrderType.Desc);

                var result = _OrganizationAggregate.CallCenterNode_T1_T2_.GetPaging(con).ToList();

                var select2 = new Select2Response<CallCenterNode>()
                {
                    items = Select2Response<CallCenterNode>.ToSelectItems(result, x => x.NodeID.ToString(), x => x.Name, x => x)
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
        /// 取得客服中心Group 節點
        /// </summary>
        /// <param name="isSelf"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetCallCenterGroupNodes")]
        public async Task<IHttpActionResult> GetCallCenterGroupNodesAsync(bool isSelf = true, int? buID = null)
        {
            try
            {
                var con = new MSSQLCondition<CALLCENTER_NODE>();

                con.IncludeBy(x => x.ORGANIZATION_NODE_DEFINITION);


                if (isSelf)
                {
                    var user = ContextUtility.GetUserIdentity().Instance;
                    con.FilterNodeFromPosition(user.JobPositions);

                    //var nodeIDs = UserIdentity.Instance.JobPositions.Select(x => x.NodeID);

                    //con.And(x => nodeIDs.Contains(x.NODE_ID));
                }
                con.And(x => x.ORGANIZATION_NODE_DEFINITION.KEY == EssentialCache.NodeDefinitionValue.Group);

                // UI 需要依BU顯示 所以預設null
                con.And(x => x.HEADQUARTERS_NODE.Any(y => y.NODE_ID == buID));


                var result = _OrganizationAggregate.CallCenterNode_T1_T2_.GetPaging(con).ToList();

                var select2 = new Select2Response<CallCenterNode>()
                {
                    items = Select2Response<CallCenterNode>.ToSelectItems(result, x => x.NodeID.ToString(), x => x.Name, x => x)
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
        /// 取得總部組織底下使用者
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetCallCenterNodeUsers")]
        public async Task<PagingResponse> GetCallCenterNodeUsersAsync(PagingRequest<NodeUserSearchViewModel> model)
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

                var ocon = new MSSQLCondition<CALLCENTER_NODE>(x =>
                     x.NODE_ID == searchTerm.NodeID &&
                     x.ORGANIZATION_TYPE == (byte)searchTerm.OrganizationType);

                ocon.IncludeBy(x => x.CALLCENTER_NODE2);

                var node = _OrganizationAggregate.CallCenterNode_T1_T2_.Get(ocon);

                var list = _OrganizationAggregate.User_T1_T2_.GetPaging(con);

                var ui = list.Select(x => new CallCenterUserListViewModel(x, node));

                return await new PagingResponse<IEnumerable<CallCenterUserListViewModel>>(ui)
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
        /// 取得該使用者底下客服Uesr清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetOwnCallCenterNodeUsers")]
        public async Task<IHttpActionResult> GetOwnCallCenterNodeUsersAsync(Select2Request<CallCenterUserSearchViewModel> model)
        {
            try
            {
                var searchTerm = model.criteria;
                var con = new MSSQLCondition<USER>(model.size, model.pageIndex);

                con.IncludeBy(x => x.NODE_JOB);
                con.OrderBy(x => x.USER_ID, OrderType.Asc);

                // 找出屬於自己的節點
                if (searchTerm.IsSelf)
                {
                    var user = ContextUtility.GetUserIdentity().Instance;

                    con.FilterUserFromPosition<CallCenterJobPosition>(user.JobPositions);
                }

                con.And(searchTerm);

                if (string.IsNullOrEmpty(model.keyword) == false)
                {
                    con.And(x => x.NAME.Contains(model.keyword));
                }

                var result = _OrganizationAggregate.User_T1_T2_.GetPaging(con).ToList();

                result = result.DistinctBy(x => x.UserID).ToList();

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

        /// <summary>
        /// 取得客服中心節點下的人員工作職掌
        /// </summary>
        /// <param name="nodeID"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetCallCenterNodeUsersFromID")]
        public async Task<IHttpActionResult> GetCallCenterNodeUsersFromIDAsync(int nodeID)
        {
            try
            {
                var ocon = new MSSQLCondition<CALLCENTER_NODE>(x => x.NODE_ID == nodeID);

                var organization = _OrganizationAggregate.CallCenterNode_T1_IOrganizationNode_.Get(ocon);

                var con = new MSSQLCondition<NODE_JOB>(x =>
                    x.NODE_ID == nodeID &&
                    x.ORGANIZATION_TYPE == (byte)OrganizationType.CallCenter
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
        /// 取得客服中心節點下的所有職稱定義與職稱
        /// </summary>
        /// <param name="nodeID"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetCallCenterNodeDefJobsFromID")]
        public async Task<IHttpActionResult> GetCallCenterNodeDefJobsFromIDAsync(int nodeID)
        {
            try
            {
                var con = new MSSQLCondition<NODE_JOB>(x =>
                    x.IDENTIFICATION_ID == nodeID &&
                    x.ORGANIZATION_TYPE == (byte)OrganizationType.CallCenter);

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
