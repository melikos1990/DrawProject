using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using Newtonsoft.Json;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.COMMON_BU.Service;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Case;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Master;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Domain.Types;
using SMARTII.PPCLIFE.Domain;
using SMARTII.PPCLIFE.Domain.DataList;
using SMARTII.Resource.Tag;
using SMARTII.Service.Cache;
using static SMARTII.Domain.Cache.EssentialCache;
using static SMARTII.Domain.Data.DataUtility;

namespace SMARTII.PPCLIFE.Service
{
    public class PPCLIFEFacade : PPCLIFEBaseFacade, IPPCLifeNotificationFacade
    {

        private readonly ICaseAggregate _CaseAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly IMasterAggregate _MasterAggregate;
        private readonly ICommonAggregate _CommonAggregate;
        private readonly INotificationAggregate _NotificationAggregate;
        private readonly INotificationPersonalFacade _NotificationPersonalFacade;
        private readonly ReportFacade _ReportFacade;
        private readonly PPCLifeReportFacade _PPCLifeReportFacade;

        public PPCLIFEFacade(ICaseAggregate CaseAggregate,
            IOrganizationAggregate OrganizationAggregate,
            IMasterAggregate MasterAggregate,
            ICommonAggregate CommonAggregate,
            INotificationAggregate NotificationAggregate,
            INotificationPersonalFacade NotificationPersonalFacade,
            ReportFacade ReportFacade,
            PPCLifeReportFacade PPCLifeReportFacade)
        {
            _CaseAggregate = CaseAggregate;
            _OrganizationAggregate = OrganizationAggregate;
            _MasterAggregate = MasterAggregate;
            _CommonAggregate = CommonAggregate;
            _ReportFacade = ReportFacade;
            _PPCLifeReportFacade = PPCLifeReportFacade;
            _NotificationAggregate = NotificationAggregate;
            _NotificationPersonalFacade = NotificationPersonalFacade;
        }
        #region 統一藥品來電紀錄 

