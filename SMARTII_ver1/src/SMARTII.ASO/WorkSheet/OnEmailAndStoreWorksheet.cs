﻿
using ClosedXML.Excel;
using SMARTII.COMMON_BU;
using SMARTII.Domain.Report;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SMARTII.ASO
{
    public class OnEmailAndStoreWorksheet : IWorksheet
    {
        public void CreateWorksheet(XLWorkbook book, object payload, object extend = null)
        {
            var data = (List<ASOCallHistory>)payload;
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
            ws.Cell(1, 1).Value = "單位";
            ws.Cell(1, 2).Value = "案件序號";
            ws.Cell(1, 3).Value = "立案日期";
            ws.Cell(1, 4).Value = "立案時間";
            for (int i = 1; i <= classificationCount; i++)
            {
                ws.Cell(1, 4 + i).Value = "問題分類" + i.ToString();
            }

            if (classificationCount == 0)
            {
                ws.Cell(1, 5).Value = "問題分類1";
                classificationCount = 1;
            }
            int column = 4 + classificationCount;

            ws.Cell(1, column + 1).Value = "反應門市";
            ws.Cell(1, column + 2).Value = "反應者資訊";
            ws.Cell(1, column + 3).Value = "聯繫電話";
            ws.Cell(1, column + 4).Value = "是否開立客訴";
            ws.Cell(1, column + 5).Value = "問題";
            ws.Cell(1, column + 6).Value = "回覆";
            ws.Cell(1, column + 7).Value = "處理人員";
            ws.Cell(1, column + 8).Value = "回覆日期";
            ws.Cell(1, column + 9).Value = "回覆時間";
            ws.Cell(1, column + 10).Value = "轉派單位人員";
            ws.Cell(1, column + 11).Value = "案件狀態";
            ws.Range(1, 1, 1, column + 11).Style.Fill.BackgroundColor = XLColor.FromArgb(192, 192, 192);


            #endregion
            int row = 2;
            foreach (var item in data)
            {
                ws.Cell(row, 1).Value = "'" + item.Unit;
                ws.Cell(row, 2).Value = "'" + item.CaseID;
                ws.Cell(row, 3).Value = "'" + item.Date;
                ws.Cell(row, 4).Value = "'" + item.Time;
                //分類
                for (int i = 1; i <= classificationCount; i++)
                {
                    if (item.ClassList == null)
                        continue;
                    ws.Cell(row, 4 + i).Value = "'" + item.ClassList.ParentNamePathByArray[i - 1];
                    if (item.ClassList.ParentNamePathByArray.Count() < i + 1)
                    {
                        break;
                    }
                }
                ws.Cell(row, column + 1).Value = "'" + item.ComplainedNodeName;
                ws.Cell(row, column + 2).Value = "'" + item.ConcatUserName;
                ws.Cell(row, column + 3).Value = "'" + item.ConcatMobile;
                ws.Cell(row, column + 4).Value = "'" + item.IsInvoice;
                ws.Cell(row, column + 5).Value = "'" + item.CaseContent;
                ws.Cell(row, column + 5).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 6).Value = "'" + item.FinishContent;
                ws.Cell(row, column + 6).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 7).Value = "'" + item.ApplyUserName;
                ws.Cell(row, column + 8).Value = "'" + item.ReplyDate;
                ws.Cell(row, column + 9).Value = "'" + item.ReplyTime;
                ws.Cell(row, column + 10).Value = "'" + item.AssignmentUserName;
                ws.Cell(row, column + 10).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 11).Value = "'" + item.CaseType;
                row++;
            }
            //左右至中
            ws.Range(1, 1, row - 1, column + 11).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            //問題、回覆至左
            ws.Range(2, column + 5, row - 1, column + 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

            //水平 至中
            ws.Range(1, 1, row - 1, column + 11).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

            //字體大小
            ws.Range(1, 1, row - 1, column + 11).Style.Font.FontSize = 10;
            //格線
            var rngTable3 = ws.Range(1, 1, row - 1, column + 11);
            rngTable3.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            rngTable3.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            //自動對應欄位寬度
            ws.Columns().AdjustToContents();

            //設定案件內容與結案內容固定欄寬
            ws.Column(column + 5).Width = 35;
            ws.Column(column + 6).Width = 35;
            ws.Column(column + 10).Width = 15;
            //列高
            ws.Row(1).Height = 25;
            for (int i = 2; i <= row; i++)
            {
                ws.Row(i).Height = 95;
            }
        }
    }
}
