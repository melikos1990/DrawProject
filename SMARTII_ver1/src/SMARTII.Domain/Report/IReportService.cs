using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultipartDataMediaFormatter.Infrastructure;

namespace SMARTII.Domain.Report
{
    public interface IReportService
    {
        HttpFile GetComplaintReport(string caseId, string invoiceId, bool isEncrypt = false);
        /// <summary>
        /// 統一藥品-品牌商品與問題歸類
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        void SendPPCLIFEBrandCalcExcel(DateTime start, DateTime end, string password = "");
        /// <summary>
        /// 統一藥品來電紀錄 Batch 寄信
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        void PPCLIFEBatchSendMail(DateTime start, DateTime end, string Type, string batchValue, string password = "");

        void SendEhopOnCallExcel(DateTime start, DateTime end, string password = "");


        void SendICCOnCallExcel(DateTime start, DateTime end, string password = "");


        void SendASOOnCallExcel(DateTime start, DateTime end, string password = "");

        void SendOpenPointOnCallExcel(DateTime start, DateTime end, string password = "");

        void SendColdStoneOnCallExcel(DateTime start, DateTime end, string password = "");


        void SendDonutOnCallExcel(DateTime start, DateTime end, string password = "");


        void Send21OnCallExcel(DateTime start, DateTime end, string password = "");


        void CaseRemindNotification();
    }
}
