using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;
using SMARTII.Domain.Report;

namespace SMARTII.Service.Report.Provider
{
    public class BatchReportProvider : IBatchReportProvider
    {
        public byte[] GenerateCaseRemindReport(List<CaseRemind> caseReminds)
        {

            XLWorkbook book = new XLWorkbook();

            var ws = book.AddWorksheet("逾時未完成清單");

            ws.Cell(1, 1).Value = "案件編號";
            ws.Cell(1, 2).Value = "序號";
            ws.Cell(1, 3).Value = "通知等級";
            ws.Cell(1, 4).Value = "案件等級";
            ws.Cell(1, 5).Value = "通知內容";
            ws.Cell(1, 6).Value = "生效時間";
            ws.Cell(1, 7).Value = "負責人";
            ws.Cell(1, 8).Value = "建立者";

            // 設定顏色
            ws.Range(1, 1, 1, 8).Style.Fill.BackgroundColor = XLColor.FromArgb(154, 153, 153);

            var row = 2;
            caseReminds.ForEach(caseRemind =>
            {
                // 案件編號
                ws.Cell(row, 1).Value = "'" + caseRemind.CaseID;

                //  序號
                ws.Cell(row, 2).Value = caseRemind.AssignmentID?.ToString() ?? "";

                // 通知等級
                ws.Cell(row, 3).Value = caseRemind.Type.GetDescription();

                // 案件等級
                ws.Cell(row, 4).Value = caseRemind.Case.CaseWarning?.Name ?? "";

                // 通知內容
                ws.Cell(row, 5).Value = caseRemind.Content;

                // 生效時間
                ws.Cell(row, 6).Value = StringUtility.ToDateRangePicker(caseRemind.ActiveStartDateTime, caseRemind.ActiveEndDateTime);

                // 負責人
                ws.Cell(row, 7).Value = caseRemind.Case.ApplyUserName;

                // 建立者
                ws.Cell(row, 8).Value = caseRemind.CreateUserName;


                row++;
            });


            //左右至中
            ws.Range(1, 1, row - 1, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range(1, 6, row - 1, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            //水平 至中
            ws.Range(1, 1, row - 1, 4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range(1, 6, row - 1, 8).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


            //格線
            var rngTable1 = ws.Range(1, 1, row - 1, 8);
            rngTable1.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            rngTable1.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            //自動對應欄位寬度
            ws.Rows().AdjustToContents();
            ws.Columns().AdjustToContents();

            // 內容靠左與水平置中
            ws.Cell(1, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Column(5).Width = 100;
            ws.Range(2, 5, row - 1, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Range(2, 5, row - 1, 5).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            //20200911 固定列高
            for (int i = 2; i < row; i++)
            {
                ws.Row(i).Height = 95;
            }

            return ReportUtility.ConvertBookToByte(book, "");
        }
    }
}
