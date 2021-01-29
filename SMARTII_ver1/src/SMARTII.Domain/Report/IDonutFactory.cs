using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMARTII.Domain.Report
{
    public interface IDonutFactory
    {

        #region 報表

        byte[] GenerateOnCallExcel(DateTime start, DateTime end);

        #endregion

    }
}
