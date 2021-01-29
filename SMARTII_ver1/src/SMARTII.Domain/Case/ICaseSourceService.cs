using System.Threading.Tasks;
using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Case
{
    public interface ICaseSourceService
    {
        /// <summary>
        /// 刪除預立案件
        /// </summary>
        void ClosePreventionCaseSource();

        /// <summary>
        /// 更新案件來源
        /// </summary>
        /// <param name="case"></param>
        /// <returns></returns>
        CaseSource UpdateComplete(CaseSource caseSource);

    }
}
