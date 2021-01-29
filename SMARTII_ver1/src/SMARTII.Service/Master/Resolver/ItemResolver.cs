using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using SMARTII.Domain.Master;

namespace SMARTII.Service.Master.Resolver
{
    public class ItemResolver
    {
        private readonly IMasterAggregate _MasterAggregate;

        public ItemResolver(IMasterAggregate MasterAggregate)
        {
            _MasterAggregate = MasterAggregate;
        }

        public IEnumerable<T> ResolveCollection<T>(IEnumerable<T> data) where T : IItemRelationship, new()
        {
            IDictionary<string, Item> dist = new Dictionary<string, Item>();

            var group = data.GroupBy(x => new
            {
                ID = x.ItemID
            });

            group.ForEach(pair =>
            {
                Item item = _MasterAggregate.Item_T1_T2_.Get(x => x.ID == pair.Key.ID);

                dist.Add($"{pair.Key.ID}", item);
            });

            data.ForEach(x =>
            {
                var item = dist[$"{x.ItemID}"];

                x.ItemID = item.ID;
                x.ItemName = item.Name;
            });

            return data;
        }

        public T Resolve<T>(T data) where T : IItemRelationship, new()
        {
            Item item = _MasterAggregate.Item_T1_T2_.Get(x => x.ID == data.ItemID);

            data.ItemID = item.ID;
            data.ItemName = item.Name;

            return data;
        }

        public IEnumerable<T> ResolveNullableCollection<T>(IEnumerable<T> data) where T : INullableItemRelationship, new()
        {
            IDictionary<string, Item> dist = new Dictionary<string, Item>();

            var group = data.Where(x => x.ItemID.HasValue)
                            .GroupBy(x => new
                            {
                                ID = x.ItemID
                            });

            group.ForEach(pair =>
            {
                Item item = _MasterAggregate.Item_T1_T2_.Get(x => x.ID == pair.Key.ID);

                dist.Add($"{pair.Key.ID}", item);
            });

            data.ForEach(x =>
            {
                if (x.ItemID.HasValue)
                {
                    var item = dist[$"{x.ItemID}"];

                    x.ItemID = item.ID;
                    x.ItemName = item.Name;
                }
            });

            return data;
        }

        public T ResolveNullable<T>(T data) where T : INullableItemRelationship, new()
        {
            if (data.ItemID.HasValue)
            {
                Item item = _MasterAggregate.Item_T1_T2_.Get(x => x.ID == data.ItemID);
                data.ItemID = item.ID;
                data.ItemName = item.Name;
            }

            return data;
        }
    }
}
