using System;
using System.Web.Configuration;

namespace SMARTII.Domain.Cache
{
    public class SecurityCache
    {
        private static readonly Lazy<SecurityCache> LazyInstance = new Lazy<SecurityCache>(() => new SecurityCache());

        public static SecurityCache Instance { get { return LazyInstance.Value; } }

        public static readonly int PasswordResetLimitCount = 5;

        public static readonly int ErrorCountLimit = 3;

        public SecurityCache()
        {
            this.TokenSecurityKey = WebConfigurationManager.AppSettings["TokenSecurityKey"].ToString();
            this.PersonalInfoSecurityKey = WebConfigurationManager.AppSettings["PersonalInfoSecurityKey"].ToString();
            //this.DefaultPassword = WebConfigurationManager.AppSettings["DefaultPassword"].ToString();
            //this.PersonalEncryptKey = WebConfigurationManager.AppSettings["PERSONAL_SECUTIRY_ENCRYPT_KEY"].ToString();
            this.ConnectionStringSecurityKey = WebConfigurationManager.AppSettings["ConnectionStringSecurityKey"].ToString();
        }

        public string TokenSecurityKey { get; set; }

        public string PersonalInfoSecurityKey { get; set; }

        //public string DefaultPassword { get; set; }

        //public string PersonalEncryptKey { get; set; }

        public string ConnectionStringSecurityKey { get; set; }
    }
}
