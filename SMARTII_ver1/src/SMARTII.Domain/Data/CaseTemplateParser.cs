using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoreLinq;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;
using SMARTII.Domain.IO;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;

namespace SMARTII.Domain.Master.Parser
{
    public class CaseTemplateParser
    {
        /// <summary>
        /// 解析問題分類
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        public string QuestionClassifiction(string qc)
        {
            if (string.IsNullOrEmpty(qc)) return string.Empty;

            var qcs = qc.Split('@');

            return string.Join("-", qcs);

        }
        /// <summary>
        /// 解析問題分類(至第一層)
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        public string QuestionClassifictionLevel1(string qc)
        {
            if (string.IsNullOrEmpty(qc)) return string.Empty;

            return ForQuestionClassifictionLevel(qc, 1);
        }
        /// <summary>
        /// 解析問題分類(至第二層)
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        public string QuestionClassifictionLevel2(string qc)
        {
            if (string.IsNullOrEmpty(qc)) return string.Empty;

            return ForQuestionClassifictionLevel(qc, 2);
        }
        /// <summary>
        /// 解析問題分類(至第三層)
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        public string QuestionClassifictionLevel3(string qc)
        {
            if (string.IsNullOrEmpty(qc)) return string.Empty;

            return ForQuestionClassifictionLevel(qc, 3);

        }
        /// <summary>
        /// 解析問題分類(至第四層)
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        public string QuestionClassifictionLevel4(string qc)
        {
            if (string.IsNullOrEmpty(qc)) return string.Empty;

            return ForQuestionClassifictionLevel(qc, 4);
        }
        /// <summary>
        /// 解析轉派內容明細
        /// </summary>
        /// <param name="caseAssignments"></param>
        /// <returns></returns>
        public string CaseAssignmenContentParing(List<CaseAssignment> caseAssignments)
        {
            var result = new List<string>();

            var datas = caseAssignments?.OrderBy(x => x.AssignmentID);

            if (datas == null || datas.Count() <= 0) return string.Empty;

            result = datas?.Select(data => string.Format(CaseTemplateFormatter.CaseAssignmenFormat,
                                        data.AssignmentID,
                                        data.FinishContent,
                                        data.FinishNodeName,
                                        data.FinishDateTime.DisplayWhenNull()
                    )).ToList();

            return string.Join("\r\n", result);// sb.ToString();
        }


        /// <summary>
        /// 解析案件連絡者明細
        /// </summary>
        /// <param name="caseAssignments"></param>
        /// <returns></returns>
        public string CaseConcatUsersParing(List<CaseConcatUser> caseConcatUsers)
        {

            if (caseConcatUsers == null || caseConcatUsers.Count() <= 0) return string.Empty;

            var userConcat = caseConcatUsers.Select(user =>
                     (user.UnitType == Organization.UnitType.Customer) ?
                        $"{user.UserName} {user.Gender?.GetDescription()}" :
                        user.NodeName
            );

            return string.Join("/", userConcat);
        }

        /// <summary>
        /// 解析案件連絡者明細(不須性別)
        /// </summary>
        /// <param name="caseAssignments"></param>
        /// <returns></returns>
        public string CaseConcatUsersNameParing(List<CaseConcatUser> caseConcatUsers)
        {

            if (caseConcatUsers == null || caseConcatUsers.Count() <= 0) return string.Empty;

            var userConcat = caseConcatUsers.Select(user =>
                     (user.UnitType == Organization.UnitType.Customer) ?
                        $"{user.UserName}" :
                        user.NodeName
            );

            return string.Join("/", userConcat);
        }

        /// <summary>
        /// 解析案件反應者(消費者)電話
        /// </summary>
        /// <param name="caseAssignments"></param>
        /// <returns></returns>
        public string CaseConcatUsersPhoneParing(List<CaseConcatUser> caseConcatUsers)
        {
            if (caseConcatUsers == null || caseConcatUsers.Count() <= 0) return string.Empty;
            string resultStr = "";

            if (!string.IsNullOrEmpty(caseConcatUsers.First().Mobile))
                resultStr += caseConcatUsers.First().Mobile;

            if (!string.IsNullOrEmpty(caseConcatUsers.First().Telephone))
            {
                if(resultStr.Length > 0)
                    resultStr += "\r\n";
                resultStr += caseConcatUsers.First().Telephone;
            }
               
            if (!string.IsNullOrEmpty(caseConcatUsers.First().TelephoneBak))
            {
                if (resultStr.Length > 0)
                    resultStr += "\r\n";
                resultStr += caseConcatUsers.First().TelephoneBak;
            }

            return resultStr;
        }

