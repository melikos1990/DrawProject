using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMARTII.Domain.Master;
using MoreLinq;
using SMARTII.Domain.Organization;
using SMARTII.Service.Cache;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;

namespace SMARTII.Service.Master.Resolver
{
    public class StoreResolver
    {
        private readonly IOrganizationAggregate _OrganizationAggregate;

        public StoreResolver(IOrganizationAggregate OrganizationAggregate)
        {
            _OrganizationAggregate = OrganizationAggregate;
        }

        public List<T> ResolveCollection<T>(List<T> data) where T : IStoreRelationship, new()
        {
            IDictionary<string, Store> dist = new Dictionary<string, Store>();

            var group = data.GroupBy(x => new
            {
                NodeID = x.NodeID,
                OrganizationType = x.OrganizationType
            });

            group.ForEach(pair =>
            {
                var param = _OrganizationAggregate
                .Store_T1_T2_
                .Get(x => x.NODE_ID == pair.Key.NodeID && x.ORGANIZATION_TYPE == (byte)pair.Key.OrganizationType);

                dist.Add($"{pair.Key.NodeID}", param);
            });

            data.ForEach(item =>
            {
                var store = dist[$"{item.NodeID}"];

                if (store != null)
                {
                    var buCon = new MSSQLCondition<HEADQUARTERS_NODE>(x => x.NODE_ID == store.NodeID);
                    var nodeBu = _OrganizationAggregate.HeaderQuarterNode_T1_T2_.GetOfSpecific(buCon, x => x.BU_ID);
                    var buKeyCon = new MSSQLCondition<HEADQUARTERS_NODE>(x => x.NODE_ID == nodeBu);
                    var nodeKey = _OrganizationAggregate.HeaderQuarterNode_T1_T2_.GetOfSpecific(buKeyCon, x => x.NODE_KEY);

                    item.StoreType = store.StoreType;

                    if (DataStorage.StoreTypeDict.TryGetValue(nodeKey, out var collection))
                    {
                        item.StoreTypeName = collection.FirstOrDefault(x => x.Key == store.StoreType.ToString())?.Value;
                    }
                }
            });

            return data;
        }

        public T Resolve<T>(T data) where T : IStoreRelationship, new()
        {
            var param = _OrganizationAggregate.Store_T1_T2_.Get(x => x.NODE_ID == data.NodeID && x.ORGANIZATION_TYPE == (byte)data.OrganizationType);

            if (param != null)
            {
                var buCon = new MSSQLCondition<HEADQUARTERS_NODE>(x => x.NODE_ID == data.NodeID);
                var nodeBu = _OrganizationAggregate.HeaderQuarterNode_T1_T2_.GetOfSpecific(buCon, x => x.BU_ID);
                var buKeyCon = new MSSQLCondition<HEADQUARTERS_NODE>(x => x.NODE_ID == nodeBu);
                var nodeKey = _OrganizationAggregate.HeaderQuarterNode_T1_T2_.GetOfSpecific(buKeyCon, x => x.NODE_KEY);

                data.StoreType = param.StoreType;

                if (DataStorage.StoreTypeDict.TryGetValue(nodeKey, out var collection))
                {
                    data.StoreTypeName = collection.FirstOrDefault(x => x.Key == param.StoreType.ToString())?.Value;
                }

            }


            return data;
        }
    }
}
