using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;

namespace SMARTII.Domain.Report
{
    public abstract class ExcelBaseBuilder
    {

        public XLWorkbook workbook { get; set; }


        public virtual ExcelBaseBuilder AddWorkSheet(IWorksheet worksheet, object payload = null, object extend = null)
        {
            
            worksheet.CreateWorksheet(this.workbook, payload, extend);

            return this;
        }
        

        public abstract byte[] Build();

        public void Clear()
        {
            workbook.Dispose();
            workbook = new XLWorkbook();
        }
    }
}
