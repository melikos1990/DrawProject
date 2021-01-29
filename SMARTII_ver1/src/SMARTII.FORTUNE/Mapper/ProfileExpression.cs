using AutoMapper;

namespace SMARTII.FORTUNE.Mapper
{
    public class ProfileExpression : SMARTII.Domain.Mapper.IProfileExpression
    {
        public void AddProfile(IMapperConfigurationExpression expression)
        {
            expression.AddProfile<OrganizationProfileProfile.StoreProfile>();
        }
    }
}
