using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoreLinq;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Case;
using SMARTII.Domain.Master;

namespace SMARTII.Service.Master.Resolver
{
    public class CaseRemindResolver
    {
        
        private readonly ICaseAggregate _CaseAggregate;

        public CaseRemindResolver(ICaseAggregate CaseAggregate)
        {
            _CaseAggregate = CaseAggregate;
        }


        public IEnumerable<T> ResolveCollection<T>(IEnumerable<T> datas) where T : ICaseRemindRelationship, new()
        {

            var con = new MSSQLCondition<CASE_REMIND>();

            datas.ForEach(x => con.Or(g => g.CASE_ID == x.CaseID && g.ASSIGNMENT_ID == x.AssignmentID));

            var group = _CaseAggregate.CaseRemind_T1_T2_.GetList(con).GroupBy(x => $"{x.CaseID}-{x.AssignmentID}");

            datas.ForEach(data =>
            {
                var _caseReminds = group.FirstOrDefault(x => x.Key == $"{data.CaseID}-{data.AssignmentID}");
                data.CaseReminds = _caseReminds?.ToList();
            });
            

            return datas;
        }


        public T Resolve<T>(T data) where T : ICaseRemindRelationship, new()
        {

            var con = new MSSQLCondition<CASE_REMIND>(x =>
                                x.CASE_ID == data.CaseID && 
                                x.ASSIGNMENT_ID == data.AssignmentID);

            var list = _CaseAggregate.CaseRemind_T1_T2_.GetList(con);

            data.CaseReminds = list.ToList();

            return data;
        }

    }
}
