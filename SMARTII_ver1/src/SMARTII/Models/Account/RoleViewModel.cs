using System.Collections.Generic;
using System.Linq;
using SMARTII.Domain.Organization;

namespace SMARTII.Models.Account
{
    public class RoleViewModel
    {
        public RoleViewModel()
        {
        }

        public RoleViewModel(Role role)
        {
            this.ID = role.ID;
            this.Name = role.Name;
            this.Feature = role.Feature?
                               .Select(x => new PageAuthViewModel(x))?
                               .ToList();
        }

        /// <summary>
        /// 角色代號
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 角色名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 功能清單 (merged 過的)
        /// </summary>
        public List<PageAuthViewModel> Feature { get; set; }
    }
}