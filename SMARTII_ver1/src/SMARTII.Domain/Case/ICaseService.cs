using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SMARTII.Domain.Data;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Notification.Email;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Report;

namespace SMARTII.Domain.Case
{
    public interface ICaseService
    {
        /// <summary>
        /// 更新案件
        /// </summary>
        /// <param name="case"></param>
        /// <param name="IsFinish"></param>
        /// <returns></returns>
        Case UpdateComplete(Case @case, bool IsFinish);

        /// <summary>
        /// 案件逾時未結案通知
        /// </summary>
        void CaseTimeoutNotice();
        /// <summary>
        /// 更新結案附件
        /// </summary>
        /// <param name="caseID"></param>
        /// <param name="emailPayload"></param>
        void CaseFinishedMailReply(string caseID, EmailPayload emailPayload);

        /// <summary>
        /// 信件認養
        /// </summary>
        /// <param name="messageID"></param>
        /// <param name="buID"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        Task<string> Adopt(string messageID, int buID, int groupID);

        /// <summary>
        /// 管理者指派
        /// </summary>
        /// <param name="messageIDs"></param>
        /// <param name="userID"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        Task<CounterResult> AdminOrder(string[] messageIDs, int buID, string userID, int groupID);

        /// <summary>
        /// 自動分派
        /// </summary>
        /// <param name="eachPersonMail"></param>
        /// <param name="userIDs"></param>
        /// <param name="buID"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        Task<CounterResult> AutoOrder(int eachPersonMail, string[] userIDs, int buID, int groupID);

        /// <summary>
        /// 批次回覆
        /// </summary>
        /// <param name="questionID"></param>
        /// <param name="emailContent"></param>
        /// <param name="finishContent"></param>
        /// <param name="messageIDs"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        Task<CounterResult> ReplyRange(int questionID, string emailContent, string finishContent, string[] messageIDs, int buID, int groupID);

    
        
    }
}
