using System.ComponentModel;

namespace SMARTII.Domain.Case
{
    public enum CaseMutipleTicketType
    {
        /// <summary>
        /// 只允許一張單一案件
        /// </summary>
        [Description("只允許一張單一案件")]
        One = 0,

        /// <summary>
        /// 可以多張單
        /// </summary>
        [Description("可以多張單")]
        Multiple = 1
    }
}
