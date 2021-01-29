using System.ComponentModel;

namespace SMARTII.Domain.Notification
{
    public enum PersonalNotificationType
    {
        /// <summary>
        /// 通知群組
        /// </summary>
        [Description("通知群組")]
        NotificationGroup,

        /// <summary>
        /// 案件指派
        /// </summary>
        [Description("案件指派")]
        CaseAssign,

        /// <summary>
        /// 案件結案
        /// </summary>
        [Description("案件結案")]
        CaseAssignmentFinish,

        /// <summary>
        /// 認養來信
        /// </summary>
        [Description("認養來信")]
        MailAdopt,

        /// <summary>
        /// 案件異動
        /// </summary>
        [Description("案件異動")]
        CaseModify,

        /// <summary>
        /// 官網來信 (前端識別用)
        /// </summary>
        [Description("官網來信")]
        MailIncoming,

        /// <summary>
        /// 公佈欄
        /// </summary>
        [Description("公佈欄")]
        Billboard,
        
        /// <summary>
        /// 代辦事項 (前端識別用)
        /// </summary>
        [Description("代辦事項")]
        CaseRemind,
        

        /// <summary>
        /// 統藥客制提醒
        /// </summary>
        [Description("統藥客制提醒")]
        NotificationPPCLife,
    }
}
