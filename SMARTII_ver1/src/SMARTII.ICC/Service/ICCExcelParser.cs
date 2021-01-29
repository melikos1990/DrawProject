using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMARTII.Domain.Case;
using SMARTII.Domain.Report;
using SMARTII.ICC.Domain;

namespace SMARTII.ICC.Service
{
    public class ICCExcelParser : IExcelParser
    {
        public ExcelCaseCustomerList CaseSearchItemParsing(List<CaseItem> data, ExcelCaseCustomerList exceldata)
        {
            var temp = exceldata.GetParticular<InComm_Case>().CaseItem ?? null;
            if (temp != null && temp.Count() > 0)
            {
                exceldata.OtherCardNumber = string.Join("\t\n", temp.Select(x => x.CardNumber));
            }
            return exceldata;
        }
        public ExcelCaseHSList CaseSearchHSItemParsing(List<CaseItem> data, ExcelCaseHSList exceldata)
        {
            var temp = exceldata.GetParticular<InComm_Case>().CaseItem ?? null;
            if (temp != null && temp.Count() > 0)
            {
                exceldata.OtherCardNumber = string.Join("\t\n", temp.Select(x => x.CardNumber));
            }
            return exceldata;
        }
        public ExcelCaseAssignmentList CaseSearchAssignmentItemParsing(List<CaseItem> data, ExcelCaseAssignmentList exceldata)
        {
            var temp = exceldata.GetParticular<InComm_Case>().CaseItem ?? null;
            if (temp != null && temp.Count() > 0)
            {
                exceldata.OtherCardNumber = string.Join("\t\n", temp.Select(x => x.CardNumber));
            }
            return exceldata;
        }
        
    }
}
