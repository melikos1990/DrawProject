using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Newtonsoft.Json;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Notification.Email;
using SMARTII.Domain.Organization;
using SMARTII.OpenPoint.Domain;
using SMARTII.Resource.Tag;

namespace SMARTII.OpenPoint.Service
{
    public class EmailParser : IEmailParser
    {
        public CaseSource ConvertToCaseSource(OfficialEmailEffectivePayload payload, CaseWarning caseWarning, User user)
        {
            var emailInfo = payload.GetParticular<OpenPointOfficialEmailInfo>();

            var source = new CaseSource()
            {
                NodeID = payload.NodeID,
                OrganizationType = payload.OrganizationType,
                IsTwiceCall = false,
                IsPrevention = false,
                IncomingDateTime = payload.ReceivedDateTime,
                Remark = SysCommon_lang.EMAIL_ADOPT_CASE_SOURCE,
                CaseSourceType = CaseSourceType.Email,
                GroupID = payload.GroupID,

                //來源反應者
                CaseSourceUser = new CaseSourceUser()
                {
                    UserName = payload.FromName,
                    Email = payload.FromAddress,
                    Gender = SMARTII.Domain.Common.GenderType.Male
                },

                Cases = new List<SMARTII.Domain.Case.Case>()
                        {
                                new SMARTII.Domain.Case.Case()
                                {
                                    NodeID = payload.NodeID,
                                    EMLFilePath = payload.FilePath,
                                    ApplyDateTime = DateTime.Now,
                                    ApplyUserID = user.UserID,
                                    ApplyUserName = user.Name,
                                    CaseWarningID = caseWarning.ID,
                                    GroupID = payload.GroupID,
                                    Content = "\r\n" + payload.Subject + "\r\n" + payload.Body,
                                    IsReport = true,

                                    //案件反應者
                                    CaseConcatUsers = new List<CaseConcatUser>()
                                                             {
                                                                     new CaseConcatUser()
                                                                     {
                                                                         UserName = payload.FromName,
                                                                         Email = payload.FromAddress,
                                                                         Gender = SMARTII.Domain.Common.GenderType.Male,
                                                                         Mobile = string.IsNullOrEmpty(payload.JContent) ? null : emailInfo.User?.Mobile
                                                                     }
                                                             },
                                }
                        }
            };

            return source;
        }

