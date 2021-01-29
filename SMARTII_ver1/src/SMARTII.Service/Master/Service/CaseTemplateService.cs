using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;
using SMARTII.Domain.Master;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;

namespace SMARTII.Service.Master.Service
{
    public class CaseTemplateService : ICaseTemplateService
    {

        private readonly ICaseAggregate _CaseAggregate;
        private readonly IMasterAggregate _MasterAggregate;

        public CaseTemplateService(ICaseAggregate CaseAggregate,
                                   IMasterAggregate MasterAggregate)
        {
            _CaseAggregate = CaseAggregate;
            _MasterAggregate = MasterAggregate;
        }

      
        public async Task<string> ParseTemplateUseExist<T>(string template, Func<T> func = null) where T : class, new()
        {

            if (func == null)
                return template;

            T target = func.Invoke();

            if (target == null)
                throw new NullReferenceException(Case_lang.CASE_UNDEFIND);
  
            var result = target.ParsingTemplate(template);

            return await result.Async();
        }

    
    }
}
