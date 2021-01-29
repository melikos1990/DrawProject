using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMARTII.Domain.Report
{
    public interface IPPCLIFEFactory
    {


        #region 報表
        /// <summary>
        /// 統一藥品-品牌商品與問題歸類
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        byte[] GenerateBrandCalcExcel(DateTime start, DateTime end);
        /// <summary>
        /// 統一藥品來電紀錄 前台下載
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        Task<byte[]> GenerateOnCallExcel(DateTime start, DateTime end);
        /// <summary>
        /// 統一藥品來電紀錄 Batch 不顯示個人資訊 ，而是顯示商品
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        Task<byte[]> GenerateOnCallExcelToBatch(DateTime start, DateTime end);
        /// <summary>
        /// 統一藥品來電紀錄-代理品牌
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        Task<byte[]> GenerateProxyBrandExcel(DateTime start, DateTime end);
        /// <summary>
        /// 統一藥品來電紀錄-自有品牌
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        Task<byte[]> GenerateOwnBrandExcel(DateTime start, DateTime end);
        /// <summary>
        /// 統一藥品來電紀錄-醫容美學
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        Task<byte[]> GenerateAestheticMedicineExcel(DateTime start, DateTime end);


        #endregion

    }
}
