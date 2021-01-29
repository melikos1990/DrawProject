using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMARTII.Domain.Common
{
    public class DateTimeFormatAttribute : Attribute
    {
        public DateTimeFormatAttribute(string format = "yyyy/MM/dd HH:mm:ss")
        {
            Format = format;
        }

        public string Format { get; }
    }
}
