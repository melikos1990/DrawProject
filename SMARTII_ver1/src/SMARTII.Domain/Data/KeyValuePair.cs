using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SMARTII.Domain.Data
{
    public class KeyValuePair
    {
        public KeyValuePair() { }

        [JsonProperty("KEY")]
        public string Key { get; set; }
        [JsonProperty("VALUE")]
        public string Value { get; set; }
    }
}
