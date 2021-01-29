using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMARTII.Domain.Case
{
    public interface IFlow
    {
        Task<IFlowable> Run(IFlowable flowable, params object[] args);
    }
}
