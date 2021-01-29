using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMARTII.Domain.Common
{
    public class CustomAttribute : Attribute
    {
        public CustomAttribute()
        {
        }

        public CustomAttribute(string description)
        {
            this.Description = description;
        }

        public string Description { get; set; }
    }
}
