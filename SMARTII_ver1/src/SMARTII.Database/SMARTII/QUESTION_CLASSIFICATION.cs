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
    
    public partial class QUESTION_CLASSIFICATION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public QUESTION_CLASSIFICATION()
        {
            this.QUESTION_CLASSIFICATION_ANSWER = new HashSet<QUESTION_CLASSIFICATION_ANSWER>();
            this.QUESTION_CLASSIFICATION1 = new HashSet<QUESTION_CLASSIFICATION>();
        }
    
        public int NODE_ID { get; set; }
        public int ID { get; set; }
        public Nullable<int> PARENT_ID { get; set; }
        public string NAME { get; set; }
        public bool IS_ENABLED { get; set; }
        public string UPDATE_USERNAME { get; set; }
        public System.DateTime CREATE_DATETIME { get; set; }
        public string CREATE_USERNAME { get; set; }
        public Nullable<System.DateTime> UPDATE_DATETIME { get; set; }
        public byte ORGANIZATION_TYPE { get; set; }
        public int LEVEL { get; set; }
        public int ORDER { get; set; }
        public string CODE { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QUESTION_CLASSIFICATION_ANSWER> QUESTION_CLASSIFICATION_ANSWER { get; set; }
        public virtual QUESTION_CLASSIFICATION_GUIDE QUESTION_CLASSIFICATION_GUIDE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QUESTION_CLASSIFICATION> QUESTION_CLASSIFICATION1 { get; set; }
        public virtual QUESTION_CLASSIFICATION QUESTION_CLASSIFICATION2 { get; set; }
    }
}
