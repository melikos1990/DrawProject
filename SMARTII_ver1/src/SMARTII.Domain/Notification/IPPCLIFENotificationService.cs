using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMARTII.Domain.Notification
{
    public interface IPPCLIFENotificationService
    {
        /// <summary>
        /// (PPCLIFE 客制)排程計算大量叫修數量
        /// </summary>
        void PPCLifeCalculate();

        /// <summary>
        /// (PPCLIFE 客制)重算客制案件規則
        /// </summary>
        void PPCLifeRuleReset();
    }
}
