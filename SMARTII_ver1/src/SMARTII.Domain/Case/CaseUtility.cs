using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Case
{
    public static class CaseUtility
    {
        public static bool AtLeastOneRespinsibility(this Case @case)
        {

            if (@case.CaseComplainedUsers == null || @case.CaseComplainedUsers.Count == 0)
                return true;

            return @case.CaseComplainedUsers.Count() > 0 &&
                   @case.CaseComplainedUsers.Any(x => x.CaseComplainedUserType == CaseComplainedUserType.Responsibility);
        }

   
        public static List<Case> WhereObject(this IEnumerable<Case> collection, User userInfo)
        {
        
            if (string.IsNullOrEmpty(userInfo?.Mobile) == false)
            {
                collection = collection.FilterPhone(userInfo.Mobile.Trim());                
            }

            if (string.IsNullOrEmpty(userInfo?.Name) == false)
            {
                collection = collection.FilterName(userInfo.Name.Trim());   
            }

            if (string.IsNullOrEmpty(userInfo?.Email) == false)
            {
                collection = collection.FilterEmail(userInfo.Email.Trim());
            }

            return collection.ToList();
        }


        #region filter

        private static List<Case> FilterEmail(this IEnumerable<Case> collection, string email)
        {
            var result = collection.Where(x =>
                           x.CaseConcatUsers.Any(g => g.Email.ContainsIfEmpty(email)) ||
                           x.CaseComplainedUsers.Any(g => g.Email.ContainsIfEmpty(email))
                           )
                           .ToList();

            return result;
        }

        private static List<Case> FilterName(this IEnumerable<Case> collection, string name)
        {
            var result = collection.Where(x =>
                            x.CaseConcatUsers.Any(g => g.UserName.ContainsIfEmpty(name)) ||
                            x.CaseComplainedUsers.Any(g => g.UserName.ContainsIfEmpty(name))
                            )
                            .ToList();

            return result;
        }

        private static List<Case> FilterPhone(this IEnumerable<Case> collection, string phone)
        {
            var result = collection.Where(x =>
                          x.CaseConcatUsers.Any(g => g.Telephone.ContainsIfEmpty(phone)) ||
                          x.CaseConcatUsers.Any(g => g.TelephoneBak.ContainsIfEmpty(phone)) ||
                          x.CaseConcatUsers.Any(g => g.Mobile.ContainsIfEmpty(phone)) ||
                          x.CaseComplainedUsers.Any(g => g.Telephone.ContainsIfEmpty(phone)) ||
                          x.CaseComplainedUsers.Any(g => g.TelephoneBak.ContainsIfEmpty(phone)) ||
                          x.CaseComplainedUsers.Any(g => g.Mobile.ContainsIfEmpty(phone))
                          )
                          .ToList();

            return result;
        }

        #endregion
    }
}
