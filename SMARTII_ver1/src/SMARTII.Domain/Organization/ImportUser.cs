using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMARTII.Domain.Organization
{
    public class ImportUser 
    {
        public User User { get; set; }
        public int[] RoleIDs { get; set; }
    }
}
