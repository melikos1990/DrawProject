//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace SMARTII.Database.SMARTII
{
    using System;
    using System.Collections.Generic;
    
    public partial class NOTIFICATION_GROUP
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NOTIFICATION_GROUP()
        {
            this.NOTIFICATION_GROUP_RESUME = new HashSet<NOTIFICATION_GROUP_RESUME>();
            this.NOTIFICATION_GROUP_USER = new HashSet<NOTIFICATION_GROUP_USER>();
        }
    
        public int ID { get; set; }
        public string NAME { get; set; }
        public int NODE_ID { get; set; }
        public byte ORGANIZATION_TYPE { get; set; }
        public Nullable<int> ITEM_ID { get; set; }
        public Nullable<int> QUESTION_CLASSIFICATION_ID { get; set; }
        public byte CALC_MODE { get; set; }
        public int ALERT_COUNT { get; set; }
        public int ALERT_CYCLE_DAY { get; set; }
        public int ACTUAL_COUNT { get; set; }
        public bool IS_ARRIVE { get; set; }
        public bool IS_NOTIFICATED { get; set; }
        public string UPDATE_USERNAME { get; set; }
        public System.DateTime CREATE_DATETIME { get; set; }
        public string CREATE_USERNAME { get; set; }
        public Nullable<System.DateTime> UPDATE_DATETIME { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NOTIFICATION_GROUP_RESUME> NOTIFICATION_GROUP_RESUME { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NOTIFICATION_GROUP_USER> NOTIFICATION_GROUP_USER { get; set; }
    }
}
