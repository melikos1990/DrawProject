using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMARTII.Domain.Report;
using ClosedXML.Excel;
using System.IO;

namespace SMARTII.Service.Report.Provider
{
    public class QuestionClassificationReportProvider : IQuestionClassificationReportProvider
    {
        /// <summary>
        /// 問題分類-匯出
        /// </summary>
        /// <returns></returns>
        public Byte[] CreateQuestionClassificationExcel(List<QuestionClassificationForExcel> DataList)
        {
            //取得目前清單的最大層數
            int maxLevel = DataList.Select(x => x.Names.Length).Max();

            IDictionary<int, string> dist = new Dictionary<int, string>();

            dist.Add(1, "A");
            dist.Add(2, "B");
            dist.Add(3, "C");
            dist.Add(4, "D");
            dist.Add(5, "E");
            dist.Add(6, "F");
            dist.Add(7, "G");
            dist.Add(8, "H");
            dist.Add(9, "I");
            dist.Add(10, "J");

            XLWorkbook book = new XLWorkbook();


            var ws = book.AddWorksheet("問題分類");

            int row = 1;
            #region 標頭
            ws.Cell(row, 1).Value = "企業別";
            ws.Cell(row, 2).Value = "第一層";
            ws.Cell(row, 3).Value = "第二層";
            ws.Cell(row, 4).Value = "第三層";
            ws.Cell(row, 5).Value = "第四層";
            ws.Cell(row, 6).Value = "第五層";
            ws.Cell(row, 7).Value = "第六層";
            ws.Cell(row, 8).Value = "第七層";
            ws.Cell(row, 9).Value = "第八層";
            ws.Cell(row, 10).Value = "啟用狀態";
            #endregion
            row++;


            #region 內容
            foreach (var data in DataList)
            {

                int column = 2;

                ws.Cell(row, 1).Value = data.BuName;

                foreach (var name in data.Names)
                {
                    ws.Cell(row, column).Value = name;

                    column++;
                }

                ws.Cell(row, 10).Value = data.IsEnable;
                row++;
            }
            #endregion

            ws.Range(1, 1, 1, 10).Style.Font.Bold = true;
            //左右至中
            ws.Range(1, 1, row - 1, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            //水平 至中
            ws.Range(1, 1, row - 1, 10).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);



            //格線
            var rngTable1 = ws.Range(1, 1, row - 1, 10);
            rngTable1.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            rngTable1.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            //自動對應欄位寬度
            ws.Rows().AdjustToContents();
            ws.Columns().AdjustToContents();

            //刪除空白columns(因層數最大為8 故>=8不須刪除columns)
            if (maxLevel < 8)
                ws.Range($"{dist[maxLevel + 2]}:{dist[9]}").Delete(XLShiftDeletedCells.ShiftCellsLeft); 

            var result = ReportUtility.ConvertBookToByte(book, "");



            //CreateExcel(result);

            return result;
        }
        //private void CreateExcel(byte[] result)
        //{
        //    var url = @"C:\Users\jyan\Desktop\test1.xlsx";

        //    using (FileStream fs = File.Open(url, FileMode.Create, FileAccess.ReadWrite))
        //    {
        //        fs.Write(result, 0, result.Length);
        //    }
        //}
    }
}