        /// <summary>
        /// 統一藥品 - 品牌商品與問題歸類
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="nodekey"></param>
        /// <returns></returns>
        public PPCLIFEBrandCalcDataList GetBrandCalcExcel(DateTime start, DateTime end, string nodekey)
        {
            //取得Nodekey範圍
            var conHS = new MSSQLCondition<HEADQUARTERS_NODE>();
            conHS.And(x => x.NODE_KEY == nodekey);
            var header = _OrganizationAggregate.HeaderQuarterNode_T1_IOrganizationNode_.Get(conHS);            
            
            //以bu取得bu以下所有Node
            var conHSList = new MSSQLCondition<HEADQUARTERS_NODE>();           
            conHSList.And(x => x.BU_ID == header.NodeID);
            //已啟用
            conHSList.And(x => x.IS_ENABLED == true);
            var headerList = _OrganizationAggregate.HeaderQuarterNode_T1_IOrganizationNode_.GetList(conHSList);

            //所有NodeId
            var nodeList = headerList.Select(x => x.NodeID).Cast<int?>().ToList();

            //取得品牌 案件數
            var conCase = new MSSQLCondition<CASE>();
            conCase.IncludeBy(x => x.CASE_CONCAT_USER);
            conCase.IncludeBy(x => x.CASE_COMPLAINED_USER);
            conCase.IncludeBy(x => x.CASE_ASSIGNMENT_COMMUNICATE);
            conCase.IncludeBy(x => x.CASE_ASSIGNMENT_COMMUNICATE.Select(g => g.CASE_ASSIGNMENT_COMMUNICATE_USER));
            conCase.IncludeBy(x => x.CASE_ASSIGNMENT_COMPLAINT_NOTICE);
            conCase.IncludeBy(x => x.CASE_ASSIGNMENT_COMPLAINT_NOTICE.Select(g => g.CASE_ASSIGNMENT_COMPLAINT_NOTICE_USER));
            conCase.IncludeBy(x => x.CASE_ASSIGNMENT_COMPLAINT_INVOICE);
            conCase.IncludeBy(x => x.CASE_ASSIGNMENT_COMPLAINT_INVOICE.Select(g => g.CASE_ASSIGNMENT_COMPLAINT_INVOICE_USER));
            conCase.IncludeBy(x => x.CASE_ASSIGNMENT);
            conCase.IncludeBy(x => x.CASE_ASSIGNMENT.Select(g => g.CASE_ASSIGNMENT_USER));
            conCase.IncludeBy(x => x.CASE_FINISH_REASON_DATA);
            conCase.And(x => x.NODE_ID == header.NodeID);
            conCase.And(x => x.CREATE_DATETIME > start && x.CREATE_DATETIME <= end);
            conCase.And(x => x.IS_REPORT == true);

            var caseList = _CaseAggregate.Case_T1_T2_.GetList(conCase);

            //撈取有被反應者的案件
            var caseListGroup = caseList.Where(x => x.CaseComplainedUsers.Any(y => y.CaseComplainedUserType == CaseComplainedUserType.Responsibility))
                                        .GroupBy(x => x.CaseComplainedUsers.Select(y => y.NodeID).FirstOrDefault()).OrderBy(x => x.Key)
                                        .ToList();
           
            //撈取無被反應者的資料
            var noGroupList = caseList.Where(x => x.CaseComplainedUsers.Count == 0).ToList();
            caseListGroup.Add(new Grouping<int?,Case>(null, noGroupList));

            //問題分類
            var caseClassIDList = caseList.Select(x => x.QuestionClassificationID).Cast<int?>();
            var conclass = new MSSQLCondition<VW_QUESTION_CLASSIFICATION_NESTED>();
            conclass.And(x => caseClassIDList.Contains(x.ID));
            var classList = _MasterAggregate.VWQuestionClassification_QuestionClassification_.GetList(conclass).ToList();

            //回覆顧客方式、問題要因ID
            var conReasonClass = new MSSQLCondition<CASE_FINISH_REASON_CLASSIFICATION>();
            conReasonClass.And(x => x.KEY == ReasonClassValue.REPLY || x.KEY == ReasonClassValue.FACTORS);
            var reasonClassList = _MasterAggregate.CaseFinishReasonClassification_T1_T2_.GetList(conReasonClass);
            int replyID = reasonClassList.Where(x => x.Key == ReasonClassValue.REPLY).FirstOrDefault().ID;
            int factorsID = reasonClassList.Where(x => x.Key == ReasonClassValue.FACTORS).FirstOrDefault().ID;

            //回覆顧客方式、問題要因，欄位
            var conReason = new MSSQLCondition<CASE_FINISH_REASON_DATA>();
            conReason.And(x => x.CLASSIFICATION_ID == replyID || x.CLASSIFICATION_ID == factorsID);
            var reasonList = _MasterAggregate.CaseFinishReasonData_T1_T2_.GetList(conReason);

            List<BPSCalc> bPSCalcsList = new List<BPSCalc>();
            List<BPSDetail> bPSDetailsList = new List<BPSDetail>();
                       
            //讀取有案件之品牌Key
            var caseKey = caseListGroup.Select(x => x.Key);

            //將有案件之品牌的Key從nodeList移除
            foreach(var key in caseKey)
            {
                nodeList.Remove(key);
            }

            //同下方的groupNodeName
            var groupNodeNameFirstList = caseListGroup.FirstOrDefault().GroupBy(x => x.CaseComplainedUsers.Select(y => y.NodeName).FirstOrDefault()).ToList();

            //新增無案件之品牌
            foreach (var node in nodeList)
            {
                var temp = new BPSCalc
                {
                    NodeID = node,

                    NodeName = headerList.Where(x => x.NodeID == node).Select(x => x.Name).FirstOrDefault(),
                    //總案件數
                    TotalCaseCount = 0,
                    //一般案件
                    GeneralCaseCount = 0,
                    //客訴單
                    ComplaintInvoiceCount = 0,
                    //回覆
                    ReplyCustomerList = groupNodeNameFirstList.FirstOrDefault().SelectMany(x => x.CaseFinishReasonDatas.Where(y => y.ClassificationID == replyID && y.NodeID == node)).ToList(),
                    //問題要因
                    CauseProblemList = groupNodeNameFirstList.FirstOrDefault().SelectMany(x => x.CaseFinishReasonDatas.Where(y => y.ClassificationID == factorsID && y.NodeID == node)).ToList(),
                };
                bPSCalcsList.Add(temp);
            }

            foreach (var item in caseListGroup)
            {
                //轉成Excel時，加總是以NodeID != 0來加總
                //因此將無被反應者案件之NodeID，設成999999
                int NodeID = item.Key ?? 999999;
                var groupNodeName = item.GroupBy(x => x.CaseComplainedUsers.Select(y => y.NodeName).FirstOrDefault()).ToList();                

                foreach (var nodeNameItem in groupNodeName)
                {
                    //取得彙整資料
                    var temp = new BPSCalc
                    {
                        NodeID = NodeID,

                        NodeName = string.IsNullOrEmpty(nodeNameItem.Key) ? "統一藥品" : nodeNameItem.Key,
                        //總案件數
                        TotalCaseCount = nodeNameItem.Count(),
                        //一般案件
                        GeneralCaseCount = nodeNameItem.Count() - nodeNameItem.Where(x => x.ComplaintInvoice.Any()).Count(),
                        //客訴單
                        ComplaintInvoiceCount = nodeNameItem.Where(x => x.ComplaintInvoice.Any()).Count(),
                        //回覆
                        ReplyCustomerList = nodeNameItem.SelectMany(x => x.CaseFinishReasonDatas.Where(y => y.ClassificationID == replyID)).ToList(),
                        //問題要因
                        CauseProblemList = nodeNameItem.SelectMany(x => x.CaseFinishReasonDatas.Where(y => y.ClassificationID == factorsID)).ToList(),
                    };
                    bPSCalcsList.Add(temp);

                    //取得明細
                    foreach (var cs in nodeNameItem)
                    {
                        var concat = cs.CaseConcatUsers;
                        var complained = cs.CaseComplainedUsers.Where(y => y.CaseComplainedUserType == CaseComplainedUserType.Responsibility);
                        var assignment = cs.CaseAssignments.SelectMany(y => y.NoticeUsers).ToList();
                        var assignmentInvoice = cs.ComplaintInvoice.SelectMany(c => c.NoticeUsers).ToList();
                        var assignmentNotice = cs.ComplaintNotice.SelectMany(c => c.NoticeUsers).ToList();
                        var assignmentCommincate = cs.CaseAssignmentCommunicates.Where(x => x.NoticeUsers != null).SelectMany(c => c.NoticeUsers).ToList();
                        assignment.AddRange(assignmentInvoice);
                        assignment.AddRange(assignmentNotice);
                        assignment.AddRange(assignmentCommincate);

                        var tempDetail = new BPSDetail
                        {
                            CaseID = cs.CaseID,
                            Date = cs.CreateDateTime.ToString("MM/dd"),
                            Time = cs.CreateDateTime.ToString("HH:mm"),
                            ClassList = classList.Where(x => x.ID == cs.QuestionClassificationID).FirstOrDefault(),
                            //反應品牌
                            ComplainedNodeName = complained.Count() == 0 ? "" :
                            complained.First().NodeName,
                            //姓名
                            ConcatUserName = concat.Count() == 0 ? ""
                            : concat.First().UnitType == UnitType.Customer ? concat.First().UserName + concat.First().Gender.GetDescription()
                            : concat.First().UnitType == UnitType.Organization ? concat.First().NodeName
                            : "",
                            //聯繫電話
                            ConcatMobile = concat.Count() == 0 ? ""
                            : GetMobile(concat.First()),
                            IsInvoice = cs.ComplaintInvoice.Any() ? "是" : "否",
                            //問題
                            CaseContent = cs.Content,
                            FinishContent = cs.FinishContent,
                            FinishDate = cs.FinishDateTime.HasValue ? cs.FinishDateTime.Value.ToString("yyyy/MM/dd HH:mm:ss") : "",
                            ApplyUserName = cs.ApplyUserName,
                            //轉派單位人員
                            AssignmentUserName = assignment.Count() == 0 ? "" : String.Join("\r\n", assignment.ToArray()),
                            //回覆
                            ReplyCustomerList = cs.CaseFinishReasonDatas.Where(y => y.ClassificationID == replyID).ToList(),
                            //問題要因
                            CauseProblemList = cs.CaseFinishReasonDatas.Where(y => y.ClassificationID == factorsID).ToList(),
                        };
                        if (!bPSDetailsList.Any(x => x.CaseID == tempDetail.CaseID))
                            bPSDetailsList.Add(tempDetail);
                    }
                }
            }

            //取得品牌  回覆、問題要因
            var result = new PPCLIFEBrandCalcDataList();
            result.BrandSummaryCalc = bPSCalcsList.OrderBy(x => x.NodeID).ToList();
            result.BrandSummaryDetail = bPSDetailsList.OrderBy(x=>x.CaseID).ToList();
            result.FieldList = reasonList.OrderBy(x => x.ClassificationID).ThenBy(x => x.ID).ToList();
            result.ReplyID = replyID;
            result.FactorsID = factorsID;
            return result;
        }
        /// <summary>
        /// 統一藥品來電紀錄
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="nodekey"></param>
        /// <param name="includeComplainedUser">false="全案件", true="只含有被反應者案件"</param>
        /// <returns></returns>
        public async Task<PPCLIFEDataList> GetOnCallExcelAsync(DateTime start, DateTime end, string nodekey, bool includeComplainedUser = true)
        {
            #region 取得Nodekey範圍
            var conHS = new MSSQLCondition<HEADQUARTERS_NODE>();
            conHS.And(x => x.NODE_KEY == nodekey);
            var header = _OrganizationAggregate.HeaderQuarterNode_T1_T2_.Get(conHS);
            var conHSList = new MSSQLCondition<HEADQUARTERS_NODE>();
            conHSList.And(x => x.LEFT_BOUNDARY >= header.LeftBoundary && x.RIGHT_BOUNDARY <= header.RightBoundary);
            var headerList = _OrganizationAggregate.HeaderQuarterNode_T1_T2_.GetList(conHSList);
            var nodekeyList = includeComplainedUser == true ? string.Join("','", headerList.Select(x => x.NodeID)) : null;
            #endregion

            #region 取得客訴 分類
            //根據QC  來找尋下面的小分類
            var includeStr = "Select".NestedLambdaString("QUESTION_CLASSIFICATION1", "c => c.QUESTION_CLASSIFICATION1", 5);
            var expression = await typeof(QUESTION_CLASSIFICATION).IncludeExpression<Func<QUESTION_CLASSIFICATION, object>>(includeStr);
            var conQC = new MSSQLCondition<QUESTION_CLASSIFICATION>();
            conQC.IncludeBy(expression);
            conQC.And(c => c.CODE == CodeValue.GENERAL || c.CODE == CodeValue.URGENT);
            List<QuestionClassification> qcfList = _MasterAggregate.QuestionClassification_T1_T2_.GetList(conQC).ToList();


            //根據QC  來找尋一般客訴、重大客訴 分類
            List<int> totalGneralIDs = new List<int>();
            List<int> totalUrgentIDs = new List<int>();
            var generalqclist = qcfList.Where(x => x.Code == CodeValue.GENERAL);
            foreach (var item in generalqclist)
            {
                var childs = item.Flatten();
                totalGneralIDs.AddRange(childs.Select(c => c.ID));
            }
            var urgentqclist = qcfList.Where(x => x.Code == CodeValue.URGENT);
            foreach (var item in urgentqclist)
            {
                var childs = item.Flatten();
                totalUrgentIDs.AddRange(childs.Select(c => c.ID));
            }
            string generalclassID = string.Join(",", totalGneralIDs.Select(x => x.ToString()));
            string urgentclassID = string.Join(",", totalUrgentIDs.Select(x => x.ToString()));
            string AllComplaint = generalclassID + "," + urgentclassID;


            //抓取客訴分類ID
            generalclassID = string.IsNullOrWhiteSpace(generalclassID) ? null : generalclassID;
            urgentclassID = string.IsNullOrWhiteSpace(urgentclassID) ? null : urgentclassID;

            //去除客訴紀錄的案件
            AllComplaint = string.IsNullOrWhiteSpace(generalclassID) && string.IsNullOrWhiteSpace(urgentclassID) ? null : AllComplaint;
            #endregion

            var en = new SMARTIIEntities(DataAccessCache.Instance.SmartIIConn);

            //來電紀錄、來信紀錄(不含客訴)
            var listRecord = en.SP_PPCLIFE_BATCH_RECORD(
                start,
                end,
                AllComplaint,
                null,
                (byte)header.BUID
                ).ToList();
            var tempRecord = AutoMapper.Mapper.Map<List<PPCLIFECallHistory>>(listRecord);

            //客訴紀錄 - 一般客訴
            var listGeneralComplaint = en.SP_PPCLIFE_BATCH_COMPLAINT(
                start,
                end,
                generalclassID,
                null
                ).ToList();
            var OnGeneralComplaintTemp = AutoMapper.Mapper.Map<List<PPCLIFEComplaintHistory>>(listGeneralComplaint);

            //客訴紀錄 - 重大客訴
            var listUrgentComplaint = en.SP_PPCLIFE_BATCH_COMPLAINT(
                start,
                end,
                urgentclassID,
                null
                ).ToList();
            var OnUrgentComplaintTemp = AutoMapper.Mapper.Map<List<PPCLIFEComplaintHistory>>(listUrgentComplaint);


            //上個月時間
            var lastStart = new DateTime(start.Year, start.Month - 1, 1);
            var lastEnd = new DateTime(start.Year, start.Month, 1).AddTicks(-1);

            //客訴紀錄 - 一般客訴(上個月未結案)
            var listGeneralComplaintNotFinished = en.SP_PPCLIFE_BATCH_COMPLAINT(
                lastStart,
                lastEnd,
                generalclassID,
                1
                ).ToList();
            var OnGeneralComplaintNotFinishedTemp = AutoMapper.Mapper.Map<List<PPCLIFEComplaintHistory>>(listGeneralComplaintNotFinished);

            //客訴紀錄 - 重大客訴(上個月未結案)
            var listUrgentComplaintNotFinished = en.SP_PPCLIFE_BATCH_COMPLAINT(
                lastStart,
                lastEnd,
                urgentclassID,
                1
                ).ToList();
            var OnUrgentComplaintNotFinishedTemp = AutoMapper.Mapper.Map<List<PPCLIFEComplaintHistory>>(listUrgentComplaintNotFinished);

            //過濾出該節點的案件
            tempRecord = FilterNodeIDCase(tempRecord, nodekeyList);
            OnGeneralComplaintTemp = FilterNodeIDCase(OnGeneralComplaintTemp, nodekeyList);
            OnGeneralComplaintNotFinishedTemp = FilterNodeIDCase(OnGeneralComplaintNotFinishedTemp, nodekeyList);
            OnUrgentComplaintTemp = FilterNodeIDCase(OnUrgentComplaintTemp, nodekeyList);
            OnUrgentComplaintNotFinishedTemp = FilterNodeIDCase(OnUrgentComplaintNotFinishedTemp, nodekeyList);

            //查詢案件連絡者、以及被反應者清單
            var caseIDList = tempRecord.Select(x => x.CaseID).Distinct().ToList();
            caseIDList.AddRange(OnGeneralComplaintTemp.Select(x => x.CaseID).Distinct());
            caseIDList.AddRange(OnGeneralComplaintNotFinishedTemp.Select(x => x.CaseID).Distinct());
            caseIDList.AddRange(OnUrgentComplaintTemp.Select(x => x.CaseID).Distinct());
            caseIDList.AddRange(OnUrgentComplaintNotFinishedTemp.Select(x => x.CaseID).Distinct());
            caseIDList = caseIDList.Distinct().ToList();

            // 查詢問題分類清單
             var caseClassIDList = tempRecord.Select(x => x.ClassificationID).Distinct().ToList();
            caseClassIDList.AddRange(OnGeneralComplaintTemp.Select(x => x.ClassificationID).Distinct());
            caseClassIDList.AddRange(OnGeneralComplaintNotFinishedTemp.Select(x => x.ClassificationID).Distinct());
            caseClassIDList.AddRange(OnUrgentComplaintTemp.Select(x => x.ClassificationID).Distinct());
            caseClassIDList.AddRange(OnUrgentComplaintNotFinishedTemp.Select(x => x.ClassificationID).Distinct());
            caseClassIDList = caseClassIDList.Distinct().ToList();

            //連絡者清單
            var concat = new MSSQLCondition<CASE_CONCAT_USER>();
            concat.And(x => caseIDList.Contains(x.CASE_ID));
            var concatList = _CaseAggregate.CaseConcatUser_T1_T2_.GetList(concat).ToList();
            //被反應者清單
            var complained = new MSSQLCondition<CASE_COMPLAINED_USER>();
            complained.And(x => caseIDList.Contains(x.CASE_ID));
            var complainedList = _CaseAggregate.CaseComplainedUser_T1_T2_.GetList(complained).ToList();
            //歷程對象
            var con = new MSSQLCondition<CASE>();
            con.IncludeBy(x => x.CASE_ASSIGNMENT_COMMUNICATE);
            con.IncludeBy(x => x.CASE_ASSIGNMENT_COMMUNICATE.Select(g => g.CASE_ASSIGNMENT_COMMUNICATE_USER));
            con.IncludeBy(x => x.CASE_ASSIGNMENT_COMPLAINT_NOTICE);
            con.IncludeBy(x => x.CASE_ASSIGNMENT_COMPLAINT_NOTICE.Select(g => g.CASE_ASSIGNMENT_COMPLAINT_NOTICE_USER));
            con.IncludeBy(x => x.CASE_ASSIGNMENT_COMPLAINT_INVOICE);
            con.IncludeBy(x => x.CASE_ASSIGNMENT_COMPLAINT_INVOICE.Select(g => g.CASE_ASSIGNMENT_COMPLAINT_INVOICE_USER));
            con.IncludeBy(x => x.CASE_ASSIGNMENT);
            con.IncludeBy(x => x.CASE_ASSIGNMENT.Select(g => g.CASE_ASSIGNMENT_USER));
            con.IncludeBy(x => x.CASE_ITEM);
            con.And(x => caseIDList.Contains(x.CASE_ID));
            var caseList = _CaseAggregate.Case_T1_T2_.GetList(con).ToList();

            //分類清單
            var conclass = new MSSQLCondition<VW_QUESTION_CLASSIFICATION_NESTED>();
            conclass.And(x => caseClassIDList.Contains(x.ID));
            var classList = _MasterAggregate.VWQuestionClassification_QuestionClassification_.GetList(conclass).ToList();
            //商品
            var conitem = new MSSQLCondition<ITEM>();
            var itemIDList = caseList.SelectMany(c => c.Items.Select(y => y.ItemID));
            conitem.And(x => itemIDList.Contains(x.ID));

            var itemlist = _MasterAggregate.Item_T1_T2_Expendo_.GetList(conitem);

            foreach (var item in caseList)
            {
                var caseItem = caseList.Where(y => y.CaseID == item.CaseID).FirstOrDefault().Items;
                foreach (var j in caseItem)
                {
                    j.Item = itemlist.Where(c => c.ID == j.ItemID).FirstOrDefault();
                }
                item.Items = caseItem;
            }

            //組合資料 紀錄
            var buildlistRecord = CallHistoryToExcelFormat(tempRecord, concatList, complainedList, caseList, classList);

            var result = new PPCLIFEDataList();


            var _ReportFacades = GetReportFacade(nodekey);
            result.SummaryInformation = await _ReportFacades.GetSummaryInformationDataAsync(start, end, (header.BUID, nodekey));
            result.DailySummary = await _ReportFacades.GetSummaryDataAsync(end.AddDays(-1), end, (header.BUID, nodekey));
            result.MonthSummary = await _ReportFacades.GetSummaryDataAsync(start, end, (header.BUID, nodekey));

            result.OnCallHistory = buildlistRecord.Where(x => x.CaseSourceType == CaseSourceType.Phone).ToList();
            result.OnEmailHistory = buildlistRecord.Where(x => x.CaseSourceType == CaseSourceType.Email).ToList();

            result.OnGeneralComplaint = ComplaintHistoryToExcelFormat(OnGeneralComplaintTemp, concatList, complainedList, caseList, classList);
            result.OnGeneralComplaintNotFinished = ComplaintHistoryToExcelFormat(OnGeneralComplaintNotFinishedTemp, concatList, complainedList, caseList, classList);
            result.OnUrgentComplaint = ComplaintHistoryToExcelFormat(OnUrgentComplaintTemp, concatList, complainedList, caseList, classList);
            result.OnUrgentComplaintNotFinished = ComplaintHistoryToExcelFormat(OnUrgentComplaintNotFinishedTemp, concatList, complainedList, caseList, classList);


            return result;
        }
        /// <summary>
        /// 組合匯出來電紀錄/來信紀錄/其他紀錄
        /// </summary>
        /// <returns></returns>
        private List<PPCLIFECallHistory> CallHistoryToExcelFormat(List<PPCLIFECallHistory> tempRecord, List<CaseConcatUser> concatList, List<CaseComplainedUser> complainedUsers, List<Case> caseList, List<QuestionClassification> classList)
        {


            var result = new List<PPCLIFECallHistory>();
            foreach (var item in tempRecord)
            {
               
                var concat = concatList.Where(y => y.CaseID == item.CaseID);
                var complained = complainedUsers.Where(y => y.CaseID == item.CaseID && y.CaseComplainedUserType == CaseComplainedUserType.Responsibility);
                var caseModel = caseList.Where(y => y.CaseID == item.CaseID).FirstOrDefault();
                var assignment = caseList.Where(y => y.CaseID == item.CaseID).SelectMany(x => x.CaseAssignments.SelectMany(y => y.NoticeUsers)).ToList();
                var assignmentInvoice = caseList.Where(y => y.CaseID == item.CaseID).SelectMany(x => x.ComplaintInvoice.SelectMany(c => c.NoticeUsers)).ToList();
                var assignmentNotice = caseList.Where(y => y.CaseID == item.CaseID).SelectMany(x => x.ComplaintNotice.SelectMany(c => c.NoticeUsers)).ToList();
                var assignmentCommincate = caseList.Where(y => y.CaseID == item.CaseID).SelectMany(x => x.CaseAssignmentCommunicates.Where(t => t.NoticeUsers != null).SelectMany(c => c.NoticeUsers)).ToList();
                assignment.AddRange(assignmentInvoice);
                assignment.AddRange(assignmentNotice);
                assignment.AddRange(assignmentCommincate);

                var caseitem = caseList.Where(y => y.CaseID == item.CaseID).SelectMany(c => c.Items).ToList();

                var list = new PPCLIFECallHistory()
                {
                    CaseID = item.CaseID,
                    CaseSourceType = item.CaseSourceType,
                    Date = item.CreateDateTime.ToString("MM/dd"),
                    Time = item.CreateDateTime.ToString("HH:mm"),
                    ClassificationID = item.ClassificationID,
                    ClassList = classList.Where(x => x.ID == item.ClassificationID).FirstOrDefault(),
                    //反應品牌
                    ComplainedNodeName = complained.Count() == 0 ? "" :
                    complained.First().NodeName,
                    //姓名
                    ConcatUserName = concat.Count() == 0 ? ""
                    : concat.First().UnitType == UnitType.Customer ? concat.First().UserName + concat.First().Gender.GetDescription()
                    : concat.First().UnitType == UnitType.Organization ? concat.First().NodeName
                    : "",
                    //聯繫電話
                    ConcatMobile = concat.Count() == 0 ? ""
                    :GetMobile(concat.First()),
                    CaseContent = item.CaseContent,
                    FinishContent = item.FinishContent,
                    FinishDate = item.FinishDateTime.HasValue ? item.FinishDateTime.Value.ToString("MM/dd") : "",
                    ApplyUserName = item.ApplyUserName,
                    //轉派單位人員
                    AssignmentUserName = assignment.Count() == 0 ? "" : String.Join("\r\n", assignment.ToArray()),
                    //信件回覆時間
                    ReplyDate = caseModel.FinishReplyDateTime.HasValue ? caseModel.FinishReplyDateTime.Value.ToString("MM/dd") : "",
                    ReplyTime = caseModel.FinishReplyDateTime.HasValue ? caseModel.FinishReplyDateTime.Value.ToString("HH:mm") : "",

                };
                //商品名稱
                list = GetCaseItem(caseitem, list);

                result.Add(list);
            }


            return result;
        }
        /// <summary>
        /// 組合匯出客訴紀錄
        /// </summary>
        /// <returns></returns>
        private List<PPCLIFEComplaintHistory> ComplaintHistoryToExcelFormat(List<PPCLIFEComplaintHistory> tempComplainde, List<CaseConcatUser> concatList, List<CaseComplainedUser> complainedUsers, List<Case> caseList, List<QuestionClassification> classList)
        {
            var result = new List<PPCLIFEComplaintHistory>();
            foreach (var item in tempComplainde)
            {
                var concat = concatList.Where(y => y.CaseID == item.CaseID);
                var complained = complainedUsers.Where(y => y.CaseID == item.CaseID && y.CaseComplainedUserType == CaseComplainedUserType.Responsibility);
                var assignmentInvoiceUsers = caseList.Where(y => y.CaseID == item.CaseID).SelectMany(x => x.ComplaintInvoice.SelectMany(c => c.Users.Select(g => g.UserName))).ToList();

                var @case = caseList.FirstOrDefault(y => y.CaseID == item.CaseID);
                var caseitem = @case.Items;

                var list = new PPCLIFEComplaintHistory()
                {
                    SourceType = item.SourceType,
                    Date = item.CreateDateTime.ToString("MM/dd"),
                    Time = item.CreateDateTime.ToString("HH:mm"),
                    ClassList = classList.Where(x => x.ID == item.ClassificationID).FirstOrDefault(),
                    //反應品牌
                    ComplainedNodeName = complained.Count() == 0 ? "" :
                    complained.First().NodeName,
                    ClassificationID = item.ClassificationID,
                    CaseContent = item.CaseContent,
                    FinishContent = item.FinishContent,
                    ApplyUserName = item.ApplyUserName,
                    //轉派單位人員
                    AssignmentUserName = GetNotifyUsers(@case) // assignmentInvoiceUsers.Count() == 0 ? "" : String.Join("\r\n", assignmentInvoiceUsers.ToArray())
                };
                //商品名稱
                list = GetCaseItem(caseitem, list);

                result.Add(list);
            }
            return result;
        }
        /// <summary>
        /// 解析PPCLIFE CASE_ITEM 資料
        /// </summary>
        /// <param name="data"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private PPCLIFECallHistory GetCaseItem(List<CaseItem> data, PPCLIFECallHistory list)
        {
            var temp = data?.Select(x => x.GetParticular<PPCLIFE_CaseItem>()) ?? null;
            if (temp != null && temp.Count() > 0)
            {
                list.BatchNo = string.Join("\t\n", temp.Select(x => x.BatchNo));
                var itemList = data?.Where(x => x.Item != null).Select(c => JsonConvert.DeserializeObject<PPCLIFE_Item>(((Item<ExpandoObject>)c.Item).JContent));
                if (itemList != null)
                    list.InternationalBarcode = string.Join("\t\n", itemList.Select(c => c.InternationalBarcode));
                list.CommodityName = string.Join("\t\n", data.Where(x => x.Item != null).Select(c => c.Item.Name));
            }
            return list;
        }
        /// <summary>
        /// 解析PPCLIFE CASE_ITEM 資料
        /// </summary>
        /// <param name="data"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private PPCLIFEComplaintHistory GetCaseItem(List<CaseItem> data, PPCLIFEComplaintHistory list)
        {
            var temp = data?.Select(x => x.GetParticular<PPCLIFE_CaseItem>()) ?? null;
            if (temp != null && temp.Count() > 0)
            {
                list.BatchNo = string.Join("\t\n", temp.Select(x => x.BatchNo));
                var itemList = data?.Where(x => x.Item != null).Select(c => JsonConvert.DeserializeObject<PPCLIFE_Item>(((Item<ExpandoObject>)c.Item).JContent));
                if (itemList != null)
                    list.InternationalBarcode = string.Join("\t\n", itemList.Select(c => c.InternationalBarcode));
                list.CommodityName = string.Join("\t\n", data.Where(x => x.Item != null).Select(c => c.Item.Name));
            }
            return list;
        }

