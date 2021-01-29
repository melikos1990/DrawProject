using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ClosedXML.Excel;
using SMARTII.COMMON_BU;
using SMARTII.COMMON_BU.Service;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Common;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Report;
using SMARTII.Service.Cache;
using System.Transactions;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Data;
using static SMARTII.Domain.Cache.EssentialCache;

namespace SMARTII.EShop.Service
{
    public class EShopFactory : IEShopFactory, IImportStoreFactory
    {
        private readonly EShopFacade _EShopFacade;
        private readonly ReportProvider _ReportProvider;
        private readonly ReportFacade _ReportFacade;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly ReportFactory _ReportFactory;
        private readonly ICommonAggregate _CommonAggregate;

        public EShopFactory(EShopFacade EShopFacade, ReportProvider ReportProvider, ReportFacade ReportFacade, IOrganizationAggregate OrganizationAggregate, ReportFactory ReportFactory, ICommonAggregate CommonAggregate)
        {
            _EShopFacade = EShopFacade;
            _ReportProvider = ReportProvider;
            _ReportFacade = ReportFacade;
            _OrganizationAggregate = OrganizationAggregate;
            _ReportFactory = ReportFactory;
            _CommonAggregate = CommonAggregate;
        }


        /// <summary>
        /// 統一藥品eShop來電紀錄
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public async Task<byte[]> GenerateOnCallExcel(DateTime start, DateTime end)
        {

            //Facade 找資料SP
            var list = await _EShopFacade.GenerateOnCallExcel(start, end, EssentialCache.BusinessKeyValue.EShop);
            //匯出excel
            var result = _ReportProvider.GetOnCallExcel(list, end);
            return result;
        }

        /// <summary>
        /// 匯入門市
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns></returns>
        public bool ImportStore(IXLWorksheet worksheet, string NodeKey, out string ErrorMessage)
        {
            bool success = true;
            ErrorMessage = "";
            try
            {
                // 定義資料起始、結束 Cell
                var firstCell = worksheet.FirstCellUsed();
                var lastCell = worksheet.LastCellUsed();

                // 使用資料起始、結束 Cell，來定義出一個資料範圍
                var data = worksheet.Range(firstCell.Address, lastCell.Address);

                // 將資料範圍轉型
                var table = data.AsTable();

                // 開始抓出資料，並組合DomainModel
                var list = _ReportFactory.ImportStoreCommon(table);


                var con = new MSSQLCondition<HEADQUARTERS_NODE>();
                con.And(x => x.NODE_KEY == NodeKey);
                var Node = _OrganizationAggregate.HeaderQuarterNode_T1_T2_.Get(con);


                var conHead = new MSSQLCondition<HEADQUARTERS_NODE>();
                conHead.And(x => x.BU_ID == Node.BUID);
                conHead.IncludeBy(x => x.STORE);
                conHead.IncludeBy(x => x.ORGANIZATION_NODE_DEFINITION);
                conHead.IncludeBy(x => x.ORGANIZATION_NODE_DEFINITION.JOB);
                var HeadQuarters = _OrganizationAggregate.HeaderQuarterNode_T1_T2_.GetList(conHead).ToList();

                //尋找組織定義門市
                var conDefonition = new MSSQLCondition<ORGANIZATION_NODE_DEFINITION>();
                conDefonition.And(x => x.IDENTIFICATION_ID == Node.BUID && x.KEY == NodeDefinitionValue.Store && x.IS_ENABLED == true);
                var defonitions = _OrganizationAggregate.OrganizationNodeDefinition_T1_T2_.GetFirstOrDefault(conDefonition);
                if (defonitions == null)
                    throw new Exception($"該BU組織定義，未定義門市。");
                var defonitionID = defonitions.ID;

                var conJob = new MSSQLCondition<JOB>();
                conJob.And(x => x.DEFINITION_ID == defonitionID);
                var JobList = _OrganizationAggregate.Job_T1_T2_.GetList(conJob).ToList();
                if (!JobList.Any())
                {
                    throw new Exception("找不到職稱主檔清單，門市匯入無新增Job");
                }

                //檢查必填欄位 不可有重複資料
                _ReportFactory.VerifyStoreData(ref list, HeadQuarters, JobList);

                //匯入門市
                _ReportFactory.ImportStoreData(list, NodeKey, defonitions);
            }

            catch (Exception ex)
            {
                ErrorMessage = ex.ToString();
                success = false;
            }

            return success;
        }

