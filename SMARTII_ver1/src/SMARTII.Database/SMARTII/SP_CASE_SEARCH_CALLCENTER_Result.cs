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
    
    public partial class SP_CASE_SEARCH_CALLCENTER_Result
    {
        public string NodeKey { get; set; }
        public string NodeName { get; set; }
        public Nullable<byte> SourceType { get; set; }
        public string CaseID { get; set; }
        public Nullable<System.DateTime> CreateDateTime { get; set; }
        public Nullable<System.DateTime> IncomingDateTime { get; set; }
        public Nullable<System.DateTime> ExpectDateTime { get; set; }
        public string ApplyUserName { get; set; }
        public Nullable<byte> CaseType { get; set; }
        public string CaseWarningName { get; set; }
        public Nullable<bool> IsPrevention { get; set; }
        public Nullable<bool> IsAttension { get; set; }
        public string CaseContent { get; set; }
        public Nullable<System.DateTime> PromiseDateTime { get; set; }
        public string FinishContent { get; set; }
        public Nullable<System.DateTime> FinishDateTime { get; set; }
        public Nullable<int> ClassificationID { get; set; }
    }
}
