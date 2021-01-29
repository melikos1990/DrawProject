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
    
    public partial class HEADQUARTERS_NODE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HEADQUARTERS_NODE()
        {
            this.HEADQUARTERS_NODE1 = new HashSet<HEADQUARTERS_NODE>();
            this.CALLCENTER_NODE = new HashSet<CALLCENTER_NODE>();
            this.VENDOR_NODE = new HashSet<VENDOR_NODE>();
        }
    
        public int NODE_ID { get; set; }
        public byte ORGANIZATION_TYPE { get; set; }
        public int LEFT_BOUNDARY { get; set; }
        public int RIGHT_BOUNDARY { get; set; }
        public string NAME { get; set; }
        public System.DateTime CREATE_DATETIME { get; set; }
        public string CREATE_USERNAME { get; set; }
        public Nullable<System.DateTime> UPDATE_DATETIME { get; set; }
        public string UPDATE_USERNAME { get; set; }
        public int DEPTH_LEVEL { get; set; }
        public string NODE_TYPE_KEY { get; set; }
        public Nullable<int> NODE_TYPE { get; set; }
        public bool IS_ENABLED { get; set; }
        public Nullable<int> ENTERPRISE_ID { get; set; }
        public string PARENT_PATH { get; set; }
        public Nullable<int> PARENT_ID { get; set; }
        public Nullable<int> BU_ID { get; set; }
        public string NODE_KEY { get; set; }
    
        public virtual ENTERPRISE ENTERPRISE { get; set; }
        public virtual ORGANIZATION_NODE_DEFINITION ORGANIZATION_NODE_DEFINITION { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HEADQUARTERS_NODE> HEADQUARTERS_NODE1 { get; set; }
        public virtual HEADQUARTERS_NODE HEADQUARTERS_NODE2 { get; set; }
        public virtual STORE STORE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CALLCENTER_NODE> CALLCENTER_NODE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VENDOR_NODE> VENDOR_NODE { get; set; }
    }
}
