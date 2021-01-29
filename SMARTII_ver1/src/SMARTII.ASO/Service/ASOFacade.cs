using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.ASO.Domain;
using SMARTII.COMMON_BU.Service;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Report;
using static SMARTII.Domain.Cache.EssentialCache;

namespace SMARTII.ASO.Service
{
    public class ASOFacade
    {
        private readonly ICaseAggregate _CaseAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly IMasterAggregate _MasterAggregate;
        private readonly ReportFacade _ReportFacade;

        public ASOFacade(ICaseAggregate CaseAggregate, IOrganizationAggregate OrganizationAggregate, IMasterAggregate MasterAggregate, ReportFacade ReportFacade)
        {
            _CaseAggregate = CaseAggregate;
            _OrganizationAggregate = OrganizationAggregate;
            _MasterAggregate = MasterAggregate;
            _ReportFacade = ReportFacade;
        }
        /// <summary>
        /// ASO 來電紀錄
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="nodekey"></param>
        /// <returns></returns>
        public async Task<ASODataList> GenerateOnCallExcel(DateTime start, DateTime end, string nodekey)
        {
            //取得Nodekey範圍
            var conHS = new MSSQLCondition<HEADQUARTERS_NODE>();
            conHS.And(x => x.NODE_KEY == nodekey);
            var header = _OrganizationAggregate.HeaderQuarterNode_T1_T2_.Get(conHS);

            var conHSList = new MSSQLCondition<HEADQUARTERS_NODE>();
            conHSList.And(x => x.LEFT_BOUNDARY >= header.LeftBoundary && x.RIGHT_BOUNDARY <= header.RightBoundary);
            var headerList = _OrganizationAggregate.HeaderQuarterNode_T1_T2_.GetList(conHSList).ToList();
            var nodeList = headerList.Select(x => x.NodeID).Cast<int?>();

            //取得案件
            var con = new MSSQLCondition<CASE>();
            con.IncludeBy(x => x.CASE_SOURCE);
            con.IncludeBy(x => x.CASE_ASSIGNMENT_COMMUNICATE);
            con.IncludeBy(x => x.CASE_ASSIGNMENT_COMMUNICATE.Select(g => g.CASE_ASSIGNMENT_COMMUNICATE_USER));
            con.IncludeBy(x => x.CASE_ASSIGNMENT_COMPLAINT_NOTICE);
            con.IncludeBy(x => x.CASE_ASSIGNMENT_COMPLAINT_NOTICE.Select(g => g.CASE_ASSIGNMENT_COMPLAINT_NOTICE_USER));
            con.IncludeBy(x => x.CASE_ASSIGNMENT_COMPLAINT_INVOICE);
            con.IncludeBy(x => x.CASE_ASSIGNMENT_COMPLAINT_INVOICE.Select(g => g.CASE_ASSIGNMENT_COMPLAINT_INVOICE_USER));
            con.IncludeBy(x => x.CASE_ASSIGNMENT);
            con.IncludeBy(x => x.CASE_ASSIGNMENT.Select(g => g.CASE_ASSIGNMENT_USER));
            con.IncludeBy(x => x.CASE_CONCAT_USER);
            con.IncludeBy(x => x.CASE_COMPLAINED_USER);
            con.And(x => x.CREATE_DATETIME > start && x.CREATE_DATETIME <= end);
            con.And(x => nodeList.Contains(x.NODE_ID));
            con.And(x => x.IS_REPORT == true);

            var caseList = _CaseAggregate.Case_T1_T2_.GetList(con).ToList();

            var caseClassIDList = caseList.Select(x => x.QuestionClassificationID).Distinct().Cast<int?>();

            //分類清單
            var conclass = new MSSQLCondition<VW_QUESTION_CLASSIFICATION_NESTED>();
            conclass.And(x => caseClassIDList.Contains(x.ID));
            var classList = _MasterAggregate.VWQuestionClassification_QuestionClassification_.GetList(conclass).ToList();

            //組合資料
            var combinationData = CombinationData(caseList, classList);

            var result = new ASODataList();
            result.SummaryInformation = await _ReportFacade.GetSummaryInformationDataAsync(start, end, (header.BUID, nodekey));
            result.DailySummary = await _ReportFacade.GetSummaryDataAsync(end.AddDays(-1), end, (header.BUID, nodekey));
            result.MonthSummary = await _ReportFacade.GetSummaryDataAsync(start, end, (header.BUID, nodekey));
            result.OnCallHistory = combinationData.Where(x => x.CaseSourceType == CaseSourceType.Phone).ToList();
            result.OnEmailHistory = combinationData.Where(x => x.CaseSourceType == CaseSourceType.Email).ToList();
            result.OnOtherHistory = combinationData.Where(x => x.CaseSourceType == CaseSourceType.Other).ToList();
            result.OnStoreHistory = combinationData.Where(x => x.CaseSourceType == CaseSourceType.StoreEmail).ToList();

            return result;
        }
        /// <summary>
        /// ASO 來電紀錄 - 組合來電、來信、其他紀錄
        /// </summary>
        /// <param name="data"></param>
        /// <param name="classList"></param>
        /// <returns></returns>
        private List<ASOCallHistory> CombinationData(List<Case> data, List<QuestionClassification> classList)
        {
            var result = new List<ASOCallHistory>();

            foreach (var item in data)
            {
                var concat = item.CaseConcatUsers.Where(y => y.CaseID == item.CaseID);
                var complained = item.CaseComplainedUsers.Where(y => y.CaseID == item.CaseID && y.CaseComplainedUserType == CaseComplainedUserType.Responsibility);
                var assignment = item.CaseAssignments.SelectMany(y => y.NoticeUsers).ToList();
                var assignmentInvoice = item.ComplaintInvoice.SelectMany(c => c.NoticeUsers).ToList();
                var assignmentNotice = item.ComplaintNotice.SelectMany(c => c.NoticeUsers).ToList();
                var assignmentCommincate = item.CaseAssignmentCommunicates.Where(x => x.NoticeUsers != null).SelectMany(c => c.NoticeUsers).ToList();
                assignment.AddRange(assignmentInvoice);
                assignment.AddRange(assignmentNotice);
                assignment.AddRange(assignmentCommincate);

                var temp = new ASOCallHistory();

                temp.CaseSourceType = item.CaseSource.CaseSourceType;
                temp.Unit = complained.Count() == 0 ? ""
                    : complained.First().NodeName;
                temp.CaseID = item.CaseID;
                temp.Date = item.CreateDateTime.ToString("MM/dd");
                temp.Time = item.CreateDateTime.ToString("HH:mm");
                temp.ClassList = classList.Where(x => x.ID == item.QuestionClassificationID).FirstOrDefault();
                //反應門市
                temp.ComplainedNodeName = complained.Count() == 0 ? ""
                    : complained.Where(x => x.UnitType == UnitType.Store).Count() == 0 ? ""
                    : complained.Where(x => x.UnitType == UnitType.Store).First().NodeName;
                //姓名
                temp.ConcatUserName = concat.Count() == 0 ? ""
                    : concat.First().UnitType == UnitType.Customer ? concat.First().UserName + concat.First().Gender.GetDescription()
                    : concat.First().UnitType == UnitType.Store ? concat.First().NodeName
                    : concat.First().UnitType == UnitType.Organization ? concat.First().NodeName
                    : "";
                //聯繫電話
                temp.ConcatMobile = concat.Count() == 0 ? ""
                    : GetMobile(concat.First());
                temp.IsInvoice = "";
                temp.CaseContent = item.Content;
                temp.FinishContent = item.FinishContent;
                temp.FinishDate = item.FinishDateTime.HasValue ? item.FinishDateTime.Value.ToString("MM/dd") : "";
                temp.FinishTime = item.FinishDateTime.HasValue ? item.FinishDateTime.Value.ToString("HH:mm") : "";
                temp.ApplyUserName = item.ApplyUserName;
                //轉派單位人員
                temp.AssignmentUserName = assignment.Count() == 0 ? "" : String.Join("\r\n", assignment.ToArray());
                //信件回覆時間
                temp.ReplyDate = item.FinishReplyDateTime.HasValue ? item.FinishReplyDateTime.Value.ToString("MM/dd") : "";
                temp.ReplyTime = item.FinishReplyDateTime.HasValue ? item.FinishReplyDateTime.Value.ToString("HH:mm") : "";
                temp.CaseType = item.CaseType.GetDescription();

                result.Add(temp);
            }
            return result;
        }

