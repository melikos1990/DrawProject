using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Master;

namespace SMARTII.Service.Organization.Resolver
{
    public class UserResolver
    {
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly IMasterAggregate _MasterAggregate;

        public UserResolver(IOrganizationAggregate OrganizationAggregate, IMasterAggregate MasterAggregate)
        {
            _OrganizationAggregate = OrganizationAggregate;
            _MasterAggregate = MasterAggregate;
        }

        private User GetUser(string userID)
        {
            var con = new MSSQLCondition<USER>(x => x.USER_ID == userID);

            return _OrganizationAggregate.User_T1_T2_.GetFirstOrDefault(con);
        }

        private UserParameter GetUserParameter(string userID)
        {
            var con = new MSSQLCondition<USER_PARAMETER>(x => x.USER_ID == userID);

            return _MasterAggregate.UserParameter_T1_T2_.GetFirstOrDefault(con);
        }

        public IEnumerable<T> ResolveCollection<T>(IEnumerable<T> data) where T : IUserRelationship, new()
        {
            IDictionary<string, User> dist = new Dictionary<string, User>();

            var group = data.GroupBy(x => new
            {
                UserID = x.UserID,
            });

            group.ForEach(pair =>
            {
                User user = GetUser(pair.Key.UserID);

                if (user != null)
                    dist.Add($"{pair.Key.UserID}", user);
            });

            data.ForEach(item =>
            {
                var user = dist[$"{item.UserID}"];

                item.User = user;
                item.UserID = user.UserID;
                item.UserName = user.Name;
            });

            return data;
        }

        public T Resolve<T>(T data) where T : IUserRelationship, new()
        {
            User user = GetUser(data.UserID);

            if (user != null)
            {
                data.User = user;
                data.UserID = user.UserID;
                data.UserName = user.Name;
            }

            return data;
        }

        public IEnumerable<T> GetUserImageResolve<T>(IEnumerable<T> data) where T : IUserImageRelationship, new()
        {
            IDictionary<string, string> dist = new Dictionary<string, string>();

            var group = data.GroupBy(x => new
            {
                UserID = x.CreateUserID,
            });

            group.ForEach(pair =>
            {
                var user = GetUserParameter(pair.Key.UserID);

                if (user != null)
                    dist.Add($"{pair.Key.UserID}", user.ImagePath);
            });

            data.ForEach(item =>
            {
                var user = dist[$"{item.CreateUserID}"];

                item.ImagePath = user;
            });

            return data;
        }
    }
}