        public (OfficialEmailEffectivePayload, OfficialEmailHistory) ConvertToOfficialEmail(OfficialEmailEffectivePayload payload)
        {
            //解析信件
            Regex reg;
            string sOrgBodyHtml = payload.Body;

            //去除原始內容標頭部分(改自ActiveUp元件ParseMimePart原始碼)
            //var iBodyStart = Regex.Match(sOrgBodyHtml, "(?<=\r?\n\r?\n).").Index;
            //if (iBodyStart != 0 && iBodyStart < sOrgBodyHtml.Length)
            //    sOrgBodyHtml = payload.Body.Substring(iBodyStart);

            //去除HtmlTag
            //將&nbsp;轉換成空白
            sOrgBodyHtml = sOrgBodyHtml.Replace("&nbsp;", " ");
            //將</p>或<br>轉換成換行
            reg = new Regex("<(/[Pp]|[Bb][Rr] */?)>");
            sOrgBodyHtml = reg.Replace(sOrgBodyHtml, "");
            //去除Tag
            reg = new Regex("<[^>]*>");
            sOrgBodyHtml = reg.Replace(sOrgBodyHtml, String.Empty);

            //去除多餘的換行 & Tab & 一行開頭的空白
            reg = new Regex("[\r\n\t]+ *");
            sOrgBodyHtml = reg.Replace(sOrgBodyHtml, "");

            //超過4000字則去除尾端
            if (sOrgBodyHtml.Length > 4000)
            {
                sOrgBodyHtml = sOrgBodyHtml.Substring(0, 4000);
            }

            var caseID = sOrgBodyHtml.SpecificString(@"\[::客服中心立案編號", @"\::]").FirstOrDefault();

            OfficialEmailEffectivePayload data = new OfficialEmailEffectivePayload
            {
                HasAttachment = payload.HasAttachment,
                NodeID = payload.NodeID,
                OrganizationType = payload.OrganizationType,
                MessageID = payload.MessageID,
                Subject = payload.Subject,
                FromAddress = payload.FromAddress,
                FromName = payload.FromName,
                Body = caseID != null ? sOrgBodyHtml.Replace($@"[::客服中心立案編號{caseID}]::]", "") : sOrgBodyHtml,
                ReceivedDateTime = payload.ReceivedDateTime,
                FilePath = payload.FilePath,
                CreateDateTime = payload.CreateDateTime,
                Mail = payload.Mail,
                CaseID = caseID == null ? null : caseID.Trim().Length == 14 ? caseID.Trim() : null,
                Email_Group_ID = payload.Email_Group_ID
            };


            // OP APP 信件格式
            //OPENPOINT 帳號：093XXXXXXX (可能沒有)
            //姓名：XXX
            //Email：XXX@gmail.com
            //聯絡電話：0939941XXX

            //手機型號：SM-A7050
            //手機作業系統：Android
            //版本：9
            //APP版本：4.0.0
            //反應內容：
            //兌換的東西跑了好幾家都換不到，快要到期了怎麼辦;

            // 如果是從OP APP 寄出,就解析內部資訊
            if (payload.FromAddress == "picapp@pic.net.tw")
            {
                //var _tempBody = sOrgBodyHtml.Replace("\r", "").Replace("\n", "");

                var emailInfo = ParsingInfo(sOrgBodyHtml);

                data = CoverData(data, emailInfo);
            }
            else if (payload.FromAddress == "opadm@mail.openpoint.com.tw")
            {
                //2020 / 06 / 23 代OP提供 正式格式

                var emailInfo = new OpenPointOfficialEmailInfo()
                {
                    AppVersion = sOrgBodyHtml.SpecificString("APP版本：", "反應內容：").FirstOrDefault(),
                    Version = sOrgBodyHtml.SpecificString("版本：", "APP版本：").FirstOrDefault()?.Replace("APP", ""),
                    System = sOrgBodyHtml.Split(new string[] { "手機作業系統：" }, StringSplitOptions.None).ElementAtOrDefault(1)?.Split(new string[] { "版本：" }, StringSplitOptions.None).ElementAtOrDefault(0),//sOrgBodyHtml.SpecificString("\r\n手機作業系統：", "\r\n版本：").FirstOrDefault(),
                    PhoneModel = sOrgBodyHtml.SpecificString("手機型號：", "手機作業系統：").FirstOrDefault(),
                    OpenPointAppAccount = sOrgBodyHtml.SpecificString("OPENPOINT 帳號：", "姓名：").FirstOrDefault(),
                    Content = sOrgBodyHtml.Split(new string[] { "反應內容：" }, StringSplitOptions.None).ElementAtOrDefault(1),

                    User = new ConcatableUser()
                    {
                        UserName = sOrgBodyHtml.SpecificString("姓名 ：", "Email：").FirstOrDefault(),
                        Email = sOrgBodyHtml.SpecificString("Email：", "聯絡電話：").FirstOrDefault(),
                        Mobile = sOrgBodyHtml.SpecificString("聯絡電話：", "手機型號：").FirstOrDefault(),
                    }
                };

                //如果無法解析信件，就將信件內容都放進Content
                if (string.IsNullOrWhiteSpace(emailInfo.User.Email) || string.IsNullOrWhiteSpace(emailInfo.User.Mobile))
                {
                    emailInfo.Content = sOrgBodyHtml;
                }

                data = CoverData(data, emailInfo);
            }

            OfficialEmailHistory history = new OfficialEmailHistory()
            {
                NodeID = payload.NodeID,
                OrganizationType = payload.OrganizationType,
                EmailGroupID = payload.Email_Group_ID,
                DownloadDateTime = payload.ReceivedDateTime,
                MessageID = payload.MessageID
            };
            return (data, history);
        }
     
        /// <summary>
        /// 覆蓋來源者及mail
        /// </summary>
        public OfficialEmailEffectivePayload CoverData(OfficialEmailEffectivePayload data, OpenPointOfficialEmailInfo emailInfo)
        {
            data.JContent = JsonConvert.SerializeObject(emailInfo);

            // 覆蓋來源者及mail
            data.FromAddress = emailInfo.User.Email;
            data.FromName = emailInfo.User.UserName;
            return data;
        }

        public OpenPointOfficialEmailInfo ParsingInfo(string sOrgBodyHtml)
        {
            return new OpenPointOfficialEmailInfo()
            {
                AppVersion = sOrgBodyHtml.SpecificString("APP版本：", "反應內容：").FirstOrDefault(),
                Version = sOrgBodyHtml.SpecificString("版本：", "APP版本：").FirstOrDefault()?.Replace("APP", ""),
                System = sOrgBodyHtml.Split(new string[] { "手機作業系統：" }, StringSplitOptions.None).ElementAtOrDefault(1)?.Split(new string[] { "版本：" }, StringSplitOptions.None).ElementAtOrDefault(0),//sOrgBodyHtml.SpecificString("\r\n手機作業系統：", "\r\n版本：").FirstOrDefault(),
                PhoneModel = sOrgBodyHtml.SpecificString("手機型號：", "手機作業系統：").FirstOrDefault(),
                OpenPointAppAccount = sOrgBodyHtml.SpecificString("OPENPOINT 帳號：", "姓名：").FirstOrDefault(),
                Content = sOrgBodyHtml.Split(new string[] { "反應內容：" }, StringSplitOptions.None).ElementAtOrDefault(1),

                User = new ConcatableUser()
                {
                    UserName = sOrgBodyHtml.SpecificString("姓名：", "OPEN POINT 帳號：").FirstOrDefault(),
                    Email = sOrgBodyHtml.SpecificString("Email：", "聯絡電話：").FirstOrDefault(),
                    Mobile = sOrgBodyHtml.SpecificString("聯絡電話：", "手機型號：").FirstOrDefault(),
                }
            };
        }
       
    }
}
