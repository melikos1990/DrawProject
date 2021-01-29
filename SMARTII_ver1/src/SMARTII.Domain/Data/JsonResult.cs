using System;

namespace SMARTII.Domain.Data
{
    public class JsonResult : IHttpResult
    {
        public JsonResult()
        {
        }

        public JsonResult(Boolean isSuccess)
        {
            this.isSuccess = isSuccess;
        }

        public JsonResult(string message,
                              Boolean isSuccess)
        {
            this.message = message;
            this.isSuccess = isSuccess;
        }

        /// <summary>
        /// 傳輸訊息
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool isSuccess { get; set; }

        /// <summary>
        /// 額外資訊
        /// </summary>
        public object extend { get; set; }
    }

    public class JsonResult<TElement> : JsonResult
    {
        public JsonResult()
        {
        }

        public JsonResult(TElement element)
        {
            this.element = element;
        }

        public JsonResult(TElement element, Boolean isSuccess) : base(isSuccess)
        {
            this.element = element;
        }

        public JsonResult(TElement element, String message, Boolean isSuccess) : base(message, isSuccess)
        {
            this.element = element;
        }

        /// <summary>
        /// 傳輸資料
        /// </summary>
        public TElement element { get; set; }
    }
}