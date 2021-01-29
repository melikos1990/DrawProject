using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using SMARTII.Domain.System;

namespace SMARTII.Service.System.Resolver
{
    public class SystemParameterResolver
    {
        private readonly ISystemAggregate _SystemAggregate;

        public SystemParameterResolver(ISystemAggregate SystemAggregate)
        {
            _SystemAggregate = SystemAggregate;
        }

        public IEnumerable<T> ResolveCollection<T>(IEnumerable<T> data) where T : ISystemParameterRelationship, new()
        {
            IDictionary<string, SystemParameter> dist = new Dictionary<string, SystemParameter>();

            var group = data.GroupBy(x => new
            {
                ID = x.ID,
                Key = x.Key
            });

            group.ForEach(pair =>
            {
                SystemParameter param = _SystemAggregate
                .SystemParameter_T1_T2_
                .Get(x => x.ID == pair.Key.ID && x.KEY == pair.Key.Key);

                dist.Add($"{pair.Key.ID}-{pair.Key.Key}", param);
            });

            data.ForEach(item =>
            {
                var node = dist[$"{item.ID}-{item.Key}"];

                item.Value = node?.Value;
                item.Text = node?.Text;
            });

            return data;
        }

        public T Resolve<T>(T data) where T : ISystemParameterRelationship, new()
        {
            SystemParameter param = _SystemAggregate
             .SystemParameter_T1_T2_
             .Get(x => x.ID == data.ID && x.KEY == data.Key);

            data.Value = param.Value;
            data.Text = param.Text;

            return data;
        }
    }
}