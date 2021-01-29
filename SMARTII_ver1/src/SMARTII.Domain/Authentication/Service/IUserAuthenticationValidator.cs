using System;
using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Authentication.Service
{
    public interface IUserAuthenticationValidator
    {
        void Valid(User tokenUser);

        Boolean ValidEffectiveness(User tokenUser);

        Boolean ValidVersion(User tokenUser);
    }
}