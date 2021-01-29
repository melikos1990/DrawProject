using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Report
{
    public interface IUserReportProvider
    {
       Task<byte[]> GetAuthorityReport(UserSearchCondition condition,List<User> user);
    }
}
