using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMARTII.Domain.Master
{
    public interface IOfficialEailGroupRelationship
    {
        int Email_Group_ID { get; set; }
        string Account { get; set; }
        string BuMailAccount { get; set; }
        string MailDisplayName { get; set; }
    }
}
