using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Case;
using SMARTII.Domain.Common;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using SMARTII.Service.Report.Factory;

namespace SMARTII.Service.Report.Service
{
    public class ImportQuestionClassificationService : IImportQuestionClassificationService
    {
        private readonly ICaseAggregate _CaseAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly ICommonAggregate _CommonAggregate;
        private readonly IImportQuestionClassificationFactory _ImportQuestionClassificationFactory;

        public ImportQuestionClassificationService(ICaseAggregate CaseAggregate, IOrganizationAggregate OrganizationAggregate, ICommonAggregate CommonAggregate, IImportQuestionClassificationFactory ImportQuestionClassificationFactory)
        {
            _CaseAggregate = CaseAggregate;
            _OrganizationAggregate = OrganizationAggregate;
            _CommonAggregate = CommonAggregate;
            _ImportQuestionClassificationFactory = ImportQuestionClassificationFactory;
        }


        /// <summary>
        /// 匯入問題分類
        /// </summary>
        public void ImportQuestionClassification()
        {
            var errorMsg = new List<string>();
            try
            {
                _CommonAggregate.Logger.Info($"【問題分類匯入】，開始匯入問題分類");
                _CommonAggregate.Logger.Info($"【問題分類匯入】，取得Excel檔案路徑");
                //取得excel檔案路徑
                string filePath = GlobalizationCache.Instance.ImportQuestionClassificationFilePath;

                _CommonAggregate.Logger.Info($"【問題分類匯入】，讀取Excel檔案");
                //讀取excel檔案
                var workbook = new XLWorkbook(filePath);
                _CommonAggregate.Logger.Info($"【問題分類匯入】，解析Sheet，依照各BU匯入問題分類");
                //解析sheet，依照各BU匯入問題分類

                foreach (var item in workbook.Worksheets)
                {
                    string ErrorMsg = "";
                    string BuName = GetBuName(item.Name);
                    if (string.IsNullOrWhiteSpace(BuName))
                    {
                        errorMsg.Add($"{item.Name}無法解析BU_Key");
                        break;
                    }
                    _CommonAggregate.Logger.Info($"【問題分類匯入】，BU:{item.Name}，匯入問題分類");
                    var success = _ImportQuestionClassificationFactory.ImportQuestionClassification(item, BuName, out ErrorMsg);
                    if (!success)
                        errorMsg.Add(item.Name + " 問題分類匯入失敗，" + ErrorMsg);
                }
                _CommonAggregate.Logger.Info($"【問題分類匯入】，結束匯入問題分類");
                if (errorMsg.Count() != 0)
                {
                    foreach (var item in errorMsg)
                    {
                        _CommonAggregate.Logger.Info($"【問題分類匯入】失敗，原因 : " + item.ToString());
                    }
                    throw new Exception("問題分類匯入資料錯誤");
                }
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Info($"【問題分類匯入】失敗，原因 : " + ex.ToString());
                _CommonAggregate.Loggers["Email"].Error($"【問題分類匯入】失敗，原因 : {ex.Message}。，+錯誤明細：{string.Join("/n", errorMsg)}");
            }
        }

        private string GetBuName(string workSheetName)
        {
            var BuNameList = workSheetName.Split('_');
            if (BuNameList.Count() == 2)
            {
                return BuNameList[1];
            }
            else
            {
                return "";
            }

        }
    }
}
