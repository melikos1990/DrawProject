using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;
using SMARTII.Domain.DI;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Report;

namespace SMARTII.Service.Case.Parser
{
    public class CaseAssignmentCallCenterCommuncateData : ICaseAssignmentCallCenterIntegrationData
    {
        private readonly IMasterAggregate _MasterAggregate;
        private readonly IIndex<string, IExcelParser> _ExcelParser;

        public CaseAssignmentCallCenterCommuncateData(IMasterAggregate MasterAggregate, IIndex<string, IExcelParser> ExcelParser)
        {
            _MasterAggregate = MasterAggregate;
            _ExcelParser = ExcelParser;
        }

        public List<ExcelCaseAssignmentList> DataToDomainModel(List<SP_GetCaseAssignmentList> data, CaseAssignmentCallCenterCondition condition)
        {
            var temp = new List<SP_GetCaseAssignmentList>();
            var result = new List<ExcelCaseAssignmentList>();
            //取出所有分類
            var con = new MSSQLCondition<VW_QUESTION_CLASSIFICATION_NESTED>();
            var dataClassList = data.Select(x => x.ClassificationID).ToArray();
            con.And(x => dataClassList.Contains(x.ID));
            var classList = _MasterAggregate.VWQuestionClassification_QuestionClassification_.GetList(con).ToList();

            foreach (var item in data)
            {
                var concat = item.CaseConcatUsersList == null ? null : item.CaseConcatUsersList.First();
                var complained = item.CaseComplainedUsersList == null ?
                    null : item.CaseComplainedUsersList.Where(x => x.CaseComplainedUserType == CaseComplainedUserType.Responsibility).Count() == 0
                    ? null : item.CaseComplainedUsersList.Where(x => x.CaseComplainedUserType == CaseComplainedUserType.Responsibility).First();
                var ui = new ExcelCaseAssignmentList()
                {
                    CaseAssignmentProcessType = CaseAssignmentProcessType.Communication,
                    IdentityID = item.IdentityID,
                    NodeName = item.NodeName,
                    SourceType = item.SourceType.GetDescription(),
                    IncomingDateTime = item.IncomingDateTime.HasValue ? item.IncomingDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                    CreateTime = item.CreateDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    CaseID = item.CaseID,
                    SN = "",
                    ExpectDateTime = item.ExpectDateTime.HasValue ? item.ExpectDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                    CaseWarningName = item.CaseWarningName,
                    CaseType = item.CaseType == null ? "" : item.CaseType.GetDescription(),
                    IsPrevention = item.IsPrevention ? "是" : "否",
                    IsAttension = item.IsAttension ? "是" : "否",
                    ClassificationID = item.ClassificationID,
                    ClassificationName = classList.Where(g => g.ID == item.ClassificationID).Count() == 0 ? null :
                    new List<string>(classList.Where(g => g.ID == item.ClassificationID).FirstOrDefault().ParentNamePathByArray),
                    //反應者
                    ConcatUnitType = concat == null ? "" : concat.UnitType.GetDescription(),
                    //消費者
                    ConcatCustomerName = concat == null ? "" : concat.UnitType == UnitType.Customer ? concat.UserName + concat.Gender.GetDescription() : "",
                    ConcatCustomerMobile = concat == null ? "" : concat.UnitType == UnitType.Customer ? concat.Mobile : "",
                    ConcatCustomerTelephone1 = concat == null ? "" : concat.UnitType == UnitType.Customer ? concat.Telephone : "",
                    ConcatCustomerTelephone2 = concat == null ? "" : concat.UnitType == UnitType.Customer ? concat.TelephoneBak : "",
                    ConcatCustomerMail = concat == null ? "" : concat.UnitType == UnitType.Customer ? concat.Email : "",
                    ConcatCustomerAddress = concat == null ? "" : concat.UnitType == UnitType.Customer ? concat.Address : "",
                    //門市
                    ConcatStore = concat == null ? "" : concat.UnitType == UnitType.Store ? concat.NodeName + concat.StoreNo : "",
                    ConcatStoreName = concat == null ? "" : concat.UnitType == UnitType.Store ? concat.UserName : "",
                    ConcatStoreTelephone = concat == null ? "" : concat.UnitType == UnitType.Store ? concat.Telephone : "",
                    //組織
                    ConcatOrganization = concat == null ? "" : concat.UnitType == UnitType.Organization ? concat.NodeName : "",
                    ConcatOrganizationName = concat == null ? "" : concat.UnitType == UnitType.Organization ? concat.UserName : "",
                    ConcatOrganizationTelephone = concat == null ? "" : concat.UnitType == UnitType.Organization ? concat.Telephone : "",

                    //被反應者
                    ComplainedUnitType = complained == null ? "" : complained.UnitType.GetDescription(),
                    //組織單位
                    ComplainedOrganization = complained != null ? complained.ParentPathName : "",

                    //門市
                    ComplainedStore = complained != null ? (complained.UnitType == UnitType.Store ? complained.NodeName + complained.StoreNo : "") : "",
                    ComplainedStoreApplyUserName = complained != null ? (complained.UnitType == UnitType.Store ? complained.OwnerUserName : "") : "",
                    ComplainedStoreSupervisorUserName = complained != null ? (complained.UnitType == UnitType.Store ? complained.SupervisorUserName : "") : "",
                    ComplainedStoreTelephone = complained != null ? (complained.UnitType == UnitType.Store ? complained.Telephone : "") : "",

                    //組織
                    ComplainedOrganizationName = complained != null ? (complained.UnitType == UnitType.Organization ? complained.OwnerUserName : "") : "",
                    ComplainedOrganizationNodeName = complained != null ? (complained.UnitType == UnitType.Organization ? complained.NodeName : "") : "",
                    ComplainedOrganizationTelephone = complained != null ? (complained.UnitType == UnitType.Organization ? complained.OwnerUserPhone : "") : "",

                    CaseContent = "'" + item.CaseContent,
                    PromiseDateTime = item.PromiseDateTime.HasValue ? item.PromiseDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",

                    ModeType = CaseAssignmentProcessType.Communication.GetDescription(),
                    AssignmentType = "",
                    InvoiceType = item.InvoiceType ?? "",
                    AssignmentUser = null,
                    NoticeContent = "'" + item.NoticeContent,
                    NoticeTime = item.NoticeDateTime.HasValue ? item.NoticeDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",


                    //回覆內容
                    RetryContent = "",
                    CloseCaseContent = "",
                    CloseCaseTime = "",
                    CloseCaseUser = "",
                    FinishContent = item.FinishContent,
                    FinishDateTime = item.FinishDateTime.HasValue ? item.FinishDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                    ReasonName = item.CaseFinishReasonDatas != null ? string.Join("/", item.CaseFinishReasonDatas.Select(y => y.Text)) : "",
                    ApplyUserName = item.ApplyUserName,
                    CaseTagName = item.CaseTagList != null ? string.Join("/", item.CaseTagList.Select(y => y.Name)) : "",
                };
                //ReasonName依所屬Title分類 20200828
                IDictionary<string, string> dist = new Dictionary<string, string>();
                var titleList = item.CaseFinishReasonDatas.Select(x => x.CaseFinishReasonClassification.Title).Distinct().ToList();
                if (titleList.Any())
                {
                    titleList.ForEach(x =>
                    {
                        dist.Add($"{x}", "'" + string.Join("/", item.CaseFinishReasonDatas.Where(y => y.CaseFinishReasonClassification.Title == x).Select(y => y.Text)));
                    });
                }
                ui.ReasonList = dist;
                result.Add(ui);
            }

            return result;
        }
        /// <summary>
        /// 取得權責單位
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string GetCaseComplainedUserName(List<CaseComplainedUser> data)
        {
            string result = "";
            //依照門市/單位回傳
            var ResponsibilityList = data.Where(y => y.CaseComplainedUserType == CaseComplainedUserType.Responsibility);
            result = ResponsibilityList.First().UnitType == UnitType.Store ?
                ResponsibilityList.First().UserName + ResponsibilityList.First().StoreNo
                : ResponsibilityList.First().UserName;
            return result;

        }
    }
}
