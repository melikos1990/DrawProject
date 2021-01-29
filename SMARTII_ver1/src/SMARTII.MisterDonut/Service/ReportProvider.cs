using ClosedXML.Excel;
using System.Linq;
using MultipartDataMediaFormatter.Infrastructure;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;
using SMARTII.Domain.Report;
using SMARTII.MisterDonut.Domain;
using SMARTII.Service.Report.Provider;
using SMARTII.Domain.Master;
using System;
using SMARTII.Service.Report.Builder;
using SMARTII.COMMON_BU;
using System.Collections.Generic;
using SMARTII.Domain.Cache;

namespace SMARTII.MisterDonut.Service
{
    public class ReportProvider : ReportProviderBase, IReportProvider
    {
        private readonly ExcelBuilder _builder;
        public ReportProvider(ICaseAggregate CaseAggregate,
            IMasterAggregate IMasterAggregate, ExcelBuilder builder) : base(CaseAggregate, IMasterAggregate)
        {
            _builder = builder;
        }

        public HttpFile ComplaintedReport(BaseComplaintReport data)
        {
            _MisterDonutReport report = (_MisterDonutReport)data;
            var wb = new XLWorkbook();

            #region 內文

            IXLWorksheet ws = wb.Worksheets.Add("C02R101");
            //Donut圖片

            ws.Range(1, 1, 1, 3).Row(1).Merge();
            ws.AddPicture(Properties.Resources.misterdonut)
                .MoveTo(1, 1)
                .Scale(0.5); // optional: resize picture

            //主旨
            ws.Range(1, 4, 1, 15).Row(1).Merge();
            ws.Cell(1, 4).Value = "顧客反應處理單";

            ws.Range(3, 1, 3, 17).Row(1).Merge();
            //
            ws.Cell(4, 1).Value = "開單日";
            ws.Range(4, 2, 4, 6).Row(1).Merge();
            ws.Cell(4, 2).Value = report.CreateDate;

            ws.Cell(4, 7).Value = "回饋日";
            ws.Range(4, 8, 4, 11).Row(1).Merge();
            ws.Cell(4, 8).Value = "";
            ws.Range(4, 12, 4, 13).Row(1).Merge();
            ws.Cell(4, 12).Value = "編號";
            ws.Range(4, 14, 4, 17).Row(1).Merge();
            ws.Cell(4, 14).Value = report.InvoicID;

            ws.Cell(5, 1).Value = "受理單位";
            ws.Range(5, 2, 5, 17).Row(1).Merge();
            if (report.Unit == null)
                ws.Cell(5, 2).Value = "□營運部　□行銷部　□公司相關單位　□二度處理單";
            else
                ws.Cell(5, 2).Value = string.Format("{0}營運部　{1}行銷部　□公司相關單位　□二度處理單", report.Unit == "A" ? "■" : "□", report.Unit == "B" ? "■" : "□");

            ws.Range(6, 1, 6, 4).Row(1).Merge();
            ws.Cell(6, 1).Value = "區組名稱";
            ws.Range(6, 5, 6, 10).Row(1).Merge();
            ws.Cell(6, 5).Value = "門市名稱";
            ws.Range(6, 11, 6, 14).Row(1).Merge();
            ws.Cell(6, 11).Value = "服務人員";
            ws.Range(6, 15, 6, 17).Row(1).Merge();
            ws.Cell(6, 15).Value = "區顧問";

            ws.Range(7, 1, 7, 4).Row(1).Merge();
            ws.Cell(7, 1).Value = report.AreaName;
            ws.Range(7, 5, 7, 10).Row(1).Merge();
            ws.Cell(7, 5).Value = report.StoreName;
            ws.Range(7, 11, 7, 14).Row(1).Merge();
            ws.Cell(7, 11).Value = "";
            ws.Range(7, 15, 7, 17).Row(1).Merge();
            ws.Cell(7, 15).Value = report.Supervisor;

            ws.Range(9, 1, 9, 17).Row(1).Merge();

            var asign = "";
            var asign2 = "";
            if (report.IsDispatch == null)
            {
                asign = "□";
                asign2 = "□";
            }
            else if (report.IsDispatch.Value == true)
            {
                asign = "■";
                asign2 = "□";
            }
            else
            {
                asign = "□";
                asign2 = "■";
            }

            ws.Cell(9, 1).Value = string.Format("案件分類：　{0}緊急件　{1}速件", asign, asign2);

            ws.Range(10, 1, 10, 17).Row(1).Merge();
            ws.Cell(10, 1).Value = $"反應內容：　{(report.ServiceAttitude ? '■' : '□')}服務態度　{(report.ServiceDefect ? '■' : '□')}服務瑕疵(物/事)　{(report.ProductAnomaly ? '■' : '□')}商品異常　{(report.Other ? '■' : '□')}其他疏失或建議";

            ws.Range(11, 1, 11, 2).Row(1).Merge();
            ws.Cell(11, 1).Value = "反應者";
            ws.Range(11, 3, 11, 9).Row(1).Merge();
            ws.Cell(11, 3).Value = "□內用顧客(E.I.)　　□外帶顧客(T.O.)";
            ws.Range(11, 10, 11, 12).Row(1).Merge();
            ws.Cell(11, 10).Value = "購買日期";
            ws.Range(11, 13, 11, 14).Row(1).Merge();
            ws.Cell(11, 13).Value = "";
            ws.Range(11, 15, 11, 16).Row(1).Merge();
            ws.Cell(11, 15).Value = "時間";
            ws.Cell(11, 17).Value = "";

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
            ws.Range(12, 15, 12, 17).Row(1).Merge();
            ws.Cell(12, 15).Value = report.Email;
            //ws.Range(13, 1, 14, 2).Row(1).Merge();

            ws.Cell(13, 1).Value = "反應內容";
            ws.Range(13, 3, 13, 17).Row(1).Merge();
            string content = report.Content.Replace("\n", "\r\n");
            ws.Cell(13, 3).Value = content;

            ws.Range(14, 3, 14, 17).Row(1).Merge();
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
            ws.Range(16, 10, 16, 17).Row(1).Merge();
            ws.Cell(16, 10).Value = "處理經過(詳述溝通重點)：由區顧問/承辦人填寫";

            ws.Range(17, 1, 17, 17).Row(1).Merge();

            ws.Range(18, 1, 18, 7).Row(1).Merge();
            ws.Cell(18, 1).Value = "門市實際改善作法：由店經理填寫";
            ws.Range(18, 8, 18, 17).Row(1).Merge();
            ws.Cell(18, 8).Value = "□處理結束　□持續處理中　□無法處理(無法聯繫顧客)";

            ws.Range(19, 1, 19, 17).Row(1).Merge();

            ws.Range(20, 1, 20, 17).Row(1).Merge();
            ws.Cell(20, 1).Value = "總部備註欄位";

            ws.Range(21, 1, 21, 17).Row(1).Merge();
            ws.Cell(21, 1).Value = "1.急/速件分類原則：人身安全相關屬急件；商品品質不良(含未擠餡)及服務瑕疵相關屬速件。";

            ws.Range(22, 1, 22, 17).Row(1).Merge();
            ws.Cell(22, 1).Value = "2.緊急件需在D日內完成；速件則在D+2日內完成；一般件D+3日內完成處理，並依循程序呈報上一層主管。";

            ws.Range(23, 1, 23, 17).Row(1).Merge();
            ws.Cell(23, 1).Value = "3.請務必依規定時限處理，客服中心將每月統計未按規定案件，呈報相關主管。";

            ws.Range(25, 1, 25, 2).Row(1).Merge();
            ws.Cell(25, 1).Value = "營支TEAM經理";
            ws.Range(25, 3, 25, 5).Row(1).Merge();
            ws.Cell(25, 3).Value = "營業TEAM經理";
            ws.Range(25, 6, 25, 7).Row(1).Merge();
            ws.Cell(25, 6).Value = "區顧問";
            ws.Range(25, 8, 25, 9).Row(1).Merge();
            ws.Cell(25, 8).Value = "店經理";
            ws.Range(25, 10, 25, 12).Row(1).Merge();
            ws.Cell(25, 10).Value = "客服中心";

            ws.Range(26, 1, 26, 2).Row(1).Merge();
            ws.Cell(26, 1).Value = "　　月　　日";
            ws.Range(26, 3, 26, 5).Row(1).Merge();
            ws.Cell(26, 3).Value = "　　月　　日";
            ws.Range(26, 6, 26, 7).Row(1).Merge();
            ws.Cell(26, 6).Value = "　　月　　日";
            ws.Range(26, 8, 26, 9).Row(1).Merge();
            ws.Cell(26, 8).Value = "　　月　　日";
            ws.Range(26, 10, 26, 12).Row(1).Merge();
            ws.Cell(26, 10).Value = "　　月　　日";

            ws.Range(28, 1, 28, 7).Row(1).Merge();
            ws.Cell(28, 1).Value = "□成立　□不成立";

            ws.Range(29, 1, 29, 17).Row(1).Merge();
            ws.Cell(29, 1).Value = "※客訴案件是否成立，將由營業支援TEAM判斷。";

            //高度設定
            ws.Rows(1, 29).Height = 21;
            ws.Row(1).Height = 54;
            ws.Row(2).Height = 10.5;
            ws.Row(3).Height = 13.5;
            ws.Row(6).Height = 13.5;
            ws.Row(8).Height = 6.75;
            ws.Row(11).Height = 14.25;
            ws.Row(13).Height = 105.75;
            ws.Row(14).Height = 21.75;
            ws.Row(15).Height = 7.5;
            ws.Row(17).Height = 60;
            ws.Row(19).Height = 159.75;
            ws.Row(20).Height = 18;
            ws.Row(21).Height = 13.5;
            ws.Row(22).Height = 13.5;
            ws.Row(23).Height = 13.5;
            ws.Row(24).Height = 7.5;
            ws.Row(25).Height = 13.5;
            ws.Row(26).Height = 42.75;
            ws.Row(27).Height = 6.75;
            ws.Row(28).Height = 13.5;
            //寬度設定
            ws.Column(1).Width = 8;
            ws.Columns(2, 16).Width = 3.57;
            ws.Column(7).Width = 8;
            ws.Column(9).Width = 8;
            ws.Column(14).Width = 8;
            ws.Column(17).Width = 12.43;
            //ws.Rows().AdjustToContents();
            //ws.Columns().AdjustToContents();

            //框線
            var rngTable1 = ws.Range("A3:Q3");
            rngTable1.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            var rngTable2 = ws.Range("A4:Q7");
            rngTable2.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            rngTable2.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            var rngTable3 = ws.Range("A9:Q14");
            rngTable3.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            rngTable3.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            var rngTable4 = ws.Range("A16:Q23");
            rngTable4.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            rngTable4.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            var rngTable7 = ws.Range("A21:Q23");
            rngTable7.Style.Border.InsideBorder = XLBorderStyleValues.Dashed;
            var rngTable5 = ws.Range("A25:L26");
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

            ws.Range(11, 1, 11, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range(11, 10, 11, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range(11, 15, 11, 15).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            ws.Range(12, 1, 12, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range(12, 7, 12, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range(12, 13, 12, 13).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            ws.Range(13, 1, 13, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range(16, 1, 16, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range(16, 6, 16, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range(16, 10, 16, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            ws.Row(18).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Row(20).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Row(25).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Row(26).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            ws.Row(28).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            //垂直 至中、向上對齊
            ws.Columns(1, 17).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Rows(1, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Bottom);
            ws.Range(13, 1, 13, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range(13, 3, 13, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            ws.Range(14, 3, 14, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

            ws.Rows(17, 17).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            ws.Rows(19, 19).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            ws.Rows(26, 26).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            ws.Rows(29, 29).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

            //字體大小
            ws.Cell(1, 4).Style.Font.FontSize = 20;
            ws.Range(13, 1, 13, 1).Style.Font.FontSize = 10;
            ws.Range(4, 1, 7, 17).Style.Font.FontSize = 10;

            ws.Row(9).Style.Font.FontSize = 13;
            ws.Row(10).Style.Font.FontSize = 13;

            ws.Row(11).Style.Font.FontSize = 12;
            ws.Cell(11, 3).Style.Font.FontSize = 10;
            ws.Cell(11, 13).Style.Font.FontSize = 10;
            ws.Cell(11, 17).Style.Font.FontSize = 10;

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
            ws.Rows(21, 28).Style.Font.FontSize = 10;
            ws.Row(29).Style.Font.FontSize = 14;

            //字型
            ws.Rows(1, 28).Style.Font.FontName = "新細明體";
            ws.Cell(1, 4).Style.Font.FontName = "標楷體";
            ws.Cell(4, 1).Style.Font.FontName = "標楷體";
            ws.Cell(4, 7).Style.Font.FontName = "標楷體";
            ws.Cell(4, 12).Style.Font.FontName = "標楷體";
            ws.Row(5).Style.Font.FontName = "標楷體";
            ws.Row(6).Style.Font.FontName = "標楷體";
            ws.Row(9).Style.Font.FontName = "標楷體";
            ws.Row(10).Style.Font.FontName = "標楷體";
            ws.Cell(11, 1).Style.Font.FontName = "標楷體";
            ws.Cell(11, 3).Style.Font.FontName = "標楷體";
            ws.Cell(11, 10).Style.Font.FontName = "標楷體";
            ws.Cell(11, 15).Style.Font.FontName = "標楷體";
            ws.Cell(12, 1).Style.Font.FontName = "標楷體";
            ws.Cell(12, 7).Style.Font.FontName = "標楷體";
            ws.Cell(12, 13).Style.Font.FontName = "標楷體";
            ws.Cell(13, 1).Style.Font.FontName = "標楷體";
            ws.Cell(14, 3).Style.Font.FontName = "標楷體";
            ws.Cell(16, 1).Style.Font.FontName = "標楷體";
            ws.Cell(16, 6).Style.Font.FontName = "標楷體";
            ws.Cell(16, 10).Style.Font.FontName = "標楷體";
            ws.Cell(18, 1).Style.Font.FontName = "標楷體";
            ws.Cell(18, 8).Style.Font.FontName = "標楷體";
            ws.Rows(20, 23).Style.Font.FontName = "標楷體";
            ws.Row(26).Style.Font.FontName = "標楷體";
            ws.Row(29).Style.Font.FontName = "標楷體";

            //字體色彩
            ws.Cell(4, 1).Style.Font.FontColor = XLColor.White;
            ws.Cell(4, 7).Style.Font.FontColor = XLColor.White;
            ws.Cell(4, 12).Style.Font.FontColor = XLColor.White;
            ws.Cell(6, 1).Style.Font.FontColor = XLColor.White;
            ws.Cell(6, 5).Style.Font.FontColor = XLColor.White;
            ws.Cell(6, 11).Style.Font.FontColor = XLColor.White;
            ws.Cell(6, 15).Style.Font.FontColor = XLColor.White;

            ws.Cell(11, 1).Style.Font.FontColor = XLColor.White;
            ws.Cell(11, 10).Style.Font.FontColor = XLColor.White;
            ws.Cell(11, 15).Style.Font.FontColor = XLColor.White;
            ws.Cell(12, 1).Style.Font.FontColor = XLColor.White;
            ws.Cell(12, 7).Style.Font.FontColor = XLColor.White;
            ws.Cell(12, 13).Style.Font.FontColor = XLColor.White;
            ws.Cell(13, 1).Style.Font.FontColor = XLColor.White;

            //欄位色彩
            ws.Cell(4, 1).Style.Fill.BackgroundColor = XLColor.Black;
            ws.Cell(4, 7).Style.Fill.BackgroundColor = XLColor.Black;
            ws.Cell(4, 12).Style.Fill.BackgroundColor = XLColor.Black;
            ws.Cell(6, 1).Style.Fill.BackgroundColor = XLColor.Black;
            ws.Cell(6, 5).Style.Fill.BackgroundColor = XLColor.Black;
            ws.Cell(6, 11).Style.Fill.BackgroundColor = XLColor.Black;
            ws.Cell(6, 15).Style.Fill.BackgroundColor = XLColor.Black;

            ws.Cell(11, 1).Style.Fill.BackgroundColor = XLColor.Black;
            ws.Cell(11, 10).Style.Fill.BackgroundColor = XLColor.Black;
            ws.Cell(11, 15).Style.Fill.BackgroundColor = XLColor.Black;
            ws.Cell(12, 1).Style.Fill.BackgroundColor = XLColor.Black;
            ws.Cell(12, 7).Style.Fill.BackgroundColor = XLColor.Black;
            ws.Cell(12, 13).Style.Fill.BackgroundColor = XLColor.Black;
            ws.Cell(13, 1).Style.Fill.BackgroundColor = XLColor.Black;
            ws.Cell(16, 1).Style.Fill.BackgroundColor = XLColor.FromArgb(200, 200, 200);
            ws.Cell(16, 6).Style.Fill.BackgroundColor = XLColor.FromArgb(200, 200, 200);
            ws.Cell(16, 10).Style.Fill.BackgroundColor = XLColor.FromArgb(200, 200, 200);
            ws.Cell(18, 1).Style.Fill.BackgroundColor = XLColor.FromArgb(200, 200, 200);
            ws.Cell(18, 8).Style.Fill.BackgroundColor = XLColor.Gray;
            ws.Cell(20, 1).Style.Fill.BackgroundColor = XLColor.Gray;
            ws.Cell(28, 1).Style.Fill.BackgroundColor = XLColor.FromArgb(200, 200, 200);

            //粗體
            ws.Cell(1, 4).Style.Font.Bold = true;
            ws.Cell(29, 1).Style.Font.Bold = true;

            //合併兩Row
            ws.Range("A13:B14").Merge();

            #endregion 內文

            var result = ReportUtility.ConvertBookToByte(wb, data.Password);

            return new HttpFile(
                    data.fileName.GetComplaintInvoiceName("xlsx", EssentialCache.BusinessKeyValue.MisterDonut),
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
                // 取得 案件緊急資料
                con.IncludeBy(c => c.CASE_WARNING);
                // 取得 反應單 資料
                con.IncludeBy(c => c.CASE_ASSIGNMENT_COMPLAINT_INVOICE);
            });

            //取得問題分類項目
            var quest = this.GetQuestionClassification(@case.QuestionClassificationID);


            System.Func<CaseAssignmentComplaintInvoice, bool> predicate = x => x.InvoiceID == invoiceID;
            var invoice = @case.ComplaintInvoice.FirstOrDefault(predicate);
            var user = @case.CaseConcatUsers?.FirstOrDefault();
            var store = @case.CaseComplainedUsers.
                         FirstOrDefault(s => s.CaseComplainedUserType == CaseComplainedUserType.Responsibility &&
                                        s.UnitType == SMARTII.Domain.Organization.UnitType.Store);

            // 組合資料
            var result = new _MisterDonutReport();

            //開單日
            result.CreateDate = invoice?.CreateDateTime;
            //編號
            result.InvoicID = invoice?.InvoiceID;
            //受理單位
            result.Unit = invoice?.InvoiceType;
            //區組名稱
            result.AreaName = store?.ParentPathName;
            //門市名稱            
            result.StoreName = store?.StoreNo + store?.NodeName;
            //區顧問
            result.Supervisor = store?.SupervisorUserName;

            //案件分類
            if (@case.CaseWarning.Name == "急件")
                result.IsDispatch = true;
            else if (@case.CaseWarning.Name == "速件")
                result.IsDispatch = false;
            else
                result.IsDispatch = null;

            if (quest != null)
            {
                //反應內容  ↓↓↓↓↓↓↓
                System.Func<string, bool> quest_predicate = x => x == quest.Code;
                result.ServiceAttitude = result.ServiceAttitudes.Any(quest_predicate) ? true : false;
                result.ServiceDefect = result.ServiceDefects.Any(quest_predicate) ? true : false;
                result.ProductAnomaly = result.ProductAnomalys.Any(quest_predicate) ? true : false;
                result.Other = result.Others.Any(quest_predicate) ? true : false;
                //反應內容  ↑↑↑↑↑↑↑
            }

            //顧客姓名
            result.UserName = user?.UserName;
            result.Gender = user?.Gender.GetDescription();

            //聯絡電話
            result.Telephone = user?.Mobile + "/" + user?.Telephone;
            //E-MAIL
            result.Email = user?.Email;
            //問題描述
            result.Content = @case.Content;
            //是否回電
            result.IsRecall = invoice?.IsRecall;

            return result;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public byte[] GetOnCallExcel(MisterDonutDataList model, DateTime end)
        {
            _builder.Clear();
            var @byte = _builder
                .AddWorkSheet(new SummaryWorksheet(), model.SummaryInformation, end)
                .AddWorkSheet(new DailyMonthSummaryWorksheet(), model.DailySummary, "日")
                .AddWorkSheet(new DailyMonthSummaryWorksheet(), model.MonthSummary, "月")

                .AddWorkSheet(new OnCallAndOtherWorksheet(), model.CommonOnCallsHistory, "來電紀錄")
                .AddWorkSheet(new OnEmailWorksheet(), model.CommonOnEmailHistory, "來信紀錄")
                .AddWorkSheet(new OnCallAndOtherWorksheet(), model.CommonOthersHistory, "其他紀錄")
                .AddWorkSheet(new OnComplaintWorksheet(), model.MisterDonutComplaintHistory, "客訴紀錄")
                .Build();
            return @byte;
        }
    }
}
