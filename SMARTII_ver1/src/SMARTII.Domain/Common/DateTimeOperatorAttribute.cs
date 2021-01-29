using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMARTII.Domain.Common
{
    public class DateTimeOperatorAttribute : Attribute
    {
        public DateTimeOperatorAttribute(double Value, DateTimeOperatorType Type)
        {
            this.Type = Type;
            this.Value = Value;
        }

        public double Value { get; }

        public DateTimeOperatorType Type { get; }
    }
}