        /// <summary>
        /// ASO 來電紀錄(時效)
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="nodekey"></param>
        /// <returns></returns>
        public async Task<ASOAssignmentDataList> GenerateAssignmentOnCallExcel(DateTime start, DateTime end, string nodekey)
        {
            //取得Nodekey範圍
            var conHS = new MSSQLCondition<HEADQUARTERS_NODE>();
            conHS.And(x => x.NODE_KEY == nodekey);
            var header = _OrganizationAggregate.HeaderQuarterNode_T1_T2_.Get(conHS);

            var conHSList = new MSSQLCondition<HEADQUARTERS_NODE>();
            conHSList.And(x => x.LEFT_BOUNDARY >= header.LeftBoundary && x.RIGHT_BOUNDARY <= header.RightBoundary);
            var headerList = _OrganizationAggregate.HeaderQuarterNode_T1_T2_.GetList(conHSList).ToList();
            var nodeList = headerList.Select(x => x.NodeID).Cast<int?>();

            //取得案件
            var con = new MSSQLCondition<CASE>();
            con.IncludeBy(x => x.CASE_FINISH_REASON_DATA);
            con.IncludeBy(x => x.CASE_SOURCE);
            con.IncludeBy(x => x.CASE_ASSIGNMENT);
            con.IncludeBy(x => x.CASE_ASSIGNMENT.Select(g => g.CASE_ASSIGNMENT_USER));
            con.IncludeBy(x => x.CASE_CONCAT_USER);
            con.IncludeBy(x => x.CASE_COMPLAINED_USER);
            con.And(x => x.CREATE_DATETIME > start && x.CREATE_DATETIME <= end);
            con.And(x => x.IS_REPORT == true);
            con.And(x => nodeList.Contains(x.NODE_ID));

            var caseList = _CaseAggregate.Case_T1_T2_.GetList(con).ToList();

            var caseClassIDList = caseList.Select(x => x.QuestionClassificationID).Distinct().Cast<int?>();

            var caseIDList = caseList.Select(x => x.CaseID);
            //取得回覆內容清單
            var assignmentResumeList = _CaseAggregate.CaseAssignmentResume_T1_T2_
                                         .GetList(x => caseIDList.Contains(x.CASE_ID))
                                         .OrderBy(x => x.CreateDateTime).ToList();
            //滿意度ID
            var conReasonClass = new MSSQLCondition<CASE_FINISH_REASON_CLASSIFICATION>();
            conReasonClass.And(x => x.KEY == ReasonClassValue.SATISFACTION);
            var reasonClass = _MasterAggregate.CaseFinishReasonClassification_T1_T2_.Get(conReasonClass);
            int satisfactionID = reasonClass.ID;

            //分類清單
            var conclass = new MSSQLCondition<VW_QUESTION_CLASSIFICATION_NESTED>();
            conclass.And(x => caseClassIDList.Contains(x.ID));
            var classList = _MasterAggregate.VWQuestionClassification_QuestionClassification_.GetList(conclass).ToList();

            //組合資料
            var combinationData = CombinationAssignmentData(caseList, classList, assignmentResumeList, satisfactionID);

            var result = new ASOAssignmentDataList();
            result.SummaryInformation = await _ReportFacade.GetSummaryInformationDataAsync(start, end, (header.BUID, nodekey));
            result.DailySummary = await _ReportFacade.GetSummaryDataAsync(end.AddDays(-1), end, (header.BUID, nodekey));
            result.MonthSummary = await _ReportFacade.GetSummaryDataAsync(start, end, (header.BUID, nodekey));
            result.OnCallHistory = combinationData.Where(x => x.CaseSourceType == CaseSourceType.Phone).ToList();
            result.OnEmailHistory = combinationData.Where(x => x.CaseSourceType == CaseSourceType.Email).ToList();
            result.OnOtherHistory = combinationData.Where(x => x.CaseSourceType == CaseSourceType.Other).ToList();
            result.OnStoreHistory = combinationData.Where(x => x.CaseSourceType == CaseSourceType.StoreEmail).ToList();


            return result;
        }

