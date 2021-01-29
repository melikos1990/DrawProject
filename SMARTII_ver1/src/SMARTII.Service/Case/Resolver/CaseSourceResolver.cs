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
    public class CaseSourceResolver
    {
        private readonly ICaseAggregate _CaseAggregate;

        public CaseSourceResolver(ICaseAggregate CaseAggregate)
        {
            _CaseAggregate = CaseAggregate;
        }

        public IEnumerable<T> ResolveCollection<T>(IEnumerable<T> data) where T : ICaseSourceRelationship, new()
        {
            IDictionary<string, CaseSource> dist = new Dictionary<string, CaseSource>();

            var group = data.GroupBy(x => new
            {
                ID = x.SourceID
            });

            var con = new MSSQLCondition<CASE_SOURCE>();
            con.IncludeBy(x => x.CASE_SOURCE_USER);

            group.ForEach(pair =>
            {
                con.And(x => x.SOURCE_ID == pair.Key.ID);
               

                var caseSource = _CaseAggregate.CaseSource_T1_T2_.GetFirstOrDefault(con);

                if (caseSource == null) return;

                dist.Add($"{pair.Key.ID}", caseSource);

                con.ClearFilters();
            });

            data.ForEach(item =>
            {
                var key = $"{item.SourceID}";

                if (dist.TryGetValue(key, out var value))
                {
                    item.CaseSource = value;
                    item.SourceID = value.SourceID;
                }

            });

            return data;
        }

        public T Resolve<T>(T data) where T : ICaseSourceRelationship, new()
        {
            var con = new MSSQLCondition<CASE_SOURCE>(x => x.SOURCE_ID == data.SourceID);
            con.IncludeBy(x => x.CASE_SOURCE_USER);
            var caseSource = _CaseAggregate.CaseSource_T1_T2_.GetFirstOrDefault(con);

            if (caseSource != null)
            {

                data.SourceID = caseSource.SourceID;
                data.CaseSource = caseSource;
            }

            return data;
        }


    }
}
