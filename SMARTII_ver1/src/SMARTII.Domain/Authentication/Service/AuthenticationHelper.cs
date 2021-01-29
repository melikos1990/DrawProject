using System.Collections.Generic;
using System.Linq;
using SMARTII.Domain.Authentication.Object;

namespace SMARTII.Domain.Authentication.Service
{
    public class AuthenticationHelper
    {
        /// <summary>
        /// 計算頁面權限
        /// </summary>
        /// <param name="roleAuths"></param>
        /// <param name="userAuths"></param>
        /// <returns></returns>
        public List<PageAuth> CalcPageAuth(List<PageAuth> roleAuths, List<PageAuth> userAuths)
        {
            //準備要回傳的
            List<PageAuth> result = new List<PageAuth>();

            if (userAuths != null && userAuths.Count > 0)
                result.AddRange(userAuths);

            var userAuthDict = userAuths.ToDictionary(x => $"{x.Feature}");

            foreach (var roleAuth in roleAuths)
            {
                string key = $"{ roleAuth.Feature}";

                if (!userAuthDict.ContainsKey(key))
                {
                    result.Add(new PageAuth()
                    {
                        AuthenticationType = roleAuth.AuthenticationType,
                        Feature = roleAuth.Feature,
                        Deny = false,
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// 合併頁面權限 , 排除無權限的
        /// </summary>
        /// <param name="roleAuths"></param>
        /// <param name="userAuths"></param>
        /// <returns></returns>
        public List<PageAuth> MergedPageAuth(List<PageAuth> roleAuths, List<PageAuth> userAuths)
        {
            List<PageAuth> mergedList = new List<PageAuth>();

            mergedList.AddRange(userAuths);

            foreach (var roleAuth in roleAuths)
            {
                if (mergedList.Any(x => x.Feature == roleAuth.Feature) == false)
                    mergedList.Add(roleAuth);
            }

            var pageAuths = mergedList.Where(x => x.Deny == false &&
                                             x.AuthenticationType != AuthenticationType.None)
                                      .ToList();

            return pageAuths;
        }

        /// <summary>
        /// 合併完整得頁面權限
        /// </summary>
        /// <param name="roleAuths"></param>
        /// <param name="userAuths"></param>
        /// <returns></returns>
        public List<PageAuth> GetCompleteMergedPageAuth(List<PageAuth> roleAuths, List<PageAuth> userAuths)
        {
            var merged = CalcPageAuth(roleAuths, userAuths);

            var result = MergedPageAuth(merged, userAuths);

            return result;
        }
    }
}