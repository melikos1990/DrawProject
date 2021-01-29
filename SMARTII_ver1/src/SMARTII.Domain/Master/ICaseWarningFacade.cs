using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMARTII.Domain.Case;

namespace SMARTII.Domain.Master
{
    public interface ICaseWarningFacade
    {
        /// <summary>
        /// 單一新增明細
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        Task Create(CaseWarning data);
        /// <summary>
        /// 更新明細
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        Task Update(CaseWarning data);
    }
}
