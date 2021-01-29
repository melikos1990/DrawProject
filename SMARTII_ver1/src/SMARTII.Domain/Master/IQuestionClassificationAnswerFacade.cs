using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMARTII.Domain.Master
{
    public interface IQuestionClassificationAnswerFacade
    {
        /// <summary>
        /// 新增常用語
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task Create(List<QuestionClassificationAnswer> data);
        /// <summary>
        /// 單一更新常用語
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task Update(QuestionClassificationAnswer data);


    }
}
