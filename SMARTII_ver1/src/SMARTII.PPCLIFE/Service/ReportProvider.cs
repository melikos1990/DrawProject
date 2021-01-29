using System;
using System.Collections.Generic;
using System.Linq;
using ClosedXML.Excel;
using MultipartDataMediaFormatter.Infrastructure;
using SMARTII.COMMON_BU;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;
using SMARTII.Domain.Master;
using SMARTII.Domain.Report;
using SMARTII.PPCLIFE.Domain;
using SMARTII.PPCLIFE.Domain.DataList;
using SMARTII.Service.Report.Builder;
using SMARTII.Service.Report.Provider;
using static SMARTII.Domain.Cache.EssentialCache;

namespace SMARTII.PPCLIFE.Service
{
    public class ReportProvider : ReportProviderBase, IReportProvider
    {
        private readonly ExcelBuilder _builder;
        public ReportProvider(ICaseAggregate CaseAggregate, IMasterAggregate IMasterAggregate, ExcelBuilder builder) : base(CaseAggregate, IMasterAggregate)
        {
            _builder = builder;
        }

        public HttpFile ComplaintedReport(BaseComplaintReport data)
        {
            _PPCLifeReport report = (_PPCLifeReport)data;
            var wb = new XLWorkbook();

            IXLWorksheet ws = wb.Worksheets.Add("C02R104_Ppclife");

            #region 內文

            //Donut圖片
            ws.Range(1, 1, 1, 3).Row(1).Merge();

            //主旨
            ws.Range(1, 4, 1, 15).Row(1).Merge();
            ws.Cell(1, 4).Value = "重大客訴案件處理單";

            ws.Range(3, 1, 3, 16).Row(1).Merge();
            //
            ws.Cell(4, 1).Value = "開單日";
            ws.Range(4, 2, 4, 6).Row(1).Merge();
            ws.Cell(4, 2).Value = report.CreateDate;

            ws.Cell(4, 7).Value = "回饋日";
            ws.Range(4, 8, 4, 11).Row(1).Merge();
            ws.Cell(4, 8).Value = "";
            ws.Range(4, 12, 4, 13).Row(1).Merge();
            ws.Cell(4, 12).Value = "編號";
            ws.Range(4, 14, 4, 16).Row(1).Merge();
            ws.Cell(4, 14).Value = report.InvoicID;

            ws.Cell(5, 1).Value = "受理單位";
            ws.Range(5, 2, 5, 16).Row(1).Merge();

            if (report.Unit == null)
                ws.Cell(5, 2).Value = string.Format("□藥妝部　□美容部");
            else
                ws.Cell(5, 2).Value = string.Format("{0}藥妝部　{1}美容部", report.Unit == "A" ? "■" : "□", report.Unit == "B" ? "■" : "□");


            ws.Range(6, 1, 6, 10).Row(1).Merge();
            ws.Cell(6, 1).Value = "品牌名稱";
            ws.Range(6, 11, 6, 16).Row(1).Merge();
            ws.Cell(6, 11).Value = "結案時間(合計幾日)";

            ws.Range(7, 1, 7, 10).Row(1).Merge();
            ws.Cell(7, 1).Value = report.BrandName;
            ws.Range(7, 11, 7, 16).Row(1).Merge();
            ws.Cell(7, 11).Value = "";

            ws.Range(9, 1, 9, 16).Row(1).Merge();
            ws.Cell(9, 1).Value = "案件分類：　■速件";

            string respones = "";
            report?.ResponesList.ForEach(x =>
            {
                respones += string.Format("{0}{1}　", x.Item2 ? "■" : "□", x.Item1);
            });

            ws.Range(10, 1, 10, 16).Row(1).Merge();
            ws.Cell(10, 1).Value = string.Format("反應內容：　{0}", respones);

            ws.Range(11, 1, 11, 2).Row(1).Merge();
            ws.Cell(11, 1).Value = "反應者";
            ws.Range(11, 3, 11, 6).Row(1).Merge();
            ws.Cell(11, 3).Value = "";
            ws.Range(11, 7, 11, 8).Row(1).Merge();
            ws.Cell(11, 7).Value = "購買日期";
            ws.Range(11, 9, 11, 12).Row(1).Merge();
            ws.Cell(11, 9).Value = "";
            ws.Range(11, 13, 11, 14).Row(1).Merge();
            ws.Cell(11, 13).Value = "時間";
            ws.Range(11, 15, 11, 16).Row(1).Merge();
            ws.Cell(11, 15).Value = "";

            ws.Range(12, 1, 12, 2).Row(1).Merge();
            ws.Cell(12, 1).Value = "顧客姓名";
            ws.Range(12, 3, 12, 6).Row(1).Merge();
            ws.Cell(12, 3).Value = report.UserName + report.Gender;
            ws.Range(12, 7, 12, 8).Row(1).Merge();
            ws.Cell(12, 7).Value = "連絡電話";
            ws.Range(12, 9, 12, 12).Row(1).Merge();
            ws.Cell(12, 9).Value = report.Telephone;
            ws.Range(12, 13, 12, 14).Row(1).Merge();
            ws.Cell(12, 13).Value = "E-MAIL";
            ws.Range(12, 15, 12, 16).Row(1).Merge();
            ws.Cell(12, 15).Value = report.Email;

            ws.Cell(13, 1).Value = "反應內容";
            ws.Range(13, 3, 13, 16).Row(1).Merge();
            string content = report.Content.Replace("\n", "\r\n");
            ws.Cell(13, 3).Value = content;

            ws.Range(14, 3, 14, 16).Row(1).Merge();
            if (report.IsRecall == null)
            {
                ws.Cell(14, 3).Value = "是否需主管回電：　□是　□否";
            }
            else
            {
                ws.Cell(14, 3).Value = string.Format("是否需主管回電：　{0}是　{1}否", report.IsRecall == true ? "■" : "□", report.IsRecall == true ? "□" : "■");
            }

            ws.Range(16, 1, 16, 2).Row(1).Merge();
            ws.Cell(16, 1).Value = "處理日期";
            ws.Range(16, 3, 16, 5).Row(1).Merge();
            ws.Cell(16, 3).Value = "";

            ws.Range(16, 6, 16, 7).Row(1).Merge();
            ws.Cell(16, 6).Value = "處理時間";
            ws.Range(16, 8, 16, 9).Row(1).Merge();
            ws.Cell(16, 8).Value = "";
            ws.Range(16, 10, 16, 16).Row(1).Merge();
            ws.Cell(16, 10).Value = "處理經過(詳述溝通重點)：由客服/轉單處理人填寫";

            ws.Range(17, 1, 17, 16).Row(1).Merge();

            ws.Range(18, 1, 18, 4).Row(1).Merge();
            ws.Cell(18, 1).Value = "客訴追蹤結果：";
            ws.Range(18, 5, 18, 7).Row(1).Merge();
            ws.Cell(18, 5).Value = "月　日　時　分";
            ws.Range(18, 8, 18, 16).Row(1).Merge();
            ws.Cell(18, 8).Value = "□非常滿意　□滿意　□普通　□不滿意　□非常不滿意";

            ws.Range(19, 1, 19, 16).Row(1).Merge();
            ws.Cell(19, 1).Value = "";

            ws.Range(20, 1, 20, 16).Row(1).Merge();
            ws.Cell(20, 1).Value = "總部備註欄位";

            ws.Range(21, 1, 21, 16).Row(1).Merge();
            ws.Cell(21, 1).Value = "客訴案件以3工作日內結案為原則，若涉及身體傷害或法律相關問題，不受工作日限制。";

            ws.Range(22, 1, 22, 16).Row(1).Merge();
            ws.Range(23, 1, 23, 16).Row(1).Merge();

            ws.Range(25, 1, 25, 2).Row(1).Merge();
            ws.Cell(25, 1).Value = "總經理";
            ws.Range(25, 3, 25, 5).Row(1).Merge();
            ws.Cell(25, 3).Value = "副總經理";
            ws.Range(25, 6, 25, 7).Row(1).Merge();
            ws.Cell(25, 6).Value = "事業部主管";
            ws.Range(25, 8, 25, 9).Row(1).Merge();
            ws.Cell(25, 8).Value = "管理部部長";
            ws.Range(25, 10, 25, 12).Row(1).Merge();
            ws.Cell(25, 10).Value = "風管TEAM經理";
            ws.Range(25, 13, 25, 14).Row(1).Merge();
            ws.Cell(25, 13).Value = "客服人員";

            ws.Range(26, 1, 26, 2).Row(1).Merge();
            ws.Cell(26, 1).Value = "";
            ws.Range(26, 3, 26, 5).Row(1).Merge();
            ws.Cell(26, 3).Value = "";
            ws.Range(26, 6, 26, 7).Row(1).Merge();
            ws.Cell(26, 6).Value = "";
            ws.Range(26, 8, 26, 9).Row(1).Merge();
            ws.Cell(26, 8).Value = "";
            ws.Range(26, 10, 26, 12).Row(1).Merge();
            ws.Cell(26, 10).Value = "";
            ws.Range(26, 13, 26, 14).Row(1).Merge();
            ws.Cell(26, 13).Value = "";

            //高度設定
            ws.Rows(1, 29).Height = 21;
            ws.Row(1).Height = 54;
            ws.Row(2).Height = 10.5;
            ws.Row(3).Height = 13.5;
            ws.Row(6).Height = 13.5;
            ws.Row(8).Height = 6.75;
            ws.Row(10).Height = 42;

            ws.Row(13).Height = 84.75;
            ws.Row(14).Height = 21.75;
            ws.Row(15).Height = 7.5;
            ws.Row(17).Height = 159.75;
            ws.Row(19).Height = 60;
            ws.Row(21).Height = 13.5;
            ws.Row(22).Height = 13.5;
            ws.Row(23).Height = 13.5;
            ws.Row(24).Height = 7.5;
            ws.Row(25).Height = 13.5;
            ws.Row(26).Height = 42.75;
            //寬度設定
            ws.Column(1).Width = 8;
            ws.Columns(2, 16).Width = 3.57;
            ws.Column(4).Width = 1;
            ws.Column(5).Width = 6.29;
            ws.Column(7).Width = 8;
            ws.Column(9).Width = 8;
            ws.Column(14).Width = 8;
            ws.Column(16).Width = 16.71;
            //ws.Rows().AdjustToContents();
            //ws.Columns().AdjustToContents();

            //框線
            var rngTable1 = ws.Range("A3:P3");
            rngTable1.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            var rngTable2 = ws.Range("A4:P7");
            rngTable2.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            rngTable2.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            var rngTable3 = ws.Range("A9:P14");
            rngTable3.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            rngTable3.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            var rngTable4 = ws.Range("A16:P23");
            rngTable4.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            rngTable4.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            //虛線
            var rngTable7 = ws.Range("A21:P23");
            rngTable7.Style.Border.InsideBorder = XLBorderStyleValues.Dashed;

            var rngTable5 = ws.Range("A25:N26");
            rngTable5.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            rngTable5.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            //水平 至中、向左對齊、向右對齊
            ws.Range(1, 4, 1, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            ws.Row(1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range(4, 1, 4, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range(4, 2, 4, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            ws.Range(4, 7, 4, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range(4, 12, 4, 12).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Row(6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Row(7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            ws.Range(11, 1, 11, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range(11, 7, 11, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range(11, 13, 11, 13).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            ws.Range(12, 1, 12, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range(12, 7, 12, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range(12, 13, 12, 13).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            ws.Range(13, 1, 13, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range(13, 3, 13, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

            ws.Range(16, 1, 16, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range(16, 6, 16, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range(16, 10, 16, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            ws.Row(18).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range(18, 5, 18, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

            ws.Row(20).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Row(25).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            //垂直 至中、向上對齊
            ws.Columns(1, 16).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Rows(1, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Bottom);
            ws.Range(10, 1, 10, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range(13, 1, 13, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range(13, 3, 13, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            ws.Range(14, 3, 14, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

            ws.Rows(17, 17).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            ws.Rows(19, 19).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

            //字體大小
            ws.Cell(1, 4).Style.Font.FontSize = 20;

            ws.Range(4, 1, 7, 16).Style.Font.FontSize = 10;

            ws.Row(9).Style.Font.FontSize = 13;
            ws.Row(10).Style.Font.FontSize = 13;

            ws.Row(11).Style.Font.FontSize = 12;
            ws.Cell(11, 3).Style.Font.FontSize = 10;
            ws.Cell(11, 9).Style.Font.FontSize = 10;
            ws.Cell(11, 15).Style.Font.FontSize = 10;

            ws.Row(12).Style.Font.FontSize = 12;
            ws.Cell(12, 3).Style.Font.FontSize = 10;
            ws.Cell(12, 9).Style.Font.FontSize = 10;
            ws.Cell(12, 15).Style.Font.FontSize = 10;

            ws.Cell(13, 1).Style.Font.FontSize = 12;
            ws.Cell(13, 3).Style.Font.FontSize = 10;
            ws.Cell(14, 3).Style.Font.FontSize = 11;

            ws.Row(16).Style.Font.FontSize = 10;
            ws.Cell(16, 1).Style.Font.FontSize = 11;
            ws.Cell(16, 6).Style.Font.FontSize = 11;

            ws.Row(17).Style.Font.FontSize = 10;
            ws.Row(18).Style.Font.FontSize = 11;
            ws.Row(19).Style.Font.FontSize = 10;
            ws.Row(20).Style.Font.FontSize = 11;
            ws.Rows(21, 26).Style.Font.FontSize = 10;

            //字型
            ws.Rows(1, 29).Style.Font.FontName = "新細明體";
            ws.Cell(1, 4).Style.Font.FontName = "標楷體";
            ws.Cell(4, 1).Style.Font.FontName = "標楷體";
            ws.Cell(4, 7).Style.Font.FontName = "標楷體";
            ws.Cell(4, 12).Style.Font.FontName = "標楷體";
            ws.Row(5).Style.Font.FontName = "標楷體";
            ws.Row(6).Style.Font.FontName = "標楷體";
            ws.Row(7).Style.Font.FontName = "標楷體";
            ws.Row(9).Style.Font.FontName = "標楷體";
            ws.Row(10).Style.Font.FontName = "標楷體";
            ws.Cell(11, 1).Style.Font.FontName = "標楷體";
            ws.Cell(11, 3).Style.Font.FontName = "標楷體";
            ws.Cell(11, 7).Style.Font.FontName = "標楷體";
            ws.Cell(11, 13).Style.Font.FontName = "標楷體";
            ws.Cell(12, 1).Style.Font.FontName = "標楷體";
            ws.Cell(12, 7).Style.Font.FontName = "標楷體";
            ws.Cell(12, 13).Style.Font.FontName = "標楷體";
            ws.Cell(13, 1).Style.Font.FontName = "標楷體";
            ws.Cell(14, 3).Style.Font.FontName = "標楷體";
            ws.Cell(16, 1).Style.Font.FontName = "標楷體";
            ws.Cell(16, 6).Style.Font.FontName = "標楷體";
            ws.Cell(16, 10).Style.Font.FontName = "標楷體";
            ws.Row(18).Style.Font.FontName = "標楷體";

            ws.Rows(20, 23).Style.Font.FontName = "標楷體";
            ws.Row(26).Style.Font.FontName = "標楷體";

            //字體色彩
            ws.Cell(4, 1).Style.Font.FontColor = XLColor.White;
            ws.Cell(4, 7).Style.Font.FontColor = XLColor.White;
            ws.Cell(4, 12).Style.Font.FontColor = XLColor.White;
            ws.Cell(6, 1).Style.Font.FontColor = XLColor.White;
            ws.Cell(6, 11).Style.Font.FontColor = XLColor.White;

            ws.Cell(11, 1).Style.Font.FontColor = XLColor.White;
            ws.Cell(11, 7).Style.Font.FontColor = XLColor.White;
            ws.Cell(11, 13).Style.Font.FontColor = XLColor.White;
            ws.Cell(12, 1).Style.Font.FontColor = XLColor.White;
            ws.Cell(12, 7).Style.Font.FontColor = XLColor.White;
            ws.Cell(12, 13).Style.Font.FontColor = XLColor.White;
            ws.Cell(13, 1).Style.Font.FontColor = XLColor.White;

            //欄位色彩
            ws.Cell(4, 1).Style.Fill.BackgroundColor = XLColor.Black;
            ws.Cell(4, 7).Style.Fill.BackgroundColor = XLColor.Black;
            ws.Cell(4, 12).Style.Fill.BackgroundColor = XLColor.Black;
            ws.Cell(6, 1).Style.Fill.BackgroundColor = XLColor.Black;
            ws.Cell(6, 11).Style.Fill.BackgroundColor = XLColor.Black;

            ws.Cell(11, 1).Style.Fill.BackgroundColor = XLColor.Black;
            ws.Cell(11, 7).Style.Fill.BackgroundColor = XLColor.Black;
            ws.Cell(11, 13).Style.Fill.BackgroundColor = XLColor.Black;
            ws.Cell(12, 1).Style.Fill.BackgroundColor = XLColor.Black;
            ws.Cell(12, 7).Style.Fill.BackgroundColor = XLColor.Black;
            ws.Cell(12, 13).Style.Fill.BackgroundColor = XLColor.Black;
            ws.Cell(13, 1).Style.Fill.BackgroundColor = XLColor.Black;
            ws.Cell(16, 1).Style.Fill.BackgroundColor = XLColor.FromArgb(220, 220, 220);
            ws.Cell(16, 6).Style.Fill.BackgroundColor = XLColor.FromArgb(220, 220, 220);
            ws.Cell(16, 10).Style.Fill.BackgroundColor = XLColor.FromArgb(220, 220, 220);
            ws.Cell(18, 1).Style.Fill.BackgroundColor = XLColor.FromArgb(220, 220, 220);
            ws.Cell(18, 5).Style.Fill.BackgroundColor = XLColor.Gray;
            ws.Cell(18, 8).Style.Fill.BackgroundColor = XLColor.Gray;
            ws.Cell(20, 1).Style.Fill.BackgroundColor = XLColor.Gray;

            //自動換行
            ws.Range(10, 1, 10, 16).Style.Alignment.WrapText = true;
            //粗體
            ws.Cell(1, 4).Style.Font.Bold = true;
            ws.Cell(30, 1).Style.Font.Bold = true;
            //合併兩Row
            ws.Range("A13:B14").Merge();

            #endregion 內文

            var result = ReportUtility.ConvertBookToByte(wb, data.Password);

            return new HttpFile(
              data.fileName.GetComplaintInvoiceName("xlsx", EssentialCache.BusinessKeyValue.PPCLIFE),
              "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
              result);
        }


        public BaseComplaintReport GeneratorPayload(string caseID, string invoiceID)
        {
            var @case = this.GetCaseWithComplaint(caseID, invoiceID, (con) =>
            {

                // 取得 案件連絡者 資料                
                con.IncludeBy(c => c.CASE_CONCAT_USER);
                // 取得 案件被反應者 資料              
                con.IncludeBy(c => c.CASE_COMPLAINED_USER);
                // 取得 案件來源資料
                con.IncludeBy(c => c.CASE_SOURCE);
                // 取得 反應單 資料
                con.IncludeBy(c => c.CASE_ASSIGNMENT_COMPLAINT_INVOICE);
            });

            //取得特定識別碼 問題分類下層分類
            QuestionClassification quset = this.GetQuestionClassificationByCode((con) =>
            {
                con.And(c => c.CODE == CodeValue.URGENT);
                con.IncludeBy(c => c.QUESTION_CLASSIFICATION1);
            });

            System.Func<CaseAssignmentComplaintInvoice, bool> predicate = x => x.InvoiceID == invoiceID;
            var invoice = @case.ComplaintInvoice.FirstOrDefault(predicate);
            var user = @case.CaseConcatUsers?.FirstOrDefault();


            // 組合資料
            var result = new _PPCLifeReport();

            //開單日
            result.CreateDate = invoice?.CreateDateTime;
            //編號
            result.InvoicID = invoice?.InvoiceID;
            //受理單位
            result.Unit = invoice?.InvoiceType;
            //品牌名稱
            result.BrandName = @case.CaseComplainedUsers.FirstOrDefault(x => x.CaseComplainedUserType == CaseComplainedUserType.Responsibility &&
                                                                        x.UnitType == SMARTII.Domain.Organization.UnitType.Organization)?.NodeName;
            //反應內容
            result.ResponesList = new List<Tuple<string, bool>>();
            if (quset != null)
            {
                foreach (var child in quset.Children)
                    result.ResponesList.Add(new Tuple<string, bool>(child.Name, @case.QuestionClassificationID == child.ID ? true : false));

            }

            //顧客姓名
            result.UserName = user?.UserName;
            result.Gender = user?.Gender.GetDescription();
            //聯絡電話
            result.Telephone = user?.Mobile + "/" + user?.Telephone;
            //E-MAIL
            result.Email = user?.Email;
            //反應內容
            result.Content = @case.Content;
            //是否回電
            result.IsRecall = invoice?.IsRecall;

            return result;
        }

        #region PPCLIFE 匯出Excel來電紀錄
        /// <summary>
        /// 統一藥品來電紀錄 前台下載
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public byte[] GetOnCallExcel(PPCLIFEDataList model, DateTime end)
        {
            _builder.Clear();
            var @byte = _builder
                .AddWorkSheet(new SummaryWorksheet(), model.SummaryInformation, end)
                .AddWorkSheet(new DailyMonthSummaryWorksheet(), model.DailySummary, "日")
                .AddWorkSheet(new DailyMonthSummaryWorksheet(), model.MonthSummary, "月")

                .AddWorkSheet(new OnCallAndOtherWorksheet(), model.OnCallHistory, "來電紀錄")
                .AddWorkSheet(new OnEmailWorksheet(), model.OnEmailHistory, "來信紀錄")

                .AddWorkSheet(new OnComplaintWorksheet(), model.OnGeneralComplaint, "客訴紀錄-一般客訴")
                .AddWorkSheet(new OnComplaintWorksheet(), model.OnGeneralComplaintNotFinished, "客訴紀錄-一般客訴(上個月未結案)")
                .AddWorkSheet(new OnComplaintWorksheet(), model.OnUrgentComplaint, "客訴紀錄-重大客訴")
                .AddWorkSheet(new OnComplaintWorksheet(), model.OnUrgentComplaintNotFinished, "客訴紀錄-重大客訴(上個月未結案)")
                .Build();
            return @byte;
        }
        /// <summary>
        /// 統一藥品來電紀錄 Batch
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public byte[] GetOnCallExcelToBatch(PPCLIFEDataList model, DateTime end)
        {
            _builder.Clear();
            var @byte = _builder
                .AddWorkSheet(new SummaryWorksheet(), model.SummaryInformation, end)
                .AddWorkSheet(new DailyMonthSummaryWorksheet(), model.DailySummary, "日")
                .AddWorkSheet(new DailyMonthSummaryWorksheet(), model.MonthSummary, "月")

                .AddWorkSheet(new OnCallAndOtherWorksheetBatch(), model.OnCallHistory, "來電紀錄")
                .AddWorkSheet(new OnEmailWorksheetBatch(), model.OnEmailHistory, "來信紀錄")

                .AddWorkSheet(new OnComplaintWorksheet(), model.OnGeneralComplaint, "客訴紀錄-一般客訴")
                .AddWorkSheet(new OnComplaintWorksheet(), model.OnGeneralComplaintNotFinished, "客訴紀錄-一般客訴(上個月未結案)")
                .AddWorkSheet(new OnComplaintWorksheet(), model.OnUrgentComplaint, "客訴紀錄-重大客訴")
                .AddWorkSheet(new OnComplaintWorksheet(), model.OnUrgentComplaintNotFinished, "客訴紀錄-重大客訴(上個月未結案)")
                .Build();
            return @byte;
        }


        /// <summary>
        /// 統一藥品 - 品牌商品與問題歸類
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public byte[] GetBrandCalcExcel(PPCLIFEBrandCalcDataList model, DateTime start)
        {
            var @byte = _builder
                .AddWorkSheet(new BrandSummaryWorksheet(), model, start)
                .AddWorkSheet(new DetailWorksheet(), model)
                .Build();
            return @byte;
        }


        #endregion
    }
}
