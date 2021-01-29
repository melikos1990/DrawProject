using AutoMapper;

namespace SMARTII.PPCLIFE.Mapper
{
    public class ProfileExpression : SMARTII.Domain.Mapper.IProfileExpression
    {
        public void AddProfile(IMapperConfigurationExpression expression)
        {
            expression.AddProfile<MasterProfile.ItemProfile>();
            expression.AddProfile<CaseProfile.CaseItemProfile>();
            expression.AddProfile<CaseProfile.Sp_GetOnCallRecordListProfile>();
            expression.AddProfile<CaseProfile.Sp_GetOnCallComplaintListProfile>();
            
        }
    }
}
