using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Common;
using SMARTII.Domain.Master;

namespace SMARTII.Service.Report.Service
{
    public class ImportUserService : IImportUserService
    {
        private readonly ICommonAggregate _CommonAggregate;
        private readonly IImportUserFactory _ImportUserFactory;

        public ImportUserService(ICommonAggregate CommonAggregate, IImportUserFactory ImportUserFactory)
        {
            _CommonAggregate = CommonAggregate;
            _ImportUserFactory = ImportUserFactory;
        }

        /// <summary>
        /// 使用者匯入
        /// </summary>
        public void ImportUser()
        {
            var errorMsg = new List<string>();
            try
            {
                _CommonAggregate.Logger.Info($"【使用者匯入】，開始匯入使用者");
                _CommonAggregate.Logger.Info($"【使用者匯入】，取得Excel檔案路徑");
                //取得excel檔案路徑
                string filePath = GlobalizationCache.Instance.ImportUserFilePath;

                _CommonAggregate.Logger.Info($"【使用者匯入】，讀取Excel檔案");
                //讀取excel檔案
                var workbook = new XLWorkbook(filePath);
                _CommonAggregate.Logger.Info($"【使用者匯入】，解析Sheet，依照各BU匯入使用者");
                //解析sheet，依照各BU匯入門市

                foreach (var item in workbook.Worksheets)
                {
                    string ErrorMsg = "";
                    //string BuName = GetBuName(item.Name);
                    //if (string.IsNullOrWhiteSpace(BuName))
                    //{
                    //    errorMsg.Add($"{item.Name}無法解析BU_Key");
                    //    break;
                    //}
                    _CommonAggregate.Logger.Info($"【使用者匯入】，BU:{item.Name}，匯入使用者");
                    var success = _ImportUserFactory.ImportUser(item, out ErrorMsg);
                    if (!success)
                        errorMsg.Add(item.Name + " 使用者匯入失敗，" + ErrorMsg);
                }
                _CommonAggregate.Logger.Info($"【使用者匯入】，結束匯入門市");
                if (errorMsg.Count() != 0)
                {
                    foreach (var item in errorMsg)
                    {
                        _CommonAggregate.Logger.Info($"【使用者匯入】失敗，原因 : " + item.ToString());
                    }
                    throw new Exception("使用者匯入資料錯誤");
                }
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Info($"【使用者匯入】失敗，原因 : " + ex.ToString());
                _CommonAggregate.Loggers["Email"].Error($"【使用者匯入】失敗，原因 : {ex.Message}。，+錯誤明細：{string.Join("/n", errorMsg)}");
            }
        }
    }
}
