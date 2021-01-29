namespace SMARTII.Domain.Cache
{
    public static class EssentialCache
    {
        #region 職稱定義識別值

        public class JobValue
        {
            public static string OFC = "OFC";
            public static string OWNER = "OWNER";

            public static string[] All = new string[] {
            OFC,
            OWNER,
            };
        }

        #endregion 職稱定義識別值

        #region 組織定義識別值

        public class NodeDefinitionValue
        {
            public static string BusinessUnit = "BU";
            public static string Store = "STORE";
            public static string Group = "GROUP";
            public static string Vendor = "VENDOR";
            public static string VendorGroup = "VENDOR_GROUP";
            public static string CallCenter = "CALLCENTER";

            public static string[] All = new string[] {
            BusinessUnit,
            Store,
            Group,
            Vendor,
            VendorGroup,
            CallCenter
            };
        }

        #endregion 組織定義識別值

        #region 企業名稱識別值

        public class BusinessKeyValue
        {
            public static string COMMONBU = "000";
            public static string PPCLIFE = "005";
            public static string ASO = "007";
            public static string ColdStone = "003";
            public static string MisterDonut = "002";
            public static string EShop = "004";
            public static string ICC = "006";
            public static string OpenPoint = "008";
            public static string _21Century = "001";
            public static string FORTUNE = "009";

            public static string[] All = new string[] {
                PPCLIFE,
                ASO,
                MisterDonut,
                ColdStone,
                EShop,
                ICC,
                OpenPoint,
                _21Century,
                FORTUNE
            };
        }

        #endregion 企業名稱識別值

        #region 協定
        public class MailProtocolKeyValue
        {
            public static string POP3 = "POP3";
            public static string OFFICE365 = "OFFICE365";

            public static string[] All = new string[] {
                POP3,
                OFFICE365,
            };
        }
        #endregion

        #region 系統參數

        public class LayoutValue
        {
            // 動態表單物件
            public static string CaseTemplate = "CASE_TEMPLATE";
            public static string ItemDeatilTemplate = "ITEM_DETAIL";
            public static string ItemQueryTemplate = "ITEM_QUERY";
            public static string StoreDeatilTemplate = "STORE_DETAIL";
            public static string StoreQueryTemplate = "STORE_QUERY";
            public static string CaseFinishTemplate = "CASE_FINISH";
            public static string PrecaseOverDueDeleteTemplate = "PRECASE_OVERDUEDELETE";
            public static string StoreTypeTemplate = "STORE_TYPE";
            public static string CaseOtherTemplate = "CASE_OTHER";
            public static string MailCaseAssignmentNotcieTypeTemplate = "MAIL_CASE_ASSIGNMENTNOTICE_TYPE";
        }

        public class WorkValue
        {

            public static string WorkOffDay = "WORKOFFDAY";
            public static string ServiceTime = "SERVICETIME";

        }

        public class CaseValue
        {
            public static string SOURCE_RELATEDCASE = "SOURCE_RELATEDCASE";
            public static string CASE_ASSIGNMENTINVOICECOMPLAIN = "CASE_ASSIGNMENTINVOICECOMPLAIN";
            public static string CASE_ASSIGNMENTINVOICECOMPLAIN_ZIPCODE = "CASE_ASSIGNMENTINVOICECOMPLAIN_ZIPCODE";
            public static string CASE_ALLOW_FASTCLOSE = "CASE_ALLOWFASTCLOSE";
            public static string CASE_MULTIASSIGNMENTINVOICECOMPLAIN = "CASE_MULTIASSIGNMENTINVOICECOMPLAIN";
            public static string CASE_SOURCE = "CASE_SOURCE";
            public static string DAILYREPORT_ZIPCODE = "DAILYREPORT_ZIPCODE";
            public static string CASE_FINISHRETURN = "CASE_FINISHRETURN";
            public static string CASE_GETASSIGNMENTUSER = "CASE_GETASSIGNMENTUSER";
        }

        public class MasterValue
        {
            public static string STORE_TYPE = "STORE_TYPE";
        }

        public class LoginValue
        {
            public static string SYSTEM_SETTING = "SYSTEM_SETTING";
            public static string OUTSIDE_AD_ALLOW = "OUTSIDE_AD_ALLOW";
        }

        public class EmailValue
        {
            public static string EMAIL_DEFAULT_TITLE = "EMAIL_DEFAULT_TITLE";
        }

        public class CaseFinishTimeValue
        {
            public static string CASE_UNCLOSENOTIFICATION = "CASE_UNCLOSENOTIFICATION";
        }

        public class LDAPValue
        {
            public static string LDAP_AUTHENTICATION = "LDAP_AUTHENTICATION";
            public static string USER = "USER";
        }

        #endregion 系統參數

        #region 各BU信件認養歷程種類歸屬
        public class OfficialEmailAssignmentValue
        {
            /// <summary>
            /// 一般通知
            /// </summary>
            public static string NoticeTemplate = "0";

            /// <summary>
            /// 單位溝通
            /// </summary>
            public static string CommunicateTemplate = "3";

            public static string[] All = new string[] {
                NoticeTemplate,
                CommunicateTemplate,
            };
        }

        #endregion 各BU信件認養歷程種類歸屬

        #region PPCLife客制條件

        /// <summary>
        /// 問題分類 Code
        /// </summary>
        public class PPCLifeCustomerValue
        {
            /// <summary>
            /// 商品變質
            /// </summary>
            public static string TH0277 = "TH0277";

            /// <summary>
            /// 包裝標示不清
            /// </summary>
            public static string TH0278 = "TH0278";

            /// <summary>
            /// 商品瑕疵
            /// </summary>
            public static string TH0279 = "TH0279";

            /// <summary>
            /// 其他
            /// </summary>
            public static string TH0280 = "TH0280";

            /// <summary>
            /// 包裝標示不清
            /// </summary>
            public static string TH0285 = "TH0285";

            /// <summary>
            /// 商品瑕疵
            /// </summary>
            public static string TH0287 = "TH0287";

            /// <summary>
            /// 過敏或身體不適
            /// </summary>
            public static string TH0289 = "TH0289"; 

            /// <summary>
            /// 其他
            /// </summary>
            public static string TH0291 = "TH0291";

            /// <summary>
            /// 重大品質異常(指內容物)
            /// </summary>
            public static string TH0290 = "TH0290";

            /// <summary>
            /// 內容物(數)不符
            /// </summary>
            public static string TH0288 = "TH0288";

            /// <summary>
            /// 商品變質
            /// </summary>
            public static string TH0286 = "TH0286";

            /// <summary>
            /// 未來商品
            /// </summary>
            public static string TH0284 = "TH0284";

            /// <summary>
            /// 活動諮詢－PPC
            /// </summary>
            public static string TH0098 = "TH0098";

            /// <summary>
            /// 通路販售需求洽詢
            /// </summary>
            public static string TH0091 = "TH0091";

            /// <summary>
            /// 產品諮詢－通路
            /// </summary>
            public static string TH0087 = "TH0087";

            /// <summary>
            /// 產品諮詢－專業
            /// </summary>
            public static string TH0086 = "TH0086";

            /// <summary>
            /// 產品諮詢－一般
            /// </summary>
            public static string TH0085 = "TH0085";

            /// <summary>
            /// 販售地點/時間查詢
            /// </summary>
            public static string TH0084 = "TH0084";

            /// <summary>
            /// 統藥 特殊問題分類(判斷特定處置原因)
            /// </summary>
            public static string[] SpecialClassification = new string[] {
                TH0291,
                TH0290,
                TH0289,
                TH0288,
                TH0287,
                TH0286,
                TH0285,
                TH0284,
                TH0279,
                TH0098,
                TH0091,
                TH0087,
                TH0086,
                TH0085,
                TH0084
            };

            public static string[] All = new string[] {
                TH0277,
                TH0278,
                TH0279,
                TH0280,
                TH0285,
                TH0287,
                TH0289,
                TH0291
            };
        }
        

        #endregion 各BU信件認養歷程種類歸屬

        #region 組織節點Range
        public class NodeKeyValue
        {
            /// <summary>
            /// 21Century - 通路
            /// </summary>
            public static string _21PATHWAY = "101";
            /// <summary>
            /// 酷聖石 - 通路
            /// </summary>
            public static string PATHWAY = "301";
            /// <summary>
            /// 統藥部門-代理品牌
            /// </summary>
            public static string GENERATE = "501";
            /// <summary>
            ///  統藥部門-自我品牌
            /// </summary>
            public static string OWNBRAND = "502";
            /// <summary>
            /// 統藥部門-醫學美容
            /// </summary>
            public static string MEDICAL = "503";
            /// <summary>
            /// ASO-網購相關
            /// </summary>
            public static string ONLINE_SHOPPING = "701";
            /// <summary>
            /// ASO-BESO品牌
            /// </summary>
            public static string BESO_BRAND = "702";

        }
        #endregion

        #region 問題分類識別值
        public class CodeValue
        {
            /// <summary>
            /// 統一藥品 -一般客訴分類
            /// </summary>
            public static string GENERAL = "SE0025";
            /// <summary>
            /// 統一藥品 -重大客訴分類
            /// </summary>
            public static string URGENT = "SE0060";
            /// <summary>
            /// ASO - 生活誌問題
            /// </summary>
            public static string LIFE_ISSUES = "FI0043";
            /// <summary>
            /// ASO - 異業合作/團銷
            /// </summary>
            public static string GROUP_SALES = "TH0652";
            /// <summary>
            /// ASO - 維修費用查詢
            /// </summary>
            public static string REPAIR_COST = "SE0127";
            /// <summary>
            /// ASO - 維修
            /// </summary>
            public static string REPAIR = "SE0134";
            /// <summary>
            /// 酷聖石 客訴分類
            /// </summary>
            public static string GENERAL_CS = "FI0018";
            /// <summary>
            /// 21 客訴分類
            /// </summary>
            public static string GENERAL_21 = "FI0007";
        }
        #endregion

        #region 統藥-品牌商品與問題歸類
        public class ReasonClassValue
        {
            /// <summary>
            /// 統一藥品　回覆顧客方式
            /// </summary>
            public static string REPLY = "C001";
            /// <summary>
            /// 統一藥品　問題要因
            /// </summary>
            public static string FACTORS = "Q001";
            /// <summary>
            /// ASO　滿意度
            /// </summary>
            public static string SATISFACTION = "S001";
            /// <summary>
            /// 21世紀 改善方式
            /// </summary>
            public static string RECOGNIZE = "R001";
        }
        #endregion

        #region Batch
        public class BatchValue
        {
            public static string PPCLIFEGroupDaily = "統一藥品日報表";

            public static string PPCLIFEGroupMonth = "統一藥品月報表";

            public static string PPCLIFEMedicineGroup = "統一藥品報表 醫學美容";//統一藥品報表 醫學美容

            public static string PPCLIFEOwnBrandGroup = "統一藥品報表 美麗事業";//統一藥品報表 自有品牌

            public static string PPCLIFEProxyBrandGroup = "統一藥品報表 保健事業";//統一藥品報表 代理品牌

            public static string PPCLIFEBrandGroup = "統藥品牌商品與問題歸類";

            public static string ColdStoneGroupDaily = "酷聖石0800客服日報表";

            public static string ASOGroupDaily = "ASO 日報表寄送";

            public static string EShopGroupDaily = "統一藥品eShop日報表";

            public static string MisterDonutGroupDaily = "多拿滋日報表";

            public static string OpenPointGroupDaily = "iCashPoint日報";

            public static string _21CenterGroupDaily = "21世紀日報表";

            public static string ICCGroupDaily = "InComm卡日報表";


        }

        #endregion

        #region 使用者管理密碼識別值
        public class UserPasswordKeyValue
        {
            public static string SYSTEM_SETTING = "SYSTEM_SETTING";
            public static string USER_DEFAULTCODE = "USER_DEFAULTCODE";
        }
        #endregion

    }
}
