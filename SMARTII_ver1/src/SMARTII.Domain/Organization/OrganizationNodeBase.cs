using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using AutoMapper;
using MoreLinq;
using Newtonsoft.Json;
using SMARTII.Domain.Data;

namespace SMARTII.Domain.Organization
{
    public class OrganizationNodeBase
    {
        public string Name { get; set; }
        public int NodeID { get; set; }
        public string NodeKey { get; set; }
        public Boolean IsEnabled { get; set; }
        public int? NodeType { get; set; }
        public string NodeTypeKey { get; set; }
        public int? ParentLocator { get; set; }
        public string ParentPath { get; set; }
        public int LeftBoundary { get; set; }

        public OrganizationType OrganizationType { get; set; }
        public int RightBoundary { get; set; }
        public List<IRecursivelyModel> Children { get; set; } = new List<IRecursivelyModel>();

        public List<JobPosition> JobPosition { get; set; }

        [JsonIgnore]
        [IgnoreMap]
        public List<Job> Jobs
        {
            get
            {
                return JobPosition?
                        .Select(g => g.Job)
                        .ToList();
            }
        }

        [JsonIgnore]
        [IgnoreMap]
        public List<User> Users
        {
            get
            {
                return JobPosition?
                        .SelectMany(g => g.Users)
                        .DistinctBy(x => x.UserID)
                        .ToList();
            }
        }

        #region impl

        [JsonIgnore]
        [IgnoreMap]
        public int ID
        {
            get
            {
                return this.NodeID;
            }
            set
            {
                this.NodeID = value;
            }
        }

        #endregion impl

        #region Dirty Field

        [IgnoreMap]
        public bool Target { get; set; }

        [IgnoreMap]
        public ExpandoObject Extend { get; set; }

        #endregion Dirty Field
    }
}
