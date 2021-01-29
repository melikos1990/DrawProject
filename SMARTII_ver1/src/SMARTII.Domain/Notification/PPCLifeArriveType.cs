using System.ComponentModel;

namespace SMARTII.Domain.Notification
{
    public enum PPCLifeArriveType
    {
        /// <summary>
        /// 同批號同產品
        /// </summary>
        [Description("同批號同產品")]
        AllSame = 0,

        /// <summary>
        /// 不同批號同產品
        /// </summary>
        [Description("不同批號同產品")]
        DiffBatcNo = 1,

        /// <summary>
        /// 無批號同產品
        /// </summary>
        [Description("無批號同產品")]
        NothingBatchNo = 2
    }
}
