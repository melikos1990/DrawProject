using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMARTII.ASO.Domain;
using SMARTII.Domain.Case;
using SMARTII.Domain.Master;
using SMARTII.Domain.Report;

namespace SMARTII.ASO.Service
{
    public class ASOExcelParser: IExcelParser
    {
        public ExcelCaseCustomerList CaseSearchItemParsing(List<CaseItem> data, ExcelCaseCustomerList exceldata)
        {
            var temp = exceldata.GetParticular<ASO_Case>().CaseItem ?? null;
            if (temp != null && temp.Count() > 0)
            {
                exceldata.OtherProductModel = string.Join("\t\n", temp.Select(x => x.ProductModel));
                exceldata.OtherProductName = string.Join("\t\n", temp.Select(x => x.ProductName));
                exceldata.OtherPurchaseDay = string.Join("\t\n", temp.Select(x => x.PurchaseDay?.ToString("yyyy-MM-dd HH:mm:ss")));
            }
            return exceldata;
        }
        public ExcelCaseHSList CaseSearchHSItemParsing(List<CaseItem> data, ExcelCaseHSList exceldata)
        {
            var temp = exceldata.GetParticular<ASO_Case>().CaseItem ?? null;
            if (temp != null && temp.Count() > 0)
            {
                exceldata.OtherProductModel = string.Join("\t\n", temp.Select(x => x.ProductModel));
                exceldata.OtherProductName = string.Join("\t\n", temp.Select(x => x.ProductName));
                exceldata.OtherPurchaseDay = string.Join("\t\n", temp.Select(x => x.PurchaseDay?.ToString("yyyy-MM-dd HH:mm:ss")));
            }
            return exceldata;
        }
        public ExcelCaseAssignmentList CaseSearchAssignmentItemParsing(List<CaseItem> data, ExcelCaseAssignmentList exceldata)
        {
            var temp = exceldata.GetParticular<ASO_Case>().CaseItem ?? null;
            if (temp != null && temp.Count() > 0)
            {
                exceldata.OtherProductModel = string.Join("\t\n", temp.Select(x => x.ProductModel));
                exceldata.OtherProductName = string.Join("\t\n", temp.Select(x => x.ProductName));
                exceldata.OtherPurchaseDay = string.Join("\t\n", temp.Select(x => x.PurchaseDay?.ToString("yyyy-MM-dd HH:mm:ss")));
            }
            return exceldata;
        }
    }
}
