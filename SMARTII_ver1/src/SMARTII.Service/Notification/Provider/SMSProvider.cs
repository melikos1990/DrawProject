using System;
using System.Net.Http;
using System.Text;
using System.Xml;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Notification;

namespace SMARTII.Service.Notification.Provider
{
    public class SMSProvider : INotificationProvider
    {
        public void Send(ISenderPayload payload, Action<object> beforeSend = null, Action<object> afterSend = null)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.Timeout = TimeSpan.FromSeconds(30);

                    var url = ThirdPartyCache.Instance.SMSApiUrl;

                    var httpContent = new StringContent(XmlFile(), Encoding.UTF8, "application/xml"); ;

                    var response = client.PostAsync(url, httpContent).GetAwaiter().GetResult();

                    var result = response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", ex.Message);
                }
            }
        }

        /// <summary>
        /// 定義XML格式
        /// </summary>
        /// <returns></returns>
        public string XmlFile()
        {
            XmlDocument doc = new XmlDocument();

            //節點1
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);

            //節點2
            XmlElement element = doc.CreateElement(string.Empty, "SmsSendViewModel", string.Empty);
            doc.AppendChild(element);

            XmlElement element_c1 = doc.CreateElement(string.Empty, "UserName", string.Empty);
            XmlText text = doc.CreateTextNode("smartii");
            element_c1.AppendChild(text);
            element.AppendChild(element_c1);

            XmlElement element_c2 = doc.CreateElement(string.Empty, "PasswordHash", string.Empty);
            XmlText text2 = doc.CreateTextNode("Ptc12876266");
            element_c2.AppendChild(text2);
            element.AppendChild(element_c2);

            XmlElement element_c3 = doc.CreateElement(string.Empty, "body", string.Empty);
            XmlText text3 = doc.CreateTextNode("[SMARTII]-測試簡訊，請忽略");
            element_c3.AppendChild(text3);
            element.AppendChild(element_c3);

            XmlElement element_c4 = doc.CreateElement(string.Empty, "PhoneNumber", string.Empty);
            XmlText text4 = doc.CreateTextNode("0919921182");
            element_c4.AppendChild(text4);
            element.AppendChild(element_c4);

            return doc.OuterXml;
        }
    }
}