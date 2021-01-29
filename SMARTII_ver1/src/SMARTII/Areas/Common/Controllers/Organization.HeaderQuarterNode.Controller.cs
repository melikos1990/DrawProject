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
using SMARTII.Areas.Organization.Models;
using SMARTII.Areas.Organization.Models.HeaderQuarterNode;
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
        /// 取得BU參數設定值
        /// ※ 參數清單
        /// </summary>
        /// <param name="buID"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetBUParameters")]
        public async Task<IHttpActionResult> GetBUParametersAsync(int buID)
        {
            try
            {
                var term = _HeaderQuarterNodeProcessProvider.GetTerm(buID, OrganizationType.HeaderQuarter);

                return Ok(new BusinesssUnitParameters((HeaderQuarterTerm)term));
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return await StatusCode(HttpStatusCode.InternalServerError).Async();
            }
        }

        /// <summary>
        /// 取得BU參數設定值(依NodeKey)
        /// ※ 參數清單
        /// </summary>
        /// <param name="NodeKey"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetBUParametersByNodeKey")]
        public async Task<IHttpActionResult> GetBUParametersByNodeKeyAsync(string nodeKey)
        {
            try
            {
                var term = _HeaderQuarterNodeProcessProvider.GetTerm(nodeKey, OrganizationType.HeaderQuarter);

                return Ok(new BusinesssUnitParameters((HeaderQuarterTerm)term));
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return await StatusCode(HttpStatusCode.InternalServerError).Async();
            }
        }

        /// <summary>
        /// 取得BU參數設定值
        /// ※ Layout / 
        /// </summary>
        /// <param name="buID"></param>
        /// <param name="isEnabled"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetBULayouts")]
        public async Task<IHttpActionResult> GetBULayout(int buID, bool isEnabled)
        {
            try
            {


                var term = (HeaderQuarterTerm)_HeaderQuarterNodeProcessProvider.GetTerm(buID, OrganizationType.HeaderQuarter);

                var layout = new string[] {
                    EssentialCache.LayoutValue.CaseOtherTemplate,
                    EssentialCache.LayoutValue.CaseFinishTemplate,
                    EssentialCache.LayoutValue.CaseTemplate,
                    EssentialCache.LayoutValue.ItemDeatilTemplate,
                    EssentialCache.LayoutValue.ItemQueryTemplate,
                    EssentialCache.LayoutValue.StoreDeatilTemplate,
                    EssentialCache.LayoutValue.StoreQueryTemplate,
                };


                var parameters = _SystemAggregate.SystemParameter_T1_T2_.GetList(x => layout.Contains(x.ID) &&
                                                                                      x.KEY == term.NodeKey)?
                                                                        .ToList();

                var con = new MSSQLCondition<CASE_FINISH_REASON_CLASSIFICATION>(x => x.NODE_ID == buID &&
                                                                                    x.ORGANIZATION_TYPE == (byte)OrganizationType.HeaderQuarter);

                con.IncludeBy(x => x.CASE_FINISH_REASON_DATA);
                con.OrderBy(x => x.ORDER, OrderType.Asc);

                if (isEnabled)
                {
                    con.And(x => x.IS_ENABLED == true);
                }

                var classifications = _MasterAggregate.CaseFinishReasonClassification_T1_T2_
                                                      .GetList(con)
                                                      .ToList();

                classifications.ForEach(x =>
                {
                    x.CaseFinishReasonDatas = x.CaseFinishReasonDatas.OrderBy(y => y.Order).ToList();

                    if (isEnabled)
                    {
                        x.CaseFinishReasonDatas = x.CaseFinishReasonDatas.Where(y => y.IsEnabled == true).ToList();
                    }
                });

                var result = new BusinessUnitLayouts(parameters, classifications);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return await StatusCode(HttpStatusCode.InternalServerError).Async();
            }
        }

        /// <summary>
        /// 取得企業別清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetEnterprises")]
        public async Task<IHttpActionResult> GetEnterpriseAsync()
        {
            try
            {
                var con = new MSSQLCondition<ENTERPRISE>();
                var result = _OrganizationAggregate.Enterprise_T1_T2_.GetList(con).ToList();

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

        /// <summary>
        /// 取得總部組織樹
        /// </summary>
        /// <param name="isSelf"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetHeaderQuarterNodeTree")]
        public async Task<IHttpActionResult> GetHeaderQuarterNodeTreeAsync(OrganizationDataRangeSearchViewModel model)
        {
            try
            {
                var con = new MSSQLCondition<HEADQUARTERS_NODE>(model);
                con.IncludeBy(x => x.HEADQUARTERS_NODE2);

                if (model.IsSelf)
                    con.FilterHeaderQuartersTreeNodeRange(model.Goal);

                if (model.NodeID.HasValue)
                {
                    if (model.IsStretch)
                    {
                        con.FilterSpecifyNodeField(x => x.BU_ID == model.NodeID);
                    }
                    else
                    {
                        con.FilterSpecifyNodeField(x => x.NODE_ID == model.NodeID);
                    }
                }

                if (!string.IsNullOrEmpty(model.DefKey))
                    con.FilterSpecifyNodeField(x => x.NODE_TYPE_KEY == model.DefKey);

                if (model.NotIncludeDefKey != null && model.NotIncludeDefKey.Count() > 0)
                {
                    con.FilterSpecifyNodeField(x => !model.NotIncludeDefKey.Contains(x.NODE_TYPE_KEY));
                }

                con.IncludeBy(x => x.ORGANIZATION_NODE_DEFINITION);

                var nodes = _OrganizationAggregate.HeaderQuarterNode_T1_IOrganizationNode_.GetList(con);

                var nested = (HeaderQuarterNode)nodes.AsNestedNSM();

                var result = new JsonResult<HeaderQuarterNodeViewModel>(
                    new HeaderQuarterNodeViewModel(nested), true);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return await StatusCode(HttpStatusCode.InternalServerError).Async();
            }
        }

        /// <summary>
        /// 取得總部企業組織根結點
        /// </summary>
        /// <param name="isSelf"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetHeaderQuarterRootNodes")]
        public async Task<IHttpActionResult> GetHeaderQuarterRootNodesAsync(OrganizationType typeStyle, bool isSelf = true, int? buID = null, bool? IsSearchEnabled = null)
        {
            try
            {
                var con = new MSSQLCondition<HEADQUARTERS_NODE>(x => x.ORGANIZATION_NODE_DEFINITION.KEY == EssentialCache.NodeDefinitionValue.BusinessUnit);

                con.IncludeBy(x => x.ORGANIZATION_NODE_DEFINITION);

                if (isSelf)
                {
                    var buIDs = UserIdentity.Instance.DownProviderBUDist.TryGetBuList(typeStyle).Cast<int?>();

                    con.And(x => buIDs.Contains(x.BU_ID));
                }

                if (buID.HasValue)
                {
                    con.And(x => x.BU_ID == buID);
                }

                if (IsSearchEnabled.HasValue)
                {
                    con.And(x => x.IS_ENABLED == IsSearchEnabled);
                }

                con.OrderBy(x => x.CREATE_DATETIME, OrderType.Desc);

                var result = _OrganizationAggregate.HeaderQuarterNode_T1_T2_.GetPaging(con).ToList();

                var select2 = new Select2Response<HeaderQuarterNode>()
                {
                    items = Select2Response<HeaderQuarterNode>.ToSelectItems(result, x => x.NodeID.ToString(), x => x.Name, x => x)
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
        /// 從組織層級取得組織清單
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetHeaderQurterNodesByLevel")]
        public async Task<IHttpActionResult> GetHeaderQurterNodesByLevelAsync(HeaderQuarterNodeSearchViewModel model)
        {
            try
            {
                var con = new MSSQLCondition<HEADQUARTERS_NODE>();

                if (model.IsSelf)
                {
                    var user = this.UserIdentity.Instance;

                    con.FilterNodeFromPosition(user.JobPositions);
                }

                con.And(model);

                con.IncludeBy(x => x.ORGANIZATION_NODE_DEFINITION);

                var result = _OrganizationAggregate.HeaderQuarterNode_T1_T2_.GetList(con).ToList();

                var select2 = new Select2Response<OrganizationNodeViewModel>()
                {
                    items = Select2Response<OrganizationNodeViewModel>.ToSelectItems(result,
                    x => x.NodeID.ToString(),
                    x => $"{x.Name}({x.OrganizationNodeDefinitaion?.Name})",
                    x => new OrganizationNodeViewModel(x))
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
        [ActionName("GetHeaderQuarterNodeUsers")]
        public async Task<PagingResponse> GetHeaderQuarterNodeUsersAsync(PagingRequest<NodeUserSearchViewModel> model)
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

                var ocon = new MSSQLCondition<HEADQUARTERS_NODE>(x =>
                     x.NODE_ID == searchTerm.NodeID &&
                     x.ORGANIZATION_TYPE == (byte)searchTerm.OrganizationType);

                ocon.IncludeBy(x => x.HEADQUARTERS_NODE2);

                var node = _OrganizationAggregate.HeaderQuarterNode_T1_T2_.Get(ocon);

                var list = _OrganizationAggregate.User_T1_T2_.GetPaging(con);

                var ui = list.Select(x => new HeaderQuarterUserListViewModel(x, node));

                return await new PagingResponse<IEnumerable<HeaderQuarterUserListViewModel>>(ui)
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
        /// 取得總部節點下的人員工作職掌
        /// </summary>
        /// <param name="nodeID"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetHeaderQuarterNodeUsersFromID")]
        public async Task<IHttpActionResult> GetHeaderQuarterNodeUsersFromIDAsync(int nodeID)
        {
            try
            {
                var ocon = new MSSQLCondition<HEADQUARTERS_NODE>(x => x.NODE_ID == nodeID);

                var organization = _OrganizationAggregate.HeaderQuarterNode_T1_IOrganizationNode_.Get(ocon);

                var con = new MSSQLCondition<NODE_JOB>(x =>
                    x.NODE_ID == nodeID &&
                    x.ORGANIZATION_TYPE == (byte)OrganizationType.HeaderQuarter
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
        /// 取得總部節點下的所有職稱定義與職稱
        /// </summary>
        /// <param name="nodeID"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetHeaderQuarterNodeDefJobsFromID")]
        public async Task<IHttpActionResult> GetHeaderQuarterNodeDefJobsFromIDAsync(int nodeID)
        {
            try
            {
                var con = new MSSQLCondition<NODE_JOB>(x =>
                x.IDENTIFICATION_ID == nodeID &&
                x.ORGANIZATION_TYPE == (byte)OrganizationType.HeaderQuarter);

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

        /// <summary>
        /// 傳入之節點可向上查找職稱與底下的使用者
        /// </summary>
        /// <param name="nodeID">目標節點 , 會查找該節點以上的節點</param>
        /// <param name="jobKey">底下職稱識別值 / 如 OFC ... </param>
        /// <param name="isTraversing"> 是否向上遍歷 </param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetAboveHeaderQuarterNodeJobUsers")]
        public async Task<IHttpActionResult> GetAboveHeaderQuarterNodeJobUsersAsync(int nodeID, string jobKey, bool isTraversing = true)
        {
            try
            {
                var node = _OrganizationAggregate.HeaderQuarterNode_T1_IOrganizationNode_
                                                 .Get(x => x.NODE_ID == nodeID);

                var nodeChain = (
                                  isTraversing ?
                                     node.ParentPath?
                                         .Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)
                                         .Select(x => int.Parse(x))
                                    : new int[] { }
                                )
                                .Append(node.NodeID);

                // 根據節點 , 找到組織職稱關係
                var con = new MSSQLCondition<NODE_JOB>
                    (
                      x => nodeChain.Contains(x.NODE_ID) &&
                           x.JOB.KEY == jobKey &&
                           x.ORGANIZATION_TYPE == (byte)OrganizationType.HeaderQuarter
                    );

                con.IncludeBy(x => x.JOB);
                con.IncludeBy(x => x.USER);

                var positions = _OrganizationAggregate.JobPosition_T1_T2_.GetList(con);

                positions.ForEach(async x =>
                {
                    x.Node = await _NodeProviders[x.OrganizationType].Get(x.NodeID);
                });

                var list = positions.Select(x => new JobPositionListViewModel(x));


                return Ok(list);
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return await StatusCode(HttpStatusCode.InternalServerError).Async();
            }
        }

        /// <summary>
        /// 取得總部企業組織根結點(依ID)
        /// </summary>
        /// <param name="isSelf"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetHeaderQuarterRootNodeKey")]
        public async Task<IHttpActionResult> GetHeaderQuarterRootNodeKeyAsync(int buID)
        {
            try
            {
                var con = new MSSQLCondition<HEADQUARTERS_NODE>(x => x.ORGANIZATION_NODE_DEFINITION.KEY == EssentialCache.NodeDefinitionValue.BusinessUnit);

                con.IncludeBy(x => x.ORGANIZATION_NODE_DEFINITION);

                con.And(x => x.BU_ID == buID);

                var result = _OrganizationAggregate.HeaderQuarterNode_T1_T2_.Get(con);

                return Ok(result.NodeKey);
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return await StatusCode(HttpStatusCode.InternalServerError).Async();
            }
        }
    }
}
