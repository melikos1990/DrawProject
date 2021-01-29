using System.Threading.Tasks;
using SMARTII.Domain.Case;

namespace SMARTII.Domain.Master
{
    public interface ICaseTagFacade
    {
        Task<CaseTag> Update(CaseTag data);

        Task<CaseTag> Create(CaseTag data);
    }
}