using System;
using System.Configuration;
using SMARTII.Domain.Security;

namespace SMARTII.Domain.Cache
{
    public class DataAccessCache
    {
        private static readonly Lazy<DataAccessCache> LazyInstance = new Lazy<DataAccessCache>(() => new DataAccessCache());

        public static DataAccessCache Instance { get { return LazyInstance.Value; } }

        public DataAccessCache()
        {
            this.SmartIIConn = ConfigurationManager.ConnectionStrings["SMARTIIEntities"].ConnectionString.AesDecryptBase64(SecurityCache.Instance.ConnectionStringSecurityKey);
        }

        public string SmartIIConn { get; set; }
    }
}
