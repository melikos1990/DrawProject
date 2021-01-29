using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMARTII.Domain.Case;

namespace SMARTII.Domain.Notification
{
    public class PPCLifeEffectiveSummary
    {
        /// <summary>
        /// 流水編號      
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 商品編號      
        /// </summary>
        public int ItemID { get; set; }

        /// <summary>
        /// 達標規則
        /// </summary>
        public PPCLifeArriveType PPCLifeArriveType { get; set; }

        /// <summary>
        /// 批號
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 建立人員
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 案件
        /// </summary>
        public List<CasePPCLife> CasePPCLifes { get; set; }


    }
}
