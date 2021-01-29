using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMARTII.Domain.Case
{
    public interface ICaseRelationship
    {
        string CaseID { get; set; }

        Case @case { get; set; }
    }
}
