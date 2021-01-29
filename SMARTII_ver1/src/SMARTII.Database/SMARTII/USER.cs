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
    
    public partial class USER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public USER()
        {
            this.PERSONAL_NOTIFICATION = new HashSet<PERSONAL_NOTIFICATION>();
            this.NODE_JOB = new HashSet<NODE_JOB>();
            this.OFFICIAL_EMAIL_GROUP = new HashSet<OFFICIAL_EMAIL_GROUP>();
            this.ROLE = new HashSet<ROLE>();
        }
    
        public string USER_ID { get; set; }
        public string ACCOUNT { get; set; }
        public string PASSWORD { get; set; }
        public string NAME { get; set; }
        public string EMAIL { get; set; }
        public string MOBILE_PUSH_ID { get; set; }
        public int ERROR_PASSWORD_COUNT { get; set; }
        public Nullable<System.DateTime> LAST_CHANGE_PASSWORD_DATETIME { get; set; }
        public string PAST_PASSWORD_RECORD { get; set; }
        public bool IS_AD { get; set; }
        public bool IS_ENABLED { get; set; }
        public Nullable<System.DateTime> LOCKOUT_DATETIME { get; set; }
        public System.DateTime CREATE_DATETIME { get; set; }
        public string CREATE_USERNAME { get; set; }
        public string UPDATE_USERNAME { get; set; }
        public Nullable<System.DateTime> UPDATE_DATETIME { get; set; }
        public string FEATURE { get; set; }
        public string TELEPHONE { get; set; }
        public string IMAGE_PATH { get; set; }
        public System.DateTime VERSION { get; set; }
        public string EXT { get; set; }
        public byte PLATFORM_TYPE { get; set; }
        public string MOBILE { get; set; }
        public string ADDRESS { get; set; }
        public Nullable<System.DateTime> ACTIVE_START_DATETIME { get; set; }
        public Nullable<System.DateTime> ACTIVE_END_DATETIME { get; set; }
        public bool IS_SYSTEM_USER { get; set; }
        public string MEMBER_ID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PERSONAL_NOTIFICATION> PERSONAL_NOTIFICATION { get; set; }
        public virtual USER_PARAMETER USER_PARAMETER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NODE_JOB> NODE_JOB { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OFFICIAL_EMAIL_GROUP> OFFICIAL_EMAIL_GROUP { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ROLE> ROLE { get; set; }
    }
}
