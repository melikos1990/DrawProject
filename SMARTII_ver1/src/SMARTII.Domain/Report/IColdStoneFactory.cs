using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMARTII.Domain.Report
{
    public interface IColdStoneFactory
    {

        #region 報表

        Task<byte[]> GetOnCallExcel(DateTime start, DateTime end);

        #endregion

    }
}
