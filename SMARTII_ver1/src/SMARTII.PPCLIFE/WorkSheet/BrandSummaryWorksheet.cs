
using ClosedXML.Excel;
using SMARTII.Domain.Case;
using SMARTII.Domain.Report;
using SMARTII.PPCLIFE.Domain;
using SMARTII.PPCLIFE.Domain.DataList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static SMARTII.Domain.Cache.EssentialCache;

namespace SMARTII.PPCLIFE
{
    public class BrandSummaryWorksheet : IWorksheet
    {
        public void CreateWorksheet(XLWorkbook book, object payload, object extend = null)
        {
            var data = (PPCLIFEBrandCalcDataList)payload;
            var fieldList = data.FieldList;
            var startTime = (DateTime)extend;
            var replyID = data.ReplyID;
            var factorsID = data.FactorsID;
            var endTime = startTime.AddMonths(1).AddDays(-1);
            //匯整
            var ws = book.AddWorksheet("匯整");
            string header = startTime.ToString("yyyy") + "年" + startTime.ToString("MM") + "月數據統計(" + startTime.ToString("MMdd") + "-" + endTime.ToString("MMdd") + ")";


            ws.Range(1, 2, 1, 4 + fieldList.Count()).Merge();
            ws.Cell(1, 2).Value = header;

            //格線
            var rngTable = ws.Range(1, 2, 1, 4 + fieldList.Count());
            rngTable.Style.Border.TopBorder = XLBorderStyleValues.Thick;
            rngTable.Style.Border.LeftBorder = XLBorderStyleValues.Thick;
            rngTable.Style.Border.RightBorder = XLBorderStyleValues.Thick;

            #region 開頭

            ws.Cell(2, 1).Value = "品牌/問題/件數";
            ws.Cell(2, 2).Value = "總案件數";
            ws.Cell(2, 3).Value = "一般案件數";
            ws.Cell(2, 4).Value = "客訴單";
            int column = 5;
            foreach (var item in fieldList.Where(x => x.ClassificationID == replyID))
            {
                ws.Cell(2, column).Value = item.Text;
                column++;
            }

            foreach (var item in fieldList.Where(x => x.ClassificationID == factorsID))
            {
                ws.Cell(2, column).Value = item.Text;
                column++;
            }
            ws.Cell(2, column).Value = "佔比";
            ws.Cell(2, column + 1).Value = "服務費八萬";
            ws.Range(2, 1, 2, column + 1).Style.Fill.BackgroundColor = XLColor.FromArgb(192, 192, 192);


            #endregion

            //計算總和
            var totalCaseCount = data.BrandSummaryCalc.Where(x=>x.NodeID != 0).Sum(x => x.TotalCaseCount);
            var generalCaseCount = data.BrandSummaryCalc.Where(x => x.NodeID != 0).Sum(x => x.GeneralCaseCount);
            var complaintInvoiceCount = data.BrandSummaryCalc.Where(x => x.NodeID != 0).Sum(x => x.ComplaintInvoiceCount);
            int row = 3;
            foreach (var item in data.BrandSummaryCalc)
            {
                ws.Cell(row, 1).Value = item.NodeName;
                ws.Cell(row, 2).Value = item.TotalCaseCount;
                ws.Cell(row, 3).Value = item.GeneralCaseCount;
                ws.Cell(row, 4).Value = item.ComplaintInvoiceCount;
                int cColumn = 5;
                foreach (var rr in fieldList.Where(x => x.ClassificationID == replyID))
                {
                    var replyCount = item.ReplyCustomerList.Where(x => x.ID == rr.ID);
                    if (replyCount.Any())
                    {
                        ws.Cell(row, cColumn).Value = replyCount.Count();
                    }
                    else
                    {
                        ws.Cell(row, cColumn).Value = 0;
                    }
                    cColumn++;
                }
                foreach (var rf in fieldList.Where(x => x.ClassificationID == factorsID))
                {
                    var causeProblemCount = item.CauseProblemList.Where(x => x.ID == rf.ID);
                    if (causeProblemCount.Any())
                    {
                        ws.Cell(row, cColumn).Value = causeProblemCount.Count();
                    }
                    else
                    {
                        ws.Cell(row, cColumn).Value = 0;
                    }
                    cColumn++;
                }

                // 過濾無被反應者
                if (item.NodeID != 0)
                {
                    double value = Convert.ToDouble(item.TotalCaseCount) / totalCaseCount;
                    //小数据取两位
                    string result = string.Format("{0:0.00%}", value);//得到5.88%
                    ws.Cell(row, cColumn).Value = result;
                    ws.Cell(row, cColumn + 1).Value = Math.Round((value * 80000), 0);
                }
                else
                {
                    double value = Convert.ToDouble(item.TotalCaseCount) / totalCaseCount;
                    //小数据取两位
                    string result = string.Format("{0:0.00%}", value);
                    ws.Cell(row, cColumn).Value = result;
                    ws.Cell(row, cColumn + 1).Value = Math.Round((value * 80000), 0);
                }

                row++;
            }

            //Total
            ws.Cell(row, 1).Value = "Total";
            ws.Cell(row, 2).Value = totalCaseCount;
            ws.Cell(row, 3).Value = generalCaseCount;
            ws.Cell(row, 4).Value = complaintInvoiceCount;
            int totalColumn = 5;
            foreach (var tt in fieldList.Where(x => x.ClassificationID == replyID))
            {
                var allCount = data.BrandSummaryCalc.Where(x => x.NodeID != 0).SelectMany(x => x.ReplyCustomerList).Where(y => y.ID == tt.ID).Count();
                ws.Cell(row, totalColumn).Value = allCount;
                totalColumn++;
            }
            foreach (var rf in fieldList.Where(x => x.ClassificationID == factorsID))
            {
                var allCount = data.BrandSummaryCalc.Where(x => x.NodeID != 0).SelectMany(x => x.CauseProblemList).Where(y => y.ID == rf.ID).Count();
                ws.Cell(row, totalColumn).Value = allCount;
                totalColumn++;
            }
            var rngTable2 = ws.Range(row, 1, row, totalColumn - 1);
            rngTable2.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            rngTable2.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            rngTable2.Style.Fill.BackgroundColor = XLColor.FromArgb(192, 192, 192);
            ws.Range(2, 1, row, 2).Style.Fill.BackgroundColor = XLColor.FromArgb(192, 192, 192);
            ws.Cell(row, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            //左右至中
            ws.Range(1, 1, row - 1, column + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range(3, 2, row - 1, column + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

            //水平 至中
            ws.Range(1, 1, row - 1, column + 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range(3, 2, row - 1, column + 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

            //字體大小
            ws.Range(2, 1, row - 1, column + 1).Style.Font.FontSize = 10;
            ws.Range(1, 1, 1, column + 1).Style.Font.FontSize = 20;
            //格線
            var rngTable3 = ws.Range(2, 1, row - 1, column + 1);
            rngTable3.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            rngTable3.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            //自動對應欄位寬度
            ws.Rows().AdjustToContents();
            ws.Columns().AdjustToContents();

        }
    }
}
