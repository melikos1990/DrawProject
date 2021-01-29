using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoreLinq;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Case;

namespace SMARTII.Service.Case.Resolver
{
    public class CaseResolver
    {
        private readonly ICaseAggregate _CaseAggregate;

        public CaseResolver(ICaseAggregate CaseAggregate)
        {
            _CaseAggregate = CaseAggregate;
        }

        public IEnumerable<T> ResolveCollection<T>(IEnumerable<T> data,Action<MSSQLCondition<CASE>> action) where T : ICaseRelationship, new()
        {
            IDictionary<string, Domain.Case.Case> dist = new Dictionary<string, Domain.Case.Case>();

            var group = data.GroupBy(x => new
            {
                ID = x.CaseID
            });

            var con = new MSSQLCondition<CASE>();

            action(con);

            group.ForEach(pair =>
            {
                con.And(x => x.CASE_ID == pair.Key.ID);


                var @case = _CaseAggregate.Case_T1_T2_.GetFirstOrDefault(con);

                if (@case == null) return;

                dist.Add($"{pair.Key.ID}", @case);

                con.ClearFilters();
            });

            data.ForEach(item =>
            {
                var key = $"{item.CaseID}";

                if (dist.TryGetValue(key, out var value))
                {
                    item.@case = value;
                    item.CaseID = value.CaseID;
                }

            });

            return data;
        }

        public T Resolve<T>(T data, Action<MSSQLCondition<CASE>> action) where T : ICaseRelationship, new()
        {
            var con = new MSSQLCondition<CASE>(x => x.CASE_ID == data.CaseID);

            action(con);

            var @case = _CaseAggregate.Case_T1_T2_.GetFirstOrDefault(con);

            if (@case != null)
            {

                data.CaseID = @case.CaseID;
                data.@case = @case;
            }

            return data;
        }


    }
}