        /// <summary>
        /// ASO 來電紀錄(時效)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="classList"></param>
        /// <returns></returns>
        private List<ASOAssignmentCallHistory> CombinationAssignmentData(List<Case> data, List<QuestionClassification> classList, List<CaseAssignmentResume> assignmentResumeList, int satisfactionID)
        {
            var result = new List<ASOAssignmentCallHistory>();

            foreach (var @case in data)
            {
                var concat = @case.CaseConcatUsers;
                var complained = @case.CaseComplainedUsers.Where(y => y.CaseComplainedUserType == CaseComplainedUserType.Responsibility);

                var temp = new ASOAssignmentCallHistory();

                temp.CaseSourceType = @case.CaseSource.CaseSourceType;
                temp.Unit = complained.Count() == 0 ? ""
                    : complained.First().NodeName;
                temp.CaseID = @case.CaseID;
                temp.Date = @case.CreateDateTime.ToString("MM/dd");
                temp.Time = @case.CreateDateTime.ToString("HH:mm");
                temp.ClassList = classList.Where(x => x.ID == @case.QuestionClassificationID).FirstOrDefault();

                temp.ComplainParent = complained.Count() == 0 ? ""
                    : complained.Where(x => x.UnitType == UnitType.Store).Count() == 0 ? ""
                    : complained.Where(x => x.UnitType == UnitType.Store).First().ParentPathName;
                //反應門市
                temp.ComplainedNodeName = complained.Count() == 0 ? ""
                    : complained.Where(x => x.UnitType == UnitType.Store).Count() == 0 ? ""
                    : complained.Where(x => x.UnitType == UnitType.Store).First().NodeName;
                //姓名
                temp.ConcatUserName = concat.Count() == 0 ? ""
                    : concat.First().UnitType == UnitType.Customer ? concat.First().UserName + concat.First().Gender.GetDescription()
                    : concat.First().UnitType == UnitType.Store ? concat.First().NodeName
                    : concat.First().UnitType == UnitType.Organization ? concat.First().NodeName
                    : "";
                //聯繫電話
                temp.ConcatMobile = concat.Count() == 0 ? ""
                    : GetMobile(concat.First());
                temp.IsInvoice = "";
                temp.CaseContent = @case.Content;
                temp.FinishContent = @case.FinishContent;
                //處理人員
                temp.ApplyUserName = @case.ApplyUserName;
                temp.FinishDate = @case.FinishDateTime.HasValue ? @case.FinishDateTime.Value.ToString("MM/dd") : "";
                temp.FinishTime = @case.FinishDateTime.HasValue ? @case.FinishDateTime.Value.ToString("HH:mm") : "";
                //信件回覆時間
                temp.ReplyDate = @case.FinishReplyDateTime.HasValue ? @case.FinishReplyDateTime.Value.ToString("MM/dd") : "";
                temp.ReplyTime = @case.FinishReplyDateTime.HasValue ? @case.FinishReplyDateTime.Value.ToString("HH:mm") : "";
                temp.CaseType = @case.CaseType.GetDescription();

                temp.CreatDateTime = @case.CreateDateTime.ToString("yyyy/MM/dd HH:mm:ss");

                //非轉派案件處理時間
                if (@case.FinishDateTime.HasValue)
                    temp.WithoutAssignmentProcessDateTime = ToCustomString(@case.FinishDateTime.Value - @case.CreateDateTime);

                temp.CaseFinishDateTime = @case.FinishDateTime.HasValue ? @case.FinishDateTime.Value.ToString("yyyy/MM/dd HH:mm:ss") : "";

                temp.Satisfaction = @case.CaseFinishReasonDatas.Where(x => x.ClassificationID == satisfactionID).Any() ?
                    @case.CaseFinishReasonDatas.Where(x => x.ClassificationID == satisfactionID).First().Text
                    : "";

                // 若有轉派則攤平寫入，反之則單筆寫入即可
                if (@case.CaseAssignments != null && @case.CaseAssignments.Count > 0)
                {
                    foreach (var item in @case.CaseAssignments.ToList())
                    {
                        var tempResult = temp.Clone();

                        #region 轉派    
                        var assignment = item.CaseAssignmentUsers.Select(y => y.UserName).ToList();
                        //轉派單位人員
                        tempResult.AssignmentUserName = assignment.Count() == 0 ? "" : String.Join("\r\n", assignment.ToArray());

                        tempResult.AssignmentCreatDateTime = item.CreateDateTime.ToString("yyyy/MM/dd HH:mm:ss");
                        tempResult.AssignmentContent = item.Content;
                        tempResult.AssignmentFinishDateTime = item.FinishDateTime.HasValue ? item.FinishDateTime.Value.ToString("yyyy/MM/dd HH:mm:ss") : "";
                        //轉派回覆內容
                        tempResult.AssignmentResume = ResumeRetryContent(assignmentResumeList.Where(y => y.CaseAssignmentID == item.AssignmentID && y.CaseID == item.CaseID).ToList());
                        if (item.FinishDateTime.HasValue)
                            tempResult.AssignmentProcessDateTime = ToCustomString(item.FinishDateTime.Value - item.CreateDateTime);
                        #endregion                        

                        result.Add(tempResult);
                    }
                }
                else
                {
                    temp.AssignmentCreatDateTime = string.Empty;
                    temp.AssignmentContent = string.Empty;
                    temp.AssignmentFinishDateTime = string.Empty;
                    temp.AssignmentResume = string.Empty;
                    temp.AssignmentProcessDateTime = string.Empty;

                    result.Add(temp);
                }
            }
            return result;
        }
        
