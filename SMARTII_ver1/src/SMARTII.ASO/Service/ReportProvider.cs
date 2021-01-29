using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMARTII.ASO.Domain;
using SMARTII.COMMON_BU;
using SMARTII.Service.Report.Builder;

namespace SMARTII.ASO.Service
{
    public class ReportProvider
    {
        private readonly ExcelBuilder _builder;

        public ReportProvider(ExcelBuilder builder)
        {
            _builder = builder;
        }

        #region ASO 匯出Excel來電紀錄

        /// <summary>
        /// ASO 來電紀錄
        /// </summary>
        /// <param name="model"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public byte[] GetOnCallExcel(ASODataList model, DateTime end)
        {
            _builder.Clear();
            var @byte = _builder
                .AddWorkSheet(new SummaryWorksheet(), model.SummaryInformation, end)
                .AddWorkSheet(new DailyMonthSummaryWorksheet(), model.DailySummary, "日")
                .AddWorkSheet(new DailyMonthSummaryWorksheet(), model.MonthSummary, "月")

                .AddWorkSheet(new OnCallAndOtherWorksheet(), model.OnCallHistory, "來電紀錄")
                .AddWorkSheet(new OnEmailAndStoreWorksheet(), model.OnEmailHistory, "來信紀錄")
                .AddWorkSheet(new OnEmailAndStoreWorksheet(), model.OnStoreHistory, "門市來信紀錄")
                .AddWorkSheet(new OnCallAndOtherWorksheet(), model.OnOtherHistory, "其他紀錄")
                .Build();
            return @byte;
        }

        /// <summary>
        /// ASO 來電紀錄(時效)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public byte[] GetAssignmentOnCallExcel(ASOAssignmentDataList model, DateTime end)
        {
            _builder.Clear();
            var @byte = _builder
                .AddWorkSheet(new SummaryWorksheet(), model.SummaryInformation, end)
                .AddWorkSheet(new DailyMonthSummaryWorksheet(), model.DailySummary, "日")
                .AddWorkSheet(new DailyMonthSummaryWorksheet(), model.MonthSummary, "月")

                .AddWorkSheet(new AssignmentOnCallAndOtherWorksheet(), model.OnCallHistory, "來電紀錄")
                .AddWorkSheet(new AssignmentOnEmailAndStoreWorksheet(), model.OnEmailHistory, "來信紀錄")
                .AddWorkSheet(new AssignmentOnEmailAndStoreWorksheet(), model.OnStoreHistory, "門市來信紀錄")
                .AddWorkSheet(new AssignmentOnCallAndOtherWorksheet(), model.OnOtherHistory, "其他紀錄")
                .Build();
            return @byte;
        }

        /// <summary>
        /// ASO客服日報
        /// </summary>
        /// <param name="model"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public byte[] GetEveryDayExcel(List<DailyReport> model, DateTime start, DateTime end)
        {
            List<DateTime> dateTimes = new List<DateTime>();
            dateTimes.Add(start);
            dateTimes.Add(end);
            _builder.Clear();
            var @byte = _builder
                .AddWorkSheet(new EveryDayWorksheet(), model, dateTimes)
                .Build();
            return @byte;
        }
        #endregion
    }
}