        private string GetMobile(ConcatableUser concatableUser)
        {
            List<string> list = new List<string>();

            list.Add(concatableUser.Mobile);
            list.Add(concatableUser.Telephone);
            list.Add(concatableUser.TelephoneBak);
            list.RemoveAll(x => x == null);

            string result = list.Count() == 0 ? "" : String.Join("\r\n", list.ToArray());
            return result;
        }



        /// <summary>
        /// 取得所有歷程的通知對象
        /// </summary>
        /// <param name="case"></param>
        /// <returns></returns>
        private string GetNotifyUsers(Case @case)
        {
            var assignment = @case.CaseAssignments?.SelectMany(y => y.NoticeUsers).ToList() ?? new List<string>();
            var assignmentInvoice = @case.ComplaintInvoice?.SelectMany(c => c.NoticeUsers).ToList() ?? new List<string>();
            var assignmentNotice = @case.ComplaintNotice?.SelectMany(c => c.NoticeUsers).ToList() ?? new List<string>();
            var assignmentCommincate = @case.CaseAssignmentCommunicates?.Where(x => x.NoticeUsers != null)?.SelectMany(c => c.NoticeUsers).ToList() ?? new List<string>();
            assignment.AddRange(assignmentInvoice);
            assignment.AddRange(assignmentNotice);
            assignment.AddRange(assignmentCommincate);

            return assignment.Count() == 0 ? "" : String.Join("\r\n", assignment.ToArray());
        }

