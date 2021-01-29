using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Data;
using SMARTII.Domain.IO;

namespace SMARTII.Service.IO
{
    public class VoiceFacade : IVoiceFacade
    {
        private static readonly HttpClient _HttpClient;

        static VoiceFacade()
        {
            _HttpClient = new HttpClient();
        }

        /// <summary>
        /// 音檔查詢清單
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<VoiceSearchResponse> GetList(VoiceRequest data)
        {
            var parameter = data.ToNameValueCollection().ToQueryString();

            HttpResponseMessage response = await _HttpClient.GetAsync(
                $"{ThirdPartyCache.Instance.VoiceSearchUrl}{parameter}");

            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStreamAsync();

            var xmlSerializer = new XmlSerializer(typeof(VoiceSearchResponse));

            var source = (VoiceSearchResponse)xmlSerializer.Deserialize(responseBody);

            return source;
        }

        /// <summary>
        /// 音檔配對
        /// </summary>
        /// <param name="eid"></param>
        /// <returns></returns>
        public async Task<VoiceMatchResponse> Match(string eid)
        {
            var tid = VoiceType.now.ToString();

            var mid = Guid.NewGuid().ToString().Replace("-", string.Empty);

            var matchUrl = $"{ThirdPartyCache.Instance.VoiceMatchUrl}?tid={tid}&eid={eid}&mid={mid}";

            HttpResponseMessage response = await _HttpClient.GetAsync(matchUrl);

            var result = await response.Content.ReadAsStringAsync();
            var responseBody = await response.Content.ReadAsStreamAsync();

            var xmlSerializer = new XmlSerializer(typeof(VoiceMatchResponse));

            var source = (VoiceMatchResponse)xmlSerializer.Deserialize(responseBody);

            return source;
        }
    }
}