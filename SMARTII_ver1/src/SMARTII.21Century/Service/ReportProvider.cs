using System;
using System.Collections.Generic;
using System.Linq;
using ClosedXML.Excel;
using MultipartDataMediaFormatter.Infrastructure;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII._21Century.Domain;
using SMARTII.COMMON_BU;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;
using SMARTII.Domain.Master;
using SMARTII.Domain.Report;
using SMARTII.Service.Report.Builder;
using SMARTII.Service.Report.Provider;

namespace SMARTII._21Century.Service
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
            _21ComplaintReport report = (_21ComplaintReport)data;
            var wb = new XLWorkbook();

            #region 內文

            IXLWorksheet ws = wb.Worksheets.Add("C02R101");

            ws.Range(1, 1, 1, 5).Row(1).Merge();
            ws.Cell(1, 1).Value = "二十一世紀顧客問題處理回報單";

            ws.Range(1, 6, 1, 8).Row(1).Merge();

            if (report.IsRecognize.HasValue)
            {
                ws.Cell(1, 6).Value = string.Format("是否認列 : {0} 是  {1} 否", report.IsRecognize.Value ? "■" : "□", report.IsRecognize.Value ? "□" : "■");
            }
            else
            {
                ws.Cell(1, 6).Value = string.Format("是否認列 : {0} 是  {1} 否", "□", "□");
            }

            ws.Cell(2, 1).Value = "門市名稱";
            ws.Cell(2, 2).Value = report.StoreName;
            ws.Cell(2, 3).Value = "反應日期";
            ws.Cell(2, 4).Value = report.CaseCreateDate.ToString("yyyy/MM/dd HH:mm");
            ws.Cell(2, 5).Value = "發生日期";
            ws.Cell(2, 6).Value = report.CreateDate == null ? "" : report.CreateDate.Value.ToString("yyyy/MM/dd HH:mm");
            ws.Cell(2, 7).Value = "當班經理";
            ws.Cell(2, 8).Value = "";
            //隱藏
            ws.Row(3).Hide();
            //
            ws.Cell(4, 1).Value = "顧客姓名";
            ws.Cell(4, 2).Value = report.UserName + report.Gender;
            ws.Cell(4, 3).Value = "顧客電話";
            ws.Cell(4, 4).Value = report.Telephone;
            ws.Cell(4, 5).Value = "E-MAIL";
            ws.Cell(4, 6).Value = report.Email;
            ws.Range(4, 6, 4, 8).Row(1).Merge();

            ws.Cell(5, 1).Value = "問題反應來源";
            ws.Cell(5, 2).Value = string.Format("{0}0800客服專線", report.CallCenter ? "■" : "□");
            ws.Range(5, 3, 5, 4).Row(1).Merge();
            ws.Cell(5, 3).Value = string.Format("{0}21世紀官方網站", report.OfficalWebSite ? "■" : "□");
            ws.Range(5, 5, 5, 6).Row(1).Merge();
            ws.Cell(5, 5).Value = string.Format("{0}其他", report.Other ? "■" : "□");
            ws.Cell(5, 7).Value = "檔案編號";
            ws.Cell(5, 8).Value = report.InvoicID;

            ws.Range(6, 1, 6, 8).Row(1).Merge();
            ws.Cell(6, 1).Value = "問題反應權責處理單位請勾選(可複選)：\n" +
            "□營運管理  □行銷企劃  □品保採購  □財會總務  □工務發展  □其他(通路商品)";
            ws.Rows(6, 6).Style.Alignment.WrapText = true;

            ws.Range(7, 1, 7, 8).Row(1).Merge();
            string content = report.Content.Replace("\n", "\r\n");
            ws.Cell(7, 1).Value = $"一、問題敘述：{content} \r\n                               　　　　　　　　　　　　　　　　　　　　　　填單者：{report.WriteUser}";
            //隱藏
            ws.Row(8).Hide();

            ws.Range(9, 1, 9, 8).Row(1).Merge();
            ws.Cell(9, 1).Value = "二、處理方式說明：";

            ws.Range(10, 1, 10, 8).Row(1).Merge();
            ws.Cell(10, 1).Value = "門市處理者：";

            ws.Range(11, 1, 11, 8).Row(1).Merge();
            ws.Cell(11, 1).Value = "三、權責單位處理 & 改善對策：";
            //隱藏
            ws.Row(12).Hide();

            ws.Range(13, 1, 13, 8).Row(1).Merge();
            ws.Cell(13, 1).Value = "處理日期：                          門市店經理：";

            ws.Range(14, 1, 14, 8).Row(1).Merge();
            ws.Cell(14, 1).Value = "四、上層主管（區經理）確認顧客滿意/再次處理：";

            ws.Range(15, 1, 15, 8).Row(1).Merge();
            ws.Cell(15, 1).Value = "追蹤日期：                          區經理：";

            ws.Range(16, 1, 16, 8).Row(1).Merge();
            ws.Cell(16, 1).Value = "五、主管指示：";
            //隱藏
            ws.Row(17).Hide();

            ws.Range(18, 1, 18, 8).Row(1).Merge();
            ws.Cell(18, 1).Value = "單位主管/經理：";

            ws.Range(19, 1, 19, 8).Row(1).Merge();
            ws.Cell(19, 1).Value = "六、統智科技_再次評比顧客滿意評分表(不滿意0→滿意2)";

            ws.Range(20, 1, 20, 2).Row(1).Merge();
            ws.Range(20, 3, 20, 8).Row(1).Merge();
            ws.Cell(20, 1).Value = "顧客對於此次處理事件滿意度：";
            ws.Cell(20, 3).Value = "";

            ws.Range(21, 1, 21, 2).Row(1).Merge();
            ws.Range(21, 3, 21, 8).Row(1).Merge();
            ws.Cell(21, 1).Value = "請說明滿意度原因：";
            ws.Cell(21, 3).Value = "";

            ws.Range(22, 1, 22, 2).Row(1).Merge();
            ws.Range(22, 3, 22, 8).Row(1).Merge();
            ws.Cell(22, 1).Value = "顧客對於此次處理事件回購度：";
            ws.Cell(22, 3).Value = "";
            //隱藏
            ws.Row(23).Hide();

            //ws.Range(24, 1, 24, 2).Row(1).Merge();
            //ws.Cell(24, 1).Value = "總部客服業務窗口";
            //ws.Range(24, 3, 24, 4).Row(1).Merge();
            //ws.Cell(24, 3).Value = "後勤管理經理";
            //ws.Range(24, 5, 24, 6).Row(1).Merge();
            //ws.Cell(24, 5).Value = "知會\n" +
            //"相關單位經理 / 部主管";

            //ws.Range(24, 7, 24, 8).Row(1).Merge();
            //ws.Cell(24, 7).Value = "總經理";
            //ws.Rows(24, 24).Style.Alignment.WrapText = true;

            //ws.Range(25, 1, 25, 2).Row(1).Merge();
            //ws.Cell(25, 1).Value = "";
            //ws.Range(25, 3, 25, 4).Row(1).Merge();
            //ws.Cell(25, 3).Value = "";
            //ws.Range(25, 5, 25, 6).Row(1).Merge();
            //ws.Cell(25, 5).Value = "";
            //ws.Range(25, 7, 25, 8).Row(1).Merge();
            //ws.Cell(25, 7).Value = "";


            ws.Cell(24, 1).Value = "總部\n" + "客服業務窗口";
            ws.Cell(24, 2).Value = "總經理";
            ws.Cell(24, 3).Value = "後勤管理\n" + "經理";
            ws.Cell(24, 4).Value = "□知會\n" + "商品經理";
            ws.Range(24, 5, 27, 6).Row(1).Merge();
            ws.Cell(24, 5).Value = "□知會\n" + "整合行銷經理";

            ws.Range(24, 7, 27, 8).Row(1).Merge();
            ws.Cell(24, 7).Value = "部主管";
            ws.Rows(24, 27).Style.Alignment.WrapText = true;

            ws.Cell(25, 1).Value = "";
            ws.Cell(25, 2).Value = "";
            ws.Cell(25, 3).Value = "";
            ws.Cell(25, 4).Value = "";
            ws.Range(25, 5, 25, 6).Row(1).Merge();
            ws.Cell(25, 5).Value = "";
            ws.Range(25, 7, 25, 8).Row(1).Merge();
            ws.Cell(25, 7).Value = "";

            //高度設定
            ws.Rows(1, 23).Height = 21;
            ws.Row(1).Height = 27.75;
            ws.Row(2).Height = 19.5;
            ws.Row(6).Height = 42;
            ws.Row(7).Height = 99;
            ws.Row(9).Height = 70.5;
            ws.Row(10).Height = 21;
            ws.Row(11).Height = 70.5;
            ws.Row(13).Height = 21;
            ws.Row(14).Height = 70.5;
            ws.Row(15).Height = 21;
            ws.Row(16).Height = 70.5;
            ws.Row(18).Height = 21;
            ws.Row(24).Height = 35.25;
            ws.Row(25).Height = 63.75;
            ws.Row(27).Height = 35.25;
            ws.Row(28).Height = 63.75;
            //寬度設定
            ws.Column(1).Width = 14.14;
            ws.Column(2).Width = 16.71;
            ws.Column(3).Width = 9.86;
            ws.Column(4).Width = 15;
            ws.Column(5).Width = 9.86;
            ws.Column(6).Width = 15;
            ws.Column(7).Width = 9.57;
            ws.Column(8).Width = 13.71;
            //ws.Rows().AdjustToContents();
            //ws.Columns().AdjustToContents();

            //框線
            var rngTable1 = ws.Range("A1:H2");
            rngTable1.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            rngTable1.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            var rngTable2 = ws.Range("A4:H7");
            rngTable2.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            rngTable2.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            var rngTable3 = ws.Range("A9:H11");
            rngTable3.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            rngTable3.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            var rngTable4 = ws.Range("A13:H16");
            rngTable4.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            rngTable4.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            var rngTable5 = ws.Range("A18:H22");
            rngTable5.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            rngTable5.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            var rngTable6 = ws.Range("A24:H25");
            rngTable6.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            rngTable6.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            //var rngTable7 = ws.Range("A27:H28");
            //rngTable7.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            //rngTable7.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            //水平 至中、向左對齊
            ws.Range(1, 1, 1, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range(2, 6, 2, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range(2, 4, 2, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            ws.Rows(24, 25).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //ws.Rows(27, 28).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            //垂直 至中、向上對齊
            ws.Columns(1, 8).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Rows(7, 7).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            ws.Rows(9, 9).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            ws.Rows(11, 11).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            ws.Rows(14, 14).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            ws.Rows(16, 16).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

            //字體大小
            ws.Rows(1, 1).Style.Font.FontSize = 12;
            ws.Cell(1, 1).Style.Font.FontSize = 19;
            //字型
            ws.Rows(1, 25).Style.Font.FontName = "新細明體";
            ws.Columns(1, 8).Style.Font.FontName = "新細明體";

            #endregion 內文

            var result = ReportUtility.ConvertBookToByte(wb, data.Password);
            
            return new HttpFile(
                data.fileName.GetComplaintInvoiceName("xlsx", EssentialCache.BusinessKeyValue._21Century),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                result);
        }

        public BaseComplaintReport GeneratorPayload(string caseID, string invoiceID)
        {
            var classificationID = this.GetClassification();

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
                //取得改善方式
                con.IncludeBy(c => c.CASE_FINISH_REASON_DATA);
            });

            System.Func<CaseAssignmentComplaintInvoice, bool> predicate = x => x.InvoiceID == invoiceID;
            var invoice = @case.ComplaintInvoice.FirstOrDefault(predicate);
            var user = @case.CaseConcatUsers?.FirstOrDefault();

            // 組合資料
            var result = new _21ComplaintReport();

            //門市名稱
            result.StoreName = @case.CaseComplainedUsers.
                         FirstOrDefault(s => s.CaseComplainedUserType == CaseComplainedUserType.Responsibility &&
                                        s.UnitType == SMARTII.Domain.Organization.UnitType.Store)?.NodeName;
            //反應日期
            result.CaseCreateDate = @case.CreateDateTime;
            //發生日期
            result.CreateDate = invoice?.CreateDateTime;
            //顧客姓名
            result.UserName = user?.UserName;
            result.Gender = user?.Gender.GetDescription();
            //顧客電話
            result.Telephone = user?.Mobile + "/" + user?.Telephone;
            //E-MAIL
            result.Email = user?.Email;
            //是否認列
            var df = @case.CaseFinishReasonDatas.Where(x => x.ClassificationID == classificationID);
            if (df.Any())
            {
                var reason = df.First().Text == "認列";
                result.IsRecognize = reason;
            }

            //問題反應來源 ↓↓↓↓↓↓↓

            //0800客服專線
            result.CallCenter = @case.CaseSource.CaseSourceType == CaseSourceType.Phone ? true : false;
            //21世紀官方網站
            result.OfficalWebSite = @case.CaseSource.CaseSourceType == CaseSourceType.Email ? true : false;
            //其他
            result.Other = @case.CaseSource.CaseSourceType == CaseSourceType.Other ? true : false;

            //問題反應來源 ↑↑↑↑↑↑↑

            //檔案編號
            result.InvoicID = invoice?.InvoiceID;
            //問題描述
            result.Content = @case.Content;
            //填單者
            result.WriteUser = invoice?.CreateUserName;


            return result;
        }


        #region ColdStone 匯出Excel來電紀錄

        public byte[] GetOnCallExcel(_21CenturyDataList model, DateTime end)
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
                .AddWorkSheet(new OnComplaintWorksheet(), model._21CenturyComplaintHistory, "客訴紀錄")
                .Build();
            return @byte;
        }
        #endregion
    }
}
