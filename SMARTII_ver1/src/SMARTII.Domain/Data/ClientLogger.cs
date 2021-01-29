using System;
using System.Collections.Generic;

namespace SMARTII.Domain.Data
{
    public class ClientLogInfo
    {
        public ClientLogInfo()
        {
        }

        public string message { get; set; }

        public string logger { get; set; }

        public TimeSpan timestamp { get; set; }

        public string level { get; set; }

        public string url { get; set; }
    }

    public class ClientLog
    {
        public string layout { get; set; }

        public List<ClientLogInfo> data { get; set; }
    }
}