using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMARTII.Domain.Master
{
    public interface IQuestionClassificationGuideFacade
    {
        /// <summary>
        /// 單一新增流程引導
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task Create(QuestionClassificationGuide data);
        /// <summary>
        /// 單一更新流程引導
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task Update(QuestionClassificationGuide data);
    }
}
