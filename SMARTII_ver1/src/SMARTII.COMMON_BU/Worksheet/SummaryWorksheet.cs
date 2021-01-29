
using ClosedXML.Excel;
using SMARTII.Domain.Case;
using SMARTII.Domain.Report;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SMARTII.COMMON_BU
{
    public class SummaryWorksheet : IWorksheet
    {
        public void CreateWorksheet(XLWorkbook book, object payload, object extend = null)
        {
            var data = (SummaryInfoData)payload;

            var ws = book.AddWorksheet("彙總資訊");

            //IXLRange
            IXLRange rngTable1;

            //來源項目數量
            int sourceCount = data.SelectItems.Count();

            //大分類項目
            int Col = 0;
            int Row = sourceCount + 2;

            #region 開頭
            //累計
            ws.Range(1, 2, 3, sourceCount + 2).Merge();
            ws.Cell(1, 2).Value = "累計";
            ws.Cell(1, 2).Style.Fill.BackgroundColor = XLColor.FromArgb(255, 204, 153);
            //單日
            ws.Range(1, sourceCount + 3, 1, (sourceCount + 1) * 2 + 1).Merge();
            ws.Cell(1, sourceCount + 3).Value = "單日";
            ws.Cell(1, sourceCount + 3).Style.Fill.BackgroundColor = XLColor.FromArgb(153, 204, 0);
            //年
            ws.Cell(2, sourceCount + 3).Value = "年";
            //月
            ws.Cell(2, sourceCount + 4).Value = "月";
            //日
            ws.Cell(2, sourceCount + 5).Value = "日";

            ws.Range(2, sourceCount + 3, 2, (sourceCount + 1) * 2 + 1).Style.Fill.BackgroundColor = XLColor.FromArgb(192, 192, 192);
            DateTime lastDateTime = (DateTime)extend;
            //年
            ws.Cell(3, sourceCount + 3).Value = lastDateTime.ToString("yyyy");
            //月
            ws.Cell(3, sourceCount + 4).Value = lastDateTime.ToString("MM");
            //日
            ws.Cell(3, sourceCount + 5).Value = lastDateTime.ToString("dd");

            ws.Cell(4, 1).Value = "客訴類型";
            ws.Cell(4, 1).Style.Fill.BackgroundColor = XLColor.FromArgb(153, 204, 0);
            //總計數
            Col += (sourceCount + 1);
            int colForSourceTitle = 1;
            foreach (var source in data.SelectItems)
            {
                int col = colForSourceTitle + (Col - sourceCount);
                ws.Cell(4, col).Value = source.text + "總計數";
                ws.Cell(4, col).Style.Fill.BackgroundColor = XLColor.FromArgb(255, 153, 204);
                colForSourceTitle++;
            }
            ws.Cell(4, sourceCount + 2).Value = "合計總計數";
            ws.Cell(4, sourceCount + 2).Style.Fill.BackgroundColor = XLColor.FromArgb(255, 255, 153);
            //單日計數
            colForSourceTitle = 1;
            foreach (var source in data.SelectItems)
            {
                int col = colForSourceTitle + sourceCount + 2;
                ws.Cell(4, col).Value = "單日" + source.text + "計數";
                colForSourceTitle++;
            }
            ws.Cell(4, (sourceCount + 1) * 2 + 1).Value = "單日合計總計數";

            #endregion

            //子項目Col
            int childCol = 1;
            //子項目ROW
            int childRow = 5;


            #region 問題分類項目

            foreach (var item in data.QuestionClassifications)
            {

                //找出第一階
                var level2Childs = item.Children.Where(c => c.Level == 2).ToList();

                #region 第二層開始迴圈


                //此大分類底下案件數量
                var cases = data.Cases.FirstOrDefault(x => x.ClassifictionID == item.ID).cases;

                //第二層迴圈
                foreach (var chid in level2Childs)
                {
                    //擺放第一、二階
                    ws.Cell(childRow, childCol).Value = chid.Parent.Name + "_" + chid.Name;
                    ws.Cell(childRow, childCol).Style.Fill.BackgroundColor = XLColor.FromArgb(255, 255, 0);

                    //攤平2階之後(包含自己)
                    var fall = chid.Flatten();
                    //上層父ID
                    var parentId = chid.ID;

                    //擺放來源案件數量
                    var ids = fall.Select(c => c.ID).ToList();

                    int col = 1;
                    int Allcount = 0;
                    //來電、來信、其他總計數
                    foreach (var source in data.SelectItems)
                    {
                        int cols = childCol + col;
                        int caseCount = cases.Where(x => ids.Contains(x.QuestionClassificationID) && x.CaseSource.CaseSourceType == (CaseSourceType)int.Parse(source.id)).Count();
                        ws.Cell(childRow, cols).Value = $"{caseCount}";
                        GetCenter(ref ws, childRow, cols);
                        Allcount += caseCount;
                        col++;
                    }
                    //合計總計數
                    ws.Cell(childRow, childCol + col).Value = $"{Allcount}";
                    //單日
                    int singlecol = childCol + col;
                    int AllSingleCount = 0;
                    DateTime yeslastDateTime = lastDateTime.AddDays(-1).AddSeconds(1);
                    foreach (var source in data.SelectItems)
                    {
                        int cols = childCol + singlecol;
                        int caseCount = cases.Where(x => ids.Contains(x.QuestionClassificationID) && x.CaseSource.CaseSourceType == (CaseSourceType)int.Parse(source.id)
                        && x.CreateDateTime > yeslastDateTime && x.CreateDateTime < lastDateTime).Count();
                        ws.Cell(childRow, cols).Value = $"{caseCount}";
                        GetCenter(ref ws, childRow, cols);
                        AllSingleCount += caseCount;
                        singlecol++;
                    }
                    //單日總計數
                    ws.Cell(childRow, childCol + singlecol).Value = $"{AllSingleCount}";


                    RecursionChild(fall, parentId, cases, data, childCol, lastDateTime, ref childRow, ref ws);

                    //第二階 結束後 加入空白
                    childRow += 1;

                }

                #endregion
            }

            #endregion

            //左右至中
            ws.Range("A1:I3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            //水平 至中
            ws.Range("A1:I3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

            //字體大小
            //ws.Range("A1:I" + row.ToString()).Style.Font.FontSize = 10;

            //格線
            var rngTable3 = ws.Range(1, 1, childRow - 1, (sourceCount + 1) * 2 + 1);
            rngTable3.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            rngTable3.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            //自動長寬
            ws.Columns().AdjustToContents();
            ws.Rows().AdjustToContents();

        }


        /// <summary>
        /// 遞迴(第三層開始)
        /// </summary>
        /// <param name="fall"></param>
        /// <param name="parentId"></param>
        /// <param name="cases"></param>
        /// <param name="data"></param>
        /// <param name="childCol"></param>
        /// <param name="childRow"></param>
        /// <param name="wsh"></param>
        /// <param name="level"></param>
        private static void RecursionChild(List<Domain.Master.QuestionClassification> fall,
                                           int parentId,
                                           List<Case> cases, SummaryInfoData data,
                                           int childCol,
                                           DateTime lastDateTime,
                                           ref int childRow,
                                           ref IXLWorksheet ws,
                                           int level = 3)
        {
            var fallList = fall.Where(c => c.Level == level && c.ParentID == parentId).ToList();

            foreach (var item in fallList)
            {
                childRow++;

                //擺放階層內容
                ws.Cell(childRow, childCol).Value = item.Name;
                ws.Cell(childRow, childCol).Style.Fill.BackgroundColor = GetLevelColor(item.Level);

                //此階層底下層數(包含自己)
                var childFalls = item.Flatten();

                int Allcount = 0;
                //擺放來源案件數量
                var ids = childFalls.Select(c => c.ID).ToList();
                int col = 1;
                foreach (var source in data.SelectItems)
                {
                    int cols = childCol + col;
                    var caseCount = cases.Where(x => ids.Contains(x.QuestionClassificationID) && x.CaseSource.CaseSourceType == (CaseSourceType)int.Parse(source.id)).Count();
                    ws.Cell(childRow, cols).Value = $"{caseCount}";
                    GetCenter(ref ws, childRow, cols);
                    Allcount += caseCount;
                    col++;
                }
                //合計總計數
                ws.Cell(childRow, childCol + col).Value = $"{Allcount}";
                //單日
                int singlecol = childCol + col;
                int AllSingleCount = 0;
                DateTime yeslastDateTime = lastDateTime.AddDays(-1).AddSeconds(1);
                foreach (var source in data.SelectItems)
                {
                    int cols = childCol + singlecol;
                    int caseCount = cases.Where(x => ids.Contains(x.QuestionClassificationID) && x.CaseSource.CaseSourceType == (CaseSourceType)int.Parse(source.id)
                    && x.CreateDateTime > yeslastDateTime && x.CreateDateTime < lastDateTime).Count();
                    ws.Cell(childRow, cols).Value = $"{caseCount}";
                    GetCenter(ref ws, childRow, cols);
                    AllSingleCount += caseCount;
                    singlecol++;
                }
                //單日總計數
                ws.Cell(childRow, childCol + singlecol).Value = $"{AllSingleCount}";
                //最底層 則顯示白色
                ws.Cell(childRow, childCol).Style.Fill.BackgroundColor = GetLevelColor(0);
            }
        }
        //欄位致中
        private static void GetCenter(ref IXLWorksheet ws, int Row, int Col) => ws.Cell(Row, Col).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

        //背景色票
        private static XLColor GetLevelColor(int level)
        {

            switch (level)
            {
                case 3:
                    return XLColor.FromArgb(142, 169, 219);
                case 4:
                    return XLColor.FromArgb(198, 224, 180);
                case 5:
                    return XLColor.FromArgb(244, 176, 132);
                case 6:
                    return XLColor.FromArgb(237, 186, 244);
                case 7:
                    return XLColor.FromArgb(108, 232, 238);
                case 8:
                    return XLColor.FromArgb(245, 81, 147);
                case 0:
                    return XLColor.FromArgb(255, 255, 255);
                default:
                    throw new Exception("取得顏色發生錯誤!!");
            }

        }
    }
}
