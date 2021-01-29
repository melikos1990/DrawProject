using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMARTII.COMMON_BU;
using SMARTII.Service.Report.Builder;

namespace SMARTII.EShop.Service
{
    public class ReportProvider
    {
        private readonly ExcelBuilder _builder;

        public ReportProvider(ExcelBuilder builder)
        {
            _builder = builder;
        }


        public Byte[] GetOnCallExcel(EShopDataList model, DateTime end)
        {
            var @byte = _builder
                .AddWorkSheet(new SummaryWorksheet(), model.SummaryInformation, end)
                .AddWorkSheet(new DailyMonthSummaryWorksheet(), model.DailySummary, "日")
                .AddWorkSheet(new DailyMonthSummaryWorksheet(), model.MonthSummary, "月")

                .AddWorkSheet(new EShopCallHistoryWorksheet(), model.OnCallHistory, "來電紀錄")
                .AddWorkSheet(new EShopEmailHistoryWorksheet(), model.OnEmailHistory, "來信紀錄")
                .AddWorkSheet(new EShopCallHistoryWorksheet(), model.OnOtherHistory, "其他紀錄")
                .Build();
            return @byte;
        }
    }
}
