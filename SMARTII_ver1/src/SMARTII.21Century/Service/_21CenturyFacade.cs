using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII._21Century.Domain;
using SMARTII.COMMON_BU;
using SMARTII.COMMON_BU.Service;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using static SMARTII.Domain.Cache.EssentialCache;

namespace SMARTII._21Century.Service
{
    public class _21CenturyFacade
    {
        private readonly ICaseAggregate _CaseAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly IMasterAggregate _MasterAggregate;
        private readonly ReportFacade _ReportFacade;

        public _21CenturyFacade(ICaseAggregate CaseAggregate, IOrganizationAggregate OrganizationAggregate, IMasterAggregate MasterAggregate, ReportFacade ReportFacade)
        {
            _CaseAggregate = CaseAggregate;
            _OrganizationAggregate = OrganizationAggregate;
            _MasterAggregate = MasterAggregate;
            _ReportFacade = ReportFacade;
        }
        public async Task<_21CenturyDataList> GenerateOnCallExcel(DateTime start, DateTime end, string nodekey)
        {
            //取得Nodekey範圍
            var conHS = new MSSQLCondition<HEADQUARTERS_NODE>();
            conHS.And(x => x.NODE_KEY == nodekey);
            var header = _OrganizationAggregate.HeaderQuarterNode_T1_T2_.Get(conHS);

            var conHSList = new MSSQLCondition<HEADQUARTERS_NODE>();
            conHSList.And(x => x.LEFT_BOUNDARY >= header.LeftBoundary && x.RIGHT_BOUNDARY <= header.RightBoundary);
            var headerList = _OrganizationAggregate.HeaderQuarterNode_T1_T2_.GetList(conHSList);
            var nodeList = headerList.Select(x => x.NodeID).Cast<int?>();

            var pathway = headerList.Where(x => x.NodeKey == NodeKeyValue._21PATHWAY).First().NodeID;

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
            con.IncludeBy(x => x.CASE_FINISH_REASON_DATA);
            con.And(x => x.CREATE_DATETIME > start && x.CREATE_DATETIME <= end);
            con.And(x => nodeList.Contains(x.NODE_ID));
            con.And(x => x.IS_REPORT == true);

            var caseList = _CaseAggregate.Case_T1_T2_.GetList(con).ToList();

            //是否認列ID
            var conReasonClass = new MSSQLCondition<CASE_FINISH_REASON_CLASSIFICATION>();

            conReasonClass.And(x => x.KEY == ReasonClassValue.RECOGNIZE);
            var reasonClass = _MasterAggregate.CaseFinishReasonClassification_T1_T2_.Get(conReasonClass);
            int recognizeID = reasonClass.ID;

            var caseClassIDList = caseList.Select(x => x.QuestionClassificationID).Distinct().Cast<int?>();

            //分類清單
            var conclass = new MSSQLCondition<VW_QUESTION_CLASSIFICATION_NESTED>();
            conclass.And(x => caseClassIDList.Contains(x.ID));
            var classList = _MasterAggregate.VWQuestionClassification_QuestionClassification_.GetList(conclass).ToList();

            // 客訴問題分類(含子項目)
            var generalQuestion = classList.Where(x => x.FirstCode == CodeValue.GENERAL_21)?.Select(x => x.ID).ToList() ?? new List<int>();
            //有開反應單的案件

            //分割資料
            var casePathwayList = caseList.Where(x => x.CaseComplainedUsers.Any(y => y.NodeID == pathway)).ToList();

            //有開反應單的案件[INVOICE_ID] x.ComplaintInvoice != null
            var caseComplainList = caseList.Where(x => generalQuestion.Contains(x.QuestionClassificationID) ||
                                                       x.ComplaintInvoice.Any(c => c.CaseAssignmentComplaintInvoiceType != CaseAssignmentComplaintInvoiceType.Cancel)).ToList();

            var pathwayCaseID = casePathwayList.Select(y => y.CaseID);
            var complainCaseID = caseComplainList.Select(y => y.CaseID);
            //案件去除 通路紀錄、客訴紀錄案件
            caseList.RemoveAll(x => pathwayCaseID.Contains(x.CaseID));
            caseList.RemoveAll(x => complainCaseID.Contains(x.CaseID));

            //組合資料
            var combinationData = CombinationData(caseList, classList);

            var result = new _21CenturyDataList();
            result.SummaryInformation = await _ReportFacade.GetSummaryInformationDataAsync(start, end, (header.BUID, nodekey));
            result.DailySummary = await _ReportFacade.GetSummaryDataAsync(end.AddDays(-1), end, (header.BUID, nodekey));
            result.MonthSummary = await _ReportFacade.GetSummaryDataAsync(start, end, (header.BUID, nodekey));
            result.CommonOnCallsHistory = combinationData.Where(x => x.CaseSourceType == CaseSourceType.Phone).ToList();
            result.CommonOnEmailHistory = combinationData.Where(x => x.CaseSourceType == CaseSourceType.Email).ToList();
            result.CommonOthersHistory = combinationData.Where(x => x.CaseSourceType == CaseSourceType.Other).ToList();
            result.CommonPathwayHistory = CombinationPathwayData(casePathwayList, classList);
            result._21CenturyComplaintHistory = CombinationComplaintData(caseComplainList, classList, recognizeID, generalQuestion);

            return result;
        }