        #endregion




        #region BATCH
        /// <summary>
        /// (PPCLIFE 客制)比對現有的案件與商品
        /// </summary>
        public void PPCLifeCalculateCaseRepeat(List<CaseItem> caseItems)
        {
            _CommonAggregate.Logger.Info($"【統藥大量叫修通知】  準備進行比對 比對現有的案件與商品。");

            var newCasePPCLife = base.GenerateCasePPCLife(caseItems);

            var removeCasePPCLives = new List<CasePPCLife>();
            var addCasePPCLives = new List<CasePPCLife>();

            #region 比對案件
            var oCon = new MSSQLCondition<CASE_PPCLIFE>();
            var existCasePPCLives = _CaseAggregate.CasePPCLife_T1_T2_.GetList(oCon).ToList();

            addCasePPCLives = newCasePPCLife.Where(x => !existCasePPCLives.Any(y => y.CaseID == x.CaseID && y.ItemID == x.ItemID)).ToList();
            removeCasePPCLives = existCasePPCLives.Where(x => !newCasePPCLife.Any(y => y.CaseID == x.CaseID && y.ItemID == x.ItemID)).ToList();
            #endregion


            #region 刪除至客制案件表 20200915不需刪除
            //_CommonAggregate.Logger.Info($"【統藥大量叫修通知】  準備進行資料的刪除至客制案件表。");
            //_CommonAggregate.Logger.Info($"【統藥大量叫修通知】  此次刪除  , 共 {removeCasePPCLives.Count()} 筆。");

            //removeCasePPCLives.ForEach(item =>
            //{
            //    _CaseAggregate.CasePPCLife_T1_T2_.Remove(x => x.CASE_ID == item.CaseID && x.ITEM_ID == item.ItemID);
            //});

            //_CommonAggregate.Logger.Info($"【統藥大量叫修通知】  刪除至客制案件表完畢。");
            #endregion


            #region 寫入至客制案件表
            _CommonAggregate.Logger.Info($"【統藥大量叫修通知】  準備進行資料的寫入至客制案件表。");
            _CommonAggregate.Logger.Info($"【統藥大量叫修通知】  此次寫入  , 共 {addCasePPCLives.Count()} 筆。");

            _CaseAggregate.CasePPCLife_T1_T2_.AddRange(addCasePPCLives);

            _CommonAggregate.Logger.Info($"【統藥大量叫修通知】  寫入至客制案件表完畢。");
            #endregion        

            _CommonAggregate.Logger.Info($"【統藥大量叫修通知】  比對現有的案件與商品 完畢。");
        }

