using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace SMARTII.Domain.Organization
{
    public static class OrganizationUtility
    {
        public static int?[] GetOwnerRootNodeIDs<T>(this List<JobPosition> jobPositions) where T : JobPosition
        {
            return jobPositions.OfType<T>()
                               .Select(x => x.IdentificationID)
                               .ToArray();
        }

        public static int[] GetRootNodeParentPathArray(this string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return new int[] { };
            }

            var concat = path.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)
                             .Skip(2)
                             .ToArray();


            return Array.ConvertAll(concat, x => int.Parse(x));


        }


        public static IEnumerable<T> SortNode<T, C>(this IEnumerable<T> datas, Func<T, C> selector, C[] sorts)
        {
            var result = new List<T>();

            sorts.ForEach(sort =>
            {
                var data = datas.Single(x => selector(x).Equals(sort));

                result.Add(data);
            });

            return result;
        }

    }
}
