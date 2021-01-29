using System.Collections.Generic;
using System.Linq;
using SMARTII.Areas.Organization.Models.User;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Common.Models.Organization
{
    public class JobPositionListViewModel
    {
        public JobPositionListViewModel()
        {
        }

        public JobPositionListViewModel(JobPosition jobPosition)
        {
            this.JobID = jobPosition.Job?.ID;
            this.JobName = jobPosition.Job?.Name;
            this.NodeJobID = jobPosition.ID;
            this.NodeName = jobPosition.Node.Name;
            this.Users = jobPosition.Users.Select(user => new UserListViewModel(user)).ToList();
        }

        public int? JobID { get; set; }

        public string JobName { get; set; }
        
        public int NodeJobID { get; set; }
        public string NodeName { get; set; }

        public List<UserListViewModel> Users { get; set; }
    }
}
