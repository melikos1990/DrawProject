using System.Threading.Tasks;
using SMARTII.Domain.Case;

namespace SMARTII.Domain.Master
{
    public interface ICaseTemplateFacade
    {
        Task<CaseTemplate> Update(CaseTemplate data);

        Task<CaseTemplate> Create(CaseTemplate data);
        
    }
}
