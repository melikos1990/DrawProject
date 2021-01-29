using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMARTII.Domain.Case;

namespace SMARTII.Domain.Master
{
    public interface ICaseFinishReasonFacade
    {
        /// <summary>
        /// 單一新增明細
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        Task Create(CaseFinishReasonData data);
        /// <summary>
        /// 單一新增分類
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task CreateClassification(CaseFinishReasonClassification data);

        /// <summary>
        /// 更新明細
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        Task Update(CaseFinishReasonData data);

        /// <summary>
        /// 更新分類
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task UpdateClassification(CaseFinishReasonClassification data);

        Task<(bool, CaseFinishReasonData)> CheckExistDefault(int ID);

    }
}
