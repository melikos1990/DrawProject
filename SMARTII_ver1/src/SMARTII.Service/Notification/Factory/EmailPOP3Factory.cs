using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using MultipartDataMediaFormatter.Infrastructure;
using OpenPop.Mime;
using OpenPop.Pop3;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Common;
using SMARTII.Domain.DI;
using SMARTII.Domain.IO;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Notification.Email;

namespace SMARTII.Service.Notification.Factory
{
    public class EmailPOP3Factory : IEmailMailProtocolFactory
    {
        private readonly IIndex<string, IEmailParser> _IEmailParserFactory;
        private readonly ICommonAggregate _CommonAggregate;
        public EmailPOP3Factory(ICommonAggregate CommonAggregate,
            IIndex<string, IEmailParser> IEmailParserFactory)
        {
            _IEmailParserFactory = IEmailParserFactory;
            _CommonAggregate = CommonAggregate;
        }

        /// <summary>
        /// POP3協定登入方式
        /// </summary>
        /// <returns></returns>
        public List<(OfficialEmailEffectivePayload, OfficialEmailHistory)> LoginMailProtocol(OfficialEmailGroup data, Dictionary<string, OfficialEmailHistory> historyList, string NodeKey)
        {
            List<(OfficialEmailEffectivePayload, OfficialEmailHistory)> result = new List<(OfficialEmailEffectivePayload, OfficialEmailHistory)>();
            //取得所有實體新清單
            var mailList = FetchAllMessages(data.HostName, 110, false, data.Account, data.Password);
            IEmailParser emailParser = _IEmailParserFactory.TryGetService(NodeKey, EssentialCache.BusinessKeyValue.COMMONBU);
            var DeleteMessageList = new List<string>();
            foreach (var item in mailList)
            {
                try
                {
                    // DateTime.Now 如果在點時間內呼叫會造成時間重複的情況
                    // 這裡 讓執行續 停留 25 ms 參考網址如下
                    // https://docs.microsoft.com/zh-tw/dotnet/api/system.datetime.now?view=netframework-4.8
                    //Thread.Sleep(25);

                    var now = DateTime.Now;

                    // 信件 Header
                    var Headers = item.Headers;

                    if (historyList.Any(x => x.Key == Headers.MessageId))
                    {
                        //刪除時效過期的實體信
                        if (historyList[Headers.MessageId].DownloadDateTime < now.AddDays(-data.KeepDay))
                        {
                            DeleteMessageList.Add(Headers.MessageId);
                        }
                    }
                    else
                    {
                        //email body, 
                        string body = "";
                        MessagePart messagePart = item.MessagePart;
                        if (messagePart.IsText)
                        {
                            body = messagePart.GetBodyAsText();
                        }
                        else if (messagePart.IsMultiPart)
                        {
                            MessagePart plainTextPart2 = item.FindFirstHtmlVersion();
                            if (plainTextPart2 != null)
                            {
                                // The message had a text/plain version - show that one
                                body = plainTextPart2.GetBodyAsText();
                            }
                            else
                            {
                                // Try to find a body to show in some of the other text versions
                                List<MessagePart> textVersions = item.FindAllTextVersions();
                                if (textVersions.Count >= 1)
                                {
                                    body = textVersions[0].GetBodyAsText();

                                }
                            }
                        }
                        var officialData = new OfficialEmailEffectivePayload();
                        //MSG Reader讀出附件
                        Stream sFile = new MemoryStream(item.RawMessage);
                        var eml = MsgReader.Mime.Message.Load(sFile);
                        var attachments = eml.Attachments;

                        //檢查附件，是否有EXE或BAT
                        var isAttachmentContain = false;
                        //排除清單
                        var excludeList = new List<string>();
                        excludeList.Add(".exe");
                        excludeList.Add(".ex_");
                        excludeList.Add(".bat");
                        excludeList.Add(".ba_");

                        //讀取附件，檢查檔名
                        foreach (var att in attachments)
                        {
                            foreach (var list in excludeList)
                            {
                                //若附件包含排除清單內之檔名，則為Ture
                                if (att.FileName.Contains(list))
                                {
                                    isAttachmentContain = true;
                                }
                            }
                        }

                        //附件不含EXE或BAT，才收信
                        if (isAttachmentContain == false)
                        {
                            if (attachments.Count() != 0)
                            {
                                officialData.HasAttachment = attachments.Any(x => x.IsAttachment == true);
                            }
                            else
                            {
                                officialData.HasAttachment = false;
                            }
                            officialData.NodeID = data.NodeID;
                            officialData.OrganizationType = data.OrganizationType;
                            officialData.MessageID = Headers.MessageId;
                            officialData.Subject = Headers.Subject;
                            officialData.FromName = Headers.From.DisplayName;
                            officialData.FromAddress = Headers.From.Address;
                            officialData.Body = body;
                            var receivedDateTime = DateTime.Parse(Headers.Date);
                            officialData.ReceivedDateTime = receivedDateTime;
                            officialData.FilePath = "";
                            officialData.CreateDateTime = now;
                            string fileName = Regex.Replace(Headers.Subject, @"[\W_]+", "");
                            officialData.Mail = new HttpFile(MakeFilenameValid(fileName + ".eml"), "", item.RawMessage);
                            officialData.Email_Group_ID = data.ID;
                            //將實體轉成新增清單
                            result.Add(emailParser.ConvertToOfficialEmail(officialData));
                        }
                    }
                }
                catch (Exception ex)
                {
                    _CommonAggregate.Logger.Error($"【解析官網來信】，實體信撈取失敗，錯誤訊息: {ex.Message}。");
                }
            }
            if (DeleteMessageList != null && DeleteMessageList.Count() != 0)
            {
                using (Pop3Client client = new Pop3Client())
                {
                    // Connect to the server
                    client.Connect(data.HostName, 110, false);

                    // Authenticate ourselves towards the server
                    client.Authenticate(data.Account, data.Password);
                    //刪除實體信
                    DeleteMessageByMessageId(client, DeleteMessageList);

                }
            }

            return result;

        }
        public string MakeFilenameValid(string FN)
        {
            if (FN == null) throw new ArgumentNullException();
            if (FN.EndsWith(".")) FN = Regex.Replace(FN, @"\.+$", "");
            if (FN.Length == 0) throw new ArgumentException();
            if (FN.Length > 245) throw new PathTooLongException();
            foreach (char c in Path.GetInvalidFileNameChars()) FN = FN.Replace(c, '_');
            return FN;
        }
        /// <summary>
        /// Example showing:
        ///  - how to fetch all messages from a POP3 server
        /// </summary>
        /// <param name="hostname">Hostname of the server. For example: pop3.live.com</param>
        /// <param name="port">Host port to connect to. Normally: 110 for plain POP3, 995 for SSL POP3</param>
        /// <param name="useSsl">Whether or not to use SSL to connect to server</param>
        /// <param name="username">Username of the user on the server</param>
        /// <param name="password">Password of the user on the server</param>
        /// <returns>All Messages on the POP3 server</returns>
        public List<Message> FetchAllMessages(string hostname, int port, bool useSsl, string username, string password)
        {
            // The client disconnects from the server when being disposed
            using (Pop3Client client = new Pop3Client())
            {
                _CommonAggregate.Logger.Info($"開始連線，POP3。{client.ApopSupported}");

                // Connect to the server
                client.Connect(hostname, port, useSsl);

                _CommonAggregate.Logger.Info($"開始驗證，POP3。hostname:{hostname}，port:{port}，useSsl:{useSsl}");

                // Authenticate ourselves towards the server
                client.Authenticate(username, password, OpenPop.Pop3.AuthenticationMethod.UsernameAndPassword);

                _CommonAggregate.Logger.Info($"驗證結束，POP3。username:{username}，password:{password}");

                // Get the number of messages in the inbox
                int messageCount = client.GetMessageCount();

                // We want to download all messages
                List<Message> allMessages = new List<Message>(messageCount);

                // Messages are numbered in the interval: [1, messageCount]
                // Ergo: message numbers are 1-based.
                // Most servers give the latest message the highest number

                for (int i = messageCount; i > 0; i--)
                {
                    allMessages.Add(client.GetMessage(i));
                }
                client.Disconnect();

                // Now return the fetched messages
                return allMessages;
            }
        }
        /// <summary>
        /// Example showing:
        ///  - how to delete fetch an emails headers only
        ///  - how to delete a message from the server
        /// </summary>
        /// <param name="client">A connected and authenticated Pop3Client from which to delete a message</param>
        /// <param name="messageId">A message ID of a message on the POP3 server. Is located in <see cref="MessageHeader.MessageId"/></param>
        /// <returns><see langword="true"/> if message was deleted, <see langword="false"/> otherwise</returns>
        public void DeleteMessageByMessageId(Pop3Client client, List<string> messageId)
        {
            // Get the number of messages on the POP3 server
            int messageCount = client.GetMessageCount();

            // Run trough each of these messages and download the headers
            for (int messageItem = messageCount; messageItem > 0; messageItem--)
            {
                // If the Message ID of the current message is the same as the parameter given, delete that message
                if (messageId.Any(x => x == client.GetMessageHeaders(messageItem).MessageId))
                {
                    // Delete
                    client.DeleteMessage(messageItem);

                }
            }
        }

    }
}
