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
    
    public partial class CASE_COMPLAINED_USER
    {
        public int ID { get; set; }
        public string CASE_ID { get; set; }
        public Nullable<int> NODE_ID { get; set; }
        public string NODE_NAME { get; set; }
        public Nullable<byte> ORGANIZATION_TYPE { get; set; }
        public string PARENT_PATH_NAME { get; set; }
        public byte COMPLAINED_USER_TYPE { get; set; }
        public string USER_ID { get; set; }
        public string USER_NAME { get; set; }
        public Nullable<int> JOB_ID { get; set; }
        public string JOB_NAME { get; set; }
        public string STORE_NO { get; set; }
        public Nullable<int> BU_ID { get; set; }
        public string BU_NAME { get; set; }
        public byte UNIT_TYPE { get; set; }
        public byte NOTIFICATION_BEHAVIOR { get; set; }
        public string NOTIFICATION_KIND { get; set; }
        public string NOTIFICATION_REMARK { get; set; }
        public string ADDRESS { get; set; }
        public string MOBILE { get; set; }
        public string TELEPHONE { get; set; }
        public string TELEPHONE_BAK { get; set; }
        public Nullable<byte> GENDER { get; set; }
        public string EMAIL { get; set; }
        public string OWNER_USERNAME { get; set; }
        public string OWNER_USER_PHONE { get; set; }
        public string OWNER_JOB_NAME { get; set; }
        public string OWNER_USER_EMAIL { get; set; }
        public string SUPERVISOR_USERNAME { get; set; }
        public string SUPERVISOR_USER_PHONE { get; set; }
        public string SUPERVISOR_USER_EMAIL { get; set; }
        public string SUPERVISOR_JOB_NAME { get; set; }
        public string SUPERVISOR_NODE_NAME { get; set; }
    
        public virtual CASE CASE { get; set; }
    }
}
