using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using ClosedXML.Excel;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Case;
using SMARTII.Domain.Common;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Report;

namespace SMARTII.Service.Master.Service
{
    public class ImportStoreService : IImportStoreService
    {
        private readonly ICaseAggregate _CaseAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly ICommonAggregate _CommonAggregate;
        private readonly IIndex<string, IImportStoreFactory> _ImportStoreFactory;

        public ImportStoreService(ICaseAggregate CaseAggregate, IOrganizationAggregate OrganizationAggregate, ICommonAggregate CommonAggregate, IIndex<string, IImportStoreFactory> ImportStoreFactory)
        {
            _CaseAggregate = CaseAggregate;
            _OrganizationAggregate = OrganizationAggregate;
            _CommonAggregate = CommonAggregate;
            _ImportStoreFactory = ImportStoreFactory;
        }



        /// <summary>
        /// 匯入門市
        /// </summary>
        public void ImportStore()
        {
            var errorMsg = new List<string>();
            try
            {
                _CommonAggregate.Logger.Info($"【門市匯入】，開始匯入門市");
                _CommonAggregate.Logger.Info($"【門市匯入】，取得Excel檔案路徑");
                //取得excel檔案路徑
                string filePath = GlobalizationCache.Instance.ImportStoreFilePath;

                _CommonAggregate.Logger.Info($"【門市匯入】，讀取Excel檔案");
                //讀取excel檔案
                var workbook = new XLWorkbook(filePath);
                _CommonAggregate.Logger.Info($"【門市匯入】，解析Sheet，依照各BU匯入門市");
                //解析sheet，依照各BU匯入門市

                foreach (var item in workbook.Worksheets)
                {
                    string ErrorMsg = "";
                    string BuName = GetBuName(item.Name);
                    if (string.IsNullOrWhiteSpace(BuName))
                    {
                        errorMsg.Add($"{item.Name}無法解析BU_Key");
                        break;
                    }
                    _CommonAggregate.Logger.Info($"【門市匯入】，BU:{item.Name}，匯入門市");
                    var success = _ImportStoreFactory[BuName].ImportStore(item, BuName, out ErrorMsg);
                    if (!success)
                        errorMsg.Add(item.Name+ " 門市匯入失敗，" + ErrorMsg);
                }
                _CommonAggregate.Logger.Info($"【門市匯入】，結束匯入門市");
                if (errorMsg.Count() != 0)
                {
                    foreach (var item in errorMsg)
                    {
                        _CommonAggregate.Logger.Info($"【門市匯入】失敗，原因 : " + item.ToString());
                    }
                    throw new Exception("門市匯入資料錯誤");
                }
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Info($"【門市匯入】失敗，原因 : " + ex.ToString());
                _CommonAggregate.Loggers["Email"].Error($"【門市匯入】失敗，原因 : {ex.Message}。，+錯誤明細：{string.Join("/n", errorMsg)}");
            }
        }
        /// <summary>
        /// 匯入門市人員
        /// </summary>
        public void ImportStorePersonnel()
        {
            var errorMsg = new List<string>();
            try
            {
                _CommonAggregate.Logger.Info($"【門市人員匯入】，開始匯入門市");
                _CommonAggregate.Logger.Info($"【門市人員匯入】，取得Excel檔案路徑");
                //取得excel檔案路徑
                string filePath = GlobalizationCache.Instance.ImportStorePersonnelFilePath;

                _CommonAggregate.Logger.Info($"【門市人員匯入】，讀取Excel檔案");
                //讀取excel檔案
                var workbook = new XLWorkbook(filePath);
                _CommonAggregate.Logger.Info($"【門市人員匯入】，解析Sheet，依照各BU匯入門市");
                //解析sheet，依照各BU匯入門市

                foreach (var item in workbook.Worksheets)
                {
                    string ErrorMsg = "";
                    string BuName = GetBuName(item.Name);
                    if (string.IsNullOrWhiteSpace(BuName))
                    {
                        errorMsg.Add($"{item.Name}無法解析BU_Key");
                        break;
                    }
                    _CommonAggregate.Logger.Info($"【門市人員匯入】，BU:{item.Name}，匯入門市");
                    var success = _ImportStoreFactory[BuName].ImportStorePersonnel(item, BuName, out ErrorMsg);
                    if (!success)
                        errorMsg.Add(item.Name + " 門市人員匯入失敗，" + ErrorMsg);
                }
                _CommonAggregate.Logger.Info($"【門市人員匯入】，結束匯入門市");
                if (errorMsg.Count() != 0)
                {
                    foreach (var item in errorMsg)
                    {
                        _CommonAggregate.Logger.Info($"【門市人員匯入】失敗，原因 : " + item.ToString());
                    }
                    throw new Exception("門市人員匯入資料錯誤");
                }

            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Info($"【門市人員匯入】失敗，原因 : " + ex.ToString());
                _CommonAggregate.Loggers["Email"].Error($"【門市人員匯入】失敗，原因 : {ex.Message}");
            }
        }
        /// <summary>
        /// 匯入門市資訊
        /// </summary>
        public void ImportStoreInformation()
        {
            List<string> errorMsg = new List<string>();
            try
            {
                _CommonAggregate.Logger.Info($"【門市資訊匯入】，開始匯入門市資訊");
                _CommonAggregate.Logger.Info($"【門市資訊匯入】，取得Excel檔案路徑");
                //取得excel檔案路徑
                string filePath = GlobalizationCache.Instance.ImportStoreInformationFilePath;

                _CommonAggregate.Logger.Info($"【門市資訊匯入】，讀取Excel檔案");
                //讀取excel檔案
                var workbook = new XLWorkbook(filePath);
                _CommonAggregate.Logger.Info($"【門市資訊匯入】，解析Sheet，依照各BU匯入門市");
                //解析sheet，依照各BU匯入門市

                foreach (var item in workbook.Worksheets)
                {
                    string ErrorMsg = "";
                    string BuName = GetBuName(item.Name);
                    if (string.IsNullOrWhiteSpace(BuName))
                    {
                        errorMsg.Add($"{item.Name}無法解析BU_Key");
                        break;
                    }
                    var success = _ImportStoreFactory[BuName].ImportStoreInformation(item, BuName, out ErrorMsg);
                    if (!success)
                        errorMsg.Add(item.Name + " 門市資訊匯入失敗，" + ErrorMsg);
                }
                _CommonAggregate.Logger.Info($"【門市資訊匯入】，結束匯入門市");
                if (errorMsg.Count() != 0)
                {
                    foreach (var item in errorMsg)
                    {
                        _CommonAggregate.Logger.Info($"【門市資訊匯入】失敗，原因 : " + item.ToString());
                    }
                    throw new Exception("門市資訊資料錯誤");
                }
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Info($"【門市資訊匯入】失敗，原因 : " + ex.ToString());
                _CommonAggregate.Loggers["Email"].Error($"【門市資訊匯入】失敗，原因 : {ex.Message}");
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
