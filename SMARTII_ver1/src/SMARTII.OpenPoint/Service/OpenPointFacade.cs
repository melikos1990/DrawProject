using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.COMMON_BU.Service;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using SMARTII.OpenPoint.Domain;
using static SMARTII.Domain.Cache.EssentialCache;

namespace SMARTII.OpenPoint.Service
{
    public class OpenPointFacade
    {
        private readonly ICaseAggregate _CaseAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly IMasterAggregate _MasterAggregate;
        private readonly ReportFacade _ReportFacade;

        public OpenPointFacade(ICaseAggregate CaseAggregate, IOrganizationAggregate OrganizationAggregate, IMasterAggregate MasterAggregate, ReportFacade ReportFacade)
        {
            _CaseAggregate = CaseAggregate;
            _OrganizationAggregate = OrganizationAggregate;
            _MasterAggregate = MasterAggregate;
            _ReportFacade = ReportFacade;
        }
        /// <summary>
        /// OpenPoint 來電紀錄
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="nodeKey"></param>
        /// <returns></returns>
        public async Task<OpenPointDataList> GetOnCallExcel(DateTime start, DateTime end, string nodekey)
        {
            //取得Nodekey範圍
            var conHS = new MSSQLCondition<HEADQUARTERS_NODE>();
            conHS.And(x => x.NODE_KEY == nodekey);
            var header = _OrganizationAggregate.HeaderQuarterNode_T1_T2_.Get(conHS);

            var conHSList = new MSSQLCondition<HEADQUARTERS_NODE>();
            conHSList.And(x => x.LEFT_BOUNDARY >= header.LeftBoundary && x.RIGHT_BOUNDARY <= header.RightBoundary);
            var headerList = _OrganizationAggregate.HeaderQuarterNode_T1_T2_.GetList(conHSList);
            var nodeList = headerList.Select(x => x.NodeID).Cast<int?>();

            //取得案件
            var con = new MSSQLCondition<CASE>();
            con.And(x => nodeList.Contains(x.NODE_ID));
            con.And(x => x.IS_REPORT == true);
            con.And(x => x.CREATE_DATETIME > start && x.CREATE_DATETIME <= end);
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

            var caseList = _CaseAggregate.Case_T1_T2_.GetList(con).ToList();

            var caseClassIDList = caseList.Select(x => x.QuestionClassificationID).Distinct().Cast<int?>();

            //分類清單
            var conclass = new MSSQLCondition<VW_QUESTION_CLASSIFICATION_NESTED>();
            conclass.And(x => caseClassIDList.Contains(x.ID));
            var classList = _MasterAggregate.VWQuestionClassification_QuestionClassification_.GetList(conclass).ToList();

            //組合資料
            var combinationData = CombinationData(caseList, classList);

            var result = new OpenPointDataList();
            result.SummaryInformation = await _ReportFacade.GetSummaryInformationDataAsync(start, end, (header.BUID, nodekey));
            result.DailySummary = await _ReportFacade.GetSummaryDataAsync(end.AddDays(-1), end, (header.BUID, nodekey));
            result.MonthSummary = await _ReportFacade.GetSummaryDataAsync(start, end, (header.BUID, nodekey));
            result.CommonOnCallsHistory = combinationData.Where(x => x.CaseSourceType == CaseSourceType.Phone).ToList();
            result.CommonOnEmailHistory = combinationData.Where(x => x.CaseSourceType == CaseSourceType.Email).ToList();
            result.CommonOnOtherHistory = combinationData.Where(x => x.CaseSourceType == CaseSourceType.Other).ToList();


            return result;
        }

        /// <summary>
        /// 組合來電、來信、其他紀錄
        /// </summary>
        /// <param name="data"></param>
        /// <param name="classList"></param>
        /// <returns></returns>
        private List<OpenPointCallHistory> CombinationData(List<Case> data, List<QuestionClassification> classList)
        {
            var result = new List<OpenPointCallHistory>();

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

                var temp = new OpenPointCallHistory();

                temp.CaseSourceType = item.CaseSource.CaseSourceType;
                temp.CaseID = item.CaseID;
                temp.Date = item.CreateDateTime.ToString("yyyy/MM/dd");
                temp.Time = item.CreateDateTime.ToString("HH:mm");
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
                temp.EMail = concat.Count() == 0 ? "" : concat.First().Email;
                temp.CaseContent = item.Content;
                temp.FinishContent = item.FinishContent;
                temp.ApplyUserName = item.ApplyUserName;
                //信件回復時間(來信)
                temp.FinishReplyDateTime = item.FinishReplyDateTime.HasValue ? item.FinishReplyDateTime.Value.ToString("MM/dd HH:mm") : "";
                //案件結案時間(來信/其他)
                temp.FinishDateTime = item.FinishDateTime.HasValue ? item.FinishDateTime.Value.ToString("MM/dd HH:mm") : "";
                //轉派單位人員
                temp.AssignmentUserName = assignment.Count() == 0 ? "" : String.Join("/", assignment.ToArray());

                temp.ClassList = classList.Where(x => x.ID == item.QuestionClassificationID).FirstOrDefault();
                temp.IncomingDateTime = item.CaseSource.IncomingDateTime.HasValue ? item.CaseSource.IncomingDateTime.Value.ToString("yyyy/MM/dd HH:mm") : "";

                result.Add(temp);
            }
            return result;
        }
    }
}
