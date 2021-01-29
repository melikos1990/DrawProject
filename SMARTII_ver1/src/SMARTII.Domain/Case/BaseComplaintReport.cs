using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMARTII.Domain.Case
{
    public class BaseComplaintReport
    {
        /// <summary>
        /// 密碼
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 顧客姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 顧客性別
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// 手機
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 電話
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// E-MAIL
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 問題描述
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 檔案編號
        /// </summary>
        public string InvoicID { get; set; }
        /// <summary>
        /// 開單人
        /// </summary>
        public string CreateUser { get; set; }
        /// <summary>
        /// 開單日
        /// </summary>
        public Nullable<DateTime> CreateDate { get; set; }
        /// <summary>
        /// 是否需主管回電
        /// </summary>
        public bool? IsRecall { get; set; }
        /// <summary>
        /// Excel檔案名稱
        /// </summary>
        public string fileName { get; set; }
        /// <summary>
        /// 是否認列
        /// </summary>
        public bool? IsRecognize { get; set; }
    }
}
