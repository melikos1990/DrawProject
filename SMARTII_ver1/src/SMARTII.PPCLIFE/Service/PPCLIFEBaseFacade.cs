using System;
using System.Collections.Generic;
using System.Linq;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Case;
using SMARTII.Domain.Master;
using SMARTII.Domain.Types;
using SMARTII.PPCLIFE.Domain;

namespace SMARTII.PPCLIFE.Service
{
    public class PPCLIFEBaseFacade
    {
        public List<CasePPCLife> GenerateCasePPCLife(List<CaseItem> actualCases)
        {
            return actualCases
                 .Select(x => new CasePPCLife()
                 {
                     ItemID = x.ItemID,
                     CaseID = x.CaseID,
                     IsIgnore = false,
                     AllSameFinish = false,
                     DiffBatchNoFinish = false,
                     NothingBatchNoFinish = false,
                     CreateDateTime = DateTime.Now,
                     CreateUserName = GlobalizationCache.APName,

                 }).ToList();

        }

        public PPCLIFE_Item GeneratePPCLIFEItem(Item<System.Dynamic.ExpandoObject> actualItem)
        {
            var result = new PPCLIFE_Item();

            if(actualItem == null)
                return null;

            result.Name = actualItem.Name ?? "無資訊";
            result.InternationalBarcode = actualItem.Particular.CastTo<PPCLIFE_Item>().InternationalBarcode ?? "無資訊";

            return result;
        }
    }
}
