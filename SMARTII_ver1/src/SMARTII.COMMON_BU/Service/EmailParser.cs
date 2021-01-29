using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Notification.Email;
using SMARTII.Domain.Organization;
using SMARTII.Resource.Tag;

namespace SMARTII.COMMON_BU.Service
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

            //去除特殊字元
            reg = new Regex(@"(<!--\[.*\]-->)", RegexOptions.Singleline);
            sOrgBodyHtml = reg.Replace(sOrgBodyHtml, "");

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

            //var caseID = sOrgBodyHtml.SpecificString(@"\[::客服中心立案編號", @"\::]").FirstOrDefault();
            var caseID = sOrgBodyHtml.SpecificString(@"\[::客服中心立案編號", @"\::]").LastOrDefault();//20201013若有多個取最後

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
                    Gender = Domain.Common.GenderType.Male
                },

                Cases = new List<Domain.Case.Case>()
                        {
                                new Domain.Case.Case()
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
                                                                         Gender = Domain.Common.GenderType.Male
                                                                     }
                                                             },
                                }
                        }
            };

            return source;
        }
    }
}
