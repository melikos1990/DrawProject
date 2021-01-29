using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMARTII.Domain.Data;

namespace SMARTII.Domain.Report
{
    /// <summary>
    /// 案件查詢(總部、門市)
    /// </summary>
    public class ExcelCaseHSList : DynamicSerializeObject
    {
        /// <summary>
        /// 企業別
        /// </summary>
        public string NodeName { get; set; }
        /// <summary>
        /// 案件來源
        /// </summary>
        public string CaseSourceType { get; set; }
        /// <summary>
        /// 案件編號
        /// </summary>
        public string CaseID { get; set; }
        /// <summary>
        /// 立案時間
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 來源時間
        /// </summary>
        public string IncomingDateTime { get; set; }
        /// <summary>
        /// 負責人
        /// </summary>
        public string ApplyUserName { get; set; }
        /// <summary>
        /// 案件狀態
        /// </summary>
        public string CaseType { get; set; }
        /// <summary>
        /// 案件等級
        /// </summary>
        public string CaseWarningName { get; set; }
        /// <summary>
        /// 預立案
        /// </summary>
        public string IsPrevention { get; set; }
        /// <summary>
        /// 關注案件
        /// </summary>
        public string IsAttension { get; set; }
        /// <summary>
        /// 反應者型態
        /// </summary>
        public string ConcatUnitType { get; set; }
        /// <summary>
        /// 反應者(消費者)
        /// </summary>
        public string ConcatCustomerName { get; set; }
        /// <summary>
        /// 手機(消費者)
        /// </summary>
        public string ConcatCustomerMobile { get; set; }
        /// <summary>
        /// 電話1(消費者)
        /// </summary>
        public string ConcatCustomerTelephone1 { get; set; }
        /// <summary>
        /// 電話2(消費者)
        /// </summary>
        public string ConcatCustomerTelephone2 { get; set; }
        /// <summary>
        /// MAIL(消費者)
        /// </summary>
        public string ConcatCustomerMail { get; set; }
        /// <summary>
        /// 地址(消費者)
        /// </summary>
        public string ConcatCustomerAddress { get; set; }
        /// <summary>
        /// 門市(門市)
        /// </summary>
        public string ConcatStore { get; set; }
        /// <summary>
        /// 姓名(門市)
        /// </summary>
        public string ConcatStoreName { get; set; }
        /// <summary>
        /// 電話(門市)
        /// </summary>
        public string ConcatStoreTelephone { get; set; }
        /// <summary>
        /// 組織單位(組織)
        /// </summary>
        public string ConcatOrganization { get; set; }
        /// <summary>
        /// 姓名(組織)
        /// </summary>
        public string ConcatOrganizationName { get; set; }
        /// <summary>
        /// 電話(組織)
        /// </summary>
        public string ConcatOrganizationTelephone { get; set; }

        /// <summary>
        /// 被反應者類型
        /// </summary>
        public string ComplainedUnitType { get; set; }
        /// <summary>
        /// 門市(門市)
        /// </summary>
        public string ComplainedStore { get; set; }
        /// <summary>
        /// 門市區經理(門市)
        /// </summary>
        public string ComplainedStoreSupervisorUserName { get; set; }
        /// <summary>
        /// 門市負責人(門市)
        /// </summary>
        public string ComplainedStoreApplyUserName { get; set; }
        /// <summary>
        /// 電話(門市)
        /// </summary>
        public string ComplainedStoreTelephone { get; set; }
        /// <summary>
        /// 組織單位(組織)
        /// </summary>
        public string ComplainedOrganization { get; set; }
        /// <summary>
        /// 姓名(組織)
        /// </summary>
        public string ComplainedOrganizationName { get; set; }
        /// <summary>
        /// 電話(組織)
        /// </summary>
        public string ComplainedOrganizationTelephone { get; set; }
        /// <summary>
        /// 案件內容
        /// </summary>
        public string CaseContent { get; set; }
        /// <summary>
        /// 案件標籤
        /// </summary>
        public string CaseTag { get; set; }
        /// <summary>
        /// 其他資訊(商品名稱)
        /// </summary>
        public string OtherCommodityName { get; set; }
        /// <summary>
        /// 國際條碼
        /// </summary>
        public string OtherInternationalBarcode { get; set; }
        /// <summary>
        /// 批號
        /// </summary>
        public string OtherBatchNo { get; set; }
        /// <summary>
        /// 卡號
        /// </summary>
        public string OtherCardNumber { get; set; }
        /// <summary>
        /// 型號
        /// </summary>
        public string OtherProductModel { get; set; }
        /// <summary>
        /// 名稱
        /// </summary>
        public string OtherProductName { get; set; }
        /// <summary>
        /// 購買日期
        /// </summary>
        public string OtherPurchaseDay { get; set; }
        /// <summary>
        /// 結案內容
        /// </summary>
        public string FinishContent { get; set; }
        /// <summary>
        /// 結案處置
        /// </summary>
        public string ReasonName { get; set; }

        /// <summary>
        /// 結案處置(依Title分類)
        /// </summary>
        public IDictionary<string, string> ReasonList { get; set; }

        /// <summary>
        /// 結案時間
        /// </summary>
        public string FinishDateTime { get; set; }
        /// <summary>
        /// 分類ID
        /// </summary>
        public int? ClassificationID { get; set; }
    }
}
