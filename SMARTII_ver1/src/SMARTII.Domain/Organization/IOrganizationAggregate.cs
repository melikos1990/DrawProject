using System.Dynamic;
using Ptc.Data.Condition2.Mssql.Repository;
using SMARTII.Database.SMARTII;

namespace SMARTII.Domain.Organization
{
    public interface IOrganizationAggregate
    {
        IMSSQLRepository<USER, User> User_T1_T2_ { get; }
        IMSSQLRepository<USER> User_T1_ { get; }
        IMSSQLRepository<ROLE, Role> Role_T1_T2_ { get; }
        IMSSQLRepository<ROLE> Role_T1_ { get; }
        IMSSQLRepository<VW_UNION_ORGANIZATION, OrganizationNodeBase> OrganizationNodeBase_T1_T2_ { get; }
        IMSSQLRepository<ORGANIZATION_NODE_DEFINITION, OrganizationNodeDefinition> OrganizationNodeDefinition_T1_T2_ { get; }
        IMSSQLRepository<ORGANIZATION_NODE_DEFINITION> OrganizationNodeDefinition_T1_ { get; }
        IMSSQLRepository<HEADQUARTERS_NODE, HeaderQuarterNode> HeaderQuarterNode_T1_T2_ { get; }
        IMSSQLRepository<HEADQUARTERS_NODE, IOrganizationNode> HeaderQuarterNode_T1_IOrganizationNode_ { get; }
        IMSSQLRepository<HEADQUARTERS_NODE> HeaderQuarterNode_T1_ { get; }
        IMSSQLRepository<VENDOR_NODE, VendorNode> VendorNode_T1_T2_ { get; }
        IMSSQLRepository<VENDOR_NODE, IOrganizationNode> VendorNode_T1_IOrganizationNode_ { get; }
        IMSSQLRepository<VENDOR_NODE> VendorNode_T1_ { get; }
        IMSSQLRepository<CALLCENTER_NODE, CallCenterNode> CallCenterNode_T1_T2_ { get; }
        IMSSQLRepository<CALLCENTER_NODE, IOrganizationNode> CallCenterNode_T1_IOrganizationNode_ { get; }
        IMSSQLRepository<CALLCENTER_NODE> CallCenterNode_T1_ { get; }
        IMSSQLRepository<ENTERPRISE> Enterprise_T1_ { get; }
        IMSSQLRepository<ENTERPRISE, Enterprise> Enterprise_T1_T2_ { get; }
        IMSSQLRepository<JOB> Job_T1_ { get; }
        IMSSQLRepository<JOB, Job> Job_T1_T2_ { get; }
        IMSSQLRepository<NODE_JOB, JobPosition> JobPosition_T1_T2_ { get; }
        IMSSQLRepository<STORE, Store<ExpandoObject>> Store_T1_T2_Expendo_ { get; }

        IMSSQLRepository<STORE> Store_T1_ { get; }
        IMSSQLRepository<STORE, Store> Store_T1_T2_ { get; }
    }
}
