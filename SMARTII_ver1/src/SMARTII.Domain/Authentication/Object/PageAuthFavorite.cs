using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMARTII.Domain.Authentication.Object
{
    public class PageAuthFavorite : PageAuth
    {
        public PageAuthFavorite()
        { }
        public int Order { get; set; }
    }
}
