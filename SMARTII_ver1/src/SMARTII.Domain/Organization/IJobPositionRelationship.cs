using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMARTII.Domain.Organization
{
    public interface IJobPositionRelationship
    {
        int NodeID { get; set; }

        List<JobPosition> JobPositions { get; set; }
    }
}
