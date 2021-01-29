using AutoMapper;

namespace SMARTII.COMMON_BU.Mapper
{
    public class ProfileExpression : SMARTII.Domain.Mapper.IProfileExpression
    {
        public void AddProfile(IMapperConfigurationExpression expression)
        {
            expression.AddProfile<MasterProfile.ItemProfile>();
            expression.AddProfile<OrganizationProfile.StoreProfile>();
        }
    }
}
