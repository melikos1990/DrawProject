using System;
using System.Collections.Generic;
using System.Linq;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Case;
using SMARTII.PPCLIFE.Domain;

namespace SMARTII.PPCLIFE.Service
{
    public class PPCLifeNotificationBase
    {
        public List<PPCLifeBatchRule> GeneratePPCLifeRule(List<CaseItem> actualCases)
        {
            return actualCases
                 .Select(x => new PPCLifeBatchRule()
                 {
                     ItemID = x.ItemID,
                     BatchNo = x.GetParticular<PPCLIFE_CaseItem>().BatchNo ?? ""

                 }).ToList();
        }

        public List<CasePPCLife> GenerateCasePPCLife(List<CaseItem> actualCases)
        {
            return actualCases
                 .Select(x => new CasePPCLife()
                 {
                     ItemID = x.ItemID,
                     CaseID = x.CaseID,

                 }).ToList();
        }
    }
}