        /// <summary>
        /// 解析案件被反應者明細
        /// </summary>
        /// <param name="caseAssignments"></param>
        /// <returns></returns>
        public string CaseComplainedUsersParing(List<CaseComplainedUser> CaseComplainedUsers)
        {

            if (CaseComplainedUsers == null || CaseComplainedUsers.Count() <= 0) return string.Empty;

            var userConcat = CaseComplainedUsers.Select(user => user.NodeName);

            return string.Join("/", userConcat);
        }

        /// <summary>
        /// 解析案件被反應者資訊
        /// </summary>
        /// <param name="caseAssignments"></param>
        /// <returns></returns>
        public string CaseComplainedUsersInfoParing(List<CaseComplainedUser> CaseComplainedUsers)
        {
            if (CaseComplainedUsers == null || CaseComplainedUsers.Count() <= 0 || !CaseComplainedUsers.Any(x => x.CaseComplainedUserType == CaseComplainedUserType.Responsibility)) return string.Empty;
            string resultStr = "";
            var userConcat = CaseComplainedUsers.First(x => x.CaseComplainedUserType == CaseComplainedUserType.Responsibility);

            if (!string.IsNullOrEmpty(userConcat.NodeName) && !string.IsNullOrEmpty(userConcat.Address))
                resultStr = userConcat.NodeName + "\r\n" + userConcat.Address;

            return resultStr;
        }
        /// <summary>
        /// 解析反應單-被反應者名稱 
        /// </summary>
        /// <param name="CaseComplainedUsers"></param>
        /// <returns></returns>
        public string InvoiceTitleNodeNameParing(List<CaseComplainedUser> CaseComplainedUsers)
        {
            string NodeName = "";
            var complaint = CaseComplainedUsers.Where(x => x.CaseComplainedUserType == CaseComplainedUserType.Responsibility);

            if (complaint == null || complaint.Count() == 0)
                return NodeName;

            if (complaint.First().UnitType == UnitType.Store)
            {
                NodeName = "-" + complaint.First().ParentPathName + "-" + complaint.First().NodeName + "-";
            }
            else if (complaint.First().UnitType == UnitType.Organization)
            {
                NodeName = "-";
            }
            return NodeName;
        }
        /// <summary>
        /// 解析反應單-FOR 21世紀產生主旨 被反應者名稱 
        /// </summary>
        /// <param name="CaseComplainedUsers"></param>
        /// <returns></returns>
        public string InvoiceTitle21NodeNameParing(List<CaseComplainedUser> CaseComplainedUsers)
        {
            string NodeName = "";
            var complaint = CaseComplainedUsers.Where(x => x.CaseComplainedUserType == CaseComplainedUserType.Responsibility);

            if (complaint == null || complaint.Count() == 0)
                return NodeName;

            return "-" + complaint.First().NodeName + "-";
        }
        /// <summary>
        /// 解析案件信件主旨
        /// </summary>
        /// <param name="caseAssignments"></param>
        /// <returns></returns>
        public string CaseEmailTitleParing(string emilPath)
        {
            if (string.IsNullOrEmpty(emilPath)) return string.Empty;

            var arge = ParsinRequestParam(emilPath);

            if (arge.Count <= 0) return string.Empty;

            string path = string.Format(FilePathFormatted.OfficialEmailPhysicalFilePath, arge[0], arge[1], arge[2]);

            var fileInfo = new FileInfo(path);
            var eml = MsgReader.Mime.Message.Load(fileInfo);

            var subject = $"RE:{eml.Headers.Subject}";

            return subject;
        }

        /// <summary>
        /// 解析反應單-單號
        /// </summary>
        /// <param name="caseAssignments"></param>
        /// <returns></returns>
        public string CaseAssignmenInvoiceIDParing(List<CaseAssignmentComplaintInvoice> caseAssignmentInvoics)
        {

            if (caseAssignmentInvoics == null || caseAssignmentInvoics.Count() <= 0) return string.Empty;

            var Invoice = caseAssignmentInvoics.FirstOrDefault();

            return Invoice?.InvoiceID ?? string.Empty;
        }

        /// <summary>
        /// 解析通知對象(反應單)
        /// </summary>
        /// <param name="caseAssignments"></param>
        /// <returns></returns>
        public string CaseAssignmenInvoiceParing(List<CaseAssignmentComplaintInvoice> caseAssignmentInvoics)
        {


            if (caseAssignmentInvoics == null || caseAssignmentInvoics.Count() <= 0) return string.Empty;

            var sb = new StringBuilder();

            var noticeUsers = caseAssignmentInvoics.FirstOrDefault()?.NoticeUsers;

            if (noticeUsers != null)
            {
                sb.Append(string.Join("/", noticeUsers));

            }


            return noticeUsers != null ? sb.ToString() : string.Empty;
        }

