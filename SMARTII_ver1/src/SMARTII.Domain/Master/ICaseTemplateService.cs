using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMARTII.Domain.Case;

namespace SMARTII.Domain.Master
{
    public interface ICaseTemplateService
    {
     

        Task<string> ParseTemplateUseExist<T>(string template, Func<T> func = null) where T : class, new();

    }
}
