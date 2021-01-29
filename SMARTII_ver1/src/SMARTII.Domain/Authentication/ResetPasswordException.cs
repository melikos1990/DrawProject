using System;
using System.Runtime.Serialization;

namespace SMARTII.Domain.Authentication
{
    public class ResetPasswordException : Exception
    {
        public ResetPasswordException()
        {
        }

        public ResetPasswordException(string message)
            : base(message)
        {
        }

        public ResetPasswordException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected ResetPasswordException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public override string Message
        {
            get
            {
                return "change";
            }
        }
    }
}