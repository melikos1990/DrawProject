using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Master.Models.QuestionClassification
{
    public class QuestionClassificationDetailViewModel
    {
        public QuestionClassificationDetailViewModel()
        {
        }

        public QuestionClassificationDetailViewModel(Domain.Master.QuestionClassification domain)
        {
            // 移除最後一個(自己)
            var parentName = domain.ParentNamePath;

            this.ID = domain.ID;
            this.BuID = domain.NodeID;
            this.Level = domain.Level;
            this.ParentNodeID = domain.ParentID;
            this.Name = domain.Name;
            this.IsEnable = domain.IsEnabled;
            this.Order = domain.Order;
            this.ParentNamePath = string.Join("-", parentName);
            this.OrganizationType = domain.OrganizationType;
            this.CreateTime = domain.CreateDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.CreateUserName = domain.CreateUserName;
            this.UpdateTime = domain.UpdateDateTime.DisplayWhenNull(text: "無");
            this.UpdateUserName = domain.UpdateUserName;

            
        }

        public QuestionClassificationDetailViewModel(Domain.Master.QuestionClassification domain, List<SelectItem> parentPath): this(domain)
        {
            this.ParentNodePath = parentPath;
        }

        public int ID { get; set; }

        public int BuID { get; set; }

        public string Name { get; set; }

        public int Level { get; set; }

        public int? ParentNodeID { get; set; }

        public bool IsEnable { get; set; }

        public string CreateUserName { get; set; }

        public string CreateTime { get; set; }

        public string UpdateUserName { get; set; }

        public string UpdateTime { get; set; }

        public int Order { get; set; }
        public string ParentNamePath { get; set; }

        public OrganizationType OrganizationType { get; set; }

        public List<QuestionClassificationAnswerViewModel> Answers { get; set; } = new List<QuestionClassificationAnswerViewModel>();

        public QuestionClassificationDetailViewModel Parent { get; set; }

        public List<SelectItem> ParentNodePath { get; set; }
    }
}
