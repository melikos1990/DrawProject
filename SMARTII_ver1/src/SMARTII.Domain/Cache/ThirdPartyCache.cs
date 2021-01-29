using System;
using System.Web.Configuration;

namespace SMARTII.Domain.Cache
{
    public class ThirdPartyCache
    {
        private static readonly Lazy<ThirdPartyCache> LazyInstance = new Lazy<ThirdPartyCache>(() => new ThirdPartyCache());

        public static ThirdPartyCache Instance { get { return LazyInstance.Value; } }

        public ThirdPartyCache()
        {
            this.LDAPUrl = WebConfigurationManager.AppSettings["LDAPUrl"].ToString();
            this.VoiceSearchUrl = WebConfigurationManager.AppSettings["VOICE_SEARCH_URL"].ToString();
            this.VoiceMatchUrl = WebConfigurationManager.AppSettings["VOICE_MATCH_URL"].ToString();
            this.SMTPServerIP = WebConfigurationManager.AppSettings["SMTP_SERVER_IP"].ToString();
            this.SMTPServerPort = int.Parse(WebConfigurationManager.AppSettings["SMTP_SERVER_PORT"].ToString());
            this.SMSApiUrl = WebConfigurationManager.AppSettings["SMS_API_URL"].ToString();
        }
        
        public string LDAPUrl { get; set; }

        public string VoiceSearchUrl { get; set; }

        public string VoiceMatchUrl { get; set; }

        public string SMTPServerIP { get; set; }

        public int SMTPServerPort { get; set; }

        public string SMSApiUrl { get; set; }
    }
}
