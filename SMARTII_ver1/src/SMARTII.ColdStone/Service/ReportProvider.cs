using System;
using System.Linq;
using ClosedXML.Excel;
using MultipartDataMediaFormatter.Infrastructure;
using SMARTII.ColdStone.Domain;
using SMARTII.COMMON_BU;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;
using SMARTII.Domain.Master;
using SMARTII.Domain.Report;
using SMARTII.Service.Report.Builder;
using SMARTII.Service.Report.Provider;

namespace SMARTII.ColdStone.Service
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
            ColdStoneReport report = (ColdStoneReport)data;
            var wb = new XLWorkbook();

            #region 內文

            IXLWorksheet ws = wb.Worksheets.Add("C02R101");
            //酷聖石圖片
            ws.Range(1, 1, 1, 3).Row(1).Merge();
            ws.AddPicture(Properties.Resources.coldstone)
               .MoveTo(1, 1)
               .Scale(0.2); // optional: resize picture

            //主旨
            ws.Range(1, 4, 1, 15).Row(1).Merge();
            ws.Cell(1, 4).Value = "顧客反應處理單";

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
            ws.Cell(5, 2).Value = "■營業TEAM　□行銷TEAM　□其他";

            ws.Range(6, 1, 6, 4).Row(1).Merge();
            ws.Cell(6, 1).Value = "區組名稱";
            ws.Range(6, 5, 6, 10).Row(1).Merge();
            ws.Cell(6, 5).Value = "門市名稱";
            ws.Range(6, 11, 6, 14).Row(1).Merge();
            ws.Cell(6, 11).Value = "服務人員";
            ws.Range(6, 15, 6, 16).Row(1).Merge();
            ws.Cell(6, 15).Value = "區經理";

            ws.Range(7, 1, 7, 4).Row(1).Merge();
            ws.Cell(7, 1).Value = report.Area;
            ws.Range(7, 5, 7, 10).Row(1).Merge();
            ws.Cell(7, 5).Value = report.StoreName;
            ws.Range(7, 11, 7, 14).Row(1).Merge();
            ws.Cell(7, 11).Value = "";
            ws.Range(7, 15, 7, 16).Row(1).Merge();
            ws.Cell(7, 15).Value = report.Manager;

            ws.Range(9, 1, 9, 16).Row(1).Merge();
            ws.Cell(9, 1).Value = "案件分類：　■速件";

            ws.Range(10, 1, 10, 16).Row(1).Merge();
            ws.Cell(10, 1).Value = string.Format("反應內容：　{0}服務相關(人/事/物)　{1}商品面　{2}形象面　{3}其他疏失或建議",
                                                report.AboutService ? "■" : "□",
                                                report.AboutBrand ? "■" : "□",
                                                report.AboutImage ? "■" : "□",
                                                report.Other ? "■" : "□");

            ws.Range(11, 1, 11, 2).Row(1).Merge();
            ws.Cell(11, 1).Value = "顧客姓名";
            ws.Range(11, 3, 11, 6).Row(1).Merge();
            ws.Cell(11, 3).Value = report.UserName + report.Gender;
            ws.Range(11, 7, 11, 8).Row(1).Merge();
            ws.Cell(11, 7).Value = "購買日期";
            ws.Range(11, 9, 11, 12).Row(1).Merge();
            ws.Cell(11, 9).Value = "";
            ws.Range(11, 13, 11, 14).Row(1).Merge();
            ws.Cell(11, 13).Value = "時間";
            ws.Range(11, 15, 11, 16).Row(1).Merge();
            ws.Cell(11, 15).Value = "";

            ws.Range(12, 1, 12, 2).Row(1).Merge();
            ws.Cell(12, 1).Value = "連絡電話";
            ws.Range(12, 3, 12, 6).Row(1).Merge();
            ws.Cell(12, 3).Value = report.Telephone;
            ws.Range(12, 7, 12, 8).Row(1).Merge();
            ws.Cell(12, 7).Value = "E-MAIL";
            ws.Range(12, 9, 12, 16).Row(1).Merge();
            ws.Cell(12, 9).Value = report.Email;

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
            ws.Cell(16, 10).Value = "處理經過(詳述溝通重點)：由總監/承辦人填寫";

            ws.Range(17, 1, 17, 16).Row(1).Merge();

            ws.Range(18, 1, 18, 7).Row(1).Merge();
            ws.Cell(18, 1).Value = "實際改善作法：由總監/總部負責夥伴填寫";
            ws.Range(18, 8, 18, 16).Row(1).Merge();
            ws.Cell(18, 8).Value = "□處理結束　□持續處理中　□無法處理(無法聯繫顧客)";

            ws.Range(19, 1, 19, 16).Row(1).Merge();
            ws.Row(19).Hide();

            ws.Range(20, 1, 20, 16).Row(1).Merge();

            ws.Range(21, 1, 21, 16).Row(1).Merge();
            ws.Cell(21, 1).Value = "總部備註欄位";

            ws.Range(22, 1, 22, 16).Row(1).Merge();
            ws.Cell(22, 1).Value = "營業Team客訴請於三天內處理，並回覆處理結果";

            ws.Range(23, 1, 23, 16).Row(1).Merge();
            ws.Cell(23, 1).Value = "行銷Team客訴請於一天內處理，並回覆處理結果";

            ws.Range(24, 1, 24, 16).Row(1).Merge();
            ws.Cell(24, 1).Value = "";

            ws.Range(26, 1, 26, 2).Row(1).Merge();
            ws.Cell(26, 1).Value = "";
            ws.Range(26, 3, 26, 5).Row(1).Merge();
            ws.Cell(26, 3).Value = "Team主管";
            ws.Range(26, 6, 26, 7).Row(1).Merge();
            ws.Cell(26, 6).Value = "總部負責夥伴";
            ws.Range(26, 8, 26, 9).Row(1).Merge();
            ws.Cell(26, 8).Value = "區經理";
            ws.Range(26, 10, 26, 12).Row(1).Merge();
            ws.Cell(26, 10).Value = "總監";
            ws.Range(26, 13, 26, 14).Row(1).Merge();
            ws.Cell(26, 13).Value = "客服中心";

            ws.Range(27, 1, 27, 2).Row(1).Merge();
            ws.Cell(27, 1).Value = "";
            ws.Range(27, 3, 27, 5).Row(1).Merge();
            ws.Cell(27, 3).Value = "　　月　　日";
            ws.Range(27, 6, 27, 7).Row(1).Merge();
            ws.Cell(27, 6).Value = "　　月　　日";
            ws.Range(27, 8, 27, 9).Row(1).Merge();
            ws.Cell(27, 8).Value = "　　月　　日";
            ws.Range(27, 10, 27, 12).Row(1).Merge();
            ws.Cell(27, 10).Value = "　　月　　日";
            ws.Range(27, 13, 27, 14).Row(1).Merge();
            ws.Cell(27, 13).Value = "　　月　　日";

            ws.Range(29, 1, 29, 7).Row(1).Merge();
            ws.Cell(29, 1).Value = "□成立　□不成立";

            ws.Range(30, 1, 30, 16).Row(1).Merge();
            ws.Cell(30, 1).Value = "※客訴案件是否成立，將由營業TEAM判斷。";

            //高度設定
            ws.Rows(1, 29).Height = 21;
            ws.Row(1).Height = 54;
            ws.Row(2).Height = 10.5;
            ws.Row(3).Height = 13.5;
            ws.Row(6).Height = 13.5;
            ws.Row(8).Height = 6.75;

            ws.Row(13).Height = 105.75;
            ws.Row(14).Height = 21.75;
            ws.Row(15).Height = 7.5;
            ws.Row(17).Height = 60;
            ws.Row(20).Height = 159.75;
            ws.Row(21).Height = 18;
            ws.Row(22).Height = 13.5;
            ws.Row(23).Height = 13.5;
            ws.Row(24).Height = 13.5;
            ws.Row(25).Height = 7.5;
            ws.Row(26).Height = 13.5;
            ws.Row(27).Height = 42.75;
            ws.Row(28).Height = 6.75;
            ws.Row(29).Height = 13.5;
            //寬度設定
            ws.Column(1).Width = 8;
            ws.Columns(2, 16).Width = 3.57;
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
            var rngTable4 = ws.Range("A16:P24");
            rngTable4.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            rngTable4.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            var rngTable7 = ws.Range("A22:P24");
            rngTable7.Style.Border.InsideBorder = XLBorderStyleValues.Dashed;

            var rngTable5 = ws.Range("A26:N27");
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
            ws.Range(11, 7, 11, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range(11, 13, 11, 13).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            ws.Range(12, 1, 12, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range(12, 7, 12, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range(12, 13, 12, 13).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            ws.Range(13, 1, 13, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            ws.Range(16, 1, 16, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range(16, 6, 16, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range(16, 10, 16, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            ws.Row(18).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Row(21).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Row(26).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Row(27).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            ws.Row(29).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            //垂直 至中、向上對齊
            ws.Columns(1, 16).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Rows(1, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Bottom);
            ws.Range(13, 1, 13, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range(13, 3, 13, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            ws.Range(14, 3, 14, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

            ws.Rows(17, 17).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            ws.Rows(20, 20).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            ws.Rows(27, 27).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            ws.Rows(30, 30).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

            //字體大小
            ws.Cell(1, 4).Style.Font.FontSize = 20;
            ws.Range(13, 1, 13, 1).Style.Font.FontSize = 10;
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

            ws.Cell(13, 1).Style.Font.FontSize = 12;
            ws.Cell(13, 3).Style.Font.FontSize = 10;
            ws.Cell(14, 3).Style.Font.FontSize = 11;

            ws.Row(16).Style.Font.FontSize = 10;
            ws.Cell(16, 1).Style.Font.FontSize = 11;
            ws.Cell(16, 6).Style.Font.FontSize = 11;

            ws.Row(17).Style.Font.FontSize = 10;
            ws.Cell(18, 1).Style.Font.FontSize = 10;
            ws.Cell(18, 8).Style.Font.FontSize = 11;
            ws.Row(20).Style.Font.FontSize = 10;
            ws.Row(21).Style.Font.FontSize = 11;
            ws.Rows(22, 29).Style.Font.FontSize = 10;
            ws.Row(30).Style.Font.FontSize = 14;

            //字型
            ws.Rows(1, 29).Style.Font.FontName = "新細明體";
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
            ws.Cell(11, 7).Style.Font.FontName = "標楷體";
            ws.Cell(11, 13).Style.Font.FontName = "標楷體";
            ws.Cell(12, 1).Style.Font.FontName = "標楷體";
            ws.Cell(12, 7).Style.Font.FontName = "標楷體";
            ws.Cell(13, 1).Style.Font.FontName = "標楷體";
            ws.Cell(14, 3).Style.Font.FontName = "標楷體";
            ws.Cell(16, 1).Style.Font.FontName = "標楷體";
            ws.Cell(16, 6).Style.Font.FontName = "標楷體";
            ws.Cell(16, 10).Style.Font.FontName = "標楷體";
            ws.Cell(18, 1).Style.Font.FontName = "標楷體";
            ws.Cell(18, 8).Style.Font.FontName = "標楷體";
            ws.Rows(21, 23).Style.Font.FontName = "標楷體";
            ws.Row(27).Style.Font.FontName = "標楷體";
            ws.Row(30).Style.Font.FontName = "標楷體";

            //字體色彩
            ws.Cell(4, 1).Style.Font.FontColor = XLColor.White;
            ws.Cell(4, 7).Style.Font.FontColor = XLColor.White;
            ws.Cell(4, 12).Style.Font.FontColor = XLColor.White;
            ws.Cell(6, 1).Style.Font.FontColor = XLColor.White;
            ws.Cell(6, 5).Style.Font.FontColor = XLColor.White;
            ws.Cell(6, 11).Style.Font.FontColor = XLColor.White;
            ws.Cell(6, 15).Style.Font.FontColor = XLColor.White;

            ws.Cell(11, 1).Style.Font.FontColor = XLColor.White;
            ws.Cell(11, 7).Style.Font.FontColor = XLColor.White;
            ws.Cell(11, 13).Style.Font.FontColor = XLColor.White;
            ws.Cell(12, 1).Style.Font.FontColor = XLColor.White;
            ws.Cell(12, 7).Style.Font.FontColor = XLColor.White;
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
            ws.Cell(11, 7).Style.Fill.BackgroundColor = XLColor.Black;
            ws.Cell(11, 13).Style.Fill.BackgroundColor = XLColor.Black;
            ws.Cell(12, 1).Style.Fill.BackgroundColor = XLColor.Black;
            ws.Cell(12, 7).Style.Fill.BackgroundColor = XLColor.Black;
            ws.Cell(13, 1).Style.Fill.BackgroundColor = XLColor.Black;
            ws.Cell(16, 1).Style.Fill.BackgroundColor = XLColor.FromArgb(220, 220, 220);
            ws.Cell(16, 6).Style.Fill.BackgroundColor = XLColor.FromArgb(220, 220, 220);
            ws.Cell(16, 10).Style.Fill.BackgroundColor = XLColor.FromArgb(220, 220, 220);
            ws.Cell(18, 1).Style.Fill.BackgroundColor = XLColor.FromArgb(220, 220, 220);
            ws.Cell(18, 8).Style.Fill.BackgroundColor = XLColor.Gray;
            ws.Cell(21, 1).Style.Fill.BackgroundColor = XLColor.Gray;
            ws.Cell(29, 1).Style.Fill.BackgroundColor = XLColor.FromArgb(220, 220, 220);

            //粗體
            ws.Cell(1, 4).Style.Font.Bold = true;
            ws.Cell(30, 1).Style.Font.Bold = true;
            //合併兩Row
            ws.Range("A13:B14").Merge();

            #endregion 內文

            var result = ReportUtility.ConvertBookToByte(wb, report.Password);

            return new HttpFile(
                data.fileName.GetComplaintInvoiceName("xlsx", EssentialCache.BusinessKeyValue.ColdStone),
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

            //取得案件 問題分類上層分類
            var quset = this.GetQuestionClassificationByCode((con) =>
            {
                con.And(c => c.ID == @case.QuestionClassificationID);
                con.IncludeBy(c => c.QUESTION_CLASSIFICATION2);
            });

            System.Func<CaseAssignmentComplaintInvoice, bool> predicate = x => x.InvoiceID == invoiceID;
            var invoice = @case.ComplaintInvoice.FirstOrDefault(predicate);
            var user = @case.CaseConcatUsers?.FirstOrDefault();
            var store = @case.CaseComplainedUsers.
                         FirstOrDefault(s => s.CaseComplainedUserType == CaseComplainedUserType.Responsibility &&
                                        s.UnitType == SMARTII.Domain.Organization.UnitType.Store);

            // 組合資料
            var result = new ColdStoneReport();

            //開單日
            result.CreateDate = invoice?.CreateDateTime;
            //編號
            result.InvoicID = invoice?.InvoiceID;
            //區組名稱
            result.Area = store?.SupervisorNodeName;
            //門市名稱            
            result.StoreName = store?.StoreNo + store?.NodeName;
            //區經理
            result.Manager = store?.SupervisorUserName;

            //反應內容  ↓↓↓↓↓↓↓
            result.AboutService = result.AboutServices.Any(c => c == quset.Parent?.Code);
            result.AboutBrand = result.AboutBrands.Any(c => c == quset.Parent?.Code);
            result.AboutImage = result.AboutImages.Any(c => c == quset.Parent?.Code);
            result.Other = (result.AboutService || result.AboutImage || result.AboutBrand) ? false : true;
            //反應內容  ↑↑↑↑↑↑↑

            //顧客姓名
            result.UserName = user?.UserName;
            result.Gender = user?.Gender.GetDescription();

            //聯絡電話
            result.Telephone = user?.Mobile + "/" + user?.Telephone;
            //E-MAIL
            result.Email = user?.Email;
            //反應內文
            result.Content = @case.Content;
            //是否回電
            result.IsRecall = invoice?.IsRecall ;

            return result;
        }
        #region ColdStone 匯出Excel來電紀錄

        public byte[] GetOnCallExcel(ColdStoneDataList model, DateTime end)
        {
            _builder.Clear();
            var @byte = _builder
                .AddWorkSheet(new SummaryWorksheet(), model.SummaryInformation, end)
                .AddWorkSheet(new DailyMonthSummaryWorksheet(), model.DailySummary, "日")
                .AddWorkSheet(new DailyMonthSummaryWorksheet(), model.MonthSummary, "月")

                .AddWorkSheet(new OnCallAndOtherWorksheet(), model.CommonOnCallsHistory, "來電紀錄")
                .AddWorkSheet(new OnEmailWorksheet(), model.CommonOnEmailHistory, "來信紀錄")
                .AddWorkSheet(new OnCallAndOtherWorksheet(), model.CommonOthersHistory, "其他紀錄")
                .AddWorkSheet(new OnPathwayWorksheet(), model.CommonPathwayHistory, "通路紀錄")
                .AddWorkSheet(new OnComplaintWorksheet(), model.ColdStoneComplaint, "客訴紀錄")
                .Build();
            return @byte;
        }
        #endregion
    }
}
