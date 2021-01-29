using SMARTII.Database.SMARTII;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.System;

namespace SMARTII.Assist.Mapper
{
    public static class SystemProfile
    {
        public class SystemParameterProfile : AutoMapper.Profile
        {
            public SystemParameterProfile()
            {
                CreateMap<SYSTEM_PARAMETER, SystemParameter>()
                     .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                     .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                     .ForMember(dest => dest.UpdateDateTime, opt => opt.MapFrom(src => src.UPDATE_DATETIME))
                     .ForMember(dest => dest.UpdateUserName, opt => opt.MapFrom(src => src.UPDATE_USERNAME))
                     .ForMember(dest => dest.ActiveDateTime, opt => opt.MapFrom(src => src.ACTIVE_DATETIME))
                     .ForMember(dest => dest.IsUnDeletable, opt => opt.MapFrom(src => src.IS_UNDELETABLE))
                     .ForMember(dest => dest.NextValue, opt => opt.MapFrom(src => src.NEXT_VALUE))
                     .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                     .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.KEY))
                     .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.VALUE))
                     .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.TEXT))
                     .IgnoreAllNonExisting()
                     .ReverseMap();
            }
        }

        public class SystemLogProfile : AutoMapper.Profile
        {
            public SystemLogProfile()
            {
                CreateMap<SYSTEM_LOG, SystemLog>()
                     .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                     .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.CONTENT))
                     .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                     .ForMember(dest => dest.CreateUserAccount, opt => opt.MapFrom(src => src.CREATE_USER_ACCOUNT))
                     .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                     .ForMember(dest => dest.FeatureName, opt => opt.MapFrom(src => src.FEATURE_NAME))
                     .ForMember(dest => dest.FeatureTag, opt => opt.MapFrom(src => src.FEATURE_TAG))
                     .ForMember(dest => dest.Operator, opt => opt.MapFrom(src => (AuthenticationType)src.OPERATOR))
                     .ReverseMap()
                     .ForPath(dest => dest.OPERATOR, opt => opt.MapFrom(src => (int)src.Operator))
                     .IgnoreAllNonExisting();
            }
        }
    }
}
