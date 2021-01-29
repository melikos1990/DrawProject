using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Master
{
    public class Queue
    {
        public Queue()
        {
        }

        /// <summary>
        /// QUEUE 代號
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// QUEUE名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否啟用(預設:是)
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 節點代號
        /// </summary>
        public int NodeID { get; set; }

        /// <summary>
        /// 組織型態定義 (HEADQUARTER : 0 ; CC : 1 ; VENDOR : 2)
        /// </summary>
        public OrganizationType OrganizationType { get; set; }

        /// <summary>
        /// CC節點
        /// </summary>
        public CallCenterNode CallCenterNode { get; set; }
    }
}
