using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;

namespace SMARTII.Domain.Master
{
    public interface IImportUserFactory
    {
        bool ImportUser(IXLWorksheet worksheet, out string ErrorMsg);
    }
}
