using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Report;


namespace SMARTII.OpenPoint.Service
{
    public class OpenPointFactory: IOpenPointFactory
    {
        private readonly OpenPointFacade _OpenPointFacade;
        private readonly ReportProvider _ReportProvider;

        public OpenPointFactory(OpenPointFacade OpenPointFacade, ReportProvider ReportProvider)
        {
            _OpenPointFacade = OpenPointFacade;
            _ReportProvider = ReportProvider;
        }
        /// <summary>
        /// OpenPoint 來電紀錄
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public async Task<byte[]> GetOnCallExcel(DateTime start, DateTime end)
        {
            //Facade 找資料SP
            var list = await _OpenPointFacade.GetOnCallExcel(start, end, EssentialCache.BusinessKeyValue.OpenPoint);
            //匯出excel
            var result = _ReportProvider.GetOnCallExcel(list, end);
            return result;
        }
    }
}
