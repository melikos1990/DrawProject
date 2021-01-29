using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMARTII.Domain.Cache
{
    public class WebComponentsCache
    {

        private static readonly Lazy<WebComponentsCache> LazyInstance = new Lazy<WebComponentsCache>(() => new WebComponentsCache());

        public static WebComponentsCache Instance { get { return LazyInstance.Value; } }


        public WebComponentsCache() { }


        public string TryGet(string key)
        {
            try
            {
                return this.Components[key];
            }
            catch (Exception ex)
            {
                return key;
            }
        }

        public Dictionary<string, string> Components = new Dictionary<string, string>()
        {
            { "CaseTemplate","範本主檔管理"},
            { "Stores","門市資訊管理"},
            { "Item","商品主檔維護"},
            { "QuestionClassificationAnswer","常用語設定"},
            { "QuestionClassificationGuide","流程引導設定"},
            { "QuestionCategory","問題分類"},
            { "NotificationGroup","提醒群組設定"},
            { "Billboard","公佈欄設定"},
            { "BillboardDisplay","公佈欄顯示"},
            { "CaseAssignGroup","派工群組設定 - 明細頁面"},
            { "CaseTag","案件標籤設定"},
            { "CaseFinishedReason","處理原因主檔"},
            { "CaseRemind","案件追蹤示警管理"},
            { "OfficialEmailGroup","官網來信提醒"},
            { "CaseWarning","案件時效管理"},
            { "WorkSchedule","特定假日設定"},
            { "Enterprise","集團主檔"},
            { "Role","操作權限管理"},
            { "User","使用者管理"},
            { "UserParameter","個人參數設定"},
            { "SystemParameter","系統參數設定 - 明細頁面"},
            { "SystemLog","系統紀錄追蹤"},
            { "PersonalChangePassword","更改密碼"},
            { "C1","案件立案"},
            { "OfficialEmailAdopt","認養外部工單(Email)"},
            { "NotificationGroupSender","提醒統計通知"},
            { "PpclifeEffectiveSummary","統藥大量叫修"},
            { "NodeDefinition","組織節點定義檔"},
            { "HeaderquarterNode","組織設定（總部）"},
            { "CallCenterNode","客服組織維護"},
            { "VendorNode","組織設定（廠商）"},
            { "CaseApply","代理人分配"},
            { "CaseNotice","代理人通知"},
            { "CallCenterCaseSearch","案件查詢(客服使用)"},
            { "HeaderQurterNodeStoreCaseSearch","案件查詢(總部 門市)"},
            { "CallCenterAssignmentSearch","轉派查詢(客服)"},
            { "HeaderqurterStoreAssignmentSearch","轉派查詢(總部/門市)"},
            { "VendorAssignmentSearch","轉派查詢(廠商)"},
            { "Km","常見問題討論區"},
            { "DailyReport","日報表"},
            { "AsoDailyReport","阿瘦日報表"}
        };

    }
}
