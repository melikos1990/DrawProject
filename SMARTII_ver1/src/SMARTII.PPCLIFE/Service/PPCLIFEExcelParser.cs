using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using SMARTII.Domain.Case;
using SMARTII.Domain.Master;
using SMARTII.Domain.Report;
using SMARTII.PPCLIFE.Domain;

namespace SMARTII.PPCLIFE.Service
{
    public class PPCLIFEExcelParser : IExcelParser
    {
        public ExcelCaseCustomerList CaseSearchItemParsing(List<CaseItem> data, ExcelCaseCustomerList exceldata)
        {
            var temp = data?.Select(x => x.GetParticular<PPCLIFE_CaseItem>()) ?? null;
            if (temp != null && temp.Count() > 0)
            {
                exceldata.OtherBatchNo = string.Join("\t\n", temp.Select(x => x.BatchNo));
                var itemList = data?.Where(x => x.Item != null).Select(c => JsonConvert.DeserializeObject<PPCLIFE_Item>(((Item<ExpandoObject>)c.Item).JContent));
                if (itemList != null)
                    exceldata.OtherInternationalBarcode = string.Join("\t\n", itemList.Select(c => c.InternationalBarcode));
                exceldata.OtherCommodityName = string.Join("\t\n", data.Where(x => x.Item != null).Select(c => c.Item.Name));
            }
            return exceldata;
        }
        public ExcelCaseHSList CaseSearchHSItemParsing(List<CaseItem> data, ExcelCaseHSList exceldata)
        {
            var temp = data?.Select(x => x.GetParticular<PPCLIFE_CaseItem>()) ?? null;
            if (temp != null && temp.Count() > 0)
            {
                exceldata.OtherBatchNo = string.Join("\t\n", temp.Select(x => x.BatchNo));
                var itemList = data?.Where(x => x.Item != null).Select(c => JsonConvert.DeserializeObject<PPCLIFE_Item>(((Item<ExpandoObject>)c.Item).JContent));
                if (itemList != null)
                    exceldata.OtherInternationalBarcode = string.Join("\t\n", itemList.Select(c => c.InternationalBarcode));
                exceldata.OtherCommodityName = string.Join("\t\n", data.Where(x => x.Item != null).Select(c => c.Item.Name));
            }
            return exceldata;
        }
        public ExcelCaseAssignmentList CaseSearchAssignmentItemParsing(List<CaseItem> data, ExcelCaseAssignmentList exceldata)
        {
            var temp = data?.Select(x => x.GetParticular<PPCLIFE_CaseItem>()) ?? null;
            if (temp != null && temp.Count() > 0)
            {
                exceldata.OtherBatchNo = string.Join("\t\n", temp.Select(x => x.BatchNo));
                var itemList = data?.Where(x => x.Item != null).Select(c => JsonConvert.DeserializeObject<PPCLIFE_Item>(((Item<ExpandoObject>)c.Item).JContent));
                if (itemList != null)
                    exceldata.OtherInternationalBarcode = string.Join("\t\n", itemList.Select(c => c.InternationalBarcode));
                exceldata.OtherCommodityName = string.Join("\t\n", data.Where(x => x.Item != null).Select(c => c.Item.Name));
            }
            return exceldata;
        }

    }
}
