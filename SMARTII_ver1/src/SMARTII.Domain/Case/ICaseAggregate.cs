using System.Dynamic;
using Ptc.Data.Condition2.Mssql.Repository;
using SMARTII.Database.SMARTII;

namespace SMARTII.Domain.Case
{
    public interface ICaseAggregate
    {
        IMSSQLRepository<CASE> Case_T1_ { get; }
        IMSSQLRepository<CASE, Case> Case_T1_T2_ { get; }
        IMSSQLRepository<CASE_SOURCE> CaseSource_T1_ { get; }
        IMSSQLRepository<CASE_SOURCE, CaseSource> CaseSource_T1_T2_ { get; }
        IMSSQLRepository<CASE_SOURCE_CODE> CaseSourceCode_T1_ { get; }
        IMSSQLRepository<CASE_CODE> CaseCode_T1_ { get; }
        IMSSQLRepository<CASE_ASSIGNMENT> CaseAssignment_T1_ { get; }
        IMSSQLRepository<CASE_ASSIGNMENT, CaseAssignment> CaseAssignment_T1_T2_ { get; }
        IMSSQLRepository<CASE_ASSIGNMENT_USER> CaseAssignmentUser_T1_ { get; }
        IMSSQLRepository<CASE_ASSIGNMENT_USER, CaseAssignmentUser> CaseAssignmentUser_T1_T2_ { get; }
        IMSSQLRepository<CASE_ASSIGNMENT_CONCAT_USER> CaseAssignmentConcatUser_T1_ { get; }
        IMSSQLRepository<CASE_ASSIGNMENT_CONCAT_USER, CaseAssignmentConcatUser> CaseAssignmentConcatUser_T1__T1_T2_ { get; }
        IMSSQLRepository<CASE_ASSIGNMENT_COMPLAINT_INVOICE> CaseAssignmentComplaintInvoice_T1_ { get; }
        IMSSQLRepository<CASE_ASSIGNMENT_COMPLAINT_INVOICE, CaseAssignmentComplaintInvoice> CaseAssignmentComplaintInvoice_T1_T2_ { get; }
        IMSSQLRepository<CASE_ASSIGNMENT_COMPLAINT_NOTICE> CaseAssignmentComplaintNotice_T1_ { get; }
        IMSSQLRepository<CASE_ASSIGNMENT_COMPLAINT_NOTICE, CaseAssignmentComplaintNotice> CaseAssignmentComplaintNotice_T1_T2_ { get; }
        IMSSQLRepository<CASE_ASSIGNMENT_COMPLAINT_INVOICE_CODE> CaseAssignmentComplaintInvoiceCode_T1_ { get; }
        IMSSQLRepository<CASE_ASSIGNMENT_COMPLAINT_INVOICE_USER , CaseAssignmentComplaintInvoiceUser> CaseAssignmentComplaintInvoiceUser_T1_T2 { get; }
        IMSSQLRepository<CASE_ASSIGNMENT_COMPLAINT_INVOICE_USER> CaseAssignmentComplaintInvoiceUser_T1_ { get; }
        IMSSQLRepository<CASE_ASSIGNMENT_COMMUNICATE, CaseAssignmentCommunicate> CaseAssignmentCommunicate_T1_T2_ { get; }
        IMSSQLRepository<CASE_ASSIGNMENT_COMMUNICATE> CaseAssignmentCommunicate_T1_ { get; }
        IMSSQLRepository<CASE_ASSIGN_GROUP> CaseAssignmentGroup_T1_ { get; }
        IMSSQLRepository<CASE_ASSIGN_GROUP, CaseAssignGroup> CaseAssignmentGroup_T1_T2_ { get; }
        IMSSQLRepository<CASE_ASSIGN_GROUP_USER> CaseAssignmentGroupUser_T1_ { get; }
        IMSSQLRepository<CASE_ASSIGN_GROUP_USER, CaseAssignGroupUser> CaseAssignmentGroupUser_T1_T2_ { get; }
        IMSSQLRepository<CASE_SOURCE_USER> CaseSourceUser_T1_ { get; }
        IMSSQLRepository<CASE_SOURCE_USER, CaseSourceUser> CaseSourceUser_T1_T2_ { get; }
        IMSSQLRepository<CASE_RESUME> CaseResume_T1_ { get; }
        IMSSQLRepository<CASE_RESUME, CaseResume> CaseResume_T1_T2_ { get; }
        IMSSQLRepository<CASE_REMIND> CaseRemind_T1_ { get; }
        IMSSQLRepository<CASE_REMIND, CaseRemind> CaseRemind_T1_T2_ { get; }
        IMSSQLRepository<CASE_CONCAT_USER> CaseConcatUser_T1_ { get; }
        IMSSQLRepository<CASE_CONCAT_USER, CaseConcatUser> CaseConcatUser_T1_T2_ { get; }
        IMSSQLRepository<CASE_COMPLAINED_USER> CaseComplainedUser_T1_ { get; }
        IMSSQLRepository<CASE_COMPLAINED_USER, CaseComplainedUser> CaseComplainedUser_T1_T2_ { get; }
        IMSSQLRepository<CASE_ASSIGNMENT_RESUME> CaseAssignmentResume_T1_ { get; }
        IMSSQLRepository<CASE_ASSIGNMENT_RESUME, CaseAssignmentResume> CaseAssignmentResume_T1_T2_ { get; }
        IMSSQLRepository<CASE_NOTICE> CaseNotice_T1_ { get; }
        IMSSQLRepository<CASE_NOTICE, CaseNotice> CaseNotice_T1_T2_ { get; }
        IMSSQLRepository<CASE_HISTORY> CaseHistory_T1_ { get; }
        IMSSQLRepository<CASE_HISTORY, CaseHistory> CaseHistory_T1_T2_ { get; }
        IMSSQLRepository<CASE_SOURCE_HISTORY> CaseSourceHistory_T1_ { get; }
        IMSSQLRepository<CASE_SOURCE_HISTORY, CaseSourceHistory> CaseSourceHistory_T1_T2_ { get; }
        IMSSQLRepository<CASE_ASSIGNMENT_HISTORY> CaseAssignmentHistory_T1_ { get; }
        IMSSQLRepository<CASE_ASSIGNMENT_HISTORY, CaseAssignmentHistory> CaseAssignmentHistory_T1_T2_ { get; }
        IMSSQLRepository<CASE_PPCLIFE> CasePPCLife_T1_ { get; }
        IMSSQLRepository<CASE_PPCLIFE, CasePPCLife> CasePPCLife_T1_T2_ { get; }
        IMSSQLRepository<CASE_ITEM> CaseItem_T1_ { get; }
        IMSSQLRepository<CASE_ITEM, CaseItem> CaseItem_T1_T2_ { get; }
    }
}
