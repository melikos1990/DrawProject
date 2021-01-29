using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;

namespace SMARTII.Domain.Master
{
    public interface IImportQuestionClassificationFactory
    {
        bool ImportQuestionClassification(IXLWorksheet worksheet, string NodeKey, out string ErrorMessage);
    }
}
