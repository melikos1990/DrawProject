using System.Collections.Generic;
using System.Linq;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Master.Models.KMClassification
{
    public class KMClassificationNodeViewModel
    {
        public KMClassificationNodeViewModel(Domain.Master.KMClassification kMClassification, string pathNames)
        {
            pathNames = pathNames + "-" + kMClassification.Name;

            this.IsRoot = false;
            this.NodeID = kMClassification.NodeID;
            this.OrganizationType = kMClassification.OrganizationType;
            this.ClassificationID = kMClassification.ID;
            this.ClassificationName = kMClassification.Name;        
            this.PathName = pathNames;
            this.Children = kMClassification.Children
                                            .Select(x => new KMClassificationNodeViewModel(x as Domain.Master.KMClassification, pathNames))
                                            .ToList();
        }

        public KMClassificationNodeViewModel(HeaderQuarterNode node, List<Domain.Master.KMClassification> kMClassifications, string pathNames)
        {
            this.IsRoot = true;
            this.NodeID = node.NodeID;
            this.NodeName = node.Name;
            this.OrganizationType = node.OrganizationType;

            pathNames = node.Name;
            this.Children = kMClassifications.Select(x => new KMClassificationNodeViewModel(x, pathNames))
                                             .ToList();
        }

        public bool IsRoot { get; set; }

        public int NodeID { get; set; }

        public string NodeName { get; set; }

        public OrganizationType OrganizationType { get; set; }

        public int? ClassificationID { get; set; }

        public string ClassificationName { get; set; }

        public List<KMClassificationNodeViewModel> Children { get; set; } = new List<KMClassificationNodeViewModel>();

        public string PathName { get; set; }
    }
}
