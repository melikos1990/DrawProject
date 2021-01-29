using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMARTII.ICC.Domain;
using SMARTII.COMMON_BU;
using SMARTII.ICC;
using SMARTII.ICC.Domain;
using SMARTII.Service.Report.Builder;

namespace SMARTII.ICC.Service
{
    public class ReportProvider
    {
        private readonly ExcelBuilder _builder;

        public ReportProvider(ExcelBuilder builder)
        {
            _builder = builder;
        }

        #region ICC 匯出Excel來電紀錄

        /// <summary>
        /// ASO 來電紀錄
        /// </summary>
        /// <param name="model"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public byte[] GetOnCallExcel(InComm_DataList model, DateTime end)
        {
            _builder.Clear();
            var @byte = _builder
                .AddWorkSheet(new SummaryWorksheet(), model.SummaryInformation, end)
                .AddWorkSheet(new DailyMonthSummaryWorksheet(), model.DailySummary, "日")
                .AddWorkSheet(new DailyMonthSummaryWorksheet(), model.MonthSummary, "月")

                .AddWorkSheet(new InComm_CallWorksheet(), model.OnCallHistory, "來電紀錄")
                .AddWorkSheet(new InComm_EmailWorksheet(), model.OnEmailHistory, "來信紀錄")
                .AddWorkSheet(new InComm_CallWorksheet(), model.OnOtherHistory, "其他紀錄")
                .Build();
            return @byte;
        }
        #endregion
    }
}
