using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ptc.Data.Condition2.Common.Class;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Report;

namespace SMARTII.Domain.Case
{
    public interface ICaseSearchFacade
    {
        List<ExcelCaseAssignmentList> GetCaseAssignmentNoticeForHSLists(CaseAssignmentHSCondition condition, string headNodeID, OrganizationType organizationType);
        List<ExcelCaseAssignmentList> GetCaseAssignmentCommuncateForHSLists(CaseAssignmentHSCondition condition, string headNodeID, OrganizationType organizationType);

        List<ExcelCaseAssignmentList> GetCaseAssignmentInvoiceForHSLists(CaseAssignmentHSCondition condition, string headNodeID, OrganizationType organizationType);

        List<ExcelCaseAssignmentList> GetCaseAssignmentForHSLists(CaseAssignmentHSCondition condition, string headNodeID, OrganizationType organizationType);

        List<ExcelCaseAssignmentList> GetCaseAssignmentNoticeForCustomerLists(CaseAssignmentCallCenterCondition condition, string GroupID);
        List<ExcelCaseAssignmentList> GetCaseAssignmentCommuncateForCustomerLists(CaseAssignmentCallCenterCondition condition, string GroupID);

        List<ExcelCaseAssignmentList> GetCaseAssignmentInvoiceForCustomerLists(CaseAssignmentCallCenterCondition condition, string GroupID);

        List<ExcelCaseAssignmentList> GetCaseAssignmentForCustomerLists(CaseAssignmentCallCenterCondition condition, string GroupID);

        List<SP_GetCaseList> GetCaseHSLists(CaseHSCondition condition, string HeadNodeID);

        List<SP_GetCaseList> GetCaseForCustomerLists(CaseCallCenterCondition condition, string GroupID);

        PagedList<CaseAssignment> GetHeadquarterUnFinishCaseAssignment(HeaderQuarterSummarySearchType searchType, MSSQLCondition<CASE_ASSIGNMENT> con);

        int GetHeadquarterUnFinishCaseAssignmentCount(HeaderQuarterSummarySearchType searchType);

        PagedList<CaseAssignment> GetVenderUnFinishCaseAssignment(HeaderQuarterSummarySearchType searchType, MSSQLCondition<CASE_ASSIGNMENT> con);

        int GetVenderUnFinishCaseAssignmentCount(HeaderQuarterSummarySearchType searchType);

        Task<string> GetClassificationIDGroup(int ClassificationID);
    }
}
