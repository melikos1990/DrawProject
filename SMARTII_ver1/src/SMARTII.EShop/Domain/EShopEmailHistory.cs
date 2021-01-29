using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMARTII.EShop
{
    /// <summary>
    /// 來信紀錄
    /// </summary>
    public class EShopEmailHistory
    {
        /// <summary>
        /// 日期
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// 時間
        /// </summary>
        public string Time { get; set; }
        /// <summary>
        /// 分類
        /// </summary>
        public string Classification { get; set; }
        /// <summary>
        /// 類別
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// 項目
        /// </summary>
        public string Project { get; set; }
        /// <summary>
        /// 反應品牌
        /// </summary>
        public string Brand { get; set; }
        /// <summary>
        /// 商品名稱
        /// </summary>
        public string Product { get; set; }
        /// <summary>
        /// 國際條碼
        /// </summary>
        public string EANBarcode { get; set; }
        /// <summary>
        /// 批號
        /// </summary>
        public string BatchNo { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 聯絡電話
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 問題
        /// </summary>
        public string Problem { get; set; }
        /// <summary>
        /// 回覆
        /// </summary>
        public string Reply { get; set; }
        /// <summary>
        /// 處理人員
        /// </summary>
        public string ProcessPerson { get; set; }
        /// <summary>
        /// 完成回覆日期
        /// </summary>
        public string FinshDate { get; set; }
        /// <summary>
        /// 完成回覆時間
        /// </summary>
        public string FinshTime { get; set; }
        /// <summary>
        /// 轉派單位人員
        /// </summary>
        public string AssignedPerson { get; set; }
        /// <summary>
        /// 回覆處理內容
        /// </summary>
        public string ReplyContent { get; set; }
    }
}
