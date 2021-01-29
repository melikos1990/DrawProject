using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMARTII.Domain.Organization
{
    public interface INodeDefinitionFacade
    {
        /// <summary>
        /// 單一更新定義
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task Update(OrganizationNodeDefinition data);

        /// <summary>
        /// 單一停用定義
        /// </summary>
        /// <param name="organizationType"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        Task Disable(OrganizationType organizationType, int ID);

        /// <summary>
        /// 單一更新定義職稱
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<Job> UpdateJob(Job data);

        /// <summary>
        /// 單一停用定義職稱
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        Task DisableJob(int ID);
    }
}
