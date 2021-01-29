using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using SMARTII.COMMON_BU.Service;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Report;
using static SMARTII.Domain.Cache.EssentialCache;

namespace SMARTII.FORTUNE.Service
{
    public class FORTUNEFacade
    {
        private readonly ICaseAggregate _CaseAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly IMasterAggregate _MasterAggregate;
        private readonly ReportFacade _ReportFacade;

        public FORTUNEFacade(ICaseAggregate CaseAggregate, IOrganizationAggregate OrganizationAggregate, IMasterAggregate MasterAggregate, ReportFacade ReportFacade)
        {
            _CaseAggregate = CaseAggregate;
            _OrganizationAggregate = OrganizationAggregate;
            _MasterAggregate = MasterAggregate;
            _ReportFacade = ReportFacade;
        }
        

        /// <summary>
        /// 轉派案件處理時間 轉字串
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        private string ToCustomString(TimeSpan span)
        {
            return string.Format("{0:00}時{1:00}分{2:00}秒", (span.Hours + span.Days * 24), span.Minutes, span.Seconds);
        }
        /// <summary>
        /// 回覆內容
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string ResumeRetryContent(List<CaseAssignmentResume> data)
        {
            string RetryContent = "";
            if (data != null && data.Count != 0)
            {
                foreach (var y in data)
                {
                    RetryContent += "*********************************\r\n";
                    RetryContent += "回覆單位:" + y.CreateNodeName + "\r\n" + "回覆內容:\r\n" + y.Content + "\r\n" + "回覆時間:" + y.CreateDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    RetryContent += "\t\n";
                };
            }
            return RetryContent;
        }

        /// <summary>
        /// 聯繫電話
        /// </summary>
        /// <param name="concatableUser"></param>
        /// <returns></returns>
        private string GetMobile(ConcatableUser concatableUser)
        {
            List<string> list = new List<string>();

            list.Add(concatableUser.Mobile);
            list.Add(concatableUser.Telephone);
            list.Add(concatableUser.TelephoneBak);
            list.RemoveAll(x => x == null);

            string result = list.Count() == 0 ? "" : String.Join("\r\n", list.ToArray());
            return result;
        }
    }
}
