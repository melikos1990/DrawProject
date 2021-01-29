using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SMARTII.Areas.Organization.Models
{
    public class AddUserViewModel
    {
        public AddUserViewModel()
        {
        }

        [Required]
        public int? NodeJobID { get; set; }

        [Required]
        public List<string> UserIDs { get; set; }
    }
}
