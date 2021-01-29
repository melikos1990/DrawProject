using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SMARTII.ColdStone.Domain;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Notification.Email;
using SMARTII.Domain.Organization;
using SMARTII.Resource.Tag;

namespace SMARTII.ColdStone.Service
{
    public class EmailParser : IEmailParser
    {
      
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
                Body = caseID != null ? sOrgBodyHtml.Replace($@"[::客服中心立案編號{caseID}::]", "") : sOrgBodyHtml,
                ReceivedDateTime = payload.ReceivedDateTime,
                FilePath = payload.FilePath,
                CreateDateTime = payload.CreateDateTime,
                Mail = payload.Mail,
                CaseID = caseID == null ? null : caseID.Trim().Length == 14 ? caseID.Trim() : null,
                Email_Group_ID = payload.Email_Group_ID
            };


            //屬於酷聖石官網直接轉寄信 , 才需解析轉寄信的前一個收件人
            if (payload.FromAddress == "coldstone@mail.7-11.com.tw")
            {

                //取出寄件者名稱、信箱
                string PreFromName = "";
                string PreFromAddr = "";

                var bodyList = sOrgBodyHtml.Split(new string[] { "性別 :", "姓名 :", "E-Mail :", "市話 :" }, StringSplitOptions.None);

                if (bodyList.Any(x => x.Contains("姓名 :")))
                {
                    data.FromName = bodyList.Where(x => x.Contains("姓名 :")).FirstOrDefault().Replace("姓名 :", "");
                }
                if (bodyList.Any(x => x.Contains("E-Mail :")))
                {
                    data.FromAddress = bodyList.Where(x => x.Contains("E-Mail :")).FirstOrDefault().Replace("E-Mail :", "");
                }



                //var emailInfo = new ColdStoneOfficialEmailInfo()
                //{
                //    User = new ConcatableUser()
                //    {
                //        UserName = sOrgBodyHtml.SpecificString("姓名 :", "E-Mail :").FirstOrDefault(),
                //        Email = sOrgBodyHtml.SpecificString("E-Mail :", "市話 :").FirstOrDefault(),
                //    }
                //};


                //data.JContent = Newtonsoft.Json.JsonConvert.SerializeObject(emailInfo);

                //// 覆蓋來源者及mail
                //data.FromAddress = emailInfo.User.Email;
                //data.FromName = emailInfo.User.UserName;

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

        public CaseSource ConvertToCaseSource(OfficialEmailEffectivePayload payload, CaseWarning caseWarning, User user)
        {
            // 目前與其他BU一樣 ,為保留彈性 , 因此先開立此METHOD

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

                Cases = new List<Case>()
                        {
                                new Case()
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
                                                                         Gender = SMARTII.Domain.Common.GenderType.Male
                                                                     }
                                                             },
                                }
                        }
            };

            return source;
        }
      
    }
}
