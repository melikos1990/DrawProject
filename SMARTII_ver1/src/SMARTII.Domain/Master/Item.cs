using System;
using System.Collections.Generic;
using System.ComponentModel;
using AutoMapper;
using MultipartDataMediaFormatter.Infrastructure;
using Newtonsoft.Json;
using SMARTII.Domain.Case;
using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Master
{
    public class Item<T> : Item
    {
        private T _particular;
        private string _jContent;

        public Item()
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
                    //jsonSerializerSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
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

    public class Item : IOrganizationRelationship
    {
        /// <summary>
        /// 代號
        /// </summary>
        [Description("商品代號")]
        public int ID { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        [Description("商品名稱")]
        public string Name { get; set; }

        /// <summary>
        /// 商品代號
        /// </summary>
        [Description("商品代號")]
        public string Code { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Description("商品描述")]
        public string Description { get; set; }

        /// <summary>
        /// 商品圖片路徑
        /// </summary>
        public string[] ImagePath { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public List<HttpFile> Picture { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        public Boolean IsEnabled { get; set; }

        /// <summary>
        /// 建立人員
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        public string UpdateUserName { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime? UpdateDateTime { get; set; }

        #region impl

        /// <summary>
        /// 組織節點代號(企業別)
        /// </summary>
        public int NodeID { get; set; }

        /// <summary>
        /// 組織型態
        /// </summary>
        public OrganizationType OrganizationType { get; set; }

        /// <summary>
        /// 組織名稱
        /// </summary>
        public string NodeName { get; set; }

        /// <summary>
        /// 組織
        /// </summary>
        public IOrganizationNode Node { get; set; }

        #endregion impl

        /// <summary>
        /// 與案件關聯
        /// </summary>
        public List<CaseItem> CaseItem { get; set; }
    }
}
