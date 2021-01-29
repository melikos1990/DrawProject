using Ptc.Data.Condition2.Mssql.Repository;
using SMARTII.Database.SMARTII;

namespace SMARTII.Domain.System
{
    public interface ISystemAggregate
    {
        IMSSQLRepository<SYSTEM_PARAMETER, SystemParameter> SystemParameter_T1_T2_ { get; }
        IMSSQLRepository<SYSTEM_PARAMETER> SystemParameter_T1_ { get; }
        IMSSQLRepository<SYSTEM_LOG, SystemLog> SystemLog_T1_T2_ { get; }
        IAsyncMSSQLRepository<SYSTEM_LOG, SystemLog> Async_SystemLog_T1_T2_ { get; }
        IMSSQLRepository<SYSTEM_LOG> SystemLog_T1_ { get; }
    }
}