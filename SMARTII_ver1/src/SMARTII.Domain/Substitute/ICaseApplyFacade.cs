using System.Collections.Generic;
using System.Threading.Tasks;
using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Substitute
{
    public interface ICaseApplyFacade
    {
        /// <summary>
        /// 分派案件
        /// </summary>
        /// <param name="user"></param>
        /// <param name="caseIDs"></param>
        /// <returns></returns>
        Task Apply(User user, List<string> caseIDs);
    }
}
