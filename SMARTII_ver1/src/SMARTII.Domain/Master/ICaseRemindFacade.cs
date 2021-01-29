using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMARTII.Domain.Master
{
    public interface ICaseRemindFacade
    {
        /// <summary>
        /// 新增案件追蹤
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task Create(Domain.Case.CaseRemind data);
        /// <summary>
        /// 更新案件追蹤
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task Update(Domain.Case.CaseRemind data);
    }
}
