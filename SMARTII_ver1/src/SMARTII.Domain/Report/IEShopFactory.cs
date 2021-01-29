using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMARTII.Domain.Report
{
    public interface IEShopFactory
    {

        #region 報表

        Task<byte[]> GenerateOnCallExcel(DateTime start, DateTime end);

        #endregion

    }
}
