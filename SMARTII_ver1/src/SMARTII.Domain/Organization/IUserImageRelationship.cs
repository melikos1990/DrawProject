using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMARTII.Domain.Organization
{
    public interface IUserImageRelationship
    {
        string CreateUserID { get; set; }
        string ImagePath { get; set; }
    }
}
