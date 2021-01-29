using System;
using System.Collections.Generic;
using System.Linq;
using ClosedXML.Excel;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Case;
using SMARTII.Domain.Report;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Master;
using System.IO;
using Autofac.Features.Indexed;
using SMARTII.Domain.DI;
using SMARTII.Service.Cache;

namespace SMARTII.Service.Report.Provider
{
    public class CaseSearchProvider : ICaseSearchProvider
    {
        private readonly IMasterAggregate _IMasterAggregate;
        private readonly ICaseAggregate _CaseAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly IIndex<string, IExcelParser> _ExcelParser;



        public CaseSearchProvider(IMasterAggregate IMasterAggregate, ICaseAggregate CaseAggregate, IOrganizationAggregate OrganizationAggregate, IIndex<string, IExcelParser> ExcelParser)
        {
            _IMasterAggregate = IMasterAggregate;
            _CaseAggregate = CaseAggregate;
            _OrganizationAggregate = OrganizationAggregate;
            _ExcelParser = ExcelParser;
        }

        /// <summary>
        /// 案件查詢-Excel匯出 (客服)SP
        /// </summary>
        /// <returns></returns>
        public Byte[] CreateCaseCustomerExcel(List<SP_GetCaseList> list, CaseCallCenterCondition condition)
        {
            //撈取的資料
            var data = GetCaseCustomerList(list);

            //查詢條件-分類清單
            var classList_Search = new List<QuestionClassification>();
            if (condition.ClassificationID != null)
            {
                var con = new MSSQLCondition<VW_QUESTION_CLASSIFICATION_NESTED>();
                con.And(x => x.ID == (int)condition.ClassificationID);
                classList_Search = _IMasterAggregate.VWQuestionClassification_QuestionClassification_.GetList(con).ToList();
            }

            //查詢結果-分類清單
            var con2 = new MSSQLCondition<VW_QUESTION_CLASSIFICATION_NESTED>();
            var dataClassList = data.Select(x => x.ClassificationID).ToArray();
            con2.And(x => dataClassList.Contains(x.ID));
            var classList = _IMasterAggregate.VWQuestionClassification_QuestionClassification_.GetList(con2).ToList();
            int classificationCount = classList.Count == 0 ? 0 : classList.Max(x => x.Level);

            //查詢結果-結案處置
            var con3 = new MSSQLCondition<CASE_FINISH_REASON_CLASSIFICATION>();
            var nodeList = classList.Select(x => x.NodeID).Distinct().ToList();//無nodeid 資料,故使用分類查詢結果為來源
            con3.And(x => nodeList.Contains(x.NODE_ID));
            var titleList = _IMasterAggregate.CaseFinishReasonClassification_T1_T2_.GetList(con3)?.Select(x => x.Title).ToList();




            #region 產出Excel
            XLWorkbook book = new XLWorkbook();

            var ws = book.AddWorksheet("工作表1");

            #region 開頭

            ws.Cell(1, 1).Value = "企業別：";
            ws.Cell(1, 2).Value = "'" + condition.NodeName;
            ws.Cell(1, 3).Value = "案件來源：";
            ws.Cell(1, 4).Value = condition.CaseSourceType == null ? "" : "'" + ((CaseSourceType)condition.CaseSourceType).GetDescription();
            ws.Cell(1, 5).Value = "負責人：";
            ws.Cell(1, 6).Value = "'" + condition.ApplyUserName;
            ws.Cell(1, 7).Value = "立案時間：";
            ws.Cell(1, 8).Value = "'" + condition.CreateTimeRange;

            ws.Cell(2, 1).Value = "反應者類型：";
            ws.Cell(2, 2).Value = condition.CaseConcatUnitType == null ? "" : "'" + ((UnitType)condition.CaseConcatUnitType).GetDescription();

            switch (condition.CaseConcatUnitType)
            {
                case UnitType.Customer:
                    ws.Cell(2, 3).Value = "姓名：";
                    ws.Cell(2, 4).Value = "'" + condition.ConcatName;
                    ws.Cell(2, 5).Value = "電話：";
                    ws.Cell(2, 6).Value = "'" + condition.ConcatTelephone;
                    ws.Cell(2, 7).Value = "電子信箱：";
                    ws.Cell(2, 8).Value = "'" + condition.ConcatEmail;
                    break;
                case UnitType.Store:
                    ws.Cell(2, 3).Value = "店名：";
                    ws.Cell(2, 4).Value = "'" + condition.ConcatStoreName;
                    ws.Cell(2, 5).Value = "店號：";
                    ws.Cell(2, 6).Value = "'" + condition.ConcatStoreNo;
                    ws.Cell(2, 7).Value = "組織：";
                    ws.Cell(2, 8).Value = condition.ConcatNode == null || condition.ConcatNode.Count() == 0 ? "" : string.Join("/", condition.ConcatNode.Select(x => x.Name));
                    break;
                case UnitType.Organization:
                    ws.Cell(2, 3).Value = "單位名稱：";
                    ws.Cell(2, 4).Value = "'" + condition.ConcatNodeName;
                    ws.Cell(2, 5).Value = "組織：";
                    ws.Cell(2, 6).Value = condition.ConcatNode == null || condition.ConcatNode.Count() == 0 ? "" : string.Join("/", condition.ConcatNode.Select(x => x.Name));
                    break;
                case null:
                    break;
            }
            ws.Cell(3, 1).Value = "案件編號：";
            ws.Cell(3, 2).Value = "'" + condition.CaseID;
            ws.Cell(3, 3).Value = "案件內容：";
            ws.Cell(3, 4).Value = "'" + condition.CaseContent;
            ws.Cell(3, 5).Value = "結案內容：";
            ws.Cell(3, 6).Value = "'" + condition.FinishContent;
            ws.Cell(3, 7).Value = "反應單號：";
            ws.Cell(3, 8).Value = "'" + condition.InvoiceID;

            ws.Cell(4, 1).Value = "被反應者類型：";
            ws.Cell(4, 2).Value = condition.CaseComplainedUnitType == null ? "" : "'" + ((UnitType)condition.CaseComplainedUnitType).GetDescription();
            switch (condition.CaseComplainedUnitType)
            {
                case UnitType.Store:
                    ws.Cell(4, 3).Value = "店名：";
                    ws.Cell(4, 4).Value = "'" + condition.CaseComplainedStoreName;
                    ws.Cell(4, 5).Value = "店號：";
                    ws.Cell(4, 6).Value = "'" + condition.CaseComplainedStoreNo;
                    ws.Cell(4, 7).Value = "組織：";
                    ws.Cell(4, 8).Value = condition.ComplainedNode == null || condition.ComplainedNode.Count() == 0 ? "" : string.Join("/", condition.ComplainedNode.Select(x => x.Name));
                    ws.Cell(4, 9).Value = "期望期限：";
                    ws.Cell(4, 10).Value = "'" + condition.ExpectDateTimeRange;
                    break;
                case UnitType.Organization:
                    ws.Cell(4, 3).Value = "單位名稱：";
                    ws.Cell(4, 4).Value = "'" + condition.CaseComplainedNodeName;
                    ws.Cell(4, 5).Value = "組織：";
                    ws.Cell(4, 6).Value = condition.ComplainedNode == null || condition.ComplainedNode.Count() == 0 ? "" : string.Join("/", condition.ComplainedNode.Select(x => x.Name));
                    ws.Cell(4, 7).Value = "期望期限：";
                    ws.Cell(4, 8).Value = "'" + condition.ExpectDateTimeRange;
                    break;
                case null:
                    break;
            }

            if (condition.ClassificationID == null || !classList_Search.Any())
            {
                ws.Cell(5, 1).Value = "問題分類1：";
                ws.Cell(5, 2).Value = "";
                ws.Cell(5, 3).Value = "問題分類2：";
                ws.Cell(5, 4).Value = "";
                ws.Cell(5, 5).Value = "問題分類3：";
                ws.Cell(5, 6).Value = "";
                ws.Cell(5, 7).Value = "案件標籤：";
                ws.Cell(5, 8).Value = condition.CaseTagList != null ? "'" + string.Join("/", condition.CaseTagList) : "";
            }
            else
            {
                string[] parentList = classList_Search.Where(x => x.ID == condition.ClassificationID).First().ParentNamePathByArray;
                int Column = 1;
                for (int i = 0; i < parentList.Count(); i++)
                {
                    ws.Cell(5, Column).Value = "問題分類" + (i + 1) + "：";
                    Column = Column + 1;
                    ws.Cell(5, Column).Value = "'" + parentList[i];
                    Column = Column + 1;
                }
                ws.Cell(5, Column).Value = "案件標籤：";
                Column = Column + 1;
                ws.Cell(5, Column).Value = condition.CaseTagList != null ? "'" + string.Join("/", condition.CaseTagList) : "";
            }
            ws.Cell(6, 1).Value = "案件等級：";
            ws.Cell(6, 2).Value = "'" + condition.CaseWarningName;
            ws.Cell(6, 3).Value = "案件狀態：";
            ws.Cell(6, 4).Value = condition.CaseType == null ? "" : "'" + ((CaseType)condition.CaseType).GetDescription();
            ws.Cell(6, 5).Value = "結案處置：";
            ws.Cell(6, 6).Value = condition.ReasonName != null ? "'" + string.Join("/", condition.ReasonName) : ""; ;

            int Field = 8;

            ws.Cell(Field, 1).Value = "企業別";
            ws.Cell(Field, 2).Value = "案件來源";
            ws.Cell(Field, 3).Value = "來源時間";
            ws.Cell(Field, 4).Value = "立案時間";
            ws.Cell(Field, 5).Value = "案件編號";
            ws.Cell(Field, 6).Value = "期望期限";
            ws.Cell(Field, 7).Value = "案件等級";
            ws.Cell(Field, 8).Value = "案件狀態";
            ws.Cell(Field, 9).Value = "預立案";
            ws.Cell(Field, 10).Value = "關注案件";
            for (int i = 1; i <= classificationCount; i++)
            {
                ws.Cell(Field, 10 + i).Value = "問題分類" + i.ToString();
            }
            int column = 10 + classificationCount;

            ws.Cell(Field, column + 1).Value = "反應者類型";
            ws.Cell(Field, column + 2).Value = "反應者";
            ws.Cell(Field, column + 3).Value = "手機";
            ws.Cell(Field, column + 4).Value = "電話1";
            ws.Cell(Field, column + 5).Value = "電話2";
            ws.Cell(Field, column + 6).Value = "電子信箱";
            ws.Cell(Field, column + 7).Value = "地址";
            ws.Cell(Field, column + 8).Value = "門市";
            ws.Cell(Field, column + 9).Value = "姓名";
            ws.Cell(Field, column + 10).Value = "電話";
            ws.Cell(Field, column + 11).Value = "組織單位";
            ws.Cell(Field, column + 12).Value = "姓名";
            ws.Cell(Field, column + 13).Value = "電話";
            ws.Cell(Field, column + 14).Value = "被反應者類型";
            ws.Cell(Field, column + 15).Value = "門市";
            ws.Cell(Field, column + 16).Value = "門市負責人";
            ws.Cell(Field, column + 17).Value = "門市區經理";
            ws.Cell(Field, column + 18).Value = "門市電話";
            ws.Cell(Field, column + 19).Value = "組織單位";
            ws.Cell(Field, column + 20).Value = "姓名";
            ws.Cell(Field, column + 21).Value = "電話";
            ws.Cell(Field, column + 22).Value = "案件內容";
            ws.Cell(Field, column + 23).Value = "案件期限";
            ws.Cell(Field, column + 24).Value = "商品名稱";
            ws.Cell(Field, column + 25).Value = "國際條碼";
            ws.Cell(Field, column + 26).Value = "批號";
            ws.Cell(Field, column + 27).Value = "卡號";
            ws.Cell(Field, column + 28).Value = "型號";
            ws.Cell(Field, column + 29).Value = "名稱";
            ws.Cell(Field, column + 30).Value = "購買日期";
            ws.Cell(Field, column + 31).Value = "結案內容";
            ws.Cell(Field, column + 32).Value = "結案時間";

            int column2 = titleList.Count;
            if (titleList.Any())
            {
                int index = 0;
                foreach (var titleitem in titleList)
                {
                    ws.Cell(Field, column + index + 33).Value = $"結案處置({titleitem})";
                    index++;
                }
                column2 = column2 - 1;
            }
            else
            {
                ws.Cell(Field, column + 33).Value = "結案處置";
            }

            ws.Cell(Field, column + column2 + 34).Value = "負責人";
            ws.Cell(Field, column + column2 + 35).Value = "案件標籤";


            #endregion

            int row = 9;
            foreach (var item in data)
            {
                ws.Cell(row, 1).Value = item.NodeName;
                ws.Cell(row, 2).Value = item.CaseSourceType;
                ws.Cell(row, 3).Value = item.IncomingDateTime;
                ws.Cell(row, 4).Value = item.CreateTime;
                ws.Cell(row, 5).Value = item.CaseID;
                ws.Cell(row, 6).Value = item.ExpectedPeriod;
                ws.Cell(row, 7).Value = item.CaseWarningName;
                ws.Cell(row, 8).Value = item.CaseType;
                ws.Cell(row, 9).Value = item.IsPrevention;
                ws.Cell(row, 10).Value = item.IsAttension;
                //分類
                for (int i = 1; i <= classificationCount; i++)
                {
                    if (classList.Where(x => x.ID == item.ClassificationID).Count() == 0)
                        continue;
                    var listclass = classList.Where(x => x.ID == item.ClassificationID).FirstOrDefault();
                    ws.Cell(row, 10 + i).Value = "'" + listclass.ParentNamePathByArray[i - 1];

                    if (listclass.ParentNamePathByArray.Count() < i + 1)
                    {
                        break;
                    }
                }
                ws.Cell(row, column + 1).Value = item.ConcatUnitType;
                ws.Cell(row, column + 2).Value = item.ConcatCustomerName;
                ws.Cell(row, column + 3).Value = item.ConcatCustomerMobile;
                ws.Cell(row, column + 4).Value = item.ConcatCustomerTelephone1;
                ws.Cell(row, column + 5).Value = item.ConcatCustomerTelephone2;
                ws.Cell(row, column + 6).Value = item.ConcatCustomerMail;
                ws.Cell(row, column + 7).Value = item.ConcatCustomerAddress;
                ws.Cell(row, column + 8).Value = item.ConcatStore;
                ws.Cell(row, column + 9).Value = item.ConcatStoreName;
                ws.Cell(row, column + 10).Value = item.ConcatStoreTelephone;
                ws.Cell(row, column + 11).Value = item.ConcatOrganization;
                ws.Cell(row, column + 12).Value = item.ConcatOrganizationName;
                ws.Cell(row, column + 13).Value = item.ConcatOrganizationTelephone;
                ws.Cell(row, column + 14).Value = item.ComplainedUnitType;
                ws.Cell(row, column + 15).Value = item.ComplainedStore;
                ws.Cell(row, column + 16).Value = item.ComplainedStoreApplyUserName;
                ws.Cell(row, column + 17).Value = item.ComplainedStoreSupervisorUserName;
                ws.Cell(row, column + 18).Value = item.ComplainedStoreTelephone;
                ws.Cell(row, column + 19).Value = item.ComplainedOrganization;
                ws.Cell(row, column + 20).Value = item.ComplainedOrganizationName;
                ws.Cell(row, column + 21).Value = item.ComplainedOrganizationTelephone;
                ws.Cell(row, column + 22).Value = item.CaseContent;
                ws.Cell(row, column + 22).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 23).Value = item.PromiseDateTime;
                ws.Cell(row, column + 24).Value = "'" + item.OtherCommodityName;
                ws.Cell(row, column + 24).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 25).Value = "'" + item.OtherInternationalBarcode;
                ws.Cell(row, column + 25).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 26).Value = "'" + item.OtherBatchNo;
                ws.Cell(row, column + 26).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 27).Value = "'" + item.OtherCardNumber;
                ws.Cell(row, column + 27).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 28).Value = "'" + item.OtherProductModel;
                ws.Cell(row, column + 28).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 29).Value = "'" + item.OtherProductName;
                ws.Cell(row, column + 29).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 30).Value = "'" + item.OtherPurchaseDay;
                ws.Cell(row, column + 30).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 31).Value = item.FinishContent;
                ws.Cell(row, column + 31).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 32).Value = item.FinishDateTime;

                if (titleList.Any())
                {
                    int index = 0;
                    foreach (var titleitem in titleList)
                    {
                        if (item.ReasonList.ContainsKey(titleitem))
                            ws.Cell(row, column + index + 33).Value = item.ReasonList[$"{titleitem}"];
                        else
                            ws.Cell(row, column + index + 33).Value = "";
                        index++;
                    }
                }
                else
                {
                    ws.Cell(row, column + 33).Value = item.ReasonName;
                }
                ws.Cell(row, column + column2 + 34).Value = item.ApplyUserName;
                ws.Cell(row, column + column2 + 35).Value = item.CaseTag;
                row++;
            }
            //左右至中
            ws.Range(Field, 1, row - 1, column + column2 + 35).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            //水平 至中
            ws.Range(Field, 1, row - 1, column + column2 + 35).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            //格線
            var rngTable1 = ws.Range(Field, 1, row - 1, column + column2 + 35);
            rngTable1.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            rngTable1.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            //自動對應欄位寬度

            ws.Columns().AdjustToContents();

            //設定案件內容與結案內容固定欄寬
            ws.Column(column + 22).Width = 100;
            ws.Column(column + 31).Width = 100;
            ws.Range(Field + 1, column + 22, row - 1, column + 22).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Range(Field + 1, column + 31, row - 1, column + 31).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

            //設定結案處置固定欄寬
            int indexw = 0;
            do
            {
                ws.Column(column + indexw + 33).Width = 25;
                indexw++;
            }
            while (indexw < titleList.Count);


            ws.Rows().AdjustToContents();
            //自動調整列高後，需.ClearHeight才可依內容高度顯示
            //for (int i = 9; i <= row; i++)
            //{
            //    ws.Row(i).ClearHeight();
            //}

            //20200911 固定列高
            for (int i = 9; i < row; i++)
            {
                ws.Row(i).Height = 95;
            }


            var result = ReportUtility.ConvertBookToByte(book, "");

            #endregion

            return result;
        }
        /// <summary>
        /// 案件查詢 - 轉換Excel Model(客服用)
        /// </summary>
        /// <returns></returns>
        private List<ExcelCaseCustomerList> GetCaseCustomerList(List<SP_GetCaseList> list)
        {
            List<ExcelCaseCustomerList> excelList = new List<ExcelCaseCustomerList>();

            foreach (var data in list)
            {
                var concat = data.CaseConcatUsersList.Count() == 0 ? null : data.CaseConcatUsersList.First();
                var complained = data.CaseComplainedUsersList.Where(x => x.CaseComplainedUserType == CaseComplainedUserType.Responsibility).Count() == 0
                    ? null : data.CaseComplainedUsersList.Where(x => x.CaseComplainedUserType == CaseComplainedUserType.Responsibility).First();

                var callCenter = new ExcelCaseCustomerList()
                {
                    NodeName = "'" + data.NodeName,
                    CaseSourceType = "'" + data.SourceType.GetDescription(),
                    IncomingDateTime = data.IncomingDateTime.HasValue ? "'" + data.IncomingDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                    CreateTime = "'" + data.CreateDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    CaseID = "'" + data.CaseID,
                    ExpectedPeriod = data.ExpectDateTime.HasValue ? "'" + data.ExpectDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                    CaseWarningName = "'" + data.CaseWarningName,
                    CaseType = "'" + data.CaseType.GetDescription(),
                    IsPrevention = data.IsPrevention ? "是" : "否",
                    IsAttension = data.IsAttension ? "是" : "否",
                    ClassificationID = data.ClassificationID,
                    //反應者
                    ConcatUnitType = concat == null ? "" : "'" + concat.UnitType.GetDescription(),
                    //消費者
                    ConcatCustomerName = concat == null ? "" : concat.UnitType == UnitType.Customer ? "'" + concat.UserName + concat.Gender.GetDescription() : "",
                    ConcatCustomerMobile = concat == null ? "" : concat.UnitType == UnitType.Customer ? "'" + concat.Mobile : "",
                    ConcatCustomerTelephone1 = concat == null ? "" : concat.UnitType == UnitType.Customer ? "'" + concat.Telephone : "",
                    ConcatCustomerTelephone2 = concat == null ? "" : concat.UnitType == UnitType.Customer ? "'" + concat.TelephoneBak : "",
                    ConcatCustomerMail = concat == null ? "" : concat.UnitType == UnitType.Customer ? "'" + concat.Email : "",
                    ConcatCustomerAddress = concat == null ? "" : concat.UnitType == UnitType.Customer ? "'" + concat.Address : "",
                    //門市
                    ConcatStore = concat == null ? "" : concat.UnitType == UnitType.Store ? "'" + concat.NodeName + concat.StoreNo : "",
                    ConcatStoreName = concat == null ? "" : concat.UnitType == UnitType.Store ? "'" + concat.UserName : "",
                    ConcatStoreTelephone = concat == null ? "" : concat.UnitType == UnitType.Store ? "'" + concat.Telephone : "",
                    //組織
                    ConcatOrganization = concat == null ? "" : concat.UnitType == UnitType.Organization ? "'" + concat.NodeName : "",
                    ConcatOrganizationName = concat == null ? "" : concat.UnitType == UnitType.Organization ? "'" + concat.UserName : "",
                    ConcatOrganizationTelephone = concat == null ? "" : concat.UnitType == UnitType.Organization ? "'" + concat.Telephone : "",

                    //被反應者
                    ComplainedUnitType = complained == null ? "" : "'" + complained.UnitType.GetDescription(),

                    //門市
                    ComplainedStore = complained != null ? (complained.UnitType == UnitType.Store ? "'" + complained.NodeName + complained.StoreNo : "") : "",
                    ComplainedStoreApplyUserName = complained != null ? (complained.UnitType == UnitType.Store ? "'" + complained.OwnerUserName : "") : "",
                    ComplainedStoreSupervisorUserName = complained != null ? (complained.UnitType == UnitType.Store ? "'" + complained.SupervisorUserName : "") : "",
                    ComplainedStoreTelephone = complained != null ? (complained.UnitType == UnitType.Store ? "'" + complained.Telephone : "") : "",

                    //組織
                    ComplainedOrganization = complained != null ? (complained.UnitType == UnitType.Organization ? "'" + complained.NodeName : "") : "",
                    ComplainedOrganizationName = complained != null ? (complained.UnitType == UnitType.Organization ? "'" + complained.OwnerUserName : "") : "",
                    ComplainedOrganizationTelephone = complained != null ? (complained.UnitType == UnitType.Organization ? "'" + complained.OwnerUserPhone : "") : "",
                    //案件內容
                    CaseContent = "'" + data.CaseContent,
                    PromiseDateTime = data.PromiseDateTime.HasValue ? "'" + data.PromiseDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",

                    FinishContent = "'" + data.FinishContent,
                    FinishDateTime = data.FinishDateTime.HasValue ? "'" + data.FinishDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                    //ReasonName = data.CaseFinishReasonDatas.Any() ? "'" + string.Join("/", data.CaseFinishReasonDatas.Select(y => y.Text)) : "",
                    ApplyUserName = "'" + data.ApplyUserName,
                    CaseTag = data.CaseTagList.Any() ? "'" + string.Join("/", data.CaseTagList.Select(y => y.Name)) : "",
                    JContent = data.JContent,
                };
                //ReasonName依所屬Title分類 20200828
                IDictionary<string, string> dist = new Dictionary<string, string>();
                var titleList = data.CaseFinishReasonDatas.Select(x => x.CaseFinishReasonClassification.Title).Distinct().ToList();
                if (titleList.Any())
                {
                    titleList.ForEach(x =>
                    {
                        dist.Add($"{x}", "'" + string.Join("/", data.CaseFinishReasonDatas.Where(y => y.CaseFinishReasonClassification.Title == x).Select(y => y.Text)));
                    });
                }
                callCenter.ReasonList = dist;

                //其他資訊
                var excelParser = _ExcelParser.TryGetService(data.NodeKey);
                callCenter = excelParser != null ? _ExcelParser[data.NodeKey].CaseSearchItemParsing(data.CaseItemList, callCenter) : callCenter;

                excelList.Add(callCenter);
            }
            //資料匯出，排序案件編號
            excelList = excelList.OrderBy(x => x.CaseID).ToList();
            return excelList;
        }

        /// <summary>
        /// 案件查詢(總部、門市)
        /// </summary>
        /// <returns></returns>
        public Byte[] CreateCaseHSExcel(List<SP_GetCaseList> list, CaseHSCondition condition)
        {
            //撈取的資料
            var data = GetCaseHSList(list);

            //查詢條件-分類清單
            var classList_Search = new List<QuestionClassification>();
            if (condition.ClassificationID != null)
            {
                var con = new MSSQLCondition<VW_QUESTION_CLASSIFICATION_NESTED>();
                con.And(x => x.ID == (int)condition.ClassificationID);
                classList_Search = _IMasterAggregate.VWQuestionClassification_QuestionClassification_.GetList(con).ToList();
            }

            //查詢結果-分類清單
            var con2 = new MSSQLCondition<VW_QUESTION_CLASSIFICATION_NESTED>();
            var dataClassList = data.Select(x => x.ClassificationID).ToArray();
            con2.And(x => dataClassList.Contains(x.ID));
            var classList = _IMasterAggregate.VWQuestionClassification_QuestionClassification_.GetList(con2).ToList();
            int classificationCount = classList.Count == 0 ? 0 : classList.Max(x => x.Level);

            //查詢結果-結案處置
            var con3 = new MSSQLCondition<CASE_FINISH_REASON_CLASSIFICATION>();
            var nodeList = classList.Select(x => x.NodeID).Distinct().ToList();//無nodeid 資料,故使用分類查詢結果為來源
            con3.And(x => nodeList.Contains(x.NODE_ID));
            var titleList = _IMasterAggregate.CaseFinishReasonClassification_T1_T2_.GetList(con3)?.Select(x => x.Title).ToList();


            #region 產出Excel
            XLWorkbook book = new XLWorkbook();

            var ws = book.AddWorksheet("工作表1");

            #region 開頭

            ws.Cell(1, 1).Value = "企業別：";
            ws.Cell(1, 2).Value = "'" + condition.NodeName;
            ws.Cell(1, 3).Value = "案件來源：";
            ws.Cell(1, 4).Value = condition.CaseSourceType == null ? "" : "'" + ((CaseSourceType)condition.CaseSourceType).GetDescription();
            ws.Cell(1, 5).Value = "反應單號：";
            ws.Cell(1, 6).Value = "'" + condition.InvoiceID;
            ws.Cell(1, 7).Value = "立案時間：";
            ws.Cell(1, 8).Value = "'" + condition.CreateTimeRange;

            ws.Cell(2, 1).Value = "反應者類型：";
            ws.Cell(2, 2).Value = condition.CaseConcatUnitType == null ? "" : "'" + ((UnitType)condition.CaseConcatUnitType).GetDescription();

            switch (condition.CaseConcatUnitType)
            {
                case UnitType.Customer:
                    ws.Cell(2, 3).Value = "姓名：";
                    ws.Cell(2, 4).Value = "'" + condition.ConcatName;
                    ws.Cell(2, 5).Value = "電話：";
                    ws.Cell(2, 6).Value = "'" + condition.ConcatTelephone;
                    ws.Cell(2, 7).Value = "電子信箱：";
                    ws.Cell(2, 8).Value = "'" + condition.ConcatEmail;
                    break;
                case UnitType.Store:
                    ws.Cell(2, 3).Value = "店名：";
                    ws.Cell(2, 4).Value = "'" + condition.ConcatStoreName;
                    ws.Cell(2, 5).Value = "店號：";
                    ws.Cell(2, 6).Value = "'" + condition.ConcatStoreNo;
                    ws.Cell(2, 7).Value = "組織：";
                    ws.Cell(2, 8).Value = condition.ConcatNode == null || condition.ConcatNode.Count() == 0 ? "" : string.Join("/", condition.ConcatNode.Select(x => x.Name));
                    break;
                case UnitType.Organization:
                    ws.Cell(2, 3).Value = "單位名稱：";
                    ws.Cell(2, 4).Value = "'" + condition.ConcatNodeName;
                    ws.Cell(2, 5).Value = "組織：";
                    ws.Cell(2, 6).Value = condition.ConcatNode == null || condition.ConcatNode.Count() == 0 ? "" : string.Join("/", condition.ConcatNode.Select(x => x.Name));
                    break;
                case null:
                    break;
            }
            ws.Cell(3, 1).Value = "被反應者類型：";
            ws.Cell(3, 2).Value = condition.CaseComplainedUnitType == null ? "" : "'" + ((UnitType)condition.CaseComplainedUnitType).GetDescription();
            switch (condition.CaseComplainedUnitType)
            {
                case UnitType.Store:
                    ws.Cell(3, 3).Value = "店名：";
                    ws.Cell(3, 4).Value = "'" + condition.CaseComplainedStoreName;
                    ws.Cell(3, 5).Value = "店號：";
                    ws.Cell(3, 6).Value = "'" + condition.CaseComplainedStoreNo;
                    ws.Cell(3, 7).Value = "組織：";
                    ws.Cell(3, 8).Value = condition.ComplainedNode == null || condition.ComplainedNode.Count() == 0 ? "" : string.Join("/", condition.ComplainedNode.Select(x => x.Name));
                    break;
                case UnitType.Organization:
                    ws.Cell(3, 3).Value = "單位名稱：";
                    ws.Cell(3, 4).Value = "'" + condition.CaseComplainedNodeName;
                    ws.Cell(3, 5).Value = "組織：";
                    ws.Cell(3, 6).Value = condition.ComplainedNode == null || condition.ComplainedNode.Count() == 0 ? "" : string.Join("/", condition.ComplainedNode.Select(x => x.Name));
                    break;
                case null:
                    break;
            }
            ws.Cell(4, 1).Value = "案件編號：";
            ws.Cell(4, 2).Value = "'" + condition.CaseID;
            ws.Cell(4, 3).Value = "案件內容：";
            ws.Cell(4, 4).Value = "'" + condition.CaseContent;
            ws.Cell(4, 5).Value = "結案內容：";
            ws.Cell(4, 6).Value = "'" + condition.FinishContent;

            if (condition.ClassificationID == null || !classList_Search.Any())
            {
                ws.Cell(5, 1).Value = "問題分類1：";
                ws.Cell(5, 2).Value = "";
                ws.Cell(5, 3).Value = "問題分類2：";
                ws.Cell(5, 4).Value = "";
                ws.Cell(5, 5).Value = "問題分類3：";
                ws.Cell(5, 6).Value = "";
                ws.Cell(5, 7).Value = "案件標籤：";
                ws.Cell(5, 8).Value = condition.CaseTagList != null ? "'" + string.Join("/", condition.CaseTagList) : "";
            }
            else
            {
                string[] parentList = classList_Search.Where(x => x.ID == condition.ClassificationID).First().ParentNamePathByArray;
                int Column = 1;
                for (int i = 0; i < parentList.Count(); i++)
                {
                    ws.Cell(5, Column).Value = "問題分類" + (i + 1) + "：";
                    Column = Column + 1;
                    ws.Cell(5, Column).Value = "'" + parentList[i];
                    Column = Column + 1;
                }
                ws.Cell(5, Column).Value = "案件標籤：";
                Column = Column + 1;
                ws.Cell(5, Column).Value = condition.CaseTagList != null ? "'" + string.Join("/", condition.CaseTagList) : "";
            }
            ws.Cell(6, 1).Value = "案件等級：";
            ws.Cell(6, 2).Value = "'" + condition.CaseWarningName;
            ws.Cell(6, 3).Value = "案件狀態：";
            ws.Cell(6, 4).Value = condition.CaseType == null ? "" : "'" + ((CaseType)condition.CaseType).GetDescription();
            ws.Cell(6, 5).Value = "結案處置：";
            ws.Cell(6, 6).Value = condition.ReasonName != null ? "'" + string.Join("/", condition.ReasonName) : ""; ;

            int Field = 8;

            ws.Cell(Field, 1).Value = "企業別";
            ws.Cell(Field, 2).Value = "案件來源";
            ws.Cell(Field, 3).Value = "來源時間";
            ws.Cell(Field, 4).Value = "立案時間";
            ws.Cell(Field, 5).Value = "案件編號";
            ws.Cell(Field, 6).Value = "案件等級";
            ws.Cell(Field, 7).Value = "案件狀態";
            ws.Cell(Field, 8).Value = "預立案";
            ws.Cell(Field, 9).Value = "關注案件";

            for (int i = 1; i <= classificationCount; i++)
            {
                ws.Cell(Field, 9 + i).Value = "問題分類" + i.ToString();
            }
            int column = 9 + classificationCount;

            ws.Cell(Field, column + 1).Value = "反應者類型";
            ws.Cell(Field, column + 2).Value = "反應者";
            ws.Cell(Field, column + 3).Value = "手機";
            ws.Cell(Field, column + 4).Value = "電話1";
            ws.Cell(Field, column + 5).Value = "電話2";
            ws.Cell(Field, column + 6).Value = "電子信箱";
            ws.Cell(Field, column + 7).Value = "地址";
            ws.Cell(Field, column + 8).Value = "門市";
            ws.Cell(Field, column + 9).Value = "姓名";
            ws.Cell(Field, column + 10).Value = "電話";
            ws.Cell(Field, column + 11).Value = "組織單位";
            ws.Cell(Field, column + 12).Value = "姓名";
            ws.Cell(Field, column + 13).Value = "電話";
            ws.Cell(Field, column + 14).Value = "被反應者類型";
            ws.Cell(Field, column + 15).Value = "門市";
            ws.Cell(Field, column + 16).Value = "門市負責人";
            ws.Cell(Field, column + 17).Value = "門市區經理";
            ws.Cell(Field, column + 18).Value = "門市電話";
            ws.Cell(Field, column + 19).Value = "組織單位";
            ws.Cell(Field, column + 20).Value = "姓名";
            ws.Cell(Field, column + 21).Value = "電話";
            ws.Cell(Field, column + 22).Value = "案件內容";
            ws.Cell(Field, column + 23).Value = "商品名稱";
            ws.Cell(Field, column + 24).Value = "國際條碼";
            ws.Cell(Field, column + 25).Value = "批號";
            ws.Cell(Field, column + 26).Value = "卡號";
            ws.Cell(Field, column + 27).Value = "型號";
            ws.Cell(Field, column + 28).Value = "名稱";
            ws.Cell(Field, column + 29).Value = "購買日期";
            ws.Cell(Field, column + 30).Value = "結案內容";
            ws.Cell(Field, column + 31).Value = "結案時間";

            int column2 = titleList.Count;
            if (titleList.Any())
            {
                int index = 0;
                foreach (var titleitem in titleList)
                {
                    ws.Cell(Field, column + index + 32).Value = $"結案處置({titleitem})";
                    index++;
                }
                column2 = column2 - 1;
            }
            else
            {
                ws.Cell(Field, column + 32).Value = "結案處置";
            }

            ws.Cell(Field, column + column2 + 33).Value = "負責人";
            ws.Cell(Field, column + column2 + 34).Value = "案件標籤";

            #endregion

            int row = 9;
            foreach (var item in data)
            {
                ws.Cell(row, 1).Value = item.NodeName;
                ws.Cell(row, 2).Value = item.CaseSourceType;
                ws.Cell(row, 3).Value = item.IncomingDateTime;
                ws.Cell(row, 4).Value = item.CreateTime;
                ws.Cell(row, 5).Value = item.CaseID;
                ws.Cell(row, 6).Value = item.CaseWarningName;
                ws.Cell(row, 7).Value = item.CaseType;
                ws.Cell(row, 8).Value = item.IsPrevention;
                ws.Cell(row, 9).Value = item.IsAttension;
                //分類
                for (int i = 1; i <= classificationCount; i++)
                {
                    if (classList.Where(x => x.ID == item.ClassificationID).Count() == 0)
                        continue;
                    var listclass = classList.Where(x => x.ID == item.ClassificationID).FirstOrDefault();
                    ws.Cell(row, 9 + i).Value = "'" + listclass.ParentNamePathByArray[i - 1];

                    if (listclass.ParentNamePathByArray.Count() < i + 1)
                    {
                        break;
                    }
                }

                ws.Cell(row, column + 1).Value = item.ConcatUnitType;
                ws.Cell(row, column + 2).Value = item.ConcatCustomerName;
                ws.Cell(row, column + 3).Value = item.ConcatCustomerMobile;
                ws.Cell(row, column + 4).Value = item.ConcatCustomerTelephone1;
                ws.Cell(row, column + 5).Value = item.ConcatCustomerTelephone2;
                ws.Cell(row, column + 6).Value = item.ConcatCustomerMail;
                ws.Cell(row, column + 7).Value = item.ConcatCustomerAddress;
                ws.Cell(row, column + 8).Value = item.ConcatStore;
                ws.Cell(row, column + 9).Value = item.ConcatStoreName;
                ws.Cell(row, column + 10).Value = item.ConcatStoreTelephone;
                ws.Cell(row, column + 11).Value = item.ConcatOrganization;
                ws.Cell(row, column + 12).Value = item.ConcatOrganizationName;
                ws.Cell(row, column + 13).Value = item.ConcatOrganizationTelephone;
                ws.Cell(row, column + 14).Value = item.ComplainedUnitType;
                ws.Cell(row, column + 15).Value = item.ComplainedStore;
                ws.Cell(row, column + 16).Value = item.ComplainedStoreApplyUserName;
                ws.Cell(row, column + 17).Value = item.ComplainedStoreSupervisorUserName;
                ws.Cell(row, column + 18).Value = item.ComplainedStoreTelephone;
                ws.Cell(row, column + 19).Value = item.ComplainedOrganization;
                ws.Cell(row, column + 20).Value = item.ComplainedOrganizationName;
                ws.Cell(row, column + 21).Value = item.ComplainedOrganizationTelephone;
                ws.Cell(row, column + 22).Value = item.CaseContent;
                ws.Cell(row, column + 22).Style.Alignment.WrapText = true;

                ws.Cell(row, column + 23).Value = "'" + item.OtherCommodityName;
                ws.Cell(row, column + 23).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 24).Value = "'" + item.OtherInternationalBarcode;
                ws.Cell(row, column + 24).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 25).Value = "'" + item.OtherBatchNo;
                ws.Cell(row, column + 25).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 26).Value = "'" + item.OtherCardNumber;
                ws.Cell(row, column + 26).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 27).Value = "'" + item.OtherProductModel;
                ws.Cell(row, column + 27).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 28).Value = "'" + item.OtherProductName;
                ws.Cell(row, column + 28).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 29).Value = "'" + item.OtherPurchaseDay;
                ws.Cell(row, column + 29).Style.Alignment.WrapText = true;

                ws.Cell(row, column + 30).Value = item.FinishContent;
                ws.Cell(row, column + 30).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 31).Value = item.FinishDateTime;

                if (titleList.Any())
                {
                    int index = 0;
                    foreach (var titleitem in titleList)
                    {
                        if (item.ReasonList.ContainsKey(titleitem))
                            ws.Cell(row, column + index + 32).Value = item.ReasonList[$"{titleitem}"];
                        else
                            ws.Cell(row, column + index + 32).Value = "";
                        index++;
                    }
                }
                else
                {
                    ws.Cell(row, column + 32).Value = item.ReasonName;
                }

                ws.Cell(row, column + column2 + 33).Value = item.ApplyUserName;
                ws.Cell(row, column + column2 + 34).Value = item.CaseTag;


                row++;
            }
            //左右至中
            ws.Range(Field, 1, row - 1, column + column2 + 34).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            //水平 至中
            ws.Range(Field, 1, row - 1, column + column2 + 34).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


            //格線
            var rngTable1 = ws.Range(Field, 1, row - 1, column + column2 + 34);
            rngTable1.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            rngTable1.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            //自動對應欄位寬度
            ws.Columns().AdjustToContents();

            //設定案件內容與結案內容固定欄寬
            ws.Column(column + 22).Width = 100;
            ws.Column(column + 30).Width = 100;
            ws.Range(Field + 1, column + 22, row - 1, column + 22).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Range(Field + 1, column + 30, row - 1, column + 30).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

            //設定結案處置固定欄寬
            int indexw = 0;
            do
            {
                ws.Column(column + indexw + 32).Width = 25;
                indexw++;
            }
            while (indexw < titleList.Count);

            ws.Rows().AdjustToContents();
            //自動調整列高後，需.ClearHeight才可依內容高度顯示
            //for (int i = 9; i <= row; i++)
            //{
            //    ws.Row(i).ClearHeight();
            //}

            //20200911 固定列高
            for (int i = 9; i < row; i++)
            {
                ws.Row(i).Height = 95;
            }


            var result = ReportUtility.ConvertBookToByte(book, "");
            #endregion
            return result;
        }

        /// <summary>
        /// 案件查詢 - 轉換Excel Model(總部、門市)
        /// </summary>
        /// <returns></returns>
        private List<ExcelCaseHSList> GetCaseHSList(List<SP_GetCaseList> list)
        {
            List<ExcelCaseHSList> excelList = new List<ExcelCaseHSList>();

            foreach (var data in list)
            {
                var concat = data.CaseConcatUsersList.Count() == 0 ? null : data.CaseConcatUsersList.First();
                var complained = data.CaseComplainedUsersList.Where(x => x.CaseComplainedUserType == CaseComplainedUserType.Responsibility).Count() == 0
                    ? null : data.CaseComplainedUsersList.Where(x => x.CaseComplainedUserType == CaseComplainedUserType.Responsibility).First();

                var callCenter = new ExcelCaseHSList()
                {
                    NodeName = "'" + data.NodeName,
                    CaseSourceType = "'" + data.SourceType.GetDescription(),
                    IncomingDateTime = data.IncomingDateTime.HasValue ? "'" + data.IncomingDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                    CreateTime = "'" + data.CreateDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    CaseID = "'" + data.CaseID,
                    CaseWarningName = "'" + data.CaseWarningName,
                    CaseType = "'" + data.CaseType.GetDescription(),
                    IsPrevention = data.IsPrevention ? "是" : "否",
                    IsAttension = data.IsAttension ? "是" : "否",
                    ClassificationID = data.ClassificationID,
                    //反應者
                    ConcatUnitType = concat == null ? "" : "'" + concat.UnitType.GetDescription(),
                    //消費者
                    ConcatCustomerName = concat == null ? "" : concat.UnitType == UnitType.Customer ? "'" + concat.UserName + concat.Gender.GetDescription() : "",
                    ConcatCustomerMobile = concat == null ? "" : concat.UnitType == UnitType.Customer ? "'" + concat.Mobile : "",
                    ConcatCustomerTelephone1 = concat == null ? "" : concat.UnitType == UnitType.Customer ? "'" + concat.Telephone : "",
                    ConcatCustomerTelephone2 = concat == null ? "" : concat.UnitType == UnitType.Customer ? "'" + concat.TelephoneBak : "",
                    ConcatCustomerMail = concat == null ? "" : concat.UnitType == UnitType.Customer ? "'" + concat.Email : "",
                    ConcatCustomerAddress = concat == null ? "" : concat.UnitType == UnitType.Customer ? "'" + concat.Address : "",
                    //門市
                    ConcatStore = concat == null ? "" : concat.UnitType == UnitType.Store ? "'" + concat.NodeName + concat.StoreNo : "",
                    ConcatStoreName = concat == null ? "" : concat.UnitType == UnitType.Store ? "'" + concat.UserName : "",
                    ConcatStoreTelephone = concat == null ? "" : concat.UnitType == UnitType.Store ? "'" + concat.Telephone : "",
                    //組織
                    ConcatOrganization = concat == null ? "" : concat.UnitType == UnitType.Organization ? "'" + concat.NodeName : "",
                    ConcatOrganizationName = concat == null ? "" : concat.UnitType == UnitType.Organization ? "'" + concat.UserName : "",
                    ConcatOrganizationTelephone = concat == null ? "" : concat.UnitType == UnitType.Organization ? "'" + concat.Telephone : "",

                    //被反應者
                    ComplainedUnitType = complained == null ? "" : "'" + complained.UnitType.GetDescription(),

                    //門市
                    ComplainedStore = complained != null ? (complained.UnitType == UnitType.Store ? "'" + complained.NodeName + complained.StoreNo : "") : "",
                    ComplainedStoreApplyUserName = complained != null ? (complained.UnitType == UnitType.Store ? "'" + complained.OwnerUserName : "") : "",
                    ComplainedStoreSupervisorUserName = complained != null ? (complained.UnitType == UnitType.Store ? "'" + complained.SupervisorUserName : "") : "",
                    ComplainedStoreTelephone = complained != null ? (complained.UnitType == UnitType.Store ? "'" + complained.Telephone : "") : "",

                    //組織
                    ComplainedOrganization = complained != null ? (complained.UnitType == UnitType.Organization ? "'" + complained.NodeName : "") : "",
                    ComplainedOrganizationName = complained != null ? (complained.UnitType == UnitType.Organization ? "'" + complained.OwnerUserName : "") : "",
                    ComplainedOrganizationTelephone = complained != null ? (complained.UnitType == UnitType.Organization ? "'" + complained.OwnerUserPhone : "") : "",
                    //案件內容
                    CaseContent = "'" + data.CaseContent,
                    FinishContent = "'" + data.FinishContent,
                    FinishDateTime = data.FinishDateTime.HasValue ? "'" + data.FinishDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                    //ReasonName = data.CaseFinishReasonDatas.Any() ? "'" + string.Join("/", data.CaseFinishReasonDatas.Select(y => y.Text)) : "",
                    ApplyUserName = "'" + data.ApplyUserName,
                    CaseTag = data.CaseTagList.Any() ? "'" + string.Join("/", data.CaseTagList.Select(y => y.Name)) : "",
                    JContent = data.JContent,
                };

                //ReasonName依所屬Title分類 20200828
                IDictionary<string, string> dist = new Dictionary<string, string>();
                var titleList = data.CaseFinishReasonDatas.Select(x => x.CaseFinishReasonClassification.Title).Distinct().ToList();
                if (titleList.Any())
                {
                    titleList.ForEach(x =>
                    {
                        dist.Add($"{x}", "'" + string.Join("/", data.CaseFinishReasonDatas.Where(y => y.CaseFinishReasonClassification.Title == x).Select(y => y.Text)));
                    });
                }
                callCenter.ReasonList = dist;

                //其他資訊
                var excelParser = _ExcelParser.TryGetService(data.NodeKey);
                callCenter = excelParser != null ? _ExcelParser[data.NodeKey].CaseSearchHSItemParsing(data.CaseItemList, callCenter) : callCenter;

                excelList.Add(callCenter);
            }
            //資料匯出，排序案件編號
            excelList = excelList.OrderBy(x => x.CaseID).ToList();

            return excelList;
        }

        /// <summary>
        /// 案件轉派查詢(客服用)
        /// </summary>
        /// <returns></returns>
        public Byte[] CreateCaseAssignmentCustomerExcel(List<ExcelCaseAssignmentList> data, CaseAssignmentCallCenterCondition condition)
        {

            //查詢條件-分類清單
            var classList_Search = new List<QuestionClassification>();
            if (condition.ClassificationID != null)
            {
                var con = new MSSQLCondition<VW_QUESTION_CLASSIFICATION_NESTED>();
                con.And(x => x.ID == (int)condition.ClassificationID);
                classList_Search = _IMasterAggregate.VWQuestionClassification_QuestionClassification_.GetList(con).ToList();
            }

            //查詢結果-分類清單
            var con2 = new MSSQLCondition<VW_QUESTION_CLASSIFICATION_NESTED>();
            var dataClassList = data.Select(x => x.ClassificationID).ToArray();
            con2.And(x => dataClassList.Contains(x.ID));
            var classList = _IMasterAggregate.VWQuestionClassification_QuestionClassification_.GetList(con2).ToList();
            int classificationCount = classList.Count == 0 ? 0 : classList.Max(x => x.Level);

            //查詢結果-結案處置
            var con4 = new MSSQLCondition<CASE_FINISH_REASON_CLASSIFICATION>();
            var nodeList = classList.Select(x => x.NodeID).Distinct().ToList();//無nodeid 資料,故使用分類查詢結果為來源
            con4.And(x => nodeList.Contains(x.NODE_ID));
            var titleList = _IMasterAggregate.CaseFinishReasonClassification_T1_T2_.GetList(con4)?.Select(x => x.Title).ToList();


            // 反應類別代碼轉換
            var con3 = new MSSQLCondition<HEADQUARTERS_NODE>(
                            x => x.NODE_ID == condition.NodeID &&
                            x.ORGANIZATION_TYPE == (byte)OrganizationType.HeaderQuarter);

            var nodeKey = _OrganizationAggregate.HeaderQuarterNode_T1_.GetOfSpecific(con3, x => x.NODE_KEY);

            var invoiceTypeList = new List<SelectItem>();
            if (DataStorage.CaseAssignmentComplaintInvoiceTypeDict.TryGetValue(nodeKey ?? string.Empty, out var CaseComplaintInvoiceTypes))
            {
                invoiceTypeList = CaseComplaintInvoiceTypes;
            }


            #region 產出Excel
            XLWorkbook book = new XLWorkbook();

            var ws = book.AddWorksheet("工作表1");

            #region 開頭
            ws.Cell(1, 1).Value = "企業別：";
            ws.Cell(1, 2).Value = "'" + condition.NodeName;
            ws.Cell(1, 3).Value = "轉派對象：";
            ws.Cell(1, 4).Value = condition.AssignmentUser == null ? "" : "'" + string.Join("/", condition.AssignmentUser);
            ws.Cell(1, 5).Value = "通知時間：";
            ws.Cell(1, 6).Value = "'" + condition.NoticeDateTimeRange;
            ws.Cell(1, 7).Value = "立案時間：";
            ws.Cell(1, 8).Value = "'" + condition.CreateTimeRange;


            ws.Cell(2, 1).Value = "反應者類型：";
            ws.Cell(2, 2).Value = condition.CaseConcatUnitType == null ? "" : "'" + ((UnitType)condition.CaseConcatUnitType).GetDescription();

            switch (condition.CaseConcatUnitType)
            {
                case UnitType.Customer:
                    ws.Cell(2, 3).Value = "姓名：";
                    ws.Cell(2, 4).Value = "'" + condition.ConcatName;
                    ws.Cell(2, 5).Value = "電話：";
                    ws.Cell(2, 6).Value = "'" + condition.ConcatTelephone;
                    ws.Cell(2, 7).Value = "電子信箱：";
                    ws.Cell(2, 8).Value = "'" + condition.ConcatEmail;
                    break;
                case UnitType.Store:
                    ws.Cell(2, 3).Value = "店名：";
                    ws.Cell(2, 4).Value = "'" + condition.ConcatStoreName;
                    ws.Cell(2, 5).Value = "店號：";
                    ws.Cell(2, 6).Value = "'" + condition.ConcatStoreNo;
                    ws.Cell(2, 7).Value = "組織：";
                    ws.Cell(2, 8).Value = condition.ConcatNode == null || condition.ConcatNode.Count() == 0 ? "" : string.Join("/", condition.ConcatNode.Select(x => x.Name));
                    break;
                case UnitType.Organization:
                    ws.Cell(2, 3).Value = "單位名稱：";
                    ws.Cell(2, 4).Value = "'" + condition.ConcatNodeName;
                    ws.Cell(2, 5).Value = "組織：";
                    ws.Cell(2, 6).Value = condition.ConcatNode == null || condition.ConcatNode.Count() == 0 ? "" : string.Join("/", condition.ConcatNode.Select(x => x.Name));
                    break;
                case null:
                    break;
            }
            ws.Cell(3, 1).Value = "案件編號：";
            ws.Cell(3, 2).Value = "'" + condition.CaseID;
            ws.Cell(3, 3).Value = "案件內容：";
            ws.Cell(3, 4).Value = "'" + condition.CaseContent;
            ws.Cell(3, 5).Value = "通知內容：";
            ws.Cell(3, 6).Value = "'" + condition.NoticeContent;

            ws.Cell(4, 1).Value = "被反應者類型：";
            ws.Cell(4, 2).Value = condition.CaseComplainedUnitType == null ? "" : "'" + ((UnitType)condition.CaseComplainedUnitType).GetDescription();
            switch (condition.CaseComplainedUnitType)
            {
                case UnitType.Store:
                    ws.Cell(4, 3).Value = "店名：";
                    ws.Cell(4, 4).Value = "'" + condition.CaseComplainedStoreName;
                    ws.Cell(4, 5).Value = "店號：";
                    ws.Cell(4, 6).Value = "'" + condition.CaseComplainedStoreNo;
                    ws.Cell(4, 7).Value = "組織：";
                    ws.Cell(4, 8).Value = condition.ComplainedNode == null || condition.ComplainedNode.Count() == 0 ? "" : string.Join("/", condition.ComplainedNode.Select(x => x.Name));
                    break;
                case UnitType.Organization:
                    ws.Cell(4, 3).Value = "單位名稱：";
                    ws.Cell(4, 4).Value = "'" + condition.CaseComplainedNodeName;
                    ws.Cell(4, 5).Value = "組織：";
                    ws.Cell(4, 6).Value = condition.ComplainedNode == null || condition.ComplainedNode.Count() == 0 ? "" : string.Join("/", condition.ComplainedNode.Select(x => x.Name));
                    break;
                case null:
                    break;
            }


            //歷程狀態string 待CaseAssignmentModeType修改後修正
            string assignmentState = "";
            if (condition.AssignmentType != null && condition.Type != null)
            {
                switch ((int)condition.AssignmentType)
                {
                    case (int)CaseAssignmentProcessType.Notice:
                        assignmentState = ((CaseAssignmentComplaintNoticeType)condition.Type).GetDescription();
                        break;
                    case (int)CaseAssignmentProcessType.Invoice:
                        assignmentState = ((CaseAssignmentComplaintInvoiceType)condition.Type).GetDescription();
                        break;
                    case (int)CaseAssignmentProcessType.Assignment:
                        assignmentState = ((CaseAssignmentType)condition.Type).GetDescription();
                        break;
                    case (int)CaseAssignmentProcessType.Communication:
                        break;
                    default:
                        break;

                }
            }



            ws.Cell(5, 1).Value = "歷程模式：";
            ws.Cell(5, 2).Value = condition.AssignmentType == null ? "" : "'" + condition.AssignmentType.GetDescription();
            ws.Cell(5, 3).Value = "歷程狀態：";
            ws.Cell(5, 4).Value = "'" + assignmentState;
            ws.Cell(5, 5).Value = "反應類別：";
            ws.Cell(5, 6).Value = !string.IsNullOrEmpty(condition.InvoiceType) ? "'" + invoiceTypeList.Where(x => x.id == condition.InvoiceType).FirstOrDefault()?.text ?? "" : "";
            ws.Cell(5, 7).Value = "反應單號：";
            ws.Cell(5, 8).Value = "'" + condition.InvoiceID;

            if (condition.ClassificationID == null || !classList_Search.Any())
            {
                ws.Cell(6, 1).Value = "問題分類1：";
                ws.Cell(6, 2).Value = "";
                ws.Cell(6, 3).Value = "問題分類2：";
                ws.Cell(6, 4).Value = "";
                ws.Cell(6, 5).Value = "問題分類3：";
                ws.Cell(6, 6).Value = "";
            }
            else
            {
                string[] parentList = classList_Search.Where(x => x.ID == condition.ClassificationID).First().ParentNamePathByArray;
                int Column = 1;
                for (int i = 0; i < parentList.Count(); i++)
                {
                    ws.Cell(6, Column).Value = "問題分類" + (i + 1) + "：";
                    Column = Column + 1;
                    ws.Cell(6, Column).Value = "'" + parentList[i];
                    Column = Column + 1;
                }
            }
            int Field = 8;

            ws.Cell(Field, 1).Value = "企業別";
            ws.Cell(Field, 2).Value = "案件來源";
            ws.Cell(Field, 3).Value = "來源時間";
            ws.Cell(Field, 4).Value = "立案時間";
            ws.Cell(Field, 5).Value = "案件編號";
            ws.Cell(Field, 6).Value = "序號(反應單號)";
            ws.Cell(Field, 7).Value = "期望期限";
            ws.Cell(Field, 8).Value = "案件等級";
            ws.Cell(Field, 9).Value = "案件狀態";
            ws.Cell(Field, 10).Value = "預立案";
            ws.Cell(Field, 11).Value = "關注案件";

            for (int i = 1; i <= classificationCount; i++)
            {
                ws.Cell(Field, 11 + i).Value = "問題分類" + i.ToString();
            }
            int column = 11 + classificationCount;

            ws.Cell(Field, column + 1).Value = "反應者類型";
            ws.Cell(Field, column + 2).Value = "反應者";
            ws.Cell(Field, column + 3).Value = "手機";
            ws.Cell(Field, column + 4).Value = "電話1";
            ws.Cell(Field, column + 5).Value = "電話2";
            ws.Cell(Field, column + 6).Value = "電子信箱";
            ws.Cell(Field, column + 7).Value = "地址";
            ws.Cell(Field, column + 8).Value = "門市";
            ws.Cell(Field, column + 9).Value = "姓名";
            ws.Cell(Field, column + 10).Value = "電話";
            ws.Cell(Field, column + 11).Value = "組織單位";
            ws.Cell(Field, column + 12).Value = "姓名";
            ws.Cell(Field, column + 13).Value = "電話";
            ws.Cell(Field, column + 14).Value = "被反應者類型";
            ws.Cell(Field, column + 15).Value = "門市";
            ws.Cell(Field, column + 16).Value = "門市負責人";
            ws.Cell(Field, column + 17).Value = "門市區經理";
            ws.Cell(Field, column + 18).Value = "門市電話";
            ws.Cell(Field, column + 19).Value = "組織單位";
            ws.Cell(Field, column + 20).Value = "姓名";
            ws.Cell(Field, column + 21).Value = "電話";
            ws.Cell(Field, column + 22).Value = "案件內容";
            ws.Cell(Field, column + 23).Value = "案件期限";
            ws.Cell(Field, column + 24).Value = "商品名稱";
            ws.Cell(Field, column + 25).Value = "國際條碼";
            ws.Cell(Field, column + 26).Value = "批號";
            ws.Cell(Field, column + 27).Value = "卡號";
            ws.Cell(Field, column + 28).Value = "型號";
            ws.Cell(Field, column + 29).Value = "名稱";
            ws.Cell(Field, column + 30).Value = "購買日期";
            ws.Cell(Field, column + 31).Value = "歷程模式";
            ws.Cell(Field, column + 32).Value = "歷程狀態";
            ws.Cell(Field, column + 33).Value = "反應類別";
            ws.Cell(Field, column + 34).Value = "轉派對象";
            ws.Cell(Field, column + 35).Value = "通知內容";
            ws.Cell(Field, column + 36).Value = "通知時間";
            ws.Cell(Field, column + 37).Value = "回覆內容";
            ws.Cell(Field, column + 38).Value = "銷案內容";
            ws.Cell(Field, column + 39).Value = "銷案時間";
            ws.Cell(Field, column + 40).Value = "銷案人";
            ws.Cell(Field, column + 41).Value = "結案內容";
            ws.Cell(Field, column + 42).Value = "結案時間";

            int column2 = titleList.Count;
            if (titleList.Any())
            {
                int index = 0;
                foreach (var titleitem in titleList)
                {
                    ws.Cell(Field, column + index + 43).Value = $"結案處置({titleitem})";
                    index++;
                }
                column2 = column2 - 1;
            }
            else
            {
                ws.Cell(Field, column + 43).Value = "結案處置";
            }

            ws.Cell(Field, column + column2 + 44).Value = "負責人";
            ws.Cell(Field, column + column2 + 45).Value = "案件標籤";

            #endregion

            int row = 9;
            foreach (var item in data)
            {
                ws.Cell(row, 1).Value = "'" + item.NodeName;
                ws.Cell(row, 2).Value = "'" + item.SourceType;
                ws.Cell(row, 3).Value = "'" + item.IncomingDateTime;
                ws.Cell(row, 4).Value = "'" + item.CreateTime;
                ws.Cell(row, 5).Value = "'" + item.CaseID;
                ws.Cell(row, 6).Value = "'" + item.SN;
                ws.Cell(row, 7).Value = "'" + item.ExpectDateTime;
                ws.Cell(row, 8).Value = "'" + item.CaseWarningName;
                ws.Cell(row, 9).Value = "'" + item.CaseType;
                ws.Cell(row, 10).Value = "'" + item.IsPrevention;
                ws.Cell(row, 11).Value = "'" + item.IsAttension;
                //分類
                for (int i = 1; i <= classificationCount; i++)
                {
                    if (classList.Where(x => x.ID == item.ClassificationID).Count() == 0)
                        continue;
                    var list = classList.Where(x => x.ID == item.ClassificationID).FirstOrDefault();
                    ws.Cell(row, 11 + i).Value = "'" + list.ParentNamePathByArray[i - 1];

                    if (list.ParentNamePathByArray.Count() < i + 1)
                    {
                        break;
                    }
                }
                ws.Cell(row, column + 1).Value = "'" + item.ConcatUnitType;
                ws.Cell(row, column + 2).Value = "'" + item.ConcatCustomerName;
                ws.Cell(row, column + 3).Value = "'" + item.ConcatCustomerMobile;
                ws.Cell(row, column + 4).Value = "'" + item.ConcatCustomerTelephone1;
                ws.Cell(row, column + 5).Value = "'" + item.ConcatCustomerTelephone2;
                ws.Cell(row, column + 6).Value = "'" + item.ConcatCustomerMail;
                ws.Cell(row, column + 7).Value = "'" + item.ConcatCustomerAddress;
                ws.Cell(row, column + 8).Value = "'" + item.ConcatStore;
                ws.Cell(row, column + 9).Value = "'" + item.ConcatStoreName;
                ws.Cell(row, column + 10).Value = "'" + item.ConcatStoreTelephone;
                ws.Cell(row, column + 11).Value = "'" + item.ConcatOrganization;
                ws.Cell(row, column + 12).Value = "'" + item.ConcatOrganizationName;
                ws.Cell(row, column + 13).Value = "'" + item.ConcatOrganizationTelephone;
                ws.Cell(row, column + 14).Value = "'" + item.ComplainedUnitType;
                ws.Cell(row, column + 15).Value = "'" + item.ComplainedStore;
                ws.Cell(row, column + 16).Value = "'" + item.ComplainedStoreApplyUserName;
                ws.Cell(row, column + 17).Value = "'" + item.ComplainedStoreSupervisorUserName;
                ws.Cell(row, column + 18).Value = "'" + item.ComplainedStoreTelephone;
                ws.Cell(row, column + 19).Value = "'" + item.ComplainedOrganizationNodeName;
                ws.Cell(row, column + 20).Value = "'" + item.ComplainedOrganizationName;
                ws.Cell(row, column + 21).Value = "'" + item.ComplainedOrganizationTelephone;
                ws.Cell(row, column + 22).Value = "'" + item.CaseContent;
                ws.Cell(row, column + 22).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 23).Value = "'" + item.PromiseDateTime;
                ws.Cell(row, column + 24).Value = "'" + item.OtherCommodityName;
                ws.Cell(row, column + 24).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 25).Value = "'" + item.OtherInternationalBarcode;
                ws.Cell(row, column + 25).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 26).Value = "'" + item.OtherBatchNo;
                ws.Cell(row, column + 26).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 27).Value = "'" + item.OtherCardNumber;
                ws.Cell(row, column + 27).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 28).Value = "'" + item.OtherProductModel;
                ws.Cell(row, column + 28).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 29).Value = "'" + item.OtherProductName;
                ws.Cell(row, column + 29).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 30).Value = "'" + item.OtherPurchaseDay;
                ws.Cell(row, column + 30).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 31).Value = "'" + item.ModeType;
                ws.Cell(row, column + 32).Value = "'" + item.AssignmentType;
                ws.Cell(row, column + 33).Value = !string.IsNullOrEmpty(item.InvoiceType) ? "'" + invoiceTypeList.Where(x => x.id == item.InvoiceType).FirstOrDefault()?.text ?? "" : "";
                ws.Cell(row, column + 34).Value = item.AssignmentUser == null ? "" : "'" + string.Join("/", item.AssignmentUser);
                ws.Cell(row, column + 34).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 35).Value = "'" + item.NoticeContent;
                ws.Cell(row, column + 35).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 36).Value = "'" + item.NoticeTime;
                ws.Cell(row, column + 37).Value = "'" + item.RetryContent;
                ws.Cell(row, column + 37).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 38).Value = "'" + item.CloseCaseContent;
                ws.Cell(row, column + 38).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 39).Value = "'" + item.CloseCaseTime;
                ws.Cell(row, column + 40).Value = "'" + item.CloseCaseUser;
                ws.Cell(row, column + 41).Value = "'" + item.FinishContent;
                ws.Cell(row, column + 41).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 42).Value = "'" + item.FinishDateTime;

                if (titleList.Any())
                {
                    int index = 0;
                    foreach (var titleitem in titleList)
                    {
                        if (item.ReasonList.ContainsKey(titleitem))
                            ws.Cell(row, column + index + 43).Value = item.ReasonList[$"{titleitem}"];
                        else
                            ws.Cell(row, column + index + 43).Value = "";
                        index++;
                    }
                }
                else
                {
                    ws.Cell(row, column + 43).Value = "'" + item.ReasonName;
                }

                ws.Cell(row, column + column2 + 44).Value = "'" + item.ApplyUserName;
                ws.Cell(row, column + column2 + 45).Value = "'" + item.CaseTagName;

                row++;
            }
            //左右至中
            ws.Range(Field, 1, row - 1, column + column2 + 45).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            //水平 至中
            ws.Range(Field, 1, row - 1, column + column2 + 45).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


            //格線
            var rngTable1 = ws.Range(Field, 1, row - 1, column + column2 + 45);
            rngTable1.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            rngTable1.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            //自動對應欄位寬度

            ws.Columns().AdjustToContents();

            //設定案件內容與結案內容固定欄寬與內容靠左
            ws.Column(column + 22).Width = 100;
            ws.Column(column + 34).Width = 20;
            ws.Column(column + 35).Width = 100;
            ws.Column(column + 37).Width = 100;
            ws.Column(column + 38).Width = 100;
            ws.Column(column + 41).Width = 100;
            ws.Range(Field + 1, column + 22, row - 1, column + 22).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Range(Field + 1, column + 35, row - 1, column + 35).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Range(Field + 1, column + 37, row - 1, column + 37).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Range(Field + 1, column + 38, row - 1, column + 38).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Range(Field + 1, column + 41, row - 1, column + 41).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;


            //設定結案處置固定欄寬
            int indexw = 0;
            do
            {
                ws.Column(column + indexw + 43).Width = 25;
                indexw++;
            }
            while (indexw < titleList.Count);

            ws.Rows().AdjustToContents();
            //自動調整列高後，需.ClearHeight才可依內容高度顯示
            //for (int i = 9; i <= row; i++)
            //{
            //    ws.Row(i).ClearHeight();
            //}

            //20200911 固定列高
            for (int i = 9; i < row; i++)
            {
                ws.Row(i).Height = 95;
            }


            var result = ReportUtility.ConvertBookToByte(book, "");
            #endregion
            return result;
        }

        /// <summary>
        /// 案件轉派查詢(總部、門市)
        /// </summary>
        /// <returns></returns>
        public Byte[] CreateCaseAssignmentHSExcel(List<ExcelCaseAssignmentList> data, CaseAssignmentHSCondition condition)
        {
            //查詢條件-分類清單
            var classList_Search = new List<QuestionClassification>();
            if (condition.ClassificationID != null)
            {
                var con = new MSSQLCondition<VW_QUESTION_CLASSIFICATION_NESTED>();
                con.And(x => x.ID == (int)condition.ClassificationID);
                classList_Search = _IMasterAggregate.VWQuestionClassification_QuestionClassification_.GetList(con).ToList();
            }

            //查詢結果-分類清單
            var con2 = new MSSQLCondition<VW_QUESTION_CLASSIFICATION_NESTED>();
            var dataClassList = data.Select(x => x.ClassificationID).ToArray();
            con2.And(x => dataClassList.Contains(x.ID));
            var classList = _IMasterAggregate.VWQuestionClassification_QuestionClassification_.GetList(con2).ToList();
            int classificationCount = classList.Count == 0 ? 0 : classList.Max(x => x.Level);

            //查詢結果-結案處置
            var con4 = new MSSQLCondition<CASE_FINISH_REASON_CLASSIFICATION>();
            var nodeList = classList.Select(x => x.NodeID).Distinct().ToList();//無nodeid 資料,故使用分類查詢結果為來源
            con4.And(x => nodeList.Contains(x.NODE_ID));
            var titleList = _IMasterAggregate.CaseFinishReasonClassification_T1_T2_.GetList(con4)?.Select(x => x.Title).ToList();


            // 反應類別代碼轉換
            var con3 = new MSSQLCondition<HEADQUARTERS_NODE>(
                            x => x.NODE_ID == condition.NodeID &&
                            x.ORGANIZATION_TYPE == (byte)OrganizationType.HeaderQuarter);

            var nodeKey = _OrganizationAggregate.HeaderQuarterNode_T1_.GetOfSpecific(con3, x => x.NODE_KEY);

            var invoiceTypeList = new List<SelectItem>();
            if (DataStorage.CaseAssignmentComplaintInvoiceTypeDict.TryGetValue(nodeKey ?? string.Empty, out var CaseComplaintInvoiceTypes))
            {
                invoiceTypeList = CaseComplaintInvoiceTypes;
            }


            #region 產出Excel
            XLWorkbook book = new XLWorkbook();

            var ws = book.AddWorksheet("工作表1");
            #region 開頭
            ws.Cell(1, 1).Value = "企業別：";
            ws.Cell(1, 2).Value = "'" + condition.NodeName;
            ws.Cell(1, 3).Value = "轉派對象：";
            ws.Cell(1, 4).Value = condition.AssignmentUser == null ? "" : "'" + string.Join("/", condition.AssignmentUser);
            ws.Cell(1, 5).Value = "通知時間：";
            ws.Cell(1, 6).Value = "'" + condition.NoticeDateTimeRange;
            ws.Cell(1, 7).Value = "立案時間：";
            ws.Cell(1, 8).Value = "'" + condition.CreateTimeRange;


            ws.Cell(2, 1).Value = "反應者類型：";
            ws.Cell(2, 2).Value = condition.CaseConcatUnitType == null ? "" : "'" + ((UnitType)condition.CaseConcatUnitType).GetDescription();

            switch (condition.CaseConcatUnitType)
            {
                case UnitType.Customer:
                    ws.Cell(2, 3).Value = "姓名：";
                    ws.Cell(2, 4).Value = "'" + condition.ConcatName;
                    ws.Cell(2, 5).Value = "電話：";
                    ws.Cell(2, 6).Value = "'" + condition.ConcatTelephone;
                    ws.Cell(2, 7).Value = "電子信箱：";
                    ws.Cell(2, 8).Value = "'" + condition.ConcatEmail;
                    break;
                case UnitType.Store:
                    ws.Cell(2, 3).Value = "店名：";
                    ws.Cell(2, 4).Value = "'" + condition.ConcatStoreName;
                    ws.Cell(2, 5).Value = "店號：";
                    ws.Cell(2, 6).Value = "'" + condition.ConcatStoreNo;
                    ws.Cell(2, 7).Value = "組織：";
                    ws.Cell(2, 8).Value = condition.ConcatNode == null || condition.ConcatNode.Count() == 0 ? "" : string.Join("/", condition.ConcatNode.Select(x => x.Name));
                    break;
                case UnitType.Organization:
                    ws.Cell(2, 3).Value = "單位名稱：";
                    ws.Cell(2, 4).Value = "'" + condition.ConcatNodeName;
                    ws.Cell(2, 5).Value = "組織：";
                    ws.Cell(2, 6).Value = condition.ConcatNode == null || condition.ConcatNode.Count() == 0 ? "" : string.Join("/", condition.ConcatNode.Select(x => x.Name));
                    break;
                case null:
                    break;
            }

            //歷程狀態string 待CaseAssignmentModeType修改後修正
            string assignmentState = "";
            if (condition.AssignmentType != null && condition.Type != null)
            {
                switch ((int)condition.AssignmentType)
                {
                    case (int)CaseAssignmentProcessType.Notice:
                        assignmentState = ((CaseAssignmentComplaintNoticeType)condition.Type).GetDescription();
                        break;
                    case (int)CaseAssignmentProcessType.Invoice:
                        assignmentState = ((CaseAssignmentComplaintInvoiceType)condition.Type).GetDescription();
                        break;
                    case (int)CaseAssignmentProcessType.Assignment:
                        assignmentState = ((CaseAssignmentType)condition.Type).GetDescription();
                        break;
                    case (int)CaseAssignmentProcessType.Communication:
                        break;
                    default:
                        break;

                }
            }


            ws.Cell(3, 1).Value = "歷程模式：";
            ws.Cell(3, 2).Value = condition.AssignmentType == null ? "" : "'" + condition.AssignmentType.GetDescription();
            ws.Cell(3, 3).Value = "歷程狀態：";
            ws.Cell(3, 4).Value = "'" + assignmentState;
            ws.Cell(3, 5).Value = "反應類別：";
            ws.Cell(3, 6).Value = !string.IsNullOrEmpty(condition.InvoiceType) ? "'" + invoiceTypeList.Where(x => x.id == condition.InvoiceType).FirstOrDefault()?.text ?? "" : "";
            ws.Cell(3, 7).Value = "反應單號：";
            ws.Cell(3, 8).Value = "'" + condition.InvoiceID;

            ws.Cell(4, 1).Value = "案件編號：";
            ws.Cell(4, 2).Value = "'" + condition.CaseID;
            ws.Cell(4, 3).Value = "案件內容：";
            ws.Cell(4, 4).Value = "'" + condition.CaseContent;
            ws.Cell(4, 5).Value = "通知內容：";
            ws.Cell(4, 6).Value = "'" + condition.NoticeContent;

            ws.Cell(5, 1).Value = "被反應者類型：";
            ws.Cell(5, 2).Value = condition.CaseComplainedUnitType == null ? "" : "'" + ((UnitType)condition.CaseComplainedUnitType).GetDescription();
            switch (condition.CaseComplainedUnitType)
            {
                case UnitType.Store:
                    ws.Cell(5, 3).Value = "店名：";
                    ws.Cell(5, 4).Value = "'" + condition.CaseComplainedStoreName;
                    ws.Cell(5, 5).Value = "店號：";
                    ws.Cell(5, 6).Value = "'" + condition.CaseComplainedStoreNo;
                    ws.Cell(5, 7).Value = "組織：";
                    ws.Cell(5, 8).Value = condition.ComplainedNode == null || condition.ComplainedNode.Count() == 0 ? "" : string.Join("/", condition.ComplainedNode.Select(x => x.Name));
                    break;
                case UnitType.Organization:
                    ws.Cell(5, 3).Value = "單位名稱：";
                    ws.Cell(5, 4).Value = "'" + condition.CaseComplainedNodeName;
                    ws.Cell(5, 5).Value = "組織：";
                    ws.Cell(5, 6).Value = condition.ComplainedNode == null || condition.ComplainedNode.Count() == 0 ? "" : string.Join("/", condition.ComplainedNode.Select(x => x.Name));
                    break;
                case null:
                    break;
            }

            if (condition.ClassificationID == null || !classList_Search.Any())
            {
                ws.Cell(6, 1).Value = "問題分類1：";
                ws.Cell(6, 2).Value = "";
                ws.Cell(6, 3).Value = "問題分類2：";
                ws.Cell(6, 4).Value = "";
                ws.Cell(6, 5).Value = "問題分類3：";
                ws.Cell(6, 6).Value = "";
            }
            else
            {
                string[] parentList = classList_Search.Where(x => x.ID == condition.ClassificationID).First().ParentNamePathByArray;
                int Column = 1;
                for (int i = 0; i < parentList.Count(); i++)
                {
                    ws.Cell(6, Column).Value = "問題分類" + (i + 1) + "：";
                    Column = Column + 1;
                    ws.Cell(6, Column).Value = "'" + parentList[i];
                    Column = Column + 1;
                }
            }

            int Field = 8;

            ws.Cell(Field, 1).Value = "企業別";
            ws.Cell(Field, 2).Value = "案件來源";
            ws.Cell(Field, 3).Value = "來源時間";
            ws.Cell(Field, 4).Value = "立案時間";
            ws.Cell(Field, 5).Value = "案件編號";
            ws.Cell(Field, 6).Value = "序號(反應單號)";
            ws.Cell(Field, 7).Value = "案件等級";
            ws.Cell(Field, 8).Value = "案件狀態";
            ws.Cell(Field, 9).Value = "預立案";
            ws.Cell(Field, 10).Value = "關注案件";

            for (int i = 1; i <= classificationCount; i++)
            {
                ws.Cell(Field, 10 + i).Value = "問題分類" + i.ToString();
            }
            int column = 10 + classificationCount;

            ws.Cell(Field, column + 1).Value = "反應者類型";
            ws.Cell(Field, column + 2).Value = "反應者";
            ws.Cell(Field, column + 3).Value = "手機";
            ws.Cell(Field, column + 4).Value = "電話1";
            ws.Cell(Field, column + 5).Value = "電話2";
            ws.Cell(Field, column + 6).Value = "電子信箱";
            ws.Cell(Field, column + 7).Value = "地址";
            ws.Cell(Field, column + 8).Value = "門市";
            ws.Cell(Field, column + 9).Value = "姓名";
            ws.Cell(Field, column + 10).Value = "電話";
            ws.Cell(Field, column + 11).Value = "組織單位";
            ws.Cell(Field, column + 12).Value = "姓名";
            ws.Cell(Field, column + 13).Value = "電話";
            ws.Cell(Field, column + 14).Value = "被反應者類型";
            ws.Cell(Field, column + 15).Value = "門市";
            ws.Cell(Field, column + 16).Value = "門市負責人";
            ws.Cell(Field, column + 17).Value = "門市區經理";
            ws.Cell(Field, column + 18).Value = "門市電話";
            ws.Cell(Field, column + 19).Value = "組織單位";
            ws.Cell(Field, column + 20).Value = "姓名";
            ws.Cell(Field, column + 21).Value = "電話";
            ws.Cell(Field, column + 22).Value = "案件內容";
            ws.Cell(Field, column + 23).Value = "商品名稱";
            ws.Cell(Field, column + 24).Value = "國際條碼";
            ws.Cell(Field, column + 25).Value = "批號";
            ws.Cell(Field, column + 26).Value = "卡號";
            ws.Cell(Field, column + 27).Value = "型號";
            ws.Cell(Field, column + 28).Value = "名稱";
            ws.Cell(Field, column + 29).Value = "購買日期";
            ws.Cell(Field, column + 30).Value = "歷程模式";
            ws.Cell(Field, column + 31).Value = "歷程狀態";
            ws.Cell(Field, column + 32).Value = "反應類別";
            ws.Cell(Field, column + 33).Value = "轉派對象";
            ws.Cell(Field, column + 34).Value = "通知內容";
            ws.Cell(Field, column + 35).Value = "通知時間";
            ws.Cell(Field, column + 36).Value = "回覆內容";
            ws.Cell(Field, column + 37).Value = "銷案內容";
            ws.Cell(Field, column + 38).Value = "銷案時間";
            ws.Cell(Field, column + 39).Value = "銷案人";
            ws.Cell(Field, column + 40).Value = "結案內容";
            ws.Cell(Field, column + 41).Value = "結案時間";

            int column2 = titleList.Count;
            if (titleList.Any())
            {
                int index = 0;
                foreach (var titleitem in titleList)
                {
                    ws.Cell(Field, column + index + 42).Value = $"結案處置({titleitem})";
                    index++;
                }
                column2 = column2 - 1;
            }
            else
            {
                ws.Cell(Field, column + 42).Value = "結案處置";
            }
            ws.Cell(Field, column + column2 + 43).Value = "負責人";
            ws.Cell(Field, column + column2 + 44).Value = "案件標籤";

            #endregion

            int row = 9;
            foreach (var item in data)
            {
                ws.Cell(row, 1).Value = "'" + item.NodeName;
                ws.Cell(row, 2).Value = "'" + item.SourceType;
                ws.Cell(row, 3).Value = "'" + item.IncomingDateTime;
                ws.Cell(row, 4).Value = "'" + item.CreateTime;
                ws.Cell(row, 5).Value = "'" + item.CaseID;
                ws.Cell(row, 6).Value = "'" + item.SN;
                ws.Cell(row, 7).Value = "'" + item.CaseWarningName;
                ws.Cell(row, 8).Value = "'" + item.CaseType;
                ws.Cell(row, 9).Value = "'" + item.IsPrevention;
                ws.Cell(row, 10).Value = "'" + item.IsAttension;
                //分類
                for (int i = 1; i <= classificationCount; i++)
                {
                    if (classList.Where(x => x.ID == item.ClassificationID).Count() == 0)
                        continue;
                    var list = classList.Where(x => x.ID == item.ClassificationID).FirstOrDefault();
                    ws.Cell(row, 10 + i).Value = "'" + list.ParentNamePathByArray[i - 1];

                    if (list.ParentNamePathByArray.Count() < i + 1)
                    {
                        break;
                    }
                }
                ws.Cell(row, column + 1).Value = "'" + item.ConcatUnitType;
                ws.Cell(row, column + 2).Value = "'" + item.ConcatCustomerName;
                ws.Cell(row, column + 3).Value = "'" + item.ConcatCustomerMobile;
                ws.Cell(row, column + 4).Value = "'" + item.ConcatCustomerTelephone1;
                ws.Cell(row, column + 5).Value = "'" + item.ConcatCustomerTelephone2;
                ws.Cell(row, column + 6).Value = "'" + item.ConcatCustomerMail;
                ws.Cell(row, column + 7).Value = "'" + item.ConcatCustomerAddress;
                ws.Cell(row, column + 8).Value = "'" + item.ConcatStore;
                ws.Cell(row, column + 9).Value = "'" + item.ConcatStoreName;
                ws.Cell(row, column + 10).Value = "'" + item.ConcatStoreTelephone;
                ws.Cell(row, column + 11).Value = "'" + item.ConcatOrganization;
                ws.Cell(row, column + 12).Value = "'" + item.ConcatOrganizationName;
                ws.Cell(row, column + 13).Value = "'" + item.ConcatOrganizationTelephone;
                ws.Cell(row, column + 14).Value = "'" + item.ComplainedUnitType;
                ws.Cell(row, column + 15).Value = "'" + item.ComplainedStore;
                ws.Cell(row, column + 16).Value = "'" + item.ComplainedStoreApplyUserName;
                ws.Cell(row, column + 17).Value = "'" + item.ComplainedStoreSupervisorUserName;
                ws.Cell(row, column + 18).Value = "'" + item.ComplainedStoreTelephone;
                ws.Cell(row, column + 19).Value = "'" + item.ComplainedOrganizationNodeName;
                ws.Cell(row, column + 20).Value = "'" + item.ComplainedOrganizationName;
                ws.Cell(row, column + 21).Value = "'" + item.ComplainedOrganizationTelephone;
                ws.Cell(row, column + 22).Value = "'" + item.CaseContent;
                ws.Cell(row, column + 22).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 23).Value = "'" + item.OtherCommodityName;
                ws.Cell(row, column + 23).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 24).Value = "'" + item.OtherInternationalBarcode;
                ws.Cell(row, column + 24).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 25).Value = "'" + item.OtherBatchNo;
                ws.Cell(row, column + 25).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 26).Value = "'" + item.OtherCardNumber;
                ws.Cell(row, column + 26).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 27).Value = "'" + item.OtherProductModel;
                ws.Cell(row, column + 27).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 28).Value = "'" + item.OtherProductName;
                ws.Cell(row, column + 28).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 29).Value = "'" + item.OtherPurchaseDay;
                ws.Cell(row, column + 29).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 30).Value = "'" + item.ModeType;
                ws.Cell(row, column + 31).Value = "'" + item.AssignmentType;
                ws.Cell(row, column + 32).Value = !string.IsNullOrEmpty(item.InvoiceType) ? "'" + invoiceTypeList.Where(x => x.id == item.InvoiceType).FirstOrDefault()?.text ?? "" : "";
                ws.Cell(row, column + 33).Value = item.AssignmentUser == null ? "" : "'" + string.Join("/n", item.AssignmentUser);
                ws.Cell(row, column + 33).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 34).Value = "'" + item.NoticeContent;
                ws.Cell(row, column + 34).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 35).Value = "'" + item.NoticeTime;
                ws.Cell(row, column + 36).Value = "'" + item.RetryContent;
                ws.Cell(row, column + 36).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 37).Value = "'" + item.CloseCaseContent;
                ws.Cell(row, column + 37).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 38).Value = "'" + item.CloseCaseTime;
                ws.Cell(row, column + 39).Value = "'" + item.CloseCaseUser;
                ws.Cell(row, column + 40).Value = "'" + item.FinishContent;
                ws.Cell(row, column + 40).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 41).Value = "'" + item.FinishDateTime;

                if (titleList.Any())
                {
                    int index = 0;
                    foreach (var titleitem in titleList)
                    {
                        if (item.ReasonList.ContainsKey(titleitem))
                            ws.Cell(row, column + index + 42).Value = item.ReasonList[$"{titleitem}"];
                        else
                            ws.Cell(row, column + index + 42).Value = "";
                        index++;
                    }
                }
                else
                {
                    ws.Cell(row, column + 42).Value = "'" + item.ReasonName;
                }

                ws.Cell(row, column + column2 + 43).Value = "'" + item.ApplyUserName;
                ws.Cell(row, column + column2 + 44).Value = "'" + item.CaseTagName;

                row++;
            }
            //左右至中
            ws.Range(Field, 1, row - 1, column + column2 + 44).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            //水平 至中
            ws.Range(Field, 1, row - 1, column + column2 + 44).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


            //格線
            var rngTable1 = ws.Range(Field, 1, row - 1, column + column2 + 44);
            rngTable1.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            rngTable1.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            //自動對應欄位寬度

            ws.Columns().AdjustToContents();

            //設定案件內容與結案內容固定欄寬
            ws.Column(column + 22).Width = 100;
            ws.Column(column + 33).Width = 20;
            ws.Column(column + 34).Width = 100;
            ws.Column(column + 36).Width = 100;
            ws.Column(column + 37).Width = 100;
            ws.Column(column + 40).Width = 100;
            ws.Range(Field + 1, column + 22, row - 1, column + 22).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Range(Field + 1, column + 34, row - 1, column + 34).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Range(Field + 1, column + 36, row - 1, column + 36).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Range(Field + 1, column + 37, row - 1, column + 37).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Range(Field + 1, column + 40, row - 1, column + 40).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

            //設定結案處置固定欄寬
            int indexw = 0;
            do
            {
                ws.Column(column + indexw + 42).Width = 25;
                indexw++;
            }
            while (indexw < titleList.Count);

            ws.Rows().AdjustToContents();
            //自動調整列高後，需.ClearHeight才可依內容高度顯示
            //for (int i = 9; i <= row; i++)
            //{
            //    ws.Row(i).ClearHeight();
            //}

            //20200911 固定列高
            for (int i = 9; i < row; i++)
            {
                ws.Row(i).Height = 95;
            }

            var result = ReportUtility.ConvertBookToByte(book, "");
            #endregion
            return result;
        }

        /// <summary>
        /// 案件轉派查詢(廠商)
        /// </summary>
        /// <returns></returns>
        public Byte[] CreateCaseAssignmentVendorExcel(List<ExcelCaseAssignmentList> data, CaseAssignmentHSCondition condition)
        {
            //查詢條件-分類清單
            var classList_Search = new List<QuestionClassification>();
            if (condition.ClassificationID != null)
            {
                var con = new MSSQLCondition<VW_QUESTION_CLASSIFICATION_NESTED>();
                con.And(x => x.ID == (int)condition.ClassificationID);
                classList_Search = _IMasterAggregate.VWQuestionClassification_QuestionClassification_.GetList(con).ToList();
            }

            //查詢結果-分類清單
            var con2 = new MSSQLCondition<VW_QUESTION_CLASSIFICATION_NESTED>();
            var dataClassList = data.Select(x => x.ClassificationID).ToArray();
            con2.And(x => dataClassList.Contains(x.ID));
            var classList = _IMasterAggregate.VWQuestionClassification_QuestionClassification_.GetList(con2).ToList();
            int classificationCount = classList.Count == 0 ? 0 : classList.Max(x => x.Level);

            //查詢結果-結案處置
            var con4 = new MSSQLCondition<CASE_FINISH_REASON_CLASSIFICATION>();
            var nodeList = classList.Select(x => x.NodeID).Distinct().ToList();//無nodeid 資料,故使用分類查詢結果為來源
            con4.And(x => nodeList.Contains(x.NODE_ID));
            var titleList = _IMasterAggregate.CaseFinishReasonClassification_T1_T2_.GetList(con4)?.Select(x => x.Title).ToList();

            // 反應類別代碼轉換
            var con3 = new MSSQLCondition<HEADQUARTERS_NODE>(
                            x => x.NODE_ID == condition.NodeID &&
                            x.ORGANIZATION_TYPE == (byte)OrganizationType.HeaderQuarter);

            var nodeKey = _OrganizationAggregate.HeaderQuarterNode_T1_.GetOfSpecific(con3, x => x.NODE_KEY);

            var invoiceTypeList = new List<SelectItem>();
            if (DataStorage.CaseAssignmentComplaintInvoiceTypeDict.TryGetValue(nodeKey ?? string.Empty, out var CaseComplaintInvoiceTypes))
            {
                invoiceTypeList = CaseComplaintInvoiceTypes;
            }


            #region 產出Excel
            XLWorkbook book = new XLWorkbook();

            var ws = book.AddWorksheet("工作表1");

            #region 開頭
            ws.Cell(1, 1).Value = "企業別：";
            ws.Cell(1, 2).Value = "'" + condition.NodeName;
            ws.Cell(1, 3).Value = "轉派對象：";
            ws.Cell(1, 4).Value = condition.AssignmentUser == null ? "" : "'" + string.Join("/", condition.AssignmentUser);
            ws.Cell(1, 5).Value = "通知時間：";
            ws.Cell(1, 6).Value = "'" + condition.NoticeDateTimeRange;
            ws.Cell(1, 7).Value = "立案時間：";
            ws.Cell(1, 8).Value = "'" + condition.CreateTimeRange;


            ws.Cell(2, 1).Value = "反應者類型：";
            ws.Cell(2, 2).Value = condition.CaseConcatUnitType == null ? "" : "'" + ((UnitType)condition.CaseConcatUnitType).GetDescription();

            switch (condition.CaseConcatUnitType)
            {
                case UnitType.Customer:
                    ws.Cell(2, 3).Value = "姓名：";
                    ws.Cell(2, 4).Value = "'" + condition.ConcatName;
                    ws.Cell(2, 5).Value = "電話：";
                    ws.Cell(2, 6).Value = "'" + condition.ConcatTelephone;
                    ws.Cell(2, 7).Value = "電子信箱：";
                    ws.Cell(2, 8).Value = "'" + condition.ConcatEmail;
                    break;
                case UnitType.Store:
                    ws.Cell(2, 3).Value = "店名：";
                    ws.Cell(2, 4).Value = "'" + condition.ConcatStoreName;
                    ws.Cell(2, 5).Value = "店號：";
                    ws.Cell(2, 6).Value = "'" + condition.ConcatStoreNo;
                    ws.Cell(2, 7).Value = "組織：";
                    ws.Cell(2, 8).Value = condition.ConcatNode == null || condition.ConcatNode.Count() == 0 ? "" : string.Join("/", condition.ConcatNode.Select(x => x.Name));
                    break;
                case UnitType.Organization:
                    ws.Cell(2, 3).Value = "單位名稱：";
                    ws.Cell(2, 4).Value = "'" + condition.ConcatNodeName;
                    ws.Cell(2, 5).Value = "組織：";
                    ws.Cell(2, 6).Value = condition.ConcatNode == null || condition.ConcatNode.Count() == 0 ? "" : string.Join("/", condition.ConcatNode.Select(x => x.Name));
                    break;
                case null:
                    break;
            }

            //歷程狀態string 待CaseAssignmentModeType修改後修正
            string assignmentState = "";
            if (condition.AssignmentType != null && condition.Type != null)
            {
                switch ((int)condition.AssignmentType)
                {
                    case (int)CaseAssignmentProcessType.Notice:
                        assignmentState = ((CaseAssignmentComplaintNoticeType)condition.Type).GetDescription();
                        break;
                    case (int)CaseAssignmentProcessType.Invoice:
                        assignmentState = ((CaseAssignmentComplaintInvoiceType)condition.Type).GetDescription();
                        break;
                    case (int)CaseAssignmentProcessType.Assignment:
                        assignmentState = ((CaseAssignmentType)condition.Type).GetDescription();
                        break;
                    case (int)CaseAssignmentProcessType.Communication:
                        break;
                    default:
                        break;

                }
            }

            ws.Cell(3, 1).Value = "歷程模式：";
            ws.Cell(3, 2).Value = condition.AssignmentType == null ? "" : "'" + condition.AssignmentType.GetDescription();
            ws.Cell(3, 3).Value = "歷程狀態：";
            ws.Cell(3, 4).Value = "'" + assignmentState;
            ws.Cell(3, 5).Value = "反應類別：";
            ws.Cell(3, 6).Value = !string.IsNullOrEmpty(condition.InvoiceType) ? "'" + invoiceTypeList.Where(x => x.id == condition.InvoiceType).FirstOrDefault()?.text ?? "" : "";
            ws.Cell(3, 7).Value = "反應單號：";
            ws.Cell(3, 8).Value = "'" + condition.InvoiceID;

            ws.Cell(4, 1).Value = "案件編號：";
            ws.Cell(4, 2).Value = "'" + condition.CaseID;
            ws.Cell(4, 3).Value = "案件內容：";
            ws.Cell(4, 4).Value = "'" + condition.CaseContent;
            ws.Cell(4, 5).Value = "通知內容：";
            ws.Cell(4, 6).Value = "'" + condition.NoticeContent;

            ws.Cell(5, 1).Value = "被反應者類型：";
            ws.Cell(5, 2).Value = condition.CaseComplainedUnitType == null ? "" : "'" + ((UnitType)condition.CaseComplainedUnitType).GetDescription();
            switch (condition.CaseComplainedUnitType)
            {
                case UnitType.Store:
                    ws.Cell(5, 3).Value = "店名：";
                    ws.Cell(5, 4).Value = "'" + condition.CaseComplainedStoreName;
                    ws.Cell(5, 5).Value = "店號：";
                    ws.Cell(5, 6).Value = "'" + condition.CaseComplainedStoreNo;
                    ws.Cell(5, 7).Value = "組織：";
                    ws.Cell(5, 8).Value = condition.ComplainedNode == null || condition.ComplainedNode.Count() == 0 ? "" : string.Join("/", condition.ComplainedNode.Select(x => x.Name));
                    break;
                case UnitType.Organization:
                    ws.Cell(5, 3).Value = "單位名稱：";
                    ws.Cell(5, 4).Value = "'" + condition.CaseComplainedNodeName;
                    ws.Cell(5, 5).Value = "組織：";
                    ws.Cell(5, 6).Value = condition.ComplainedNode == null || condition.ComplainedNode.Count() == 0 ? "" : string.Join("/", condition.ComplainedNode.Select(x => x.Name));
                    break;
                case null:
                    break;
            }

            if (condition.ClassificationID == null || !classList_Search.Any())
            {
                ws.Cell(6, 1).Value = "問題分類1：";
                ws.Cell(6, 2).Value = "";
                ws.Cell(6, 3).Value = "問題分類2：";
                ws.Cell(6, 4).Value = "";
                ws.Cell(6, 5).Value = "問題分類3：";
                ws.Cell(6, 6).Value = "";
            }
            else
            {
                string[] parentList = classList_Search.Where(x => x.ID == condition.ClassificationID).First().ParentNamePathByArray;
                int Column = 1;
                for (int i = 0; i < parentList.Count(); i++)
                {
                    ws.Cell(6, Column).Value = "問題分類" + (i + 1) + "：";
                    Column = Column + 1;
                    ws.Cell(6, Column).Value = "'" + parentList[i];
                    Column = Column + 1;
                }
            }
            int Field = 8;

            ws.Cell(Field, 1).Value = "企業別";
            ws.Cell(Field, 2).Value = "案件來源";
            ws.Cell(Field, 3).Value = "來源時間";
            ws.Cell(Field, 4).Value = "立案時間";
            ws.Cell(Field, 5).Value = "案件編號";
            ws.Cell(Field, 6).Value = "序號(反應單號)";
            ws.Cell(Field, 7).Value = "案件等級";
            ws.Cell(Field, 8).Value = "案件狀態";
            ws.Cell(Field, 9).Value = "預立案";
            ws.Cell(Field, 10).Value = "關注案件";

            for (int i = 1; i <= classificationCount; i++)
            {
                ws.Cell(Field, 10 + i).Value = "問題分類" + i.ToString();
            }
            int column = 10 + classificationCount;

            ws.Cell(Field, column + 1).Value = "反應者類型";
            ws.Cell(Field, column + 2).Value = "反應者";
            ws.Cell(Field, column + 3).Value = "手機";
            ws.Cell(Field, column + 4).Value = "電話1";
            ws.Cell(Field, column + 5).Value = "電話2";
            ws.Cell(Field, column + 6).Value = "電子信箱";
            ws.Cell(Field, column + 7).Value = "地址";
            ws.Cell(Field, column + 8).Value = "門市";
            ws.Cell(Field, column + 9).Value = "姓名";
            ws.Cell(Field, column + 10).Value = "電話";
            ws.Cell(Field, column + 11).Value = "組織單位";
            ws.Cell(Field, column + 12).Value = "姓名";
            ws.Cell(Field, column + 13).Value = "電話";
            ws.Cell(Field, column + 14).Value = "被反應者類型";
            ws.Cell(Field, column + 15).Value = "門市";
            ws.Cell(Field, column + 16).Value = "門市負責人";
            ws.Cell(Field, column + 17).Value = "門市區經理";
            ws.Cell(Field, column + 18).Value = "門市電話";
            ws.Cell(Field, column + 19).Value = "組織單位";
            ws.Cell(Field, column + 20).Value = "姓名";
            ws.Cell(Field, column + 21).Value = "電話";
            ws.Cell(Field, column + 22).Value = "案件內容";
            ws.Cell(Field, column + 23).Value = "商品名稱";
            ws.Cell(Field, column + 24).Value = "國際條碼";
            ws.Cell(Field, column + 25).Value = "批號";
            ws.Cell(Field, column + 26).Value = "卡號";
            ws.Cell(Field, column + 27).Value = "型號";
            ws.Cell(Field, column + 28).Value = "名稱";
            ws.Cell(Field, column + 29).Value = "購買日期";
            ws.Cell(Field, column + 30).Value = "歷程模式";
            ws.Cell(Field, column + 31).Value = "歷程狀態";
            ws.Cell(Field, column + 32).Value = "反應類別";
            ws.Cell(Field, column + 33).Value = "轉派對象";
            ws.Cell(Field, column + 34).Value = "通知內容";
            ws.Cell(Field, column + 35).Value = "通知時間";
            ws.Cell(Field, column + 36).Value = "回覆內容";
            ws.Cell(Field, column + 37).Value = "銷案內容";
            ws.Cell(Field, column + 38).Value = "銷案時間";
            ws.Cell(Field, column + 39).Value = "銷案人";
            ws.Cell(Field, column + 40).Value = "結案內容";
            ws.Cell(Field, column + 41).Value = "結案時間";
            int column2 = titleList.Count;
            if (titleList.Any())
            {
                int index = 0;
                foreach (var titleitem in titleList)
                {
                    ws.Cell(Field, column + index + 42).Value = $"結案處置({titleitem})";
                    index++;
                }
                column2 = column2 - 1;
            }
            else
            {
                ws.Cell(Field, column + 42).Value = "結案處置";
            }

            ws.Cell(Field, column + column2 + 43).Value = "負責人";
            ws.Cell(Field, column + column2 + 44).Value = "案件標籤";

            #endregion

            int row = 9;
            foreach (var item in data)
            {
                ws.Cell(row, 1).Value = "'" + item.NodeName;
                ws.Cell(row, 2).Value = "'" + item.SourceType;
                ws.Cell(row, 3).Value = "'" + item.IncomingDateTime;
                ws.Cell(row, 4).Value = "'" + item.CreateTime;
                ws.Cell(row, 5).Value = "'" + item.CaseID;
                ws.Cell(row, 6).Value = "'" + item.SN;
                ws.Cell(row, 7).Value = "'" + item.CaseWarningName;
                ws.Cell(row, 8).Value = "'" + item.CaseType;
                ws.Cell(row, 9).Value = "'" + item.IsPrevention;
                ws.Cell(row, 10).Value = "'" + item.IsAttension;
                //分類
                for (int i = 1; i <= classificationCount; i++)
                {
                    if (classList.Where(x => x.ID == item.ClassificationID).Count() == 0)
                        continue;
                    var list = classList.Where(x => x.ID == item.ClassificationID).FirstOrDefault();
                    ws.Cell(row, 10 + i).Value = "'" + list.ParentNamePathByArray[i - 1];

                    if (list.ParentNamePathByArray.Count() < i + 1)
                    {
                        break;
                    }
                }
                ws.Cell(row, column + 1).Value = "'" + item.ConcatUnitType;
                ws.Cell(row, column + 2).Value = "'" + item.ConcatCustomerName;
                ws.Cell(row, column + 3).Value = "'" + item.ConcatCustomerMobile;
                ws.Cell(row, column + 4).Value = "'" + item.ConcatCustomerTelephone1;
                ws.Cell(row, column + 5).Value = "'" + item.ConcatCustomerTelephone2;
                ws.Cell(row, column + 6).Value = "'" + item.ConcatCustomerMail;
                ws.Cell(row, column + 7).Value = "'" + item.ConcatCustomerAddress;
                ws.Cell(row, column + 8).Value = "'" + item.ConcatStore;
                ws.Cell(row, column + 9).Value = "'" + item.ConcatStoreName;
                ws.Cell(row, column + 10).Value = "'" + item.ConcatStoreTelephone;
                ws.Cell(row, column + 11).Value = "'" + item.ConcatOrganization;
                ws.Cell(row, column + 12).Value = "'" + item.ConcatOrganizationName;
                ws.Cell(row, column + 13).Value = "'" + item.ConcatOrganizationTelephone;
                ws.Cell(row, column + 14).Value = "'" + item.ComplainedUnitType;
                ws.Cell(row, column + 15).Value = "'" + item.ComplainedStore;
                ws.Cell(row, column + 16).Value = "'" + item.ComplainedStoreApplyUserName;
                ws.Cell(row, column + 17).Value = "'" + item.ComplainedStoreSupervisorUserName;
                ws.Cell(row, column + 18).Value = "'" + item.ComplainedStoreTelephone;
                ws.Cell(row, column + 19).Value = "'" + item.ComplainedOrganizationNodeName;
                ws.Cell(row, column + 20).Value = "'" + item.ComplainedOrganizationName;
                ws.Cell(row, column + 21).Value = "'" + item.ComplainedOrganizationTelephone;
                ws.Cell(row, column + 22).Value = "'" + item.CaseContent;
                ws.Cell(row, column + 22).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 23).Value = "'" + item.OtherCommodityName;
                ws.Cell(row, column + 23).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 24).Value = "'" + item.OtherInternationalBarcode;
                ws.Cell(row, column + 24).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 25).Value = "'" + item.OtherBatchNo;
                ws.Cell(row, column + 25).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 26).Value = "'" + item.OtherCardNumber;
                ws.Cell(row, column + 26).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 27).Value = "'" + item.OtherProductModel;
                ws.Cell(row, column + 27).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 28).Value = "'" + item.OtherProductName;
                ws.Cell(row, column + 28).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 29).Value = "'" + item.OtherPurchaseDay;
                ws.Cell(row, column + 29).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 30).Value = "'" + item.ModeType;
                ws.Cell(row, column + 31).Value = "'" + item.AssignmentType;
                ws.Cell(row, column + 32).Value = !string.IsNullOrEmpty(item.InvoiceType) ? "'" + invoiceTypeList.Where(x => x.id == item.InvoiceType).FirstOrDefault()?.text ?? "" : "";
                ws.Cell(row, column + 33).Value = item.AssignmentUser == null ? "" : "'" + string.Join("/", item.AssignmentUser);
                ws.Cell(row, column + 33).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 34).Value = "'" + item.NoticeContent;
                ws.Cell(row, column + 34).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 35).Value = "'" + item.NoticeTime;
                ws.Cell(row, column + 36).Value = "'" + item.RetryContent;
                ws.Cell(row, column + 36).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 37).Value = "'" + item.CloseCaseContent;
                ws.Cell(row, column + 37).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 38).Value = "'" + item.CloseCaseTime;
                ws.Cell(row, column + 39).Value = "'" + item.CloseCaseUser;
                ws.Cell(row, column + 40).Value = "'" + item.FinishContent;
                ws.Cell(row, column + 40).Style.Alignment.WrapText = true;
                ws.Cell(row, column + 41).Value = "'" + item.FinishDateTime;

                if (titleList.Any())
                {
                    int index = 0;
                    foreach (var titleitem in titleList)
                    {
                        if (item.ReasonList.ContainsKey(titleitem))
                            ws.Cell(row, column + index + 42).Value = item.ReasonList[$"{titleitem}"];
                        else
                            ws.Cell(row, column + index + 42).Value = "";
                        index++;
                    }
                }
                else
                {
                    ws.Cell(row, column + 42).Value = "'" + item.ReasonName;
                }

                ws.Cell(row, column + column2 + 43).Value = "'" + item.ApplyUserName;
                ws.Cell(row, column + column2 + 44).Value = "'" + item.CaseTagName;

                row++;
            }
            //左右至中
            ws.Range(Field, 1, row - 1, column + column2 + 44).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            //水平 至中
            ws.Range(Field, 1, row - 1, column + column2 + 44).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


            //格線
            var rngTable1 = ws.Range(Field, 1, row - 1, column + column2 + 44);
            rngTable1.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            rngTable1.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            //自動對應欄位寬度

            ws.Columns().AdjustToContents();

            //設定案件內容與結案內容固定欄寬
            ws.Column(column + 22).Width = 100;
            ws.Column(column + 33).Width = 20;
            ws.Column(column + 34).Width = 100;
            ws.Column(column + 36).Width = 100;
            ws.Column(column + 37).Width = 100;
            ws.Column(column + 40).Width = 100;
            ws.Range(Field + 1, column + 22, row - 1, column + 22).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Range(Field + 1, column + 34, row - 1, column + 34).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Range(Field + 1, column + 36, row - 1, column + 36).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Range(Field + 1, column + 37, row - 1, column + 37).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Range(Field + 1, column + 40, row - 1, column + 40).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

            //設定結案處置固定欄寬
            int indexw = 0;
            do
            {
                ws.Column(column + indexw + 42).Width = 25;
                indexw++;
            }
            while (indexw < titleList.Count);

            ws.Rows().AdjustToContents();
            //自動調整列高後，需.ClearHeight才可依內容高度顯示
            //for (int i = 9; i <= row; i++)
            //{
            //    ws.Row(i).ClearHeight();
            //}

            //20200911 固定列高
            for (int i = 9; i < row; i++)
            {
                ws.Row(i).Height = 95;
            }

            var result = ReportUtility.ConvertBookToByte(book, "");
            #endregion
            return result;
        }
    }
}
