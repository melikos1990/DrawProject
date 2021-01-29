using System;
using System.Threading.Tasks;
using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Case
{
    public interface ICaseSourceFacade
    {
        /// <summary>
        /// 取得來源編號
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        string GetSourceCode(DateTime? date = null);

        /// <summary>
        /// 刷新預立案來源
        /// </summary>
        /// <param name="caseIDs"></param>
        void CancelPreventTagsFromCaseIDs(string[] caseIDs);

        /// <summary>
        /// 新增來源歷程後記錄
        /// </summary>
        /// <param name="caseSourceID"></param>
        /// <param name="caseSourceHistoryPreFix"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        void CreateHistory(CaseSource caseSource, string caseSourceHistoryPreFix, User user);
    }
}
