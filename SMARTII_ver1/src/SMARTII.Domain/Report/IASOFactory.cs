using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMARTII.Domain.Report
{
    public interface IASOFactory
    {

        #region 報表

        Task<byte[]> GenerateOnCallExcel(DateTime start, DateTime end);

        Task<byte[]> GenerateAssignmentOnCallExcel(DateTime start, DateTime end);

        Task<byte[]> GenerateEveryDayExcel(DateTime start, DateTime end);

        #endregion

    }
}
