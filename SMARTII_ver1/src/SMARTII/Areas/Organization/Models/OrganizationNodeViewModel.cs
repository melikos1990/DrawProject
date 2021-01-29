using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Organization.Models
{
    public class OrganizationNodeViewModel
    {
        public OrganizationNodeViewModel()
        {
        }

        public OrganizationNodeViewModel(IOrganizationNode data)
        {
            this.ID = data.NodeID;
            this.Name = data.Name;
            this.DefindName = data.OrganizationNodeDefinitaion?.Name;
            this.DefindID = data.OrganizationNodeDefinitaion?.ID;
            this.DefindKey = data.OrganizationNodeDefinitaion?.Key;
            this.LeftBoundary = data.LeftBoundary;
            this.RightBoundary = data.RightBoundary;
            this.Level = data.Level;
            this.IsEnabled = data.IsEnabled;
            this.NodeTypeKey = data.NodeTypeKey;
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public int? DefindID { get; set; }
        public string DefindName { get; set; }
        public string DefindKey { get; set; }
        public int LeftBoundary { get; set; }
        public int RightBoundary { get; set; }
        public int Level { get; set; }
        public bool Target { get; set; }
        public virtual bool IsPresist { get; }
        public string NodeTypeKey { get; set; }

        public OrganizationType OrganizationType { get; set; }
    }
}
