using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMARTII.Domain.Notification.Email;

namespace SMARTII.Domain.Notification
{
    public interface IOfficialEmalEffectiveDataFacade
    {
        Task Create(List<(OfficialEmailEffectivePayload, OfficialEmailHistory)> data);
    }
}
