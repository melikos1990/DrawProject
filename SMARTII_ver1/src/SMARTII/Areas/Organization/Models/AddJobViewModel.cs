using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SMARTII.Areas.Organization.Models
{
    public class AddJobViewModel
    {
        public AddJobViewModel()
        {
        }

        [Required]
        public int? NodeID { get; set; }

        [Required]
        public List<int> JobIDs { get; set; }
    }
}