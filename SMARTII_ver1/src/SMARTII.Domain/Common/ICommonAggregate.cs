using Autofac.Features.Indexed;

namespace SMARTII.Domain.Common
{
    public interface ICommonAggregate
    {
        Ptc.Logger.ILogger Logger { get; }

        IIndex<string, Ptc.Logger.ILogger> Loggers { get; }
    }
}