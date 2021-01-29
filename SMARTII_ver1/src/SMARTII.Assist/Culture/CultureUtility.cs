using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Web;
using SMARTII.Domain.Cache;
using SMARTII.Resource.Tag;

namespace SMARTII.Assist.Culture
{
    public static class CultureUtility
    {
        private static List<ResourceManager> _ResourceManagers;

        static CultureUtility()
        {
            Initialize();
        }

        public static void Initialize()
        {
            _ResourceManagers = new List<ResourceManager>()
            {
                Account_lang.ResourceManager,
                Billboard_lang.ResourceManager,
                CallCenterNode_lang.ResourceManager,
                CallCenterSummary_lang.ResourceManager,
                Case_lang.ResourceManager,
                CaseTemplate_lang.ResourceManager,
                CaseApply_lang.ResourceManager,
                CaseAssignGroup_lang.ResourceManager,
                CaseFinishReason_lang.ResourceManager,
                CaseNotice_lang.ResourceManager,
                CaseRemind_lang.ResourceManager,
                CaseTag_lang.ResourceManager,
                CaseTemplate_lang.ResourceManager,
                CaseWarning_lang.ResourceManager,
                Common_lang.ResourceManager,
                Enterprise_lang.ResourceManager,
                HeaderQuarterNode_lang.ResourceManager,
                HeaderQuarterSummary_lang.ResourceManager,
                Item_lang.ResourceManager,
                Job_lang.ResourceManager,
                KMClassification_lang.ResourceManager,
                NodeDefinition_lang.ResourceManager,
                NotificationGroup_lang.ResourceManager,
                NotificationGroupSender_lang.ResourceManager,
                OfficialEmail_lang.ResourceManager,
                OfficialEmailGroup_lang.ResourceManager,
                PPCLIFE_Notification_lang.ResourceManager,
                QuestionClassification_lang.ResourceManager,
                QuestionClassificationAnswer_lang.ResourceManager,
                QuestionClassificationGuide_lang.ResourceManager,
                Report_lang.ResourceManager,
                Role_lang.ResourceManager,
                Store_lang.ResourceManager,
                SysCommon_lang.ResourceManager,
                SystemLog_lang.ResourceManager,
                SystemParameter_lang.ResourceManager,
                User_lang.ResourceManager,
                UserParameter_lang.ResourceManager,
                VendorNode_lang.ResourceManager,
                VendorSummary_lang.ResourceManager,
                WorkSchedule_lang.ResourceManager,

            };
        }

        public static void DetectionCulture(this HttpRequest request)
        {
            // 取得語系
            // 從Http Header
            var cultureName = request.Headers[GlobalizationCache.CultureKey];

            // 偵測語系,並轉換顯示
            if (string.IsNullOrEmpty(cultureName) == false)
            {
                // 語系定義相關轉換功能
                System.Threading.Thread.CurrentThread.CurrentCulture =
                    new System.Globalization.CultureInfo(cultureName);

                // 設定語系定義檔
                System.Threading.Thread.CurrentThread.CurrentUICulture =
                    new System.Globalization.CultureInfo(cultureName);
            }
        }

        public static string GetSpecificLang(this string key, CultureInfo culture)
        {
            foreach (var manager in _ResourceManagers)
            {
                var lang = manager.GetString(key, culture);

                if (string.IsNullOrEmpty(lang))
                    continue;

                return lang;
            }

            return string.Empty;
        }
    }
}
