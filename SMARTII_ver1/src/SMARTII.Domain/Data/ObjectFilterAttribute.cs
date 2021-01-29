using System;

namespace SMARTII.Domain.Data
{

    public class BaseAttribute : Attribute
    {
        public object Value { get; set; }
    }
    public class ObjectFilterAttribute : BaseAttribute
    {
    }
}
