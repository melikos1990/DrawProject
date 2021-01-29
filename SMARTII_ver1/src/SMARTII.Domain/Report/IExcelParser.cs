using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMARTII.Domain.Case;

namespace SMARTII.Domain.Report
{
    public interface IExcelParser
    {
        ExcelCaseCustomerList CaseSearchItemParsing(List<CaseItem> data, ExcelCaseCustomerList exceldata);

        ExcelCaseHSList CaseSearchHSItemParsing(List<CaseItem> data, ExcelCaseHSList exceldata);

        ExcelCaseAssignmentList CaseSearchAssignmentItemParsing(List<CaseItem> data, ExcelCaseAssignmentList exceldata);
    }
}
