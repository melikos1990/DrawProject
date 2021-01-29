using System.Dynamic;
using AutoMapper;
using Newtonsoft.Json;

namespace SMARTII.Domain.Data
{
    public class DynamicSerializeObject
    {
        public DynamicSerializeObject()
        {
        }

        /// <summary>
        /// 動態內容
        /// </summary>
        public string JContent { get; set; }

        /// <summary>
        /// 動態物件
        /// </summary>
        [JsonIgnore]
        [IgnoreMap]
        public ExpandoObject Particular { get; set; }

        public T GetParticular<T>() where T : class,new()
        {
            if (string.IsNullOrEmpty(this.JContent))
                return new T();

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };


            return JsonConvert.DeserializeObject<T>(JContent , settings);
        }

        public T GetParticularUseExist<T>(string content) where T : class, new()
        {

            if (string.IsNullOrEmpty(content))
                return new T();

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };


            return JsonConvert.DeserializeObject<T>(content, settings);
        }

        public string GetJContent()
        {
            if (this.Particular == null)
                return string.Empty;

            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            return JsonConvert.SerializeObject(this.Particular);
        }

        public string GetJContentUseExist(object data)
        {
            if (data == null)
                return string.Empty;

            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            return JsonConvert.SerializeObject(data);
        }
    }
}