        /// <summary>
        /// (PPCLIFE 客制)比對達標標的與現有標的
        /// </summary>
        public void PPCLifeCalculateSubjectRepeat(List<PPCLifeEffectiveSummary> newPPCLifeEffectiveSummaries)
        {
            #region 撈取服務統藥的客服人員

            _CommonAggregate.Logger.Info($"【統藥大量叫修通知】  準備進行撈取服務統藥的客服人員。");
            var ppcLifeID = _OrganizationAggregate.HeaderQuarterNode_T1_T2_.Get(x => x.NODE_KEY == EssentialCache.BusinessKeyValue.PPCLIFE);

            var con = new MSSQLCondition<CALLCENTER_NODE>();
            con.And(x => x.NODE_TYPE_KEY == EssentialCache.NodeDefinitionValue.Group);
            con.And(x => x.HEADQUARTERS_NODE.Any(y => y.NODE_ID == ppcLifeID.NodeID));
            con.IncludeBy(x => x.HEADQUARTERS_NODE);
            var grouplist = _OrganizationAggregate.CallCenterNode_T1_T2_.GetList(con).ToList();

            var userlist = new List<User>();
            grouplist.ForEach(async group =>
            {
                var jcon = new MSSQLCondition<NODE_JOB>(x => x.NODE_ID == group.NodeID &&
                                                             x.ORGANIZATION_TYPE == (byte)group.OrganizationType);

                jcon.IncludeBy(x => x.USER);
                jcon.IncludeBy(x => x.JOB);
                var jobs = await _OrganizationAggregate.JobPosition_T1_T2_.GetList(jcon).Async();
                userlist.AddRange(jobs.SelectMany(x => x.Users).ToList());
            });

            var userIDs = userlist.GroupBy(x => new { x.UserID }).Select(x => x.First()).ToList();

            _CommonAggregate.Logger.Info($"【統藥大量叫修通知】  此次撈取服務統藥的客服人員  , 共 {userIDs.Count()} 位。");
            _CommonAggregate.Logger.Info($"【統藥大量叫修通知】  撈取服務統藥的客服人員 完畢。");
            #endregion

            #region 比對案件
            _CommonAggregate.Logger.Info($"【統藥大量叫修通知】  準備進行比對 比對達標標的與現有標的。");

            var pCon = new MSSQLCondition<PPCLIFE_EFFECTIVE_SUMMARY>();
            var existPPCLifeEffectiveSummaries = _NotificationAggregate.PPCLifeEffectiveSummary_T1_T2_.GetList(pCon).ToList();

            var recountEffectiveSummaries = newPPCLifeEffectiveSummaries.Where(x => existPPCLifeEffectiveSummaries.Any(y => y.PPCLifeArriveType == x.PPCLifeArriveType && y.ItemID == x.ItemID && y.BatchNo == x.BatchNo)).ToList();
            var failEffectiveSummaries = existPPCLifeEffectiveSummaries.Where(x => !newPPCLifeEffectiveSummaries.Any(y => y.PPCLifeArriveType == x.PPCLifeArriveType && y.ItemID == x.ItemID && y.BatchNo == x.BatchNo)).ToList();
            var successEffectiveSummaries = newPPCLifeEffectiveSummaries.Where(x => !existPPCLifeEffectiveSummaries.Any(y => y.PPCLifeArriveType == x.PPCLifeArriveType && y.ItemID == x.ItemID && y.BatchNo == x.BatchNo)).ToList();

            _CommonAggregate.Logger.Info($"【統藥大量叫修通知】  此次達標且 原也達標  , 共 {recountEffectiveSummaries.Count()} 個。");
            _CommonAggregate.Logger.Info($"【統藥大量叫修通知】  原達標但 此次未達標 , 共 {failEffectiveSummaries.Count()} 個。");
            _CommonAggregate.Logger.Info($"【統藥大量叫修通知】  此次新達標標的  , 共 {successEffectiveSummaries.Count()} 個。");
            _CommonAggregate.Logger.Info($"【統藥大量叫修通知】  比對達標標的與現有標的 完畢。");
            #endregion

            using (var scope = new TransactionScope())
            {
                #region 原規則達標並重綁關聯     
                _CommonAggregate.Logger.Info($"【統藥大量叫修通知】  準備將 此次達標且 原也達標 重綁關聯。");

                recountEffectiveSummaries.ForEach(item =>
                {
                // 刪除達標規則
                _NotificationAggregate.PPCLifeEffectiveSummary_T1_T2_.Operator(context =>
                    {
                        var db = (SMARTIIEntities)context;
                        db.Configuration.LazyLoadingEnabled = false;

                    // 取得該規則底下案件
                    var casePPCLifies = new List<CASE_PPCLIFE>();
                        item.CasePPCLifes.ForEach(x =>
                        {
                            var casePPCLife = db.CASE_PPCLIFE
                                                .Where(y => y.CASE_ID == x.CaseID &&
                                                            y.ITEM_ID == x.ItemID).FirstOrDefault();

                            casePPCLifies.Add(casePPCLife);
                        });

                        var rule = db.PPCLIFE_EFFECTIVE_SUMMARY
                                     .Include("CASE_PPCLIFE")
                                     .Where(x => x.ARRIVE_TYPE == (byte)item.PPCLifeArriveType &&
                                                 x.ITEM_ID == item.ItemID &&
                                                 x.BATCH_NO == item.BatchNo).FirstOrDefault();

                        rule.CASE_PPCLIFE.Clear();
                        rule.CASE_PPCLIFE = casePPCLifies;

                        db.SaveChanges();
                    });

                });

                _CommonAggregate.Logger.Info($"【統藥大量叫修通知】  此次達標且 原也達標 重綁關聯 完畢。");
                #endregion

                #region 砍掉既有資料 =>通知:因資料異動，達標取消

                _CommonAggregate.Logger.Info($"【統藥大量叫修通知】  準備將 原達標但 此次未達標 砍掉既有資料 並 通知:因資料異動，達標取消。");

                var failNoticeList = new List<PersonalNotification>();

                failEffectiveSummaries.ForEach(item =>
                {
                    // 刪除達標規則
                    _NotificationAggregate.PPCLifeEffectiveSummary_T1_T2_.Operator(context =>
                        {
                            var db = (SMARTIIEntities)context;
                            db.Configuration.LazyLoadingEnabled = false;

                            var rule = db.PPCLIFE_EFFECTIVE_SUMMARY
                                         .Include("CASE_PPCLIFE")
                                         .Where(x => x.ARRIVE_TYPE == (byte)item.PPCLifeArriveType &&
                                                     x.ITEM_ID == item.ItemID &&
                                                     x.BATCH_NO == item.BatchNo).FirstOrDefault();

                            rule.CASE_PPCLIFE.Clear();

                            db.PPCLIFE_EFFECTIVE_SUMMARY.Remove(rule);
                            db.SaveChanges();
                        });

                    // 取得商品相關資訊
                    var itemDeatail = new PPCLIFE_Item();
                        itemDeatail = base.GeneratePPCLIFEItem(_MasterAggregate.Item_T1_T2_Expendo_.Get(x => x.ID == item.ItemID));

                        if (itemDeatail == null)
                        {
                            itemDeatail.InternationalBarcode = "無資訊";
                            itemDeatail.Name = "無資訊";
                        }

                        // 新增小鈴鐺通知
                        userIDs.ForEach(x =>
                        {
                            var personalNotification = new PersonalNotification()
                            {
                                UserID = x.UserID,
                                Content = string.Format(SysCommon_lang.PERSONAL_NOTIFICATION_NOTIFICATION_PPCLIFE_FAIL, item.PPCLifeArriveType.GetDescription(), itemDeatail.InternationalBarcode, itemDeatail.Name),
                                CreateDateTime = DateTime.Now,
                                CreateUserName = GlobalizationCache.APName,
                                PersonalNotificationType = PersonalNotificationType.NotificationPPCLife
                            };

                            failNoticeList.Add(personalNotification);
                        });

                });

                // 新增重算後未達標通知
                _NotificationAggregate.PersonalNotification_T1_T2_.AddRange(failNoticeList);

                _NotificationPersonalFacade.NotifyWebCollection(failNoticeList.Select(x => x.UserID).ToList());

                _CommonAggregate.Logger.Info($"【統藥大量叫修通知】  原達標但 此次未達標 砍掉既有資料 並 通知:因資料異動，達標取消 完畢。");
                #endregion

                #region 新增至既有資料 =>通知:企業別＋商品條碼+商品名稱＋已達到示警次數

                _CommonAggregate.Logger.Info($"【統藥大量叫修通知】  準備將 此次新達標標的 新增至既有資料 =>通知:企業別＋商品條碼+商品名稱＋已達到示警次數。");



                var successNoticeList = new List<PersonalNotification>();

                successEffectiveSummaries.ForEach(item =>
                {

                // 新增達標規則
                _NotificationAggregate.PPCLifeEffectiveSummary_T1_T2_.Operator(context =>
                    {
                        var db = (SMARTIIEntities)context;
                        db.Configuration.LazyLoadingEnabled = false;

                    // 取得該規則底下案件
                    var casePPCLifies = new List<CASE_PPCLIFE>();
                        item.CasePPCLifes.ForEach(x =>
                        {
                            var casePPCLife = db.CASE_PPCLIFE
                                                .Where(y => y.CASE_ID == x.CaseID &&
                                                            y.ITEM_ID == x.ItemID).FirstOrDefault();

                            casePPCLifies.Add(casePPCLife);
                        });

                        var rule = new PPCLIFE_EFFECTIVE_SUMMARY();
                        rule.ARRIVE_TYPE = (byte)item.PPCLifeArriveType;
                        rule.ITEM_ID = item.ItemID;
                        rule.BATCH_NO = item.BatchNo;
                        rule.CREATE_DATETIME = item.CreateDateTime;
                        rule.CREATE_USERNAME = item.CreateUserName;
                        rule.CASE_PPCLIFE = casePPCLifies;

                        db.PPCLIFE_EFFECTIVE_SUMMARY.Add(rule);
                        db.SaveChanges();
                    });

                // 取得商品相關資訊 
                var itemDeatail = new PPCLIFE_Item();
                    itemDeatail = base.GeneratePPCLIFEItem(_MasterAggregate.Item_T1_T2_Expendo_.Get(x => x.ID == item.ItemID));

                    if (itemDeatail == null)
                    {
                        itemDeatail.InternationalBarcode = "無資訊";
                        itemDeatail.Name = "無資訊";
                    }
                    // 新增小鈴鐺通知
                    userIDs.ForEach(x =>
                    {
                        var personalNotification = new PersonalNotification()
                        {
                            UserID = x.UserID,
                            Content = string.Format(SysCommon_lang.PERSONAL_NOTIFICATION_NOTIFICATION_PPCLIFE_SUCCESS, item.PPCLifeArriveType.GetDescription(), itemDeatail.InternationalBarcode, itemDeatail.Name, item.CasePPCLifes.Count()),
                            CreateDateTime = DateTime.Now,
                            CreateUserName = GlobalizationCache.APName,
                            PersonalNotificationType = PersonalNotificationType.NotificationPPCLife
                        }; ;

                        successNoticeList.Add(personalNotification);
                    });

                });


                // 新增達標通知
                _NotificationAggregate.PersonalNotification_T1_T2_.AddRange(successNoticeList);

                _NotificationPersonalFacade.NotifyWebCollection(successNoticeList.Select(x => x.UserID).ToList());

                _CommonAggregate.Logger.Info($"【統藥大量叫修通知】  準備將 此次新達標標的 新增至既有資料 =>通知:企業別＋商品條碼+商品名稱＋已達到示警次數 完畢。");
                #endregion
                scope.Complete();
            }
        }
        #endregion


