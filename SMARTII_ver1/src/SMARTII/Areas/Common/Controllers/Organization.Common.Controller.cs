using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Ajax.Utilities;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Areas.Case.Models;
using SMARTII.Areas.Organization.Models.OrganizarionNodeDefinition;
using SMARTII.Areas.Organization.Models.User;
using SMARTII.COMMON_BU.Models.Store;
using SMARTII.COMMON_BU.Service;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using MoreLinq;

namespace SMARTII.Areas.Common.Controllers
{
    public partial class OrganizationController
    {
        /// <summary>
        /// 列舉下拉選單
        /// </summary>
        /// <param name="enumName"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetEnumType")]
        public async Task<IHttpActionResult> GetEnumTypeAsync(string enumName)
        {
            try
            {
                var select2 = new Select2Response();
                var assembly = Assembly.Load("SMARTII.Domain");
                var type = assembly.ExportedTypes.FirstOrDefault(g =>
                g.IsEnum == true &&
                g.Name == enumName);

                if (type != null)
                {
                    select2.items = type.EnumSelectListFor().ToList();
                }

                return Ok(select2);
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return await StatusCode(HttpStatusCode.InternalServerError).Async();
            }
        }

        /// <summary>
        /// 取得節點下的所有職稱定義與職稱
        /// </summary>
        /// <param name="nodeID"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetNodeDefJobsFromID")]
        public async Task<IHttpActionResult> GetNodeDefJobsFromIDAsync(int nodeID, OrganizationType type)
        {
            try
            {
                var con = new MSSQLCondition<ORGANIZATION_NODE_DEFINITION>(x =>
                x.IDENTIFICATION_ID == nodeID &&
                x.ORGANIZATION_TYPE == (byte)type);

                con.IncludeBy(x => x.JOB);
                con.OrderBy(x => x.LEVEL, OrderType.Asc);

                var nodeJobs = _OrganizationAggregate.OrganizationNodeDefinition_T1_T2_.GetList(con).ToList();

                List<Job> jobs = new List<Job>();
                nodeJobs.ForEach(x => jobs.AddRange(x.Jobs.OrderBy(c => c.Level)));

                var list = jobs?.Select(x => new JobListViewModel(x));

                return Ok(list);
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return await StatusCode(HttpStatusCode.InternalServerError).Async();
            }
        }

