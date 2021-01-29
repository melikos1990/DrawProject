using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Ajax.Utilities;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Areas.Master.Models.KMClassification;
using SMARTII.Assist.Authentication;
using SMARTII.Assist.Logger;
using SMARTII.Assist.Web;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;

namespace SMARTII.Areas.Master.Controllers
{
    [Authentication]
    public class KMClassificationController : BaseApiController
    {
        private readonly IKMClassificationFacade _KMClassificationFacade;
        private readonly ICommonAggregate _CommonAggregate;
        private readonly IMasterAggregate _MasterAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;

        public KMClassificationController(IKMClassificationFacade KMClassificationFacade,
                                          ICommonAggregate CommonAggregate,
                                          IMasterAggregate MasterAggregate,
                                          IOrganizationAggregate OrganizationAggregate)
        {
            _CommonAggregate = CommonAggregate;
            _MasterAggregate = MasterAggregate;
            _OrganizationAggregate = OrganizationAggregate;
            _KMClassificationFacade = KMClassificationFacade;
        }

        /// <summary>
        /// 取得KM TREE
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(KMClassification_lang.KMCLASSIFICATION_GET_TREE))]
        public async Task<IHttpResult> GetKMTree()
        {
            try
            {
                var con = new MSSQLCondition<HEADQUARTERS_NODE>();
                var user = ContextUtility.GetUserIdentity().Instance;

                var ids = user.DownProviderBUDist.TryGetBuList(Domain.Organization.OrganizationType.CallCenter);
                con.And(x => ids.Contains(x.NODE_ID));
                con.And(x => x.NODE_TYPE_KEY == EssentialCache.NodeDefinitionValue.BusinessUnit);
                var nodes = _OrganizationAggregate.HeaderQuarterNode_T1_T2_
                                                  .GetList(con);

                var nodesKey = nodes.Select(x => x.ID).ToList();
                var nodeDict = nodes.ToDictionary(x => x.ID);

                // 找到KM分類
                var kcon = new MSSQLCondition<KM_CLASSIFICATION>(x => nodesKey.Contains(x.NODE_ID));

                var list = _MasterAggregate.KMClassification_T1_T2_.GetList(kcon);

                var listDict = list?.DistinctBy(x=>x.NodeID)
                                    .ToDictionary(x => x.NodeID);

                var group = list.GroupBy(x => x.NodeID)
                                .Select(x => x.AsNested())
                                .ToList();


                var data = group.Select(x =>
                {
                    var nodeID = x.First().NodeID;
                    var collection = x.ToList();
                    string pathNames = "";

                    return new KMClassificationNodeViewModel(nodeDict[nodeID], collection, pathNames);
                })
                .Concat(
                    nodes.Where(x => listDict.ContainsKey(x.NodeID) == false)?
                         .Select(x => new KMClassificationNodeViewModel(x, new List<KMClassification>(), ""))                   
                 )
                .ToList();

                

                var result =
                   new JsonResult<List<KMClassificationNodeViewModel>>(data, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                                 ex.PrefixDevMessage(KMClassification_lang.KMCLASSIFICATION_GET_TREE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(KMClassification_lang.KMCLASSIFICATION_GET_TREE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 取得清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(KMClassification_lang.KMCLASSIFICATION_GETLIST))]
        [ModelValidator(false)]
        public async Task<PagingResponse> GetList(PagingRequest<KMSearchViewModel> model)
        {
            try
            {
                var con = new MSSQLCondition<KM_DATA>(
                   model.criteria ?? new KMSearchViewModel(),
                   model.pageIndex,
                   model.pageSize);

                con.IncludeBy(x => x.KM_CLASSIFICATION);

                con.OrderBy(model.sort, model.orderType);

                var list = _MasterAggregate.KMData_T1_T2_.GetPaging(con);

                var ui = list.Select(x => new KMListViewModel(x));

                return await new PagingResponse<IEnumerable<KMListViewModel>>(ui)
                {
                    isSuccess = true,
                    totalCount = list.TotalCount
                }.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(KMClassification_lang.KMCLASSIFICATION_GETLIST_FAIL));

                return await new PagingResponse()
                {
                    isSuccess = false,
                    message = ex.PrefixMessage(KMClassification_lang.KMCLASSIFICATION_GETLIST_FAIL)
                }.Async();
            }
        }

        /// <summary>
        /// 單一取得明細
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(KMClassification_lang.KMCLASSIFICATION_GET))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Get([Required]int ID)
        {
            try
            {
                var con = new MSSQLCondition<KM_DATA>();
                con.IncludeBy(x => x.KM_CLASSIFICATION);
                con.And(x => x.ID == ID);

                var data = _MasterAggregate.KMData_T1_T2_.Get(con);

                // 取得 ClassificationPathName
                var vwCon = new MSSQLCondition<VW_KM_CLASSIFICATION_NESTED>(x => x.ID == data.KMClassification.ID);
                data.KMClassification.ParentNamePath = _MasterAggregate.VWKMClassification_KMClassification_
                                .GetOfSpecific(vwCon, x => x.PARENT_PATH_NAME);

                var bucon = new MSSQLCondition<HEADQUARTERS_NODE>();
                bucon.And(x => x.NODE_ID == data.KMClassification.NodeID);
                var BuName = _OrganizationAggregate.HeaderQuarterNode_T1_T2_.Get(bucon)?.Name;

                var result = new JsonResult<KMDetailViewModel>(
                                   new KMDetailViewModel(data, BuName), true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(KMClassification_lang.KMCLASSIFICATION_GET_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(KMClassification_lang.KMCLASSIFICATION_GET_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 單一新增明細
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Add)]
        [Logger(nameof(KMClassification_lang.KMCLASSIFICATION_CREATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Create(KMDetailViewModel model)
        {
            try
            {
                var domain = model.ToDomain();

                await _KMClassificationFacade.Create(domain);

                var result = new JsonResult<string>(KMClassification_lang.KMCLASSIFICATION_CREATE_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(KMClassification_lang.KMCLASSIFICATION_CREATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(KMClassification_lang.KMCLASSIFICATION_CREATE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 單一刪除明細
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Delete)]
        [Logger(nameof(KMClassification_lang.KMCLASSIFICATION_DELETE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Delete([Required]int ID)
        {
            try
            {
                await _KMClassificationFacade.Delete(ID);

                var result = new JsonResult<string>(KMClassification_lang.KMCLASSIFICATION_DELETE_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(KMClassification_lang.KMCLASSIFICATION_DELETE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(KMClassification_lang.KMCLASSIFICATION_DELETE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 單一更新明細
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Update)]
        [Logger(nameof(KMClassification_lang.KMCLASSIFICATION_UPDATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Update(KMDetailViewModel model)
        {
            try
            {
                var domain = model.ToDomain();

                await _KMClassificationFacade.Update(domain);

                var result = new JsonResult<string>(KMClassification_lang.KMCLASSIFICATION_UPDATE_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(KMClassification_lang.KMCLASSIFICATION_UPDATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(KMClassification_lang.KMCLASSIFICATION_UPDATE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 單一新增分類
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Add)]
        [Logger(nameof(KMClassification_lang.KMCLASSIFICATION_CREATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> CreateRootClassification(int? nodeID, [Required]string name)
        {
            try
            {
                await _KMClassificationFacade.CreateRootClassification(nodeID.Value, name);

                var result = new JsonResult<string>(KMClassification_lang.KMCLASSIFICATION_CREATE_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(KMClassification_lang.KMCLASSIFICATION_CREATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(KMClassification_lang.KMCLASSIFICATION_CREATE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 單一新增分類
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Add)]
        [Logger(nameof(KMClassification_lang.KMCLASSIFICATION_CREATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> CreateClassification(int? parentID, [Required]string name)
        {
            try
            {
                var domain = new KMClassification()
                {
                    ParentID = parentID,
                    Name = name,
                };

                await _KMClassificationFacade.CreateClassification(domain);

                var result = new JsonResult<string>(KMClassification_lang.KMCLASSIFICATION_CREATE_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(KMClassification_lang.KMCLASSIFICATION_CREATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(KMClassification_lang.KMCLASSIFICATION_CREATE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 單一刪除分類
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Delete)]
        [Logger(nameof(KMClassification_lang.KMCLASSIFICATION_DELETE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> DeleteClassification([Required]int ID)
        {
            try
            {
                await _KMClassificationFacade.DeleteClassification(ID);

                var result = new JsonResult<string>(KMClassification_lang.KMCLASSIFICATION_DELETE_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(KMClassification_lang.KMCLASSIFICATION_DELETE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(KMClassification_lang.KMCLASSIFICATION_DELETE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 單一更新母節點(拖曳行為)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Update)]
        [Logger(nameof(KMClassification_lang.KMCLASSIFICATION_UPDATE))]
        public async Task<IHttpResult> DragClassification(int? ID, int? parentID)
        {
            try
            {
                await _KMClassificationFacade.DragClassification(ID.Value, parentID);

                var result = new JsonResult<string>(KMClassification_lang.KMCLASSIFICATION_UPDATE_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(KMClassification_lang.KMCLASSIFICATION_UPDATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(KMClassification_lang.KMCLASSIFICATION_UPDATE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 單一更新名稱
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Update)]
        [Logger(nameof(KMClassification_lang.KMCLASSIFICATION_UPDATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> RenameClassification([Required]int? ID, [Required]string name)
        {
            try
            {
                await _KMClassificationFacade.RenameClassification(ID.Value, name);

                var result = new JsonResult<string>(KMClassification_lang.KMCLASSIFICATION_UPDATE_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(KMClassification_lang.KMCLASSIFICATION_UPDATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(KMClassification_lang.KMCLASSIFICATION_UPDATE_FAIL), false)
                    .Async();
            }
        }
    }
}
