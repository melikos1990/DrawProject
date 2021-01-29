using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMARTII.Domain.Master
{
    public interface IQuestionClassificationFacade
    {
        Task UpdateAsync(QuestionClassification domain);

        Task DeleteAsync(int id);

        Task DeleteRangeAsync(int[] ids);

        Task CreateAsync(QuestionClassification domain);
    }
}
