using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMARTII.Domain.Notification;

namespace SMARTII.Domain.Case
{
    public class CasePPCLife
    {
        /// <summary>
        /// 案件編號
        /// </summary>
        public string CaseID { get; set; }

        /// <summary>
        /// 商品編號
        /// </summary>
        public int ItemID { get; set; }

        /// <summary>
        /// 是否忽略
        /// </summary>
        public bool IsIgnore { get; set; }

        /// <summary>
        /// 同批號同商品 是否處理
        /// </summary>
        public bool AllSameFinish { get; set; }

        /// <summary>
        /// 不同批號同商品 是否處理
        /// </summary>
        public bool DiffBatchNoFinish { get; set; }

        /// <summary>
        /// 無批號同商品 是否處理
        /// </summary>
        public bool NothingBatchNoFinish { get; set; }     

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 建立人員
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 案件內容
        /// </summary>
        public string CaseContent { get; set; }

        /// <summary>
        /// 規則流水編號      
        /// </summary>
        public int EffectiveID { get; set; }

        /// <summary>
        /// 符合規則
        /// </summary>
        public List<PPCLifeEffectiveSummary> PPCLifeEffectiveSummaries { get; set; }

    }
}
