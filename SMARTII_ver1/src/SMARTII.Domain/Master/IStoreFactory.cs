using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Master
{
    public interface IStoreFactory
    {
        Task Update<T>(Store store);
    }
}
