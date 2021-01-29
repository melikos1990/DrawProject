using System.Threading.Tasks;

namespace SMARTII.Domain.Organization
{
    public interface IEnterpriseFacade
    {
        Task<Enterprise> Create(Enterprise data);

        Task<Enterprise> Update(Enterprise data);
    }
}