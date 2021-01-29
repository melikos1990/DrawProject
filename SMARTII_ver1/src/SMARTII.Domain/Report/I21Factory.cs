using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMARTII.Domain.Report
{
    public interface I21Factory
    {

        #region 報表

        Task<byte[]> GenerateOnCallExcel(DateTime start, DateTime end);

        #endregion

    }
}
