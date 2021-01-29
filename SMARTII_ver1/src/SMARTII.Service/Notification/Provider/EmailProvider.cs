using System;
using System.IO;
using System.Net.Mail;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Data;
using SMARTII.Domain.IO;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Notification.Email;
using System.Net.Mime;

namespace SMARTII.Service.Notification.Provider
{
    public class EmailProvider : INotificationProvider
    {
        public void Send(ISenderPayload payload,
                         Action<object> beforeSend = null,
                         Action<object> afterSend = null)
        {
            var data = ((EmailPayload)payload);


            using (var message = new MailMessage())
            {
                message.From = new MailAddress(data.Sender.Email, data.Sender.UserName);
                message.Subject = data.Title.Replace("\r", "").Replace("\n", "");
                message.IsBodyHtml = true;
                message.Body = data.IsHtmlBody ? data.Content : data.Content?.ToHtmlFormat();

                data.Receiver?.ForEach(receiveUser =>
                {
                    message.To.Add(receiveUser.Email);
                });
                data.Cc?.ForEach(ccUser =>
                {
                    message.CC.Add(ccUser.Email);
                });
                data.Bcc?.ForEach(bccUser =>
                {
                    message.Bcc.Add(bccUser.Email);
                });

                data.FilePaths?.ForEach(file =>
                {
                    Attachment attachment;
                    attachment = new Attachment(file, MediaTypeNames.Application.Octet);
                    message.Attachments.Add(attachment);
                });

                data.Attachments?.ForEach(file =>
                {
                    Stream stream = new MemoryStream(file.Buffer);
                    var attachment = new Attachment(stream, file.FileName, file.MediaType);
                    message.Attachments.Add(attachment);
                });

                using (var client = new SmtpClient(ThirdPartyCache.Instance.SMTPServerIP, ThirdPartyCache.Instance.SMTPServerPort))
                {
                    beforeSend?.Invoke(data);

                    SendEml(client, message);

                    var bytes = SaveEml(client, message);

                    afterSend?.Invoke(bytes);
                }
            }
        }

        /// <summary>
        /// EML 存檔
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public byte[] SaveEml(SmtpClient client, MailMessage message)
        {
            var dirPath = FilePathFormatted.EmailTempDirPath(Guid.NewGuid().ToString());
            FileUtility.CreateDirectory(dirPath);
            client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
            client.PickupDirectoryLocation = dirPath;
            //公司SMTP不支援SSL連線
            //client.EnableSsl = true;
            client.Send(message);
            var bytes = FileUtility.GetFileBytes(dirPath, null)[0];
            FileUtility.DeleteDirectory(dirPath);

            return bytes;

        }

        /// <summary>
        /// 寄送Mail
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message"></param>
        public void SendEml(SmtpClient client, MailMessage message)
        {
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Send(message);

        }

    }
}
