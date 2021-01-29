
using ClosedXML.Excel;
using SMARTII.Domain.Data;
using SMARTII.Domain.Report;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SMARTII.MisterDonut
{
    public class OnComplaintWorksheet : IWorksheet
    {
        public void CreateWorksheet(XLWorkbook book, object payload, object extend = null)
        {
            var data = (List<MisterDonutComplaintHistory>)payload;
            //客訴紀錄
            var ws = book.AddWorksheet(extend.ToString());
            int classificationCount = 1;
            foreach (var item in data)
            {
                if (item.ClassList != null)
                {
                    if (item.ClassList.Level > classificationCount)
                    {
                        classificationCount = item.ClassList.Level;
                    }
                }
            }
            #region 開頭
            ws.Cell(1, 1).Value = "序號";
            ws.Cell(1, 2).Value = "來源";
            ws.Cell(1, 3).Value = "日期";
            ws.Cell(1, 4).Value = "時間";
            ws.Cell(1, 5).Value = "客訴門市";

            int column = 5;
            //第二層開始
            for (int i = 2; i <= classificationCount; i++)
            {
                ws.Cell(1, 5 + i - 1).Value = "問題分類" + i.ToString();
                column++;
            }

            if (classificationCount < 2)
            {
                ws.Cell(1, 6).Value = "問題分類2";
                column++;
            }

            ws.Cell(1, column + 1).Value = "姓名";
            ws.Cell(1, column + 2).Value = "聯繫電話";
            ws.Cell(1, column + 3).Value = "問題";
            ws.Cell(1, column + 4).Value = "回覆";
            ws.Cell(1, column + 5).Value = "處理人員";
            ws.Cell(1, column + 6).Value = "轉派單位人員";
            ws.Cell(1, column + 7).Value = "提供日期";

            ws.Range(1, 1, 1, column + 7).Style.Fill.BackgroundColor = XLColor.FromArgb(192, 192, 192);


            #endregion
            int row = 2;
            foreach (var item in data)
            {
                ws.Cell(row, 1).Value = "'" + item.InvoiceID;
                ws.Cell(row, 2).Value = "'" + item.Source;
                ws.Cell(row, 3).Value = "'" + item.Date;
                ws.Cell(row, 4).Value = "'" + item.Time;
                ws.Cell(row, 5).Value = "'" + item.ComplainedNodeName;
                //分類
                for (int i = 1; i < classificationCount; i++)
                {
                    if (item.ClassList == null || item.ClassList.ParentNamePathByArray.Count() < 2)
                        continue;
                    ws.Cell(row, 6 + i - 1).Value = "'" + item.ClassList.ParentNamePathByArray[i];
                    if (item.ClassList.ParentNamePathByArray.Count() < i + 2)
                    {
                        break;
                    }
                }
                ws.Cell(row, column + 1).Value = "'" + item.ConcatUserName;
                ws.Cell(row, column + 2).Value = "'" + item.ConcatMobile;
                ws.Cell(row, column + 3).Value = "'" + item.CaseContent;
                ws.Cell(row, column + 3).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 4).Value = "'" + item.FinishContent;
                ws.Cell(row, column + 4).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 5).Value = "'" + item.ApplyUserName;
                ws.Cell(row, column + 6).Value = "'" + item.AssignmentUserName;
                ws.Cell(row, column + 6).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 7).Value = "'" + item.InvoiceCreateDate;
                row++;
            }
            //左右至中
            ws.Range(1, 1, row - 1, column + 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            //問題、回覆至左
            ws.Range(2, column + 3, row - 1, column + 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

            //水平 至中
            ws.Range(1, 1, row - 1, column + 7).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

            //字體大小
            ws.Range(1, 1, row - 1, column + 7).Style.Font.FontSize = 10;
            //格線
            var rngTable3 = ws.Range(1, 1, row - 1, column + 7);
            rngTable3.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            rngTable3.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            //自動對應欄位寬度
            ws.Columns().AdjustToContents();

            //設定案件內容與結案內容固定欄寬
            ws.Column(column + 3).Width = 35;
            ws.Column(column + 4).Width = 35;
            ws.Column(column + 6).Width = 15;
            //列高
            ws.Row(1).Height = 25;
            for (int i = 2; i <= row; i++)
            {
                ws.Row(i).Height = 95;
            }
        }
    }
}
