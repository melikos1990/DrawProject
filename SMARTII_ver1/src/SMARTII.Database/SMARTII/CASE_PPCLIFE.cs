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
    
    public partial class CASE_PPCLIFE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CASE_PPCLIFE()
        {
            this.PPCLIFE_EFFECTIVE_SUMMARY = new HashSet<PPCLIFE_EFFECTIVE_SUMMARY>();
        }
    
        public string CASE_ID { get; set; }
        public int ITEM_ID { get; set; }
        public bool IS_IGNORE { get; set; }
        public Nullable<bool> ALLSAME_FINISH { get; set; }
        public Nullable<bool> DIFF_BATCHNO_FINISH { get; set; }
        public Nullable<bool> NOTHINE_BATCHNO_FINISH { get; set; }
        public System.DateTime CREATE_DATETIME { get; set; }
        public string CREATE_USERNAME { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PPCLIFE_EFFECTIVE_SUMMARY> PPCLIFE_EFFECTIVE_SUMMARY { get; set; }
    }
}
