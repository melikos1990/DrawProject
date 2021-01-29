using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;

namespace SMARTII.Domain.Report
{
    public interface IWorksheet
    {
        /// <summary>
        /// 建立 Excel Sheet
        /// </summary>
        /// <param name="book"></param>
        /// <param name="payload">Sheet的Domain物件</param>
        void CreateWorksheet(XLWorkbook book, object payload , object extend = null);
    }
}
