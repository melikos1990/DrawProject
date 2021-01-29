using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMARTII.Domain.Case;

namespace SMARTII.Domain.Notification
{
    public interface IPPCLifeNotificationFactory
    {
        List<PPCLifeEffectiveSummary> Execute(List<CasePPCLife> casePPCLives);
    }
}
