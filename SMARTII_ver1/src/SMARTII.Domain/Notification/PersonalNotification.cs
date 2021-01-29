using System;
using System.Dynamic;
using Newtonsoft.Json;
using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Notification
{
    public class PersonalNotification
    {
        /// <summary>
        /// 流水號
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 使用者代號
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 內容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 建立人員
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 通知型態(個人)
        /// </summary>
        public PersonalNotificationType PersonalNotificationType { get; set; }

        /// <summary>
        /// 人員
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// 額外訊息
        /// </summary>
        public string Extend { get; set; }

        /// <summary>
        /// 解析額外資訊
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetExtend<T>() {
            return string.IsNullOrEmpty(this.Extend) ? 
                default(T) :
                JsonConvert.DeserializeObject<T>(this.Extend);
        }
    }
}