        /// <summary>
        /// 取得門市清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetStores")]
        public async Task<IHttpActionResult> GetStoresAsync(Select2Request<StoreSearchViewModel> model)
        {
            try
            {
                var searchTerm = model.criteria;

                var con = new MSSQLCondition<STORE>(
                    searchTerm,
                    model.pageIndex,
                    model.size
                );

                con.And(x => x.IS_ENABLED == searchTerm.IsEnable);
                con.IncludeBy(x => x.HEADQUARTERS_NODE.HEADQUARTERS_NODE2);

                if (string.IsNullOrEmpty(model.keyword) == false)
                {
                    con.And(x => x.NAME.Contains(model.keyword) || x.CODE.Contains(model.keyword));
                }

                con.OrderBy(x => x.NAME, OrderType.Asc);

                var result = _OrganizationAggregate.Store_T1_T2_Expendo_
                                                   .GetPaging(con)
                                                   .ToList();

                result.ForEach(store =>
                {
                    store.OwnerJobPosition = store.OwnerNodeJobID.HasValue ? _StoreFacade.GetApplyJobPositions((int)store.OwnerNodeJobID, EssentialCache.JobValue.OWNER) : null;
                    store.OfcJobPosition = store.SupervisorNodeJobID.HasValue ? _StoreFacade.GetApplyJobPositions((int)store.SupervisorNodeJobID, EssentialCache.JobValue.OFC) : null;
                });

                var select2 = new Select2Response<StoreDetailViewModel>()
                {
                    items = Select2Response<StoreDetailViewModel>
                        .ToSelectItems(result, x => x.NodeID.ToString(), x => $"{x.Code}-{x.Name}", x => new StoreDetailViewModel(x))
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
        /// 取得組織父節點階層
        /// </summary>
        /// <param name="nodeID"></param>
        /// <param name="organizationType"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetOrganizationParentPath")]
        public async Task<IHttpActionResult> GetOrgnizationParentPathAsync(int nodeID, OrganizationType organizationType)
        {
            try
            {
                var @base = _OrganizationAggregate.OrganizationNodeBase_T1_T2_.Get(
                    x => x.NODE_ID == nodeID &&
                    x.ORGANIZATION_TYPE == (byte)organizationType);

                var targets = @base.ParentPath.GetRootNodeParentPathArray();

                var parents = _OrganizationAggregate.OrganizationNodeBase_T1_T2_.GetList(
                                    x => targets.Contains(x.NODE_ID) &&
                                    x.ORGANIZATION_TYPE == (byte)organizationType);

                var parentNames = parents.SortNode(x => x.NodeID, targets).Select(x => x.Name)
                                         .ToArray();

                return Ok(parentNames);
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return await StatusCode(HttpStatusCode.InternalServerError).Async();
            }
        }

        /// <summary>
        /// 取得負責人, 從組織節點
        /// </summary>
        /// <param name="nodeID"></param>
        /// <param name="organizationType"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetOwnerUserFromNode")]
        public async Task<IHttpActionResult> GetOwnerUserFromNodeAsync(int nodeID, OrganizationType organizationType)
        {
            try
            {
                var con = new MSSQLCondition<NODE_JOB>(x => x.NODE_ID == nodeID &&
                                                            x.ORGANIZATION_TYPE == (byte)organizationType &&
                                                            x.JOB.KEY == EssentialCache.JobValue.OWNER);
                con.IncludeBy(x => x.USER);
                con.IncludeBy(x => x.JOB);
                var jobPosition = _OrganizationAggregate.JobPosition_T1_T2_.GetFirstOrDefault(con);

                if (jobPosition == null)
                    return Ok(new UserListViewModel());


                var user = jobPosition?.Users?.FirstOrDefault();

                var organization = await _NodeProviders[jobPosition.OrganizationType].Get(jobPosition.NodeID);

                var result = user == null ?
                        new UserListViewModel() : new UserListViewModel(user, organization);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return await StatusCode(HttpStatusCode.InternalServerError).Async();
            }
        }

        /// <summary>
        /// 從門市綁定的負責職稱 , 取人員
        /// </summary>
        /// <param name="nodeJobID"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetOwnerUserFromStore")]
        public async Task<IHttpActionResult> GetOwnerUserFromStoreAsync(int nodeJobID)
        {
            try
            {
                var con = new MSSQLCondition<NODE_JOB>(x => x.ID == nodeJobID && x.ORGANIZATION_TYPE == (byte)OrganizationType.HeaderQuarter);
                con.IncludeBy(x => x.USER);
                con.IncludeBy(x => x.JOB);
                var jobPosition = _OrganizationAggregate.JobPosition_T1_T2_.Get(con);

                var user = jobPosition?.Users?.FirstOrDefault();

                var organization = await _NodeProviders[jobPosition.OrganizationType].Get(jobPosition.NodeID);

                var result = user == null ?
                        new UserListViewModel() : new UserListViewModel(user, organization);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return await StatusCode(HttpStatusCode.InternalServerError).Async();
            }
        }

        /// <summary>
        /// 傳入之節點可向上查找底下的使用者
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetNodeAllUsers")]
        public async Task<IHttpResult> GetNodeAllUsersAsync(List<CaseAssignmentUserViewModel> model)
        {
            try
            {
                var result = new List<UserListViewModel>();
                var tmpList = new List<UserListViewModel>();
                foreach (var data in model)
                {
                    var con = new MSSQLCondition<USER>();
                    con.IncludeBy(x => x.NODE_JOB.Select(
                        g => g.JOB
                    ));

                    switch (data.OrganizationType)
                    {
                        case OrganizationType.CallCenter:
                            var node = _OrganizationAggregate.CallCenterNode_T1_IOrganizationNode_
                                .Get(x => x.NODE_ID == data.NodeID);

                            var nodeChain = node.ParentPath?.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).Append(node.NodeID);

                            if (nodeChain == null)
                                continue;

                            con.And(x => x.NODE_JOB.Any(
                            c =>
                            nodeChain.Contains(c.NODE_ID) &&
                            c.ORGANIZATION_TYPE == (byte)data.OrganizationType
                            ));

                            var list = _OrganizationAggregate.User_T1_T2_.GetList(con);
                            var ui = list.Select(x => new UserListViewModel(x));
                            tmpList.AddRange(ui);
                            break;
                        case OrganizationType.HeaderQuarter:
                            var node2 = _OrganizationAggregate.HeaderQuarterNode_T1_IOrganizationNode_
                                 .Get(x => x.NODE_ID == data.NodeID);

                            var nodeChain2 = node2.ParentPath?.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).Append(node2.NodeID);

                            if (nodeChain2 == null)
                                continue;

                            con.And(x => x.NODE_JOB.Any(
                            c =>
                            nodeChain2.Contains(c.NODE_ID)&&
                            c.ORGANIZATION_TYPE == (byte)data.OrganizationType
                            ));

                            var list2 = _OrganizationAggregate.User_T1_T2_.GetList(con);
                            var ui2 = list2.Select(x => new UserListViewModel(x));
                            tmpList.AddRange(ui2);
                            break;
                        case OrganizationType.Vendor:
                            var node3 = _OrganizationAggregate.VendorNode_T1_IOrganizationNode_
                                .Get(x => x.NODE_ID == data.NodeID);

                            var nodeChain3 = node3.ParentPath?.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).Append(node3.NodeID);

                            if (nodeChain3 == null)
                                continue;

                            con.And(x => x.NODE_JOB.Any(
                            c =>
                            nodeChain3.Contains(c.NODE_ID) &&
                            c.ORGANIZATION_TYPE == (byte)data.OrganizationType
                            ));

                            var list3 = _OrganizationAggregate.User_T1_T2_.GetList(con);
                            var ui3 = list3.Select(x => new UserListViewModel(x));
                            tmpList.AddRange(ui3);
                            break;
                        default:
                            break;
                    }
                }
                foreach(var item in tmpList)
                {
                    if (!result.Any(x => x.UserID == item.UserID))
                        result.Add(item);
                }


                return await new JsonResult<List<UserListViewModel>>(result, true).Async();

            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return await new JsonResult(ex.Message, false).Async();
            }
        }
    }
}
