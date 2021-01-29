﻿using Newtonsoft.Json;
using SMARTII.Assist.Mapper;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using SMARTII.PPCLIFE.Domain;

namespace SMARTII.PPCLIFE.Mapper
{
    public class MasterProfile
    {
        public class ItemProfile : AutoMapper.Profile
        {
            public ItemProfile()
            {
                CreateMap<ITEM, Item<PPCLIFE_Item>>()
                     .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                     .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                     .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                     .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.DESCRIPTION))
                     .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                     .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.CODE))
                     .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NAME))
                     .ForMember(dest => dest.OrganizationType, opt => opt.MapFrom(src => (OrganizationType)src.ORGANIZATION_TYPE))
                     .ForMember(dest => dest.UpdateDateTime, opt => opt.MapFrom(src => src.UPDATE_DATETIME))
                     .ForMember(dest => dest.UpdateUserName, opt => opt.MapFrom(src => src.UPDATE_USERNAME))
                     .ForMember(dest => dest.JContent, opt => opt.MapFrom(src => src.J_CONTENT))
                     .ForMember(dest => dest.IsEnabled, opt => opt.MapFrom(src => src.IS_ENABLED))
                     .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.IMAGE_PATH) ? new string[] { } :
                        JsonConvert.DeserializeObject<string[]>(src.IMAGE_PATH)))
                     .ReverseMap()
                     .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte)src.OrganizationType))
                     .ForPath(dest => dest.IMAGE_PATH, opt => opt.MapFrom(src => src.ImagePath == null ?
                        JsonConvert.SerializeObject(new string[] { }) : JsonConvert.SerializeObject(src.ImagePath)))
                     .IgnoreAllNonExisting();


                CreateMap<STORE, Store<PPCLIFE_Store>>()
                    .ForMember(dest => dest.OrganizationType, opt => opt.MapFrom(src => (OrganizationType)src.ORGANIZATION_TYPE))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NAME))
                    .ForMember(dest => dest.JContent, opt => opt.MapFrom(src => src.J_CONTENT))
                    .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.CODE))
                    .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CREATE_DATETIME))
                    .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CREATE_USERNAME))
                    .ForMember(dest => dest.NodeID, opt => opt.MapFrom(src => src.NODE_ID))
                    .ForMember(dest => dest.BuID, opt => opt.MapFrom(src => src.HEADQUARTERS_NODE.BU_ID))
                    .ForMember(dest => dest.BuName, opt => opt.MapFrom(src => src.HEADQUARTERS_NODE.HEADQUARTERS_NODE2.NAME))
                    .ForMember(dest => dest.BuKey, opt => opt.MapFrom(src => src.HEADQUARTERS_NODE.HEADQUARTERS_NODE2.NODE_KEY))
                    .ForMember(dest => dest.Telephone, opt => opt.MapFrom(src => src.TELEPHONE))
                    .ForMember(dest => dest.UpdateDateTime, opt => opt.MapFrom(src => src.UPDATE_DATETIME))
                    .ForMember(dest => dest.UpdateUserName, opt => opt.MapFrom(src => src.UPDATE_USERNAME))
                    .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.ADDRESS))
                    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EMAIL))
                    .ForMember(dest => dest.StoreOpenDateTime, opt => opt.MapFrom(src => src.STORE_OPEN_DATETIME))
                    .ForMember(dest => dest.StoreCloseDateTime, opt => opt.MapFrom(src => src.STORE_CLOSE_DATETIME))
                    .ForMember(dest => dest.StoreType, opt => opt.MapFrom(src => src.STORE_TYPE))
                    .ForMember(dest => dest.Memo, opt => opt.MapFrom(src => src.MEMO))
                    .ForMember(dest => dest.IsEnabled, opt => opt.MapFrom(src => src.IS_ENABLED))
                    .ForMember(dest => dest.ServiceTime, opt => opt.MapFrom(src => src.SERVICE_TIME))
                    .ForMember(dest => dest.OwnerNodeJobID, opt => opt.MapFrom(src => src.OWNER_NODE_JOB_ID))
                    .ForMember(dest => dest.SupervisorNodeJobID, opt => opt.MapFrom(src => src.SUPERVISOR_NODE_JOB_ID))
                    .ReverseMap()
                    .ForPath(dest => dest.ORGANIZATION_TYPE, opt => opt.MapFrom(src => (byte)src.OrganizationType))
                    .IgnoreAllNonExisting();
            }
        }
    }
}
