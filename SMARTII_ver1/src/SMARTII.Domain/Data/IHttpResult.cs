using System;

namespace SMARTII.Domain.Data
{
    public interface IHttpResult
    {
        /// <summary>
        /// 回饋訊息
        /// </summary>
        string message { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        Boolean isSuccess { get; set; }

        /// <summary>
        /// 額外資訊
        /// </summary>
        object extend { get; set; }
    }
}