        /// <summary>
        /// ASO客服日報
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public async Task<List<DailyReport>> GenerateEveryDayExcel(DateTime start, DateTime end, string nodekey)
        {

            //取得Nodekey範圍
            var conHS = new MSSQLCondition<HEADQUARTERS_NODE>();
            conHS.And(x => x.NODE_KEY == nodekey);
            var header = _OrganizationAggregate.HeaderQuarterNode_T1_IOrganizationNode_.Get(conHS);

            var conHSList = new MSSQLCondition<HEADQUARTERS_NODE>();
            conHSList.And(x => x.LEFT_BOUNDARY >= header.LeftBoundary && x.RIGHT_BOUNDARY <= header.RightBoundary);
            var headerList = _OrganizationAggregate.HeaderQuarterNode_T1_T2_.GetList(conHSList).ToList();
            var nodeList = headerList.Select(x => x.NodeID).Cast<int?>();

            //取得案件
            var con = new MSSQLCondition<CASE>();
            con.And(x => x.IS_REPORT == true);
            con.And(x => nodeList.Contains(x.NODE_ID));
            con.And(x => x.CREATE_DATETIME > start && x.CREATE_DATETIME <= end);
            con.And(x => x.QUESION_CLASSIFICATION_ID != 0);
            con.IncludeBy(x => x.CASE_SOURCE);


            var caseList = _CaseAggregate.Case_T1_T2_.GetList(con).ToList();

            //分類清單

            var caseClassIDList = caseList.Select(x => x.QuestionClassificationID).Distinct().Cast<int?>();
            var conclass = new MSSQLCondition<VW_QUESTION_CLASSIFICATION_NESTED>();
            conclass.And(x => caseClassIDList.Contains(x.ID));
            var classList = _MasterAggregate.VWQuestionClassification_QuestionClassification_.GetList(conclass).ToList();

            var result = new List<DailyReport>();

            //組合資料
            foreach (var item in caseList)
            {
                var pathArray = classList.Where(x => x.ID == item.QuestionClassificationID).FirstOrDefault().ParentNamePathByArray;

                var temp = new DailyReport();
                temp.CaseSourceType = item.CaseSource.CaseSourceType;
                temp.ClassList = classList.Where(x => x.ID == item.QuestionClassificationID).FirstOrDefault();
                temp.CreateDateTime = item.CreateDateTime;
                temp.ParentName = pathArray[0];
                temp.ChildrenName = pathArray[pathArray.Count() - 1];

                result.Add(temp);
            }

            return result;
        }
        /// <summary>
        /// 轉派案件處理時間 轉字串
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        private string ToCustomString(TimeSpan span)
        {
            return string.Format("{0:00}時{1:00}分{2:00}秒", (span.Hours + span.Days * 24), span.Minutes, span.Seconds);
        }
        /// <summary>
        /// 回覆內容
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string ResumeRetryContent(List<CaseAssignmentResume> data)
        {
            string RetryContent = "";
            if (data != null && data.Count != 0)
            {
                foreach (var y in data)
                {
                    RetryContent += "*********************************\r\n";
                    RetryContent += "回覆單位:" + y.CreateNodeName + "\r\n" + "回覆內容:\r\n" + y.Content + "\r\n" + "回覆時間:" + y.CreateDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    RetryContent += "\t\n";
                };
            }
            return RetryContent;
        }

        /// <summary>
        /// 聯繫電話
        /// </summary>
        /// <param name="concatableUser"></param>
        /// <returns></returns>
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
    }
}
