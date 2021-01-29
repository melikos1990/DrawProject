using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Configuration;

namespace SMARTII.Domain.Cache
{
    public class GlobalizationCache
    {
        // HTTP 表頭
        public static string CultureKey = "Culture";

        public static string JobPositionKey = "JobPosition";

        // 系統名稱
        public static string APName = "SMART 系統";

        private static readonly Lazy<GlobalizationCache> LazyInstance = new Lazy<GlobalizationCache>(() => new GlobalizationCache());

        public static GlobalizationCache Instance { get { return LazyInstance.Value; } }

        public GlobalizationCache()
        {
            LoadAssembly();

            this.AdminMailAddress = WebConfigurationManager.AppSettings["ADMIN_MAIL_ADDRESS"].ToString();
            this.AdminMailReceiverAddress = WebConfigurationManager.AppSettings["ADMIN_MAIL_RECEIVER_ADDRESS"].ToString();
            this.ImportStoreFilePath = WebConfigurationManager.AppSettings["IMPORT_STORE_FILEPATH"].ToString();
            this.ImportStorePersonnelFilePath = WebConfigurationManager.AppSettings["IMPORT_STORE_PERSONNEL_FILEPATH"].ToString();
            this.ImportStoreInformationFilePath = WebConfigurationManager.AppSettings["IMPORT_STORE_INFORMATION_FILEPATH"].ToString();
            this.ImportUserFilePath = WebConfigurationManager.AppSettings["IMPORT_USER_FILEPATH"].ToString();
            this.ImportQuestionClassificationFilePath = WebConfigurationManager.AppSettings["IMPORT_QUESTION_CLASSIFICATION_FILEPATH"].ToString();
        }

        public string AdminMailAddress { get; set; }

        public string AdminMailReceiverAddress { get; set; }

        public string ImportStoreFilePath { get; set; }
        public string ImportStorePersonnelFilePath { get; set; }
        public string ImportStoreInformationFilePath { get; set; }
        public string ImportUserFilePath { get; set; }
        public string ImportQuestionClassificationFilePath { get; set; }


        public Dictionary<string, Assembly> AssemblyDict { get; set; }

        public void LoadAssembly()
        {
            AssemblyDict = new Dictionary<string, Assembly>()
            {
                { "MAIN" ,  Assembly.Load("SMARTII") },
                { "ASO" ,  Assembly.Load("SMARTII.ASO") },
                { "COMMON_BU" ,  Assembly.Load("SMARTII.COMMON_BU") },
                { "PPCLIFE" ,  Assembly.Load("SMARTII.PPCLIFE")},
                { "21Century" ,  Assembly.Load("SMARTII.21Century")},
                { "FORTUNE" ,  Assembly.Load("SMARTII.FORTUNE")}
            };
        }

        // 回復主旨
        public static string ReplyMailTilie = "RE: ";
    }
}
