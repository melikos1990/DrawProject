using System;

namespace SMARTII.Domain.Data
{
    public enum VoiceType
    {
        /// <summary>
        /// 配對目前音檔
        /// </summary>
        now,

        /// <summary>
        /// 配對上一個音檔
        /// </summary>
        prev
    }

    public class VoiceRequest
    {
        public VoiceRequest()
        {
        }

        /// <summary>
        /// 查詢區間(起)
        /// </summary>
        public DateTime? btime { get; set; }

        /// <summary>
        /// 查詢區間(迄)
        /// </summary>
        public DateTime? etime { get; set; }

        /// <summary>
        /// 值機員代號
        /// </summary>
        public string eid { get; set; }

        /// <summary>
        /// 音檔Key值
        /// </summary>
        public string sid { get; set; }
    }
}