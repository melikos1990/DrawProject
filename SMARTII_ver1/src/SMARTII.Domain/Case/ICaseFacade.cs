using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Notification.Email;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Report;

namespace SMARTII.Domain.Case
{
    public interface ICaseFacade
    {
        /// <summary>
        /// 取得案件編號
        /// </summary>
        /// <param name="nodeKey"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        string GetCaseCode(string nodeKey, DateTime? date = null);

        /// <summary>
        /// 更新或是新增案件標籤
        /// </summary>
        /// <param name="case"></param>
        void UpdateOrCreateTags(Case @case);

        /// <summary>
        /// 批次修改相關人員
        /// </summary>
        /// <param name="case"></param>
        void BatchModifyUsers(Case @case);

        /// <summary>
        /// 批次修改結案原因
        /// </summary>
        /// <param name="case"></param>
        void BatchModifyFinishedReasons(Case @case);

        /// <summary>
        /// 取得使用者完整資訊
        /// </summary>
        /// <param name="caseID"></param>
        /// <returns></returns>
        Case GetComplete(string caseID);

        /// <summary>
        /// 取得應完成時間
        /// </summary>
        /// <param name="case"></param>
        /// <param name="term"></param>
        /// <param name="announceDateTime"></param>
        /// <returns></returns>
        DateTime GetPromiseDateTime(Case @case, HeaderQuarterTerm term, DateTime announceDateTime);

        /// <summary>
        /// 刪除附件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="key"></param>
        /// <param name="caseType"></param>
        void DeleteFileWithUpdate(string id, string key, CaseType caseType);

        /// <summary>
        /// 檢查信件是否結案
        /// </summary>
        /// <param name="messageID"></param>
        /// <returns></returns>
        Task<(bool isUnclosed, OfficialEmailEffectivePayload officialEmailEffective, Domain.Case.Case @case)> CheckExistCaseIsUnclose(string messageID, int buID);

        /// <summary>
        /// 檢查該人員有無此Group權限
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="users"></param>
        /// <returns></returns>
        bool CheckGroupAuth(int groupID , params User[] users);


        /// <summary>
        /// 新增案件CaseResume
        /// </summary>
        /// <param name="caseID"></param>
        /// <param name="beforeCase"></param>
        /// <param name="caseResumeContent"></param>
        /// <param name="caseHistoryPreFix"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        void CreateResume(string caseID, Domain.Case.Case beforeCase, string caseResumeContent, string caseHistoryPreFix, User user);

        /// <summary>
        /// 新增案件CaseHistory
        /// </summary>
        /// <param name="caseID"></param>
        /// <param name="caseHistoryPreFix"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        void CreateHistory(string caseID, Domain.Case.Case @case, string caseHistoryPreFix, User user);

        /// <summary>
        /// 新增案件通知PersonalNotification
        /// </summary>
        /// <param name="caseID"></param>
        /// <param name="personalNotificationType"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        void CreatePersonalNotification(string caseID, PersonalNotificationType personalNotificationType, User user);

    }
}
