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
    
    public partial class CASE_SOURCE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CASE_SOURCE()
        {
            this.CASE = new HashSet<CASE>();
        }
    
        public string SOURCE_ID { get; set; }
        public int NODE_ID { get; set; }
        public byte ORGANIZATION_TYPE { get; set; }
        public bool IS_TWICE_CALL { get; set; }
        public bool IS_PREVENTION { get; set; }
        public Nullable<System.DateTime> INCOMING_DATETIME { get; set; }
        public string RELATION_CASE_SOURCE_ID { get; set; }
        public string REMARK { get; set; }
        public string RELATION_CASE_IDs { get; set; }
        public string VOICE_ID { get; set; }
        public string VOICE_LOCATOR { get; set; }
        public byte SOURCE_TYPE { get; set; }
        public System.DateTime CREATE_DATETIME { get; set; }
        public string CREATE_USERNAME { get; set; }
        public Nullable<System.DateTime> UPDATE_DATETIME { get; set; }
        public string UPDATE_USERNAME { get; set; }
        public Nullable<int> GROUP_ID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CASE> CASE { get; set; }
        public virtual CASE_SOURCE_USER CASE_SOURCE_USER { get; set; }
    }
}
