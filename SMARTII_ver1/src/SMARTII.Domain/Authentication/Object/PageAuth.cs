using System;
using Newtonsoft.Json;

namespace SMARTII.Domain.Authentication.Object
{
    public class PageAuth
    {
        public PageAuth()
        {
        }

        /// <summary>
        /// 功能名稱
        /// </summary>
        public string Feature { get; set; }

        /// <summary>
        /// 操作權限
        /// </summary>
        public AuthenticationType AuthenticationType { get; set; }

        /// <summary>
        /// 黑名單
        /// </summary>
        [JsonIgnore]
        public Boolean Deny { get; set; }
    }
}