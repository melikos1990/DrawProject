using System.DirectoryServices;
using SMARTII.Domain.Data;

namespace SMARTII.Domain.Authentication.Object
{
    public class ADUser
    {
        public ADUser(SearchResult result)
        {
            this.cn = result.Properties.DisplayWhenNull("cn");
            this.sn = result.Properties.DisplayWhenNull("sn");
            this.displayname = result.Properties.DisplayWhenNull("displayname");
            this.department = result.Properties.DisplayWhenNull("department");
            this.mail = result.Properties.DisplayWhenNull("mail");
            this.givenname = result.Properties.DisplayWhenNull("givenname");
            this.userprincipalname = result.Properties.DisplayWhenNull("cnuserprincipalname");
            this.samaccountname = result.Properties.DisplayWhenNull("samaccountname");
            this.company = result.Properties.DisplayWhenNull("company");
            this.manager = result.Properties.DisplayWhenNull("manager");
        }

        public string cn { get; set; }

        public string sn { get; set; }

        public string displayname { get; set; }

        public string department { get; set; }

        public string mail { get; set; }

        public string givenname { get; set; }

        public string userprincipalname { get; set; }

        public string samaccountname { get; set; }

        public string company { get; set; }

        public string manager { get; set; }
    }
}