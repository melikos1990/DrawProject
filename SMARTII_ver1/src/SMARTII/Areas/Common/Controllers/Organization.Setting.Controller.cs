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
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;

namespace SMARTII.Areas.Common.Controllers
{
    public partial class OrganizationController
    {
        /// <summary>
        /// 取得組織定義清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetNodeDefinitions")]
        public async Task<IHttpActionResult> GetNodeDefinitionAsync(NodeDefinitionSearchViewModel model)
        {
            try
            {
                var con = new MSSQLCondition<ORGANIZATION_NODE_DEFINITION>(model);

                if (string.IsNullOrEmpty(model.IncludeDefKey) == false)
                {
                    con.Or(x => x.ORGANIZATION_TYPE == (int)model.OrganizationType && x.KEY == model.IncludeDefKey);
                }

                var result = _OrganizationAggregate.OrganizationNodeDefinition_T1_T2_.GetList(con).ToList();
                
                var select2 = new Select2Response<OrganizationNodeDefinition>()
                {
                    items = Select2Response<OrganizationNodeDefinition>.ToSelectItems(result, x => x.ID.ToString(), x => x.Name, x => x)
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
        /// 取得組織定義清單
        /// ※ 這邊照層級分類
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetAggregateNodeDefinitionLevels")]
        public async Task<IHttpActionResult> GetAggregateNodeDefinitionLevelAsync(int? buID = null)
        {
            try
            {
                // 取得 BU 以下的階層 (1 = 總部端節點 , 2 = BU)
                var con = new MSSQLCondition<HEADQUARTERS_NODE>(x => x.DEPTH_LEVEL >= 3 && x.NODE_TYPE != null);

                con.And(x => x.BU_ID == buID && x.ORGANIZATION_TYPE == (byte)OrganizationType.HeaderQuarter);
                
                con.IncludeBy(x => x.ORGANIZATION_NODE_DEFINITION);

                con.OrderBy(x => x.DEPTH_LEVEL, OrderType.Asc);

                var nodes = _OrganizationAggregate.HeaderQuarterNode_T1_T2_.GetList(con);

                var select2 = new Select2Response();

                nodes?.GroupBy(x => x.Level)
                     .ForEach(x =>
                     {
                         var text = x.DistinctBy(g => g.NodeType).Select(g => g.OrganizationNodeDefinitaion.Name).ToArray().ConcatArray("/");
                         var id = x.Key.ToString();

                         select2.items.Add(new SelectItem(id, text));
                     });

                
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
