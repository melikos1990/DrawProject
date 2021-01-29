using System;

namespace SMARTII.Domain.Data
{
    public class PagingResponse
    {
        public PagingResponse()
        {
        }

        public PagingResponse(Boolean isSuccess)
        {
            this.isSuccess = isSuccess;
        }

        public PagingResponse(Boolean isSuccess, string message) : this(isSuccess)
        {
            this.message = message;
        }

        public Boolean isSuccess { get; set; }

        public int totalCount { get; set; }

        public object extension { get; set; }

        public string message { get; set; }
    }

    public class PagingResponse<T> : PagingResponse
    {
        public PagingResponse(T data)
        {
            this.data = data;
        }

        public T data { get; set; }
    }
}