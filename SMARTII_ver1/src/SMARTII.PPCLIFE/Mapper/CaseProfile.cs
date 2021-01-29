using SMARTII.Assist.Mapper;
using SMARTII.COMMON_BU;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Case;
using SMARTII.PPCLIFE.Domain;

namespace SMARTII.PPCLIFE.Mapper
{
    public class CaseProfile
    {
        public class CaseItemProfile : AutoMapper.Profile
        {
            public CaseItemProfile()
            {
                CreateMap<CASE_ITEM, PPCLIFE_CaseItem>()
                     .ForMember(dest => dest.Case, opt => opt.MapFrom(src=> src.CASE))
                     .ForMember(dest => dest.Item, opt => opt.MapFrom(src => src.ITEM))
                     .ForMember(dest => dest.ItemID, opt => opt.MapFrom(src => src.ITEM_ID))
                     .ForMember(dest => dest.JContent, opt => opt.MapFrom(src => src.JCONTENT))
                     .ForMember(dest => dest.CaseID, opt => opt.MapFrom(src => src.CASE_ID))
                     .ReverseMap()
                     .ForPath(dest => dest.CASE, opt => opt.MapFrom(src => default(CASE)))
                     .ForPath(dest => dest.ITEM, opt => opt.MapFrom(src => default(ITEM)))
                     .IgnoreAllNonExisting();
            }
        }

        #region 統一藥品來電紀錄 SP
        public class Sp_GetOnCallRecordListProfile : AutoMapper.Profile
        {
            public Sp_GetOnCallRecordListProfile()
            {
                CreateMap<SP_PPCLIFE_BATCH_RECORD_Result, PPCLIFECallHistory>()
                    .ForMember(dest => dest.CaseID, opt => opt.MapFrom(src => src.CaseID))
                    .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CreatDateTime))
                    .ForMember(dest => dest.ClassificationID, opt => opt.MapFrom(src => src.ClassificationID))
                    .ForMember(dest => dest.CaseContent, opt => opt.MapFrom(src => src.CaseContent))
                    .ForMember(dest => dest.FinishContent, opt => opt.MapFrom(src => src.FinishContent))
                    .ForMember(dest => dest.ApplyUserName, opt => opt.MapFrom(src => src.ApplyUserName))
                    .ForMember(dest => dest.FinishDateTime, opt => opt.MapFrom(src => src.FinishDateTime))
                    .ForMember(dest => dest.CaseSourceType, opt => opt.MapFrom(src => (CaseSourceType)src.SourceType))
                    .IgnoreAllNonExisting();
            }
        }
        public class Sp_GetOnCallComplaintListProfile : AutoMapper.Profile
        {
            public Sp_GetOnCallComplaintListProfile()
            {
                CreateMap<SP_PPCLIFE_BATCH_COMPLAINT_Result, PPCLIFEComplaintHistory>()
                    .ForMember(dest => dest.CaseID, opt => opt.MapFrom(src => src.CaseID))
                    .ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CreatDateTime))
                    .ForMember(dest => dest.ClassificationID, opt => opt.MapFrom(src => src.ClassificationID))
                    .ForMember(dest => dest.CaseContent, opt => opt.MapFrom(src => src.CaseContent))
                    .ForMember(dest => dest.FinishContent, opt => opt.MapFrom(src => src.FinishContent))
                    .ForMember(dest => dest.ApplyUserName, opt => opt.MapFrom(src => src.ApplyUserName))
                    .ForMember(dest => dest.SourceType, opt => opt.MapFrom(src => (CaseSourceType)src.SourceType))
                    .IgnoreAllNonExisting();
            }
        }

        #endregion
    }
}
