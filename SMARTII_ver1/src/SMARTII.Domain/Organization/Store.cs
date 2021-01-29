using System;
using System.Collections.Generic;
using AutoMapper;
using Newtonsoft.Json;
using SMARTII.Domain.Master;

namespace SMARTII.Domain.Organization
{
    public class Store<T> : Store
    {
        private T _particular;
        private string _jContent;

        public Store()
        {
        }

        public string JContent
        {
            get
            {
                return _jContent;
            }

            set
            {
                _jContent = value;

                if (string.IsNullOrEmpty(_jContent) == false)
                {
                    var jsonSerializerSettings = new JsonSerializerSettings();
                    jsonSerializerSettings.NullValueHandling = NullValueHandling.Include;
                    jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

                    _particular = JsonConvert.DeserializeObject<T>(_jContent, jsonSerializerSettings);
                }
            }
        }

        [JsonIgnore]
        [IgnoreMap]
        public T Particular
        {
            get
            {
                return _particular;
            }
            set
            {
                _particular = value;

                if (_particular != null)
                {
                    _jContent = JsonConvert.SerializeObject(_particular);
                }
            }
        }
    }

    public class Store
    {
        public Store()
        {
        }
        /// <summary>
        /// 門市節點
        /// </summary>
        public int NodeID { get; set; }

        /// <summary>
        /// 組織型態
        /// </summary>
        public OrganizationType OrganizationType { get; set; }

        /// <summary>
        /// 門市代號
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 市話
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// 門市名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 與總部組織樹關聯
        /// </summary>
        public HeaderQuarterNode HeaderQuarterNode { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 建立人員
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime? UpdateDateTime { get; set; }

        /// <summary>
        /// 更新人員
        /// </summary>
        public string UpdateUserName { get; set; }

        /// <summary>
        /// 開店時間
        /// </summary>
        public DateTime? StoreOpenDateTime { get; set; }

        /// <summary>
        /// 關店時間
        /// </summary>
        public DateTime? StoreCloseDateTime { get; set; }

        /// <summary>
        /// 門市地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 門市信箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 門市型態(EX: 百貨店/快閃店) 由系統參數定義
        /// </summary>
        public int StoreType { get; set; }
        
        /// <summary>
        /// 備註
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        public Boolean IsEnabled { get; set; }

        /// <summary>
        /// 服務時間
        /// </summary>
        public string ServiceTime { get; set; }

        /// <summary>
        /// 負責人
        /// </summary>
        public int? OwnerNodeJobID { get; set; }

        /// <summary>
        /// 區經理
        /// </summary>
        public int? SupervisorNodeJobID { get; set; }

        /// <summary>
        /// 組織節點代號(企業別)
        /// </summary>
        public int BuID { get; set; }
        /// <summary>
        /// 組織名稱
        /// </summary>
        public string BuName { get; set; }

        /// <summary>
        /// 組織節點識別值
        /// </summary>
        public string BuKey { get; set; }


        /// <summary>
        /// OFC 職稱人員
        /// ※ 透過二次查詢求得
        /// </summary>
        public JobPosition OfcJobPosition { get; set; }

        /// <summary>
        /// 負責職稱人員
        /// ※ 透過二次查詢求得
        /// </summary>
        public JobPosition OwnerJobPosition { get; set; }

    

    }
}
