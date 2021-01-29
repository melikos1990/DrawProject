using System;
using System.Collections.Generic;
using System.Linq;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Organization.Models.OrganizarionNodeDefinition
{
    public class NodeDefinitionDetailViewModel
    {
        public NodeDefinitionDetailViewModel()
        {
        }

        public NodeDefinitionDetailViewModel(OrganizationNodeDefinition data)
        {
            this.ID = data.ID;
            this.Name = data.Name;
            this.Identification = data.Identification;
            this.IdentificationName = data.IdentificationName;
            this.OrganizationType = data.OrganizationType;
            this.CreateDateTime = data.CreateDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.CreateUserName = data.CreateUserName;
            this.UpdateDateTime = data.UpdateDateTime.DisplayWhenNull();
            this.UpdateUserName = data.UpdateUserName;
            this.IsEnabled = data.IsEnabled;
            this.Level = data.Level;
            this.Key = data.Key;
            this.Jobs = data.Jobs?.Select(x => new JobListViewModel(x)).OrderBy(x=>x.Level).ToList();
        }

        public int ID { get; set; }

        public OrganizationType OrganizationType { get; set; }

        public string Name { get; set; }

        public int Level { get; set; }

        public string CreateDateTime { get; set; }

        public string CreateUserName { get; set; }

        public string UpdateDateTime { get; set; }

        public string UpdateUserName { get; set; }

        public int? Identification { get; set; }

        public string IdentificationName { get; set; }

        public Boolean IsEnabled { get; set; }

        public string Key { get; set; }

        public List<JobListViewModel> Jobs { get; set; }
    }
}
