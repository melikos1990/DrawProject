using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.CodeAnalysis.Scripting;
using Newtonsoft.Json;
using Ptc.Data.Condition2.Interface.Common;
using Ptc.Data.Condition2.Mssql.Common;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;

namespace StressTest
{
    public class BaseTest
    {
        // 10.2.123.197
        public string Host = @"http://10.2.123.197/";
        public string AP = "SmartII/api/";

        //public string Host = @"https://10.2.6.60/";
        public int Port = -1;
        //public string AP = "SmartII_AP/api/";
        public string URL = "";
        public string Token = "";

        public async Task<TResult> GetAsync<TValue, TResult>(TValue Value, List<JobPosition> jobPositions, string token = "")
        {
            try
            {
                // 設定 HTTPS 連線時，不要理會憑證的有效性問題
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);

                using (HttpClient client = new HttpClient())
                {

                    if (String.IsNullOrEmpty(token))
                    {
                        token = this.Token;
                    }

                    //client.DefaultRequestHeaders.Add("Content-Type", "application/json");
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    client.DefaultRequestHeaders.Add("JobPosition", JsonConvert.SerializeObject(jobPositions));

                    var response = await client.GetAsync(ToURL(Value));

                    response.EnsureSuccessStatusCode();

                    string result = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<TResult>(result);

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public async Task<TResult> PostAsync<TValue, TResult>(TValue Value, List<JobPosition> jobPositions, string token = "")
        {
            try
            {
                // 設定 HTTPS 連線時，不要理會憑證的有效性問題
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);

                using (HttpClient client = new HttpClient())
                {

                    client.Timeout = TimeSpan.FromSeconds(100000);

                    if (String.IsNullOrEmpty(token))
                    {
                        token = this.Token;
                    }

                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                    client.DefaultRequestHeaders.Add("JobPosition", JsonConvert.SerializeObject(jobPositions));



                    var content = new StringContent(JsonConvert.SerializeObject(Value), Encoding.UTF8, "application/json");


                    var response = await client.PostAsync(ToBuilder().ToString(), content);


                    response.EnsureSuccessStatusCode();

                    string result = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<TResult>(result);

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        private UriBuilder ToBuilder()
        {
            if (string.IsNullOrEmpty(Host)) throw new ArgumentException("api path not be null");

            var builder = new UriBuilder(Host);

            builder.Port = Port;
            builder.Path = AP + URL;

            return builder;
        }
        private string ToQuery(object Value)
        {

            var pairs = ParseObjectToKeyValue(Value);

            string query = String.Join("&", pairs.Keys.Select(v => v + "=" + HttpUtility.UrlEncode(pairs[v])));

            return query;
        }
        private string ToURL(object Value)
        {

            var builder = ToBuilder();

            builder.Query = ToQuery(Value);

            return builder.ToString();

        }

        public Dictionary<string, string> ParseObjectToKeyValue(object Object)
        {

            var result = new Dictionary<string, string>();

            var props = Object?.GetType()?.GetProperties()?.ToList();

            props?.ForEach(prop =>
            {
                var name = prop.Name;
                var value = Object.GetType().GetProperty(prop.Name).GetValue(Object, null);
                if (value == null) return;

                if (value is string)
                {
                    result.Add(name, value.ToString());
                }
                else
                {
                    result.Add(name, JsonConvert.SerializeObject(value));
                }
            });

            return result;
        }


        // 設定 HTTPS 連線時，不要理會憑證的有效性問題
        public static bool ValidateServerCertificate(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }


        #region Condition2

        public class Condition2Config
        {
            static Condition2Config()
            {
                MSSQLDataSetting.DefaultScriptOptions = () =>
                {
                    var dbAssembly = typeof(SMARTIIEntities).Assembly;
                    var excutingAssembly = System.Reflection.Assembly.GetExecutingAssembly();

                    return ScriptOptions.Default
                                        .AddReferences(dbAssembly)
                                        .AddReferences(excutingAssembly)
                                        .WithImports(new string[] {
                                        "System",
                                        "System.Collections",
                                        "System.Collections.Generic",
                                        "System.Linq"
                                        });
                };
            }

            public static ISetup[] Setups = new ISetup[]
            {
            // 設定MSSQL CONDITION 的相關配置
            // 相關作法請參閱 :
            // http://tfs2:8080/tfs/SD4-Collection/LibrarySD4/_git/Condition2?path=%2FREADME_MSSQL.md&version=GBmaster&_a=preview
              new MSSQLDataSetting()
              {
                  // 回傳預設連線位址實體
                  DefaultDBContextDelegate = () => new SMARTIIEntities(DataAccessCache.Instance.SmartIIConn),

                  // 回傳物件映射的設定檔
                  DefaultMappingConfig = () => AutoMapper.Mapper.Instance,
              }
            };
        }

        #endregion

    }
}
