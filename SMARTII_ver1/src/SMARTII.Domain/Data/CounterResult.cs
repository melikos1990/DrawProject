using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMARTII.Domain.Data
{
    public class CounterResult
    {
        public CounterResult()
        {
        }

        /// <summary>
        /// 成功數
        /// </summary>
        public int SuccessCount { get; set; }

        /// <summary>
        /// 失敗數
        /// </summary>
        public int FailCount { get; set; }

        /// <summary>
        /// 額外資訊
        /// </summary>
        public Dictionary<string,int> Extend { get; set; }
    }
}