        /// <summary>
        /// 匯入門市人員
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns></returns>
        public bool ImportStorePersonnel(IXLWorksheet worksheet, string NodeKey, out string ErrorMessage)
        {
            bool success = true;
            ErrorMessage = "";
            try
            {
                // 定義資料起始、結束 Cell
                var firstCell = worksheet.FirstCellUsed();
                var lastCell = worksheet.LastCellUsed();

                // 使用資料起始、結束 Cell，來定義出一個資料範圍
                var data = worksheet.Range(firstCell.Address, lastCell.Address);

                // 將資料範圍轉型
                var table = data.AsTable();

                // 開始抓出資料，並組合DomainModel
                var list = _ReportFactory.ImportStorePersonnelsCommon(table);


                var con = new MSSQLCondition<HEADQUARTERS_NODE>();
                con.And(x => x.NODE_KEY == NodeKey);
                var Node = _OrganizationAggregate.HeaderQuarterNode_T1_T2_.Get(con);


                var conHead = new MSSQLCondition<HEADQUARTERS_NODE>();
                conHead.And(x => x.BU_ID == Node.BUID);
                conHead.IncludeBy(x => x.STORE);
                conHead.IncludeBy(x => x.ORGANIZATION_NODE_DEFINITION);
                conHead.IncludeBy(x => x.ORGANIZATION_NODE_DEFINITION.JOB);
                conHead.IncludeBy(x => x.ORGANIZATION_NODE_DEFINITION.JOB.Select(c => c.NODE_JOB));
                var HeadQuarters = _OrganizationAggregate.HeaderQuarterNode_T1_T2_.GetList(conHead);


                //尋找組織定義門市
                var conDefonition = new MSSQLCondition<ORGANIZATION_NODE_DEFINITION>();
                conDefonition.And(x => x.IDENTIFICATION_ID == Node.BUID && x.KEY == NodeDefinitionValue.Store && x.IS_ENABLED == true);
                var defonitions = _OrganizationAggregate.OrganizationNodeDefinition_T1_T2_.GetFirstOrDefault(conDefonition);
                if (defonitions == null)
                    throw new Exception($"該BU組織定義，未定義門市。");
                var defonitionID = defonitions.ID;

                //門市清單
                var storeList = HeadQuarters.Where(x => x.Store != null).Select(c => c.Store).ToList();

                //驗證資料
                _ReportFactory.VerifyStorePersonnelData(defonitionID, Node.BUID, ref list, storeList);

                //新增門市人員
                _ReportFactory.ImportStorePersonnelData(list, storeList);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.ToString();
                success = false;
            }
            return success;
        }

        /// <summary>
        /// 匯入門市資訊
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns></returns>
        public bool ImportStoreInformation(IXLWorksheet worksheet, string BuName, out string ErrorMessage)
        {
            bool success = true;
            ErrorMessage = "";
            try
            {
                // 定義資料起始、結束 Cell
                var firstCell = worksheet.FirstCellUsed();
                var lastCell = worksheet.LastCellUsed();

                // 使用資料起始、結束 Cell，來定義出一個資料範圍
                var data = worksheet.Range(firstCell.Address, lastCell.Address);

                // 將資料範圍轉型
                var table = data.AsTable();


                //共通處理部分
                _CommonAggregate.Logger.Info($"【門市資訊】，BU: {BuName} sheet內容轉換");
                var list = _ReportFactory.ImportStoreInfoCommon(table);

                //客製化處理部分
                list.ForEach(x =>
                {
                    if (DataStorage.StoreTypeDict.Any(c => c.Key == BuName) && x.StoreType != 0)
                    {
                        var typeList = DataStorage.StoreTypeDict.FirstOrDefault(c => c.Key == BuName).Value.Where(a => a.Key == x.StoreType.ToString());
                        if (typeList.Count() == 0)
                        {
                            throw new Exception($"查無此門市型態: {x.StoreType}");
                        }
                        x.StoreType = int.Parse(typeList.FirstOrDefault().Key);
                    }
                    else
                        x.StoreType = 0;
                });

                //寫入Store資料
                _CommonAggregate.Logger.Info($"【門市資訊】，BU: {BuName} 將Exceel檔案寫入STORE表中");
                success = _ReportFactory.UpdateStoreInfoData(list);
            }

            catch (Exception ex)
            {
                ErrorMessage = ex.ToString();
                success = false;
            }

            return success;
        }
    }
}
