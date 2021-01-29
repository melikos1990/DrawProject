using System;

namespace SMARTII.Domain.System
{
    public class SystemParameter
    {
        public SystemParameter()
        {
        }

        /// <summary>
        /// 代號
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 關鍵值
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 實際資料
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 代表文字
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 更新者
        /// </summary>
        public string UpdateUserName { get; set; }

        /// <summary>
        /// 建立者
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 建立者
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime? UpdateDateTime { get; set; }

        /// <summary>
        /// 生效日期
        /// </summary>
        public DateTime? ActiveDateTime { get; set; }

        /// <summary>
        /// 下一次的值
        /// </summary>
        public string NextValue { get; set; }

        /// <summary>
        /// 是否可刪除
        /// </summary>
        public bool IsUnDeletable { get; set; }
    }
}