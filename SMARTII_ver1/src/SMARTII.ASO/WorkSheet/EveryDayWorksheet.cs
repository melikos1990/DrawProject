using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClosedXML.Excel;
using SMARTII.Domain.Case;
using SMARTII.Domain.Report;

namespace SMARTII.ASO
{
    public class EveryDayWorksheet : IWorksheet
    {
        public void CreateWorksheet(XLWorkbook book, object payload, object extend = null)
        {
            var data = (List<DailyReport>)payload;
            var Time = (List<DateTime>)extend;

            var startTime = Time[0];
            var endTime = Time[1];
            foreach (var day in EachDay(startTime, endTime))
            {
                var endData = day.AddDays(1).AddSeconds(-1);
                var item = data.Where(x => x.CreateDateTime > day && x.CreateDateTime < endData);
                string time = day.ToString("MMdd");
                var ws = book.AddWorksheet(time);

                //計算數量
                var callCount = item.Where(x => x.CaseSourceType == CaseSourceType.Phone).Count();
                var officialCount = item.Where(x => x.CaseSourceType == CaseSourceType.Email || x.CaseSourceType == CaseSourceType.Other || x.CaseSourceType == CaseSourceType.StoreEmail).Count();

                int allCount = (callCount + officialCount);
                double callPercentage = Convert.ToDouble(callCount) / allCount;
                double officialPercentage = Convert.ToDouble(officialCount) / allCount;


                ws.Range(1, 1, 1, 4).Merge();
                ws.Cell(1, 1).Value = "客服日報【Customer Service Daily Report】";
                ws.Cell(1, 1).Style.Font.Bold = true;
                ws.Cell(1, 5).Value = "日期";
                ws.Cell(1, 5).Style.Font.Bold = true;
                ws.Cell(1, 6).Value = "'" + day.ToString("yyyy/MM/dd");
                ws.Cell(1, 6).Style.Font.Bold = true;

                var rngTable1 = ws.Range(1, 1, 1, 6);
                rngTable1.Style.Fill.BackgroundColor = XLColor.FromArgb(192, 192, 192);

                ws.Range(1, 5, 1, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                ws.Range(2, 1, 2, 6).Merge();
                ws.Cell(2, 1).Value = "當日案件諮詢數量";
                ws.Cell(2, 1).Style.Fill.BackgroundColor = XLColor.FromArgb(0, 102, 204);
                ws.Cell(2, 1).Style.Font.Bold = true;
                ws.Cell(2, 1).Style.Font.FontColor = XLColor.FromArgb(255, 255, 255);

                ws.Range(3, 1, 3, 2).Merge();
                ws.Range(3, 3, 3, 4).Merge();
                ws.Range(3, 5, 3, 6).Merge();
                ws.Cell(3, 1).Value = "類別";
                ws.Cell(3, 1).Style.Font.Bold = true;
                ws.Cell(3, 3).Value = "0800客服專線";
                ws.Cell(3, 3).Style.Font.Bold = true;
                ws.Cell(3, 5).Value = "官網來信";
                ws.Cell(3, 5).Style.Font.Bold = true;

                ws.Range(4, 1, 4, 2).Merge();
                ws.Range(4, 3, 4, 4).Merge();
                ws.Range(4, 5, 4, 6).Merge();
                ws.Cell(4, 1).Value = "案件諮詢量 ( Incoming Volume )";
                ws.Cell(4, 5).Value = officialCount;

                ws.Range(5, 1, 5, 2).Merge();
                ws.Range(5, 3, 5, 4).Merge();
                ws.Range(5, 5, 5, 6).Merge();
                ws.Cell(5, 1).Value = "案件服務量 ( Answer Volume )";
                ws.Cell(5, 3).Value = callCount;
                ws.Cell(5, 5).Value = officialCount;

                ws.Range(6, 1, 6, 2).Merge();
                ws.Range(6, 3, 6, 4).Merge();
                ws.Range(6, 5, 6, 6).Merge();
                ws.Cell(6, 1).Value = "回應率 ( Answer Rate )";
                ws.Cell(6, 5).Value = "100%";

                ws.Range(7, 1, 7, 2).Merge();
                ws.Range(7, 3, 7, 4).Merge();
                ws.Range(7, 5, 7, 6).Merge();
                ws.Cell(7, 1).Value = "當日案件分佈比例 ( Cases distribution ) ";
                ws.Cell(7, 3).Value = allCount == 0 ? "" : string.Format("{0:0.0%}", callPercentage);
                ws.Cell(7, 5).Value = allCount == 0 ? "" : string.Format("{0:0.0%}", officialPercentage);

                ws.Range(8, 1, 8, 6).Merge();
                ws.Cell(8, 1).Value = "顧客反應問題分類";
                ws.Cell(8, 1).Style.Font.Bold = true;
                ws.Cell(8, 1).Style.Fill.BackgroundColor = XLColor.FromArgb(0, 102, 204);
                ws.Cell(8, 1).Style.Font.FontColor = XLColor.FromArgb(255, 255, 255);

                ws.Cell(9, 1).Value = "排名";
                ws.Cell(9, 2).Value = "大分類";
                ws.Cell(9, 3).Value = "細項分類";
                ws.Cell(9, 4).Value = "數量";
                ws.Cell(9, 5).Value = "小計";
                ws.Cell(9, 6).Value = "%";

                int row = 9;
                int rankRow = 1;
                var list = item.GroupBy(x => x.ParentName).OrderByDescending(g => g.Count()).ToList();
                foreach (var rank in list)
                {
                    //大分類
                    var df = rank.ToList();
                    var childrenList = rank.ToList().GroupBy(x => x.ChildrenName);
                    var childrenCount = childrenList.Sum(x => x.Count());
                    int rowTemp = row + childrenList.Count();


                    //排名
                    ws.Range(row + 1, 1, rowTemp, 1).Merge();
                    ws.Cell(row + 1, 1).Value = rankRow;

                    ws.Range(row + 1, 2, rowTemp, 2).Merge();
                    ws.Cell(row + 1, 2).Value = rank.Key;

                    int smallRow = row;
                    foreach (var small in childrenList)
                    {
                        ws.Cell(smallRow + 1, 3).Value = small.Key;
                        ws.Cell(smallRow + 1, 4).Value = small.Count();

                        smallRow++;
                    }
                    ws.Range(row + 1, 5, rowTemp, 5).Merge();
                    ws.Cell(row + 1, 5).Value = childrenCount;

                    double proportion = Convert.ToDouble(childrenCount) / allCount;
                    ws.Range(row + 1, 6, rowTemp, 6).Merge();
                    ws.Cell(row + 1, 6).Value = string.Format("{0:0%}", proportion); ;

                    row = rowTemp;
                    rankRow++;
                }
                ws.Range(row + 1, 1, row + 1, 3).Merge();
                ws.Cell(row + 1, 1).Value = "總計";

                ws.Range(row + 1, 4, row + 1, 6).Merge();
                ws.Cell(row + 1, 4).Value = allCount;

                ws.Range(2, 1, row + 1, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Range(2, 1, row + 1, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                //格線
                var rngTable3 = ws.Range(1, 1, row + 1, 6);
                rngTable3.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                rngTable3.Style.Border.InsideBorder = XLBorderStyleValues.Dotted;
                //自動對應欄位寬度
                ws.Rows().AdjustToContents();
                //欄位寬度
                ws.Columns(1, 6).Width = 20;
            }
        }

        private static IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }
    }
}