        /// <summary>
        /// 解析通知對象(派工)
        /// </summary>
        /// <param name="caseAssignments"></param>
        /// <returns></returns>
        public string CaseAssignmenNoticeUserParing(List<CaseAssignment> caseAssignments)
        {

            if (caseAssignments == null || caseAssignments.Count() <= 0) return string.Empty;

            var sb = new StringBuilder();

            var noticeUsers = caseAssignments.SelectMany(x => x.NoticeUsers).ToList();

            if (noticeUsers != null)
            {
                sb.Append(string.Join("/", noticeUsers));
            }



            return noticeUsers != null ? sb.ToString() : string.Empty;
        }

        /// <summary>
        /// 解析通知對象(一般通知)
        /// </summary>
        /// <param name="caseAssignments"></param>
        /// <returns></returns>
        public string CaseAssignmenNoticeParing(List<CaseAssignmentComplaintNotice> ComplaintNotice)
        {

            if (ComplaintNotice == null || ComplaintNotice.Count() <= 0) return string.Empty;

            var sb = new StringBuilder();

            var noticeUsers = ComplaintNotice.SelectMany(x => x.NoticeUsers).ToList();

            if (noticeUsers != null)
            {
                sb.Append(string.Join("/", noticeUsers));
            }



            return noticeUsers != null ? sb.ToString() : string.Empty;
        }

        /// <summary>
        /// 解析 通知時間 *目前通知時間為 當下解析Template的時間*
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public string NotifyDateTimeParing(DateTime time)
        {
            return DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }

        /// <summary>
        /// 解析 建立時間
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public string CreateDateTimeParing(DateTime time)
        {
            return time.ToString("yyyy/MM/dd HH:mm:ss");
        }

        /// <summary>
        /// 解析 建立人員 *Template 解析時的當下客服人員姓名*
        /// </summary>
        /// <param name="createUserName"></param>
        /// <returns></returns>
        public string CreateUserNameParing(string createUserName)
        {
            return ContextUtility.GetUserIdentity().Name ?? "system";
        }


        /// <summary>
        /// 解析單位編號 *目前只有門市在用*
        /// </summary>
        /// <param name="caseAssignments"></param>
        /// <returns></returns>
        public string UnitCodeParing(List<CaseAssignment> caseAssignments)
        {


            if (caseAssignments == null || caseAssignments.Count() <= 0) return string.Empty;

            var storeCodes = caseAssignments
                            .FirstOrDefault()?
                            .CaseAssignmentUsers
                            .Where(x => x.UnitType == UnitType.Store && x.CaseComplainedUserType == CaseComplainedUserType.Responsibility)?
                            .Select(x => x.StoreNo)
                            .ToList() ?? new List<string>();

            return string.Join("/", storeCodes);
        }


        /// <summary>
        /// 解析轉派對象 
        /// </summary>
        /// <param name="caseAssignments"></param>
        /// <returns></returns>
        public string AssignmentUsersParing(List<CaseAssignment> caseAssignments)
        {
            if (caseAssignments == null || caseAssignments.Count() <= 0) return string.Empty;

            var assignmentUsers = caseAssignments
                            .FirstOrDefault()?
                            .CaseAssignmentUsers
                            .Where(x => x.CaseComplainedUserType == CaseComplainedUserType.Responsibility)?
                            .Select(x => x.NodeName)
                            .ToList() ?? new List<string>();

            return string.Join("/", assignmentUsers);
        }

        #region private Method


        private List<string> ParsinRequestParam(string req)
        {
            var result = new List<string>();

            if (!string.IsNullOrEmpty(req))
            {

                var _temp = req;
                var tag = string.Empty;

                // 取得 Keyword poisition
                var left_index = _temp.IndexOf('=');
                var right_index = _temp.IndexOf('&');


                // 表示最後一筆 request 參數
                if (right_index < 0)
                {
                    tag = _temp.Substring(left_index);
                    result.Add(tag.Replace("=", "").Replace("&", ""));
                    return result;
                }
                else
                {
                    // 計算 擷取字串長度
                    var offset = (right_index - left_index) + 1;

                    tag = _temp.Substring(left_index, offset);
                    result.Add(tag.Replace("=", "").Replace("&", ""));
                }



                // 清掉這次的 Tag 文字
                var subTemp = _temp.Substring(right_index + 1);

                result.AddRange(ParsinRequestParam(subTemp));

            }


            return result;
        }

        private string ForQuestionClassifictionLevel(string qc, int level)
        {
            string result = "";
            var qcs = qc.Split('@');

            if (qcs.Length < level)
            {
                result = string.Join("-", qcs);
            }
            else
            {
                string[] tmpArray = new string[level];
                for (int i = 0; i < level; i++)
                {
                    tmpArray[i] = qcs[i];
                }
                result = string.Join("-", tmpArray);
            }
            return result;
        }
        #endregion
    }
}
