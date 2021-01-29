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
using SMARTII.MisterDonut.Domain;
using static SMARTII.Domain.Cache.EssentialCache;

namespace SMARTII.MisterDonut.Service
{
    public class MisterDonutFacade
    {
        private readonly ICaseAggregate _CaseAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly IMasterAggregate _MasterAggregate;
        private readonly ReportFacade _ReportFacade;

        public MisterDonutFacade(ICaseAggregate CaseAggregate, IOrganizationAggregate OrganizationAggregate, IMasterAggregate MasterAggregate, ReportFacade ReportFacade)
        {
            _CaseAggregate = CaseAggregate;
            _OrganizationAggregate = OrganizationAggregate;
            _MasterAggregate = MasterAggregate;
            _ReportFacade = ReportFacade;
        }
        /// <summary>
        /// MisterDonut 來電紀錄
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="nodeKey"></param>
        /// <returns></returns>
        public async Task<MisterDonutDataList> GetOnCallExcel(DateTime start, DateTime end, string nodekey)
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
            con.And(x => x.CREATE_DATETIME > start && x.CREATE_DATETIME <= end);
            con.And(x => x.IS_REPORT == true);
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

            //分割資料

            var caseComplainList = caseList.Where(x => x.ComplaintInvoice.Any(c => c.CaseAssignmentComplaintInvoiceType != CaseAssignmentComplaintInvoiceType.Cancel)).ToList();


            var complainCaseID = caseComplainList.Select(y => y.CaseID);
            //案件去除 客訴紀錄案件
            caseList.RemoveAll(x => complainCaseID.Contains(x.CaseID));

            //組合資料
            var combinationData = CombinationData(caseList, classList);



            var result = new MisterDonutDataList();
            result.SummaryInformation = await _ReportFacade.GetSummaryInformationDataAsync(start, end, (header.BUID, nodekey));
            result.DailySummary = await _ReportFacade.GetSummaryDataAsync(end.AddDays(-1), end, (header.BUID, nodekey));
            result.MonthSummary = await _ReportFacade.GetSummaryDataAsync(start, end, (header.BUID, nodekey));
            result.CommonOnCallsHistory = combinationData.Where(x => x.CaseSourceType == CaseSourceType.Phone).ToList();
            result.CommonOnEmailHistory = combinationData.Where(x => x.CaseSourceType == CaseSourceType.Email).ToList();
            result.CommonOthersHistory = combinationData.Where(x => x.CaseSourceType == CaseSourceType.Other).ToList();
            result.MisterDonutComplaintHistory = CombinationComplaintData(caseComplainList, classList);

            return result;
        }

        /// <summary>
        /// 組合來電、來信、其他紀錄
        /// </summary>
        /// <param name="data"></param>
        /// <param name="classList"></param>
        /// <returns></returns>
        private List<MisterDonutCallHistory> CombinationData(List<Case> data, List<QuestionClassification> classList)
        {
            var result = new List<MisterDonutCallHistory>();

            foreach (var item in data)
            {
                var concat = item.CaseConcatUsers.Where(y => y.CaseID == item.CaseID);
                var complained = item.CaseComplainedUsers.Where(y => y.CaseID == item.CaseID && y.CaseComplainedUserType == CaseComplainedUserType.Responsibility);

                var temp = new MisterDonutCallHistory();

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
        /// 組合客訴紀錄
        /// </summary>
        /// <param name="data"></param>
        /// <param name="classList"></param>
        /// <returns></returns>
        private List<MisterDonutComplaintHistory> CombinationComplaintData(List<Case> data, List<QuestionClassification> classList)
        {
            List<MisterDonutComplaintHistory> result = new List<MisterDonutComplaintHistory>();

            foreach (var item in data)
            {
                var concat = item.CaseConcatUsers.Where(y => y.CaseID == item.CaseID);
                var invoiceNotifyUsers = item.ComplaintInvoice?.SelectMany(x => x.NoticeUsers)?.Distinct().ToList() ?? new List<string>();
                var complained = item.CaseComplainedUsers.Where(y => y.CaseID == item.CaseID && y.CaseComplainedUserType == CaseComplainedUserType.Responsibility);

                var temp = new MisterDonutComplaintHistory();
                temp.InvoiceID = item.ComplaintInvoice.FirstOrDefault(x => x.CaseAssignmentComplaintInvoiceType != CaseAssignmentComplaintInvoiceType.Cancel)?.InvoiceID;
                temp.Source = item.CaseSource.CaseSourceType.GetDescription();
                temp.Date = item.CreateDateTime.ToString("MM/dd");
                temp.Time = item.CreateDateTime.ToString("HH:mm");
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
                temp.AssignmentUserName = invoiceNotifyUsers.Any() ? string.Join("\r\n", invoiceNotifyUsers) : "";
                temp.InvoiceCreateDate = item.ComplaintInvoice.FirstOrDefault(x => x.CaseAssignmentComplaintInvoiceType != CaseAssignmentComplaintInvoiceType.Cancel)?.CreateDateTime.ToString("MM/dd");

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
