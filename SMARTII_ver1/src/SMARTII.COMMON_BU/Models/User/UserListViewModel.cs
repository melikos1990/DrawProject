using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMARTII.Domain.Organization;

namespace SMARTII.COMMON_BU.Models.User
{
    public class UserListViewModel
    {

        public UserListViewModel() { }

        public UserListViewModel(SMARTII.Domain.Organization.User user, string jobName, string BuName)
        {
            this.NodeName = BuName;
            this.JobName = jobName;
            this.UserName = user.Name;
            this.Mobile = user.Mobile;
            this.Email = user.Email;
        }

        public string NodeName { get; set; }

        public string JobName { get; set; }


        public string UserName { get; set; }


        public string Mobile { get; set; }


        public string Email { get; set; }

    }
}
