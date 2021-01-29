using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Text.RegularExpressions;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Cache;
using SMARTII.Resource.Tag;

namespace SMARTII.Domain.Authentication.Service
{
    public class LDAPHelper
    {
        private DirectoryEntry Entry { get; set; }

        public void SetUp(string account, string password)
        {
            this.Entry = new DirectoryEntry(
                    ThirdPartyCache.Instance.LDAPUrl,
                    account,
                    password
                );
        }

        public List<ADUser> FindAll(string filter)
        {
            using (DirectorySearcher search = new DirectorySearcher(Entry))
            {
                var result = new List<ADUser>();
                search.Filter = filter;
                search.PropertiesToLoad.Add("cn");
                search.PropertiesToLoad.Add("sn");
                search.PropertiesToLoad.Add("displayname");
                search.PropertiesToLoad.Add("department");
                search.PropertiesToLoad.Add("mail");
                search.PropertiesToLoad.Add("givenname");
                search.PropertiesToLoad.Add("userprincipalname");
                search.PropertiesToLoad.Add("samaccountname");
                search.PropertiesToLoad.Add("company");
                search.PropertiesToLoad.Add("manager");
                var adUserList = search.FindAll();

                foreach (SearchResult adUser in adUserList)
                {
                    result.Add(new ADUser(adUser));
                }
                return result;
            }
        }

        public ADUser FindOne(string principal)
        {
            if (ContainSpecialcharacter(principal))
            {
                return null;
            }

            using (DirectorySearcher search = new DirectorySearcher(Entry))
            {
                search.Filter = string.Format("(SAMAccountName={0})", principal);
                search.PropertiesToLoad.Add("cn");
                search.PropertiesToLoad.Add("sn");
                search.PropertiesToLoad.Add("displayname");
                search.PropertiesToLoad.Add("department");
                search.PropertiesToLoad.Add("mail");
                search.PropertiesToLoad.Add("givenname");
                search.PropertiesToLoad.Add("userprincipalname");
                search.PropertiesToLoad.Add("samaccountname");
                search.PropertiesToLoad.Add("company");
                search.PropertiesToLoad.Add("manager");
                var adUser = search.FindOne();

                if (adUser == null) return null;

                return new ADUser(adUser);
            }
        }

        public bool IsADUser(string account)
        {
            bool authenticated = false;

            try
            {
                if (string.IsNullOrEmpty(account))
                    throw new Exception(Common_lang.PARAMETER_NULL);

                if (FindOne(account) == null)
                    throw new Exception(Common_lang.AD_USER_NULL);

                authenticated = true;
            }
            catch (Exception ex)
            {
                authenticated = false;
            }

            return authenticated;
        }

        public (bool,string) IsADLogin(string account, string password)
        {
            bool authenticated = false;
            var message = "";
            try
            {
                if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(password))
                    throw new Exception(Common_lang.PARAMETER_NULL);

                using (DirectoryEntry entry = new DirectoryEntry(ThirdPartyCache.Instance.LDAPUrl, account, password))
                {
                    object nativeObject = entry.NativeObject;
                    entry.Dispose();
                }

                authenticated = true;

            }
            catch (Exception ex)
            {
                authenticated = false;
                message = ex.Message;
            }

            return (authenticated,message);
        }

        //驗證是否含有特殊字元&前置空白/後置空白
        public bool ContainSpecialcharacter(string principal)
        {
            string[] characterlist = { "/", ",", @"\", "#", ">", "<", "=", "+", ";", "@", "“" };

            foreach (string item in characterlist)
            {
                if (principal.Contains(item) || principal.Length > principal.Trim().Length )
                {
                    return true;    
                }
            }
            return false;
        }    
    }
}
