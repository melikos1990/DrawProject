
using ClosedXML.Excel;
using SMARTII.Domain.Report;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SMARTII.Service.Report.Builder
{
    public class ExcelBuilder : ExcelBaseBuilder
    {

        public ExcelBuilder()
        {
            this.workbook = new XLWorkbook();
        }
        

        public override byte[] Build()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream); 
                
                return memoryStream.ToArray();
            }
        }
    }
}