        /// <summary>
        /// 組合來電、來信、其他紀錄
        /// </summary>
        /// <param name="data"></param>
        /// <param name="classList"></param>
        /// <returns></returns>
        private List<CommonCallHistory> CombinationData(List<Case> data, List<QuestionClassification> classList)
        {
            var result = new List<CommonCallHistory>();

            foreach (var item in data)
            {
                var concat = item.CaseConcatUsers.Where(y => y.CaseID == item.CaseID);
                var complained = item.CaseComplainedUsers.Where(y => y.CaseID == item.CaseID && y.CaseComplainedUserType == CaseComplainedUserType.Responsibility);


                var temp = new CommonCallHistory();
                temp.CaseID = item.CaseID;
                temp.CaseSourceType = item.CaseSource.CaseSourceType;
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
                temp.ApplyUserName = item.ApplyUserName;
                //轉派單位人員
                temp.AssignmentUserName = GetNotifyUsers(item);
                //信件回覆時間
                temp.ReplyDate = item.FinishReplyDateTime.HasValue ? item.FinishReplyDateTime.Value.ToString("MM/dd") : "";
                temp.ReplyTime = item.FinishReplyDateTime.HasValue ? item.FinishReplyDateTime.Value.ToString("HH:mm") : "";


                result.Add(temp);
            }
            return result;
        }
        /// <summary>
        /// 組合通路紀錄
        /// </summary>
        /// <param name="data"></param>
        /// <param name="classList"></param>
        /// <returns></returns>
        private List<CommonPathwayHistory> CombinationPathwayData(List<Case> data, List<QuestionClassification> classList)
        {
            List<CommonPathwayHistory> result = new List<CommonPathwayHistory>();

            foreach (var item in data)
            {
                var concat = item.CaseConcatUsers.Where(y => y.CaseID == item.CaseID);
                var complained = item.CaseComplainedUsers.Where(y => y.CaseID == item.CaseID && y.CaseComplainedUserType == CaseComplainedUserType.Responsibility);


                var temp = new CommonPathwayHistory();
                temp.Date = item.CreateDateTime.ToString("MM/dd");
                temp.Time = item.CreateDateTime.ToString("HH:mm");
                temp.ClassList = classList.Where(x => x.ID == item.QuestionClassificationID).FirstOrDefault();
                //姓名
                temp.ConcatUserName = concat.Count() == 0 ? ""
                    : concat.First().UnitType == UnitType.Customer ? concat.First().UserName + concat.First().Gender.GetDescription()
                    : concat.First().UnitType == UnitType.Store ? concat.First().NodeName
                    : concat.First().UnitType == UnitType.Organization ? concat.First().NodeName
                    : "";
                //聯繫電話
                temp.ConcatMobile = concat.Count() == 0 ? ""
                    : GetMobile(concat.First());
                temp.CaseContent = item.Content;
                temp.FinishContent = item.FinishContent;
                temp.FinishDate = item.FinishDateTime.HasValue ? item.FinishDateTime.Value.ToString("MM/dd") : "";
                temp.FinishTime = item.FinishDateTime.HasValue ? item.FinishDateTime.Value.ToString("HH:mm") : "";
                temp.ApplyUserName = item.ApplyUserName;
                //轉派單位人員
                temp.AssignmentUserName = GetNotifyUsers(item);

                result.Add(temp);
            }

            return result;

        }

