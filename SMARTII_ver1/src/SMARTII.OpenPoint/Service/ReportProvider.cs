using ClosedXML.Excel;
using System.Linq;
using MultipartDataMediaFormatter.Infrastructure;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;
using SMARTII.Domain.Report;
using SMARTII.OpenPoint.Domain;
using SMARTII.Service.Report.Provider;
using SMARTII.Domain.Master;
using System;
using SMARTII.Service.Report.Builder;
using SMARTII.COMMON_BU;

namespace SMARTII.OpenPoint.Service
{
    public class ReportProvider
    {
        private readonly ExcelBuilder _builder;
        public ReportProvider(ExcelBuilder builder)
        {
            _builder = builder;
        }


        /// <summary>
        /// OpenPoint 來電紀錄
        /// </summary>
        /// <param name="model"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public byte[] GetOnCallExcel(OpenPointDataList model, DateTime end)
        {
            _builder.Clear();
            var @byte = _builder
                .AddWorkSheet(new SummaryWorksheet(), model.SummaryInformation, end)
                .AddWorkSheet(new DailyMonthSummaryWorksheet(), model.DailySummary, "日")
                .AddWorkSheet(new DailyMonthSummaryWorksheet(), model.MonthSummary, "月")

                .AddWorkSheet(new OnCallWorksheet(), model.CommonOnCallsHistory, "來電紀錄")
                .AddWorkSheet(new OnEmailWorksheet(), model.CommonOnEmailHistory, "來信紀錄")
                .AddWorkSheet(new OnCallWorksheet(), model.CommonOnOtherHistory, "其他紀錄")
                .Build();
            return @byte;
        }
    }
}
