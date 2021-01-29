using System;
using System.Collections.Generic;
using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Case
{
    public interface ICaseAssignmentFacade
    {
        /// <summary>
        /// 產生反應單序號
        /// </summary>
        /// <param name="type"></param>
        /// <param name="buCode"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        string GetInvoiceCode(string type, string buCode, DateTime? date = null);

        /// <summary>
        /// 批次異動人員
        /// </summary>
        /// <param name="caseAssignment"></param>
        void BatchModifyUsers(CaseAssignment caseAssignment);

        /// <summary>
        /// 取得轉派流水號
        /// </summary>
        /// <param name="caseID"></param>
        /// <returns></returns>
        int GetAssignmentCode(string caseID);

        /// <summary>
        /// 取得歷程清單
        /// -> 反應單
        /// -> 轉派
        /// -> 一般通知
        /// </summary>
        /// <param name="caseID"></param>
        /// <returns></returns>
        List<CaseAssignmentBase> GetAssignmentAggregate(string caseID);


        /// <summary>
        /// 刪除一般通知附檔
        /// </summary>
        /// <param name="id"></param>
        /// <param name="key"></param>
        void DeleteNoticeFileWithUpdate(int id, string key);

        /// <summary>
        /// 刪除反應單附檔
        /// </summary>
        /// <param name="id"></param>
        /// <param name="key"></param>
        void DeleteInvoiceFileWithUpdate(int id, string key);


        /// <summary>
        /// 刪除轉派附檔
        /// </summary>
        /// <param name="caseID"></param>
        /// <param name="id"></param>
        /// <param name="key"></param>
        void DeleteAssignmentFinishFileWithUpdate(string caseID , int id, string key);

        /// <summary>
        /// 新增轉派歷程CaseAssignmentResume
        /// </summary>
        /// <param name="beforeCaseAssignment"></param>
        /// <param name="caseAssignmentResumeContent"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        void CreateResume(CaseAssignment beforeCaseAssignment, string caseAssignmentResumeContent, string userName, JobPosition jobPosition = null);

        /// <summary>
        /// 新增轉派CaseAssignmentHistory
        /// </summary>
        /// <param name="caseAssignment"></param>
        /// <param name="caseAssignmentHistoryPreFix"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        void CreateHistory(CaseAssignment caseAssignment, string caseAssignmentHistoryPreFix, string userName);

        /// <summary>
        /// 新增轉派PersonalNotification
        /// </summary>
        /// <param name="caseAssignment"></param>
        /// <param name="user"></param>
        void CreatePersonalNotification(CaseAssignment caseAssignment, string userName, string content = null);
    }
}
