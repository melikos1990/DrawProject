using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMARTII.Domain.Report
{
    public interface IQuestionClassificationReportProvider
    {
        /// <summary>
        /// 問題分類-匯出
        /// </summary>
        /// <returns></returns>
        Byte[] CreateQuestionClassificationExcel(List<QuestionClassificationForExcel> DataList);
    }
}
