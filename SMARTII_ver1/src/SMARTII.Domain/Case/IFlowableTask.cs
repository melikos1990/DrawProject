using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMARTII.Domain.Case
{
    public interface IFlowableTask
    {
        Task<IFlowable> Execute(IFlowable flowable, params object[] args);

        Task Validator(IFlowable flowable , params object[] args);
    }
}
