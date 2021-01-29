using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Master
{
    public interface IStoreFacade
    {

        Store<T> GetComplete<T>(int nodeID, OrganizationType organizationType);

        JobPosition GetApplyJobPositions(int nodeJobID, string key);

    }
}
