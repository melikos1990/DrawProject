using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Organization.Models.OrganizarionNodeDefinition
{
    public class NodeDefinitionListViewModel
    {
        public NodeDefinitionListViewModel()
        {
        }

        public NodeDefinitionListViewModel(OrganizationNodeDefinition data)
        {
            this.ID = data.ID;
            this.Name = data.Name;
            this.OrganizationTypeName = data.OrganizationType.GetDescription();
            this.OrganizationType = data.OrganizationType;
            this.CreateDateTime = data.CreateDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.CreateUserName = data.CreateUserName;
            this.Level = data.Level;
            this.IsEnabled = data.IsEnabled.DisplayBit();
            this.IdentificationName = data.IdentificationName.DisplayWhenNull();
        }

        public int ID { get; set; }

        public string OrganizationTypeName { get; set; }

        public OrganizationType OrganizationType { get; set; }

        public string Name { get; set; }

        public int Level { get; set; }

        public string CreateDateTime { get; set; }

        public string CreateUserName { get; set; }

        public string IsEnabled { get; set; }

        public string IdentificationName { get; set; }
    }
}