using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMARTII.Domain.Data
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class ReplaceAttribute: Attribute
    {
        public ReplaceAttribute(string key, Type parser = null, string methodName = null)
        {
            this.Key = key;
            this.Parser = parser;
            this.MethodName = methodName;
        }
        public string Key { get; set; }
        public Type Parser { get; set; }
        public string MethodName { get; set; }
    }
}
