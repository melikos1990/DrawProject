using SMARTII.Domain.Case;
using SMARTII.Domain.Notification.Email;
using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Notification
{
    public interface IEmailParser
    {
        /// <summary>
        /// 根據實體信轉換換Model
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        (OfficialEmailEffectivePayload, OfficialEmailHistory) ConvertToOfficialEmail(OfficialEmailEffectivePayload payload);

        /// <summary>
        /// 根據實體信轉成案件來源
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="caseWarning"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        CaseSource ConvertToCaseSource(OfficialEmailEffectivePayload payload, CaseWarning caseWarning, User user);
    }
}
