using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;

namespace SMARTII.Domain.Report
{
    public interface IImportStoreFactory
    {
        /// <summary>
        /// 匯入門市
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns></returns>
        bool ImportStore(IXLWorksheet worksheet, string NodeKey ,out string ErrorMessage);
        /// <summary>
        /// 匯入門市人員
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns></returns>

        bool ImportStorePersonnel(IXLWorksheet worksheet, string NodeKey, out string ErrorMessage);
        /// <summary>
        /// 匯入門市資訊
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns></returns>
        bool ImportStoreInformation(IXLWorksheet worksheet,string BuName, out string ErrorMessage);
    }
}
