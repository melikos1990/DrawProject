using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMARTII.Domain.Organization;

namespace SMARTII.ColdStone.Domain
{
    /// <summary>
    /// 官網來信 信件內容夾雜的額外資訊
    /// </summary>
    public class ColdStoneOfficialEmailInfo
    {

        /// <summary>
        /// 使用者資訊
        /// </summary>
        public ConcatableUser User { get; set; }
    }
}
