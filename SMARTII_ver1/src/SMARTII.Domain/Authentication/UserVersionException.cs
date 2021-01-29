using System;
using SMARTII.Resource.Tag;

namespace SMARTII.Domain.Authentication
{
    public class UserVersionException : Exception
    {
        public UserVersionException()
        {
        }

        public override string Message
        {
            get
            {
                return User_lang.USER_VERSION_ERROR;
            }
        }
    }
}