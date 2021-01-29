using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMARTII.Domain.Organization;

namespace SMARTII.OpenPoint.Domain
{
    /// <summary>
    /// 官網來信 信件內容夾雜的額外資訊
    /// </summary>
    public class OpenPointOfficialEmailInfo
    {
        
        /// <summary>
        /// APP 帳號
        /// </summary>
        public string OpenPointAppAccount { get; set; }

        /// <summary>
        /// 手機 APP 版本
        /// </summary>
        public string AppVersion { get; set; }

        /// <summary>
        /// 手機的版本
        /// </summary>
        public string Version { get; set; }
        
        /// <summary>
        /// 手機作業系統
        /// </summary>
        public string System { get; set; }

        /// <summary>
        /// 手機型號
        /// </summary>
        public string PhoneModel { get; set; }

        /// <summary>
        /// 反應內容
        /// </summary>
        public string Content { get; set; }


        /// <summary>
        /// 使用者資訊
        /// </summary>
        public ConcatableUser User { get; set; }

    }
}
