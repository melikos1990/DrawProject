using System;

namespace SMARTII.Domain.System
{
    public class BatchRecord
    {
        /// <summary>
        /// 排程名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 最近執行時間
        /// </summary>
        public DateTime RecentExecuteDateTime { get; set; }

        /// <summary>
        /// 最近完成時間
        /// </summary>
        public DateTime RecentFinishDateTime { get; set; }

        /// <summary>
        /// 累計執行次數
        /// </summary>
        public int ExecuteCount { get; set; }
    }
}