        /// <summary>
        /// 組合客訴紀錄
        /// </summary>
        /// <param name="data"></param>
        /// <param name="classList"></param>
        /// <returns></returns>
        private List<_21CenturyComplaintHistory> CombinationComplaintData(List<Case> data, List<QuestionClassification> classList, int recognizeID, List<int> generalQuestion)
        {
            List<_21CenturyComplaintHistory> result = new List<_21CenturyComplaintHistory>();

            foreach (var item in data)
            {
                var concat = item.CaseConcatUsers.Where(y => y.CaseID == item.CaseID);
                var complained = item.CaseComplainedUsers.Where(y => y.CaseID == item.CaseID && y.CaseComplainedUserType == CaseComplainedUserType.Responsibility);
                //只有顯示反應單轉派單位人員
                var complaintInvoice = item.ComplaintInvoice.FirstOrDefault(x => x.CaseAssignmentComplaintInvoiceType != CaseAssignmentComplaintInvoiceType.Cancel);

                if (complaintInvoice == null && !generalQuestion.Any(x => x == item.QuestionClassificationID))
                    continue;

                var temp = new _21CenturyComplaintHistory();
                temp.InvoiceID = complaintInvoice?.InvoiceID ?? "";
                temp.Source = item.CaseSource.CaseSourceType.GetDescription();
                temp.Date = item.CreateDateTime.ToString("MM/dd");
                temp.Time = item.CreateDateTime.ToString("HH:mm");
                temp.Zone = complained.Count() == 0 ? ""
                    : complained.Where(x => x.UnitType == UnitType.Store).Count() == 0 ? ""
                    : complained.Where(x => x.UnitType == UnitType.Store).First().ParentPathName;
                //反應門市
                temp.ComplainedNodeName = complained.Count() == 0 ? ""
                    : complained.Where(x => x.UnitType == UnitType.Store).Count() == 0 ? ""
                    : complained.Where(x => x.UnitType == UnitType.Store).First().NodeName;

                temp.ClassList = classList.Where(x => x.ID == item.QuestionClassificationID).FirstOrDefault();
                //姓名
                temp.ConcatUserName = concat.Count() == 0 ? ""
                    : concat.First().UnitType == UnitType.Customer ? concat.First().UserName + concat.First().Gender.GetDescription()
                    : concat.First().UnitType == UnitType.Store ? concat.First().NodeName
                    : concat.First().UnitType == UnitType.Organization ? concat.First().NodeName
                    : "";
                //聯繫電話
                temp.ConcatMobile = concat.Count() == 0 ? ""
                    : GetMobile(concat.First());
                temp.CaseContent = item.Content;
                temp.FinishContent = item.FinishContent;
                temp.ApplyUserName = item.ApplyUserName;
                //轉派單位人員
                temp.AssignmentUserName = GetNotifyUsers(item);
                temp.InvoiceCreateDate = complaintInvoice?.CreateDateTime.ToString("MM/dd") ?? "";
                temp.IsRecognize = item.CaseFinishReasonDatas.Where(x => x.ClassificationID == recognizeID).Any() ?
                        item.CaseFinishReasonDatas.Where(x => x.ClassificationID == recognizeID).First().Text
                        : ""; ;

                result.Add(temp);
            }

            return result.OrderBy(x => x.InvoiceID).ToList();
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

    }
}
