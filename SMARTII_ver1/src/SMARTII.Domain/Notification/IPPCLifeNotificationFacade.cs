using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMARTII.Domain.Case;

namespace SMARTII.Domain.Notification
{
    public interface IPPCLifeNotificationFacade
    {
        #region BATCH
        /// <summary>
        /// (PPCLIFE 客制)比對現有的案件與商品
        /// </summary>
        void PPCLifeCalculateCaseRepeat(List<CaseItem> caseItems);

        /// <summary>
        /// (PPCLIFE 客制)比對達標標的與現有標的
        /// </summary>
        void PPCLifeCalculateSubjectRepeat(List<PPCLifeEffectiveSummary> pPCLifeEffectiveSummaries);
        #endregion
    }
}
