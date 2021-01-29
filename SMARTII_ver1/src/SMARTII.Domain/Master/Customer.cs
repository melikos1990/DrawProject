using SMARTII.Domain.Common;

namespace SMARTII.Domain.Master
{
    public class Customer
    {
        public Customer()
        {
        }

        /// <summary>
        /// 流水號
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 手機
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 市話
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 信箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 性別
        /// </summary>
        public GenderType? Gender { get; set; }

        /// <summary>
        /// 會員編號
        /// </summary>
        public string MemberID { get; set; }

        /// <summary>
        /// 企業別
        /// </summary>
        public int BUID { get; set; }
    }
}
