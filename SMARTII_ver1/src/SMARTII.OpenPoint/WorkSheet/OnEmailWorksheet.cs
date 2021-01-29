
using ClosedXML.Excel;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.COMMON_BU;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Master;
using SMARTII.Domain.Report;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SMARTII.OpenPoint
{
    public class OnEmailWorksheet : IWorksheet
    {
        public void CreateWorksheet(XLWorkbook book, object payload, object extend = null)
        {
            var data = (List<OpenPointCallHistory>)payload;
            int classificationCount = 0;
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

            //來信紀錄
            var ws = book.AddWorksheet(extend.ToString());

            #region 開頭
            ws.Cell(1, 1).Value = "案件編號";
            ws.Cell(1, 2).Value = "日期";
            ws.Cell(1, 3).Value = "時間";
            ws.Cell(1, 4).Value = "反應門市";
            ws.Cell(1, 5).Value = "姓名";
            ws.Cell(1, 6).Value = "E-Mail";
            ws.Cell(1, 7).Value = "問題";
            ws.Cell(1, 8).Value = "回覆";
            ws.Cell(1, 9).Value = "處理人員";
            ws.Cell(1, 10).Value = "回覆時間";
            ws.Cell(1, 11).Value = "轉派單位人員";
            ws.Cell(1, 12).Value = "回覆處理內容";

            for (int i = 1; i <= classificationCount; i++)
            {
                ws.Cell(1, 12 + i).Value = "問題分類" + i.ToString();
            }
            if (classificationCount == 0)
            {
                ws.Cell(1, 13).Value = "問題分類1";
                classificationCount = 1;
            }
            int column = 12 + classificationCount;

            ws.Cell(1, column + 1).Value = "反應日期";
            ws.Range(1, 1, 1, column + 1).Style.Fill.BackgroundColor = XLColor.FromArgb(192, 192, 192);

            #endregion
            int row = 2;
            foreach (var item in data)
            {
                ws.Cell(row, 1).Value = "'" + item.CaseID;
                ws.Cell(row, 2).Value = "'" + item.Date;
                ws.Cell(row, 3).Value = "'" + item.Time;
                ws.Cell(row, 4).Value = "'" + item.ComplainedNodeName;
                ws.Cell(row, 5).Value = "'" + item.ConcatUserName;
                ws.Cell(row, 6).Value = "'" + item.EMail;
                ws.Cell(row, 7).Value = "'" + item.CaseContent;
                ws.Cell(row, 7).Style.Alignment.WrapText = true;
                ws.Cell(row, 8).Value = "'" + item.FinishContent;
                ws.Cell(row, 8).Style.Alignment.WrapText = true;
                ws.Cell(row, 9).Value = "'" + item.ApplyUserName;
                ws.Cell(row, 10).Value = "'" + item.FinishReplyDateTime;
                ws.Cell(row, 11).Value = "'" + item.AssignmentUserName;
                ws.Cell(row, 11).Style.Alignment.WrapText = true;
                ws.Cell(row, 12).Value = "";
                //分類
                for (int i = 1; i <= classificationCount; i++)
                {
                    if (item.ClassList == null)
                        continue;
                    ws.Cell(row, 12 + i).Value = "'" + item.ClassList.ParentNamePathByArray[i - 1];
                    if (item.ClassList.ParentNamePathByArray.Count() < i + 1)
                    {
                        break;
                    }
                }
                ws.Cell(row, column + 1).Value = "'" + item.IncomingDateTime;
                row++;
            }
            int allColumn = column + 1;

            //左右至中
            ws.Range(1, 1, row - 1, allColumn).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            //問題、回覆至左
            ws.Range(2, 7, row - 1, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

            //水平 至中
            ws.Range(1, 1, row - 1, allColumn).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

            //字體大小
            ws.Range(1, 1, row - 1, allColumn).Style.Font.FontSize = 10;
            //格線
            var rngTable3 = ws.Range(1, 1, row - 1, allColumn);
            rngTable3.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            rngTable3.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            //自動對應欄位寬度
            ws.Columns().AdjustToContents();

            //設定案件內容與結案內容固定欄寬
            ws.Column(7).Width = 35;
            ws.Column(8).Width = 35;
            ws.Column(11).Width = 15;
            //列高
            ws.Row(1).Height = 25;
            for (int i = 2; i <= row; i++)
            {
                ws.Row(i).Height = 95;
            }
        }
    }
}
