using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMARTII.Domain.Master
{
    public interface IWorkScheduleFacade
    {
        /// <summary>
        /// 新增-特定假日
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        Task Create(List<WorkSchedule> group);
        /// <summary>
        /// 更新-特定假日
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        Task Update(WorkSchedule group);

        /// <summary>
        /// 確認是否已有案件計算過
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> ChackIncorporatedCase(int id);
    }
}
