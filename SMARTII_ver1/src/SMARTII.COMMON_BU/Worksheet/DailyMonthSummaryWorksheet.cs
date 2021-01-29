
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ClosedXML.Excel;
using SMARTII.Domain.Case;
using SMARTII.Domain.Report;

namespace SMARTII.COMMON_BU
{
    public class DailyMonthSummaryWorksheet : IWorksheet
    {
        /// <summary>
        /// 日/月彙總表
        /// </summary>
        /// <param name="book"></param>
        /// <param name="payload"></param>
        /// <param name="extend"></param>
        public void CreateWorksheet(XLWorkbook book, object payload, object extend = null)
        {
            var data = (SummaryInfoData)payload;

            var wsh = book.AddWorksheet($"本{extend}彙總表");

            #region 紀錄參數
            //IXLRange
            IXLRange rngTable1;
            //來源項目數量
            int sourceCount = data.SelectItems.Count();

            //大分類項目
            int Col = 0;
            int Row = sourceCount + 2;
            int totalRows = 0;
            //各大分類項目合計
            int itemIndex = 0;
            int[,] sourceTotalsByClassificition = new int[data.QuestionClassifications.Count(), sourceCount];

            //合計Col
            int[] sourceTotals = new int[sourceCount];


            //子項目Col
            int childCol = 1;
            #endregion

            #region 問題分類項目

            foreach (var item in data.QuestionClassifications)
            {
                Col += (sourceCount + 1);

                //大分類項目
                wsh.Range(Row, Col - sourceCount, Row, Col).Merge();
                wsh.Cell(Row, Col - sourceCount).Value = item.Name;
                GetCenter(ref wsh, Row, Col - sourceCount);

                //來源項目
                wsh.Range(Row + 1, Col - sourceCount, Row + 2, Col - sourceCount).Merge();
                wsh.Cell(Row + 1, Col - sourceCount).Value = "項目";
                GetCenter(ref wsh, Row + 1, Col - sourceCount);

                int colForSourceTitle = 1;
                foreach (var source in data.SelectItems)
                {
                    int col = colForSourceTitle + (Col - sourceCount);
                    wsh.Cell(Row + 1, col).Value = source.text;
                    GetCenter(ref wsh, Row + 1, col);
                    wsh.Cell(Row + 2, col).Value = $"本{extend}";
                    GetCenter(ref wsh, Row + 2, col);
                    colForSourceTitle++;
                }
                //找出第二階
                var level2Childs = item.Children.Where(c => c.Level == 2).ToList();

                #region 第二層開始迴圈
                //子項目ROW
                int childRow = sourceCount + 5;

                //此大分類底下案件數量
                var cases = data.Cases.FirstOrDefault(x => x.ClassifictionID == item.ID).cases;

                //第二層迴圈
                foreach (var chid in level2Childs)
                {
                    //擺放第二階
                    wsh.Cell(childRow, childCol).Value = chid.Name;
                    wsh.Cell(childRow, childCol).Style.Fill.BackgroundColor = XLColor.FromArgb(255, 255, 0);

                    //攤平2階之後(包含自己)
                    var fall = chid.Flatten();
                    //上層父ID
                    var parentId = chid.ID;

                    //擺放來源案件數量
                    var ids = fall.Select(c => c.ID).ToList();

                    int col = 1;
                    int childIndex = 0;
                    foreach (var source in data.SelectItems)
                    {
                        int cols = childCol + col;
                        int caseCount = cases.Where(x => ids.Contains(x.QuestionClassificationID) && x.CaseSource.CaseSourceType == (CaseSourceType)int.Parse(source.id)).Count();
                        wsh.Cell(childRow, cols).Value = $"{caseCount}";
                        GetCenter(ref wsh, childRow, cols);

                        col++;

                        //紀錄來源數量
                        sourceTotals[childIndex] += caseCount;
                        sourceTotalsByClassificition[itemIndex, childIndex] += caseCount;
                        childIndex++;
                    }

                    RecursionChild(fall, parentId, cases, data, childCol, ref childRow, ref wsh);

                    //第二階 結束後 加入空白
                    childRow += 2;

                    //計算最常值
                    totalRows = totalRows < childRow ? childRow : totalRows;
                }

                #endregion

                //結束後 跨來源加入下一個 Col 位置座標
                childCol += (sourceCount + 1);
                //記錄各大分類項目總計
                itemIndex++;
            }

            #endregion

            #region 合計數量計算
            //來源總合計
            int sourceRow = 1;
            foreach (var source in data.SelectItems)
            {
                wsh.Cell(sourceRow, 1).Value = $"{source.text}總合計";
                wsh.Cell(sourceRow, 2).Value = $"{sourceTotals[sourceRow - 1]}";
                GetCenter(ref wsh, sourceRow, 2);
                sourceRow++;
            }

            int sumCol = 1;
            int index = 0;
            foreach (var item in data.QuestionClassifications)
            {
                //本月小計
                wsh.Cell(totalRows + 1, sumCol).Value = $"本{extend}小計";
                GetCenter(ref wsh, totalRows + 1, sumCol);
                //合計
                wsh.Cell(totalRows + 2, sumCol).Value = "合計";
                GetCenter(ref wsh, totalRows + 2, sumCol);

                int total = 0;
                int sourceIndex = 0;
                int sourceCol = 1;
                foreach (var source in data.SelectItems)
                {
                    total += sourceTotalsByClassificition[index, sourceIndex];
                    //本日小計
                    wsh.Cell(totalRows + 1, (sumCol + sourceCol)).Value = $"{sourceTotalsByClassificition[index, sourceIndex]}";
                    GetCenter(ref wsh, totalRows + 1, (sumCol + sourceCol));
                    sourceCol++;
                    sourceIndex++;
                }
                //合計                                            //下一個Col座標 再加上來源數 -1 (因為是從1開始記數所以要扣掉)
                wsh.Range(totalRows + 2, sumCol + 1, totalRows + 2, (sumCol + sourceCol - 1)).Merge();
                wsh.Cell(totalRows + 2, sumCol + 1).Value = $"{total}";
                GetCenter(ref wsh, totalRows + 2, sumCol + 1);

                //結束後 跨來源加入下一個 Col 位置座標
                sumCol += (sourceCount + 1);
                index++;
            }

            #endregion

            //格線 固定底下兩Row , 每個大分類佔 大分類數成上來源+1(第一Col)
            rngTable1 = wsh.Range(sourceCount + 2, 1, totalRows + 2, ((data.QuestionClassifications.Count()) * (sourceCount + 1)));
            rngTable1.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            rngTable1.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            //底下總結灰色
            rngTable1 = wsh.Range(totalRows + 1, 1, totalRows + 2, ((data.QuestionClassifications.Count()) * (sourceCount + 1)));
            rngTable1.Style.Fill.BackgroundColor = XLColor.FromArgb(192, 192, 192);
            //上半部資訊灰色
            rngTable1 = wsh.Range(Row, 1, Row + 2, ((data.QuestionClassifications.Count()) * (sourceCount + 1)));
            rngTable1.Style.Fill.BackgroundColor = XLColor.FromArgb(192, 192, 192);
            //自動長寬
            wsh.Columns().AdjustToContents();
            wsh.Rows().AdjustToContents();

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
                                           ref int childRow,
                                           ref IXLWorksheet wsh,
                                           int level = 3)
        {
            var fallList = fall.Where(c => c.Level == level && c.ParentID == parentId).ToList();

            foreach (var item in fallList)
            {
                childRow++;

                //擺放階層內容
                wsh.Cell(childRow, childCol).Value = item.Name;
                wsh.Cell(childRow, childCol).Style.Fill.BackgroundColor = GetLevelColor(item.Level);

                //此階層底下層數(包含自己)
                var childFalls = item.Flatten();

                //擺放來源案件數量
                var ids = childFalls.Select(c => c.ID).ToList();
                int col = 1;
                foreach (var source in data.SelectItems)
                {
                    int cols = childCol + col;
                    var caseCount = cases.Where(x => ids.Contains(x.QuestionClassificationID) && x.CaseSource.CaseSourceType == (CaseSourceType)int.Parse(source.id)).Count();
                    wsh.Cell(childRow, cols).Value = $"{caseCount}";
                    GetCenter(ref wsh, childRow, cols);
                    col++;
                }

                //遞迴底下階層
                if (childFalls.Any(c => c.Level == (level + 1)))
                {
                    level++;

                    parentId = item.ID;

                    RecursionChild(fall, parentId, cases, data, childCol, ref childRow, ref wsh, level);
                }
                //最底層 則顯示白色
                else
                { 
                    wsh.Cell(childRow, childCol).Style.Fill.BackgroundColor = GetLevelColor(0);
                }
            }
        }

        //欄位致中
        private static void GetCenter(ref IXLWorksheet wsh, int Row, int Col) => wsh.Cell(Row, Col).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

        //背景色票
        private static XLColor GetLevelColor(int level) {

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
