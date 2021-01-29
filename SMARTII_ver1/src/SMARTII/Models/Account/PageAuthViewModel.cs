using SMARTII.Domain.Authentication.Object;

namespace SMARTII.Models.Account
{
    public class PageAuthViewModel
    {
        public PageAuthViewModel()
        {
        }

        public PageAuthViewModel(PageAuth pageAuth)
        {
            this.Feature = pageAuth.Feature;
            this.AuthenticationType = pageAuth.AuthenticationType;
        }

        public string Feature { get; set; }

        public AuthenticationType AuthenticationType { get; set; }
    }
}