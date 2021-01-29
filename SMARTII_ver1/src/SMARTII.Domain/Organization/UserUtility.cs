using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SMARTII.Database.SMARTII;

namespace SMARTII.Domain.Organization
{
    public static class UserUtility
    {
        public static Func<User, bool> ValidExpression() => (user) => user.IsEnabled == true;

        public static Func<User, bool> ValidExpression(string userID) => (user) => user.UserID == userID && user.IsEnabled == true;

        public static Func<User, bool> ValidExpression(List<string> userIDs) => (user) => userIDs.Contains(user.UserID) && user.IsEnabled == true;

        public static Expression<Func<USER, bool>> ValidExpressionByER() => (user) => user.IS_ENABLED == true;

        public static Expression<Func<USER, bool>> ValidExpressionByER(string userID) => (user) => user.USER_ID == userID && user.IS_ENABLED == true;

        public static Expression<Func<USER, bool>> ValidExpressionByER(List<string> userIDs) => (user) => userIDs.Contains(user.USER_ID) && user.IS_ENABLED == true;


        public static bool IsDuplicate(this List<User> users) => (users ?? new List<User>()).GroupBy(x => x.UserID).Any(g => g.Count() > 1);
    }
}
