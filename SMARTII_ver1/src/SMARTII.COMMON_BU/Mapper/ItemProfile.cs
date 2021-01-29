using System.Dynamic;
using Newtonsoft.Json;
using SMARTII.Assist.Mapper;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;

namespace SMARTII.COMMON_BU.Mapper
{
    public class MasterProfile
    {
        public class ItemProfile : AutoMapper.Profile
        {
            public ItemProfile()
            {

                CreateMap<ITEM, Item<ExpandoObject>>()
                    .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                    .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.CODE))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NAME))
                    .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                    .ForMember(dest => dest.IsEnabled, opt => opt.MapFrom(src => src.IS_ENABLED))
                    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.DESCRIPTION))
                    .ForMember(dest => dest.OrganizationType, opt => opt.MapFrom(src => (OrganizationType)src.ORGANIZATION_TYPE))
                    .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                    .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                    .ForMember(dest => dest.UpdateDateTime, opt => opt.MapFrom(src => src.UPDATE_DATETIME))
                    .ForMember(dest => dest.UpdateUserName, opt => opt.MapFrom(src => src.UPDATE_USERNAME))
                    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.DESCRIPTION))
                    .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.IMAGE_PATH) ? new string[] { } :
                        JsonConvert.DeserializeObject<string[]>(src.IMAGE_PATH)))
                    .ForMember(dest => dest.JContent, opt => opt.MapFrom(src => src.J_CONTENT))
                    .ReverseMap()
                    .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte)src.OrganizationType))
                    .ForPath(dest => dest.IMAGE_PATH, opt => opt.MapFrom(src => src.ImagePath == null ?
                        JsonConvert.SerializeObject(new string[] { }) : JsonConvert.SerializeObject(src.ImagePath)))
                    .IgnoreAllNonExisting();

                CreateMap<ITEM, Item>()
                    .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                    .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.CODE))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NAME))
                    .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                    .ForMember(dest => dest.IsEnabled, opt => opt.MapFrom(src => src.IS_ENABLED))
                    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.DESCRIPTION))
                    .ForMember(dest => dest.OrganizationType, opt => opt.MapFrom(src => (OrganizationType)src.ORGANIZATION_TYPE))
                    .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                    .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                    .ForMember(dest => dest.UpdateDateTime, opt => opt.MapFrom(src => src.UPDATE_DATETIME))
                    .ForMember(dest => dest.UpdateUserName, opt => opt.MapFrom(src => src.UPDATE_USERNAME))
                    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.DESCRIPTION))
                    .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.IMAGE_PATH) ? new string[] { } :
                        JsonConvert.DeserializeObject<string[]>(src.IMAGE_PATH)))
                    .ReverseMap()
                    .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte)src.OrganizationType))
                    .ForPath(dest => dest.IMAGE_PATH, opt => opt.MapFrom(src => src.ImagePath == null ?
                        JsonConvert.SerializeObject(new string[] { }) : JsonConvert.SerializeObject(src.ImagePath)))
                    .IgnoreAllNonExisting();
                    //.ConstructUsing((x) => AutoMapper.Mapper.Map<Item<ExpandoObject>>(x))
                    //.IgnoreAllNonExisting();
            }
        }
        
    }
}
