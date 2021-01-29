using System.Collections.Generic;
using System.Linq;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Organization;

namespace SMARTII.Models.Account
{
    public class AccountUserViewModel
    {
        public AccountUserViewModel(string Mode)
        {
            this.Mode = Mode;
        }

        public AccountUserViewModel(User user, TokenPair tokenPair)
        {
            this.Account = user.Account;
            this.UserName = user.Name;
            this.AccessToken = tokenPair?.AccessToken;
            this.RefreshToken = tokenPair?.RefreshToken;

            this.Feature = user.Feature?
                               .Select(x => new PageAuthViewModel(x))?
                               .ToList();

            this.Role = user.Roles
                            .Select(x => new RoleViewModel(x))
                            .ToList();

            this.JobPosition = user.JobPositions
                                    .Select(x => new JobPositionViewModel(x))
                                    .ToList();

            this.FavariteFeature = user.UserParameter?.FavoriteFeature?
                   .Select(x => new PageAuthViewModel(x))?
                   .ToList();

            this.DownProviderBUDist = user.DownProviderBUDist;

            this.Mode = "login";
        }

        public string Account { get; set; }

        public string UserName { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public List<PageAuthViewModel> Feature { get; set; }

        public List<RoleViewModel> Role { get; set; }

        public List<JobPositionViewModel> JobPosition { get; set; }

        public List<PageAuthViewModel> FavariteFeature { get; set; }

        public IDictionary<OrganizationType, int[]> DownProviderBUDist { get; set; }
            = new Dictionary<OrganizationType, int[]>();

        public string Mode { get; set; }
    }
}
