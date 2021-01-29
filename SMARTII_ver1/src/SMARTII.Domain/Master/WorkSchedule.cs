using System;
using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Master
{
    public class WorkSchedule : IOrganizationRelationship
    {
        public int ID { get; set; }

        public DateTime Date { get; set; }
        public WorkType WorkType { get; set; }
        public string Title { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public string UpdateUserName { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string CreateUserName { get; set; }

        #region impl

        public OrganizationType OrganizationType { get; set; }
        public int NodeID { get; set; }
        public string NodeName { get; set; }
        public IOrganizationNode Node { get; set; }

        #endregion impl
    }
}
