using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Organization;

namespace SMARTII.Assist.Mapper
{
    public static class OrganizationProfile
    {
        public class UserProfile : AutoMapper.Profile
        {
            public UserProfile()
            {
                CreateMap<USER, User>()
                     .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.USER_ID))
                     .ForMember(dest => dest.Account, opt => opt.MapFrom(src => src.ACCOUNT))
                     .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                     .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                     .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EMAIL))
                     .ForMember(dest => dest.IsAD, opt => opt.MapFrom(src => src.IS_AD))
                     .ForMember(dest => dest.IsEnabled, opt => opt.MapFrom(src => src.IS_ENABLED))
                     .ForMember(dest => dest.LockoutDateTime, opt => opt.MapFrom(src => src.LOCKOUT_DATETIME))
                     .ForMember(dest => dest.LastChangePasswordDateTime, opt => opt.MapFrom(src => src.LAST_CHANGE_PASSWORD_DATETIME))
                     .ForMember(dest => dest.Telephone, opt => opt.MapFrom(src => src.TELEPHONE))
                     .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NAME))
                     .ForMember(dest => dest.MobilePushID, opt => opt.MapFrom(src => src.MOBILE_PUSH_ID))
                     .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.IMAGE_PATH))
                     .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.PASSWORD))
                     .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.ADDRESS))
                     .ForMember(dest => dest.Mobile, opt => opt.MapFrom(src => src.MOBILE))
                     .ForMember(dest => dest.Ext, opt => opt.MapFrom(src => src.EXT))
                     .ForMember(dest => dest.UpdateDateTime, opt => opt.MapFrom(src => src.UPDATE_DATETIME))
                     .ForMember(dest => dest.UpdateUserName, opt => opt.MapFrom(src => src.UPDATE_USERNAME))
                     .ForMember(dest => dest.Version, opt => opt.MapFrom(src => src.VERSION))
                     .ForMember(dest => dest.IsSystemUser, opt => opt.MapFrom(src => src.IS_SYSTEM_USER))
                     .ForMember(dest => dest.ActiveStartDateTime, opt => opt.MapFrom(src => src.ACTIVE_START_DATETIME))
                     .ForMember(dest => dest.ActiveEndDateTime, opt => opt.MapFrom(src => src.ACTIVE_END_DATETIME))
                     .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.ROLE))
                     .ForMember(dest => dest.JobPositions, opt => opt.MapFrom(src => src.NODE_JOB))
                     .ForMember(dest => dest.UserParameter, opt => opt.MapFrom(src => src.USER_PARAMETER))

                     .ForMember(dest => dest.Feature, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.FEATURE) ?
                        new List<PageAuth>() : JsonConvert.DeserializeObject<List<PageAuth>>(src.FEATURE)))
                     .ForMember(dest => dest.PastPasswordQueue, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.PAST_PASSWORD_RECORD) ?
                        new PasswordQueue() : new PasswordQueue(JsonConvert.DeserializeObject<string[]>(src.PAST_PASSWORD_RECORD))))
                     .ReverseMap()
                      .ForPath(dest => dest.FEATURE, opt => opt.MapFrom(src => src.Feature == null ?
                      JsonConvert.SerializeObject(new PageAuth[] { }) : JsonConvert.SerializeObject(src.Feature.ToArray())))
                     .ForPath(dest => dest.PAST_PASSWORD_RECORD, opt => opt.MapFrom(src => src.PastPasswordQueue == null ?
                        JsonConvert.SerializeObject(new string[] { }) : JsonConvert.SerializeObject(src.PastPasswordQueue.ToArray())))
                     .IgnoreAllNonExisting();
            }
        }

        public class RoleProfile : AutoMapper.Profile
        {
            public RoleProfile()
            {
                CreateMap<ROLE, Role>()
                     .ForMember(dest => dest.UpdateDateTime, opt => opt.MapFrom(src => src.UPDATE_DATETIME))
                     .ForMember(dest => dest.UpdateUserName, opt => opt.MapFrom(src => src.UPDATE_USERNAME))
                     .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                     .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                     .ForMember(dest => dest.IsEnabled, opt => opt.MapFrom(src => src.IS_ENABLED))
                     .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NAME))
                     .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.USER))
                     .ForMember(dest => dest.Feature, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.FEATURE) ?
                        new List<PageAuth>() : JsonConvert.DeserializeObject<List<PageAuth>>(src.FEATURE)))
                     .ReverseMap()
                      .ForPath(dest => dest.FEATURE, opt => opt.MapFrom(src => src.Feature == null ?
                      JsonConvert.SerializeObject(new PageAuth[] { }) : JsonConvert.SerializeObject(src.Feature.ToArray())))
                     .IgnoreAllNonExisting();
            }
        }

        public class JobProfile : AutoMapper.Profile
        {
            public JobProfile()
            {
                CreateMap<JOB, Job>()
                   .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                   .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                   .ForMember(dest => dest.IsEnabled, opt => opt.MapFrom(src => src.IS_ENABLED))
                   .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                   .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.LEVEL))
                   .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.KEY))
                   .ForMember(dest => dest.DefinitionID, opt => opt.MapFrom(src => src.DEFINITION_ID))
                   .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NAME))
                   .ForMember(dest => dest.UpdateDateTime, opt => opt.MapFrom(src => src.UPDATE_DATETIME))
                   .ForMember(dest => dest.UpdateUserName, opt => opt.MapFrom(src => src.UPDATE_USERNAME))
                   .ForMember(dest => dest.OrganizationNodeDefinitaion, opt => opt.MapFrom(src => src.ORGANIZATION_NODE_DEFINITION))
                   .ForMember(dest => dest.OrganizationType, opt => opt.MapFrom(src => (OrganizationType)src.ORGANIZATION_TYPE))
                   .ReverseMap()
                   .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte)src.OrganizationType))
                   .IgnoreAllNonExisting();
            }
        }

        public class OrganizationNodeDefinitaionProfile : AutoMapper.Profile
        {
            public OrganizationNodeDefinitaionProfile()
            {
                CreateMap<ORGANIZATION_NODE_DEFINITION, OrganizationNodeDefinition>()
                     .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                     .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                     .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                     .ForMember(dest => dest.IsEnabled, opt => opt.MapFrom(src => src.IS_ENABLED))
                     .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NAME))
                     .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.LEVEL))
                     .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.KEY))
                     .ForMember(dest => dest.UpdateDateTime, opt => opt.MapFrom(src => src.UPDATE_DATETIME))
                     .ForMember(dest => dest.UpdateUserName, opt => opt.MapFrom(src => src.UPDATE_USERNAME))
                     .ForMember(dest => dest.Identification, opt => opt.MapFrom(src => src.IDENTIFICATION_ID))
                     .ForMember(dest => dest.IdentificationName, opt => opt.MapFrom(src => src.IDENTIFICATION_NAME))
                     .ForMember(dest => dest.OrganizationType, opt => opt.MapFrom(src => (OrganizationType)src.ORGANIZATION_TYPE))
                     .ForMember(dest => dest.Jobs, opt => opt.MapFrom(src => src.JOB))
                     .ReverseMap()
                     .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte)src.OrganizationType))
                     .IgnoreAllNonExisting();
            }
        }

        public class CallCenterNodeProfile : AutoMapper.Profile
        {
            public CallCenterNodeProfile()
            {
                CreateMap<CALLCENTER_NODE, CallCenterNode>()
                     .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                     .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                     .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.DEPTH_LEVEL))
                     .ForMember(dest => dest.IsEnabled, opt => opt.MapFrom(src => src.IS_ENABLED))
                     .ForMember(dest => dest.LeftBoundary, opt => opt.MapFrom(src => src.LEFT_BOUNDARY))
                     .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NAME))
                     .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                     .ForMember(dest => dest.NodeKey, opt => opt.MapFrom(src => src.NODE_KEY))
                     .ForMember(dest => dest.NodeType, opt => opt.MapFrom(src => src.NODE_TYPE))
                     .ForMember(dest => dest.NodeTypeKey, opt => opt.MapFrom(src => src.NODE_TYPE_KEY))
                     .ForMember(dest => dest.ParentLocator, opt => opt.MapFrom(src => src.PARENT_ID))
                     .ForMember(dest => dest.ParentPath, opt => opt.MapFrom(src => src.PARENT_PATH))
                     .ForMember(dest => dest.CallCenterID, opt => opt.MapFrom(src => src.CALLCENTER_ID))
                     .ForMember(dest => dest.WorkProcessType, opt => opt.MapFrom(src => (WorkProcessType)src.WORK_PROCESS_TYPE))
                     .ForMember(dest => dest.IsWorkProcessNotice, opt => opt.MapFrom(src => src.IS_WORK_PROCESS_NOTICE))                   
                     .ForMember(dest => dest.RightBoundary, opt => opt.MapFrom(src => src.RIGHT_BOUNDARY))
                     .ForMember(dest => dest.UpdateDateTime, opt => opt.MapFrom(src => src.UPDATE_DATETIME))
                     .ForMember(dest => dest.UpdateUserName, opt => opt.MapFrom(src => src.UPDATE_USERNAME))
                     .ForMember(dest => dest.OrganizationNodeDefinitaion, opt => opt.MapFrom(src => src.ORGANIZATION_NODE_DEFINITION))
                     .ForMember(dest => dest.HeaderQuarterNodes, opt => opt.MapFrom(src => src.HEADQUARTERS_NODE))
                     .ForMember(dest => dest.Queues, opt => opt.MapFrom(src => src.QUEUE))
                     .ForMember(dest => dest.CallCenterChildren, opt => opt.MapFrom(src => src.CALLCENTER_NODE1))
                     .ForMember(dest => dest.CallCenterParent, opt => opt.MapFrom(src => src.CALLCENTER_NODE2))
                     .ReverseMap()
                     .ForPath(dest => dest.WORK_PROCESS_TYPE, opt => opt.MapFrom(src => (byte)src.WorkProcessType))
                     .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte)src.OrganizationType))
                     .IgnoreAllNonExisting();

                CreateMap<CALLCENTER_NODE, IOrganizationNode>()
                   .ConstructUsing((x) => AutoMapper.Mapper.Map<CallCenterNode>(x))
                   .IgnoreAllNonExisting();

                CreateMap<CALLCENTER_NODE, IExecutiveOrganizationNode>()
                   .ConstructUsing((x) => AutoMapper.Mapper.Map<CallCenterNode>(x))
                   .IgnoreAllNonExisting();
            }
        }

        public class HeaderQuarterNodeProfile : AutoMapper.Profile
        {
            public HeaderQuarterNodeProfile()
            {
                CreateMap<HEADQUARTERS_NODE, HeaderQuarterNode>()
                     .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                     .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                     .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.DEPTH_LEVEL))
                     .ForMember(dest => dest.IsEnabled, opt => opt.MapFrom(src => src.IS_ENABLED))
                     .ForMember(dest => dest.LeftBoundary, opt => opt.MapFrom(src => src.LEFT_BOUNDARY))
                     .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NAME))
                     .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                     .ForMember(dest => dest.NodeKey, opt => opt.MapFrom(src => src.NODE_KEY))
                     .ForMember(dest => dest.NodeType, opt => opt.MapFrom(src => src.NODE_TYPE))
                     .ForMember(dest => dest.NodeTypeKey, opt => opt.MapFrom(src => src.NODE_TYPE_KEY))
                     .ForMember(dest => dest.ParentLocator, opt => opt.MapFrom(src => src.PARENT_ID))
                     .ForMember(dest => dest.ParentPath, opt => opt.MapFrom(src => src.PARENT_PATH))
                     .ForMember(dest => dest.BUID, opt => opt.MapFrom(src => src.BU_ID))
                     .ForMember(dest => dest.RightBoundary, opt => opt.MapFrom(src => src.RIGHT_BOUNDARY))
                     .ForMember(dest => dest.EnterpriseID, opt => opt.MapFrom(src => src.ENTERPRISE_ID))
                     .ForMember(dest => dest.UpdateDateTime, opt => opt.MapFrom(src => src.UPDATE_DATETIME))
                     .ForMember(dest => dest.UpdateUserName, opt => opt.MapFrom(src => src.UPDATE_USERNAME))
                     .ForMember(dest => dest.OrganizationNodeDefinitaion, opt => opt.MapFrom(src => src.ORGANIZATION_NODE_DEFINITION))
                     .ForMember(dest => dest.BusinessChildren, opt => opt.MapFrom(src => src.HEADQUARTERS_NODE1))
                     .ForMember(dest => dest.BusinessParent, opt => opt.MapFrom(src => src.HEADQUARTERS_NODE2))
                     .ForMember(dest => dest.VendorNodes, opt => opt.MapFrom(src => src.VENDOR_NODE))
                     .ForMember(dest => dest.CallCenterNodes, opt => opt.MapFrom(src => src.CALLCENTER_NODE))
                     .ForMember(dest => dest.Store, opt => opt.MapFrom(src => src.STORE))

                     .ReverseMap()
                     .IgnoreAllNonExisting();

                CreateMap<HEADQUARTERS_NODE, IOrganizationNode>()
                .ConstructUsing((x) => AutoMapper.Mapper.Map<HeaderQuarterNode>(x))
                .IgnoreAllNonExisting();

                CreateMap<HEADQUARTERS_NODE, IReceiveOrganizationNode>()
               .ConstructUsing((x) => AutoMapper.Mapper.Map<HeaderQuarterNode>(x))
               .IgnoreAllNonExisting();
            }
        }


        public class NodeBaseProfile : AutoMapper.Profile
        {
            public NodeBaseProfile()
            {
                CreateMap<VW_UNION_ORGANIZATION, OrganizationNodeBase>()
                     .ForMember(dest => dest.OrganizationType, opt => opt.MapFrom(src => (OrganizationType)src.ORGANIZATION_TYPE))
                     .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NAME))
                     .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                     .ForMember(dest => dest.ParentLocator, opt => opt.MapFrom(src => src.PARENT_ID))
                     .ForMember(dest => dest.ParentPath, opt => opt.MapFrom(src => src.PARENT_PATH))

                     .ReverseMap()
                     .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte)src.OrganizationType))
                     .IgnoreAllNonExisting();

               
            }
        }

        public class VendorNodeProfile : AutoMapper.Profile
        {
            public VendorNodeProfile()
            {
                CreateMap<VENDOR_NODE, VendorNode>()
                     .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                     .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                     .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.DEPTH_LEVEL))
                     .ForMember(dest => dest.IsEnabled, opt => opt.MapFrom(src => src.IS_ENABLED))
                     .ForMember(dest => dest.LeftBoundary, opt => opt.MapFrom(src => src.LEFT_BOUNDARY))
                     .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NAME))
                     .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                     .ForMember(dest => dest.NodeKey, opt => opt.MapFrom(src => src.NODE_KEY))
                     .ForMember(dest => dest.NodeType, opt => opt.MapFrom(src => src.NODE_TYPE))
                     .ForMember(dest => dest.NodeTypeKey, opt => opt.MapFrom(src => src.NODE_TYPE_KEY))
                     .ForMember(dest => dest.ParentLocator, opt => opt.MapFrom(src => src.PARENT_ID))
                     .ForMember(dest => dest.VendorID, opt => opt.MapFrom(src => src.VENDER_ID))
                     .ForMember(dest => dest.ParentPath, opt => opt.MapFrom(src => src.PARENT_PATH))
                     .ForMember(dest => dest.RightBoundary, opt => opt.MapFrom(src => src.RIGHT_BOUNDARY))
                     .ForMember(dest => dest.UpdateDateTime, opt => opt.MapFrom(src => src.UPDATE_DATETIME))
                     .ForMember(dest => dest.UpdateUserName, opt => opt.MapFrom(src => src.UPDATE_USERNAME))
                     .ForMember(dest => dest.OrganizationNodeDefinitaion, opt => opt.MapFrom(src => src.ORGANIZATION_NODE_DEFINITION))
                     .ForMember(dest => dest.HeaderQuarterNodes, opt => opt.MapFrom(src => src.HEADQUARTERS_NODE))
                     .ForMember(dest => dest.VendorChildren, opt => opt.MapFrom(src => src.VENDOR_NODE1))
                     .ForMember(dest => dest.VendorParent, opt => opt.MapFrom(src => src.VENDOR_NODE2))
                     .ReverseMap()
                     .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte)src.OrganizationType))
                     .IgnoreAllNonExisting();

                CreateMap<VENDOR_NODE, IOrganizationNode>()
                .ConstructUsing((x) => AutoMapper.Mapper.Map<VendorNode>(x))
                .IgnoreAllNonExisting();

                CreateMap<VENDOR_NODE, IExecutiveOrganizationNode>()
                .ConstructUsing((x) => AutoMapper.Mapper.Map<VendorNode>(x))
                .IgnoreAllNonExisting();
            }
        }

        public class UserJobPositionProfile : AutoMapper.Profile
        {
            public UserJobPositionProfile()
            {
                CreateMap<NODE_JOB, JobPosition>()
                 .PreserveReferences()
                 .ConstructUsing((x, context) =>
                 {
                     switch (((OrganizationType)x.ORGANIZATION_TYPE))
                     {
                         case OrganizationType.CallCenter:
                             return context.Mapper.Map<NODE_JOB, CallCenterJobPosition>(x);

                         case OrganizationType.Vendor:
                             return context.Mapper.Map<NODE_JOB, VendorJobPosition>(x);

                         case OrganizationType.HeaderQuarter:
                             return context.Mapper.Map<NODE_JOB, HeaderQuarterJobPosition>(x);

                         default:
                             return null;
                     }
                 })
                  .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.USER))
                  .ForMember(dest => dest.Job, opt => opt.MapFrom(src => src.JOB))
                  .IgnoreAllNonExisting();

                CreateMap<NODE_JOB, CallCenterJobPosition>()
                     .ForMember(dest => dest.LeftBoundary, opt => opt.MapFrom(src => src.LEFT_BOUNDARY))
                     .ForMember(dest => dest.RightBoundary, opt => opt.MapFrom(src => src.RIGHT_BOUNDARY))
                     .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                     .ForMember(dest => dest.IdentificationID, opt => opt.MapFrom(src => src.IDENTIFICATION_ID))
                     .ForMember(dest => dest.JobID, opt => opt.MapFrom(src => src.JOB_ID))
                     .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                     .ForMember(dest => dest.OrganizationType, opt => opt.MapFrom(src => (OrganizationType)src.ORGANIZATION_TYPE))
                     .ReverseMap()
                     .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte)src.OrganizationType))
                     .IgnoreAllNonExisting();

                CreateMap<NODE_JOB, VendorJobPosition>()
                     .ForMember(dest => dest.LeftBoundary, opt => opt.MapFrom(src => src.LEFT_BOUNDARY))
                     .ForMember(dest => dest.RightBoundary, opt => opt.MapFrom(src => src.RIGHT_BOUNDARY))
                     .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                     .ForMember(dest => dest.IdentificationID, opt => opt.MapFrom(src => src.IDENTIFICATION_ID))
                     .ForMember(dest => dest.JobID, opt => opt.MapFrom(src => src.JOB_ID))
                     .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                     .ForMember(dest => dest.OrganizationType, opt => opt.MapFrom(src => (OrganizationType)src.ORGANIZATION_TYPE))
                     .ReverseMap()
                     .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte)src.OrganizationType))
                     .IgnoreAllNonExisting();

                CreateMap<NODE_JOB, HeaderQuarterJobPosition>()
                     .ForMember(dest => dest.LeftBoundary, opt => opt.MapFrom(src => src.LEFT_BOUNDARY))
                     .ForMember(dest => dest.RightBoundary, opt => opt.MapFrom(src => src.RIGHT_BOUNDARY))
                     .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                     .ForMember(dest => dest.IdentificationID, opt => opt.MapFrom(src => src.IDENTIFICATION_ID))
                     .ForMember(dest => dest.Job, opt => opt.MapFrom(src => src.JOB))
                     .ForMember(dest => dest.JobID, opt => opt.MapFrom(src => src.JOB_ID))
                     .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                     .ForMember(dest => dest.OrganizationType, opt => opt.MapFrom(src => (OrganizationType)src.ORGANIZATION_TYPE))
                     .ReverseMap()
                     .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte)src.OrganizationType))
                     .IgnoreAllNonExisting();
            }
        }

        public class EnterpriseProfile : AutoMapper.Profile
        {
            public EnterpriseProfile()
            {
                CreateMap<ENTERPRISE, Enterprise>()
                     .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                     .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NAME))
                     .ForMember(dest => dest.IsEnabled, opt => opt.MapFrom(src => src.IS_ENABLED))
                     .ForMember(dest => dest.UpdateDateTime, opt => opt.MapFrom(src => src.UPDATE_DATETIME))
                     .ForMember(dest => dest.UpdateUserName, opt => opt.MapFrom(src => src.UPDATE_USERNAME))
                     .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                     .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))

                     .ReverseMap()
                     .IgnoreAllNonExisting();
            }
        }

        
    }
}