        #region private

        /// <summary>
        /// 判斷實作
        /// </summary>
        /// <param name="nodeKey"></param>
        /// <returns></returns>
        private ReportFacade GetReportFacade(string nodeKey)
        {
           return BusinessKeyValue.PPCLIFE == nodeKey ? _ReportFacade : _PPCLifeReportFacade;
        }

        /// <summary>
        /// 過濾出該節點的案件
        /// </summary>
        /// <param name="pPCLIFEComplaintHistory"></param>
        /// <param name="nodekeyList"></param>
        /// <returns></returns>
        private List<PPCLIFEComplaintHistory> FilterNodeIDCase(List<PPCLIFEComplaintHistory> pPCLIFECallHistories, string nodekeyList)
        {
            var result = new List<PPCLIFEComplaintHistory>();

            var en = new SMARTIIEntities(DataAccessCache.Instance.SmartIIConn);
            var caseList = string.Join("','", pPCLIFECallHistories.Select(x => x.CaseID));
            var vs = en.SP_FILTER_NODE_ID_CASE(
                caseList,
                nodekeyList
                ).ToList();

            result = pPCLIFECallHistories.Where(x => vs.Contains(x.CaseID)).ToList();

            return result;
        }

        private List<PPCLIFECallHistory> FilterNodeIDCase(List<PPCLIFECallHistory> pPCLIFECallHistories, string nodekeyList)
        {
            var result = new List<PPCLIFECallHistory>();

            var en = new SMARTIIEntities(DataAccessCache.Instance.SmartIIConn);
            var caseList = string.Join("','", pPCLIFECallHistories.Select(x => x.CaseID));
            var vs = en.SP_FILTER_NODE_ID_CASE(
                caseList,
                nodekeyList
                ).ToList();

            result = pPCLIFECallHistories.Where(x => vs.Contains(x.CaseID)).ToList();

            return result;
        }

        #endregion
    }
}
