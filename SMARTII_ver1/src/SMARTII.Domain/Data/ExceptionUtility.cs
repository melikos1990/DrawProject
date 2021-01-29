using System;

namespace SMARTII.Domain.Data
{
    public static class ExceptionUtility
    {
        public static string PrefixMessage(this Exception ex, string prefix)
        {
            return $"{prefix}-{ex.Message}";
        }

        public static string PrefixDevMessage(this Exception ex, string prefix)
        {
            return $"{prefix}-{ex.ToString()}";
        }
    }
}
