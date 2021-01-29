using System.Collections.Generic;
using System.Linq;
using SMARTII.Domain.Data;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Master.Models.CaseAssignGroup
{
    public class CaseAssignGroupDetailViewModel
    {
        public CaseAssignGroupDetailViewModel()
        {
        }

        public CaseAssignGroupDetailViewModel(Domain.Case.CaseAssignGroup data)
        {
            this.ID = data.ID;
            this.BuID = data.NodeID;
            this.Name = data.Name;
            this.OrganizationType = data.OrganizationType;
            this.CaseAssignGroupType = data.CaseAssignGroupType;
            this.CreateDateTime = data.CreateDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.CreateUserName = data.CreateUserName;
            this.UpdateDateTime = data.UpdateDateTime.DisplayWhenNull(text: "無");
            this.UpdateUserName = data.UpdateUserName;
            this.Users = data.CaseAssignGroupUsers
                             .Select(x => new CaseAssignGroupUserListViewModel(x))
                             .ToList();
        }

        public Domain.Case.CaseAssignGroup ToDomain()
        {
            var result = new Domain.Case.CaseAssignGroup();

            result.ID = this.ID;
            result.Name = this.Name;
            result.NodeID = this.BuID;
            result.OrganizationType = this.OrganizationType;
            result.CaseAssignGroupType = this.CaseAssignGroupType;
            result.CaseAssignGroupUsers = this.Users?
                                                .Select(x => x.ToDomain())
                                                .ToList();

            return result;
        }

        /// <summary>
        /// 識別規格
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 企業別
        /// </summary>
        public int BuID { get; set; }

        /// <summary>
        /// 群組名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 組織類型
        /// </summary>
        public OrganizationType OrganizationType { get; set; }

        /// <summary>
        /// 群組類型
        /// </summary>
        public CaseAssignGroupType CaseAssignGroupType { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public string CreateDateTime { get; set; }

        /// <summary>
        /// 建立者
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public string UpdateDateTime { get; set; }

        /// <summary>
        /// 更新者
        /// </summary>
        public string UpdateUserName { get; set; }

        /// <summary>
        /// 底下的人員
        /// </summary>
        public List<CaseAssignGroupUserListViewModel> Users { get; set; }
    }
}