using System;
using System.Collections.Generic;
using System.Linq;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Master.Models.QuestionClassification
{
    public class QuestionClassificationListViewModel
    {
        public QuestionClassificationListViewModel()
        {
        }

        public QuestionClassificationListViewModel(Domain.Master.QuestionClassification domain)
        {
            this.ID = domain.ID;
            this.OrganizationType = domain.OrganizationType;
            this.Node_ID = domain.NodeID;
            this.IsEnable = domain.IsEnabled ? "啟用" : "停用";
            this.BuName = domain.NodeName;

            this.Names = domain.ParentNamePathByArray;

            //this.AnswerNames = domain.QuesionClassificationAnswer?.Select(x => x.Content).ToArray() ?? new string[] { };
        }

        public int ID { get; set; }

        public string BuName { get; set; }

        public OrganizationType OrganizationType { get; set; }

        public int Node_ID { get; set; }

        //public string BuName { get; set; }

        public string[] Names { get; set; }

        public string IsEnable { get; set; }

        public string[] AnswerNames { get; set; }

        public List<string> getParentNames(Domain.Master.QuestionClassification domain)
        {
            var names = new List<string>();
            names.Add(domain.Name);
            if (domain.Parent != null)
            {
                names.AddRange(this.getParentNames(domain.Parent));
            }
            return names;
        }
    }
}
